using System.Text;
using System.Text.Json;

namespace ConsoleCodeAnalysis4Nov2024
{
    public class ApiClient
    {
        private readonly string _baseUrl;
        private readonly int _contextWindow;
        private readonly HttpClient _httpClient;

        public ApiClient(string baseUrl, int contextWindow)
        {
            _baseUrl = baseUrl;
            _contextWindow = contextWindow;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };
        }

        public async Task<bool> IsServerHealthyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/health");
                if (!response.IsSuccessStatusCode)
                    return false;

                var content = await response.Content.ReadAsStringAsync();
                var health = JsonSerializer.Deserialize<JsonElement>(content);
                return health.GetProperty("status").GetString() == "ok";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking server health: {ex.Message}");
                return false;
            }
        }

        public async Task<string> PostCompletionAsync(string prompt)
        {
            var payload = new
            {
                prompt,
                temperature = 0.8,
                stop = new[] { "</s>", "Assistant:", "User:" }
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/completion", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error processing request.");
                    return string.Empty;
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
                return jsonResponse.GetProperty("content").GetString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return string.Empty;
            }
        }

        public int EstimateTokenCount(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            char[] delimiters = { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '-', '_', '(', ')', '[', ']', '{', '}', '"', '\'' };
            var tokens = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return tokens.Length;
        }

        public bool CheckTokenLimit(int tokenCount)
        {
            bool result = true;
            Console.WriteLine($"Estimated Token Count: {tokenCount}");
            if (tokenCount <= _contextWindow * 0.7)
            {
                Console.WriteLine("Your context is within 70 percent of the model's token limit.");
            }
            else
            {
                Console.WriteLine($"Warning: Your context exceeds 70 percent of the model's limit of {_contextWindow} tokens.");
                result = false;
            }
            return result;
        }
    }
}
