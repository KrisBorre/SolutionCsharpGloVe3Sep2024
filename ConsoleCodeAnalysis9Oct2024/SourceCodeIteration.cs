namespace ConsoleCodeAnalysis9Oct2024
{
    public class SourceCodeIteration
    {
        public int Number { get; set; }
        public List<SourceCode> SourceCodes { get; } = new List<SourceCode>();
        public bool IsCompiled { get; set; }
        public bool CompilationSuccess { get; set; }
        public string DiagnosticResults { get; set; } = string.Empty;
        public bool HasNoClassProgram { get; set; }
        public bool HasNoMain { get; set; }
        public string ExecutionOutput { get; set; } = string.Empty;
    }
}
