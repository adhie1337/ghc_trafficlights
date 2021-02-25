namespace TrafficLights.Console
{
    public readonly struct Car
    {
        public readonly int PathLength;
        
        public readonly Street[] Streets;

        public readonly int Index;

        public Car(int pathLength, Street[] streets, int index) => (this.PathLength, this.Streets, this.Index) = (pathLength, streets, index);
        
        public Car(int pathLength, Street[] streets) : this(pathLength, streets, 0) {}
    }
}