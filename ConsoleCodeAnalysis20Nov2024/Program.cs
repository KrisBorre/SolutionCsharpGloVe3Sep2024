using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis18Sep2024
namespace ConsoleCodeAnalysis20Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string codeSnippet = @"using System;
using System.Linq;

class Example
{
    static void Main(string[] args)
    {
        Func<int, int> square = x => x * x;
        var numbers = new[] { 1, 2, 3 };
        var doubled = numbers.Select(n => n * 2).ToList();
        numbers.ToList().ForEach(n => Console.WriteLine(n));
    }
}";

            CodeAnalyzer.AnalyzeCode(codeSnippet);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: Example
                Method: Main() 
            */
        }
    }
}
