using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis4Sep2024_ICloneable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // DeepSeek
            string code = @"using System;

            public class Person : ICloneable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // Implement the Clone method to return a deep copy of the Person object
                public object Clone()
                {
                    // Create a new Person object and copy the properties to it
                    return new Person(Name, Age);
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
                    // Create a Person object
                    var originalPerson = new Person(""Alice"", 30);

                    // Clone the Person object
                    var clonedPerson = (Person)originalPerson.Clone();

                    // Modify the clone
                    clonedPerson.Name = ""Bob"";
                    clonedPerson.Age = 25;

                    // Print both the original and cloned Person objects
                    Console.WriteLine(""Original Person: "" + originalPerson);
                    Console.WriteLine(""Cloned Person: "" + clonedPerson);
                }
            }";

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Get the root SyntaxNode of the SyntaxTree
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all MethodDeclarationSyntax nodes
            var methodDeclarations = root.DescendantNodes()
                                         .OfType<MethodDeclarationSyntax>();

            Console.WriteLine("Methods found in the code snippet:");

            // Print method names
            foreach (var method in methodDeclarations)
            {
                Console.WriteLine($"- {method.Identifier.Text}");
            }

            /*
            Methods found in the code snippet:
            - Clone
            - ToString
            - Main
            */

            Console.ReadLine();
        }
    }
}
