namespace PhotoSorterLib.Logging
{
    public interface ILoggerService
    {
        void Write(string message);
        void WriteLine(string message);
    }
}
