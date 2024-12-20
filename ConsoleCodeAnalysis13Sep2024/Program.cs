using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis13Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {

            string code = @"
       using System;

class Example
{
    [Obsolete]
    void OldMethod1() { }

    void NewMethod() { }

    [Obsolete]
    void OldMethod2() { }
}";

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            // Create a compilation for semantic analysis
            var compilation = CSharpCompilation.Create("AttributeAnalysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            // Get the SemanticModel for analysis
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Find all method declarations in the syntax tree
            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            // Filter methods with the [Obsolete] attribute
            int obsoleteMethodCount = methodDeclarations.Count(method =>
            {
                var symbol = semanticModel.GetDeclaredSymbol(method);
                if (symbol == null) return false;

                // Check if any attributes on the method are named "Obsolete"
                return symbol.GetAttributes().Any(attr => attr.AttributeClass.Name == "ObsoleteAttribute");
            });

            // Output the result
            Console.WriteLine($"Number of methods marked with [Obsolete]: {obsoleteMethodCount}");

            /*
            Number of methods marked with [Obsolete]: 2
            */

            Console.ReadLine();
        }
    }
}