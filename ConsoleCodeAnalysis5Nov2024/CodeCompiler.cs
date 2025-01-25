using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis5Nov2024
{
    public class CodeCompiler
    {
        private readonly MetadataReferenceManager _metadataManager;

        public CodeCompiler(MetadataReferenceManager metadataManager)
        {
            _metadataManager = metadataManager;
        }

        public CompilationResult Compile(string sourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
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
    }

}
