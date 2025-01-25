using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ConsoleCodeAnalysis28Nov2024
{
    public class KnowledgeBaseCreator28Nov2024
    {
        public List<SourceCodeIteration26Nov2024> Iterations = new List<SourceCodeIteration26Nov2024>();
        private readonly CodeProcessing26Nov2024 _codeProcessing;

        public KnowledgeBaseCreator28Nov2024(CodeProcessing26Nov2024 codeProcessing)
        {
            _codeProcessing = codeProcessing;
        }

        public KnowledgeBase26Nov2024 CreateKnowledgeBase()
        {
            var knowledgeBase = new KnowledgeBase26Nov2024();
            var codeBlocks = new List<string>();
            var namespaceDictionary = new Dictionary<string, HashSet<string>>();

            // Add classes with Main method
            if (!AddMainContainingClasses(codeBlocks, namespaceDictionary))
            {
                knowledgeBase.Success = false;
                return knowledgeBase;
            }

            // Add other valid classes
            AddOtherValidClasses(codeBlocks, namespaceDictionary);

            knowledgeBase.Success = codeBlocks.Count > 0;
            if (knowledgeBase.Success) Console.WriteLine($"codeBlocks.Count: {codeBlocks.Count}");
            knowledgeBase.CodeBlocks = codeBlocks;
            return knowledgeBase;
        }

        private bool AddMainContainingClasses(List<string> codeBlocks, Dictionary<string, HashSet<string>> namespaceDictionary)
        {
            foreach (var iteration in Iterations.AsEnumerable().Reverse())
            {
                foreach (var sourceCode in iteration.SourceCodes)
                {
                    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode.Code);
                    var namespaceMap = ExtractNamespaceMap(syntaxTree);

                    if (TryAddMainClass(namespaceMap, namespaceDictionary))
                    {
                        codeBlocks.Add(sourceCode.Code);
                        return true;
                    }
                }
            }
            return false;
        }

        // TODO: Add all classes that are used in the Main method.
        private void AddOtherValidClasses(List<string> codeBlocks, Dictionary<string, HashSet<string>> namespaceDictionary)
        {
            foreach (var iteration in Iterations.AsEnumerable().Reverse())
            {
                foreach (var sourceCode in iteration.SourceCodes)
                {
                    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode.Code);
                    var namespaceMap = ExtractNamespaceMap(syntaxTree);

                    if (IsAlreadyAdded(namespaceMap, namespaceDictionary))
                        continue;

                    // Validate by attempting compilation
                    var attempt = new List<string>(codeBlocks) { sourceCode.Code };
                    if (_codeProcessing.CodeBlocksCompile(attempt.ToArray()))
                    {
                        codeBlocks.Add(sourceCode.Code);
                        UpdateNamespaceDictionary(namespaceMap, namespaceDictionary);
                    }
                }
            }
        }

        private Dictionary<string, Dictionary<string, List<string>>> ExtractNamespaceMap(SyntaxTree syntaxTree)
        {
            var namespaceMapExtractor = new NamespaceMapExtractor26Nov2024(syntaxTree);
            return namespaceMapExtractor.ExtractNamespaceMap();
        }

        private bool TryAddMainClass(
            Dictionary<string, Dictionary<string, List<string>>> namespaceMap,
            Dictionary<string, HashSet<string>> namespaceDictionary)
        {
            foreach (var (namespaceName, classes) in namespaceMap)
            {
                foreach (var (className, methods) in classes)
                {
                    if (methods.Contains("Main"))
                    {
                        if (!namespaceDictionary.ContainsKey(namespaceName))
                        {
                            namespaceDictionary[namespaceName] = new HashSet<string>();
                        }
                        namespaceDictionary[namespaceName].Add(className);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsAlreadyAdded(
            Dictionary<string, Dictionary<string, List<string>>> namespaceMap,
            Dictionary<string, HashSet<string>> namespaceDictionary)
        {
            foreach (var (namespaceName, classes) in namespaceMap)
            {
                if (namespaceDictionary.TryGetValue(namespaceName, out var existingClasses))
                {
                    if (classes.Keys.Any(existingClasses.Contains))
                        return true;
                }
            }
            return false;
        }

        private void UpdateNamespaceDictionary(
            Dictionary<string, Dictionary<string, List<string>>> namespaceMap,
            Dictionary<string, HashSet<string>> namespaceDictionary)
        {
            foreach (var (namespaceName, classes) in namespaceMap)
            {
                if (!namespaceDictionary.ContainsKey(namespaceName))
                {
                    namespaceDictionary[namespaceName] = new HashSet<string>();
                }

                foreach (var className in classes.Keys)
                {
                    namespaceDictionary[namespaceName].Add(className);
                }
            }
        }
    }
}
