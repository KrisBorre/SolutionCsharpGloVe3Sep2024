using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis27Nov2024
{
    public class SyntaxTreeMerger27Nov2024
    {
        public SyntaxTreeMerger27Nov2024()
        {

        }

        public SyntaxTree MergeSyntaxTrees(IEnumerable<SyntaxTree> syntaxTrees)
        {
            // Collect all the root nodes (CompilationUnits) from the provided SyntaxTrees
            var allMembers = syntaxTrees
                .Select(tree => tree.GetRoot() as CompilationUnitSyntax) // Cast roots to CompilationUnitSyntax
                .Where(root => root != null) // Ignore null roots
                .SelectMany(root => root.Members); // Collect all members from each root

            // Create a new CompilationUnitSyntax with combined members
            var mergedCompilationUnit = SyntaxFactory.CompilationUnit()
                .WithMembers(SyntaxFactory.List(allMembers)) // Add combined members
                .NormalizeWhitespace(); // Normalize whitespace for readability

            // Create and return a new SyntaxTree from the merged CompilationUnitSyntax
            return CSharpSyntaxTree.Create(mergedCompilationUnit);
        }
    }
}