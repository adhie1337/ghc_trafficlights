namespace TrafficLights.Console
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using TrafficLights.Common;

    class Program
    {
        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                //Console.WriteLine($"Processing {path}");

                var input = InputParser.Parse(File.ReadLines(path));

                var schedule = Algorithms.SqrtWeightedSchedulerOrdered.Calculate(input);

                var sw = Stopwatch.StartNew();
                var score = Scorer.Score(input, schedule);
                var count = 10;

                for (var i = 1; i < count; ++ i) score = Scorer.Score(input, schedule);
                sw.Stop();

                Console.WriteLine($"{path} score is {score, 7} in {sw.Elapsed / count}");

                var outputPath = path.Replace("problems/", "solutions/");
                var parentDir = new FileInfo(outputPath).Directory;

                if (!parentDir!.Exists) parentDir.Create();

                File.WriteAllText(outputPath, SchedulePrinter.Print(schedule));

                //Console.WriteLine();
            }

            Console.WriteLine("Done!");
        }
    }
}
