namespace TrafficLights.Console
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                Console.WriteLine($"Processing {path}");

                var input = InputParser.Parse(File.ReadLines(path));

                var schedule = Algorithms.SqrtWeightedScheduler.Calculate(input);

                File.WriteAllText(path.Replace("problems/", "solutions/"), SchedulePrinter.Print(schedule));

                Console.WriteLine();
            }
        }
    }
}
