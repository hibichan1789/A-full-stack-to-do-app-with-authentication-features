using System.Diagnostics;

namespace TodoAuthApi.Services.SummaryService
{
    public class MetricResult
    {
        public double AverageMs { get; set; }
        public double MinMs { get; set; }
        public double MaxMs { get; set; }
        public List<double> AllResults { get; set; } = new List<double>();
    }
    public class MetricService
    {
        public async Task<MetricResult> MeasureSummaryAsync<T>(T service, string description, int count = 10)
            where T: ISummaryService
        {
            var result = new MetricResult();

            for(int i = 0; i< count; i++)
            {
                Console.WriteLine($"{i}回目の計測");
                var sw = Stopwatch.StartNew();
                await service.GenerateSummaryAsync(description);
                sw.Stop();

                result.AllResults.Add(sw.Elapsed.TotalMilliseconds);
            }

            result.AverageMs = result.AllResults.Average();
            result.MinMs = result.AllResults.Min();
            result.MaxMs = result.AllResults.Max();

            return result;
        }

    }
}
