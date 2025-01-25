namespace LibraryCodeAnalysis1Dec2024
{
    public class NamespaceMap30Nov2024
    {
        private Dictionary<string, Dictionary<string, List<string>>> keyValuePairs = new Dictionary<string, Dictionary<string, List<string>>>();

        public Dictionary<string, Dictionary<string, List<string>>> KeyValuePairs { get { return keyValuePairs; } }

        public NamespaceMap30Nov2024(Dictionary<string, Dictionary<string, List<string>>> keyValuePairs)
        {
            this.keyValuePairs = keyValuePairs;
        }
    }
}
