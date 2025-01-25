using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis14Sep2024
namespace ConsoleCodeAnalysis19Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code = @"class Example
{
    void Test()
    {
        if (true)
            Console.WriteLine(""No braces here!"");
        if (false)
            Console.WriteLine(""Another unbraced statement!"");
    }
}";

            CodeAnalyzer.AnalyzeCode(code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: Example
                Method: Test() 
            */
        }
    }
}
