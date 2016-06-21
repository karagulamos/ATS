using System;
using System.ComponentModel.Composition;
using Library.Core.Bootstrapper;

namespace Library.Services.Tasks
{
    public class TaskManager
    {
        public static ITaskRunner GetEmailSender()
        {
            return GetTaskRunner(TaskType.EmailSender);
        }

        public static ITaskRunner GetEmailListener()
        {
            return GetTaskRunner(TaskType.EmailListener);
        }

        public static ITaskRunner GetCandidateDocumentReader()
        {
            return GetTaskRunner(TaskType.CandidateDocumentReader);
        }
        
        private static ITaskRunner GetTaskRunner(string contractName)
        {
            return Manager.Value._taskFactory.Create<ITaskRunner>(contractName);
        }

        static readonly Lazy<TaskManager> Manager = new Lazy<TaskManager>(() => new TaskManager(), true);

        private TaskManager()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        [Import]
        private ITaskRunnerFactory _taskFactory;
    }
}
