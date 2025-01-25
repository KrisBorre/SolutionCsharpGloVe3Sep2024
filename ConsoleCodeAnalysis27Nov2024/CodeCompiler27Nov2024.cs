using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis27Nov2024
{
    public class CodeCompiler27Nov2024
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;

        public CodeCompiler27Nov2024(MetadataReferenceManager8Nov2024 metadataManager)
        {
            _metadataManager = metadataManager;
        }


        public CompilationResult Compile(params string[] sourceCodes)
        {
            if (sourceCodes == null || sourceCodes.Length == 0)
            {
                return new CompilationResult(false, "No source code provided.", null);
            }

            var syntaxTrees = new SyntaxTree[sourceCodes.Length];
            for (int i = 0; i < sourceCodes.Length; i++)
            {
                syntaxTrees[i] = CSharpSyntaxTree.ParseText(sourceCodes[i]);
            }

            return CompileInternal(syntaxTrees);
        }


        public CompilationResult Compile(SyntaxTree syntaxTree)
        {
            if (syntaxTree == null)
            {
                return new CompilationResult(false, "Syntax tree is null.", null);
            }

            return CompileInternal(new[] { syntaxTree });
        }


        private CompilationResult CompileInternal(SyntaxTree[] syntaxTrees)
        {
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
    }
}
