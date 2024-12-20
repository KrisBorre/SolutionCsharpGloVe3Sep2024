using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IEnumerable1
{
    /// <summary>
    /// ChatGPT; modified
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code = @"
using System;
            using System.Collections;
            using System.Collections.Generic;

            public class NumberCollection : IEnumerable<int>
            {
                private readonly List<int> _numbers = new List<int>();

                public void Add(int number)
                {
                    _numbers.Add(number);
                }

                public IEnumerator<int> GetEnumerator()
                {
                    return _numbers.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    var numbers = new NumberCollection();

                    numbers.Add(1);
                    numbers.Add(2);
                    numbers.Add(3);

                    foreach (var number in numbers)
                    {
                        Console.WriteLine(number);
                    }
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
                var type = assembly.GetType("Program");
                var method = type.GetMethod("Main");

                // Invoke the method
                var instance = Activator.CreateInstance(type);
                var output = method.Invoke(instance, new object[] { "Roslyn" });
                Console.WriteLine(output);

                /*
                (38,33): error CS0012: The type 'Object' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
                (38,25): error CS0012: The type 'Decimal' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'. 
                */

                Console.ReadLine();
            }
        }
    }
}
