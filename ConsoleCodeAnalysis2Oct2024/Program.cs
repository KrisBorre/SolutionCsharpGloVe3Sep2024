using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis2Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run run = new Run();

            //Project project3 = new Project();
            //run.Projects.Add(project3);
            //project3.Name = "IFormattable";
            //Attempt attempt3_1 = new Attempt();
            //project3.Attempts.Add(attempt3_1);

            //{
            //    // DeepSeek
            //    string code3 = @"using System;

            //            public class Person : IFormattable
            //            {
            //                public string Name { get; set; }
            //                public int Age { get; set; }

            //                public Person(string name, int age)
            //                {
            //                    Name = name;
            //                    Age = age;
            //                }

            //                // Implement the ToString method to provide custom formats for displaying Person information
            //                public override string ToString()
            //                {
            //                    return ToString(""F"", null);
            //                }

            //                public string ToString(string format, IFormatProvider formatProvider)
            //                {
            //                    switch (format.";

            //    attempt3_1.SourceCodes.Add(code3);
            //}


            //Attempt attempt3_4 = new Attempt();
            //project3.Attempts.Add(attempt3_4);

            //{
            //    // DeepSeek
            //    string code3 = @"using System;

            //public class Person : IFormattable
            //{
            //    public string Name { get; set; }
            //    public int Age { get; set; }

            //    public Person(string name, int age)
            //    {
            //        Name = name;
            //        Age = age;
            //    }

            //    public override string ToString()
            //    {
            //        return ToString(""G"", null);
            //    }

            //    public string ToString(string format, IFormatProvider formatProvider)
            //    {
            //        switch (format)
            //        {
            //            case null:
            //            case ""G"":
            //                return $""{Name} ({Age} years old)"";
            //            case ""N"":
            //                return Name;
            //            case ""A"":
            //                return Age.ToString();
            //            default:
            //                throw new FormatException($""The {format} format specifier is not supported."");
            //        }
            //    }
            //}

            //class Program
            //{
            //    static void Main(string[] args)
            //    {
            //        // Create a Person object
            //        Person person = new Person(""Alice"", 30);

            //        // Use different formats with string interpolation
            //        Console.WriteLine($""{person:N}""); // Displays only the name
            //        Console.WriteLine($""{person:A}""); // Displays only the age
            //        Console.WriteLine($""{person}"");   // Displays name and age
            //    }
            //}";
            //    attempt3_4.SourceCodes.Add(code3);
            //}

            Project project1 = new Project();
            run.Projects.Add(project1);
            project1.Name = "ICloneable";
            Attempt attempt1 = new Attempt();
            project1.Attempts.Add(attempt1);
            attempt1.Number = 1;

            // DeepSeek
            string code1 = @"using System;

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

            attempt1.SourceCodes.Add(code1);

            //Project project2 = new Project();
            //run.Projects.Add(project2);
            //project2.Name = "IObservable";
            //Attempt attempt2_1 = new Attempt();
            //attempt2_1.Number = 1;
            //project2.Attempts.Add(attempt2_1);

            //// DeepSeek
            //string code = @"using System;
            //using System.Collections.Generic;
            //using System.Reactive.Linq;
            //using System.Reactive.Subjects;

            //// The observable class that publishes temperature readings
            //public class TemperatureSensor : IObservable<int>
            //{
            //    private Subject<int> temperatureSubject = new Subject<int>();

            //    // Publish the temperature readings
            //    public IDisposable Subscribe(IObserver<int> observer)
            //    {
            //        return temperatureSubject.Subscribe(observer);
            //    }

            //    // Simulate temperature readings
            //    public void SimulateTemperatureReadings()
            //    {
            //        for (int i = 0; i < 5; i++)
            //        {
            //            int temperature = new Random().Next(0, 100);
            //            temperatureSubject.OnNext(temperature);
            //            System.Threading.Thread.Sleep(1000); // Simulate delay
            //        }
            //        temperatureSubject.OnCompleted();
            //    }
            //}

            //// The observer class that receives and displays the readings
            //public class TemperatureDisplay : IObserver<int>
            //{
            //    public void OnCompleted()
            //    {
            //        Console.WriteLine(""Temperature readings completed."");
            //    }

            //    public void OnError(Exception error)
            //    {
            //        Console.WriteLine($""Error occurred: {error.Message}"");
            //    }

            //    public void OnNext(int value)
            //    {
            //        Console.WriteLine($""Current temperature: {value}°C"");
            //    }
            //}

            //public class Program
            //{
            //    public static void Main(string[] args)
            //    {
            //        // Create a TemperatureSensor and a TemperatureDisplay
            //        TemperatureSensor sensor = new TemperatureSensor();
            //        TemperatureDisplay display = new TemperatureDisplay();

            //        // Subscribe the display to the sensor
            //        using (sensor.Subscribe(display))
            //        {
            //            // Simulate temperature readings
            //            sensor.SimulateTemperatureReadings();
            //        }
            //    }
            //}";

            //attempt2_1.SourceCodes.Add(code);


            foreach (var project in run.Projects)
            {
                foreach (var attempt in project.Attempts)
                {
                    foreach (var sourceCode in attempt.SourceCodes)
                    {
                        // Create a syntax tree from the code
                        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

                        // Add references, including System.Runtime
                        var references = new[] {
                                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // mscorlib/System.Private.CoreLib
                                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections
                                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
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

                            attempt.IsCompiled = true;
                            attempt.CompilationSuccess = result.Success;

                            if (!result.Success)
                            {
                                foreach (var diagnostic in result.Diagnostics)
                                {
                                    Console.WriteLine(diagnostic.ToString());
                                    attempt.DiagnosticResults += diagnostic.ToString();
                                }
                            }

                            if (result.Success)
                            {
                                ms.Seek(0, SeekOrigin.Begin);

                                // Load the compiled assembly from the memory stream
                                var assembly = Assembly.Load(ms.ToArray());

                                // Get the 'Program' type
                                var type = assembly.GetType("Program");

                                if (type == null)
                                {
                                    attempt.hasNoClassProgram = true; // The 'Program' class was not found in the compiled assembly.                                    
                                }
                                else
                                {
                                    // Get the 'Main' method
                                    var method = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                                    //var method = type.GetMethod("Main", BindingFlags.Static);

                                    if (method == null)
                                    {
                                        attempt.hasNoMain = true; // The 'Main' method was not found in the 'Program' class.                                        
                                    }
                                    else
                                    {
                                        // Invoke the 'Main' method and handle the output
                                        try
                                        {
                                            var output = method.Invoke(null, new object[] { new string[0] });

                                            // If the output needs to be stored in attempt.ExecutionOutput
                                            attempt.ExecutionOutput = output?.ToString() ?? "No output returned by Main.";
                                        }
                                        catch (TargetInvocationException ex)
                                        {
                                            // Handle any exceptions that occur within the invoked method
                                            throw new Exception($"An error occurred while executing 'Main': {ex.InnerException?.Message}", ex.InnerException);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*
            (22,36): error CS1001: Identifier expected
            (22,36): error CS1026: ) expected
            (22,36): error CS1514: { expected
            (22,36): error CS1513: } expected
            (22,36): error CS1513: } expected
            (22,36): error CS1513: } expected
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (22,36): warning CS1522: Empty switch block
            (20,31): error CS0161: 'Person.ToString(string, IFormatProvider)': not all code paths return a value
            (3,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (4,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (9,25): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,63): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            */

            /*
            (22,48): error CS1001: Identifier expected
            (22,48): error CS1026: ) expected
            (22,48): error CS1514: { expected
            (22,48): error CS1513: } expected
            (22,48): error CS1513: } expected
            (22,48): error CS1513: } expected
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (22,48): warning CS1522: Empty switch block
            (20,43): error CS0161: 'Person.ToString(string, IFormatProvider)': not all code paths return a value
            Original Person: Name: Alice, Age: 30
            Cloned Person: Name: Bob, Age: 25
            (3,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (4,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (9,25): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,63): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            */

            Console.ReadLine();
        }
    }
}
