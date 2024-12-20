using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IObservable1
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
            (3,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (4,26): error CS0234: The type or namespace name 'Reactive' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (9,25): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,63): error CS0246: The type or namespace name 'Subject<>' could not be found (are you missing a using directive or an assembly reference?)
            */

            Console.ReadLine();
        }
    }
}
