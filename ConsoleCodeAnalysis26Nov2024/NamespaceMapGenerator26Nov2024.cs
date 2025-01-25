using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ConsoleCodeAnalysis26Nov2024
{
    internal class NamespaceMapGenerator26Nov2024
    {
        public NamespaceMapGenerator26Nov2024()
        {
            
        }     

        public Dictionary<string, Dictionary<string, List<string>>> GenerateNamespaceMap(string code)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            var namespaceMap = new Dictionary<string, Dictionary<string, List<string>>>();

            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                if (!namespaceMap.ContainsKey(namespaceName))
                {
                    namespaceMap[namespaceName] = new Dictionary<string, List<string>>();
                }

                var classNodes = namespaceNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in classNodes)
                {
                    string className = classNode.Identifier.Text;
                    if (!namespaceMap[namespaceName].ContainsKey(className))
                    {
                        namespaceMap[namespaceName][className] = new List<string>();
                    }

                    var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();
                    foreach (var methodNode in methodNodes)
                    {
                        string methodName = methodNode.Identifier.Text;
                        namespaceMap[namespaceName][className].Add(methodName);
                    }
                }
            }

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

            return namespaceMap;
        }
    }
}
