namespace ConsoleCodeAnalysis26Nov2024
{
    public class KnowledgeBaseCreator26Nov2024
    {
        public List<SourceCodeIteration26Nov2024> Iterations = new List<SourceCodeIteration26Nov2024>();
        private CodeProcessing26Nov2024 codeProcessing;


        public KnowledgeBaseCreator26Nov2024(CodeProcessing26Nov2024 codeProcessing)
        {
            this.codeProcessing = codeProcessing;
        }

        public KnowledgeBase26Nov2024 CreateKnowledgeBase()
        {
            KnowledgeBase26Nov2024 knowledgeBase = new KnowledgeBase26Nov2024();

            bool success = false;

            NamespaceMapGenerator26Nov2024 namespaceMapGenerator = new NamespaceMapGenerator26Nov2024();

            List<string> codeBlocks = new List<string>();
            Dictionary<string, HashSet<string>> namespaceDictionary = new Dictionary<string, HashSet<string>>();

            bool lets_continue = true;

            // First pass: Add main-containing classes
            for (int i = Iterations.Count - 1; i >= 0 && lets_continue; i--)
            {
                SourceCodeIteration26Nov2024 sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count && lets_continue; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];

                    var namespaceMap = namespaceMapGenerator.GenerateNamespaceMap(sourceCode.Code);

                    bool containsMain = false;
                    foreach (var namespaceEntry in namespaceMap)
                    {
                        foreach (var classEntry in namespaceEntry.Value)
                        {
                            if (classEntry.Value.Contains("Main"))
                            {
                                containsMain = true;

                                if (!namespaceDictionary.ContainsKey(namespaceEntry.Key))
                                {
                                    namespaceDictionary[namespaceEntry.Key] = new HashSet<string>();
                                }
                                namespaceDictionary[namespaceEntry.Key].Add(classEntry.Key);
                                break;
                            }
                        }
                        if (containsMain) break;
                    }

                    if (containsMain)
                    {
                        codeBlocks.Add(sourceCode.Code);
                        lets_continue = false;
                    }
                }
            }

            // Second pass: Add other valid classes
            for (int i = Iterations.Count - 1; i >= 0; i--)
            {
                SourceCodeIteration26Nov2024 sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];
                    var namespaceMap = namespaceMapGenerator.GenerateNamespaceMap(sourceCode.Code);

                    bool alreadyAdded = false;

                    foreach (var namespaceEntry in namespaceMap)
                    {
                        if (namespaceDictionary.ContainsKey(namespaceEntry.Key))
                        {
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                if (namespaceDictionary[namespaceEntry.Key].Contains(classEntry.Key))
                                {
                                    alreadyAdded = true;
                                    break;
                                }
                            }
                        }
                        if (alreadyAdded) break;
                    }

                    if (alreadyAdded) continue;

                    // Validate by attempting compilation
                    List<string> attempt = new List<string>(codeBlocks);
                    attempt.Add(sourceCode.Code);

                    if (this.codeProcessing.CodeBlocksCompile(attempt.ToArray()))
                    {
                        codeBlocks.Add(sourceCode.Code);

                        foreach (var namespaceEntry in namespaceMap)
                        {
                            if (!namespaceDictionary.ContainsKey(namespaceEntry.Key))
                            {
                                namespaceDictionary[namespaceEntry.Key] = new HashSet<string>();
                            }
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                namespaceDictionary[namespaceEntry.Key].Add(classEntry.Key);
                            }
                        }
                    }
                }
            }

            success = codeBlocks.Count > 0;

            if (success) Console.WriteLine($"codeBlocks.Count: {codeBlocks.Count}");

            knowledgeBase.Success = success;
            if (success)
            {
                knowledgeBase.CodeBlocks = codeBlocks;
            }

            return knowledgeBase;
        }


        public bool CreateKnowledgeBase_old(CodeProcessing26Nov2024 codeProcessing)
        {
            bool success = false;

            NamespaceMapGenerator26Nov2024 namespaceMapGenerator = new NamespaceMapGenerator26Nov2024();

            List<string> codeBlocks = new List<string>();
            Dictionary<string, HashSet<string>> namespaceDictionary = new Dictionary<string, HashSet<string>>();

            bool lets_continue = true;

            // First pass: Add main-containing classes
            for (int i = Iterations.Count - 1; i >= 0 && lets_continue; i--)
            {
                SourceCodeIteration26Nov2024 sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count && lets_continue; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];

                    var namespaceMap = namespaceMapGenerator.GenerateNamespaceMap(sourceCode.Code);

                    bool containsMain = false;
                    foreach (var namespaceEntry in namespaceMap)
                    {
                        foreach (var classEntry in namespaceEntry.Value)
                        {
                            if (classEntry.Value.Contains("Main"))
                            {
                                containsMain = true;

                                if (!namespaceDictionary.ContainsKey(namespaceEntry.Key))
                                {
                                    namespaceDictionary[namespaceEntry.Key] = new HashSet<string>();
                                }
                                namespaceDictionary[namespaceEntry.Key].Add(classEntry.Key);
                                break;
                            }
                        }
                        if (containsMain) break;
                    }

                    if (containsMain)
                    {
                        codeBlocks.Add(sourceCode.Code);
                        lets_continue = false;
                    }
                }
            }

            // Second pass: Add other valid classes
            for (int i = Iterations.Count - 1; i >= 0; i--)
            {
                SourceCodeIteration26Nov2024 sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];
                    var namespaceMap = namespaceMapGenerator.GenerateNamespaceMap(sourceCode.Code);

                    bool alreadyAdded = false;

                    foreach (var namespaceEntry in namespaceMap)
                    {
                        if (namespaceDictionary.ContainsKey(namespaceEntry.Key))
                        {
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                if (namespaceDictionary[namespaceEntry.Key].Contains(classEntry.Key))
                                {
                                    alreadyAdded = true;
                                    break;
                                }
                            }
                        }
                        if (alreadyAdded) break;
                    }

                    if (alreadyAdded) continue;

                    // Validate by attempting compilation
                    List<string> attempt = new List<string>(codeBlocks);
                    attempt.Add(sourceCode.Code);

                    if (codeProcessing.CodeBlocksCompile(attempt.ToArray()))
                    {
                        codeBlocks.Add(sourceCode.Code);

                        foreach (var namespaceEntry in namespaceMap)
                        {
                            if (!namespaceDictionary.ContainsKey(namespaceEntry.Key))
                            {
                                namespaceDictionary[namespaceEntry.Key] = new HashSet<string>();
                            }
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                namespaceDictionary[namespaceEntry.Key].Add(classEntry.Key);
                            }
                        }
                    }
                }
            }

            success = codeBlocks.Count > 0;

            if (success) Console.WriteLine($"codeBlocks.Count: {codeBlocks.Count}");

            return success;
        }

    }
}
