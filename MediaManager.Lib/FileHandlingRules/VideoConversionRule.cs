using MediaManager.Lib.FileHandlingRules;
using MediaManager.Lib.Logging;
using Xabe.FFmpeg;

public class VideoConversionRule(ILoggerService logger, string inputDirectory, string outputDirectory) : IFileHandleRule
{
    private readonly ILoggerService _logger = logger;
    private readonly string inputDirectory = inputDirectory;
    private readonly string outputDirectory = outputDirectory;
    private static readonly string[] _videoExtensions =
    {
        ".avi", ".mpg", ".3gp",
        ".mp4"
    };

    public bool ShouldHandle(FileInfo fileInfo)
    {
        return _videoExtensions.Contains(fileInfo.Extension.ToLower());
    }

    public void Handle(FileInfo fileInfo)
    {
        string relativePath = Path.GetRelativePath(inputDirectory, fileInfo.FullName);

        string outputFilePath = Path.Combine(outputDirectory, relativePath);

        outputFilePath = Path.ChangeExtension(outputFilePath, ".mp4");

        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)); 


        var conversionTask = ConvertToH265(fileInfo.FullName, outputFilePath);
        conversionTask.GetAwaiter().GetResult();

        _logger.WriteLine($"Converted video {fileInfo.FullName} to MP4 (H.265): {outputFilePath}");
    }

    private async Task ConvertToH265(string inputFile, string outputFile)
    {
        var conversion = FFmpeg.Conversions.New()
           .AddParameter($"-i \"{inputFile}\"")
           .AddParameter("-c:v libx265") // Set the codec to H.265
           .AddParameter("-crf 20")
           .AddParameter($"\"{outputFile}\"");

        await conversion.Start();
    }
}
