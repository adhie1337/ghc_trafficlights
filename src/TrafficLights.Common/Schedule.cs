namespace TrafficLights.Common
{
    public readonly struct Schedule
    {
        public readonly IntersectionSchedule[] Get;

        public readonly int? Score;

        public Schedule(IntersectionSchedule[] value, int? score) => (this.Get, this.Score) = (value, score);

        public Schedule(IntersectionSchedule[] value) : this(value, null) { }

        public static Schedule Empty(Input input)
        {
            var intersectionSchedules = new IntersectionSchedule[input.IntersectionCount];

            for (var i = 0; i < input.IntersectionCount; ++i)
            {
                var intersection = input.Intersections[i];
                var streetSchedules = new StreetSchedule[intersection.From.Length];

                for (var j = 0; j < intersection.From.Length; ++j) streetSchedules[j] = new StreetSchedule(intersection.From[j], 1);

                intersectionSchedules[i] = new IntersectionSchedule(intersection, streetSchedules);
            }

            return new Schedule(intersectionSchedules);
        }
    }

    public readonly struct IntersectionSchedule
    {
        public readonly Intersection Intersection;

        public readonly StreetSchedule[] Streets;

        public IntersectionSchedule(Intersection intersection, StreetSchedule[] streets) => (this.Intersection, this.Streets) = (intersection, streets);
    }

    public readonly struct StreetSchedule
    {
        public readonly Street Street;

        public readonly int Time;

        public StreetSchedule(Street street, int time) => (this.Street, this.Time) = (street, time);
    }
}
