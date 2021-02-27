namespace TrafficLights.Common
{
    public readonly struct Schedule
    {
        public readonly (Intersection Intersection, (Street Street, int Time)[] Streets)[] Get;

        public Schedule((Intersection, (Street, int)[])[] value) => this.Get = value;
    }
}
