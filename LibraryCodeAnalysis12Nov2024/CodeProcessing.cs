using Microsoft.CodeAnalysis.CSharp;

namespace LibraryCodeAnalysis12Nov2024
{
    public class CodeProcessing
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;
        private readonly CodeCompiler _compiler;
        private readonly CodeExecutor _executor;

        public CodeProcessing()
        {
            _metadataManager = new MetadataReferenceManager8Nov2024();
            _compiler = new CodeCompiler(_metadataManager);
            _executor = new CodeExecutor();
        }


        public void ProcessCodeBlock(string codeBlock)
        {
            var sourceCode = new SourceCode8Nov2024(codeBlock);
            //iteration.SourceCodes.Add(sourceCode);

            string modifiedCode = RemoveConsoleReadKey(sourceCode.Code);
            CompilationResult compilationResult = _compiler.Compile(modifiedCode);

            sourceCode.IsCompiled = true;
            sourceCode.CompilationSuccess = compilationResult.Success;

            if (compilationResult.Success)
            {
                ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                sourceCode.ExecutionOutput = executionResult.Output;
            }
            else
            {
                sourceCode.CompilationErrors = compilationResult.Diagnostics;
            }
        }

        public void ProcessCodeBlock(SourceCodeIteration iteration, string codeBlock)
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

        public void ProcessTwoCodeBlocks(SourceCodeIteration iteration, string codeBlock1, string codeBlock2)
        {
            var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
            iteration.SourceCodes.Add(sourceCode1);

            string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);

            var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
            iteration.SourceCodes.Add(sourceCode2);

            string modifiedCode2 = RemoveConsoleReadKey(sourceCode1.Code);

            CompilationResult compilationResult = _compiler.Compile(modifiedCode1, modifiedCode2);

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


        public void ProcessCodeBlock(SourceCodeIteration iteration, string codeBlock1, string codeBlock2, string codeBlock3)
        {
            var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
            iteration.SourceCodes.Add(sourceCode1);

            string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);

            var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
            iteration.SourceCodes.Add(sourceCode2);

            string modifiedCode2 = RemoveConsoleReadKey(sourceCode2.Code);

            var sourceCode3 = new SourceCode8Nov2024(codeBlock3);
            iteration.SourceCodes.Add(sourceCode3);

            string modifiedCode3 = RemoveConsoleReadKey(sourceCode3.Code);

            CompilationResult compilationResult = _compiler.Compile(modifiedCode1, modifiedCode2, modifiedCode3);

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

        public void ProcessThreeCodeBlocks(SourceCodeIteration iteration, string codeBlock1, string codeBlock2, string codeBlock3)
        {
            var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
            iteration.SourceCodes.Add(sourceCode1);

            string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);

            var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
            iteration.SourceCodes.Add(sourceCode2);

            string modifiedCode2 = RemoveConsoleReadKey(sourceCode2.Code);

            var sourceCode3 = new SourceCode8Nov2024(codeBlock3);
            iteration.SourceCodes.Add(sourceCode3);

            string modifiedCode3 = RemoveConsoleReadKey(sourceCode3.Code);

            CompilationResult compilationResult = _compiler.Compile(modifiedCode1, modifiedCode2, modifiedCode3);

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
    }

}
