using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis19Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main()
        {
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
        }";

            Console.WriteLine("Original Code:");
            Console.WriteLine(code);

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Specify the method name to replace and the new name
            Console.WriteLine("Enter the method name to replace:");
            string oldMethodName = Console.ReadLine();
            Console.WriteLine("Enter the new method name:");
            string newMethodName = Console.ReadLine();

            // Traverse the SyntaxNode tree and replace method names
            var updatedRoot = ReplaceMethodName(root, oldMethodName, newMethodName);

            // Output the updated code
            string updatedCode = updatedRoot.ToFullString();
            Console.WriteLine("Modified Code:");
            Console.WriteLine(updatedCode);

            /*
            Original Code:

                    using System;

                    public class SampleClass
                    {
                        public void MethodOne()
                        {
                            Console.WriteLine("Hello from MethodOne");
                        }

                        private int MethodTwo(int x)
                        {
                            return x * x;
                        }
                    }
            Enter the method name to replace:
            MethodTwo
            Enter the new method name:
            Kwadraat
            Modified Code:

                    using System;

                    public class SampleClass
                    {
                        public void MethodOne()
                        {
                            Console.WriteLine("Hello from MethodOne");
                        }

                        private int Kwadraat(int x)
                        {
                            return x * x;
                        }
                    }
            */

            Console.ReadLine();
        }

        static SyntaxNode ReplaceMethodName(SyntaxNode root, string oldMethodName, string newMethodName)
        {
            return root.ReplaceNodes(
                root.DescendantNodes().OfType<MethodDeclarationSyntax>(),
                (originalNode, _) =>
                {
                    // Check if the method name matches the old method name
                    if (originalNode.Identifier.Text == oldMethodName)
                    {
                        // Replace the method identifier with the new name
                        return originalNode.WithIdentifier(SyntaxFactory.Identifier(newMethodName))
                                           .WithTriviaFrom(originalNode);
                    }
                    return originalNode;
                });
        }
    }
}
