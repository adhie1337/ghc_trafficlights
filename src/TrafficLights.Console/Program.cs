namespace TrafficLights.Console
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.IO;
    using TrafficLights.Common;

    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Usage: TrafficLights.Console (-q) (-bb) (-d) <file1> <file2>");
            }

            var quiet = args.Contains("-q");
            var bareBones = args.Contains("-bb");
            var debug = args.Contains("-d");
            var sumScore = 0;
            var sumSw = Stopwatch.StartNew();

            foreach (var path in args.Where(_ => !_.StartsWith("-")))
            {
                if (!quiet) Console.WriteLine($"Processing {path}");

                var input = InputParser.Parse(File.ReadLines(path));

                var sw = Stopwatch.StartNew();
                var schedule = Algorithms.Genetic.Calculate(input, bareBones || quiet);
                sw.Stop();

                schedule = new Schedule(schedule.Get.Where(_ => _.Streets.Any()).ToArray(), schedule.Score);

                if (!quiet && !bareBones && debug) Console.WriteLine($"{path} carCount: {input.CarCount}, streetCount: {input.StreetCount}, intersection count: {input.IntersectionCount}, avg path length: {input.Cars.Average(_ => _.Streets.Length)}, avg street length: {input.Streets.Average(_ => _.Time)}");
                var score = Scorer.Score(input, schedule);

                sumScore += score;

                if (!quiet && !bareBones) Console.WriteLine($"{path} score is {score, 7} in {sw.Elapsed}");

                var outputPath = path.Replace("problems/", "solutions/");
                var parentDir = new FileInfo(outputPath).Directory;

                if (!parentDir!.Exists) parentDir.Create();

                File.WriteAllText(outputPath, SchedulePrinter.Print(schedule));

                if (!quiet && !bareBones) Console.WriteLine();
            }

            if (!quiet) Console.WriteLine($"Done, sum of scores: {sumScore, 8} in {sumSw.Elapsed}!");
        }
    }
}
