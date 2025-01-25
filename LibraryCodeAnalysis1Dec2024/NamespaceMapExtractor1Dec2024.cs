using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryCodeAnalysis1Dec2024
{
    public class NamespaceMapExtractor1Dec2024
    {
        private SyntaxTree syntaxTree;

        public NamespaceMapExtractor1Dec2024(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
        }
               
        public NamespaceMap30Nov2024 BuildNamespaceMap()
        {
            SyntaxNode root = this.syntaxTree.GetRoot();

            var keyValuePairs = new Dictionary<string, Dictionary<string, List<string>>>();

            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                if (!keyValuePairs.ContainsKey(namespaceName))
                {
                    keyValuePairs[namespaceName] = new Dictionary<string, List<string>>();
                }

                var classNodes = namespaceNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var classNode in classNodes)
                {
                    string className = classNode.Identifier.Text;
                    if (!keyValuePairs[namespaceName].ContainsKey(className))
                    {
                        keyValuePairs[namespaceName][className] = new List<string>();
                    }

                    var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();
                    foreach (var methodNode in methodNodes)
                    {
                        string methodName = methodNode.Identifier.Text;
                        keyValuePairs[namespaceName][className].Add(methodName);
                    }
                }
            }

            var globalClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                    .Where(cls => cls.Parent is not NamespaceDeclarationSyntax);

            foreach (var globalClass in globalClasses)
            {
                string className = globalClass.Identifier.Text;
                if (!keyValuePairs.ContainsKey("<global namespace>"))
                {
                    keyValuePairs["<global namespace>"] = new Dictionary<string, List<string>>();
                }

                if (!keyValuePairs["<global namespace>"].ContainsKey(className))
                {
                    keyValuePairs["<global namespace>"][className] = new List<string>();
                }

                var methodNodes = globalClass.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (var methodNode in methodNodes)
                {
                    string methodName = methodNode.Identifier.Text;
                    keyValuePairs["<global namespace>"][className].Add(methodName);
                }
            }

            return new NamespaceMap30Nov2024(keyValuePairs);
        }
    }
}

