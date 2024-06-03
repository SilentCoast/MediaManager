namespace PhotoSorterLib.Logging
{
    public interface ILogOutput
    {
        void Write(string message);
        void WriteLine(string message);
    }
}
