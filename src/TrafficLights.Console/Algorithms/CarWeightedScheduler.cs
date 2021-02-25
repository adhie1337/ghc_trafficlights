namespace TrafficLights.Console.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CarWeightedScheduler
    {
        public static Schedule Calculate(Input input)
        {
            var streetPopularity = input.Cars.SelectMany(_ => Enumerable.Range(0, _.PathLength).SelectMany(i => _.Streets.Select(_ => _.Id)))
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

        private readonly struct IntersectionPopularity
        {
            internal readonly int Id;

            internal readonly int Sum;

            internal readonly Dictionary<string, int> Popularity;

            internal IntersectionPopularity(int id, int sum, Dictionary<string, int> popularity) => (this.Id, this.Sum, this.Popularity) = (id, sum, popularity);

            internal IntersectionPopularity Simplify
            {
                get
                {
                    if (this.Popularity.Values.Count(_ => _ != 0) < 2) return this;

                    var gcd = GCD(this.Popularity.Values.Where(_ => _ != 0));

                    return gcd == 1 ? this
                        : new IntersectionPopularity(this.Id, this.Sum / gcd, this.Popularity.ToDictionary(_ => _.Key, _ => _.Value / gcd));
                }
            }

            static int GCD(IEnumerable<int> numbers) => numbers.Aggregate(GCD);

            static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
        }
    }
}