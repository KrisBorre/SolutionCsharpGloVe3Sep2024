using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace ConsoleCodeAnalysis1Sep2024_IObservable2
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
            string code1 = @"public class TemperatureSensor : IObservable<int>
            {
                private List<IObserver<int>> observers;

                public TemperatureSensor()
                {
                    observers = new List<IObserver<int>>();
                }

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

            string code3 = @"public static void Main(string[] args)
            {
                var sensor = new TemperatureSensor();
                var display = new TemperatureDisplay();

                IDisposable subscription = sensor.Subscribe(display);

                sensor.PublishTemperature(25);
                sensor.PublishTemperature(35);
                sensor.PublishTemperature(40);

                subscription.Dispose();
            }";

            string code = code1 + Environment.NewLine + code2 + Environment.NewLine + code3;


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
            (64,1): error CS8803: Top-level statements must precede namespace and type declarations.
            (64,1): error CS0106: The modifier 'public' is not valid for this item
            (47,35): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (1,34): error CS0246: The type or namespace name 'IObservable<>' could not be found (are you missing a using directive or an assembly reference?)
            (3,25): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (3,30): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (54,37): error CS0246: The type or namespace name 'Exception' could not be found (are you missing a using directive or an assembly reference?)
            (10,46): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (10,24): error CS0246: The type or namespace name 'IDisposable' could not be found (are you missing a using directive or an assembly reference?)
            (29,46): error CS0246: The type or namespace name 'IDisposable' could not be found (are you missing a using directive or an assembly reference?)
            (31,29): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (31,34): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (32,29): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (34,41): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (34,46): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (34,73): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (7,37): error CS0246: The type or namespace name 'List<>' could not be found (are you missing a using directive or an assembly reference?)
            (7,42): error CS0246: The type or namespace name 'IObserver<>' could not be found (are you missing a using directive or an assembly reference?)
            (69,17): error CS0246: The type or namespace name 'IDisposable' could not be found (are you missing a using directive or an assembly reference?)
            (69,61): error CS1503: Argument 1: cannot convert from 'TemperatureDisplay' to 'IObserver<int>'
            (64,20): warning CS7022: The entry point of the program is global code; ignoring 'Main(string[])' entry point.
            (64,20): warning CS8321: The local function 'Main' is declared but never used
            (25,50): error CS0246: The type or namespace name 'Exception' could not be found (are you missing a using directive or an assembly reference?)
            (51,21): error CS0103: The name 'Console' does not exist in the current context
            (56,21): error CS0103: The name 'Console' does not exist in the current context
            (61,21): error CS0103: The name 'Console' does not exist in the current context
            */

            Console.ReadLine();
        }
    }
}
