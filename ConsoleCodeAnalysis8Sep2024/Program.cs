using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis8Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            // Input: Unformatted C# code snippet
            string code = @"
        using System;namespace DemoApp{class Program{static void Main(){Console.WriteLine(""Hello, World!"");}}}
        ";

            Console.WriteLine("Original Code:");
            Console.WriteLine(code);

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Normalize the whitespace in the syntax tree
            SyntaxNode formattedRoot = root.NormalizeWhitespace();

            // Output the formatted code
            string formattedCode = formattedRoot.ToFullString();
            Console.WriteLine("\nFormatted Code:");
            Console.WriteLine(formattedCode);

            /*
            Original Code:

                    using System;namespace DemoApp{class Program{static void Main(){Console.WriteLine("Hello, World!");}}}


            Formatted Code:
            using System;

            namespace DemoApp
            {
                class Program
                {
                    static void Main()
                    {
                        Console.WriteLine("Hello, World!");
                    }
                }
            }

            */

        }
    }
}