using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// see ConsoleCodeAnalysis11Sep2024_IEnumerable1
namespace ConsoleCodeAnalysis15Nov2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
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

            // Parse the code into a syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = syntaxTree.GetRoot();

            // Extract namespaces and classes
            var namespaces = ExtractNamespacesAndClasses(root);

            // Print the results
            foreach (var ns in namespaces)
            {
                Console.WriteLine($"Namespace: {ns.Key}");
                foreach (var className in ns.Value)
                {
                    Console.WriteLine($"  Class: {className}");
                }
            }

            /*
            Namespace: <global namespace>
              Class: NumberCollection
              Class: Program 
            */

            Console.ReadLine();
        }

        static Dictionary<string, List<string>> ExtractNamespacesAndClasses(SyntaxNode root)
        {
            var namespaceClassMap = new Dictionary<string, List<string>>();

            // Handle classes within explicit namespaces
            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                var classNames = new List<string>();

                foreach (var classNode in namespaceNode.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    classNames.Add(classNode.Identifier.Text);
                }

                namespaceClassMap[namespaceName] = classNames;
            }

            // Handle classes in the global namespace
            var globalClasses = new List<string>();
            foreach (var classNode in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                if (classNode.Parent is not NamespaceDeclarationSyntax)
                {
                    globalClasses.Add(classNode.Identifier.Text);
                }
            }

            if (globalClasses.Count > 0)
            {
                namespaceClassMap["<global namespace>"] = globalClasses;
            }

            return namespaceClassMap;
        }
    }
}
