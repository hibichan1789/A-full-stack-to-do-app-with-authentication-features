using System.Text;
using System.Text.Json;
using TodoAuthApi.DTOs.SummaryDto;

namespace TodoAuthApi.Services.SummaryService
{
    public class SummaryService:ISummaryService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SummaryService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateSummaryAsync(string description)
        {
            if (String.IsNullOrWhiteSpace(description))
            {
                return description;
            }

            string baseUrl = _configuration["AzureFunctions:SummaryUrl"]!;
            Console.WriteLine(baseUrl);
            string functionKey = _configuration["AzureFunctions:FunctionKey"]!;
            // azure functionsをクラウド上で動かすならばcode=が必要になるから修正が今後必要になるかもしれない
            string url = $"{baseUrl}?Code={functionKey}";

            var summaryRequest = new SummaryRequest
            {
                Description = description
            };
            var summaryRequestJson = JsonSerializer.Serialize(summaryRequest);
            var content = new StringContent(summaryRequestJson, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                var summaryResponse = await response.Content.ReadFromJsonAsync<SummaryResponse>();

                return summaryResponse?.Summary ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SummaryService error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
