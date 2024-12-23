using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis22Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Example user-provided code
            string userCode = @"
            public class TestClass
            {
                public void TestMethod()
                {
                    if (true)
                    {
                        Console.WriteLine(""This will always execute."");
                    }

                    if (false)
                    {
                        Console.WriteLine(""This will never execute."");
                    }

                    if (1 + 1 == 2)
                    {
                        Console.WriteLine(""Always true, but not a literal true."");
                    }
                }
            }";

            // Parse the code into a syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);
            var root = syntaxTree.GetRoot();

            // Find all 'if' statements
            var ifStatements = root.DescendantNodes().OfType<IfStatementSyntax>();

            // Analyze each 'if' statement
            foreach (var ifStatement in ifStatements)
            {
                if (ifStatement.Condition.IsKind(SyntaxKind.TrueLiteralExpression))
                {
                    Console.WriteLine("Hint: The condition in this 'if' statement is always true.");
                }
            }

            /*
            Hint: The condition in this 'if' statement is always true. 
            */
        }
    }
}
