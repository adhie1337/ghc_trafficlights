namespace TrafficLights.Console
{
    using System;
    using System.IO;
    using TrafficLights.Common;

    class Program
    {
        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                Console.WriteLine($"Processing {path}");

                var input = InputParser.Parse(File.ReadLines(path));

                var schedule = SchedulePrinter.Print(Algorithms.SqrtWeightedScheduler.Calculate(input));

                var outputPath = path.Replace("problems/", "solutions/");
                var parentDir = new FileInfo(outputPath).Directory;

                if (!parentDir!.Exists) parentDir.Create();

                File.WriteAllText(outputPath, schedule);

                Console.WriteLine();
            }

            Console.WriteLine("Done!");
        }
    }
}
