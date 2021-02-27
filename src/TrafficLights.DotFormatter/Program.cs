namespace TrafficLights.DotFormatter
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

                var dot = InputToDot.Format(input);

                var outputPath = path.Replace("problems/", "graphs/").Replace(".txt", ".dot");
                var parentDir = new FileInfo(outputPath).Directory;

                if (!parentDir!.Exists) parentDir.Create();

                File.WriteAllText(outputPath, dot);

                Console.WriteLine();
            }

            Console.WriteLine("Done!");
        }
    }
}
