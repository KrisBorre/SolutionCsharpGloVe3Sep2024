namespace ConsoleCodeAnalysis8Oct2024
{
    public class InteractionHandler
    {
        private readonly ApiClient _apiClient;

        public InteractionHandler(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> HandleInteractionsAsync(string context, string[] prompts)
        {
            foreach (var prompt in prompts)
            {
                Console.WriteLine($"User: {prompt}");

                string assistantResponse = await _apiClient.PostCompletionAsync(context + $"\nUser: {prompt}\nAssistant:");
                Console.WriteLine($"Assistant: {assistantResponse}");

                context = UpdateContext(context: context, userInput: prompt, assistantResponse: assistantResponse);

                int tokenCount = _apiClient.EstimateTokenCount(context);
                if (!_apiClient.CheckTokenLimit(tokenCount))
                {
                    context = await CondenseContextAsync(context);
                }
                else
                {
                    bool isValid = IsCsharpBlockProperlyClosed(assistantResponse);

                    if (isValid)
                    {
                        Console.WriteLine("The code has properly closed `csharp` blocks.");
                    }
                    else
                    {
                        Console.WriteLine("The code has improper `csharp` block closures.");
                        // If code block is improper, ask the assistant to complete the code
                        context = await HandleIncompleteCode(context);
                    }

                    context = await ValidateAssistantResponseAsync(assistantResponse: assistantResponse, context: context);
                }

            }
            return context;
        }

        private async Task<string> ValidateAssistantResponseAsync(string assistantResponse, string context)
        {
            const int minimumResponseLength = 140;

            if (assistantResponse.Length < minimumResponseLength)
            {
                Console.WriteLine($"Assistant response is shorter than {minimumResponseLength} characters. Verifying completeness...");

                // Prompt to ask the LLM if the response is complete
                string verificationPrompt = "Is your last response complete? If not, please complete it.";
                string verificationResponse = await _apiClient.PostCompletionAsync(context + $"\nUser: {verificationPrompt}\nAssistant:");

                Console.WriteLine("Assistant verification response: " + verificationResponse);

                string newContext = UpdateContext(context, verificationPrompt, verificationResponse);

                return newContext;
            }

            Console.WriteLine("Assistant response is sufficiently long.");
            return context;
        }

        private async Task<string> HandleIncompleteCode(string context)
        {
            string instruction = "Complete the last C# code you wrote.";
            string secondResponse = await _apiClient.PostCompletionAsync(context + $"\nUser: {instruction}\nAssistant:");
            Console.WriteLine("Assistant: " + secondResponse);

            string newContext = UpdateContext(context, instruction, secondResponse);

            int tokenCount = _apiClient.EstimateTokenCount(newContext);
            _apiClient.CheckTokenLimit(tokenCount);

            return newContext;
        }

        private bool IsCsharpBlockProperlyClosed(string input)
        {
            string[] startTags = { "```csharp", "```C#" };
            const string endTag = "```";

            int currentIndex = 0;

            while (true)
            {
                // Find the earliest occurrence of any valid start tag
                int startIndex = startTags
                    .Select(tag => input.IndexOf(tag, currentIndex, StringComparison.OrdinalIgnoreCase))
                    .Where(index => index != -1)
                    .DefaultIfEmpty(-1)
                    .Min();

                if (startIndex == -1) // No more start tags found
                    break;

                int endIndex = input.IndexOf(endTag, startIndex + 1);

                if (endIndex == -1) // No closing tag found
                    return false;

                currentIndex = endIndex + endTag.Length; // Move past the closing tag
            }

            return true; // All start tags are properly closed
        }


        public async Task<string> ResolveTodosAsync(string context)
        {
            if (context.Contains("TODO:"))
            {
                string prompt = "Are there unresolved TODOs?";
                Console.WriteLine(prompt);
                string response = await _apiClient.PostCompletionAsync(context + $"\nUser: {prompt}\nAssistant:");
                Console.WriteLine($"Assistant: {response}");

                string newContext = UpdateContext(context: context, userInput: prompt, assistantResponse: response);
                return newContext;
            }

            return context;
        }

        private string UpdateContext(string context, string userInput, string assistantResponse)
        {
            return $"{context}\nUser: {userInput}\nAssistant: {assistantResponse}";
        }

        private async Task<string> CondenseContextAsync(string context, int contextWindow = 4096)
        {
            string condensePrompt = @$"You are an expert C# coding assistant. The conversation has exceeded the context window, and we need to condense the history while retaining essential information for continued programming support.

### Instructions:
Analyze the conversation history provided below:

Extract unresolved programming tasks or questions.
Retain key C# code snippets and their context, including a concise description of existing classes, their responsibilities, and key methods.
Highlight discussions about design decisions, errors, or solutions relevant to ongoing tasks.
Preserve existing client code and unit tests in the condensed history for continued use.
Indicate when reimplementation of existing classes is necessary, such as when modifications are explicitly required.
Filter out redundant or resolved discussions.
Guide future programming efforts:

Ensure new client code uses existing classes and methods whenever possible.
Avoid reimplementing existing classes unless modifications are explicitly required.
Provide concise descriptions of existing C# classes, their responsibilities, and their key methods for clarity.
Follow these best practices for new programming tasks:

Self-explanatory code: Ensure variable and method names are descriptive, making the code understandable without comments.
TODO annotations: Include // TODO markers for incomplete sections.
Object-oriented principles: Adhere to object-oriented programming best practices, such as encapsulation, modularity, and appropriate use of inheritance and interfaces.
Unit testing: Provide comprehensive xUnit tests for all classes and methods.
Code focus: Focus solely on generating code without explanatory text unless explicitly requested.

### Input:
Full conversation history: {context}

### Output:
Provide a condensed context that:

Includes critical programming details.
Describes existing classes, their responsibilities, and key methods concisely.
Retains unresolved tasks, ongoing discussions, existing client code, and unit tests.
Excludes unnecessary or resolved exchanges.
Uses a clear structure: Existing Classes and Responsibilities, Unresolved Tasks, Key Design Decisions, Relevant Code Snippets, Unit Tests, and Next Steps.
Fits within the model's {contextWindow} context window for efficient query handling.
";

            string prompt = $"User: {condensePrompt}\nAssistant:";
            string condensedContext = await _apiClient.PostCompletionAsync(prompt: prompt);

            Console.WriteLine($"total token estimate: {_apiClient.EstimateTokenCount(prompt + condensedContext)}");

            Console.WriteLine($"Condensed Context: {condensedContext}");
            return condensedContext;
        }
    }
}
