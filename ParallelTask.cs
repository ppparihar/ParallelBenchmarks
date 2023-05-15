namespace ParallelBenchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ParallelTask
    {
        static int delay = 50;
        public async static Task SemaphoreSlim(int parallelThread, IEnumerable<int> list)
        {
            var COUNTER = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var allTasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: parallelThread);


            foreach (var quoteId in list)
            {

                await throttler.WaitAsync();
                allTasks.Add(
                     Task.Run(async () =>
                     {
                         try
                         {
                             Thread.Sleep(delay);
                             await Task.Run(() =>
                             {
                                 Interlocked.Increment(ref COUNTER);
                             });
                         }

                         finally
                         {
                             throttler.Release();
                         }
                     }));
            }
            await Task.WhenAll(allTasks);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var elapsedTime = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"[{nameof(SemaphoreSlim)}] COUNTER: {COUNTER} ElapsedMilliseconds: {elapsedTime}");
        }

        public static async Task ParallelForEach(int parallelThread, IEnumerable<int> list)
        {
            //  var list = Enumerable.Range(1, totalRecord);
            var COUNTER = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = parallelThread
            };

            await Parallel.ForEachAsync(list, parallelOptions, async (i, token) =>
            {
                Thread.Sleep(delay);
                await Task.Run(() =>
                {
                    Interlocked.Increment(ref COUNTER);
                });

            });
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var elapsedTime = stopWatch.ElapsedMilliseconds;

            Console.WriteLine($"[{nameof(ParallelForEach)}] COUNTER: {COUNTER} ElapsedMilliseconds: {elapsedTime}");
        }
    }

}
