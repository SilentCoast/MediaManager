namespace PhotoSorterLib
{
    public interface IFileProcessor
    {
        void ProcessFiles(string directory, string destinationRoot);
        void AddFileHandlingRule(IFileHandlingRule rule);
    }
}