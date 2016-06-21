﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Library.Data.Common;
using Library.Data.Common.Extensions;
using Library.Models;

namespace Library.Data.Repositories
{
    [Export(typeof(IOutboundEmailRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class OutboundEmailRepository : AbstractDataManager<OutboundEmail, AtsDbContext>, IOutboundEmailRepository
    {
        public IList<OutboundEmailDetail> GetOutboundEmailDetails()
        {

            using (var dbContext = new AtsDbContext())
            {
                var maxAttempts = Convert.ToInt32(ConfigurationManager.AppSettings["MaxEmailAttempts"]);
                var outboundHost = ConfigurationManager.AppSettings["SmtpServer"];
                var outboundPort = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                var userName = ConfigurationManager.AppSettings["Sender"];
                var password = ConfigurationManager.AppSettings["Password"];
                var useSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;

                var details = from email in dbContext.OutboundEmails.Where(CanSend().And(AttemptsNotExceeded(maxAttempts)))
                               select new OutboundEmailDetail
                               {
                                   OutboundHost = outboundHost,
                                   OutboundPort = outboundPort,
                                   Sender = email.Sender,
                                   Subject = email.Subject,
                                   Username = userName,
                                   Password = password,
                                   UseSsl = useSsl,
                                   Body = email.Body,
                                   OutboundEmail = email,
                                   OutboundAttachments = email.OutboundAttachments,
                                   OutboundImages = email.OutboundImages,
                                   OutboundRecipients = email.OutboundRecipients
                               };

                return details.ToArray();
            }
        }

        #region Query Helpers

        private static Expression<Func<OutboundEmail, bool>> CanSend()
        {
            return email => email.EmailStatusId != (int)EmailStatuses.Sent;
        }

        private static Expression<Func<OutboundEmail, bool>> AttemptsNotExceeded(int maxAttempts)
        {
            return email => email.Attempts < maxAttempts;
        }

        #endregion
    }
}