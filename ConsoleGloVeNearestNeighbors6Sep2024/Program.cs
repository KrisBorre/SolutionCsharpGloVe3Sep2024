using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors6Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50; // Assuming GloVe's 50-dimensional embeddings

            // Load pretrained embeddings
            EmbeddingLoader embeddingLoader = new EmbeddingLoader(filePath, embeddingDim);
            var wordEmbeddings = new WordEmbeddings(embeddingLoader.Embeddings);

            GloVeExamples examples = new GloVeExamples(wordEmbeddings);

            string[] sentence1 = { "the", "electron", "is", "a", "fundamental", "particle" };
            string[] sentence2 = { "an", "atom", "contains", "protons", "and", "electrons" };
            string[] sentence3 = { "that", "lady", "eats", "at", "a", "restaurant" };

            examples.DisplaySentenceSimilarity(sentence1, sentence2);
            examples.DisplaySentenceSimilarity(sentence1, sentence3);
            examples.DisplaySentenceSimilarity(sentence2, sentence3);

            /*
            Similarity between sentences: 0,8326224794404662
            Similarity between sentences: 0,6692918379019768
            Similarity between sentences: 0,5127842725812689
            */
            Console.ReadLine();
        }
    }
}
