using Avalonia;
using System;
using System.IO;
using Serilog;
using Serilog.Enrichers.CallerInfo;

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
            SetupLogging();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        private static void SetupLogging()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var logsFolder = Path.Combine(appDataFolder, "Xeovis", "Logs");
            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);

            var logFilePath = Path.Combine(logsFolder, $"Xeovis_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");

            var outputTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{ThreadName,10}] {Level:u3} - ({SourceFile}:{LineNumber}).{Method} -> {Message:lj}{NewLine}{Exception}";
            // var path = "log/xeovis.txt";
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithCallerInfo(
                    includeFileInfo: true,
                    assemblyPrefix: "Xeovis.",
                    filePathDepth: 1
                )
                .Enrich.WithProperty("ThreadName", "main") // default thread name if absent
                .MinimumLevel.Debug()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    logFilePath,
                    outputTemplate: outputTemplate,
                    fileSizeLimitBytes: 1 * 1024 * 1024, // 1 MB file size limit
                    rollOnFileSizeLimit: true             // rollover when file exceeds the limit
                )
                .CreateLogger();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
