using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleCodeAnalysis3Nov2024
{
    public class CodeCompiler3Nov2024
    {
        private readonly List<MetadataReference> _references_old0 = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

        private readonly List<MetadataReference> _references_old1 = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
            MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
            MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
        };

        private readonly List<MetadataReference> _references = new()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
            MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
            MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
            MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
        };

        public void ProcessAttempts(SourceCodeIteration sourceCodeIteration)
        {
            CompileAndExecute(sourceCodeIteration);
        }


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
                    Execute_new(sourceCodeIteration, sourceCode.Code);
                }
            }
            else if (sourceCodeIteration.SourceCodes.Count > 1)
            {
                Console.WriteLine("sourceCodeIteration.SourceCodes.Count > 1");
            }
        }


        private void Execute_new(SourceCodeIteration sourceCodeIteration, string userCode)
        {
            //Console.WriteLine($"userCode: {userCode}");

            // Remove Console.ReadKey() calls from the user code
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);
            var root = syntaxTree.GetRoot();

            var rewriter = new ConsoleReadKeyRemovalRewriter();
            var modifiedRoot = rewriter.Visit(root);

            // Convert the modified syntax tree back to source code
            string modifiedCode = modifiedRoot.ToFullString();

            //Console.WriteLine($"modifiedCode: {modifiedCode}");

            // Recompile the modified code
            var updatedSyntaxTree = CSharpSyntaxTree.ParseText(modifiedCode);
            //ms.Seek(0, SeekOrigin.Begin);
            var compilation = CSharpCompilation.Create(
                "ModifiedAssembly",
                new[] { updatedSyntaxTree },
                _references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            // Emit the assembly to a memory stream
            using (var newMs = new MemoryStream())
            {
                var emitResult = compilation.Emit(newMs);
                if (!emitResult.Success)
                {
                    //    sourceCodeIteration.DiagnosticResults = emitResult.Diagnostics.ToString();
                    return;
                }

                //newMs.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(newMs.ToArray());

                var type = assembly.GetType("Program");
                if (type == null)
                {
                    //    sourceCodeIteration.HasNoClassProgram = true;
                    return;
                }

                var method = type.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (method == null)
                {
                    //    sourceCodeIteration.HasNoMain = true;
                    return;
                }

                // Redirect console output
                var originalConsoleOut = Console.Out;
                var originalConsoleIn = Console.In;
                using (var stringWriter = new StringWriter())
                {
                    // Simulate console input (e.g., for Console.ReadLine)
                    var simulatedInput = "Sample input for Console.ReadLine\n";
                    using (var stringReader = new StringReader(simulatedInput))
                    {
                        try
                        {
                            Console.SetOut(stringWriter);
                            Console.SetIn(stringReader);

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
                            // Restore original console input and output
                            Console.SetOut(originalConsoleOut);
                            Console.SetIn(originalConsoleIn);
                        }
                    }
                }
            }
        }

    }
}
