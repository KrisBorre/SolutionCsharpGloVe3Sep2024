using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis6Nov2024
{
    public class ProjectPromptHandler
    {
        private readonly ApiClientHandler _apiClientHandler;

        public ProjectPromptHandler(ApiClientHandler apiClientHandler)
        {
            _apiClientHandler = apiClientHandler;
        }

        public async Task HandlePromptsAsync(Project9Oct2024 project, string initialContext, int attempt = 1)
        {
            string context = initialContext;

            var metadataManager = new MetadataReferenceManager();
            var compiler = new CodeCompiler(metadataManager);

            var executor = new CodeExecutor();

            foreach (var prompt in project.Prompts)
            {
                Console.WriteLine($"User: {prompt}");
                string assistantResponse = await _apiClientHandler.SendPromptAsync(context, prompt);
                Console.WriteLine($"Assistant: {assistantResponse}");

                context = ContextManager.UpdateContext(context, prompt, assistantResponse);

                if (!_apiClientHandler.CheckTokenLimit(_apiClientHandler.EstimateTokenCount(context)))
                {
                    context = await ContextManager.CondenseContextAsync(context, _apiClientHandler);
                }

                // Validate if C# code blocks are properly closed
                if (!CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse))
                {
                    Console.WriteLine("The code has improper `csharp` block closures.");

                    string instruction = "Complete the last C# code you wrote.";
                    assistantResponse = await _apiClientHandler.SendPromptAsync(context, instruction);
                    Console.WriteLine("Assistant: " + assistantResponse);

                    context = ContextManager.UpdateContext(context, instruction, assistantResponse);

                    if (!CodeValidator.IsCsharpBlockProperlyClosed(assistantResponse))
                    {
                        Console.WriteLine("I'm terminating this project.");
                        break; // terminate this project immediately
                    }
                }

                Console.WriteLine("The code has properly closed `csharp` blocks.");

                // Extract C# code blocks from the assistant's response
                List<string> extractedCodeBlocks = CodeValidator.ExtractCSharpCode(assistantResponse);

                // Create a new conversation and source code iteration
                Conversation conversation = new Conversation
                {
                    Number = attempt
                };
                project.Conversations.Add(conversation);

                SourceCodeIteration iteration = new SourceCodeIteration
                {
                    Number = conversation.SourceCodeIterations.Count + 1
                };
                conversation.SourceCodeIterations.Add(iteration);

                // Only one code block is actually supported.
                // Process each extracted code block
                foreach (string codeBlock in extractedCodeBlocks)
                {
                    // Create a SourceCode instance and add it to the iteration
                    SourceCode1Nov2024 sourceCode = new SourceCode1Nov2024(codeBlock);
                    iteration.SourceCodes.Add(sourceCode);


                    // Remove Console.ReadKey() calls from the user code
                    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode.Code);
                    var root = syntaxTree.GetRoot();
                    var rewriter = new ConsoleReadKeyRemovalRewriter();
                    var modifiedRoot = rewriter.Visit(root);
                    // Convert the modified syntax tree back to source code
                    string modifiedCode = modifiedRoot.ToFullString();



                    // Compile the code block
                    CompilationResult compilationResult = compiler.Compile(modifiedCode);
                    iteration.IsCompiled = true;
                    iteration.CompilationSuccess = compilationResult.Success;

                    if (compilationResult.Success)
                    {
                        ExecutionResult executionResult = executor.Execute(compilationResult.Assembly);
                        iteration.ExecutionOutput = executionResult.Output;
                    }
                    else
                    {
                        iteration.DiagnosticResults = compilationResult.Diagnostics;
                    }
                }

                // Display results of the compilation and execution
                Console.WriteLine($"Project: {project.Name}");
                Console.WriteLine($" Conversation #{conversation.Number}:");
                Console.WriteLine($"  Iteration #{iteration.Number}:");
                Console.WriteLine($"    Compilation success: {iteration.CompilationSuccess}");
                if (!iteration.CompilationSuccess)
                    Console.WriteLine($"    Diagnostics: {iteration.DiagnosticResults}");
                if (iteration.CompilationSuccess)
                    Console.WriteLine($"    Execution Output: {iteration.ExecutionOutput}");

                // Handle failed compilations (only if there's one source code block in the iteration)
                if (!iteration.CompilationSuccess && iteration.SourceCodes.Count == 1)
                {
                    // Construct the prompt with diagnostics and code
                    string promptForFix = "We get the following compilation messages:" + Environment.NewLine +
                                    $"{iteration.DiagnosticResults}" + Environment.NewLine + Environment.NewLine +
                                    "```csharp" + Environment.NewLine +
                                    $"{iteration.SourceCodes[0].Code}" + Environment.NewLine +
                                    "```";

                    Console.WriteLine(promptForFix);

                    // Send the prompt to the assistant
                    string assistantFixResponse = await _apiClientHandler.SendPromptAsync(context, promptForFix);
                    Console.WriteLine("Assistant: " + assistantFixResponse);

                    // Update the context with the assistant's response
                    context = ContextManager.UpdateContext(context, promptForFix, assistantFixResponse);

                    // Validate token limits for the updated context
                    int tokenCount = _apiClientHandler.EstimateTokenCount(context);
                    if (!_apiClientHandler.CheckTokenLimit(tokenCount))
                    {
                        Console.WriteLine("Warning: Token limit exceeded. Consider condensing the context.");
                    }



                    if (!CodeValidator.IsCsharpBlockProperlyClosed(assistantFixResponse))
                    {
                        Console.WriteLine("The code has improper `csharp` block closures.");

                        string instruction = "Complete the last C# code you wrote.";
                        assistantFixResponse = await _apiClientHandler.SendPromptAsync(context, instruction);
                        Console.WriteLine("Assistant: " + assistantFixResponse);

                        context = ContextManager.UpdateContext(context, instruction, assistantFixResponse);

                        if (!CodeValidator.IsCsharpBlockProperlyClosed(assistantFixResponse))
                        {
                            Console.WriteLine("I'm terminating this project.");
                            break; // terminate this project immediately
                        }
                    }



                    // Extract new code blocks from the response and reprocess them
                    List<string> newCodeBlocks = CodeValidator.ExtractCSharpCode(assistantFixResponse);

                    SourceCodeIteration newIteration = new SourceCodeIteration
                    {
                        Number = conversation.SourceCodeIterations.Count + 1
                    };
                    conversation.SourceCodeIterations.Add(newIteration);

                    // Only one code block is supported
                    foreach (string newCodeBlock in newCodeBlocks)
                    {
                        SourceCode1Nov2024 newSourceCode = new SourceCode1Nov2024(newCodeBlock);
                        newIteration.SourceCodes.Add(newSourceCode);


                        // Remove Console.ReadKey() calls from the user code
                        var syntaxTree = CSharpSyntaxTree.ParseText(newSourceCode.Code);
                        var root = syntaxTree.GetRoot();
                        var rewriter = new ConsoleReadKeyRemovalRewriter();
                        var modifiedRoot = rewriter.Visit(root);
                        // Convert the modified syntax tree back to source code
                        string modifiedCode = modifiedRoot.ToFullString();


                        CompilationResult newCompilationResult = compiler.Compile(modifiedCode);
                        newIteration.CompilationSuccess = newCompilationResult.Success;

                        if (newCompilationResult.Success)
                        {
                            ExecutionResult newExecutionResult = executor.Execute(newCompilationResult.Assembly);
                            newIteration.ExecutionOutput = newExecutionResult.Output;
                        }
                        else
                        {
                            newIteration.DiagnosticResults = newCompilationResult.Diagnostics;
                        }
                    }

                    // Display results of the new iteration
                    Console.WriteLine($"  Iteration #{newIteration.Number}:");
                    Console.WriteLine($"    Compilation success: {newIteration.CompilationSuccess}");
                    if (!newIteration.CompilationSuccess)
                        Console.WriteLine($"    Diagnostics: {newIteration.DiagnosticResults}");
                    if (newIteration.CompilationSuccess)
                        Console.WriteLine($"    Execution Output: {newIteration.ExecutionOutput}");
                }

            }
        }
    }
}