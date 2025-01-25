using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis27Nov2024
{
    public class CodeProcessing26Nov2024
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;
        private readonly CodeCompiler27Nov2024 _compiler;

        public CodeProcessing26Nov2024()
        {
            _metadataManager = new MetadataReferenceManager8Nov2024();
            _compiler = new CodeCompiler27Nov2024(_metadataManager);
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
