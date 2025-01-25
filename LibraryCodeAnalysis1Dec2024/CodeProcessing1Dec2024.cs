namespace LibraryCodeAnalysis1Dec2024
{
    public class CodeProcessing1Dec2024
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;
        private readonly CodeCompiler1Dec2024 _compiler;
        private readonly CodeExecutor _executor;

        public CodeProcessing1Dec2024()
        {
            _metadataManager = new MetadataReferenceManager8Nov2024();
            _compiler = new CodeCompiler1Dec2024(_metadataManager);
            _executor = new CodeExecutor();
        }


        public void ProcessCodeBlocks(SourceCodeIteration26Nov2024 iteration, params string[] codeBlocks)
        {
            ConsoleReadKeyRemover28Nov2024 consoleReadKeyRemover = new ConsoleReadKeyRemover28Nov2024();

            // Create an array to hold modified code blocks after removing Console.ReadKey
            string[] modifiedCodeBlocks = new string[codeBlocks.Length];
            for (int i = 0; i < codeBlocks.Length; i++)
            {
                modifiedCodeBlocks[i] = consoleReadKeyRemover.RemoveConsoleReadKey(codeBlocks[i]);
            }

            // Compile the modified code blocks
            CompilationResult compilationResult = _compiler.Compile(modifiedCodeBlocks);

            // Update the iteration details
            iteration.IsCompiled = true;
            if (compilationResult != null)
            {
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
        }


        public bool CodeBlocksCompile(params string[] codeBlocks)
        {
            bool codeBlocksCompile = false;

            if (codeBlocks != null && codeBlocks.Length > 0)
            {
                // Create an array to hold modified code blocks after removing Console.ReadKey
                string[] modifiedCodeBlocks = new string[codeBlocks.Length];

                ConsoleReadKeyRemover28Nov2024 consoleReadKeyRemover = new ConsoleReadKeyRemover28Nov2024();

                for (int i = 0; i < codeBlocks.Length; i++)
                {
                    modifiedCodeBlocks[i] = consoleReadKeyRemover.RemoveConsoleReadKey(codeBlocks[i]);
                }

                CompilationResult compilationResult = _compiler.Compile(modifiedCodeBlocks);

                if (compilationResult != null)
                {
                    codeBlocksCompile = compilationResult.Success;
                }
            }

            return codeBlocksCompile;
        }
    }

}
