using Avalonia;
using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI
{
    internal sealed class Program
    {
        private static string _logFolder;
        public static string LogFolder => _logFolder;
            
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var logPath = SetupLogging();
            
            // 1. GLOBAL HANDLER: Catches rogue background thread exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                LogFatalCrash(ex, logPath, "AppDomain (Background Thread)");
            };
            
            // 2. TASK HANDLER: Catches exceptions in unawaited async Tasks
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Log.Error(e.Exception, "An unobserved background task threw an exception.");
                
                // CRITICAL: This prevents background tasks from crashing the whole application!
                e.SetObserved(); 
            };
            
            // Safely fire-and-forget the API Host so it doesn't crash the main thread if it fails
            _ = Task.Run(() => ApiHost.Start()).ContinueWith(t => 
            {
                if (t.Exception != null) 
                    Log.Error(t.Exception, "ApiHost failed to start or crashed.");
            });
            
            try
            {
                Log.Information("Application started.");
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
                Log.Information("Application closed.");
            }
            catch (Exception ex)
            {
                // Catches exceptions specifically on the Main UI Thread
                LogFatalCrash(ex, logPath, "Main UI Thread");
            }
            finally
            {
                // Ensure all logs are written to disk before the app exits normally
                Log.CloseAndFlush();
            }
            
            // // Start API server in the background
            // ApiHost.Start();
            //
            // try
            // {
            //     Log.Information("Application started.");
            //     BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            //     Log.Information("Application closed.");
            // }
            // catch (Exception ex)
            // {
            //     Log.Fatal("Unhandled exception. Terminating the app...\n{ex}\n\n{contact}", ex.Message,
            //         "Please send this log file to Mohammad Humand:\nhumandm@student.unimelb.edu.au");
            //     
            //     DirectoryHelper.GoToDirectory(Directory.GetParent(logPath).FullName);
            //     DirectoryHelper.GoToDirectory(logPath);
            // }
        }
        
        private static void LogFatalCrash(Exception ex, string logPath, string source)
        {
            Log.Fatal("Unhandled exception in {Source}. Terminating the app...\n{Exception}\n\n{Contact}", 
                source, 
                ex?.Message,
                "Please send this log file to Mohammad Humand:\nhumandm@student.unimelb.edu.au");
            
            // CRITICAL: Force Serilog to write to the file IMMEDIATELY. 
            // If the app crashes hard, buffered logs are lost without this line.
            Log.CloseAndFlush();
            
            try
            {
                var parentDir = Directory.GetParent(logPath)?.FullName;
                if (parentDir != null) DirectoryHelper.GoToDirectory(parentDir);
                DirectoryHelper.GoToDirectory(logPath);
            }
            catch 
            {
                // If the app is crashing, don't let the directory opener cause a second crash
            }
        }
        
        private static string SetupLogging()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var logsFolder = Path.Combine(appDataFolder, "Xeovis", "Logs");
            _logFolder = logsFolder;
            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);

            var logFilePath = Path.Combine(logsFolder, $"Xeovis_{DateTime.Now:yyyyMMdd_HHmmss}.log");

            var outputTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss} [{ThreadName}] {Level:u3} - ({SourceFile}:{LineNumber}).{Method} -> {Message:lj}{NewLine}{Exception}";
            // var path = "log/xeovis.txt";
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithCallerInfo(
                    includeFileInfo: true,
                    assemblyPrefix: "Xeovis.",
                    filePathDepth: 1
                )
                .Enrich.WithProperty("ThreadName", "main") // default thread name if absent
                .MinimumLevel.Debug()
                .WriteTo.File(
                    logFilePath,
                    outputTemplate: outputTemplate,
                    fileSizeLimitBytes: 1 * 1024 * 1024, // 1 MB file size limit
                    rollOnFileSizeLimit: true             // rollover when file exceeds the limit
                )
                .CreateLogger();

            return logFilePath;
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
