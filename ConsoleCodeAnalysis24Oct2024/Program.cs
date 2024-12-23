using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis24Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Step 1: Define your code snippets
            string code1 = @"using System;
            using System.Collections.Generic;

            public class TemperatureSensor : IObservable<int>
            {
                private List<IObserver<int>> observers = new List<IObserver<int>>();

                public IDisposable Subscribe(IObserver<int> observer)
                {
                    if (!observers.Contains(observer))
                        observers.Add(observer);

                    return new Unsubscriber(observers, observer);
                }

                public void PublishTemperature(int temperature)
                {
                    foreach (var observer in observers)
                    {
                        if (temperature >= 30)
                            observer.OnNext(temperature);
                        else
                            observer.OnError(new Exception(""Temperature too low""));
                    }
                }

                private class Unsubscriber : IDisposable
                {
                    private List<IObserver<int>> _observers;
                    private IObserver<int> _observer;

                    public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
                    {
                        this._observers = observers;
                        this._observer = observer;
                    }

                    public void Dispose()
                    {
                        if (_observer != null && _observers.Contains(_observer))
                            _observers.Remove(_observer);
                    }
                }
            }";

            string code2 = @"using System;

            public class TemperatureDisplay : IObserver<int>
            {
                public void OnNext(int temperature)
                {
                    Console.WriteLine($""Temperature: {temperature}"");
                }

                public void OnError(Exception error)
                {
                    Console.WriteLine(error.Message);
                }

                public void OnCompleted()
                {
                    Console.WriteLine(""Sensor readings complete"");
                }
            }";

            string code3 = @"using System;

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var sensor = new TemperatureSensor();
                    var display = new TemperatureDisplay();

                    using (sensor.Subscribe(display))
                    {
                        sensor.PublishTemperature(25);
                        sensor.PublishTemperature(35);
                        sensor.PublishTemperature(40);
                    }
                }
            }";

            // Step 2: Parse the code snippets into syntax trees
            var syntaxTrees = new[]
            {
                CSharpSyntaxTree.ParseText(code1),
                CSharpSyntaxTree.ParseText(code2),
                CSharpSyntaxTree.ParseText(code3)
            };

            // Step 3: Add references
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
            };

            // Step 4: Compile into a single assembly
            var compilation = CSharpCompilation.Create(
                "MergedAssembly",
                syntaxTrees,
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Step 5: Emit the assembly to memory or disk
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (result.Success)
                {
                    Console.WriteLine("Compilation successful!");

                    // Load and execute the assembly
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var programType = assembly.GetType("Program");
                    var mainMethod = programType.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                    mainMethod.Invoke(null, new object[] { new string[0] });
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                }
            }

            /*
            Compilation successful!
            Temperature too low
            Temperature: 35
            Temperature: 40
            */

            Console.ReadLine();
        }
    }
}
