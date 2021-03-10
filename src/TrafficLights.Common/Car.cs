namespace TrafficLights.Common
{
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("{Path}")]
    public readonly struct Car
    {
        public readonly int PathLength;

        public readonly int Id;

        public readonly Street[] Streets;

        public readonly (int StreetId, int IntersectionId)[] Intersections;

        public string Path => string.Join(" -> ", this.Streets.Select(_ => _.Id));

        public Car(int id, int pathLength, Street[] streets, (int, int)[] intersections) => (this.Id, this.PathLength, this.Streets, this.Intersections) = (id, pathLength, streets, intersections);
    }
}
