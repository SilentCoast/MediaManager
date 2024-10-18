using MediaManager.Lib.FileHandlingRules;
using MediaManager.Lib.Logging;
using MediaManager.Lib.Processors;
using MediaManager.Lib;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xabe.FFmpeg;

namespace MediaManager.WIndowsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FFmpeg.SetExecutablesPath("C:\\tools\\msys64\\mingw64\\bin");

            //set up the required Logger
            ILoggerService loggerService = new LoggerService(new ConsoleLogOutput());

            IFileProcessor fileProcessor = new FileProcessor(loggerService);

            //define target and destination derectories
            string inputDirectory = "C:\\Users\\Andre\\OneDrive\\Рабочий стол\\test";
            string outputDirectory = "C:\\Users\\Andre\\OneDrive\\Рабочий стол\\MediaSorted";

            GetExtensionRule getExtensionRule = new GetExtensionRule(loggerService);

            //set up rules for proccesing
            fileProcessor.FileHandleRules.AddRange(
                new List<IFileHandleRule>()
                {
                    //deletes unnecessary files
                    //new DeleteThumbsDbRule(loggerService),
                    //Sorts files by years
                    //new SortByYearRule(destinationRoot, loggerService),
                    //splits Photos and Videos
                    //new SplitPhotoAndVideoRule(sourceDirectory, destinationRoot, loggerService)
                    //define your own rules for processing...
                    //new VideoConversionRule(loggerService),
                    //getExtensionRule,

                    new ImageConversionRule(loggerService, inputDirectory, outputDirectory),
                    new VideoConversionRule(loggerService, inputDirectory, outputDirectory),
                });

            fileProcessor.ProcessFiles(inputDirectory);

            var uniqueExtensions = getExtensionRule.GetExtensions();
            foreach (var ext in uniqueExtensions)
            {
                loggerService.WriteLine($"Unique Extension: {ext}");
            }

            FileHelper.DeleteEmptyFolders(outputDirectory);

            //get some statistics
            //StatisticsCalculator statisticsCalculator = new StatisticsCalculator();
            //(var totalPhotoSize, var totalVideoSize) = statisticsCalculator.CalculatePhotoAndVideoSizeGb(destinationRoot);

            //Console.WriteLine($"Total Photo Size: {totalPhotoSize} GB");
            //Console.WriteLine($"Total Video Size: {totalVideoSize} GB");

            Console.WriteLine("\nApp finished working");
            Console.ReadKey();
        }
    }
}