using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IEquatable3
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

            public class Person : IEquatable<Person>
            {
                public string Name { get; set; }
                public int Age { get; set; }

                // Constructor
                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // Implement the Equals method to compare Person objects based on Name and Age
                public bool Equals(Person other)
                {
                    if (other == null) return false;

                    return this.Name == other.Name && this.Age == other.Age;
                }

                // Override the Equals method to compare Person objects based on Name and Age
                public override bool Equals(object obj)
                {
                    if (obj == null) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != this.GetType()) return false;
                    return Equals(obj as Person);
                }

                // Override the GetHashCode method to provide a hash code based on Name and Age
                public override int GetHashCode()
                {
                    return Name.GetHashCode() ^ Age.GetHashCode();
                }

                public override string ToString()
                {
                    return $""Name: {Name}, Age: {Age}"";
                }
            }

            class Program
            {
                static void Main(string[] args)
                {
                    // Create two Person objects
                    Person person1 = new Person(""Alice"", 30);
                    Person person2 = new Person(""Alice"", 30);

                    // Check equality between two Person objects
                    bool areEqual = person1.Equals(person2);

                    // Print the result
                    Console.WriteLine($""Are the persons equal? {areEqual}"");
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
            System.NullReferenceException
            Problem is probably that the code that DeepSeek generated isn't a public class with a public method.
            */

            Console.ReadLine();
        }
    }
}
