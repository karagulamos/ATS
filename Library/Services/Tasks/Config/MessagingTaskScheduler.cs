using System;
using System.Runtime;
using log4net.Config;
using Library.Core.Scheduler;

namespace Library.Services.Tasks.Config
{
    public class MessagingTaskScheduler : MultipleTaskScheduler
    {
        // The first task will start N seconds after this class is instantiated and started:
        protected override TimeSpan FirstInterval { get { return TimeSpan.FromSeconds(5); } }

        public MessagingTaskScheduler()
        {
            XmlConfigurator.Configure(); // Bootstrap Log4net file logger
            GCSettings.LatencyMode = GCLatencyMode.Interactive;

            Tasks = new[] {
                new Task { Action = CandidateDocumentReaderTask, MinInterval = TimeSpan.FromMinutes(1) },
                new Task { Action = ExecuteEmailListenerTask, MinInterval = TimeSpan.FromMinutes(2) },
                //new Task { Action = ExecuteEmailSenderTask, MinInterval = TimeSpan.FromMinutes(2) }
            };
        }

        private static void ExecuteEmailListenerTask()
        {
            TaskManager.GetEmailListener().Execute();
        }

        private static void ExecuteEmailSenderTask()
        {
            TaskManager.GetEmailSender().Execute();
        }

        private static void CandidateDocumentReaderTask()
        {
            TaskManager.GetCandidateDocumentReader().Execute();
        }
    }
}
