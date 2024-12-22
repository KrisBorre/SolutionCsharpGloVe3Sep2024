namespace ConsoleCodeAnalysis5Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var run = new Run();


            var project1 = new Project { Name = "ICloneable" };
            project1.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"using System;

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
            }")
                }
            });
            run.Projects.Add(project1);


            var project2 = new Project { Name = "IObservable" };
            project2.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"using System;
            using System.Collections.Generic;
            using System.Reactive.Linq;
            using System.Reactive.Subjects;

            // The observable class that publishes temperature readings
            public class TemperatureSensor : IObservable<int>
            {
                private Subject<int> temperatureSubject = new Subject<int>();

                // Publish the temperature readings
                public IDisposable Subscribe(IObserver<int> observer)
                {
                    return temperatureSubject.Subscribe(observer);
                }

                // Simulate temperature readings
                public void SimulateTemperatureReadings()
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int temperature = new Random().Next(0, 100);
                        temperatureSubject.OnNext(temperature);
                        System.Threading.Thread.Sleep(1000); // Simulate delay
                    }
                    temperatureSubject.OnCompleted();
                }
            }

            // The observer class that receives and displays the readings
            public class TemperatureDisplay : IObserver<int>
            {
                public void OnCompleted()
                {
                    Console.WriteLine(""Temperature readings completed."");
                }

                public void OnError(Exception error)
                {
                    Console.WriteLine($""Error occurred: {error.Message}"");
                }

                public void OnNext(int value)
                {
                    Console.WriteLine($""Current temperature: {value}°C"");
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    // Create a TemperatureSensor and a TemperatureDisplay
                    TemperatureSensor sensor = new TemperatureSensor();
                    TemperatureDisplay display = new TemperatureDisplay();

                    // Subscribe the display to the sensor
                    using (sensor.Subscribe(display))
                    {
                        // Simulate temperature readings
                        sensor.SimulateTemperatureReadings();
                    }
                }
            }")
                }
            });
            run.Projects.Add(project2);


            var project3 = new Project { Name = "IFormattable" };
            project3.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"using System;

            public class Person : IFormattable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                // Implement the ToString method to provide custom formats for displaying Person information
                public override string ToString()
                {
                    return ToString(""F"", null);
                }

                public string ToString(string format, IFormatProvider formatProvider)
                {
                    switch (format.")
                }
            });

            project3.Attempts.Add(new Attempt
            {
                Number = 4,
                SourceCodes = {
                    new SourceCode(@"using System;

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
}")
                }
            });
            run.Projects.Add(project3);


            var project4 = new Project { Name = "IEquatable" };
            project4.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"using System;

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
            }")
                }
            });

            project4.Attempts.Add(new Attempt
            {
                Number = 3,
                SourceCodes = {
                    new SourceCode(@"using System;

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
            }")
                }
            });
            run.Projects.Add(project4);


            var project5 = new Project { Name = "IEnumerable" };
            project5.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"
using System;
using System.Collections;
using System.Collections.Generic;

public class NumberCollection : IEnumerable<int>
{
    private readonly List<int> _numbers = new List<int>();

    public void Add(int number)
    {
        _numbers.Add(number);
    }

    public IEnumerator<int> GetEnumerator()
    {
        return _numbers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var numbers = new NumberCollection();

        numbers.Add(1);
        numbers.Add(2);
        numbers.Add(3);

        foreach (var number in numbers)
        {
            Console.WriteLine(number);
        }
    }
}")
                }
            });

            project5.Attempts.Add(new Attempt
            {
                Number = 3,
                SourceCodes = {
                    new SourceCode(@"using System;
            using System.Collections;
            using System.Collections.Generic;

            public class NumberCollection : IEnumerable<int>
            {
                private List<int> numbers = new List<int>();

                // Add a number to the collection
                public void Add(int number)
                {
                    numbers.Add(number);
                }

                // Implement the GetEnumerator method to return an enumerator
                public IEnumerator<int> GetEnumerator()
                {
                    return numbers.GetEnumerator();
                }

                // Explicit non-generic interface implementation
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            class Program
            {
                static void Main(string[] args)
                {
                    // Create a NumberCollection
                    NumberCollection numbers = new NumberCollection();

                    // Add numbers to the collection
                    numbers.Add(1);
                    numbers.Add(2);
                    numbers.Add(3);
                    numbers.Add(4);

                    // Iterate over the numbers with foreach
                    foreach (int number in numbers)
                    {
                        Console.WriteLine(number);
                    }
                }
            }")
                }
            });
            run.Projects.Add(project5);


            var project6 = new Project { Name = "IComparable" };
            project6.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"
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
            }")
                }
            });
            run.Projects.Add(project6);


            // Process and execute the projects
            var compiler = new CodeCompiler();
            compiler.ProcessAttempts(run);

            // Display results
            foreach (var project in run.Projects)
            {
                Console.WriteLine($"Project: {project.Name}");
                foreach (var attempt in project.Attempts)
                {
                    Console.WriteLine($"  Attempt #{attempt.Number}:");
                    Console.WriteLine($"    Compiled: {attempt.CompilationSuccess}");
                    Console.WriteLine($"    Diagnostics: {attempt.DiagnosticResults}");
                    Console.WriteLine($"    Execution Output: {attempt.ExecutionOutput}");
                }
            }

            /*
            Project: ICloneable
              Attempt #1:
                Compiled: True
                Diagnostics:
                Execution Output: Original Person: Name: Alice, Age: 30
            Cloned Person: Name: Bob, Age: 25

            Project: IObservable
              Attempt #1:
                Compiled: False
                Diagnostics: (3,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (4,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (9,25): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,63): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
                Execution Output:
            Project: IFormattable
              Attempt #1:
                Compiled: False
                Diagnostics: (22,36): error CS1001: Identifier expected
            (22,36): error CS1026: ) expected
            (22,36): error CS1514: { expected
            (22,36): error CS1513: } expected
            (22,36): error CS1513: } expected
            (22,36): error CS1513: } expected
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (22,36): warning CS1522: Empty switch block
            (20,31): error CS0161: 'Person.ToString(string, IFormatProvider)': not all code paths return a value
                Execution Output:
              Attempt #4:
                Compiled: True
                Diagnostics:
                Execution Output: Alice
            30
            Alice (30 years old)

            Project: IEquatable
              Attempt #1:
                Compiled: True
                Diagnostics:
                Execution Output: Are the two persons equal? True

              Attempt #3:
                Compiled: True
                Diagnostics:
                Execution Output: Are the persons equal? True

            Project: IEnumerable
              Attempt #1:
                Compiled: True
                Diagnostics:
                Execution Output: 1
            2
            3

              Attempt #3:
                Compiled: True
                Diagnostics:
                Execution Output: 1
            2
            3
            4

            Project: IComparable
              Attempt #1:
                Compiled: True
                Diagnostics:
                Execution Output: Name: Alice, Age: 20
            Name: Charlie, Age: 25
            Name: Bob, Age: 30
            */

            Console.ReadLine();
        }
    }
}
