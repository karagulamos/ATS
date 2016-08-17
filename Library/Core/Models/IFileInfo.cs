namespace Library.Core.Models
{
    public interface IFileInfo
    {
        string FilePath { get; }
        string FileType { get; }
        string OriginalFileName { get; }
    }
}