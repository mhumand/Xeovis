using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Xeovis.UI.Views.Helpers;

public static class DirectoryHelper
{
    public static void GoToDirectory(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var psi = new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var psi = new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"\"{path}\"",
                UseShellExecute = false
            };
            Process.Start(psi);
        }
        
        // else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        // psi.FileName = "xdg-open";
    }

    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }
}