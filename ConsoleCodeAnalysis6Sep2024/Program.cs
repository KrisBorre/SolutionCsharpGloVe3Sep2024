using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis6Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Input: Path to a C# file to analyze
            Console.WriteLine("Enter the file path to analyze:");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist. Exiting...");
                return;
            }

            // Read the file content
            string code = File.ReadAllText(filePath);

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Collect diagnostics from the SyntaxTree
            IEnumerable<Diagnostic> diagnostics = syntaxTree.GetDiagnostics();

            Console.WriteLine("Syntax Errors:");
            Console.WriteLine("----------------------------------");

            // Report all syntax errors
            foreach (var diagnostic in diagnostics)
            {
                if (diagnostic.Severity == DiagnosticSeverity.Error) // Only process errors
                {
                    Console.WriteLine($"ID: {diagnostic.Id}");
                    Console.WriteLine($"Message: {diagnostic.GetMessage()}");
                    Console.WriteLine($"Severity: {diagnostic.Severity}");
                    Console.WriteLine($"Location: Line {diagnostic.Location.GetLineSpan().StartLinePosition.Line + 1}, Column {diagnostic.Location.GetLineSpan().StartLinePosition.Character + 1}");
                    Console.WriteLine("----------------------------------");
                }
            }

            if (!diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                Console.WriteLine("No syntax errors found!");
            }

            /*
            Enter the file path to analyze:
            C:\Users\kris_\source\repos\SolutionCsharpAttention1Sep2024\ConsoleBidirectional33Oct2024\Program.cs
            Syntax Errors:
            ----------------------------------
            No syntax errors found! 
            */

            Console.ReadLine();
        }
    }
}