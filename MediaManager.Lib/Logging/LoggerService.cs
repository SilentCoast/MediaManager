namespace MediaManager.Lib.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogOutput _logOutput;

        public LoggerService(ILogOutput logOutput)
        {
            _logOutput = logOutput;
        }

        public void Write(string message)
        {
            _logOutput.Write(message);
        }

        public void WriteLine(string message)
        {
            _logOutput.WriteLine(message);
        }
    }
}
