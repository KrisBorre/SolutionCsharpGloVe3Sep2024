using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis16Sep2024_IEquatable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string sourceCode = @"using System;

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

            // Parse the source code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Create a Compilation object for the syntax tree
            Compilation compilation = CSharpCompilation.Create("Analysis")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            // Get the SemanticModel for the syntax tree
            SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Get the root of the syntax tree
            SyntaxNode root = syntaxTree.GetRoot();

            // Find all method declarations in the code
            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            foreach (var methodDeclaration in methodDeclarations)
            {
                // Get the symbol for the method using the SemanticModel
                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

                if (methodSymbol != null)
                {
                    // Check if the method is public and virtual
                    bool isPublic = methodSymbol.DeclaredAccessibility == Accessibility.Public;
                    bool isVirtual = methodSymbol.IsVirtual;

                    Console.WriteLine($"Method: {methodSymbol.Name}");
                    Console.WriteLine($"  IsPublic: {isPublic}");
                    Console.WriteLine($"  IsVirtual: {isVirtual}");
                }
            }

            /*
            Method: Equals
              IsPublic: True
              IsVirtual: False
            Method: Equals
              IsPublic: True
              IsVirtual: False
            Method: GetHashCode
              IsPublic: True
              IsVirtual: False
            Method: ToString
              IsPublic: True
              IsVirtual: False
            Method: Main
              IsPublic: True
              IsVirtual: False
            */

            Console.ReadLine();
        }
    }
}