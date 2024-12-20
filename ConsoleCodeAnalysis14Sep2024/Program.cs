using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis14Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = @"class Example
{
    void Test()
    {
        if (true)
            Console.WriteLine(""No braces here!"");
        if (false)
            Console.WriteLine(""Another unbraced statement!"");
    }
}";

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            // Find all 'if' statements in the syntax tree
            var ifStatements = root.DescendantNodes().OfType<IfStatementSyntax>()
                .Where(ifStmt => ifStmt.Statement is not BlockSyntax);

            if (!ifStatements.Any())
            {
                Console.WriteLine("No 'if' statements without braces found.");
                return;
            }

            // Rewrite 'if' statements to include braces
            var newRoot = root;
            foreach (var ifStmt in ifStatements)
            {
                // Wrap the existing statement in a BlockSyntax
                var blockStatement = SyntaxFactory.Block(ifStmt.Statement);

                // Replace the old 'if' statement with the modified one
                var newIfStmt = ifStmt.WithStatement(blockStatement)
                    .WithAdditionalAnnotations(SyntaxAnnotation.ElasticAnnotation);

                newRoot = newRoot.ReplaceNode(ifStmt, newIfStmt);
            }

            // Format the modified code
            var formattedCode = newRoot.NormalizeWhitespace().ToFullString();

            // Output the rewritten code
            Console.WriteLine("Rewritten code:");
            Console.WriteLine(formattedCode);

            /*
            Rewritten code:
            class Example
            {
                void Test()
                {
                    if (true)
                    {
                        Console.WriteLine("No braces here!");
                    }

                    if (false)
                        Console.WriteLine("Another unbraced statement!");
                }
            }
            */

            Console.ReadLine();
        }
    }
}