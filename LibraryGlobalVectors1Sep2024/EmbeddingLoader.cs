using System.Globalization;

namespace LibraryGlobalVectors1Sep2024
{
    /// <summary>
    /// ChatGPT-4o
    /// </summary>
    public class EmbeddingLoader
    {
        public Dictionary<string, double[]> Embeddings;
        private readonly int embeddingDim;

        // Constructor to load embeddings from a file
        public EmbeddingLoader(string filePath, int embeddingDim)
        {
            Embeddings = new Dictionary<string, double[]>();
            this.embeddingDim = embeddingDim;
            LoadEmbeddings(filePath);
        }

        // Load embeddings from file into a dictionary
        private void LoadEmbeddings(string filePath)
        {
            Console.WriteLine("Loading embeddings...");
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');

                if (parts.Length != embeddingDim + 1) continue;

                string word = parts[0];
                double[] vector = new double[embeddingDim];

                for (int i = 0; i < embeddingDim; i++)
                {
                    vector[i] = double.Parse(parts[i + 1], CultureInfo.InvariantCulture);
                }

                Embeddings[word] = vector;
            }
            Console.WriteLine("Embeddings loaded successfully!");
        }

        // Retrieve embedding for a word, or return a default vector if not found
        public double[] GetEmbedding(string word)
        {
            if (Embeddings.TryGetValue(word, out double[] embedding))
            {
                return embedding;
            }
            else
            {
                // Return a zero-vector if the word is not in the embeddings dictionary
                return new double[embeddingDim];
            }
        }

        // Example of using the embeddings with tokenized input
        public double[,] ConvertTokensToEmbeddings(string[] tokens)
        {
            double[,] embeddingsArray = new double[tokens.Length, embeddingDim];
            for (int i = 0; i < tokens.Length; i++)
            {
                double[] embedding = GetEmbedding(tokens[i]);
                for (int j = 0; j < embeddingDim; j++)
                {
                    embeddingsArray[i, j] = embedding[j];
                }
            }
            return embeddingsArray;
        }

    }
}
