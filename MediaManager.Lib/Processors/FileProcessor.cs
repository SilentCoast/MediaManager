using MediaManager.Lib.FileHandlingRules;
using MediaManager.Lib.Logging;

namespace MediaManager.Lib.Processors
{
    public class FileProcessor : IFileProcessor
    {
        public FileProcessor(ILoggerService loggerService)
        {
            LoggerService = loggerService;
        }

        public List<IFileHandleRule> FileHandleRules { get; set; } = new List<IFileHandleRule>();
        public ILoggerService LoggerService { get; }

        /// <summary>
        /// Processing all files in all subfolders, using specified rules
        /// </summary>
        /// <param name="directory">root directory of target</param>
        public void ProcessFiles(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                FileInfo info = new FileInfo(file);

                // Apply file handling rules
                foreach (var rule in FileHandleRules)
                {
                    if (rule.ShouldHandle(info))
                    {
                        rule.Handle(info);
                        continue;
                    }
                }
            }

            //go through subderictories
            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                LoggerService.WriteLine("Directory: " + subDirectory);
                ProcessFiles(subDirectory);
            }
        } 
    }
}
