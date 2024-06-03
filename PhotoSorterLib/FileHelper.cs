namespace PhotoSorterLib
{
    /// <summary>
    /// Temporary solution untill proper Interface for that functionality is not created
    /// </summary>
    public static class FileHelper
    {
        public static void DeleteEmptyFolders(string targetPath)
        {
            RemoveEmptyFolders(targetPath);
            void RemoveEmptyFolders(string directory)
            {
                foreach (string subDirectory in Directory.GetDirectories(directory))
                {
                    RemoveEmptyFolders(subDirectory);
                    if (Directory.GetFiles(subDirectory).Length == 0 &&
                        Directory.GetDirectories(subDirectory).Length == 0)
                    {
                        Directory.Delete(subDirectory);
                        Console.WriteLine($"Removed empty folder: {subDirectory}");
                    }
                }
            }
        }
    }
}
