using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public partial class DataManagerLoadDataViewModel : ViewModelBase
    {
        public string FolderDirectory { get; set; }
        public string SubfolderDirectory { get; set; }
        public bool IsSubfolderChecked { get; set; }

        private List<ListableItem> selectedFolders = new List<ListableItem>();
        public List<ListableItem> SelectedFolders
        {
            get { return selectedFolders; }
            set { selectedFolders = value; }
        }

        public bool HasAnySelectedFolders => SelectedFolders != null && selectedFolders.Any();

        private List<ListableItem> eligibleFolders = new List<ListableItem>();
        public List<ListableItem> EligibleFolders => eligibleFolders;


        public DataManagerLoadDataViewModel()
        {
            FolderDirectory = string.Empty;
            SubfolderDirectory = string.Empty;
        }

        public List<ListableItem> GetEligibleFoldersList(string directory, bool searchSubfolders)
        {
            if (string.IsNullOrWhiteSpace(directory)) return null;

            eligibleFolders = new List<ListableItem>();
            if (Directory.Exists(directory))
            {
                var allFolder = Directory.GetDirectories(directory).ToList();
                foreach (var folder in allFolder)
                {
                    var info = new DirectoryInfo(folder);
                    if (!info.Name.Any(char.IsDigit)) continue;
                    var files = info.GetFiles("*.xls", searchSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
                    if (files.Count == 0) continue;
                    eligibleFolders.Add(new ListableItem(info.Name, tag: info));
                }
            }

            return eligibleFolders;
        }

        public List<ListableItem> GetFolderContents(ListableItem folder)
        {
            if (folder == null) return null;

            var info = folder.Tag as DirectoryInfo;
            var files = info.GetFiles();
            if (files == null) return null;
            var contents = new List<ListableItem>();
            foreach ( var file in files)
                contents.Add(new ListableItem(file.Name, tag: file));

            return contents;
        }
    }
}
