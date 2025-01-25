using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis3Dec2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            string code = @"
            namespace ExampleNamespace
            {
                public class ExampleClass
                {
                    public void Method1()
                    {
                        Console.WriteLine(""Hello, World!"");
                    }

                    public void Method2()
                    {
                        Console.WriteLine(""Another Method"");
                    }
                }
            }
        ";

            // Parse the source code into a syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Get the root node of the syntax tree
            var root = syntaxTree.GetRoot();

            // Find the class declaration node
            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls => cls.Identifier.Text == "ExampleClass");

            if (classNode != null)
            {
                // Get the location of the class and calculate the line span
                var lineSpan = classNode.GetLocation().GetLineSpan();

                // Calculate the number of lines
                int lineCount = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line + 1;

                Console.WriteLine($"The class '{classNode.Identifier.Text}' has {lineCount} lines.");
            }
            else
            {
                Console.WriteLine("Class not found.");
            }

            // The class 'ExampleClass' has 12 lines.

            Console.ReadLine();
        }
    }
}