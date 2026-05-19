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
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var logPath = SetupLogging();
            
            // Start API server in the background
            ApiHost.Start();
            
            try
            {
                Log.Information("Application started.");
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
                Log.Information("Application closed.");
            }
            catch (Exception ex)
            {
                Log.Fatal("Unhandled exception. Terminating the app...\n{ex}\n\n{contact}", ex.Message,
                    "Please send this log file to Mohammad Humand:\nhumandm@student.unimelb.edu.au");
                
                DirectoryHelper.GoToDirectory(Directory.GetParent(logPath).FullName);
                DirectoryHelper.GoToDirectory(logPath);
            }
        }
        private static string SetupLogging()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var logsFolder = Path.Combine(appDataFolder, "Xeovis", "Logs");
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
