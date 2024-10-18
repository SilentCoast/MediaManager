using MediaManager.Lib.FileHandlingRules;
using MediaManager.Lib.Logging;
using Xabe.FFmpeg;

public class ImageConversionRule(ILoggerService logger, string inputDirectory, string outputDirectory) : IFileHandleRule
{
    private readonly ILoggerService _logger = logger;
    private readonly string inputDirectory = inputDirectory;
    private readonly string outputDirectory = outputDirectory;
    private static readonly string[] _imageExtensions = { ".jpg", ".jpeg", ".png" };
    
    public bool ShouldHandle(FileInfo fileInfo)
    {
        // Check if the file is an image with one of the supported extensions
        return _imageExtensions.Contains(fileInfo.Extension.ToLower());
    }

    public void Handle(FileInfo fileInfo)
    {
        string relativePath = Path.GetRelativePath(inputDirectory, fileInfo.FullName);

        string outputFilePath = Path.Combine(outputDirectory, relativePath);

        outputFilePath = Path.ChangeExtension(outputFilePath, ".webp");

        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

        var conversionTask = ConvertToWebP(fileInfo.FullName, outputFilePath);
        conversionTask.GetAwaiter().GetResult();

        _logger.WriteLine($"Converted image {fileInfo.FullName} to WebP: {outputFilePath}");
    }

    private async Task ConvertToWebP(string inputFile, string outputFile)
    {
        var conversion = await FFmpeg.Conversions.FromSnippet.Convert(inputFile, outputFile);
        await conversion.Start();
    }
}
