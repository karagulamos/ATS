using System.ComponentModel.Composition;
using System.Diagnostics;
using log4net;

namespace Library.Services.Logger
{
    [Export(typeof(ILoggerService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FileLoggerService : ILoggerService
    {
        private readonly ILog _fileLogger;

        public FileLoggerService()
        {
            _fileLogger = LogManager.GetLogger(typeof(FileLoggerService));
        }

        public void Audit(string message)
        {
            AddToLogs(message, LoggerTypes.Audit);
        }

        public void Info(string message)
        {
            AddToLogs(message, LoggerTypes.Info);
        }

        public void Debug(string message)
        {
            AddToLogs(message, LoggerTypes.Debug);
        }

        public void Warn(string message)
        {
            AddToLogs(message, LoggerTypes.Warn);
        }

        public void Error(string message, string errorCode = "")
        {
            AddToLogs(message, LoggerTypes.Error, errorCode);
        }

        private void AddToLogs(string message, LoggerTypes logType, string errorCode = "")
        {
            var logMessage = " " + GetExternalSource() + " - " + message;
            LogToFile(logMessage, logType, errorCode);
        }

        private static string GetExternalSource(int defaultSkip = 3)
        {
            var frame = new StackFrame(defaultSkip);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            return string.Format("{0} / {1}", type, name);
        }

        private void LogToFile(string message, LoggerTypes logType, string errorCode)
        {
            switch (logType)
            {
                case LoggerTypes.Audit:
                case LoggerTypes.Info:
                    _fileLogger.Info(message);
                    break;

                case LoggerTypes.Debug:
                    _fileLogger.Debug(message);
                    break;

                case LoggerTypes.Warn:
                    _fileLogger.Warn(message);
                    break;

                case LoggerTypes.Error:
                    _fileLogger.Error((!string.IsNullOrEmpty(errorCode) ? errorCode + " - " : string.Empty) + message);
                    break;
            }
        }
    }
}
