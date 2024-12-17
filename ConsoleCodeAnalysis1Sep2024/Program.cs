using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = @"
        using System;

        public class DynamicClass
        {
            public string SayHello(string name)
            {
                return $""Hello, {name}!"";
            }
        }";

            // Create a syntax tree from the code
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Define the compilation options and references
            var references = new[] {
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
                    };

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

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
                var type = assembly.GetType("DynamicClass");
                var method = type.GetMethod("SayHello");

                // Invoke the method
                var instance = Activator.CreateInstance(type);
                var output = method.Invoke(instance, new object[] { "Roslyn" });
                Console.WriteLine(output); // Output: Hello, Roslyn!

                /*Hello, Roslyn!*/

                Console.ReadLine();
            }
        }
    }
}
