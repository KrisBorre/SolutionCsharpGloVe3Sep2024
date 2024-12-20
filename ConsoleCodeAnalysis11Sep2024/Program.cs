using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis11Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Example C# code snippet
            string code = @"
        namespace Namespace1
        {
            class ClassA { }
            class ClassB { }
        }

        namespace Namespace2
        {
            class ClassC { }
        }";

            // Parse the code into a syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Extract namespaces and their classes
            var namespaces = ExtractNamespacesAndClasses(root);

            // Print the results
            foreach (var ns in namespaces)
            {
                Console.WriteLine($"Namespace: {ns.Key}");
                foreach (var className in ns.Value)
                {
                    Console.WriteLine($"  Class: {className}");
                }
            }

            /*
            Namespace: Namespace1
              Class: ClassA
              Class: ClassB
            Namespace: Namespace2
              Class: ClassC
            */
        }

        static Dictionary<string, List<string>> ExtractNamespacesAndClasses(SyntaxNode root)
        {
            var namespaceClassMap = new Dictionary<string, List<string>>();

            // Find all NamespaceDeclarationSyntax nodes
            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                var classNames = namespaceNode.DescendantNodes()
                                              .OfType<ClassDeclarationSyntax>()
                                              .Select(cls => cls.Identifier.Text)
                                              .ToList();

                namespaceClassMap[namespaceName] = classNames;
            }

            return namespaceClassMap;
        }
    }
}