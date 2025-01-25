namespace LibraryCodeAnalysis23Nov2024
{
    public class TokenManager
    {
        private readonly int _contextWindow;

        public TokenManager(int contextWindow = 4096)
        {
            _contextWindow = contextWindow;
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
