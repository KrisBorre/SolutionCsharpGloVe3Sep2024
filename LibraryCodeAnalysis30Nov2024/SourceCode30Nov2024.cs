using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LibraryCodeAnalysis30Nov2024
{
    public class SourceCode30Nov2024
    {
        public string Code { get; set; }

        //public bool IsCompiled { get; set; } = false;
        //public bool CompilationSuccess { get; set; } = false;


        //public string CompilationErrors { get; set; } = string.Empty;


        public bool HasNoClassProgram { get; set; } = true;
        public bool HasNoMain { get; set; } = true;

        //public string ExecutionOutput { get; set; } = string.Empty;

        public SyntaxTree SyntaxTree { get; set; }

        public NamespaceMap30Nov2024 NamespaceMap { get; set; }


        public SourceCode30Nov2024(string code)
        {
            this.Code = code;

            this.SyntaxTree = CSharpSyntaxTree.ParseText(code);

            NamespaceMapExtractor26Nov2024 namespaceMapExtractor = new NamespaceMapExtractor26Nov2024(this.SyntaxTree);
            this.NamespaceMap = namespaceMapExtractor.BuildNamespaceMap();

            foreach (var ns in this.NamespaceMap.KeyValuePairs)
            {
                foreach (var cls in ns.Value)
                {
                    string className = cls.Key;

                    if (className == "Program")
                    {
                        HasNoClassProgram = false;

                        foreach (var method in cls.Value)
                        {
                            if (method == "Main")
                            {
                                HasNoMain = false;
                            }
                        }
                    }

                }
            }
        }
    }
}
