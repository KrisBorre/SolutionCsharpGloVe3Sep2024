using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LibraryCodeAnalysis1Dec2024
{
    public class ConsoleReadKeyRemover28Nov2024
    {
        public ConsoleReadKeyRemover28Nov2024()
        {

        }

        public string RemoveConsoleReadKey(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            string result = RemoveConsoleReadKey(syntaxTree);
            return result;
        }

        private string RemoveConsoleReadKey(SyntaxTree syntaxTree)
        {
            SyntaxTreeProcessor27Nov2024 syntaxTreeProcessor = new SyntaxTreeProcessor27Nov2024();
            syntaxTree = syntaxTreeProcessor.RemoveNamespaces(syntaxTree);

            var root = syntaxTree.GetRoot();
            var rewriter = new ConsoleReadKeyRemovalRewriter();
            var modifiedRoot = rewriter.Visit(root);
            return modifiedRoot.ToFullString();
        }
    }
}
