namespace LibraryCodeAnalysis12Nov2024
{
    public class ProjectPromptHandler12Nov2024
    {
        private readonly ApiClientHandler12Nov2024 _apiClientHandler;

        private readonly CodeProcessing _codeProcessing;

        public ProjectPromptHandler12Nov2024(ApiClientHandler12Nov2024 apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
            _codeProcessing = new CodeProcessing();
        }

        //public async Task HandlePromptsAsync(Project9Oct2024 project, string initialContext, int attempt = 1)
        public async Task HandlePromptsAsync(Project9Oct2024 project, string initialContext)
        {
            string context = initialContext;

            foreach (var prompt in project.Prompts)
            {
                Console.WriteLine($"User: {prompt}");
                string assistantResponse = await GetAssistantResponseAsync(context, prompt);
                context = ContextManager.UpdateContext(context, prompt, assistantResponse);

                if (!EnsureContextWithinTokenLimit(ref context))
                {
                    Console.WriteLine("Warning: Context condensed due to token limit.");
                }

                if (!ValidateCodeBlocks(ref context, assistantResponse))
                {
                    Console.WriteLine("I'm terminating this project.");
                    break;  // terminate this project immediately
                }

                Conversation conversation = project.Conversations.Last();

                //ProcessAssistantResponse(project, conversation, ref context, assistantResponse, attempt);
                ProcessAssistantResponse(project, conversation, ref context, assistantResponse);
            }
        }

        private async Task<string> GetAssistantResponseAsync(string context, string prompt)
        {
            string response = await _apiClientHandler.SendPromptAsync(context, prompt);
            Console.WriteLine($"Assistant: {response}");
            return response;
        }

        private bool EnsureContextWithinTokenLimit(ref string context)
        {
            TokenManager tokenManager = new TokenManager();
            int tokenCount = tokenManager.EstimateTokenCount(context);
            if (!tokenManager.CheckTokenLimit(tokenCount))
            {
                context = ContextManager.CondenseContextAsync(context, _apiClientHandler).Result;
                return false;
            }
            return true;
        }

        private bool ValidateCodeBlocks(ref string context, string assistantResponse)
        {
            if (CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse)) return true;

            Console.WriteLine("The code has improper `csharp` block closures.");
            string instruction = "Complete the last C# code you wrote.";
            assistantResponse = _apiClientHandler.SendPromptAsync(context, instruction).Result;
            Console.WriteLine($"Assistant: {assistantResponse}");

            context = ContextManager.UpdateContext(context, instruction, assistantResponse);
            return CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse);
        }

        //private void ProcessAssistantResponse(Project9Oct2024 project, Conversation conversation, ref string context, string assistantResponse, int attempt)
        private void ProcessAssistantResponse(Project9Oct2024 project, Conversation conversation, ref string context, string assistantResponse)
        {
            List<string> extractedCodeBlocks = CodeValidator.ExtractCSharpCode(assistantResponse);

            ProjectManager projectManager = new ProjectManager();
            SourceCodeIteration iteration = projectManager.CreateNewIteration(conversation);

            foreach (string codeBlock in extractedCodeBlocks)
            {
                _codeProcessing.ProcessCodeBlock(iteration, codeBlock);
            }

            DisplayManagement displayManagement = new DisplayManagement();
            displayManagement.DisplayIterationResults(project, conversation, iteration);
            HandleFailedCompilation(project, ref context, conversation, iteration);
        }


        private void HandleFailedCompilation(Project9Oct2024 project, ref string context, Conversation conversation, SourceCodeIteration iteration)
        {
            if (iteration.CompilationSuccess || iteration.SourceCodes.Count != 1) return;

            string promptForFix = ConstructFixPrompt(iteration);
            Console.WriteLine(promptForFix);

            string assistantFixResponse = _apiClientHandler.SendPromptAsync(context, promptForFix).Result;
            Console.WriteLine($"Assistant: {assistantFixResponse}");

            context = ContextManager.UpdateContext(context, promptForFix, assistantFixResponse);

            if (!ValidateCodeBlocks(ref context, assistantFixResponse)) return;

            List<string> newCodeBlocks = CodeValidator.ExtractCSharpCode(assistantFixResponse);

            ProjectManager projectManager = new ProjectManager();
            SourceCodeIteration newIteration = projectManager.CreateNewIteration(conversation);

            foreach (string newCodeBlock in newCodeBlocks)
            {
                _codeProcessing.ProcessCodeBlock(newIteration, newCodeBlock);
            }

            DisplayManagement displayManagement = new DisplayManagement();
            displayManagement.DisplayIterationResults(project, conversation, newIteration);
        }

        private string ConstructFixPrompt(SourceCodeIteration iteration)
        {
            return $"We get the following compilation messages:{Environment.NewLine}" +
                   $"{iteration.DiagnosticResults}{Environment.NewLine}{Environment.NewLine}" +
                   "```csharp" + Environment.NewLine +
                   $"{iteration.SourceCodes[0].Code}" + Environment.NewLine +
                   "```";
        }
    }
}
