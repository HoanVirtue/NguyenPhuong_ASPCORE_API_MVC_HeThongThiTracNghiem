using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace MultipleChoiceTest.Repository.Service
{
    public interface IGPTService
    {
        Task<bool> GradeEssay(string question, string correctAnswer, string answerText);
    }
    public class GPTService : IGPTService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public GPTService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> GradeEssay(string question, string correctAnswer, string answerText)
        {
            var httpClient = _httpClientFactory.CreateClient("ChtpGPT");

            var prompt = $@"
            Câu hỏi: {question}
            Đáp án chính xác: {correctAnswer}
            Câu trả lời: {answerText}
            
            Kiểm tra câu trả lời và trả lời 'true' hoặc 'false'. Nếu câu trả lời của học sinh khớp với đáp án chính xác, trả lời 'true', nếu không trả lời 'false'.";

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role = "user", content = prompt}
                },
                max_tokens = 50,
                n = 1,
                temperature = 0
            };
            var requestJson = JsonSerializer.Serialize(request);
            var httpReq = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/chat/completions"),
                Headers = { { "Authorization", $"Bearer {_configuration["OpenAI:ApiKey"]}" } },
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(httpReq);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var chatResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);

            return bool.Parse(chatResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim());
        }
    }


    public class ChatCompletionResponse
    {
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
