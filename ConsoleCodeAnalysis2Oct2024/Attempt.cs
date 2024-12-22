namespace ConsoleCodeAnalysis2Oct2024
{
    internal class Attempt
    {
        public int Number;
        public List<string> SourceCodes = new List<string>();

        public bool IsCompiled = false;
        public bool CompilationSuccess = false;
        public string DiagnosticResults;
        public string ExecutionOutput;
        public bool hasNoMain = true;
        public bool hasNoClassProgram = true;

        public Attempt()
        {

        }

    }
}
