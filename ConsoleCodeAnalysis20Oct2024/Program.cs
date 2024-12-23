using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis20Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Sample user code for analysis
            string userCode = @"
            public class MyClass
            {
                public void ProcessCollection()
                {
                    foreach (var item in new[] { 1, 2, 3 })
                    {
                        Console.WriteLine(item);
                    }
                }
            }";

            // Parse the user code into a syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);
            var root = syntaxTree.GetRoot();

            // Check for the presence of a for-loop
            var containsForLoop = root.DescendantNodes().OfType<ForStatementSyntax>().Any();

            if (!containsForLoop)
            {
                Console.WriteLine("Hint: Try using a for-loop to iterate over the collection.");
            }
            else
            {
                Console.WriteLine("Good job! Your code contains a for-loop.");
            }

            // Optional: Print the original code
            Console.WriteLine("\nOriginal Code:");
            Console.WriteLine(userCode);

            /*
            Hint: Try using a for-loop to iterate over the collection.

            Original Code:

                        public class MyClass
                        {
                            public void ProcessCollection()
                            {
                                foreach (var item in new[] { 1, 2, 3 })
                                {
                                    Console.WriteLine(item);
                                }
                            }
                        }
            */

            Console.ReadLine();
        }
    }
}
