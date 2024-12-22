using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IFormattable4
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code_original = @"using System;

public class Person : IFormattable
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
        return ToString(""G"", null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        switch (format)
        {
            case null:
            case ""G"":
                return $""{Name} ({Age} years old)"";
            case ""N"":
                return Name;
            case ""A"":
                return Age.ToString();
            default:
                throw new FormatException($""The {format} format specifier is not supported."");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a Person object
        Person person = new Person(""Alice"", 30);

        // Use different formats with string interpolation
        Console.WriteLine($""{person:N}""); // Displays only the name
        Console.WriteLine($""{person:A}""); // Displays only the age
        Console.WriteLine($""{person}"");   // Displays name and age
    }
}";

            /*
            The public 'Main' method was not found in the 'Program' class.
            The 'Main' method was not found in the 'Program' class.
            */


            string code_modified1 = @"using System;

public class Person : IFormattable
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
        return ToString(""G"", null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        switch (format)
        {
            case null:
            case ""G"":
                return $""{Name} ({Age} years old)"";
            case ""N"":
                return Name;
            case ""A"":
                return Age.ToString();
            default:
                throw new FormatException($""The {format} format specifier is not supported."");
        }
    }
}

class Program
{
    public static void Main(string[] args)
    {
        // Create a Person object
        Person person = new Person(""Alice"", 30);

        // Use different formats with string interpolation
        Console.WriteLine($""{person:N}""); // Displays only the name
        Console.WriteLine($""{person:A}""); // Displays only the age
        Console.WriteLine($""{person}"");   // Displays name and age
    }
}";

            // Create a syntax tree from the code
            var syntaxTree = CSharpSyntaxTree.ParseText(code_original);
            //var syntaxTree = CSharpSyntaxTree.ParseText(code_modified1);

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
            using (var ms = new MemoryStream())
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

                ms.Seek(0, SeekOrigin.Begin);

                // Load the compiled assembly
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType("Program");

                if (type == null)
                {
                    Console.WriteLine("The 'Program' class was not found in the compiled assembly.");
                }

                var method0 = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static); // This works for the modified version.
                var method1 = type.GetMethod("Main", BindingFlags.Static);
                var method2 = type.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static); // This works for the original version.

                if (method0 == null)
                {
                    Console.WriteLine("The public 'Main' method was not found in the 'Program' class. ");
                }
                else
                {
                    method0.Invoke(null, new object[] { new string[0] });
                }

                if (method1 == null)
                {
                    Console.WriteLine("The 'Main' method was not found in the 'Program' class. ");
                }
                else
                {
                    method1.Invoke(null, new object[] { new string[0] });
                }

                if (method2 == null)
                {
                    Console.WriteLine("The non-public 'Main' method was not found. ");
                }
                else
                {
                    method2.Invoke(null, new object[] { new string[0] });
                }

            }

            /*
            Alice
            30
            Alice (30 years old)
            The 'Main' method was not found in the 'Program' class.
            */

            /*
            The public 'Main' method was not found in the 'Program' class.
            The 'Main' method was not found in the 'Program' class.
            Alice
            30
            Alice (30 years old)
            */

            Console.ReadLine();
        }
    }
}
