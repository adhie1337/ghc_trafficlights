namespace TrafficLights.Common
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("{Path}")]
    public readonly struct Car
    {
        public readonly int PathLength;

        public readonly int Id;

        public readonly Street[] Streets;

        public readonly StreetIntersection[] Intersections;

        public string Path => string.Join(" -> ", this.Streets.Select(_ => _.Id));

        public Car(int id, int pathLength, Street[] streets, StreetIntersection[] intersections) => (this.Id, this.PathLength, this.Streets, this.Intersections) = (id, pathLength, streets, intersections);
    }

    public readonly struct StreetIntersection : IEquatable<StreetIntersection>
    {
        public readonly int StreetId;

        public readonly int IntersectionId;

        public StreetIntersection(int streetId, int intersectionId) => (this.StreetId, this.IntersectionId) = (streetId, intersectionId);

        public override bool Equals(object? obj) => obj is StreetIntersection && this.Equals((StreetIntersection)obj);

        public bool Equals(StreetIntersection other)
            => this.StreetId.Equals(other.StreetId)
            && this.IntersectionId.Equals(other.IntersectionId);

        public override int GetHashCode() => unchecked(this.StreetId.GetHashCode() * 31 + this.IntersectionId.GetHashCode());
    }
}
