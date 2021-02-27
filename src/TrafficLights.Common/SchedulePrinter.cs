namespace TrafficLights.Common
{
    using System.Text;

    public static class SchedulePrinter
    {
        public static string Print(Schedule schedule)
        {
            var sb = new StringBuilder();
            var s = schedule.Get;

            sb.AppendLine("" + s.Length);

            foreach (var isc in s)
            {
                sb.AppendLine("" + isc.Intersection.Id);
                sb.AppendLine("" + isc.Streets.Length);

                foreach (var str in isc.Streets)
                {
                    sb.Append(str.Street.Name);
                    sb.Append(" ");
                    sb.AppendLine("" + str.Time);
                }
            }

            return sb.ToString();
        }
    }
}
