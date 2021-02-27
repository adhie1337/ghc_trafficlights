namespace TrafficLights.Common
{
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

        public Input(int duration, int intersectionCount, int streetCount, int carCount, int reward, Street[] streets, Car[] cars, Intersection[] intersections) => (this.Duration, this.IntersectionCount, this.StreetCount, this.CarCount, this.Reward, this.Streets, this.Cars, this.Intersections) = (duration, intersectionCount, streetCount, carCount, reward, streets, cars, intersections);
    }
}
