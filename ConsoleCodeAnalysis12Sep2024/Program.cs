using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ConsoleCodeAnalysis12Sep2024
{
    /// <summary>
    /// ChatGPT; Not what I expected!
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
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

            // Create a compilation unit and add the syntax trees
            Compilation compilation = CSharpCompilation.Create("ExampleCompilation")
                .AddSyntaxTrees(tree1, tree2);

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
              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Object' is not defined or imported
              Location: SourceFile([67..79))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Object' is not defined or imported
              Location: SourceFile([67..79))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Void' is not defined or imported
              Location: SourceFile([112..116))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Void' is not defined or imported
              Location: SourceFile([112..116))

              ID: CS5001
              Severity: Error
              Message: Program does not contain a static 'Main' method suitable for an entry point
              Location: None

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.String' is not defined or imported
              Location: SourceFile([129..135))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.String' is not defined or imported
              Location: SourceFile([140..146))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Int32' is not defined or imported
              Location: SourceFile([129..132))

              ID: CS0518
              Severity: Error
              Message: Predefined type 'System.Int32' is not defined or imported
              Location: SourceFile([137..138))

              ID: CS1729
              Severity: Error
              Message: 'object' does not contain a constructor that takes 0 arguments
              Location: SourceFile([67..79))

              ID: CS1729
              Severity: Error
              Message: 'object' does not contain a constructor that takes 0 arguments
              Location: SourceFile([67..79)) 
            */

            Console.ReadLine();
        }
    }
}