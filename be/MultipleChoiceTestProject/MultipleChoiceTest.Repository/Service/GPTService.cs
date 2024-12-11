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

            //var prompt = $@"Câu hỏi: {question}
            //    Đáp án: {correctAnswer}
            //    Trả lời: {answerText}
            //    Đúng? ('true'/'false')";
            var prompt = "điện tử là gì";

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role = "user", content = prompt}
                },
                max_tokens = 30,
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

            try
            {
                var response = await httpClient.SendAsync(httpReq);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }


            //return bool.Parse(chatResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim());
            //int maxRetries = 5;
            //int retryCount = 0;
            //int delay = 1000;
            //while (retryCount < maxRetries)
            //{
            //    try
            //    {
            //        var response = await httpClient.SendAsync(httpReq);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var responseString = await response.Content.ReadAsStringAsync();
            //            var chatResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);

            //            return bool.Parse(chatResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim());
            //        }
            //        else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            //        {
            //            var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromMicroseconds(delay);
            //            await Task.Delay(retryAfter);
            //            retryCount++;
            //            delay *= 2;
            //        }
            //        else
            //        {
            //            response.EnsureSuccessStatusCode();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //        return false;
            //    }
            //}

            return false;
        }

        //public async Task<string> GradeEssay(string question, string correctAnswer, string answerText)
        //{
        //    var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
        //    var baseUrl = _configuration.GetValue<string>("OpenAI:BaseUrl");

        //    var prompt = $@"
        //        Câu hỏi: {question}
        //        Đáp án chính xác: {correctAnswer}
        //        Câu trả lời: {answerText}

        //        Kiểm tra câu trả lời và trả lời 'true' hoặc 'false'. Nếu câu trả lời của học sinh khớp với đáp án chính xác, trả lời 'true', nếu không trả lời 'false'.";


        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        //    var request = new OpenAIRequestDto
        //    {
        //        Model = "gpt-3.5-turbo",
        //        Messages = new List<OpenAIMessageRequestDto>{
        //            new OpenAIMessageRequestDto
        //            {
        //                Role = "user",
        //                Content = prompt
        //            }
        //        },
        //        MaxTokens = 10
        //    };
        //    var json = JsonSerializer.Serialize(request);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await client.PostAsync(baseUrl, content);
        //    var resjson = await response.Content.ReadAsStringAsync();
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
        //        throw new System.Exception(errorResponse.Error.Message);
        //    }
        //    var data = JsonSerializer.Deserialize<OpenAIResponseDto>(resjson);
        //    var responseText = data.choices[0].message.content;

        //    return responseText;
        //}
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





    //
    //public class OpenAIErrorResponseDto
    //{
    //    [JsonPropertyName("error")]
    //    public OpenAIError Error { get; set; }
    //}
    //public class OpenAIError
    //{
    //    [JsonPropertyName("message")]
    //    public string Message { get; set; }

    //    [JsonPropertyName("type")]
    //    public string Type { get; set; }

    //    [JsonPropertyName("param")]
    //    public string Param { get; set; }

    //    [JsonPropertyName("code")]
    //    public string Code { get; set; }
    //}

    //public class OpenAIRequestDto
    //{
    //    [JsonPropertyName("model")]
    //    public string Model { get; set; }

    //    [JsonPropertyName("messages")]
    //    public List<OpenAIMessageRequestDto> Messages { get; set; }

    //    [JsonPropertyName("temperature")]
    //    public float Temperature { get; set; }

    //    [JsonPropertyName("max_tokens")]
    //    public int MaxTokens { get; set; }
    //}

    //public class OpenAIMessageRequestDto
    //{
    //    [JsonPropertyName("role")]
    //    public string Role { get; set; }

    //    [JsonPropertyName("content")]
    //    public string Content { get; set; }
    //}

    //public class OpenAIResponseDto
    //{
    //    public string id { get; set; }
    //    public string @object { get; set; }
    //    public int created { get; set; }
    //    public string model { get; set; }
    //    public List<Choice> choices { get; set; }
    //    public Usage usage { get; set; }
    //}

    //public class Choice
    //{
    //    public int index { get; set; }
    //    public Message message { get; set; }
    //    public object logprobs { get; set; }
    //    public string finish_reason { get; set; }
    //}
    //public class Usage
    //{
    //    public int prompt_tokens { get; set; }
    //    public int completion_tokens { get; set; }
    //    public int total_tokens { get; set; }
    //}
    //public class OpenAIChoice
    //{
    //    public string text { get; set; }
    //    public float probability { get; set; }
    //    public float[] logprobs { get; set; }
    //    public int[] finish_reason { get; set; }
    //}

    //public class Message
    //{
    //    public string role { get; set; }
    //    public string content { get; set; }
    //}
}
