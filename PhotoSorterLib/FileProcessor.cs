namespace PhotoSorterLib
{
    public class FileProcessor : IFileProcessorExtended
    {
        private readonly List<IFileHandlingRule> _fileHandlingRules = new List<IFileHandlingRule>();

        public void AddFileHandlingRule(IFileHandlingRule rule)
        {
            _fileHandlingRules.Add(rule);
        }
        /// <summary>
        /// Processing all files in all subfolders, Sorting them by years.
        /// 
        /// In destinationRoot will be created folders for each year 
        /// and all files with last modification date of this year will be put there. 
        /// if in this folder file with same name already exist, 
        /// the programm will check if the files are the same, if they the same the duplicat will be deleted, 
        /// if they are not the same, second file will be saved with generic name: 
        /// PhotoX or VideoX, where X is the number in order
        /// </summary>
        /// <param name="directory">root directory of target</param>
        /// <param name="destinationRoot">root directory of destination</param>
        public void ProcessFiles(string directory, string destinationRoot)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                FileInfo info = new FileInfo(file);

                // Apply file handling rules
                foreach (var rule in _fileHandlingRules)
                {
                    if (rule.ShouldHandle(info))
                    {
                        rule.Handle(info);
                        continue;
                    }
                }

                DateTime lastModified = info.LastWriteTime;
                int year = lastModified.Year;

                string destinationToMove = $"{destinationRoot}\\{year}\\{info.Name}";
                //make sure directory exists
                Directory.CreateDirectory($"{destinationRoot}\\{year}");

                MoveFile(file, destinationToMove);
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
                                Console.WriteLine($"Deleted repeating file: {file}");
                                return;
                            }
                            else //if files are not equal but have same name - create new name for one of the files
                            {
                                string extension = Path.GetExtension(file).ToLower();
                                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                                {
                                    destinationToMove = $"{destinationRoot}\\{year}\\Photo{PhotoName++}{extension}";
                                }
                                else
                                {
                                    destinationToMove = $"{destinationRoot}\\{year}\\Video{VideoName++}{extension}";
                                }
                                MoveFile(file, destinationToMove);
                            }
                        }
                    }
                    Console.WriteLine($"Moved from {file} to {destinationToMove}");
                }
            }

            //go through subderictories
            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                Console.WriteLine("Directory: " + subDirectory);
                ProcessFiles(subDirectory, destinationRoot);
            }
        }
            
        private static bool AreFilesEqual(string filePath1, string filePath2)
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

        private static bool BuffersAreEqual(byte[] buffer1, byte[] buffer2, int length)
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

        public void DeleteEmptyFolders(string targetPath)
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
