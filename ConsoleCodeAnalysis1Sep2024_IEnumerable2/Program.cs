using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IEnumerable2
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code1 = @"using System;
            using System.Collections;
            using System.Collections.Generic;

            public class NumberCollection : IEnumerable<int>
            {
                private List<int> numbers = new List<int>();

                // This method adds a number to the list
                public void Add(int number)
                {
                    numbers.Add(number);
                }

                // This method returns an enumerator that can iterate over the list
                public IEnumerator<int> GetEnumerator()
                {
                    return numbers.GetEnumerator();
                }

                // This method returns an enumerator that can iterate over the list
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }";

            string code2 = @"public static void Main(string[] args)
            {
                var numberCollection = new NumberCollection();
                numberCollection.Add(1);
                numberCollection.Add(2);
                numberCollection.Add(3);

                foreach (var number in numberCollection)
                {
                    Console.WriteLine(number);
                }
            }";

            string code = code1 + Environment.NewLine + code2 + Environment.NewLine;


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
            (27,1): error CS8803: Top-level statements must precede namespace and type declarations.
            (27,1): error CS0106: The modifier 'public' is not valid for this item
            (27,20): warning CS7022: The entry point of the program is global code; ignoring 'Main(string[])' entry point.
            (27,20): warning CS8321: The local function 'Main' is declared but never used
            */

            Console.ReadLine();
        }
    }
}
