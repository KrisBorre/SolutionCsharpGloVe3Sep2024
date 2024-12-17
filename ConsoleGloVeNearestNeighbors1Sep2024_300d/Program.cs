using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors1Sep2024_300d
{
    internal class Program
    {
        /// <summary>
        /// https://mmuratarat.github.io/2020-03-20/glove
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.300d.txt";

            int embeddingDim = 300;

            // Load pretrained embeddings
            EmbeddingLoader embeddingLoader = new EmbeddingLoader(filePath, embeddingDim);

            var embeddings = new Dictionary<string, double[]>();

            string[] tokens = { "paris", "french", "hello", "world", "this", "is", "a", "test", "france", "belgium", "" };

            foreach (var token in tokens)
            {
                var embedding = embeddingLoader.GetEmbedding(token);
                embeddings.Add(token, embedding);
            }

            var wordEmbeddings = new WordEmbeddings(embeddings);

            string targetWord = "france";
            int n = 4;

            var neighbors = wordEmbeddings.FindNearestNeighbors(targetWord, n);

            Console.WriteLine($"Neighbors of '{targetWord}':");
            foreach (var neighbor in neighbors)
            {
                Console.WriteLine(neighbor);
            }

            /*
            Loading embeddings...
            Embeddings loaded successfully!
            Neighbors of 'france':
            french
            paris
            belgium
            world 
            */

            Console.ReadLine();
        }
    }
}
