using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis15Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            string code = @"
        public class MyClass
        {
            public void SayHello()
            {
                Console.WriteLine(""Hello, World!"");
            }
        }";

            // Step 1: Parse the code into a syntax tree
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            // Step 2: Modify the method using a SyntaxRewriter
            var rewriter = new MethodBodyModifier();
            var modifiedRoot = rewriter.Visit(root);

            // Step 3: Print the modified code
            Console.WriteLine(modifiedRoot.ToFullString());

            /*
            Expected Output:
                    public class MyClass
                    {
                        public void SayHello()
                        {
                            Console.WriteLine("Method Called");
                            Console.WriteLine("Hello, World!");
                        }
                    }
            */

            // actual output:
            /*

                    public class MyClass
                    {
                        public void SayHello()
                        {
            Console.WriteLine("Method Called");
                            Console.WriteLine("Hello, World!");
                        }
                    }


            */

            Console.ReadLine();
        }
    }

    class MethodBodyModifier : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // Find the body of the method
            var oldBody = node.Body;

            // Add a new statement at the beginning of the method
            var newStatement = SyntaxFactory.ParseStatement("Console.WriteLine(\"Method Called\");\n");

            // Create a new block with the added statement
            var newBody = oldBody.WithStatements(oldBody.Statements.Insert(0, newStatement));

            // Return the updated method declaration
            return node.WithBody(newBody);
        }
    }
}
