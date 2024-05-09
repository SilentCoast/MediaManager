namespace PhotoSorterLib
{
    public class StatisticsCalculator : IStatisticsCalculator
    {
        private static readonly string[] PhotoExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private static readonly string[] VideoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };

        /// <summary>
        /// Calculates the total photo size and total video size in directory and all subdirectories
        /// </summary>
        /// <param name="destinationRoot"></param>
        public void CalculateAndDisplayStatistics(string destinationRoot)
        {
            long totalPhotoSize = CalculateTotalSize(destinationRoot, PhotoExtensions);
            long totalVideoSize = CalculateTotalSize(destinationRoot, VideoExtensions);

            Console.WriteLine($"Total Photo Size: {Math.Round(BytesConverter.BytesToGigabytes(totalPhotoSize),2)} GB");
            Console.WriteLine($"Total Video Size: {Math.Round(BytesConverter.BytesToGigabytes(totalVideoSize),2)} GB");
        }

        private long CalculateTotalSize(string directory, string[] extensions)
        {
            long totalSize = 0;

            foreach (string extension in extensions)
            {
                totalSize += Directory.GetFiles(directory, $"*{extension}", SearchOption.AllDirectories)
                                      .Select(file => new FileInfo(file).Length)
                                      .Sum();
            }

            return totalSize;
        }
    }
}
