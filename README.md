# PhotoSorter

PhotoSorter is a C# application designed to help you organize your photo and video files by sorting them into folders based on parameters provided. It also provides some statistic features, such as calculating the total size of photos and videos in the specified directory for example.

## Features

- **File Sorting**: Automatically sorts photo and video files into folders based on their last modification year.
- **Photo/Video splitting**: Automatically splits photo and video files into separate folders saving the relative path, so the structure of your media stays the same.
- **Duplicate Handling**: Checks for duplicate files and ensures that only one copy is retained.
- **Custom File Handling Rules**: Allows users to define custom rules for handling specific file types, for example deleting "Thumbs.db" files that may be present if media was imported from some old cameras.
- **Statistics Calculation**: Calculates and displays the total size of photos and videos in the specified directory.
- **Empty Folder Removal**: Removes empty folders from the destination directory after file sorting.

## Usage

1. **Download**: Clone or download the PhotoSorter repository to your local machine.

2. **Build**: Build the solution using Visual Studio or your preferred IDE.

3. **Customize**: Modify the `Main` method in the `Program.cs` file to specify the source and destination directories, as well as any custom file handling rules.

4. **Run**: Run the application. It will process files in a way you specifyed.

## Customization

- **Adding Custom File Handling Rules**: Implement the `IFileHandlingRule` interface to define custom file handling logic.
- **Modifying Statistics Calculation**: Customize the `StatisticsCalculator` class to include additional file extensions or change the calculation logic.

## Example

```csharp
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
