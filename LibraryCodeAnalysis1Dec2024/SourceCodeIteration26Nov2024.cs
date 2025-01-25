namespace LibraryCodeAnalysis1Dec2024
{
    public class SourceCodeIteration26Nov2024
    {
        public int Number { get; set; }
        public List<SourceCode30Nov2024> SourceCodes { get; set; } = new List<SourceCode30Nov2024>();
        public bool IsCompiled { get; set; } = false;
        public bool CompilationSuccess { get; set; } = false;

        /// <summary>
        /// CompilationErrors
        /// </summary>
        public string DiagnosticResults { get; set; } = string.Empty;

        public string ExecutionOutput { get; set; } = string.Empty;

        public KnowledgeBase30Nov2024 KnowledgeBase;



        public bool HasNoClassProgram { get; set; }
        public bool HasNoMain { get; set; }
    }
}
