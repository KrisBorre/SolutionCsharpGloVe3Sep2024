namespace LibraryCodeAnalysis11Nov2024
{
    public class SourceCodeIteration
    {
        public int Number { get; set; }
        public List<SourceCode8Nov2024> SourceCodes { get; set; } = new List<SourceCode8Nov2024>();
        public bool IsCompiled { get; set; } = false;
        public bool CompilationSuccess { get; set; } = false;


        /// <summary>
        /// CompilationErrors
        /// </summary>
        public string DiagnosticResults { get; set; } = string.Empty;


        public bool HasNoClassProgram { get; set; }
        public bool HasNoMain { get; set; }
        public string ExecutionOutput { get; set; } = string.Empty;
    }
}
