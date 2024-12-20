using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis4Sep2024_IComparable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // DeepSeek
            string code = @"
using System;
            using System.Collections.Generic;

            public class Person : IComparable<Person>
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                public int CompareTo(Person other)
                {
                    // Compare the ages of two Person objects
                    return Age.CompareTo(other.Age);
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
                    var people = new List<Person>
                    {
                        new Person(""Alice"", 20),
                        new Person(""Bob"", 30),
                        new Person(""Charlie"", 25)
                    };

                    // Sort the list of people by age
                    people.Sort();

                    // Print the sorted list
                    foreach (var person in people)
                    {
                        Console.WriteLine(person);
                    }
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
            - CompareTo
            - ToString
            - Main
            */

            Console.ReadLine();
        }
    }
}
