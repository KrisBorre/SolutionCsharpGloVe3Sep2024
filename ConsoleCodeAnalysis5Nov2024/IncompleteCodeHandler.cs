using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCodeAnalysis5Nov2024
{
    public class IncompleteCodeHandler
    {
        private readonly ApiClientHandler _apiClientHandler;

        public IncompleteCodeHandler(ApiClientHandler apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
        }

        public async Task<string> HandleIncompleteCodeAsync(string context)
        {
            string instruction = "Complete the last C# code you wrote.";
            string secondResponse = await _apiClientHandler.SendPromptAsync(context, instruction);
            Console.WriteLine("Assistant: " + secondResponse);

            string newContext = ContextManager.UpdateContext(context, instruction, secondResponse);

            int tokenCount = _apiClientHandler.EstimateTokenCount(newContext);
            _apiClientHandler.CheckTokenLimit(tokenCount);

            return newContext;
        }
    }


}
