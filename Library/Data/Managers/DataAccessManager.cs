using Library.Models;

namespace Library.Data.Managers
{
    public static class DataAccessManager
    {
        public static IDataManager<OutboundEmail> GetOutboundEmailDataManager()
        {
            return new DataManager<OutboundEmail>();
        }

        public static IDataManager<SmtpDetail> GetSmtpDetailDataManager()
        {
            return new DataManager<SmtpDetail>();
        }
    }
}
