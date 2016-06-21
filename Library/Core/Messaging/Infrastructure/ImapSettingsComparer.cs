using System.Collections.Generic;
using Library.Core.Messaging.Settings;

namespace Library.Core.Messaging.Infrastructure
{
    public class ImapSettingsComparer : IEqualityComparer<IIMapServerSettings>
    {
        public bool Equals(IIMapServerSettings x, IIMapServerSettings y)
        {
            return x.UserLogin == y.UserLogin;
        }

        public int GetHashCode(IIMapServerSettings obj)
        {
            return obj.RequestingEntityId;
        }
    }
}
