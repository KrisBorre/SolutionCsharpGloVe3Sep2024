using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleCodeAnalysis10Nov2024
{
    public class MetadataReferenceManager8Nov2024
    {
        public List<MetadataReference> GetReferences()
        {
            return new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
                MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
                MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
             };
        }
    }
}
