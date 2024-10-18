namespace MediaManager.Lib.Logging
{
    public interface ILoggerService
    {
        void Write(string message);
        void WriteLine(string message);
    }
}
