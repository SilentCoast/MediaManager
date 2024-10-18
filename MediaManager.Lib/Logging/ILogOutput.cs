namespace MediaManager.Lib.Logging
{
    public interface ILogOutput
    {
        void Write(string message);
        void WriteLine(string message);
    }
}
