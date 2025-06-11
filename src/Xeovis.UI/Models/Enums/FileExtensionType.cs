namespace Xeovis.UI.Models.Enums
{
    public enum FileExtensionType
    {
        All,
        Excel,
        JSON,
        Txt,
        XVS,
        PNG,
        JPEG,
    }

    public static class FileExtensionTypeExtensions
    {
        public static string GetStringExtention(this FileExtensionType fileExtensionType)
        {
            return fileExtensionType switch
            {
                FileExtensionType.Excel => ".xlsx",
                FileExtensionType.JSON => ".json",
                FileExtensionType.Txt => ".txt",
                FileExtensionType.XVS => ".xvs",
                FileExtensionType.PNG => ".png",
                FileExtensionType.JPEG => ".jpg",
                _ => string.Empty
            };
        }
    }
}
