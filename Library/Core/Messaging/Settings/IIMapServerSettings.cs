namespace Library.Core.Messaging.Settings
{
    public interface IIMapServerSettings : IRequestingEntity
    {
        string ImapServer { get; } 
        int Port { get; }
        bool IsSecureSocket { get; }
        string UserLogin { get; }
        string UserPassword { get; }
    }
}