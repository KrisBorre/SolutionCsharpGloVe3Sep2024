namespace ConsoleCodeAnalysis5Nov2024
{
    public class SourceCodeProcessor
    {
        private readonly CodeCompiler _compiler;
        private readonly CodeExecutor _executor;

        public SourceCodeProcessor(CodeCompiler compiler, CodeExecutor executor)
        {
            _compiler = compiler;
            _executor = executor;
        }

        /// <summary>
        /// is not called
        /// </summary>
        /// <param name="sourceCodeIteration"></param>
        public void Process(SourceCodeIteration sourceCodeIteration)
        {
            if (sourceCodeIteration.SourceCodes.Count == 1)
            {
                var sourceCode = sourceCodeIteration.SourceCodes[0];
                var compilationResult = _compiler.Compile(sourceCode.Code);

                sourceCodeIteration.IsCompiled = true;
                sourceCodeIteration.CompilationSuccess = compilationResult.Success;

                if (!compilationResult.Success)
                {
                    sourceCodeIteration.DiagnosticResults = compilationResult.Diagnostics;
                    return;
                }

                var executionResult = _executor.Execute(compilationResult.Assembly);
                sourceCodeIteration.ExecutionOutput = executionResult.Output;
            }
            else
            {
                Console.WriteLine("Multiple source codes are not supported.");
            }
        }
    }
}
