using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FlashCardAnswerService
{
    private readonly string _apiKey;
    private static readonly HttpClient _httpClient = new HttpClient(); 

    public FlashCardAnswerService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<bool> ValidateAnswerAsync(string userAnswer, string correctAnswer)
    {
        string prompt = $"Is the answer '{userAnswer}' correct for the question with the correct answer '{correctAnswer}'?";

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            max_tokens = 100,
            temperature = 0.0
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Content = content;
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error calling OpenAI API: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(responseBody);
        string responseMessage = result.choices[0].message.content.ToString().Trim();

        return responseMessage.Contains("yes", StringComparison.OrdinalIgnoreCase);
    }
}
