using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis6Sep2024_IDisposable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
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

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Collect diagnostics from the SyntaxTree
            IEnumerable<Diagnostic> diagnostics = syntaxTree.GetDiagnostics();

            Console.WriteLine("Syntax Errors:");
            Console.WriteLine("----------------------------------");

            // Report all syntax errors
            foreach (var diagnostic in diagnostics)
            {
                if (diagnostic.Severity == DiagnosticSeverity.Error) // Only process errors
                {
                    Console.WriteLine($"ID: {diagnostic.Id}");
                    Console.WriteLine($"Message: {diagnostic.GetMessage()}");
                    Console.WriteLine($"Severity: {diagnostic.Severity}");
                    Console.WriteLine($"Location: Line {diagnostic.Location.GetLineSpan().StartLinePosition.Line + 1}, Column {diagnostic.Location.GetLineSpan().StartLinePosition.Character + 1}");
                    Console.WriteLine("----------------------------------");
                }
            }

            if (!diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                Console.WriteLine("No syntax errors found!");
            }

            /*
            Syntax Errors:
            ----------------------------------
            No syntax errors found!
            */

            Console.ReadLine();
        }
    }
}