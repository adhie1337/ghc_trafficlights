namespace TrafficLights.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct Input
    {
        public readonly int Duration;

        public readonly int IntersectionCount;

        public readonly int StreetCount;

        public readonly int CarCount;

        public readonly int Reward;

        public readonly Street[] Streets;

        public readonly Car[] Cars;

        public readonly Intersection[] Intersections;

        public Input(int duration, int intersectionCount, int streetCount, int carCount, int reward, Street[] streets, Car[] cars) => (this.Duration, this.IntersectionCount, this.StreetCount, this.CarCount, this.Reward, this.Streets, this.Cars, this.Intersections) = (duration, intersectionCount, streetCount, carCount, reward, streets, cars, GetIntersections(streets));

        private static Intersection[] GetIntersections(Street[] streets)
        {
            var fromMap = streets.Select(_ => (_.End, _))
                .GroupBy(_ => _.End)
                .ToDictionary(_ => _.Key, _ => _.Select(_ => _._).ToArray());

            var toMap = streets.Select(_ => (_.Start, _))
                .GroupBy(_ => _.Start)
                .ToDictionary(_ => _.Key, _ => _.Select(_ => _._).ToArray());

            var result = new List<Intersection>();

            foreach (var id in fromMap.Keys.Union(toMap.Keys))
            {
                var from = fromMap.TryGetValue(id, out var f) ? f : new Street[0];
                var to = toMap.TryGetValue(id, out var t) ? t : new Street[0];

                result.Add(new Intersection(id, from, to));
            }

            return result.ToArray();
        }
    }
}