using System;

namespace Xeovis.UI;

public class BundleReloadCalledEventArgs : EventArgs
{
    public string BundleName { get; set; }
    public bool IsReloadAndAppend { get; set; }

    public BundleReloadCalledEventArgs(string bundleName, bool isReloadAndAppend)
    {
        BundleName = bundleName;
        IsReloadAndAppend = isReloadAndAppend;
    }
}