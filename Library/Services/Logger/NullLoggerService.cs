namespace Library.Services.Logger
{
    //[Export(typeof(ILoggerService))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    class NullLoggerService : ILoggerService
    {
        public void Audit(string message)
        {
            
        }

        public void Info(string message)
        {
            
        }

        public void Debug(string message)
        {
            
        }

        public void Warn(string message)
        {
            
        }

        public void Error(string message, string errorCode = "")
        {
            
        }
    }
}
