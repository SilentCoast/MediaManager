﻿using MediaManager.Lib.Logging;

namespace MediaManager.Lib.FileHandlingRules
{
    /// <summary>
    /// Delete files named Thumbs.db
    /// May be helpfull when sorting photos made on old cameras
    /// </summary>
    public class DeleteThumbsDbRule : IFileHandleRule
    {
        public ILoggerService LoggerService { get; }

        public DeleteThumbsDbRule(ILoggerService loggerService)
        {
            LoggerService = loggerService;
        }

        public bool ShouldHandle(FileInfo fileInfo)
        {
            return fileInfo.Name == "Thumbs.db";
        }

        public void Handle(FileInfo fileInfo)
        {
            File.Delete(fileInfo.FullName);
            LoggerService.WriteLine($"Deleted {fileInfo.FullName}");
        }
    }
}