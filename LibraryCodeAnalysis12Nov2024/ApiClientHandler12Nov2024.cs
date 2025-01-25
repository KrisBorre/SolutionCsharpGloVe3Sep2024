namespace LibraryCodeAnalysis12Nov2024
{
    public class ApiClientHandler12Nov2024
    {
        private readonly ApiClient12Nov2024 _apiClient;

        public ApiClientHandler12Nov2024(ApiClient12Nov2024 apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> SendPromptAsync(string context, string prompt)
        {
            return await _apiClient.PostCompletionAsync(context + $"\nUser: {prompt}\nAssistant:");
        }
      
    }

}
