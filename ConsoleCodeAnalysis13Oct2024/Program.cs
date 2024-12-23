using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis13Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string code = @"
        public class ExampleClass
        {
            public void ExampleMethod()
            {
                // This is a comment
                /* Another comment */
            }
        }";

            // Parse the code into a syntax tree
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            // Find the method
            var method = root.DescendantNodes()
                             .OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == "ExampleMethod");

            if (method != null)
            {
                // Get the body of the method
                var body = method.Body;

                if (body != null)
                {
                    // Check if the body contains only comments
                    bool containsOnlyComments = body.Statements.All(statement =>
                        statement.GetLeadingTrivia()
                                 .All(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia)));

                    Console.WriteLine(containsOnlyComments
                        ? "The method contains only comments."
                        : "The method contains more than just comments.");
                }
                else
                {
                    Console.WriteLine("The method does not have a body.");
                }
            }
            else
            {
                Console.WriteLine("Method not found.");
            }

            /*
            The method contains only comments. 
            */

            Console.ReadLine();
        }
    }
}