using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis4Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Input C# code snippet
            string code = @"
            using System;

            public class SampleClass
            {
                public void MethodOne()
                {
                    Console.WriteLine(""Hello from MethodOne"");
                }

                private int MethodTwo(int x)
                {
                    return x * x;
                }
            }
        ";

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Get the root SyntaxNode of the SyntaxTree
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all MethodDeclarationSyntax nodes
            var methodDeclarations = root.DescendantNodes()
                                         .OfType<MethodDeclarationSyntax>();

            Console.WriteLine("Methods found in the code snippet:");

            // Print method names
            foreach (var method in methodDeclarations)
            {
                Console.WriteLine($"- {method.Identifier.Text}");
            }

            /*
            Methods found in the code snippet:
            - MethodOne
            - MethodTwo 
            */

            Console.ReadLine();
        }
    }
}
