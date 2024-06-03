using PhotoSorterLib;
using PhotoSorterLib.FileHandlingRules;
using PhotoSorterLib.Logging;
using PhotoSorterLib.Processors;

namespace PhotoSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //set up the required Logger
            ILoggerService loggerService = new LoggerService(new ConsoleLogOutput());

            IFileProcessor fileProcessor = new FileProcessor(loggerService);

            //define target and destination derectories
            string sourceDirectory = "C:\\Media";
            string destinationRoot = "C:\\Media\\Sorted";

            //set up rules for proccesing
            fileProcessor.FileHandlingRules.AddRange(
                new List<IFileHandlingRule>()
                {
                    //deletes unnecessary files
                    new DeleteThumbsDbRule(loggerService),
                    //Sorts files by years
                    new SortByYearRule(destinationRoot, loggerService),
                    //splits Photos and Videos
                    new SplitPhotoAndVideoRule(sourceDirectory, destinationRoot, loggerService)
                    //define your own rules for processing...
                });

            fileProcessor.ProcessFiles(sourceDirectory);
            FileHelper.DeleteEmptyFolders(destinationRoot);

            //get some statistics
            StatisticsCalculator statisticsCalculator = new StatisticsCalculator();
            (var totalPhotoSize, var totalVideoSize) = statisticsCalculator.CalculatePhotoAndVideoSizeGb(destinationRoot);

            Console.WriteLine($"Total Photo Size: {totalPhotoSize} GB");
            Console.WriteLine($"Total Video Size: {totalVideoSize} GB");

            Console.WriteLine("\nApp finished working");
            Console.ReadKey();
        }
    }
}
