namespace TrafficLights.Console
{
    using System.Collections.Generic;
    using System.Linq;

    public static class InputParser
    {
        public static Input Parse(IEnumerable<string> lines)
        {
            var enumerator = lines.GetEnumerator();
            enumerator.MoveNext();

            var firstLine = enumerator.Current.Split(" ");

            var duration = int.Parse(firstLine[0]);
            var intersectionCount = int.Parse(firstLine[1]);
            var streetCount = int.Parse(firstLine[2]);
            var carCount = int.Parse(firstLine[3]);
            var reward = int.Parse(firstLine[4]);

            var streets = new Street[streetCount];
            var streetMap = new Dictionary<string, Street>(streetCount);

            for (var i = 0; i < streetCount; ++i)
            {
                enumerator.MoveNext();

                var line = enumerator.Current.Split(" ");
                var start = int.Parse(line[0]);
                var end = int.Parse(line[1]);
                var name = line[2];
                var time = int.Parse(line[3]);

                streets[i] = new Street(start, end, name, time);
                streetMap[name] = streets[i];
            }

            var cars = new Car[carCount];

            for (var i = 0; i < carCount; ++i)
            {
                enumerator.MoveNext();

                var line = enumerator.Current.Split(" ");
                var length = int.Parse(line[0]);
                var path = line.Skip(1).Select(_ => streetMap[_]).ToArray();

                cars[i] = new Car(length, path);
            }

            return new Input(duration, intersectionCount, streetCount, carCount, reward, streets, cars);
        }
    }
}