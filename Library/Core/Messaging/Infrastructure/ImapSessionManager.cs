/* 
 *  Author: Alexander Karagulamos
 *  Description: IMAP Session Manager (May 11, 2015)
 *               Builds a cache of active IMAP connections and serves them up on demand.
 *               Refreshes the cache once an expired session is detected.   
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Library.Core.Bootstrapper;
using Library.Core.Messaging.Settings;

namespace Library.Core.Messaging.Infrastructure
{
    [Export(typeof(IImapSessionManager))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ImapSessionManager : IImapSessionManager
    {
        [Import]
        private IImapMailReader _imapMailReader;

        public ImapSessionManager()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        // Unit Test Only
        public ImapSessionManager(IImapMailReader imapMailReader)
        {
            _imapMailReader = imapMailReader;
        }

        public bool IsSessionCacheEmpty
        {
            get { return ImapSessionCache.Count <= 0; }
        }

        public void BuildSessionCache(IList<IIMapServerSettings> smtpSettings)
        {
            foreach (var setting in smtpSettings)
            {
                UpdateSessionCache(setting);
            }
        }

        public IImapMailReader GetActiveImapSession(IIMapServerSettings imapSetting)
        {
            return ImapSessionCache[imapSetting];
        }

        public List<InboxMessageWrapper> GetUnreadInboxMails(int messageCount)
        {
            var activeImapSessions = ImapSessionCache.Where(session => session.Value.HasUnreadMail()).ToArray();

            _currentlyRetrievingEmails = activeImapSessions.Any();

            var retrievedEmails = new List<InboxMessageWrapper>();

            foreach(var session in activeImapSessions)
            {
                foreach (var unreadMail in session.Value.GetUnreadMails("INBOX", messageCount))
                {
                    retrievedEmails.Add(new InboxMessageWrapper
                    {
                        ImapSetting = session.Key,
                        Message = unreadMail
                    });
                }
            };

            return retrievedEmails;
        }

        private void UpdateSessionCache(IIMapServerSettings setting)
        {
            if (!ImapSessionCache.ContainsKey(setting)) // Add if session doesn't already exist
            {
                ImapSessionCache.TryAdd(setting, _imapMailReader.CreateConnection(setting));
            }
            else
            {
                RefreshSessionIfExpired(setting);
            }
        }

        private void RefreshSessionIfExpired(IIMapServerSettings setting)
        {
            var session = ImapSessionCache.FirstOrDefault(s => s.Key == setting).Value;

            if (session != null && !session.IsConnected())
            {
                IImapMailReader removed;
                if (ImapSessionCache.TryRemove(setting, out removed) && removed != null)
                {
                    ImapSessionCache.TryAdd(setting, _imapMailReader.CreateConnection(setting));
                }
            }
        }

        ~ImapSessionManager()
        {
            if (TimeElapsed.TotalMinutes > 7 && !_currentlyRetrievingEmails)
            {
                ClearSessionCache();
            }
        }

        public void ClearSessionCache()
        {
            if (!IsSessionCacheEmpty)
            {
                ImapSessionCache.Clear();

                _lastTimeCacheWasCleared = DateTime.UtcNow;
            }
        }

        private static TimeSpan TimeElapsed
        {
            get { return DateTime.UtcNow - _lastTimeCacheWasCleared; }
        }

        private bool _currentlyRetrievingEmails;
        private static DateTime _lastTimeCacheWasCleared = DateTime.UtcNow;
        private static readonly ConcurrentDictionary<IIMapServerSettings, IImapMailReader> ImapSessionCache = new ConcurrentDictionary<IIMapServerSettings, IImapMailReader>(new ImapSettingsComparer());
    }
}
