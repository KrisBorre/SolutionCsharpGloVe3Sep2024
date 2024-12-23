using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis10Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Step 1: Define your code snippets
            string code1 = @"public class TemperatureSensor : IObservable<int>
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

            string code2 = @"public class TemperatureDisplay : IObserver<int>
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

            string code3 = @"public static class Program
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
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // mscorlib/System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
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
            Compilation failed:
            (1,34): error CS0246: The type or namespace name 'IObservable<>' could not be found (are you missing a using directive or an assembly reference?)
            (1,35): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (8,33): error CS0246: The type or namespace name 'Exception' could not be found (are you missing a using directive or an assembly reference?)
            (3,21): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (3,26): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (5,42): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (5,20): error CS0246: The type or namespace name 'IDisposable' could not be found (are you missing a using directive or an assembly reference?)
            (24,42): error CS0246: The type or namespace name 'IDisposable' could not be found (are you missing a using directive or an assembly reference?)
            (26,25): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (26,30): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (27,25): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (29,37): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (29,42): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (29,69): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (3,58): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (3,63): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (8,41): error CS1503: Argument 1: cannot convert from 'TemperatureDisplay' to 'IObserver<int>'
            (20,46): error CS0246: The type or namespace name 'Exception' could not be found (are you missing a using directive or an assembly reference?)
            (5,17): error CS0103: The name 'Console' does not exist in the current context
            (10,17): error CS0103: The name 'Console' does not exist in the current context
            (15,17): error CS0103: The name 'Console' does not exist in the current context
            */

            Console.ReadLine();
        }
    }
}