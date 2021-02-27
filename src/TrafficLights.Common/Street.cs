namespace TrafficLights.Common
{
    using System.Diagnostics;

    [DebuggerDisplay("{Id}: {Name}")]
    public readonly struct Street
    {
        public readonly int Start;

        public readonly int End;

        public readonly string Name;

        public readonly int Time;

        public readonly ulong Id;

        public Street(int start, int end, string name, int time, ulong id) => (this.Start, this.End, this.Name, this.Time, this.Id) = (start, end, name, time, id);

        public Street(int start, int end, string name, int time) : this(start, end, name, time, NextId++) {}

        private static ulong NextId = 0;
    }
}
