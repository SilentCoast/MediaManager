using PhotoSorterLib;

namespace PhotoSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileProcessorExtended fileProcessor = new FileProcessor();

            IFileHandlingRule deleteThumbsDbRule = new DeleteThumbsDbRule();
            fileProcessor.AddFileHandlingRule(deleteThumbsDbRule);

            string sourceDirectory = "C:\\Photos";
            string destinationRoot = "C:\\Photos\\Sorted";

            fileProcessor.ProcessFiles(sourceDirectory, destinationRoot);
            fileProcessor.DeleteEmptyFolders(destinationRoot);

            IStatisticsCalculator statisticsCalculator = new StatisticsCalculator();
            statisticsCalculator.CalculateAndDisplayStatistics(destinationRoot);

            Console.WriteLine("\nApp fineshed working");
            Console.ReadKey();
        }
    }
}
