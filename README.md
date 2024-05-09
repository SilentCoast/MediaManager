# PhotoSorter

PhotoSorter is a C# application designed to help you organize your photo and video files by sorting them into folders based on their last modification year. It also provides statistics on the total size of photos and videos in the specified directory.

## Features

- **File Sorting**: Automatically sorts photo and video files into folders based on their last modification year.
- **Duplicate Handling**: Checks for duplicate files and ensures that only one copy is retained.
- **Custom File Handling Rules**: Allows users to define custom rules for handling specific file types, such as deleting "Thumbs.db" files.
- **Statistics Calculation**: Calculates and displays the total size of photos and videos in the specified directory.
- **Empty Folder Removal**: Removes empty folders from the destination directory after file sorting.

## Usage

1. **Download**: Clone or download the PhotoSorter repository to your local machine.

2. **Build**: Build the solution using Visual Studio or your preferred IDE.

3. **Customize**: Modify the `Main` method in the `Program.cs` file to specify the source and destination directories, as well as any custom file handling rules.

4. **Run**: Run the application. It will process files in the source directory, sort them into folders in the destination directory, calculate statistics, and remove empty folders.

## Customization

- **Adding Custom File Handling Rules**: Implement the `IFileHandlingRule` interface to define custom file handling logic for specific file types.
- **Modifying Statistics Calculation**: Customize the `StatisticsCalculator` class to include additional file extensions or change the calculation logic.

## Example

```csharp
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
