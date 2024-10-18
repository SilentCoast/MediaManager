using PhotoSorterLib.FileHandlingRules;
using PhotoSorterLib.Logging;
using Xabe.FFmpeg;

public class ImageConversionRule : IFileHandlingRule
{
    private static readonly string[] _imageExtensions = { ".jpg", ".jpeg", ".png" };
    private ILoggerService _logger;
    public ImageConversionRule(ILoggerService logger)
    {
        FFmpeg.SetExecutablesPath("C:\\tools\\msys64\\mingw64\\bin");
        _logger = logger;
    }

    public bool ShouldHandle(FileInfo fileInfo)
    {
        // Check if the file is an image with one of the supported extensions
        return _imageExtensions.Contains(fileInfo.Extension.ToLower());
    }

    public void Handle(FileInfo fileInfo)
    {
        string outputFilePath = Path.ChangeExtension(fileInfo.FullName, ".webp");

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
