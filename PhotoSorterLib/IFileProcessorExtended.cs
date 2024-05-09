namespace PhotoSorterLib
{
    public interface IFileProcessorExtended : IFileProcessor
    {
        void DeleteEmptyFolders(string targetPath);
    }
}
