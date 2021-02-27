namespace TrafficLights.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct IntersectionPopularity
    {
        public readonly int Id;

        public readonly int Sum;

        public readonly Dictionary<ulong, int> Popularity;

        public IntersectionPopularity(int id, int sum, Dictionary<ulong, int> popularity) => (this.Id, this.Sum, this.Popularity) = (id, sum, popularity);

        public IntersectionPopularity Simplify
        {
            get
            {
                if (this.Popularity.Values.Count(_ => _ != 0) < 2) return this;

                var gcd = GCD(this.Popularity.Values.Where(_ => _ != 0));

                return gcd == 1
                    ? this
                    : new IntersectionPopularity(this.Id, this.Sum / gcd, this.Popularity.ToDictionary(_ => _.Key, _ => _.Value / gcd));
            }
        }

        private static int GCD(IEnumerable<int> numbers) => numbers.Aggregate(GCD);

        private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
    }
}
