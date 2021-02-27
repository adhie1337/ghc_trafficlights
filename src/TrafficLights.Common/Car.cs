namespace TrafficLights.Common
{
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("{Path}")]
    public readonly struct Car
    {
        public readonly int PathLength;

        public readonly Street[] Streets;

        public readonly (ulong StreetId, int IntersectionId)[] Intersections;

        public string Path => string.Join(" -> ", this.Streets.Select(_ => _.Id));

        public Car(int pathLength, Street[] streets, (ulong, int)[] intersections) => (this.PathLength, this.Streets, this.Intersections) = (pathLength, streets, intersections);
    }
}
