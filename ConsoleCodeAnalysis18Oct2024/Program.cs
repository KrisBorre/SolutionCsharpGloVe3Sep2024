using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis18Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string oldCode = @"
            public class Example
            {
                public void OldMethod1() { }
                public void SharedMethod() { }
            }";

            string newCode = @"
            public class Example
            {
                public void NewMethod1() { }
                public void SharedMethod() { }
            }";

            // Parse the old and new code into syntax trees
            SyntaxTree oldTree = CSharpSyntaxTree.ParseText(oldCode);
            SyntaxTree newTree = CSharpSyntaxTree.ParseText(newCode);

            // Get the root nodes
            CompilationUnitSyntax oldRoot = oldTree.GetCompilationUnitRoot();
            CompilationUnitSyntax newRoot = newTree.GetCompilationUnitRoot();

            // Find all method declarations
            var oldMethods = oldRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();
            var newMethods = newRoot.DescendantNodes().OfType<MethodDeclarationSyntax>();

            // Compare methods
            var removedMethods = oldMethods.Except(newMethods, new MethodComparer());
            var addedMethods = newMethods.Except(oldMethods, new MethodComparer());

            // Print results
            Console.WriteLine("Removed Methods:");
            foreach (var method in removedMethods)
            {
                Console.WriteLine($"- {method.Identifier.Text}");
            }

            Console.WriteLine("\nAdded Methods:");
            foreach (var method in addedMethods)
            {
                Console.WriteLine($"- {method.Identifier.Text}");
            }

            /*
            Removed Methods:
            - OldMethod1

            Added Methods:
            - NewMethod1
            */

            Console.ReadLine();
        }
    }

    // Custom comparer for comparing MethodDeclarationSyntax nodes
    class MethodComparer : IEqualityComparer<MethodDeclarationSyntax>
    {
        public bool Equals(MethodDeclarationSyntax x, MethodDeclarationSyntax y)
        {
            if (x == null || y == null)
                return false;

            // Compare based on method name and parameter list
            return x.Identifier.Text == y.Identifier.Text &&
                   x.ParameterList.ToString() == y.ParameterList.ToString();
        }

        public int GetHashCode(MethodDeclarationSyntax obj)
        {
            return obj.Identifier.Text.GetHashCode() ^
                   obj.ParameterList.ToString().GetHashCode();
        }
    }
}
