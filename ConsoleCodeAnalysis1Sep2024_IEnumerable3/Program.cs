using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IEnumerable3
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code = @"using System;
            using System.Collections;
            using System.Collections.Generic;

            public class NumberCollection : IEnumerable<int>
            {
                private List<int> numbers = new List<int>();

                // Add a number to the collection
                public void Add(int number)
                {
                    numbers.Add(number);
                }

                // Implement the GetEnumerator method to return an enumerator
                public IEnumerator<int> GetEnumerator()
                {
                    return numbers.GetEnumerator();
                }

                // Explicit non-generic interface implementation
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            class Program
            {
                static void Main(string[] args)
                {
                    // Create a NumberCollection
                    NumberCollection numbers = new NumberCollection();

                    // Add numbers to the collection
                    numbers.Add(1);
                    numbers.Add(2);
                    numbers.Add(3);
                    numbers.Add(4);

                    // Iterate over the numbers with foreach
                    foreach (int number in numbers)
                    {
                        Console.WriteLine(number);
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
            throws System.NullReferenceException
            */

            Console.ReadLine();
        }
    }
}
