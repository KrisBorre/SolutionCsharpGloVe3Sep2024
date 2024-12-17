namespace LibraryGlobalVectors1Sep2024
{
    /// <summary>
    /// ChatGPT-4o
    /// </summary>
    public class GloVeExamples
    {
        private WordEmbeddings embeddings;

        public GloVeExamples(WordEmbeddings embeddings)
        {
            this.embeddings = embeddings;
        }

        public void DisplaySentenceSimilarity(string[] sentence1, string[] sentence2)
        {
            double[] embedding1 = embeddings.ComputeSentenceEmbedding(sentence1);
            double[] embedding2 = embeddings.ComputeSentenceEmbedding(sentence2);
            double similarity = WordEmbeddings.CosineSimilarity(embedding1, embedding2);
            Console.WriteLine($"Similarity between sentences: {similarity}");
        }
    }
}
