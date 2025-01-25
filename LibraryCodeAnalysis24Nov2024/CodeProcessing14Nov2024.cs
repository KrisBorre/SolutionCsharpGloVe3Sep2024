using Microsoft.CodeAnalysis.CSharp;

namespace LibraryCodeAnalysis24Nov2024
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
            CodeAnalyzer23Nov2024 codeAnalyzer = new CodeAnalyzer23Nov2024();
            codeAnalyzer.AnalyzeCode(codeBlock);

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
            string modifiedCode1 = RemoveConsoleReadKey(codeBlock1);
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


        public void ProcessThreeCodeBlocks(SourceCodeIteration iteration, string codeBlock1, string codeBlock2, string codeBlock3)
        {
            string modifiedCode1 = RemoveConsoleReadKey(codeBlock1);
            string modifiedCode2 = RemoveConsoleReadKey(codeBlock2);
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

        public void ProcessCodeBlocks(SourceCodeIteration iteration, params string[] codeBlocks)
        {
            // Create an array to hold modified code blocks after removing Console.ReadKey
            string[] modifiedCodeBlocks = new string[codeBlocks.Length];
            for (int i = 0; i < codeBlocks.Length; i++)
            {
                modifiedCodeBlocks[i] = RemoveConsoleReadKey(codeBlocks[i]);
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
                for (int i = 0; i < codeBlocks.Length; i++)
                {
                    modifiedCodeBlocks[i] = RemoveConsoleReadKey(codeBlocks[i]);
                }

                CompilationResult compilationResult = _compiler.Compile(modifiedCodeBlocks);

                if (compilationResult != null)
                {
                    codeBlocksCompile = compilationResult.Success;
                }
            }

            return codeBlocksCompile;
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
