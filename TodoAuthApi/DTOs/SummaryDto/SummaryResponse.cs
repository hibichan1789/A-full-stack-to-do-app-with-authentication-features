using System.Text.Json.Serialization;

namespace TodoAuthApi.DTOs.SummaryDto
{
    public class SummaryResponse
    {
        [JsonPropertyName("summary")]
        public String Summary { get; set; } = String.Empty;
    }
}
