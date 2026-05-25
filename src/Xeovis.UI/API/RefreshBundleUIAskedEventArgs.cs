namespace Xeovis.UI;

public class RefreshBundleUIAskedEventArgs
{
    public string BundleName { get; set; }

    public RefreshBundleUIAskedEventArgs(string bundleName)
    {
        BundleName = bundleName;
    }
}