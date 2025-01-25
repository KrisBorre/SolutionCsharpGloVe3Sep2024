using Microsoft.CodeAnalysis.CSharp;

namespace LibraryCodeAnalysis14Nov2024
{
    public class CodeProcessing14Nov2024
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;
        private readonly CodeCompiler _compiler;
        private readonly CodeExecutor _executor;

        public CodeProcessing14Nov2024()
        {
            _metadataManager = new MetadataReferenceManager8Nov2024();
            _compiler = new CodeCompiler(_metadataManager);
            _executor = new CodeExecutor();
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
            //var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
            //iteration.SourceCodes.Add(sourceCode1);

            //string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);
            string modifiedCode1 = RemoveConsoleReadKey(codeBlock1);

            //var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
            //iteration.SourceCodes.Add(sourceCode2);

            //string modifiedCode2 = RemoveConsoleReadKey(sourceCode1.Code);
            string modifiedCode2 = RemoveConsoleReadKey(codeBlock2);

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


        //public void ProcessCodeBlock(SourceCodeIteration iteration, string codeBlock1, string codeBlock2, string codeBlock3)
        //{
        //    var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
        //    iteration.SourceCodes.Add(sourceCode1);

        //    string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);

        //    var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
        //    iteration.SourceCodes.Add(sourceCode2);

        //    string modifiedCode2 = RemoveConsoleReadKey(sourceCode2.Code);

        //    var sourceCode3 = new SourceCode8Nov2024(codeBlock3);
        //    iteration.SourceCodes.Add(sourceCode3);

        //    string modifiedCode3 = RemoveConsoleReadKey(sourceCode3.Code);

        //    CompilationResult compilationResult = _compiler.Compile(modifiedCode1, modifiedCode2, modifiedCode3);

        //    iteration.IsCompiled = true;
        //    iteration.CompilationSuccess = compilationResult.Success;

        //    if (compilationResult.Success)
        //    {
        //        ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
        //        iteration.ExecutionOutput = executionResult.Output;
        //    }
        //    else
        //    {
        //        iteration.DiagnosticResults = compilationResult.Diagnostics;
        //    }
        //}

        public void ProcessThreeCodeBlocks(SourceCodeIteration iteration, string codeBlock1, string codeBlock2, string codeBlock3)
        {
            //var sourceCode1 = new SourceCode8Nov2024(codeBlock1);
            //iteration.SourceCodes.Add(sourceCode1);

            //string modifiedCode1 = RemoveConsoleReadKey(sourceCode1.Code);
            string modifiedCode1 = RemoveConsoleReadKey(codeBlock1);

            //var sourceCode2 = new SourceCode8Nov2024(codeBlock2);
            //iteration.SourceCodes.Add(sourceCode2);

            //string modifiedCode2 = RemoveConsoleReadKey(sourceCode1.Code);
            string modifiedCode2 = RemoveConsoleReadKey(codeBlock2);

            //var sourceCode3 = new SourceCode8Nov2024(codeBlock3);
            //iteration.SourceCodes.Add(sourceCode3);

            //string modifiedCode3 = RemoveConsoleReadKey(sourceCode3.Code);
            string modifiedCode3 = RemoveConsoleReadKey(codeBlock3);

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

        private string RemoveConsoleReadKey(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();
            var rewriter = new ConsoleReadKeyRemovalRewriter();
            var modifiedRoot = rewriter.Visit(root);
            return modifiedRoot.ToFullString();
        }
    }

}
