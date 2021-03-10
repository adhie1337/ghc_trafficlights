namespace TrafficLights.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TrafficLights.Common;

    public static class Scorer
    {
        public static int Score(Input input, Schedule schedule)
        {
            var remainingTime = input.Duration + 1;
            var time = 0;
            var score = 0;
            var arrivedCars = new HashSet<int>();
            var carIdQueue = Enumerable.Range(0, input.StreetCount).Select(strId => new Queue<int>(input.Cars.Where(_ => _.Streets[0].Id == strId).Select(_ => _.Id))).ToArray();
            var remainingTimeOnStreet = input.Cars.Select(_ => _.Streets[0].Time).ToArray();
            var carStreetIndexes = Enumerable.Repeat(0, input.CarCount).ToArray();
            var schedules = schedule.Get.ToDictionary
            (
                _ => _.Intersection.Id,
                _ => _.Streets
                    .ScanM(new { Id = 0, Wait = 0, Time = 0 }, (acc, st) => new { st.Street.Id, Wait = acc.Wait + acc.Time, st.Time })
                    .ToDictionary(_ => _.Id, _ => (_.Wait, _.Time))
            );
            var scheduleCycles = schedules.ToDictionary(_ => _.Key, _ => _.Value.Values.Sum(_ => _.Time));
            var intersectionsDone = new HashSet<int>();

            //Console.WriteLine(string.Join("\n\n", schedules.Select(_ => $"Intersection {_.Key} schedule:\n - " + string.Join("\n - ", _.Value.Select(_ => $"Street: {_.Key}, wait: {_.Value.Wait}, time: {_.Value.Time}")))));

            //Console.WriteLine("cars:\n - " + string.Join("\n - ", input.Cars.Select(c => c.Id + "\n  - " + string.Join("\n  - ", c.Streets.Select(_ => $"{_.Id} -> {_.End} in {_.Time}")))));

            while (remainingTime-- > 0)
            {
                intersectionsDone.Clear();

                foreach (var car in input.Cars)
                {
                    if (arrivedCars.Contains(car.Id)) continue;

                    var streetIndex = carStreetIndexes[car.Id];
                    var streetId = car.Streets[streetIndex].Id;

                    var remaining = remainingTimeOnStreet[car.Id] = Math.Max(0, remainingTimeOnStreet[car.Id] - 1);

                    //Console.WriteLine($"Car {car.Id}, remaining time: {remaining}");

                    if (remaining == 0 && streetIndex < car.PathLength - 1)
                    {
                        if (!carIdQueue[streetId].Contains(car.Id)) carIdQueue[streetId].Enqueue(car.Id);
                        //Console.WriteLine($"Car {car.Id}, is at the end of: {streetId}");
                    }

                    if (remaining != 0)
                    {
                        //Console.WriteLine($"Skip car {car.Id} because it has remaining time on the road: {remaining}\n");
                        continue;
                    }

                    if (streetIndex == car.PathLength - 1)
                    {
                        //Console.WriteLine($"Car {car.Id} arrived! Reward: {input.Reward} remaining time: {remainingTime}\n");
                        arrivedCars.Add(car.Id);
                        score += (input.Reward + remainingTime);
                        continue;
                    }
                    else if (remainingTime == 0)
                    {
                        //Console.WriteLine("Skipping because there is no time left.\n");
                        continue;
                    }

                    var queue = carIdQueue[streetId];
                    if (queue.Count == 0 || queue.Peek() != car.Id)
                    {
                        //Console.WriteLine("Skipping because car is not the first in line.\n");
                        continue;
                    }
                    var intersectionId = car.Intersections[streetIndex].IntersectionId;
                    if (intersectionsDone.Contains(intersectionId))
                    {
                        //Console.WriteLine("Skipping because intersection is already done.\n");
                        continue;
                    }

                    var s = schedules[intersectionId][streetId];

                    var timePart = scheduleCycles.TryGetValue(intersectionId, out var r) ? time % r : -1;

                    if (timePart == -1 || timePart < s.Wait || timePart >= s.Wait + s.Time)
                    {
                        //Console.WriteLine("Skipping because of bad timeframe.\n");
                        continue;
                    }

                    intersectionsDone.Add(intersectionId);
                    queue.Dequeue();
                    carStreetIndexes[car.Id] = streetIndex + 1;
                    var newStreet = car.Streets[streetIndex + 1];
                    remainingTimeOnStreet[car.Id] = newStreet.Time;

                    //Console.WriteLine($"Car {car.Id} goes through intersection {intersectionId} to {newStreet.Id} for {newStreet.Time}\n");
                }

                var qs = carIdQueue.Select((q, i) => $"[{ i }: { string.Join(", ", q) }]");
                //Console.WriteLine($"Queues: {string.Join(" ", qs)}\n");

                time++;
            }

            return score;
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<U> ScanM<T, U>(this IEnumerable<T> input, U state, Func<U, T, U> next)
        {
            foreach(var item in input)
            {
                state = next(state, item);
                yield return state;
            }
        }
    }
}
