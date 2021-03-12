namespace TrafficLights.Console
{
    using System;
    using System.Collections.Generic;
    using TrafficLights.Common;

    public static class Scorer
    {
        public static Schedule WithScore(Input input, Schedule schedule)
        {
            if (schedule.Score.HasValue) return schedule;

            var score = Score(input, schedule);

            return new Schedule(schedule.Get, score);
        }

        public static int Score(Input input, Schedule schedule)
        {
            if (schedule.Score.HasValue) return schedule.Score.Value;

            var remainingTime = input.Duration + 1;
            var time = 0;
            var score = 0;
            var arrivedCars = new HashSet<int>();
            var carIdQueue = new int?[input.StreetCount];
            var remainingTimeOnStreet = new int[input.CarCount];
            for (var i = 0; i < input.CarCount; ++i) remainingTimeOnStreet[i] = input.Cars[i].Streets[0].Time;
            var carStreetIndexes = new int[input.CarCount];
            var schedules = new Dictionary<int, (int Wait, int Time)>[input.IntersectionCount];
            var scheduleCycles = new int[input.IntersectionCount];

            foreach (var sch in schedule.Get)
            {
                var d = new Dictionary<int, (int Wait, int Time)>();
                var wait = 0;

                for (var i = 0; i < sch.Streets.Length; ++i)
                {
                    var s = sch.Streets[i];
                    d[s.Street.Id] = (wait, s.Time);
                    wait += s.Time;
                }

                scheduleCycles[sch.Intersection.Id] = wait;

                schedules[sch.Intersection.Id] = d;
            }

            var intersectionsDone = new HashSet<int>();

            while (remainingTime-- > 0)
            {
                intersectionsDone.Clear();

                for (var i = 0; i < input.Cars.Length; ++i)
                {
                    var car = input.Cars[i];

                    if (arrivedCars.Contains(car.Id)) continue;

                    var streetIndex = carStreetIndexes[car.Id];
                    var streetId = car.Streets[streetIndex].Id;

                    var remaining = remainingTimeOnStreet[car.Id] - 1;
                    if (remaining < 0) remaining = 0;
                    remainingTimeOnStreet[car.Id] = remaining;

                    if (remaining == 0
                        && streetIndex < car.PathLength - 1
                        && !carIdQueue[streetId].HasValue)
                    {
                        carIdQueue[streetId] = car.Id;
                    }

                    if (remaining != 0) continue;

                    if (streetIndex == car.PathLength - 1)
                    {
                        arrivedCars.Add(car.Id);
                        score += (input.Reward + remainingTime);
                        continue;
                    }
                    else if (remainingTime == 0)
                    {
                        continue;
                    }

                    if (carIdQueue[streetId] != car.Id) continue;

                    var intersectionId = car.Intersections[streetIndex].IntersectionId;

                    if (intersectionsDone.Contains(intersectionId)) continue;

                    var cycle = scheduleCycles[intersectionId];
                    var timePart = cycle > 0 ? time % cycle : -1;

                    if (timePart == -1) continue;

                    var s = schedules[intersectionId] != null && schedules[intersectionId].ContainsKey(streetId)
                        ? schedules[intersectionId][streetId]
                        : null as (int Wait, int Time)?;

                    if (!s.HasValue || timePart < s.Value.Wait || timePart >= s.Value.Wait + s.Value.Time) continue;

                    intersectionsDone.Add(intersectionId);
                    carIdQueue[streetId] = null;
                    carStreetIndexes[car.Id] = streetIndex + 1;
                    var newStreet = car.Streets[streetIndex + 1];
                    remainingTimeOnStreet[car.Id] = newStreet.Time;
                }

                time++;
            }

            return score;
        }
    }
}
