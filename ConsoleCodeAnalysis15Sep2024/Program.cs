using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis15Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Read the source code
            string sourceCode = @"class Example
{
    void Display()
    {
        Console.WriteLine(""Hello, World!"");
        string title = ""My Application"";
    }
}";

            // Parse the source code
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all string literals in the syntax tree
            var stringLiterals = root.DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .Where(node => node.IsKind(SyntaxKind.StringLiteralExpression));

            if (!stringLiterals.Any())
            {
                Console.WriteLine("No hardcoded string literals found in the file.");
                return;
            }

            // Prepare replacements
            var replacements = new Dictionary<SyntaxNode, SyntaxNode>();
            foreach (var literal in stringLiterals)
            {
                string value = literal.Token.ValueText;

                // Create a resource reference (e.g., "Resources.<key>")
                string resourceKey = GenerateResourceKey(value);
                var resourceAccess = SyntaxFactory.ParseExpression($"Resources.{resourceKey}");

                // Add to replacements dictionary
                replacements[literal] = resourceAccess;

                Console.WriteLine($"Replacing \"{value}\" with Resources.{resourceKey}");
            }

            // Apply replacements
            var newRoot = root.ReplaceNodes(replacements.Keys, (oldNode, _) => replacements[oldNode]);

            Console.WriteLine(newRoot.NormalizeWhitespace().ToFullString());

            // Optionally, generate the resource file
            //GenerateResourceFile(replacements.Values.Select(v => v.ToString().Split('.').Last()), filePath);

            /*
            Replacing "Hello, World!" with Resources.String_2B8EEEF4
            Replacing "My Application" with Resources.String_6D242281
            class Example
            {
                void Display()
                {
                    Console.WriteLine(Resources.String_2B8EEEF4);
                    string title = Resources.String_6D242281;
                }
            }
            */

        }

        static string GenerateResourceKey(string value)
        {
            // Generate a simple resource key based on the string value
            return "String_" + value.GetHashCode().ToString("X");
        }

        static void GenerateResourceFile(IEnumerable<string> resourceKeys, string sourceFilePath)
        {
            string resourceFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), "Resources.resx");
            using (var writer = new StreamWriter(resourceFilePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<root>");
                writer.WriteLine("  <resheader name=\"resmimetype\">");
                writer.WriteLine("    <value>text/microsoft-resx</value>");
                writer.WriteLine("  </resheader>");
                writer.WriteLine("  <resheader name=\"version\">");
                writer.WriteLine("    <value>2.0</value>");
                writer.WriteLine("  </resheader>");

                foreach (var key in resourceKeys)
                {
                    writer.WriteLine($"  <data name=\"{key}\" xml:space=\"preserve\">");
                    writer.WriteLine("    <value></value> <!-- Add value manually -->");
                    writer.WriteLine("  </data>");
                }

                writer.WriteLine("</root>");
            }

            Console.WriteLine($"Resource file generated at: {resourceFilePath}");
        }
    }
}