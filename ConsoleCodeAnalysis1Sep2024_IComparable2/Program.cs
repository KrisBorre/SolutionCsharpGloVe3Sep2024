using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IComparable2
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code1 = @"            public class Person : IComparable<Person>
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // This method compares the age of two Person objects
                public int CompareTo(Person other)
                {
                    if (other == null)
                        return 1;

                    // A negative integer, zero, or a positive integer
                    // as this object is less than, equal to, or greater than other.
                    return Age.CompareTo(other.Age);
                }
            }";

            string code2 = @"            public static void Main(string[] args)
            {
                var people = new List<Person>
                {
                    new Person(""Alice"", 30),
                    new Person(""Bob"", 20),
                    new Person(""Charlie"", 40)
                };

                // Sort the list of people by their Age
                people.Sort();

                foreach (var person in people)
                {
                    Console.WriteLine($""{person.Name} is {person.Age}"");
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
            (23,13): error CS8803: Top-level statements must precede namespace and type declarations.
            (23,13): error CS0106: The modifier 'public' is not valid for this item
            (1,35): error CS0246: The type or namespace name 'IComparable<>' could not be found (are you missing a using directive or an assembly reference?)
            (25,34): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (37,21): error CS0103: The name 'Console' does not exist in the current context
            (23,32): warning CS7022: The entry point of the program is global code; ignoring 'Main(string[])' entry point.
            (23,32): warning CS8321: The local function 'Main' is declared but never used
            */

            Console.ReadLine();
        }
    }
}
