using MediaManager.Lib.FileHandlingRules;

namespace MediaManager.Lib.Processors
{
    public interface IFileProcessor
    {
        List<IFileHandleRule> FileHandleRules { get; set; }
        void ProcessFiles(string directory);
        void AddFileHandlingRule(IFileHandleRule rule)
        {
            FileHandleRules.Add(rule);
        }
    }
}