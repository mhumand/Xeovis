using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Xeovis.UI.Views.Helpers;

public static class DirectoryHelper
{
    public static void GoToDirectory(string path)
    {
        var psi = new ProcessStartInfo
        {
            Arguments = $"\"{path}\"",
            UseShellExecute = false
        };
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            psi.FileName = "explorer";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            psi.FileName = "open";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            psi.FileName = "xdg-open";

        Process.Start(psi);
    }

    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }
}