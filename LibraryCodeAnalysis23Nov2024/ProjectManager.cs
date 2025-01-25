namespace LibraryCodeAnalysis23Nov2024
{
    public class ProjectManager
    {
        public void Print(Project9Oct2024 project)
        {
            Console.WriteLine($"Project: {project.Name}");
            foreach (var conversation in project.Conversations)
            {
                Console.WriteLine($"conversation #{conversation.Number}");
                foreach (var sourceCodeIteration in conversation.SourceCodeIterations)
                {
                    Console.WriteLine($"iteration #{sourceCodeIteration.Number}");

                    if (sourceCodeIteration.IsCompiled)
                    {
                        if (sourceCodeIteration.CompilationSuccess)
                        {
                            Console.WriteLine($"{sourceCodeIteration.ExecutionOutput}");
                        }
                        else
                        {
                            Console.WriteLine($"{sourceCodeIteration.DiagnosticResults}");
                        }
                    }

                    foreach (var sourceCode in sourceCodeIteration.SourceCodes)
                    {
                        Console.WriteLine($"Code: {sourceCode.Code}");

                        if (sourceCode.IsCompiled)
                        {
                            if (sourceCode.CompilationSuccess)
                            {
                                Console.WriteLine($"Execution Output: {sourceCode.ExecutionOutput}");
                            }
                            else
                            {
                                Console.WriteLine($"Compilation Errors: {sourceCode.CompilationErrors}");
                            }
                        }
                    }
                }
            }
        }

        public Conversation CreateNewConversation(Project9Oct2024 project)
        {
            var conversation = new Conversation
            {
                Number = project.Conversations.Count + 1
            };
            project.Conversations.Add(conversation);
            return conversation;
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
    }
}
