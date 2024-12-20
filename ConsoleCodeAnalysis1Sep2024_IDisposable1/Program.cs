using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IDisposable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code = @"
using System;
using System.IO;

            public class FileManager : IDisposable
            {
                private bool disposed = false;

                public void WriteToFile(string filePath, string content)
                {
                    // Check if the object has been disposed
                    if (disposed)
                    {
                        throw new ObjectDisposedException(GetType().Name);
                    }

                    // Write to the file
                    File.WriteAllText(filePath, content);
                }

                public void Dispose()
                {
                    // Check if the object has been disposed
                    if (disposed) return;

                    // Release managed resources

                    // Set the flag to true to indicate that the object has been disposed
                    disposed = true;

                    // Suppress finalization for this object
                    GC.SuppressFinalize(this);
                }

                // Finalizer, which is called when the garbage collector is about to reclaim the memory used by the object.
                // It's important to note that the garbage collector does not guarantee when this will be called.
                // In this case, we don't have any finalizable resources to clean up, so we can leave it empty.
                ~FileManager()
                {
                    // Call the Dispose method
                    Dispose();
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    // Use a using statement to ensure the resource is released
                    using (var fileManager = new FileManager())
                    {
                        // Write to a file
                        fileManager.WriteToFile(""test.txt"", ""Hello, world!"");
                    }
                }
            }";

            // Create a syntax tree from the code
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Add references, including System.Runtime
            var references = new[] {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),                  // mscorlib/System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),               // System.Console
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location),        // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)        // System.Runtime
            };

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Compile the code into memory
            using (var ms = new System.IO.MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                    return;
                }

                ms.Seek(0, System.IO.SeekOrigin.Begin);

                // Load the compiled assembly
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType("Program");
                var method = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

                // Invoke the method
                method.Invoke(null, new object[] { new string[0] });
            }


            /*
            The file is generated !
            The file test.txt contains Hello, world!
            */

            Console.ReadLine();
        }
    }
}
