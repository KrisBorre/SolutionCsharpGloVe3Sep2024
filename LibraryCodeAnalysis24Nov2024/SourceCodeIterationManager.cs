namespace LibraryCodeAnalysis24Nov2024
{
    public class SourceCodeIterationManager
    {
        public List<SourceCodeIteration> Iterations = new List<SourceCodeIteration>();
        public int PreviousIterationIndex;

        public int CurrentIterationIndex;

        public int NextIterationIndex;

        public SourceCodeIterationManager()
        {

        }

        public bool CreateNewKnowledgeBase_old(CodeProcessing14Nov2024 codeProcessing)
        {
            bool success = false;

            CodeAnalyzer23Nov2024 codeAnalyzer = new CodeAnalyzer23Nov2024();

            List<string> codeBlocks = new List<string>();
            bool lets_continue = true;

            for (int i = Iterations.Count - 1; i >= 0 && lets_continue; i--)
            {
                SourceCodeIteration sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count && lets_continue; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];

                    var namespaceMap = codeAnalyzer.GenerateNamespaceMap(sourceCode.Code);

                    bool containsMain = false;

                    foreach (var namespace1 in namespaceMap)
                    {
                        Console.WriteLine($"Namespace: {namespace1.Key}");
                        foreach (var keyValuePair1 in namespace1.Value)
                        {
                            if (keyValuePair1.Key == "Program")
                            {
                                foreach (var method1 in keyValuePair1.Value)
                                {
                                    if (method1 == "Main")
                                    {
                                        containsMain = true;
                                    }
                                }
                            }
                        }
                    }

                    if (containsMain)
                    {
                        codeBlocks.Add(sourceCode.Code);
                        lets_continue = false;
                    }
                }
            }



            for (int i = Iterations.Count - 1; i >= 0; i--)
            {
                SourceCodeIteration sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];

                    var namespaceMap = codeAnalyzer.GenerateNamespaceMap(sourceCode.Code);

                    // Skip classes with a method Main
                    bool containsMain = false;

                    foreach (var namespace1 in namespaceMap)
                    {
                        foreach (var keyValuePair1 in namespace1.Value)
                        {
                            if (keyValuePair1.Key == "Program")
                            {
                                foreach (var method1 in keyValuePair1.Value)
                                {
                                    if (method1 == "Main")
                                    {
                                        containsMain = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // Skip if the class is already added to codeBlocks
                    if (containsMain || codeBlocks.Contains(sourceCode.Code))
                    {
                        continue;
                    }

                    if (true)
                    {
                        List<string> attempt = new List<string>(codeBlocks);
                        attempt.Add(sourceCode.Code);

                        if (codeProcessing.CodeBlocksCompile(attempt.ToArray()))
                        {
                            codeBlocks.Add(sourceCode.Code);
                        }

                    }
                }
            }

            success = codeBlocks.Count > 0;

            if (success) Console.WriteLine($"codeBlocks.Count: {codeBlocks.Count}");

            return success;
        }


        public bool CreateNewKnowledgeBase(CodeProcessing14Nov2024 codeProcessing)
        {
            bool success = false;

            CodeAnalyzer23Nov2024 codeAnalyzer = new CodeAnalyzer23Nov2024();
            List<string> codeBlocks = new List<string>();
            Dictionary<string, HashSet<string>> knowledgeBase = new Dictionary<string, HashSet<string>>(); // Track classes by namespace

            bool lets_continue = true;

            // First pass: Add main-containing classes
            for (int i = Iterations.Count - 1; i >= 0 && lets_continue; i--)
            {
                SourceCodeIteration sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count && lets_continue; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];
                    var namespaceMap = codeAnalyzer.GenerateNamespaceMap(sourceCode.Code);

                    bool containsMain = false;
                    foreach (var namespaceEntry in namespaceMap)
                    {
                        foreach (var classEntry in namespaceEntry.Value)
                        {
                            if (classEntry.Value.Contains("Main"))
                            {
                                containsMain = true;

                                // Add to knowledgeBase
                                if (!knowledgeBase.ContainsKey(namespaceEntry.Key))
                                {
                                    knowledgeBase[namespaceEntry.Key] = new HashSet<string>();
                                }
                                knowledgeBase[namespaceEntry.Key].Add(classEntry.Key);
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
                SourceCodeIteration sourceCodeIteration = Iterations[i];

                for (int j = 0; j < sourceCodeIteration.SourceCodes.Count; j++)
                {
                    SourceCode8Nov2024 sourceCode = sourceCodeIteration.SourceCodes[j];
                    var namespaceMap = codeAnalyzer.GenerateNamespaceMap(sourceCode.Code);

                    bool alreadyAdded = false;

                    foreach (var namespaceEntry in namespaceMap)
                    {
                        if (knowledgeBase.ContainsKey(namespaceEntry.Key))
                        {
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                if (knowledgeBase[namespaceEntry.Key].Contains(classEntry.Key))
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

                        // Update knowledgeBase with new classes
                        foreach (var namespaceEntry in namespaceMap)
                        {
                            if (!knowledgeBase.ContainsKey(namespaceEntry.Key))
                            {
                                knowledgeBase[namespaceEntry.Key] = new HashSet<string>();
                            }
                            foreach (var classEntry in namespaceEntry.Value)
                            {
                                knowledgeBase[namespaceEntry.Key].Add(classEntry.Key);
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
