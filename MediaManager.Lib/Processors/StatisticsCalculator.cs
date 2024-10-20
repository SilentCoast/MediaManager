﻿using MediaManager.Lib.Converters;

namespace MediaManager.Lib.Processors
{
    //TODO: redo as rules
    public class StatisticsCalculator
    {
        /// <summary>
        /// Calculates the total photo size and total video size in directory and all subdirectories
        /// </summary>
        /// <param name="destinationRoot"></param>
        public (double totalPhotoSize, double totalVideoSize) CalculatePhotoAndVideoSizeGb(string destinationRoot)
        {
            long totalPhotoSize = CalculateTotalSize(destinationRoot, FileHelper.PhotoExtensions);
            long totalVideoSize = CalculateTotalSize(destinationRoot, FileHelper.VideoExtensions);

            return(Math.Round(BytesConverter.BytesToGigabytes(totalPhotoSize), 2), Math.Round(BytesConverter.BytesToGigabytes(totalVideoSize), 2));
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
