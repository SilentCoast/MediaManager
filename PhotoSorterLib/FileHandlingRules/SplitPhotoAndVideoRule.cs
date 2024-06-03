using PhotoSorterLib.Logging;

namespace PhotoSorterLib.FileHandlingRules
{
    /// <summary>
    /// Creates 2 new folders (Photo,Video) in the destinationRoot. All video files will be moved to the Video folder respecting all path that file has till the targetDirectory. Same for Photo.
    /// <para>If file has unknown extension it'll be put in the NotRecognized folder</para>
    /// <para>ShouldHandle set to always true</para>
    /// </summary>
    public class SplitPhotoAndVideoRule : IFileHandlingRule
    {
        public SplitPhotoAndVideoRule(string targetDirectory, string destinationRoot, ILoggerService loggerService)
        {
            TargetDirectory = targetDirectory;
            DestinationRoot = destinationRoot;
            LoggerService = loggerService;
        }

        public string TargetDirectory { get; }
        public string DestinationRoot { get; }
        public ILoggerService LoggerService { get; }

        public void Handle(FileInfo fileInfo)
        {
            string[] extensions;
            string fileType;

            // Determine file type based on extension
            if (IsPhoto(fileInfo.Extension))
            {
                fileType = "Photo";
            }
            else if (IsVideo(fileInfo.Extension))
            {
                fileType = "Video";
            }
            else
            {
                fileType = "NotRecognized";
            }

            // Get relative path of the file within the target directory
            string relativePath = GetRelativePath(fileInfo.FullName, TargetDirectory);

            // Construct destination directory based on file type and relative path
            string destination = Path.Combine(DestinationRoot, fileType, relativePath);

            try
            {
                // Create destination directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(destination));

                // Move the file to the destination directory
                File.Move(fileInfo.FullName, destination, true);

                LoggerService.WriteLine($"File '{fileInfo.Name}' moved to '{destination}'.");
            }
            catch (Exception ex)
            {
                LoggerService.WriteLine($"Error moving file '{fileInfo.Name}': {ex.Message}");
            }
        }

        // Helper method to check if the file extension is in the photo extensions array
        private bool IsPhoto(string extension)
        {
            return Array.Exists(FileHelper.PhotoExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        // Helper method to check if the file extension is in the video extensions array
        private bool IsVideo(string extension)
        {
            return Array.Exists(FileHelper.VideoExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        // Helper method to get the relative path of a file within a directory
        private string GetRelativePath(string fullPath, string rootPath)
        {
            string relativePath = fullPath.Substring(rootPath.Length);
            return relativePath.TrimStart(Path.DirectorySeparatorChar);
        }

        public bool ShouldHandle(FileInfo fileInfo)
        {
            return true;
        }
    }
}
