using System.Collections.Generic;
using Library.Core.Messaging.Settings;

namespace Library.Core.Messaging.Infrastructure
{
    public interface IImapSessionManager
    {
        void BuildSessionCache(IList<IIMapServerSettings> smtpSettings);
        IImapMailReader GetActiveImapSession(IIMapServerSettings imapSetting);
        List<InboxMessageWrapper> GetUnreadInboxMails(int messageCount = 1);
        void ClearSessionCache();

        bool IsSessionCacheEmpty { get; }
    }
}