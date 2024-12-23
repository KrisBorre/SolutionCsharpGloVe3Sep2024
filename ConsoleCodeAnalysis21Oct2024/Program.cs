using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text;

namespace ConsoleCodeAnalysis21Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // User-provided code
            string userCode = @"
            public class Program
            {
                public static void Main()
                {
                    Console.WriteLine(""Hello, World!"");
                }
            }";

            // Expected output
            string expectedOutput = "Hello, World!";

            // Compile the code
            var syntaxTree = CSharpSyntaxTree.ParseText(userCode);
            var references = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>();

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            );

            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                // Handle compilation errors
                Console.WriteLine("Compilation failed:");
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    Console.WriteLine(diagnostic.ToString());
                }
                return;
            }

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());
            var type = assembly.GetType("Program");
            var method = type?.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

            if (method != null)
            {
                // Execute the method and capture output
                var output = CaptureConsoleOutput(() => method.Invoke(null, null));

                // Compare the output
                if (output.Trim() == expectedOutput)
                {
                    Console.WriteLine("Correct solution!");
                }
                else
                {
                    Console.WriteLine($"Expected: {expectedOutput}, but got: {output}");
                }
            }
            else
            {
                Console.WriteLine("Could not find the 'Main' method.");
            }

            /*
            Compilation failed:
            (6,21): error CS0103: The name 'Console' does not exist in the current context 
            */

            Console.ReadLine();
        }

        /// <summary>
        /// Captures console output while executing an action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The captured console output.</returns>
        static string CaptureConsoleOutput(Action action)
        {
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
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
            return stringBuilder.ToString();
        }
    }
}
