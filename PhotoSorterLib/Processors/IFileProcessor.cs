using PhotoSorterLib.FileHandlingRules;

namespace PhotoSorterLib.Processors
{
    public interface IFileProcessor
    {
        List<IFileHandlingRule> FileHandlingRules { get; set; }
        void ProcessFiles(string directory);
        void AddFileHandlingRule(IFileHandlingRule rule)
        {
            FileHandlingRules.Add(rule);
        }
    }
}