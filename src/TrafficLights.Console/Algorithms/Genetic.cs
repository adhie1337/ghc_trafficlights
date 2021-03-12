namespace TrafficLights.Console.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using TrafficLights.Common;

    public static class Genetic
    {
        const int PopulationSize = 100;

        public static Random Random = new Random(42);

        public static Schedule Calculate(Input input, bool bareBones = true)
        {
            var baseSchedule = Schedule.Empty(input);
            var population = GetRandomPopulation(baseSchedule);
            var copy = new Schedule[population.Length * 2];

            var limit = TimeSpan.FromMinutes(2.0 * (input.CarCount / 1000d + input.StreetCount / 100000d + input.IntersectionCount / 10000d) / 3);

            if (!bareBones) Console.WriteLine($"Running for {limit}");
            var sw = Stopwatch.StartNew();
            var rounds = 0;

            do 
            {
                GetNextPopulation(input, population, copy);
                rounds++;
            } while (sw.Elapsed < limit);

            sw.Stop();
            if (!bareBones) Console.WriteLine($"Done {rounds} rounds in {sw.Elapsed} ({sw.Elapsed / rounds}/round)");

            var winner = new Schedule(new IntersectionSchedule[0], -1);

            foreach (var s in population)
            {
                var scored = Scorer.WithScore(input, s);

                if (winner.Score < scored.Score)
                {
                    winner = scored;
                }
            }

            return winner;
        }

        private static void GetNextPopulation(Input input, Schedule[] original, Schedule[] copy)
        {
            Array.Copy(original, copy, original.Length);
            Array.Copy(original, 0, copy, original.Length, original.Length);
            copy.Shuffle(original.Length);

            for (var i = 0; i < original.Length; i += 2)
            {
                var (a, b) = Cross(copy[i], copy[i + 1]);
                copy[i] = Scorer.WithScore(input, Mutate(a));
                copy[i + 1] = Scorer.WithScore(input, Mutate(b));
            }

            Array.Sort(copy, ScheduleComparer.Instance);

            // Top tier (50%)
            Array.Copy(copy, original, original.Length / 2);
            // Middle tier (25%)
            Array.Copy(copy, original.Length * 3 / 2, original, original.Length / 4, original.Length / 4);
            // Bottom tier (25%)
            Array.Copy(copy, original.Length * 7 / 8, original, original.Length * 3 / 4, original.Length / 4);
        }

        private static Schedule[] GetRandomPopulation(Schedule original)
        {
            var result = new Schedule[PopulationSize];

            result[0] = original;

            for (var i = 1; i < PopulationSize; ++i)
            {
                result[i] = Mutate(original);
            }

            return result;
        }

        private static Schedule Mutate(Schedule original, bool initial = false)
        {
            var originalS = original.Get;
            var result = new IntersectionSchedule[originalS.Length];

            Array.Copy(originalS, result, originalS.Length);

            var random = Random.Next(initial ? 2 : 3);
            var i = 0;

            switch(random)
            {
                case 0:
                    i = Random.Next(originalS.Length);
                    result[i] = MutateIncrease(originalS[i]);
                    break;
                case 1:
                    i = Random.Next(originalS.Length);
                    result[i] = MutateDecrease(originalS[i]);
                    break;
                case 2:
                    i = Random.Next(originalS.Length);
                    result[i] = MutateIncrease(originalS[i]);
                    break;
            }

            return new Schedule(result);
        }

        private static IntersectionSchedule MutateIncrease(IntersectionSchedule original)
        {
            var i = Random.Next(original.Intersection.From.Length);
            var newStreet = new StreetSchedule(original.Intersection.From[i], 1);

            for (var j = 0; j < original.Streets.Length; ++j)
            {
                var s = original.Streets[j];

                if (s.Street.Id == newStreet.Street.Id)
                {
                    var newSchedule = new StreetSchedule[original.Streets.Length];
                    Array.Copy(original.Streets, newSchedule, original.Streets.Length);
                    newSchedule[j] = new StreetSchedule(s.Street, s.Time > 5 ? s.Time : s.Time + 1);

                    return new IntersectionSchedule(original.Intersection, newSchedule);
                }
            }

            var newSchedule2 = new StreetSchedule[original.Streets.Length + 1];
            Array.Copy(original.Streets, newSchedule2, original.Streets.Length);
            newSchedule2[original.Streets.Length] = newStreet;

            return new IntersectionSchedule(original.Intersection, newSchedule2);
        }

        private static IntersectionSchedule MutateDecrease(IntersectionSchedule original)
        {
            if (original.Streets.Length == 0) return original;

            var i = Random.Next(original.Streets.Length);
            var toDecrease = original.Streets[i];

            if (toDecrease.Time == 1)
            {
                if (i == 0)
                {
                    var streets = new StreetSchedule[original.Streets.Length - 1];
                    Array.Copy(original.Streets, 1, streets, 0, original.Streets.Length - 1);

                    return new IntersectionSchedule(original.Intersection, streets);
                }
                else if (i == original.Streets.Length - 1)
                {
                    var streets = new StreetSchedule[original.Streets.Length - 1];
                    Array.Copy(original.Streets, streets, original.Streets.Length - 1);

                    return new IntersectionSchedule(original.Intersection, streets);
                }
                else
                {
                    var streets = new StreetSchedule[original.Streets.Length - 1];
                    Array.Copy(original.Streets, streets, i);
                    Array.Copy(original.Streets, i + 1, streets, i, original.Streets.Length - i - 1);

                    return new IntersectionSchedule(original.Intersection, streets);
                }
            }
            else
            {
                var streets = new StreetSchedule[original.Streets.Length];
                Array.Copy(original.Streets, streets, original.Streets.Length);
                Array.Copy(original.Streets, i + 1, streets, i, original.Streets.Length - i - 1);

                return new IntersectionSchedule(original.Intersection, streets);
            }
        }

        private static IntersectionSchedule MutateSwitch(IntersectionSchedule original)
        {
            if (original.Streets.Length <= 1) return original;

            int i = Random.Next(original.Streets.Length);
            int j = i;

            do
            {
                j = Random.Next(original.Streets.Length);
            } while(j != i);

            var newSchedule = new StreetSchedule[original.Streets.Length];
            Array.Copy(original.Streets, newSchedule, original.Streets.Length);
            var temp = newSchedule[i];
            newSchedule[j] = newSchedule[j];
            newSchedule[i] = temp;

            return new IntersectionSchedule(original.Intersection, newSchedule);
        }

        private static (Schedule, Schedule) Cross(Schedule a, Schedule b)
        {
            var min = Math.Min(a.Get.Length - 2, b.Get.Length - 2);
            if (min < 1) return (a, b);

            var random = Random.Next(min) + 1;

            var r1 = new IntersectionSchedule[b.Get.Length];
            var r2 = new IntersectionSchedule[a.Get.Length];

            // TODO: check this
            Array.Copy(b.Get, r1, random + 1);
            Array.Copy(a.Get, random + 1, r1, random + 1, a.Get.Length - random - 1);
            Array.Copy(a.Get, r2, random + 1);
            Array.Copy(b.Get, random + 1, r2, random + 1, b.Get.Length - random - 1);

            return (new Schedule(r1), new Schedule(r2));
        }
    }

    class ScheduleComparer : IComparer<Schedule>
    {
        public static readonly ScheduleComparer Instance = new ScheduleComparer();

        public int Compare(Schedule x, Schedule y) => y.Score.GetValueOrDefault() - x.Score.GetValueOrDefault();
    }

    static class ArrayExtensions
    {
        public static T[] Shuffle<T> (this T[] array, int until = -1)
        {
            int n = until == -1 ? array.Length : until;

            while (n > 1)
            {
                int k = Genetic.Random.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }

        public static T[] Concat<T>(this T[] a, params T[][] rest)
        {
            var length = a.Length;

            foreach (var r in rest) length += r.Length;

            var result = new T[length];
            var index = 0;

            Array.Copy(a, result, a.Length);
            index += a.Length;

            foreach (var act in rest)
            {
                Array.Copy(act, 0, result, index, act.Length);
                index += act.Length;
            }

            return result;
        }
    }
}
