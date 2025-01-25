using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCodeAnalysis5Nov2024
{
    public class FixCodeHandler
    {
        private readonly ApiClientHandler _apiClientHandler;

        public FixCodeHandler(ApiClientHandler apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
        }

        public async Task<string> HandleFixCodeAsync(string context, SourceCodeIteration iteration)
        {
            // Construct the prompt with diagnostics and code
            string prompt = "We get the following compilation messages:" + Environment.NewLine +
                            $"{iteration.DiagnosticResults}" + Environment.NewLine + Environment.NewLine +
                            "```csharp" + Environment.NewLine +
                            $"{iteration.SourceCodes[0].Code}" + Environment.NewLine +
                            "```";

            // Send the prompt to the assistant
            string assistantResponse = await _apiClientHandler.SendPromptAsync(context, prompt);
            Console.WriteLine("Assistant: " + assistantResponse);

            // Update the context with the assistant's response
            string newContext = ContextManager.UpdateContext(context, prompt, assistantResponse);

            // Validate token limits for the updated context
            int tokenCount = _apiClientHandler.EstimateTokenCount(newContext);
            if (!_apiClientHandler.CheckTokenLimit(tokenCount))
            {
                Console.WriteLine("Warning: Token limit exceeded. Consider condensing the context.");
            }

            return newContext;
        }
    }

}
