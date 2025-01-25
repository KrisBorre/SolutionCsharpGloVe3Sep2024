using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis19Sep2024
namespace ConsoleCodeAnalysis21Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = @"
        using System;

        public class SampleClass
        {
            public void MethodOne()
            {
                Console.WriteLine(""Hello from MethodOne"");
            }

            private int MethodTwo(int x)
            {
                return x * x;
            }
        }";

            CodeAnalyzer.AnalyzeCode(code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: SampleClass
                Method: MethodOne()
                Method: MethodTwo() 
            */
        }
    }
}
