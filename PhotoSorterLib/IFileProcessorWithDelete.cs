namespace PhotoSorterLib
{
    public interface IFileProcessorWithDelete : IFileProcessor
    {
        void DeleteEmptyFolders(string targetPath);
    }
}
