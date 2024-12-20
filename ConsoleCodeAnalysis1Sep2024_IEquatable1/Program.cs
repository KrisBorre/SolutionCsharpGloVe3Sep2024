using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IEquatable1
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

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // Implement the Equals method to compare Person objects based on Name and Age
                public bool Equals(Person other)
                {
                    if (other == null)
                    {
                        return false;
                    }

                    // Compare the Name and Age properties of the Person objects
                    return Name == other.Name && Age == other.Age;
                }

                public override bool Equals(object obj)
                {
                    // Call the type-specific Equals method
                    if (obj is Person otherPerson)
                    {
                        return Equals(otherPerson);
                    }

                    return false;
                }

                public override int GetHashCode()
                {
                    // Combine the hash codes of the Name and Age properties
                    return Name.GetHashCode() ^ Age.GetHashCode();
                }

                public override string ToString()
                {
                    return $""Name: {Name}, Age: {Age}"";
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    // Create two Person objects
                    var person1 = new Person(""Alice"", 30);
                    var person2 = new Person(""Alice"", 30);

                    // Check equality between the two Person objects
                    bool areEqual = person1.Equals(person2);

                    // Print the result
                    Console.WriteLine($""Are the two persons equal? {areEqual}"");
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
            Are the two persons equal? True
            */

            Console.ReadLine();
        }
    }
}
