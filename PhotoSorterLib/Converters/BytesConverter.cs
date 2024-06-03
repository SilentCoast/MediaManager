namespace PhotoSorterLib.Converters
{
    public static class BytesConverter
    {
        public static double BytesToMegabytes(long bytes)
        {
            return (double)bytes / (1024 * 1024);
        }
        public static double BytesToGigabytes(long bytes)
        {
            return (double)bytes / (1024 * 1024 * 1024);
        }
    }
}
