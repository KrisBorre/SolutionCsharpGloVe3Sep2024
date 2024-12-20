using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis23Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceCode = @"using System;

#region Utilities
public class Helper
{
    public void DoWork() { }
}
#endregion

#region Main
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(""Hello, World!"");
    }
}
#endregion
";

            // Parse the source code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Get the root node of the SyntaxTree
            SyntaxNode root = syntaxTree.GetRoot();

            // Extract all trivia from the root node
            var trivia = root.DescendantTrivia();

            Console.WriteLine("Regions found in the file:");

            // Filter for region directives
            var regionDirectives = trivia
                .Where(tr => tr.IsKind(SyntaxKind.RegionDirectiveTrivia));

            foreach (var region in regionDirectives)
            {
                // Print the text of the region directive
                Console.WriteLine(region.ToFullString().Trim());
            }

            /*
            Regions found in the file:
            #region Utilities
            #region Main 
            */

            Console.ReadLine();
        }
    }
}
