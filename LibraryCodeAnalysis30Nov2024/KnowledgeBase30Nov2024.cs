namespace LibraryCodeAnalysis30Nov2024
{
    public class KnowledgeBase30Nov2024
    {
        public bool Success = false;

        public List<SourceCode30Nov2024> SourceCodes = new List<SourceCode30Nov2024>();

        public SourceCode30Nov2024 MainSourceCode;

        public List<string> CodeBlocks
        {
            get
            {
                List<string> result = new List<string>();

                foreach (SourceCode30Nov2024 sourceCode in this.SourceCodes)
                {
                    result.Add(sourceCode.Code);
                }

                return result;
            }
        }

        public KnowledgeBase30Nov2024()
        {

        }
    }
}
