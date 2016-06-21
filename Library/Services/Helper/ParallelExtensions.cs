using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Library.Services.Helper
{
    public static class ParallelExtensions
    {
        //public static Task ParallelForEach<T>(this IEnumerable<T> source, Func<T, Task> body, int degreeOfParallelism = 0)
        //{
        //    var partitions = Partitioner.Create(source).GetPartitions(degreeOfParallelism > 0 ? degreeOfParallelism : Environment.ProcessorCount);

        //    var tasks = partitions.Select(async partition =>
        //        await Task.Run(async () =>
        //        {
        //            using (partition)
        //            {
        //                while (partition.MoveNext())
        //                {
        //                   await body(partition.Current);
        //                }
        //            }
        //        })
        //    );

        //   return Task.WhenAll(tasks);
        //}

        //public static void ParallelForEach<T>(this IEnumerable<T> source, Action<T> body, int degreeOfParallelism = 0)
        //{
        //    var partitions = Partitioner.Create(source).GetPartitions(degreeOfParallelism > 0 ? degreeOfParallelism : Environment.ProcessorCount);

        //    var tasks = partitions.Select(async partition =>
        //        await Task.Run(() =>
        //        {
        //            using (partition)
        //            {
        //                while (partition.MoveNext())
        //                {
        //                    body(partition.Current);
        //                }
        //            }
        //        })
        //    );

        //    Task.WaitAll(tasks.ToArray());
        //}

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> body)
        {
            List<Exception> exceptions = null;

            await Task.Run(() =>
            {
                foreach (var item in source)
                {
                    try
                    {
                        body(item);
                    }
                    catch (Exception exception)
                    {
                        if (exceptions == null) exceptions = new List<Exception>();
                        exceptions.Add(exception);
                    }
                }
            });

            if (exceptions != null) throw new AggregateException(exceptions);
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            List<Exception> exceptions = null;

            foreach (var item in source)
            {
                try
                {
                    await body(item);
                }
                catch (Exception exception)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(exception);
                }
            }

            if (exceptions != null) throw new AggregateException(exceptions);
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Action<T> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler scheduler = null)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism < 1 ? Environment.ProcessorCount : maxDegreeOfParallelism
            };

            if (scheduler != null)
                options.TaskScheduler = scheduler;

            var block = new ActionBlock<T>(body, options);

            foreach (var item in source)
                block.Post(item);

            block.Complete();
            return block.Completion;
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler scheduler = null)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism < 1 ? Environment.ProcessorCount : maxDegreeOfParallelism
            };

            if (scheduler != null)
                options.TaskScheduler = scheduler;

            var block = new ActionBlock<T>(body, options);

            foreach (var item in source)
                block.Post(item);

            block.Complete();
            return block.Completion;
        }
    }
}
