namespace PhotoSorterLib
{
    /// <summary>
    /// Delete files named Thumbs.db
    /// May be helpfull when sorting photos made on old cameras
    /// </summary>
    public class DeleteThumbsDbRule : IFileHandlingRule
    {
        public bool ShouldHandle(FileInfo fileInfo)
        {
            return fileInfo.Name == "Thumbs.db";
        }

        public void Handle(FileInfo fileInfo)
        {
            File.Delete(fileInfo.FullName);
            Console.WriteLine($"Deleted {fileInfo.FullName}");
        }
    }

}