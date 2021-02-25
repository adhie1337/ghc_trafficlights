namespace TrafficLights.Console.Algorithms
{
    using System;
    using System.Linq;

    public static class SimplestWithoutUnnecessary
    {
        public static Schedule Calculate(Input input)
        {
            var i = input.Cars.SelectMany(_ => _.Streets.Select(_ => _.Id)).ToHashSet();

            var s = input.Intersections
                .Select(_ => (_, _.From
                    .Where(_ => i.Contains(_.Id))
                    .Select(f => (f, 1))
                    .ToArray()))
                .Where(_ => _.Item2.Any())
                .ToArray();

            return new Schedule(s);
        }
    }
}