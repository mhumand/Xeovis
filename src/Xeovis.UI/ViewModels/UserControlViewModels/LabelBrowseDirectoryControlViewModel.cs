using CommunityToolkit.Mvvm.ComponentModel;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelBrowseDirectoryControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private string? directory;

        [ObservableProperty]
        private bool isFolderBrowser;

        [ObservableProperty]
        private FileExtensionType extensionType;

        private string _originalDirectory;
        public string OriginalDirectory => _originalDirectory;
        
        public LabelBrowseDirectoryControlViewModel()
        {
            directory = string.Empty;
            _originalDirectory = string.Empty;
        }

        public LabelBrowseDirectoryControlViewModel(string label, FileExtensionType extensionType, string defaultDirectory = null, bool isFolderBrowser = false)
        {
            Label = label;
            ExtensionType = extensionType;
            IsFolderBrowser = isFolderBrowser;
            Directory = defaultDirectory;
            _originalDirectory = defaultDirectory;
        }

        public LabelBrowseDirectoryControlViewModel(string label, string defaultDirectory = null, bool isFolderBrowser = false)
        {
            Label = label;
            IsFolderBrowser = isFolderBrowser;
            Directory = defaultDirectory;
            _originalDirectory = defaultDirectory;
        }
    }
}
