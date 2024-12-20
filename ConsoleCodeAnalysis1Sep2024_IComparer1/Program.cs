using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IComparer1
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
            using System.Collections.Generic;

            public class Person
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                public override string ToString()
                {
                    return $""Name: {Name}, Age: {Age}"";
                }
            }

            public class PersonNameComparer : IComparer<Person>
            {
                // Implement the Compare method to sort Person objects by Name alphabetically
                public int Compare(Person x, Person y)
                {
                    if (x == y)
                        return 0;
                    if (x == null)
                        return -1;
                    if (y == null)
                        return 1;

                    return string.Compare(x.Name, y.Name);
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    // Create a list of Person objects
                    List<Person> people = new List<Person>
                    {
                        new Person(""Alice"", 30),
                        new Person(""Charlie"", 25),
                        new Person(""Bob"", 35)
                    };

                    // Create a PersonNameComparer object
                    PersonNameComparer comparer = new PersonNameComparer();

                    // Sort the list of Person objects using the custom comparer
                    people.Sort(comparer);

                    // Print the sorted list of Person objects
                    foreach (Person person in people)
                    {
                        Console.WriteLine(person);
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
            Name: Alice, Age: 30
            Name: Bob, Age: 35
            Name: Charlie, Age: 25
            */

            Console.ReadLine();
        }
    }
}
