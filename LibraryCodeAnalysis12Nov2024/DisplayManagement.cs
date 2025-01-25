namespace LibraryCodeAnalysis12Nov2024
{
    internal class DisplayManagement
    {
        public void DisplayIterationResults(Project9Oct2024 project, Conversation conversation, SourceCodeIteration iteration)
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
    }
}
