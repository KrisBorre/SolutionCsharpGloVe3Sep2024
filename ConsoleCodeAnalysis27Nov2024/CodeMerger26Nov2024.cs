using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ConsoleCodeAnalysis27Nov2024
{
    // Do not use this.
    public class CodeMerger26Nov2024
    {
        public CodeMerger26Nov2024()
        {

        }

        public SyntaxTree MergeCodeBlocks(List<string> codeBlocks)
        {
            // Create a single merged code string by combining all code blocks
            var mergedCode = string.Join("\n", codeBlocks);

            // Parse the merged code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(mergedCode);

            return syntaxTree;
        }
    }
}
