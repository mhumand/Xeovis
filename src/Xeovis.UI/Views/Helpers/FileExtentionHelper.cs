using Avalonia.Platform.Storage;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.Views.Helpers
{
    public static class FileExtentionHelper
    {
        public static string[]? GetPatterns(FileExtensionType extensionType)
        {
            switch (extensionType)
            {
                case FileExtensionType.Excel:
                    return ["*.xls", "*.xlsx"];
                case FileExtensionType.JSON:
                    break;
                case FileExtensionType.Txt:
                    break;
            }

            return null;
        }

        public static FilePickerFileType GetFilePickerFileType(FileExtensionType extensionType)
        {
            switch (extensionType)
            {
                case FileExtensionType.All:
                    return FilePickerFileTypes.All;
                case FileExtensionType.Excel:
                    return new FilePickerFileType("Excel")
                    {
                        Patterns = GetPatterns(extensionType)
                    };
                case FileExtensionType.JSON:
                    break;
                case FileExtensionType.Txt:
                    break;
            }

            return null;
        }
    }
}
