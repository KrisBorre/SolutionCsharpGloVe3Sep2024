using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors6Sep2024_300d
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.300d.txt";
            int embeddingDim = 300;

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
            Loading embeddings...
            Embeddings loaded successfully!
            Similarity between sentences: 0,6981793184945988
            Similarity between sentences: 0,5275625367322592
            Similarity between sentences: 0,31004681308540855
            */

            /*
            Loading embeddings...
            Embeddings loaded successfully!
            Similarity between sentences: 0,6981793184945988
            Similarity between sentences: 0,5275625367322592
            Similarity between sentences: 0,31004681308540855
            */

            Console.ReadLine();
        }
    }
}
