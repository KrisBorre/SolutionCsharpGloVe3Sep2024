namespace ConsoleCodeAnalysis6Nov2024
{
    public class ApiClientHandler
    {
        private readonly ApiClient _apiClient;

        public ApiClientHandler(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> SendPromptAsync(string context, string prompt)
        {
            return await _apiClient.PostCompletionAsync(context + $"\nUser: {prompt}\nAssistant:");
        }

        public int EstimateTokenCount(string context)
        {
            return _apiClient.EstimateTokenCount(context);
        }

        public bool CheckTokenLimit(int tokenCount)
        {
            return _apiClient.CheckTokenLimit(tokenCount);
        }
    }

}
