using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryCodeAnalysis14Nov2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class CodeAnalyzer
    {
        public static void AnalyzeCode(string code)
        {
            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Store namespaces and their classes with their methods
            var namespaceMap = new Dictionary<string, Dictionary<string, List<string>>>();

            // Find all NamespaceDeclarationSyntax nodes
            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                if (!namespaceMap.ContainsKey(namespaceName))
                {
                    namespaceMap[namespaceName] = new Dictionary<string, List<string>>();
                }

                // Find all classes within this namespace
                var classNodes = namespaceNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in classNodes)
                {
                    string className = classNode.Identifier.Text;
                    if (!namespaceMap[namespaceName].ContainsKey(className))
                    {
                        namespaceMap[namespaceName][className] = new List<string>();
                    }

                    // Find methods within this class
                    var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();
                    foreach (var methodNode in methodNodes)
                    {
                        string methodName = methodNode.Identifier.Text;
                        namespaceMap[namespaceName][className].Add(methodName);
                    }
                }
            }

            // Find classes in the global namespace
            var globalClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                    .Where(cls => cls.Parent is not NamespaceDeclarationSyntax);

            foreach (var globalClass in globalClasses)
            {
                string className = globalClass.Identifier.Text;
                if (!namespaceMap.ContainsKey("<global namespace>"))
                {
                    namespaceMap["<global namespace>"] = new Dictionary<string, List<string>>();
                }

                if (!namespaceMap["<global namespace>"].ContainsKey(className))
                {
                    namespaceMap["<global namespace>"][className] = new List<string>();
                }

                var methodNodes = globalClass.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var methodNode in methodNodes)
                {
                    string methodName = methodNode.Identifier.Text;
                    namespaceMap["<global namespace>"][className].Add(methodName);
                }
            }

            // Output results to the console
            Console.WriteLine("Code Analysis Result:");
            foreach (var ns in namespaceMap)
            {
                Console.WriteLine($"Namespace: {ns.Key}");
                foreach (var cls in ns.Value)
                {
                    Console.WriteLine($"  Class: {cls.Key}");
                    foreach (var method in cls.Value)
                    {
                        Console.WriteLine($"    Method: {method}()");
                    }
                }
            }
        }
    }
}
