using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public partial class mctcLoadDataViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string mctcBundleName;

        [ObservableProperty]
        private string folderDirectory;

        public List<string> ExisitingBundleNames { get; set; }

        public List<ListableItem> RequiredFiles;
        public bool HasRequiredFiles => RequiredFiles != null && RequiredFiles.Count > 0;

        public mctcLoadDataViewModel()
        {
            FolderDirectory = string.Empty;
            mctcBundleName = "Bundle 1";
            RequiredFiles = new List<ListableItem>();
            ExisitingBundleNames = new List<string>();
        }

        public List<ListableItem> GetRequiredFilesItems()
        {
            var txtList = new List<ListableItem>()
            {
            new ListableItem("batchlines.txt"),
            new ListableItem("batchbulks.txt"),
            new ListableItem("observations.txt"),
            new ListableItem("-dr.txt")
            };

            RequiredFiles = txtList;
            return txtList;
        }

        public Dictionary<ListableItem, bool> CheckRequiredFilesExistense()
        {
            var validations = new Dictionary<ListableItem, bool>();
            foreach (var item in RequiredFiles)
            {
                item.Description = "Not Found";
                item.DescriptionColor = Color.Red;
                validations.Add(item, false);
            }

            var files = Directory.GetFiles(folderDirectory).ToList();
            if (files != null && files.Count != 0)
            {
                foreach (var key in validations.Keys)
                {
                    var file = files.FirstOrDefault(f => f.Contains(key.Label));
                    if (file != null)
                    {
                        key.Description = "Found";
                        key.DescriptionColor = Color.Green;
                        validations[key] = true;
                    }
                }
            }

            return validations;
        }
    }
}
