using LibraryCodeAnalysis1Dec2024;

namespace ConsoleCodeAnalysis2Dec2024
{
    public class ProjectAnalyzer2Dec2024
    {
        private Project9Oct2024 project;

        public ProjectAnalyzer2Dec2024(Project9Oct2024 project)
        {
            this.project = project;
        }

        public void Show(string className)
        {
            Console.WriteLine($"Counting lines of code for {className}: ");
            foreach (Conversation conversation in project.Conversations)
            {
                foreach (SourceCodeIteration26Nov2024 sourceCodeIteration in conversation.SourceCodeIterations)
                {
                    foreach (SourceCode30Nov2024 sourceCode in sourceCodeIteration.SourceCodes)
                    {
                        ClassLineCounter classLineCounter = new ClassLineCounter(sourceCode.SyntaxTree);
                        classLineCounter.Count(className);
                    }
                }
            }
        }

        // New method to sort code blocks by the length of the class definitions
        public List<SourceCode30Nov2024> SortCodeBlocksByClassLength(string className)
        {
            var codeBlocksWithLengths = new List<(SourceCode30Nov2024 Code, int Length)>();

            foreach (Conversation conversation in project.Conversations)
            {
                foreach (SourceCodeIteration26Nov2024 sourceCodeIteration in conversation.SourceCodeIterations)
                {
                    foreach (SourceCode30Nov2024 sourceCode in sourceCodeIteration.SourceCodes)
                    {
                        ClassLineCounter classLineCounter = new ClassLineCounter(sourceCode.SyntaxTree);
                        int length = classLineCounter.LineCount(className);

                        if (length > 0)
                        {
                            // Add the code block and its length to the list
                            codeBlocksWithLengths.Add((sourceCode, length));
                        }
                    }
                }
            }

            // Sort the code blocks by class length in descending order
            var sortedCodeBlocks = codeBlocksWithLengths
                .OrderByDescending(entry => entry.Length)
                .Select(entry => entry.Code)
                .ToList();

            return sortedCodeBlocks;
        }
    }
}
