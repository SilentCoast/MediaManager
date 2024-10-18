using PhotoSorterLib.FileHandlingRules;
using PhotoSorterLib.Logging;
using Xabe.FFmpeg;

public class VideoConversionRule : IFileHandlingRule
{
    private ILoggerService _logger;
    private static readonly string[] _videoExtensions = 
    { 
        ".avi", ".mpg", ".3gp",
        ".mp4"
    };
    public VideoConversionRule(ILoggerService logger)
    {
        _logger = logger;
    }

    public bool ShouldHandle(FileInfo fileInfo)
    {
        return _videoExtensions.Contains(fileInfo.Extension.ToLower());
    }

    public void Handle(FileInfo fileInfo)
    {
        string outputFilePath = Path.ChangeExtension(fileInfo.FullName, ".mp4");

        var conversionTask = ConvertToH265(fileInfo.FullName, outputFilePath);
        conversionTask.GetAwaiter().GetResult();

        _logger.WriteLine($"Converted video {fileInfo.FullName} to MP4 (H.265): {outputFilePath}");
    }

    private async Task ConvertToH265(string inputFile, string outputFile)
    {
        var conversion = FFmpeg.Conversions.New()
           .AddParameter($"-i \"{inputFile}\"")
           .AddParameter("-c:v libx265") // Set the codec to H.265
           .AddParameter("-crf 21") //quality 18 to 23 where 18 is the best and 23 is close to indistingushable
           .AddParameter($"\"{outputFile}\"");

        await conversion.Start();
    }
}
