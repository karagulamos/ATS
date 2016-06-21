using System;
using System.Diagnostics;
using System.ServiceProcess;
using Library.Services.Tasks.Config;

namespace ATS.Desktop.Service
{
    public partial class AtsDesktopService : ServiceBase
    {
        public AtsDesktopService()
        {
            InitializeComponent();
        }

        private readonly MessagingTaskScheduler _messagingTaskScheduler = new MessagingTaskScheduler();

        protected override void OnStart(string[] args)
        {
            try
            {
                EventLog.WriteEntry("Starting ATS Messaging Service...", EventLogEntryType.Information);
                _messagingTaskScheduler.Start();
                EventLog.WriteEntry("ATS Messaging Service Successfully Started.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ATS Messaging Service could not be started : " + ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                EventLog.WriteEntry("Shutting Down ATS Messaging Service...", EventLogEntryType.Information);
                _messagingTaskScheduler.Shutdown(true);
                EventLog.WriteEntry("ATS Messaging Service Stopped", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ATS Messaging Service Encountered An Error Shutting Down " + ex.Message, EventLogEntryType.Error);
            }

        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
