using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis21Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            // Input C# code
            string code = @"
        class Demo
        {
            void ShortMethod()
            {
                int x = 0;
            }

            void LongMethod()
            {
                int a = 1;
                int b = 2;
                int c = 3;
                int d = 4;
                int e = 5;
                int f = 6;
                int g = 7;
                int h = 8;
                int i = 9;
                int j = 10;
                int k = 11; // More than 10 lines
            }
        }";

            // Parse the code into a syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Analyze the syntax tree and find diagnostics
            List<Diagnostic> diagnostics = AnalyzeMethodsForLength(root);

            // Display diagnostics
            foreach (var diagnostic in diagnostics)
            {
                Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()} at {diagnostic.Location}");
            }

            /*
            LONG_METHOD: Method 'LongMethod' exceeds 10 lines (found 14 lines). at SourceFile([137..516))
            */
        }

        static List<Diagnostic> AnalyzeMethodsForLength(SyntaxNode root)
        {
            var diagnostics = new List<Diagnostic>();
            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            const int lineThreshold = 10; // Define the line count threshold

            foreach (var method in methodDeclarations)
            {
                // Get the line span of the method
                var span = method.GetLocation().GetLineSpan();
                int lineCount = span.EndLinePosition.Line - span.StartLinePosition.Line + 1;

                if (lineCount > lineThreshold)
                {
                    // Create a diagnostic for methods exceeding the threshold
                    var diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            id: "LONG_METHOD",
                            title: "Long Method",
                            messageFormat: $"Method '{method.Identifier.Text}' exceeds {lineThreshold} lines (found {lineCount} lines).",
                            category: "CodeQuality",
                            defaultSeverity: DiagnosticSeverity.Warning,
                            isEnabledByDefault: true),
                        method.GetLocation()
                    );

                    diagnostics.Add(diagnostic);
                }
            }

            return diagnostics;
        }

    }
}