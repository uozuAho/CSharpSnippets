using System;
using System.Threading;

namespace CSharpSnippets.Profiling
{
    class ProfilerTester
    {
        public static void Run()
        {
            HeavyProcessingLoad();
            LongRunningIdle();
        }

        private static void HeavyProcessingLoad()
        {
            var random = new Random();
            for (int i = 0; i < 1000000; i++)
            {
                var a = Math.Sqrt(random.NextDouble());
            }
        }

        private static void LongRunningIdle()
        {
            Thread.Sleep(500);
        }
    }
}
