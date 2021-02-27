namespace TrafficLights.DotFormatter
{
    using System.Text;
    using TrafficLights.Common;

    public static class InputToDot
    {
        public static string Format(Input input)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"digraph g {
                graph [overlap=scale, nslimit=5, nslimit1=5, splines=line];");

            foreach (var street in input.Streets)
            {
                sb.AppendLine($"  {street.Start} -> {street.End} [label=\"{street.Name}: {street.Time}\", minlen={street.Time}];");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
