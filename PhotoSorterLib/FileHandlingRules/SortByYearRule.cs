
using PhotoSorterLib.Logging;

namespace PhotoSorterLib.FileHandlingRules
{
    /// <summary>
    /// In destinationRoot will be created folders for each year 
    /// and all files with last modification date of this year will be put there. 
    /// if in this folder file with same name already exist, 
    /// the programm will check if the files are the same, if they the same the duplicat will be deleted, 
    /// if they are not the same, second file will be saved with generic name: 
    /// PhotoX or VideoX, where X is the number in order.
    /// <para></para>ShouldHandle set to always true
    /// </summary>
    public class SortByYearRule : IFileHandlingRule
    {
        public SortByYearRule(string destinationRoot, ILoggerService loggerService)
        {
            this.DestinationRoot = destinationRoot;
            LoggerService = loggerService;
        }

        public string DestinationRoot { get; }
        public ILoggerService LoggerService { get; }

        public void Handle(FileInfo fileInfo)
        {
            
            DateTime lastModified = fileInfo.LastWriteTime;
            int year = lastModified.Year;

            string destinationToMove = $"{DestinationRoot}\\{year}\\{fileInfo.Name}";
            //make sure directory exists
            Directory.CreateDirectory($"{DestinationRoot}\\{year}");

            MoveFile(fileInfo.FullName, destinationToMove);
            //Move file to destination, if already exist with same name, check if files are equal
            void MoveFile(string file, string destinationToMove)
            {
                //to give files with same names new generic names
                int PhotoName = 0;
                int VideoName = 0;
                try
                {
                    File.Move(file, destinationToMove);
                }
                catch (IOException)
                {
                    if (File.Exists(destinationToMove))
                    {
                        if (AreFilesEqual(file, destinationToMove))
                        {
                            //if files are equal - delete the copy
                            File.Delete(file);
                            LoggerService.WriteLine($"Deleted repeating file: {file}");
                            return;
                        }
                        else //if files are not equal but have same name - create new name for one of the files
                        {
                            string extension = Path.GetExtension(file).ToLower();
                            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                            {
                                destinationToMove = $"{DestinationRoot}\\{year}\\Photo{PhotoName++}{extension}";
                            }
                            else
                            {
                                destinationToMove = $"{DestinationRoot}\\{year}\\Video{VideoName++}{extension}";
                            }
                            MoveFile(file, destinationToMove);
                        }
                    }
                }
                LoggerService.WriteLine($"Moved from {file} to {destinationToMove}");
                bool AreFilesEqual(string filePath1, string filePath2)
                {
                    using (var fs1 = new FileStream(filePath1, FileMode.Open))
                    using (var fs2 = new FileStream(filePath2, FileMode.Open))
                    {
                        if (fs1.Length != fs2.Length)
                        {
                            return false; // If file sizes are different, they are not identical
                        }

                        byte[] buffer1 = new byte[1024];
                        byte[] buffer2 = new byte[1024];
                        int bytesRead1;
                        int bytesRead2;

                        while ((bytesRead1 = fs1.Read(buffer1, 0, buffer1.Length)) > 0 &&
                                (bytesRead2 = fs2.Read(buffer2, 0, buffer2.Length)) > 0)
                        {
                            if (!BuffersAreEqual(buffer1, buffer2, bytesRead1))
                            {
                                return false; // If content differs, files are not identical
                            }
                        }

                        return true; // If control reaches here, files are identical
                    }
                }

                bool BuffersAreEqual(byte[] buffer1, byte[] buffer2, int length)
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (buffer1[i] != buffer2[i])
                        {
                            return false; // If byte contents are different, buffers are not equal
                        }
                    }
                    return true; // If control reaches here, buffers are equal
                }
            }
        }

        public bool ShouldHandle(FileInfo fileInfo)
        {
            return true;
        }
    }
}
