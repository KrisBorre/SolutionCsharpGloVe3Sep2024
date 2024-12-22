using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis9Oct2024
{
    public class CodeCompiler9Oct2024
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
                foreach (var conversation in project.Conversations)
                {
                    foreach (var sourceCodeIteration in conversation.SourceCodeIterations)
                    {
                        foreach (var sourceCode in sourceCodeIteration.SourceCodes)
                        {
                            CompileAndExecute(sourceCode, sourceCodeIteration);
                        }
                    }
                }
            }
        }

        private void CompileAndExecute(SourceCode sourceCode, SourceCodeIteration sourceCodeIteration)
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
                sourceCodeIteration.IsCompiled = true;
                sourceCodeIteration.CompilationSuccess = result.Success;

                if (!result.Success)
                {
                    sourceCodeIteration.DiagnosticResults = string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString()));
                    return;
                }

                // Execute the compiled code
                Execute(ms, sourceCodeIteration);
            }
        }

        private void Execute(MemoryStream ms, SourceCodeIteration sourceCodeIteration)
        {
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var type = assembly.GetType("Program");
            if (type == null)
            {
                sourceCodeIteration.HasNoClassProgram = true;
                return;
            }

            var method = type.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
            {
                sourceCodeIteration.HasNoMain = true;
                return;
            }

            // Redirect console output
            var originalConsoleOut = Console.Out;
            using (var stringWriter = new StringWriter())
            {
                try
                {
                    Console.SetOut(stringWriter);

                    // Invoke the 'Main' method
                    method.Invoke(null, new object[] { new string[0] });

                    // Capture the console output
                    sourceCodeIteration.ExecutionOutput = stringWriter.ToString();
                }
                catch (TargetInvocationException ex)
                {
                    sourceCodeIteration.ExecutionOutput = $"Error during execution: {ex.InnerException?.Message}";
                }
                finally
                {
                    // Restore original console output
                    Console.SetOut(originalConsoleOut);
                }
            }
        }

    }
}
