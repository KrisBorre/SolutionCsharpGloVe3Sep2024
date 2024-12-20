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
}