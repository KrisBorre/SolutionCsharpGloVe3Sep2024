using System.Reflection;

namespace LibraryCodeAnalysis1Dec2024
{
    public class CodeExecutor
    {
        public ExecutionResult Execute(Assembly assembly)
        {
            var type = assembly.GetType("Program");
            if (type == null)
            {
                return new ExecutionResult("No class named 'Program' found.");
            }

            var method = type.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
            {
                return new ExecutionResult("No Main method found.");
            }

            var originalConsoleOut = Console.Out;
            var originalConsoleIn = Console.In;
            using (var stringWriter = new StringWriter())
            {
                // Simulate console input (e.g., for Console.ReadLine)
                var simulatedInput = "Sample input for Console.ReadLine\n";
                using (var stringReader = new StringReader(simulatedInput))
                {
                    try
                    {
                        Console.SetOut(stringWriter);
                        Console.SetIn(stringReader);

                        var parameters = method.GetParameters();

                        if (parameters.Length == 0)
                        {
                            method.Invoke(null, null);
                        }
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]))
                        {
                            method.Invoke(null, new object[] { new string[0] });
                        }
                        else
                        {
                            return new ExecutionResult("Unsupported Main method signature.");
                        }

                        return new ExecutionResult(stringWriter.ToString());
                    }
                    catch (Exception ex)
                    {
                        return new ExecutionResult($"Error during execution: {ex.InnerException?.Message ?? ex.Message}");
                    }
                    finally
                    {
                        Console.SetOut(originalConsoleOut);
                        Console.SetIn(originalConsoleIn);
                    }
                }
            }
        }
    }
}
