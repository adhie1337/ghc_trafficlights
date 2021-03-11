namespace TrafficLights.Console.Algorithms
{
    using System.Linq;
    using TrafficLights.Common;

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
                .Select(_ => new IntersectionSchedule(_._, _.Item2.Select(_ => new StreetSchedule(_.f, _.Item2)).ToArray()))
                .ToArray();

            return new Schedule(s);
        }
    }
}
