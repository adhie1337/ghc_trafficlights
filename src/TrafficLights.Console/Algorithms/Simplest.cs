namespace TrafficLights.Console.Algorithms
{
    using System.Linq;
    using TrafficLights.Common;

    public static class Simplest
    {
        public static Schedule Calculate(Input input)
        {
            var s = input.Intersections.Select(_ => (_, _.From.Select(f => (f, 1)).ToArray())).ToArray();

            return new Schedule(s);
        }
    }
}
