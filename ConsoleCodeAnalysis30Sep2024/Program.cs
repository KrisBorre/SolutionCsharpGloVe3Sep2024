using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis30Sep2024
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

            // Parse the code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Create a Compilation object
            Compilation compilation = CSharpCompilation.Create("CodeAnalysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            // Get the SemanticModel from the Compilation
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Specify the class name to search for
            string className = "PersonNameComparer";

            // Find all identifiers in the syntax tree
            var identifiers = syntaxTree.GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>();

            Console.WriteLine($"References to the class '{className}':");

            // Check each identifier to see if it refers to the specified class
            foreach (var identifier in identifiers)
            {
                var symbol = semanticModel.GetSymbolInfo(identifier).Symbol;

                if (symbol != null && symbol.Kind == SymbolKind.NamedType && symbol.Name == className)
                {
                    var location = identifier.GetLocation();
                    Console.WriteLine($"- Found reference at line {location.GetLineSpan().StartLinePosition.Line + 1}, column {location.GetLineSpan().StartLinePosition.Character + 1}");
                }
            }

            /*
            References to the class 'PersonNameComparer':
            - Found reference at line 50, column 21
            - Found reference at line 50, column 55
            */

            Console.ReadLine();
        }
    }
}
