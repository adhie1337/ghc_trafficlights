namespace TrafficLights.Console
{
    using System.Diagnostics;

    [DebuggerDisplay("{Id}: {Name}")]
    public readonly struct Street
    {
        public readonly int Start;

        public readonly int End;

        public readonly string Name;

        public readonly int Time;
        
        public readonly string Id;

        public Street(int start, int end, string name, int time, string id) => (this.Start, this.End, this.Name, this.Time, this.Id) = (start, end, name, time, id);
        
        public Street(int start, int end, string name, int time) : this(start, end, name, time, NextId) {}

        private static char IdChar = 'A';
        
        private static int IdInt = 0;

        private static string NextId
        {
            get
            {
                var result = $"{IdChar}{IdInt}";
                
                if (IdChar == 'Z')
                {
                    IdChar = 'A';
                    IdInt++;
                }
                else
                {
                    IdChar = (char)((byte)IdChar + 1);
                }

                return result;
            }
        }
    }
}