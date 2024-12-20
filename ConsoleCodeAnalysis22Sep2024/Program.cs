using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis22Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            // Define two separate C# code snippets
            string code1 = @"
namespace ExampleNamespace
{
    class ExampleClass
    {
        void Method1() { int x = 1; }
    }
}";

            string code2 = @"
namespace ExampleNamespace
{
    class AnotherClass
    {
        void Method2() { string y = ""test""; }
    }
}";

            // Parse the code snippets into syntax trees
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(code1);
            SyntaxTree tree2 = CSharpSyntaxTree.ParseText(code2);

            // Add references to essential .NET assemblies
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // mscorlib or System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location) // System.Linq
            };

            // Create a compilation unit and add the syntax trees
            Compilation compilation = CSharpCompilation.Create("ExampleCompilation")
                .AddSyntaxTrees(tree1, tree2)
                .AddReferences(references);

            // Get diagnostics from the compilation
            var diagnostics = compilation.GetDiagnostics();

            // Output diagnostics, if any
            Console.WriteLine("Compilation Diagnostics:");
            foreach (var diagnostic in diagnostics)
            {
                Console.WriteLine($"  ID: {diagnostic.Id}");
                Console.WriteLine($"  Severity: {diagnostic.Severity}");
                Console.WriteLine($"  Message: {diagnostic.GetMessage()}");
                Console.WriteLine($"  Location: {diagnostic.Location}");
                Console.WriteLine();
            }

            // Inform the user if no errors/warnings were found
            if (!diagnostics.Any())
            {
                Console.WriteLine("No diagnostics found. Compilation is error-free.");
            }

            /*
            Compilation Diagnostics:
              ID: CS5001
              Severity: Error
              Message: Program does not contain a static 'Main' method suitable for an entry point
              Location: None

              ID: CS0219
              Severity: Warning
              Message: The variable 'x' is assigned but its value is never used
              Location: SourceFile([93..94))

              ID: CS0219
              Severity: Warning
              Message: The variable 'y' is assigned but its value is never used
              Location: SourceFile([96..97)) 
            */
        }
    }
}
