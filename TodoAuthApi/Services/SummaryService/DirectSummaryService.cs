using OpenAI;
using OpenAI.Chat;

namespace TodoAuthApi.Services.SummaryService
{
    public class DirectSummaryService:ISummaryService
    {
        private readonly ChatClient _chatClient;
        private readonly string _model = "gpt-4o-mini";

        public DirectSummaryService(IConfiguration configuration)
        {
            var _apiKey = configuration["OpenAI:ApiKey"]!;
            var client = new OpenAIClient(_apiKey);

            _chatClient = client.GetChatClient(_model);
        }

        public async Task<string> GenerateSummaryAsync(string description)
        {
            Console.WriteLine("Direct Summary is Triggered");
            if (String.IsNullOrWhiteSpace(description))
            {
                return string.Empty;
            }

            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage(
                    "以下の文章から重要なキーワードを3〜5個抽出したリストを作成してください\n\n"+
                    "出力例:\n"+
                    "キーワード: A, B, C"
                    ),
                new UserChatMessage(description)
            };
            try
            {
                var response = await _chatClient.CompleteChatAsync(
                    messages,
                    new ChatCompletionOptions
                    {
                        MaxOutputTokenCount = 120,
                        Temperature = (float)0.30
                    }
                    );

                return response.Value.Content[0].Text.Trim();
            }
            catch(Exception e)
            {
                Console.WriteLine($"OpenAI direct summary error: {e.Message}");
                return string.Empty;
            }
        }
    }
}
