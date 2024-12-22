namespace ConsoleCodeAnalysis4Oct2024
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
            Original Person: Name: Alice, Age: 30
            Cloned Person: Name: Bob, Age: 25
            No output returned by Main.
            Alice
            30
            Alice (30 years old)
            No output returned by Main.
            Project: ICloneable
              Attempt #1:
                Compiled: True
                Diagnostics:
                Execution Output: No output returned by Main.
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
                Execution Output: No output returned by Main.
            */

            Console.ReadLine();
        }
    }

}
