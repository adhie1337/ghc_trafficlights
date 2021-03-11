namespace TrafficLights.Console.Algorithms
{
    using System;
    using System.Linq;
    using TrafficLights.Common;

    public static class SqrtWeightedSchedulerOrdered
    {
        public static Schedule Calculate(Input input)
        {
            var streetPopularity = input.Cars.SelectMany(_ => _.Intersections)
                .ToLookup(_ => _)
                .ToDictionary(_ => _.Key, _ => _.Count());

            var intersectionPopularity = input.Intersections
                .Select(i => new { i.Id, Popularity = i.From.ToDictionary(s => s.Id, s => streetPopularity.TryGetValue(new StreetIntersection(s.Id, i.Id), out var p) ? p : 0)})
                .Select(i => new IntersectionPopularity(i.Id, i.Popularity.Values.Sum(), i.Popularity).Simplify)
                .ToDictionary(_ => _.Id);

            var s = input.Intersections
                .Select(intersection => (intersection, intersection.From
                    .Where(s => intersectionPopularity[intersection.Id].Popularity.TryGetValue(s.Id, out var p) && p != 0)
                    .Select(s => (s, (int)Math.Max(1, Math.Floor(Math.Sqrt(intersectionPopularity[intersection.Id].Popularity[s.Id]) / 2.0))))
                    .ToArray()))
                .Where(_ => _.Item2.Any())
                .Select(_ => (_.intersection, _.Item2.OrderByDescending(inner => streetPopularity[new StreetIntersection(inner.s.Id, _.intersection.Id)]).Reverse().ToArray()))
                .Select(_ => new IntersectionSchedule(_.intersection, _.Item2.Select(_ => new StreetSchedule(_.s, _.Item2)).ToArray()))
                .ToArray();

            return new Schedule(s);
        }
    }
}