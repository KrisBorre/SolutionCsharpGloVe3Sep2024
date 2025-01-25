using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LibraryCodeAnalysis11Nov2024
{
    public class ConsoleReadKeyRemovalRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // Check if the node is a call to Console.ReadKey()
            if (node.ToString().Contains("Console.ReadKey"))
            {
                // Remove the invocation by returning null (removes the node entirely)
                return null;
            }

            return base.VisitInvocationExpression(node);
        }

        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            // Remove the statement if it contains Console.ReadKey()
            if (node.ToString().Contains("Console.ReadKey"))
            {
                return null;
            }

            return base.VisitExpressionStatement(node);
        }
    }
}
