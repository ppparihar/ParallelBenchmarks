namespace ParallelBenchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Running;

    [SimpleJob(RunStrategy.ColdStart, iterationCount: 10)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class ParallelBenchmarks
    {
        readonly int maxthread = 5;
        readonly IEnumerable<int> list = Enumerable.Range(1, 200);

        [Benchmark]
        public async Task SemaphoreSlim() => await ParallelTask.SemaphoreSlim(maxthread, list);

        [Benchmark]
        public async Task ParallelForEach() => await ParallelTask.ParallelForEach(maxthread, list);
    }
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ParallelBenchmarks>();

            /* Uncomment following code to debug */

            //int maxthread = 30;
            //var totalRecord = 1000;
            //IEnumerable<int> list = Enumerable.Range(1, totalRecord);
            //await ParallelTask.SemaphoreSlim(maxthread, list);
            //await ParallelTask.ParallelForEach(maxthread, list);
            //Console.ReadLine();
        }
    }

}
