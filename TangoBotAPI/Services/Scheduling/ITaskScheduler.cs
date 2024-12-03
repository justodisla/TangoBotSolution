using System;
using System.Threading.Tasks;

namespace TangoBotApi.Services.Scheduling
{
    /// <summary>
    /// Provides functionalities to schedule and manage tasks.
    /// </summary>
    public interface ITaskScheduler
    {
        /// <summary>
        /// Schedules a task to run at specified intervals.
        /// </summary>
        /// <param name="interval">The interval at which the task should run.</param>
        /// <param name="task">The task to run.</param>
        void ScheduleRecurringTask(TimeSpan interval, Func<Task> task);

        /// <summary>
        /// Schedules a task to run once at a specified time.
        /// </summary>
        /// <param name="runAt">The time at which the task should run.</param>
        /// <param name="task">The task to run.</param>
        void ScheduleOneTimeTask(DateTime runAt, Func<Task> task);
    }
}


