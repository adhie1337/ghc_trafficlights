namespace TrafficLights.Common
{
    using System.Diagnostics;

    [DebuggerDisplay("{Id}: {Name}")]
    public readonly struct Street
    {
        public readonly int Id;

        public readonly int Start;

        public readonly int End;

        public readonly string Name;

        public readonly int Time;

        public Street(int id, int start, int end, string name, int time) => (this.Id, this.Start, this.End, this.Name, this.Time) = (id, start, end, name, time);
    }
}
