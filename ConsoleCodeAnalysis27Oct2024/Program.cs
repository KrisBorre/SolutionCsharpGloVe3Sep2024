using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace ConsoleCodeAnalysis27Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class CodeExecutor
    {
        public static string ExecuteCode(string userCode)
        {
            // Create syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);

            // Define required assembly references
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
            };

            // Create Roslyn compilation
            var compilation = CSharpCompilation.Create(
                "UserCodeAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = result.Diagnostics
                    .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString());
                return "Compilation Failed:\n" + string.Join("\n", errors);
            }

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            // Execute the compiled assembly
            var entryPoint = assembly.EntryPoint;
            if (entryPoint == null)
            {
                return "No entry point found in the provided code.";
            }

            var output = CaptureConsoleOutput(() => entryPoint.Invoke(null, new object[] { new string[0] }));
            return output;
        }

        private static string CaptureConsoleOutput(Action action)
        {
            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                var originalOutput = Console.Out;
                Console.SetOut(writer);
                try
                {
                    action();
                }
                finally
                {
                    Console.SetOut(originalOutput);
                }
            }
            return output.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // User-provided code
            string userCode = @"
            using System;

            public class Program
            {
                public static void Main(string[] args)
                {
                    Console.WriteLine(""Hello, World!"");
                }
            }";

            Console.WriteLine("Compiling and executing user code...");
            string result = CodeExecutor.ExecuteCode(userCode);

            Console.WriteLine("Execution Result:");
            Console.WriteLine(result);

            /*
            Compiling and executing user code...
            Execution Result:
            Hello, World! 
            */

            Console.ReadLine();
        }
    }
}
