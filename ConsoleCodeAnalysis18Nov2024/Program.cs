using LibraryCodeAnalysis14Nov2024;

// see ConsoleCodeAnalysis12Sep2024
namespace ConsoleCodeAnalysis18Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string code1 = @"
        namespace ExampleNamespace
        {
            class ExampleClass
            {
                void Method1() { int x = 1; }
            }
        }";

            string code2 = @"
        namespace ExampleNamespace
        {
            class AnotherClass
            {
                void Method2() { string y = ""test""; }
            }
        }";


            CodeAnalyzer.AnalyzeCode(code1);

            CodeAnalyzer.AnalyzeCode(code2);

            /*
            Code Analysis Result:
            Namespace: ExampleNamespace
              Class: ExampleClass
                Method: Method1()
            Code Analysis Result:
            Namespace: ExampleNamespace
              Class: AnotherClass
                Method: Method2()
            */
        }
    }
}
