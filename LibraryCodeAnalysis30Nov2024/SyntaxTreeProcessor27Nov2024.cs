using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryCodeAnalysis30Nov2024
{
    public class SyntaxTreeProcessor27Nov2024
    {
        public SyntaxTreeProcessor27Nov2024()
        {
            
        }

        public SyntaxTree RemoveNamespaces(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetRoot();

            // Use the rewriter to modify the syntax tree
            var rewriter = new NamespaceRemover26Nov2024();
            var newRoot = (CompilationUnitSyntax)rewriter.Visit(root);

            // Return a new SyntaxTree with the modified root
            return syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
        }
    }
}
