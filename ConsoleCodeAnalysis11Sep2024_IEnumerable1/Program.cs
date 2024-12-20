using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis11Sep2024_IEnumerable1
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // DeepSeek
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

            // Extract namespaces and their classes
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
            no output
            */

            Console.ReadLine();
        }

        static Dictionary<string, List<string>> ExtractNamespacesAndClasses(SyntaxNode root)
        {
            var namespaceClassMap = new Dictionary<string, List<string>>();

            // Find all NamespaceDeclarationSyntax nodes
            var namespaceNodes = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();

            foreach (var namespaceNode in namespaceNodes)
            {
                string namespaceName = namespaceNode.Name.ToString();
                var classNames = namespaceNode.DescendantNodes()
                                              .OfType<ClassDeclarationSyntax>()
                                              .Select(cls => cls.Identifier.Text)
                                              .ToList();

                namespaceClassMap[namespaceName] = classNames;
            }

            return namespaceClassMap;
        }
    }
}