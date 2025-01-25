using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis30Sep2024
namespace ConsoleCodeAnalysis22Nov2024
{
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

            CodeAnalyzer.AnalyzeCode(code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: Person
                Method: ToString()
              Class: PersonNameComparer
                Method: Compare()
              Class: Program
                Method: Main() 
            */
        }
    }
}
