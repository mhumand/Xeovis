using Avalonia.Platform.Storage;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.Views.Helpers
{
    public static class FileExtentionHelper
    {
        public static FilePickerFileType GetFilePickerFileType(FileExtensionType extensionType)
        {
            switch (extensionType)
            {
                case FileExtensionType.All:
                    return FilePickerFileTypes.All;
                case FileExtensionType.XVS:
                    return new FilePickerFileType("Xeovis File")
                    {
                        Patterns = ["*.xvs"]
                    };
                case FileExtensionType.Excel:
                    return new FilePickerFileType("Excel File")
                    {
                        Patterns = ["*.xls", "*.xlsx"]
                    };
                case FileExtensionType.JSON:
                    break;
                case FileExtensionType.Txt:
                    return new FilePickerFileType("Text File")
                    {
                        Patterns = ["*.txt"]
                    };
                case FileExtensionType.PNG:
                    return FilePickerFileTypes.ImagePng;
                case FileExtensionType.JPEG:
                    return FilePickerFileTypes.ImageJpg;
            }

            return null;
        }
    }
}
