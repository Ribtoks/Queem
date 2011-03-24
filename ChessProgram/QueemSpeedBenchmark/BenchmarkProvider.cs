﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace QueemSpeedBenchmark
{
    public class BenchmarkProvider
    {
        protected List<BenchmarkItem> benchmarks;

        public BenchmarkProvider(string directoryPath)
        {
            ReadBenchmarks(directoryPath);
        }

        protected void ReadBenchmarks(string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            // .cbi - Chess Benchmark Item
            FileInfo[] benchmarkFiles = di.GetFiles("*.cbi");

            benchmarks = new List<BenchmarkItem>();
            foreach (var file in benchmarkFiles)
            {
                benchmarks.Add(
                    new BenchmarkItem(
                        File.ReadAllLines(file.FullName)));
            }
        }

        public void RunBenchmarks(int maxdepth)
        {
            ParallelLoopResult result = Parallel.For(0, benchmarks.Count,
                (i) =>
                {
                    benchmarks[i].Run(maxdepth);
                });

            // wait for all tests complete
            while (!result.IsCompleted) ;

            // do other stuff...
        }
    }
}
