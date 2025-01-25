namespace LibraryCodeAnalysis24Nov2024
{
    internal class DisplayManagement
    {
        public void DisplayIterationResults(Project9Oct2024 project, Conversation conversation, SourceCodeIteration iteration)
        {
            Console.WriteLine($"Project: {project.Name}");
            Console.WriteLine($" Conversation #{conversation.Number}:");
            Console.WriteLine($"  Iteration #{iteration.Number}:");

            if (iteration.IsCompiled)
            {
                Console.WriteLine($"    Compilation success: {iteration.CompilationSuccess}");
                if (iteration.CompilationSuccess)
                {
                    Console.WriteLine($"    Execution Output: {iteration.ExecutionOutput}");
                }
                else
                {
                    Console.WriteLine($"    Diagnostics: {iteration.DiagnosticResults}");
                }
            }

            foreach (var sourceCode in iteration.SourceCodes)
            {               
                if (sourceCode.IsCompiled)
                {
                    Console.WriteLine($"     Code: {sourceCode.Code}");
                    if (sourceCode.CompilationSuccess)
                    {
                        Console.WriteLine($"      Execution Output: {sourceCode.ExecutionOutput}");
                    }
                    else
                    {
                        Console.WriteLine($"      Compilation Errors: {sourceCode.CompilationErrors}");
                    }
                }
            }

        }
    }
}
