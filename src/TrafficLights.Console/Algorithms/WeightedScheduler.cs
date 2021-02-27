namespace TrafficLights.Console.Algorithms
{
    using System.Linq;
    using TrafficLights.Common;

    public static class WeightedScheduler
    {
        public static Schedule Calculate(Input input)
        {
            var streetPopularity = input.Cars.SelectMany(_ => _.Streets.Select(_ => _.Id))
                .ToLookup(_ => _)
                .ToDictionary(_ => _.Key, _ => _.Count());

            var intersectionPopularity = input.Intersections
                .Select(i => new { i.Id, Popularity = i.From.ToDictionary(s => s.Id, s => streetPopularity.TryGetValue(s.Id, out var p) ? p : 0)})
                .Select(i => new IntersectionPopularity(i.Id, i.Popularity.Values.Sum(), i.Popularity).Simplify)
                .ToDictionary(_ => _.Id);

            var s = input.Intersections
                .Select(intersection => (intersection, intersection.From
                    .Where(s => streetPopularity.ContainsKey(s.Id) && intersectionPopularity[intersection.Id].Popularity[s.Id] != 0)
                    .Select(s => (s, intersectionPopularity[intersection.Id].Popularity[s.Id]))
                    .ToArray()))
                .Where(_ => _.Item2.Any())
                .ToArray();

            return new Schedule(s);
        }
    }
}
