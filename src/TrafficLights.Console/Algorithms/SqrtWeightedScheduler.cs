namespace TrafficLights.Console.Algorithms
{
    using System;
    using System.Linq;
    using TrafficLights.Common;

    public static class SqrtWeightedScheduler
    {
        public static Schedule Calculate(Input input)
        {
            var streetPopularity = input.Cars.SelectMany(_ => _.Intersections)
                .ToLookup(_ => _)
                .ToDictionary(_ => _.Key, _ => _.Count());

            var intersectionPopularity = input.Intersections
                .Select(i => new { i.Id, Popularity = i.From.ToDictionary(s => s.Id, s => streetPopularity.TryGetValue((s.Id, i.Id), out var p) ? p : 0)})
                .Select(i => new IntersectionPopularity(i.Id, i.Popularity.Values.Sum(), i.Popularity).Simplify)
                .ToDictionary(_ => _.Id);

            var s = input.Intersections
                .Select(intersection => (intersection, intersection.From
                    .Where(s => intersectionPopularity[intersection.Id].Popularity.TryGetValue(s.Id, out var p) && p != 0)
                    .Select(s => (s, (int)Math.Max(1, Math.Min(6, Math.Floor(1.5 * Math.Pow(intersectionPopularity[intersection.Id].Popularity[s.Id], 1d / 3d))))))
                    .ToArray()))
                .Where(_ => _.Item2.Any())
                .ToArray();

            return new Schedule(s);
        }

        private static double Half(double d) => (d - 1d) / 2d + 1d;
    }
}
