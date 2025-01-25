using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis8Nov2024
{
    public class ProjectPromptHandler8Nov2024
    {
        private readonly ApiClientHandler _apiClientHandler;
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;
        private readonly CodeCompiler _compiler;
        private readonly CodeExecutor _executor;

        public ProjectPromptHandler8Nov2024(ApiClientHandler apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
            _metadataManager = new MetadataReferenceManager8Nov2024();
            _compiler = new CodeCompiler(_metadataManager);
            _executor = new CodeExecutor();
        }

        public async Task HandlePromptsAsync(Project9Oct2024 project, string initialContext, int attempt = 1)
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

                ProcessAssistantResponse(project, conversation, ref context, assistantResponse, attempt);
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
            int tokenCount = _apiClientHandler.EstimateTokenCount(context);
            if (!_apiClientHandler.CheckTokenLimit(tokenCount))
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

        private void ProcessAssistantResponse(Project9Oct2024 project, Conversation conversation, ref string context, string assistantResponse, int attempt)
        {
            List<string> extractedCodeBlocks = CodeValidator.ExtractCSharpCode(assistantResponse);
    
            SourceCodeIteration iteration = CreateNewIteration(conversation);

            foreach (string codeBlock in extractedCodeBlocks)
            {
                ProcessCodeBlock(iteration, codeBlock);
            }

            DisplayIterationResults(project, conversation, iteration);
            HandleFailedCompilation(project, ref context, conversation, iteration);
        }

        public SourceCodeIteration CreateNewIteration(Conversation conversation)
        {
            var iteration = new SourceCodeIteration
            {
                Number = conversation.SourceCodeIterations.Count + 1
            };
            conversation.SourceCodeIterations.Add(iteration);
            return iteration;
        }

        private void ProcessCodeBlock(SourceCodeIteration iteration, string codeBlock)
        {
            var sourceCode = new SourceCode8Nov2024(codeBlock);
            iteration.SourceCodes.Add(sourceCode);

            string modifiedCode = RemoveConsoleReadKey(sourceCode.Code);
            CompilationResult compilationResult = _compiler.Compile(modifiedCode);

            iteration.IsCompiled = true;
            iteration.CompilationSuccess = compilationResult.Success;

            if (compilationResult.Success)
            {
                ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                iteration.ExecutionOutput = executionResult.Output;
            }
            else
            {
                iteration.DiagnosticResults = compilationResult.Diagnostics;
            }
        }

        public string RemoveConsoleReadKey(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();
            var rewriter = new ConsoleReadKeyRemovalRewriter();
            var modifiedRoot = rewriter.Visit(root);
            return modifiedRoot.ToFullString();
        }

        private void DisplayIterationResults(Project9Oct2024 project, Conversation conversation, SourceCodeIteration iteration)
        {
            Console.WriteLine($"Project: {project.Name}");
            Console.WriteLine($" Conversation #{conversation.Number}:");
            Console.WriteLine($"  Iteration #{iteration.Number}:");
            Console.WriteLine($"    Compilation success: {iteration.CompilationSuccess}");
            if (!iteration.CompilationSuccess)
                Console.WriteLine($"    Diagnostics: {iteration.DiagnosticResults}");
            if (iteration.CompilationSuccess)
                Console.WriteLine($"    Execution Output: {iteration.ExecutionOutput}");
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
            SourceCodeIteration newIteration = CreateNewIteration(conversation);

            foreach (string newCodeBlock in newCodeBlocks)
            {
                ProcessCodeBlock(newIteration, newCodeBlock);
            }

            DisplayIterationResults(project, conversation, newIteration);
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
