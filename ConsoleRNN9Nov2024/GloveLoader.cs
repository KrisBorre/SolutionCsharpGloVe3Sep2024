using System.Globalization;

namespace ConsoleRNN9Nov2024
{
    public class GloveLoader
    {
        private Dictionary<string, double[]> embeddings;
        private int embeddingDim;

        public GloveLoader(string filePath, int embeddingDim)
        {
            this.embeddingDim = embeddingDim;
            this.embeddings = LoadEmbeddings(filePath);
        }

        private Dictionary<string, double[]> LoadEmbeddings(string filePath)
        {
            var embeddings = new Dictionary<string, double[]>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');
                var word = parts[0];
                var vector = parts.Skip(1).Select(s => double.Parse(s, CultureInfo.InvariantCulture) / 10).ToArray(); // Normalize embeddings

                embeddings[word] = vector;
            }

            return embeddings;
        }

        public double[] GetEmbedding(string word)
        {
            if (embeddings.ContainsKey(word))
            {
                return embeddings[word];
            }
            else
            {
                return new double[embeddingDim];
            }
        }

        public string FindClosestWord(double[] embedding)
        {
            double bestSimilarity = double.MinValue;
            string bestWord = null;

            foreach (var kvp in embeddings)
            {
                var word = kvp.Key;
                var vector = kvp.Value;
                double similarity = CosineSimilarity(vector, embedding);

                if (similarity > bestSimilarity)
                {
                    bestSimilarity = similarity;
                    bestWord = word;
                }
            }

            return bestWord;
        }

        private double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            double dotProduct = 0, normA = 0, normB = 0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += vectorA[i] * vectorA[i];
                normB += vectorB[i] * vectorB[i];
            }
            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }

}
