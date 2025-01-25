using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryCodeAnalysis29Nov2024
{
    // see ConsoleCodeAnalysis5Sep2024
    public class ReferenceSearcher26Nov2024
    {
        private SyntaxTree syntaxTree;

        public ReferenceSearcher26Nov2024(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;   
        }

        public void Find(string className)
        {
            // Create a Compilation object
            Compilation compilation = CSharpCompilation.Create("CodeAnalysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(this.syntaxTree);

            // Get the SemanticModel from the Compilation
            SemanticModel semanticModel = compilation.GetSemanticModel(this.syntaxTree);

            // Find all identifiers in the syntax tree
            var identifiers = this.syntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>();

            Console.WriteLine($"References to the class '{className}':");

            // Check each identifier to see if it refers to the specified class
            foreach (var identifier in identifiers)
            {
                var symbol = semanticModel.GetSymbolInfo(identifier).Symbol;

                if (symbol != null && symbol.Kind == SymbolKind.NamedType && symbol.Name == className)
                {
                    var location = identifier.GetLocation();
                    Console.WriteLine($"- Found reference at line {location.GetLineSpan().StartLinePosition.Line + 1}, column {location.GetLineSpan().StartLinePosition.Character + 1}");
                }
            }
        }
    }
}
