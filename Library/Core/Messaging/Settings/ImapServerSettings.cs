namespace Library.Core.Messaging.Settings
{
    public class ImapServerSettings : IIMapServerSettings
    {
        public string ImapServer { get; set; }
        public int Port { get; set; }
        public bool IsSecureSocket { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public int RequestingEntityId { get; set; }
    }
}