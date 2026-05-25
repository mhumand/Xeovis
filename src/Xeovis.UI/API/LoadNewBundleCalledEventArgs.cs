using System;

namespace Xeovis.UI;

public class LoadNewBundleCalledEventArgs : EventArgs
{
    public string BundleName { get; set; }
    public string Directory { get; set; }

    public LoadNewBundleCalledEventArgs(string bundleName, string directory)
    {
        BundleName = bundleName;
        Directory = directory;
    }
}