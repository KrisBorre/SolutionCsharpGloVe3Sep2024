using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ConsoleCodeAnalysis6Nov2024
{
    public class MetadataReferenceManager
    {
        public List<MetadataReference> GetReferences()
        {
            return new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
            };
        }
    }
}
