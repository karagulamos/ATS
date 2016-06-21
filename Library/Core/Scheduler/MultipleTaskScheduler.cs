using System;
using System.Collections.Generic;

namespace Library.Core.Scheduler
{
    public abstract class MultipleTaskScheduler : TaskScheduler
    {
        /// <summary>
        /// Used to define tasks to be executed periodically.
        /// </summary>
        protected sealed class Task
        {
            /// <summary>The task to be performed.</summary>
            public Action Action;
            /// <summary>The mimimum interval at which this task should be repeated. May be delayed arbitrarily though.</summary>
            public TimeSpan MinInterval;
            /// <summary>Stores the last time this task was executed.</summary>
            public DateTime LastExecuted;
            /// <summary>Calculates by how much this task has been delayed. Is used internally to pick the next task to run. Returns negative values for tasks that aren't due yet.</summary>
            public TimeSpan DelayedBy()
            {
                if (LastExecuted == default(DateTime))
                    return TimeSpan.FromDays(1000) - MinInterval; // to run shortest interval first when none of the tasks have ever executed

                return (DateTime.UtcNow - LastExecuted) - MinInterval;
            }
        }

        /// <summary>If desired, override to provide a custom interval at which the scheduler
        /// should re-check whether any task is due to start. Defaults to 1 second.</summary>
        protected override TimeSpan SubsequentInterval { get { return TimeSpan.FromSeconds(1); } }

        /// <summary>Initialize this with the list of tasks to be executed.</summary>
        protected IList<Task> Tasks;

        /// <summary>For internal use.</summary>
        protected sealed override void PeriodicTask()
        {
            TimeSpan maxDelay = TimeSpan.MinValue;
            Task maxDelayTask = null;

            foreach (var task in Tasks)
            {
                var delayedBy = task.DelayedBy();

                if (maxDelay < delayedBy && delayedBy > TimeSpan.Zero)
                {
                    maxDelay = delayedBy;
                    maxDelayTask = task;
                }
            }

            if (maxDelayTask != null)
            {
                maxDelayTask.LastExecuted = DateTime.UtcNow;
                maxDelayTask.Action();
            }
        }
    }
}