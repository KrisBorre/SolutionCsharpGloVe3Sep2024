namespace LibraryCodeAnalysis1Dec2024
{
    public class KnowledgeBaseCreator1Dec2024
    {
        public List<SourceCodeIteration26Nov2024> Iterations = new List<SourceCodeIteration26Nov2024>();
        private readonly CodeProcessing1Dec2024 _codeProcessing;

        public KnowledgeBaseCreator1Dec2024(CodeProcessing1Dec2024 codeProcessing)
        {
            _codeProcessing = codeProcessing;
        }

        private KnowledgeBase30Nov2024 knowledgeBase;

        private SourceCode30Nov2024 mainSourceCode;

        private List<string> addedClassNames = new List<string>();

        public KnowledgeBase30Nov2024 CreateKnowledgeBase()
        {
            this.knowledgeBase = new KnowledgeBase30Nov2024();

            // Add classes with Main method
            if (!AddMainContainingClasses())
            {
                this.knowledgeBase.Success = false;
                return this.knowledgeBase;
            }

            // Add other valid classes
            AddClasses();


            this.knowledgeBase.Success = _codeProcessing.CodeBlocksCompile(this.knowledgeBase.CodeBlocks.ToArray());

            if (this.knowledgeBase.Success) Console.WriteLine($"knowledgeBase.CodeBlocks.Count: {this.knowledgeBase.CodeBlocks.Count}");
            if (!this.knowledgeBase.Success) Console.WriteLine($"Count: {this.knowledgeBase.CodeBlocks.Count}");

            return this.knowledgeBase;
        }

        private bool AddMainContainingClasses()
        {
            foreach (var iteration in Iterations.AsEnumerable().Reverse())
            {
                foreach (var sourceCode in iteration.SourceCodes)
                {
                    if (!sourceCode.HasNoMain)
                    {
                        this.knowledgeBase.SourceCodes.Add(sourceCode);
                        this.mainSourceCode = sourceCode;
                        this.knowledgeBase.MainSourceCode = mainSourceCode;

                        foreach (var ns in sourceCode.NamespaceMap.KeyValuePairs)
                        {
                            foreach (var cls in ns.Value)
                            {
                                string className = cls.Key;

                                this.addedClassNames.Add(className);
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }


        private void AddClasses()
        {
            foreach (var iteration in Iterations.AsEnumerable().Reverse())
            {
                foreach (var sourceCode in iteration.SourceCodes)
                {
                    if (sourceCode.HasNoMain)
                    {

                        var namespaceMap = sourceCode.NamespaceMap;

                        bool add = false;

                        foreach (var ns in namespaceMap.KeyValuePairs)
                        {
                            foreach (var cls in ns.Value)
                            {
                                string className = cls.Key;

                                if (!this.addedClassNames.Contains(className))
                                {
                                    //Console.WriteLine("add");
                                    add = true;

                                }
                            }
                        }


                        if (add)
                        {
                            this.knowledgeBase.SourceCodes.Add(sourceCode);

                            foreach (var ns in namespaceMap.KeyValuePairs)
                            {
                                foreach (var cls in ns.Value)
                                {
                                    string className = cls.Key;

                                    this.addedClassNames.Add(className);

                                    Console.WriteLine($"{className} added");
                                }
                            }
                        }

                    }
                }
            }
        }


    }
}
