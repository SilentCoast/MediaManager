using PhotoSorterLib;
using PhotoSorterLib.Converters;
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
            string sourceDirectory = "C:\\Photos";
            string destinationRoot = "C:\\Photos\\Sorted";

            //set up rules for proccesing
            fileProcessor.FileHandlingRules.AddRange(
                new List<IFileHandlingRule>()
                {
                    new DeleteThumbsDbRule(loggerService),
                    new SortByYearRule(destinationRoot, loggerService)
                });

            fileProcessor.ProcessFiles(sourceDirectory);
            FileHelper.DeleteEmptyFolders(destinationRoot);

            //get some statistics
            StatisticsCalculator statisticsCalculator = new StatisticsCalculator();
            (var totalPhotoSize, var totalVideoSize) = statisticsCalculator.CalculatePhotoAndVideoSizeGb(destinationRoot);

            Console.WriteLine($"Total Photo Size: {totalPhotoSize} GB");
            Console.WriteLine($"Total Video Size: {totalVideoSize} GB");

            Console.WriteLine("\nApp fineshed working");
            Console.ReadKey();
        }
    }
}
