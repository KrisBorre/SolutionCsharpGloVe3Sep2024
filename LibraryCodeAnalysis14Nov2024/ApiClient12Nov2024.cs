using System.Text;
using System.Text.Json;

namespace LibraryCodeAnalysis14Nov2024
{
    public class ApiClient12Nov2024
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public ApiClient12Nov2024(string baseUrl)
        {
            _baseUrl = baseUrl;
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

    }
}
