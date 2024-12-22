using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis4Oct2024
{
    public class CodeCompiler
    {
        private readonly List<MetadataReference> _references = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

        public void ProcessAttempts(Run run)
        {
            foreach (var project in run.Projects)
            {
                foreach (var attempt in project.Attempts)
                {
                    foreach (var sourceCode in attempt.SourceCodes)
                    {
                        CompileAndExecute(sourceCode, attempt);
                    }
                }
            }
        }

        private void CompileAndExecute(SourceCode sourceCode, Attempt attempt)
        {
            // Parse the code
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode.Code);

            // Compile the code
            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                _references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                attempt.IsCompiled = true;
                attempt.CompilationSuccess = result.Success;

                if (!result.Success)
                {
                    attempt.DiagnosticResults = string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString()));
                    return;
                }

                // Execute the compiled code
                Execute(ms, attempt);
            }
        }

        private void Execute(MemoryStream ms, Attempt attempt)
        {
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var type = assembly.GetType("Program");
            if (type == null)
            {
                attempt.HasNoClassProgram = true;
                return;
            }

            var method = type.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
            {
                attempt.HasNoMain = true;
                return;
            }

            try
            {
                var output = method.Invoke(null, new object[] { new string[0] });
                attempt.ExecutionOutput = output?.ToString() ?? "No output returned by Main.";
                Console.WriteLine(attempt.ExecutionOutput);
            }
            catch (TargetInvocationException ex)
            {
                attempt.ExecutionOutput = $"Error during execution: {ex.InnerException?.Message}";
            }
        }
    }
}
