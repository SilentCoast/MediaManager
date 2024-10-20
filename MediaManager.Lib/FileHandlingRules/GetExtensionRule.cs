﻿using MediaManager.Lib.Logging;

namespace MediaManager.Lib.FileHandlingRules
{
    public class GetExtensionRule : IFileHandleRule
    {
        public ILoggerService LoggerService { get; }

        public GetExtensionRule(ILoggerService loggerService)
        {
            LoggerService = loggerService;
        }

        private HashSet<string> _extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool ShouldHandle(FileInfo fileInfo)
        {
            return true;
        }

        public void Handle(FileInfo fileInfo)
        {
            _extensions.Add(fileInfo.Extension);

            LoggerService.WriteLine($"File: {fileInfo.FullName}, Extension: {fileInfo.Extension}");
        }

        public IEnumerable<string> GetExtensions()
        {
            return _extensions;
        }
    }
}