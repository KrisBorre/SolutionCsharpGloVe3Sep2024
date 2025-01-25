using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis11Sep2024_IEnumerable1
namespace ConsoleCodeAnalysis16Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = @"
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
}";


            CodeAnalyzer.AnalyzeCode(code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: NumberCollection
                Method: Add()
                Method: GetEnumerator()
                Method: GetEnumerator()
              Class: Program
                Method: Main()
            */

            Console.ReadLine();
        }

    }
}
