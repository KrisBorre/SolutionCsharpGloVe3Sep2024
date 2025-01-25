namespace LibraryCodeAnalysis1Dec2024
{
    public class ProjectPromptHandler1Dec2024
    {
        private readonly ApiClientHandler12Nov2024 _apiClientHandler;

        private readonly CodeProcessing1Dec2024 _codeProcessing;



        public ProjectPromptHandler1Dec2024(ApiClientHandler12Nov2024 apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
            _codeProcessing = new CodeProcessing1Dec2024();
        }

        public async Task HandlePromptsAsync(Project9Oct2024 project, string initialContext)
        {
            string context = initialContext;

            Conversation conversation = project.Conversations.Last();

            foreach (var prompt in project.Prompts)
            {
                Console.WriteLine($"User: {prompt}");
                string assistantResponse = await GetAssistantResponseAsync(context, prompt);
                context = ContextManager1Dec2024.UpdateContext(context, prompt, assistantResponse);


                TokenManager tokenManager = new TokenManager();
                int tokenCount = tokenManager.EstimateTokenCount(context);
                if (!tokenManager.CheckTokenLimit(tokenCount))
                {
                    context = ContextManager1Dec2024.CondenseContext(project, initialContext);
                }


                if (!await ValidateCodeBlocksAsync(context, assistantResponse))
                {
                    Console.WriteLine("I'm terminating this project.");
                    break;  // terminate this project immediately
                }


                context = await ProcessAssistantResponseAsync(project, conversation, context, assistantResponse);
            }

            context = await HandleMainCompilationAsync(project, context, conversation);
        }


        private async Task<string> GetAssistantResponseAsync(string context, string prompt)
        {
            string response = await _apiClientHandler.SendPromptAsync(context, prompt);
            Console.WriteLine($"Assistant: {response}");
            return response;
        }



        private async Task<bool> ValidateCodeBlocksAsync(string context, string assistantResponse)
        {
            if (CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse)) return true;

            Console.WriteLine("The code has improper `csharp` block closures.");
            string instruction = "Complete the last C# code you wrote.";
            assistantResponse = await _apiClientHandler.SendPromptAsync(context, instruction);
            Console.WriteLine($"Assistant: {assistantResponse}");

            context = ContextManager1Dec2024.UpdateContext(context, instruction, assistantResponse);
            return CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse);
        }


        private async Task<string> ProcessAssistantResponseAsync(Project9Oct2024 project, Conversation conversation, string context, string assistantResponse)
        {
            List<string> extractedCodeBlocks = CodeValidator.ExtractCSharpCode(assistantResponse);

            ProjectManager26Nov2024 projectManager = new ProjectManager26Nov2024();
            SourceCodeIteration26Nov2024 iteration = projectManager.CreateNewIteration(conversation);

            foreach (string codeBlock in extractedCodeBlocks)
            {
                var sourceCode = new SourceCode30Nov2024(codeBlock);
                iteration.SourceCodes.Add(sourceCode);
            }

            KnowledgeBaseCreator1Dec2024 knowledgeBaseCreator = new KnowledgeBaseCreator1Dec2024(_codeProcessing);
            knowledgeBaseCreator.Iterations = conversation.SourceCodeIterations;
            KnowledgeBase30Nov2024 knowledgeBase = knowledgeBaseCreator.CreateKnowledgeBase();
            iteration.KnowledgeBase = knowledgeBase;

            _codeProcessing.ProcessCodeBlocks(iteration, knowledgeBase.CodeBlocks.ToArray());

            DisplayManagement1Dec2024 displayManagement = new DisplayManagement1Dec2024();
            displayManagement.DisplayIterationResults(project, conversation, iteration);

            if (!iteration.CompilationSuccess)
            {
                context = await HandleFailedCompilationAsync(project, context, conversation, iteration);
            }
            return context;
        }


        private async Task<string> HandleFailedCompilationAsync(Project9Oct2024 project, string context, Conversation conversation, SourceCodeIteration26Nov2024 iteration, int i = 0)
        {
            string promptForFix = ConstructFixPrompt(iteration);
            Console.WriteLine(promptForFix);

            string assistantFixResponse = await _apiClientHandler.SendPromptAsync(context, promptForFix);
            Console.WriteLine($"Assistant: {assistantFixResponse}");

            context = ContextManager1Dec2024.UpdateContext(context, promptForFix, assistantFixResponse);

            if (!await ValidateCodeBlocksAsync(context, assistantFixResponse)) return context;

            List<string> newCodeBlocks = CodeValidator.ExtractCSharpCode(assistantFixResponse);

            ProjectManager26Nov2024 projectManager = new ProjectManager26Nov2024();
            SourceCodeIteration26Nov2024 newIteration = projectManager.CreateNewIteration(conversation);

            foreach (string newCodeBlock in newCodeBlocks)
            {
                var sourceCode = new SourceCode30Nov2024(newCodeBlock);
                newIteration.SourceCodes.Add(sourceCode);
            }

            KnowledgeBaseCreator1Dec2024 knowledgeBaseCreator = new KnowledgeBaseCreator1Dec2024(_codeProcessing);
            knowledgeBaseCreator.Iterations = conversation.SourceCodeIterations;
            KnowledgeBase30Nov2024 knowledgeBase = knowledgeBaseCreator.CreateKnowledgeBase();
            newIteration.KnowledgeBase = knowledgeBase;

            _codeProcessing.ProcessCodeBlocks(newIteration, knowledgeBase.CodeBlocks.ToArray());

            DisplayManagement1Dec2024 displayManagement = new DisplayManagement1Dec2024();
            displayManagement.DisplayIterationResults(project, conversation, newIteration);

            i++;
            if (!newIteration.CompilationSuccess && i <= 2)
            {
                context = await HandleFailedCompilationAsync(project, context, conversation, newIteration, i);
            }
            return context;
        }

        private string ConstructFixPrompt(SourceCodeIteration26Nov2024 iteration)
        {
            string prompt = $"We get the following compilation messages:{Environment.NewLine}" +
                   $"{iteration.DiagnosticResults}{Environment.NewLine}{Environment.NewLine}";

            foreach (string codeBlock in iteration.KnowledgeBase.CodeBlocks)
            {
                prompt += "```csharp" + Environment.NewLine +
                   $"{codeBlock}" + Environment.NewLine +
                   "```" + Environment.NewLine;
            }

            return prompt;
        }


        private async Task<string> HandleMainCompilationAsync(Project9Oct2024 project, string context, Conversation conversation)
        {
            string promptForFix = ConstructMainPrompt(conversation);
            Console.WriteLine(promptForFix);

            string assistantFixResponse = await _apiClientHandler.SendPromptAsync(context, promptForFix);
            Console.WriteLine($"Assistant: {assistantFixResponse}");

            context = ContextManager1Dec2024.UpdateContext(context, promptForFix, assistantFixResponse);

            if (!await ValidateCodeBlocksAsync(context, assistantFixResponse)) return context;

            List<string> newCodeBlocks = CodeValidator.ExtractCSharpCode(assistantFixResponse);

            ProjectManager26Nov2024 projectManager = new ProjectManager26Nov2024();
            SourceCodeIteration26Nov2024 newIteration = projectManager.CreateNewIteration(conversation);

            foreach (string newCodeBlock in newCodeBlocks)
            {
                var sourceCode = new SourceCode30Nov2024(newCodeBlock);
                newIteration.SourceCodes.Add(sourceCode);
            }

            KnowledgeBaseCreator1Dec2024 knowledgeBaseCreator = new KnowledgeBaseCreator1Dec2024(_codeProcessing);
            knowledgeBaseCreator.Iterations = conversation.SourceCodeIterations;
            KnowledgeBase30Nov2024 knowledgeBase = knowledgeBaseCreator.CreateKnowledgeBase();
            newIteration.KnowledgeBase = knowledgeBase;

            _codeProcessing.ProcessCodeBlocks(newIteration, knowledgeBase.CodeBlocks.ToArray());

            DisplayManagement1Dec2024 displayManagement = new DisplayManagement1Dec2024();
            displayManagement.DisplayIterationResults(project, conversation, newIteration);

            if (!newIteration.CompilationSuccess)
            {
                context = await HandleFailedCompilationAsync(project, context, conversation, newIteration);
            }
            return context;
        }

        private string ConstructMainPrompt(Conversation conversation)
        {
            string prompt = $"Merge the following Main methods into one Main method:{Environment.NewLine}";

            foreach (SourceCodeIteration26Nov2024 sourceCodeIteration in conversation.SourceCodeIterations)
            {
                foreach (SourceCode30Nov2024 sourceCode in sourceCodeIteration.SourceCodes)
                {
                    if (!sourceCode.HasNoMain)
                    {
                        prompt += "```csharp" + Environment.NewLine +
                           $"{sourceCode.Code}" + Environment.NewLine +
                           "```" + Environment.NewLine;
                    }
                }
            }

            return prompt;
        }

    }
}
