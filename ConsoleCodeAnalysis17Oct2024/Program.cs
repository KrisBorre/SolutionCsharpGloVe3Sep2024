using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis17Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Multiple code examples
            var codeExamples = new[]
            {
                // Example 1: Method with only comments
                @"
                public class Example1
                {
                    public void Method1()
                    {
                        // This is a single-line comment
                        /* This is a multi-line comment */
                    }
                }",

                // Example 2: Method with code and comments
                @"
                public class Example2
                {
                    public void Method2()
                    {
                        // This is a comment
                        Console.WriteLine(""Hello, World!"");
                    }
                }",

                // Example 3: Empty method
                @"
                public class Example3
                {
                    public void Method3()
                    {
                    }
                }",

                // Example 4: Method with only code
                @"
                public class Example4
                {
                    public void Method4()
                    {
                        Console.WriteLine(""No comments here."");
                    }
                }"
            };

            // Analyze each example
            for (int i = 0; i < codeExamples.Length; i++)
            {
                Console.WriteLine($"Analyzing Example {i + 1}...");

                var code = codeExamples[i];
                var tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetCompilationUnitRoot();

                // Find all methods in the code
                var methods = root.DescendantNodes()
                                  .OfType<MethodDeclarationSyntax>();

                foreach (var method in methods)
                {
                    Console.WriteLine($"Method: {method.Identifier.Text}");

                    var body = method.Body;
                    if (body != null)
                    {
                        // Check if the method contains only comments
                        bool containsOnlyComments = body.Statements.All(statement =>
                            statement.GetLeadingTrivia()
                                     .All(trivia =>
                                         trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                                         trivia.IsKind(SyntaxKind.MultiLineCommentTrivia)));

                        Console.WriteLine(containsOnlyComments
                            ? "The method contains only comments."
                            : "The method contains more than just comments.");
                    }
                    else
                    {
                        Console.WriteLine("The method does not have a body.");
                    }
                }

                Console.WriteLine();
            }

            /*
            Analyzing Example 1...
            Method: Method1
            The method contains only comments.

            Analyzing Example 2...
            Method: Method2
            The method contains more than just comments.

            Analyzing Example 3...
            Method: Method3
            The method contains only comments.

            Analyzing Example 4...
            Method: Method4
            The method contains more than just comments.
            */

            Console.ReadLine();
        }
    }
}
