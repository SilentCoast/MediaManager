namespace PhotoSorter
{
    public static class FileProcesser
    {
        public static void TraverseDirectory(string directory,string destinationRoot)
        {
            int PhotoName = 1;
            int VideoName = 1;
            // Process files in the current directory
            foreach (string file in Directory.GetFiles(directory))
            {
                FileInfo info = new FileInfo(file);
                if(info.Name == "Thumbs.db")
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted {file}");
                    continue;
                }


                DateTime lastModified = info.LastWriteTime;
                int year = lastModified.Year;

                string destinationToMove = $"{destinationRoot}\\{year}\\{info.Name}";
                Directory.CreateDirectory($"{destinationRoot}\\{year}");
                MoveFile(file, destinationToMove );
                void MoveFile(string file,string destinationToMove)
                {
                    try
                    {
                        File.Move(file, destinationToMove);
                    }
                    catch (IOException)
                    {
                        if (File.Exists(destinationToMove))
                        {
                            if (AreFilesEqual(file, destinationToMove))
                            {//true
                                File.Delete(file);
                                Console.WriteLine($"Deleted repeating file: {file}");
                                return;
                            }
                            else // false
                            {
                                string extension = Path.GetExtension(file);
                                if (extension.ToLower() == ".jpg")
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
                }
                Console.WriteLine($"Moved from {file} to {destinationToMove}");
            }

            // Recursively process subdirectories
            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                Console.WriteLine("Directory: " + subDirectory);
                TraverseDirectory(subDirectory,destinationRoot);
            }
        }
        static bool AreFilesEqual(string filePath1, string filePath2)
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

        static bool BuffersAreEqual(byte[] buffer1, byte[] buffer2, int length)
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
