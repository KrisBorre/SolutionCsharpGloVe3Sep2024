using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis1Nov2024
{
    public class CodeCompiler1Nov2024
    {
        private readonly List<MetadataReference> _references = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };


        /// <param name="sourceCodeIteration"></param>
        public void ProcessAttempts(SourceCodeIteration sourceCodeIteration)
        {
            CompileAndExecute(sourceCodeIteration);
        }


        /// <param name="sourceCodeIteration"></param>
        private void CompileAndExecute(SourceCodeIteration sourceCodeIteration)
        {
            if (sourceCodeIteration.SourceCodes.Count == 1)
            {
                SourceCode1Nov2024 sourceCode = sourceCodeIteration.SourceCodes[0];

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
            else if (sourceCodeIteration.SourceCodes.Count == 2)
            {

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

                    var parameters = method.GetParameters();

                    if (parameters.Length == 0)
                    {
                        // Main method has no parameters
                        method.Invoke(null, null);
                    }
                    else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]))
                    {
                        // Main method accepts a string[] parameter
                        method.Invoke(null, new object[] { new string[0] });
                    }
                    else
                    {
                        sourceCodeIteration.ExecutionOutput = "Unsupported Main method signature.";
                    }

                    // Capture the console output
                    sourceCodeIteration.ExecutionOutput = stringWriter.ToString();
                }
                catch (TargetInvocationException ex)
                {
                    sourceCodeIteration.ExecutionOutput = $"Error during execution: {ex.InnerException?.Message}";
                }
                catch (Exception ex)
                {
                    sourceCodeIteration.ExecutionOutput = $"Unexpected error: {ex.Message}";
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
