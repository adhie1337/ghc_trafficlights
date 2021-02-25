namespace TrafficLights.Console
{
    public readonly struct Intersection
    {
        public readonly int Id;

        public readonly Street[] From;
        
        public readonly Street[] To;

        public Intersection(int id, Street[] from, Street[] to) => (this.Id, this.From, this.To) = (id, from, to);
    }
}