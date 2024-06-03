using PhotoSorterLib.FileHandlingRules;
using PhotoSorterLib.Logging;

namespace PhotoSorterLib.Processors
{
    public class FileProcessor : IFileProcessor
    {
        public FileProcessor(ILoggerService loggerService)
        {
            LoggerService = loggerService;
        }

        public List<IFileHandlingRule> FileHandlingRules { get; set; } = new List<IFileHandlingRule>();
        public ILoggerService LoggerService { get; }

        /// <summary>
        /// Processing all files in all subfolders, Sorting them by years.
        /// </summary>
        /// <param name="directory">root directory of target</param>
        public void ProcessFiles(string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                FileInfo info = new FileInfo(file);

                // Apply file handling rules
                foreach (var rule in FileHandlingRules)
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
