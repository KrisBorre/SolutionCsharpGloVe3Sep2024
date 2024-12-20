using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IFormattable2
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code1 = @"public class Person : IFormattable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // This method provides custom formats for displaying Person information
                public string ToString(string format, IFormatProvider formatProvider)
                {
                    if (format == null) format = ""G""; // default format

                    switch (format.ToUpperInvariant())
                    {
                        case ""N"": // Name only
                            return Name;
                        case ""A"": // Age only
                            return Age.ToString();
                        case ""G"": // Name and Age
                        default:
                            return $""{Name} ({Age})"";
                    }
                }

                // This method provides a default string representation of the Person object
                public override string ToString()
                {
                    return ToString(null, null);
                }
            }";

            string code2 = @"public static void Main(string[] args)
            {
                var person = new Person(""Alice"", 30);

                Console.WriteLine(person.ToString(""N"")); // Outputs: Alice
                Console.WriteLine(person.ToString(""A"")); // Outputs: 30
                Console.WriteLine(person.ToString(""G"")); // Outputs: Alice (30)
                Console.WriteLine($""{person}""); // Outputs: Alice (30) due to the default ToString implementation
            }";

            string code3 = @"";

            string code = code1 + Environment.NewLine + code2 + Environment.NewLine + code3;

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
            (35,1): error CS8803: Top-level statements must precede namespace and type declarations.
            (35,1): error CS0106: The modifier 'public' is not valid for this item
            (1,23): error CS0246: The type or namespace name 'IFormattable' could not be found (are you missing a using directive or an assembly reference?)
            (13,55): error CS0246: The type or namespace name 'IFormatProvider' could not be found (are you missing a using directive or an assembly reference?)
            (39,17): error CS0103: The name 'Console' does not exist in the current context
            (39,42): error CS7036: There is no argument given that corresponds to the required parameter 'formatProvider' of 'Person.ToString(string, IFormatProvider)'
            (40,17): error CS0103: The name 'Console' does not exist in the current context
            (40,42): error CS7036: There is no argument given that corresponds to the required parameter 'formatProvider' of 'Person.ToString(string, IFormatProvider)'
            (41,17): error CS0103: The name 'Console' does not exist in the current context
            (41,42): error CS7036: There is no argument given that corresponds to the required parameter 'formatProvider' of 'Person.ToString(string, IFormatProvider)'
            (42,17): error CS0103: The name 'Console' does not exist in the current context
            (35,20): warning CS7022: The entry point of the program is global code; ignoring 'Main(string[])' entry point.
            (35,20): warning CS8321: The local function 'Main' is declared but never used
            */

            Console.ReadLine();
        }
    }
}
