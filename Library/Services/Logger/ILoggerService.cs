namespace Library.Services.Logger
{
    public interface ILoggerService
    {
        void Audit(string message);
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(string message, string errorCode = "");
    }
}