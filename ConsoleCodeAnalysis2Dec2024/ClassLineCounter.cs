using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleCodeAnalysis2Dec2024
{
    public class ClassLineCounter
    {
        private SyntaxTree syntaxTree;

        public ClassLineCounter(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
        }

        public void Count(string className)
        {
            // Get the root node of the syntax tree
            var root = this.syntaxTree.GetRoot();

            // Find the class declaration node
            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls => cls.Identifier.Text == className);

            if (classNode != null)
            {
                // Get the location of the class and calculate the line span
                var lineSpan = classNode.GetLocation().GetLineSpan();

                // Calculate the number of lines
                int lineCount = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line + 1;

                Console.WriteLine($"The class '{classNode.Identifier.Text}' has {lineCount} lines.");
            }
            else
            {
                //Console.WriteLine("Class not found.");
            }
        }

        public int LineCount(string className)
        {
            // Get the root node of the syntax tree
            var root = this.syntaxTree.GetRoot();

            // Find the class declaration node
            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cls => cls.Identifier.Text == className);

            if (classNode != null)
            {
                // Get the location of the class and calculate the line span
                var lineSpan = classNode.GetLocation().GetLineSpan();

                // Calculate the number of lines
                int lineCount = lineSpan.EndLinePosition.Line - lineSpan.StartLinePosition.Line + 1;
                return lineCount;
            }

            return 0;
        }
    }
}
