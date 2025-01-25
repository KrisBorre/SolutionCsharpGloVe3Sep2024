using System.Reflection;

namespace LibraryCodeAnalysis14Nov2024
{
    public class CompilationResult
    {
        public bool Success { get; }
        public string Diagnostics { get; }
        public Assembly Assembly { get; }

        public CompilationResult(bool success, string diagnostics, Assembly assembly)
        {
            Success = success;
            Diagnostics = diagnostics;
            Assembly = assembly;
        }
    }
}
