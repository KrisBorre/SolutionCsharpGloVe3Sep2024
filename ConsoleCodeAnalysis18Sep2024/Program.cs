using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis18Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string codeSnippet = @"using System;
using System.Linq;

class Example
{
    static void Main(string[] args)
    {
        Func<int, int> square = x => x * x;
        var numbers = new[] { 1, 2, 3 };
        var doubled = numbers.Select(n => n * 2).ToList();
        numbers.ToList().ForEach(n => Console.WriteLine(n));
    }
}";

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeSnippet);

            // Get the root SyntaxNode
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all lambda expressions in the syntax tree
            var lambdaExpressions = root.DescendantNodes()
                                        .OfType<LambdaExpressionSyntax>();

            // Count and display the lambda expressions
            int lambdaCount = lambdaExpressions.Count();
            Console.WriteLine($"Number of lambda expressions found: {lambdaCount}");

            // Print each lambda expression
            foreach (var lambda in lambdaExpressions)
            {
                Console.WriteLine($"Lambda: {lambda.ToFullString().Trim()}");
            }

            /*
            Number of lambda expressions found: 3
            Lambda: x => x * x
            Lambda: n => n * 2
            Lambda: n => Console.WriteLine(n)
            */
        }
    }
}