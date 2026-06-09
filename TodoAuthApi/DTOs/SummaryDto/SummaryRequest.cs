using System.Text.Json.Serialization;

namespace TodoAuthApi.DTOs.SummaryDto
{
    public class SummaryRequest
    {
        [JsonPropertyName("description")]
        public String Description { get; set; } = String.Empty;
    }
}
