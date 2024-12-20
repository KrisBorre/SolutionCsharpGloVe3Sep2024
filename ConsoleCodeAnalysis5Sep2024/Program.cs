using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis5Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // File path to the C# file to analyze
            Console.WriteLine("Enter the file path to a C# file:");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist. Exiting...");
                return;
            }

            // Read the C# file content
            string code = File.ReadAllText(filePath);

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Create a Compilation object
            Compilation compilation = CSharpCompilation.Create("CodeAnalysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            // Get the SemanticModel from the Compilation
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Specify the class name to search for
            Console.WriteLine("Enter the class name to search for references:");
            string className = Console.ReadLine();

            // Find all identifiers in the syntax tree
            var identifiers = syntaxTree.GetRoot()
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

            /*
            Enter the file path to a C# file:
            C:\Users\kris_\source\repos\SolutionCsharpAttention1Sep2024\ConsoleBidirectional33Oct2024\Program.cs
            Enter the class name to search for references:
            BidirectionalRNN
            References to the class 'BidirectionalRNN':
            - Found reference at line 107, column 13
            - Found reference at line 107, column 42 
            */

            Console.ReadLine();
        }
    }
}
