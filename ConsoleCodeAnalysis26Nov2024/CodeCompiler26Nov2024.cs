using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis26Nov2024
{
    public class CodeCompiler26Nov2024
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;

        public CodeCompiler26Nov2024(MetadataReferenceManager8Nov2024 metadataManager)
        {
            _metadataManager = metadataManager;
        }
       

        public CompilationResult Compile(params string[] sourceCodes)
        {
            if (sourceCodes == null || sourceCodes.Length == 0)
            {
                return null;
            }

            var syntaxTrees = new SyntaxTree[sourceCodes.Length];
            for (int i = 0; i < sourceCodes.Length; i++)
            {
                syntaxTrees[i] = CSharpSyntaxTree.ParseText(sourceCodes[i]);
            }

            var compilation = CSharpCompilation.Create(
                "MergedAssembly",
                syntaxTrees,
                _metadataManager.GetReferences(),
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using (var ms = new MemoryStream())
            {
                var emitResult = compilation.Emit(ms);
                if (!emitResult.Success)
                {
                    var diagnostics = string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.ToString()));
                    return new CompilationResult(false, diagnostics, null);
                }

                ms.Seek(0, SeekOrigin.Begin);
                return new CompilationResult(true, null, Assembly.Load(ms.ToArray()));
            }
        }


        public CompilationResult Compile(SyntaxTree syntaxTree)
        {
            var compilation = CSharpCompilation.Create(
                "Assembly",
                new[] { syntaxTree }, // Wrap the SyntaxTree in an array
                _metadataManager.GetReferences(),
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using (var ms = new MemoryStream())
            {
                var emitResult = compilation.Emit(ms);
                if (!emitResult.Success)
                {
                    // Gather diagnostics into a string
                    var diagnostics = string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.ToString()));
                    return new CompilationResult(false, diagnostics, null);
                }

                // Reset memory stream position to the beginning
                ms.Seek(0, SeekOrigin.Begin);
                return new CompilationResult(true, null, Assembly.Load(ms.ToArray()));
            }
        }

    }

}
