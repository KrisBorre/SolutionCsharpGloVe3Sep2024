using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis26Nov2024
{
    public class NamespaceRemover26Nov2024 : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            // Flatten namespace members into the global scope
            var newMembers = node.Members.SelectMany<MemberDeclarationSyntax, MemberDeclarationSyntax>(member =>
            {
                if (member is NamespaceDeclarationSyntax namespaceDecl)
                {
                    // Extract the members of the namespace
                    return namespaceDecl.Members;
                }

                // Keep the member as-is if it's already in the global scope
                return new[] { member };
            });

            // Create a new CompilationUnitSyntax with all members in the global scope
            return node.WithMembers(SyntaxFactory.List(newMembers));
        }
    }

    public class SyntaxTreeProcessor
    {
        public static SyntaxTree RemoveNamespaces(SyntaxTree syntaxTree)
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
