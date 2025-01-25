using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace LibraryCodeAnalysis11Nov2024
{
    public class CodeCompiler
    {
        private readonly MetadataReferenceManager8Nov2024 _metadataManager;

        public CodeCompiler(MetadataReferenceManager8Nov2024 metadataManager)
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

        public CompilationResult Compile(string sourceCode1, string sourceCode2)
        {
            var syntaxTree1 = CSharpSyntaxTree.ParseText(sourceCode1);
            var syntaxTree2 = CSharpSyntaxTree.ParseText(sourceCode2);

            var compilation = CSharpCompilation.Create(
                "MergedAssembly",
                new[] { syntaxTree1, syntaxTree2 },
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


        public CompilationResult Compile(string sourceCode1, string sourceCode2, string sourceCode3)
        {
            var syntaxTree1 = CSharpSyntaxTree.ParseText(sourceCode1);
            var syntaxTree2 = CSharpSyntaxTree.ParseText(sourceCode2);
            var syntaxTree3 = CSharpSyntaxTree.ParseText(sourceCode3);

            var compilation = CSharpCompilation.Create(
                "MergedAssembly",
                new[] { syntaxTree1, syntaxTree2, syntaxTree3 },
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
