namespace TrafficLights.Common
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

                streets[i] = new Street(i, start, end, name, time);
                streetMap[name] = streets[i];
            }

            var intersections = GetIntersections(streets);

            var intersectionMap = intersections
                .SelectMany(intersection => intersection.From.SelectMany(f => intersection.To.Select(t => new { From = f.Id, intersection.Id, To = t.Id })))
                .ToDictionary(t => (t.From, t.To), t => t.Id);

            var cars = new Car[carCount];

            for (var i = 0; i < carCount; ++i)
            {
                enumerator.MoveNext();

                var line = enumerator.Current.Split(" ");
                var length = int.Parse(line[0]);
                var path = line.Skip(1).Select(_ => streetMap[_]).ToArray();
                var intersectionPath = path.Zip(path.Skip(1))
                    .Select(p => (p.First.Id, intersectionMap[(p.First.Id, p.Second.Id)]))
                    .ToArray();

                cars[i] = new Car(i, length, path, intersectionPath);
            }

            return new Input(duration, intersectionCount, streetCount, carCount, reward, streets, cars, intersections);
        }

        private static Intersection[] GetIntersections(Street[] streets)
        {
            var fromMap = streets.Select(_ => (_.End, _))
                .GroupBy(_ => _.End)
                .ToDictionary(_ => _.Key, _ => _.Select(_ => _._).ToArray());

            var toMap = streets.Select(_ => (_.Start, _))
                .GroupBy(_ => _.Start)
                .ToDictionary(_ => _.Key, _ => _.Select(_ => _._).ToArray());

            var result = new List<Intersection>();

            foreach (var id in fromMap.Keys.Union(toMap.Keys))
            {
                var from = fromMap.TryGetValue(id, out var f) ? f : new Street[0];
                var to = toMap.TryGetValue(id, out var t) ? t : new Street[0];

                result.Add(new Intersection(id, from, to));
            }

            return result.ToArray();
        }
    }
}
