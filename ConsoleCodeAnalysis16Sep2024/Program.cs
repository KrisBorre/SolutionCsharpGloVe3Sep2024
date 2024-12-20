using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis16Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceCode = @"public class SampleClass
{
    public virtual void VirtualMethod() { }
    private void PrivateMethod() { }
    public void NonVirtualMethod() { }
}";

            // Parse the source code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Create a Compilation object for the syntax tree
            Compilation compilation = CSharpCompilation.Create("Analysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            // Get the SemanticModel for the syntax tree
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Get the root of the syntax tree
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all method declarations in the code
            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var methodDeclaration in methodDeclarations)
            {
                // Get the symbol for the method using the SemanticModel
                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

                if (methodSymbol != null)
                {
                    // Check if the method is public and virtual
                    bool isPublic = methodSymbol.DeclaredAccessibility == Accessibility.Public;
                    bool isVirtual = methodSymbol.IsVirtual;

                    Console.WriteLine($"Method: {methodSymbol.Name}");
                    Console.WriteLine($"  IsPublic: {isPublic}");
                    Console.WriteLine($"  IsVirtual: {isVirtual}");
                }
            }

            /*
            Method: VirtualMethod
              IsPublic: True
              IsVirtual: True
            Method: PrivateMethod
              IsPublic: False
              IsVirtual: False
            Method: NonVirtualMethod
              IsPublic: True
              IsVirtual: False
            */

            Console.ReadLine();
        }
    }
}