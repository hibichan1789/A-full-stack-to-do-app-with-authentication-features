namespace TodoAuthApi.Services.SummaryService
{
    public interface ISummaryService
    {
        Task<string> GenerateSummaryAsync(string description);
    }
}
