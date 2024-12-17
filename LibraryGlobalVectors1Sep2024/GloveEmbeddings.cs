using System.Globalization;

namespace LibraryGlobalVectors1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class GloveEmbeddings
    {
        private Dictionary<string, double[]> _embeddings;

        public GloveEmbeddings(string filePath, int embeddingDim)
        {
            _embeddings = LoadEmbeddings(filePath, embeddingDim);
        }

        private Dictionary<string, double[]> LoadEmbeddings(string filePath, int embeddingDim)
        {
            var embeddings = new Dictionary<string, double[]>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');
                var word = parts[0];

                var vector = parts.Skip(1).Select(part => double.Parse(part, CultureInfo.InvariantCulture)).ToArray();

                if (vector.Length == embeddingDim)
                {
                    embeddings[word] = vector;
                }
            }

            return embeddings;
        }

        public double[] GetEmbedding(string word)
        {
            return _embeddings.ContainsKey(word) ? _embeddings[word] : null;
        }

        public double[] ComputeSentenceEmbedding(string sentence)
        {
            var words = sentence.Split(' ');
            var validVectors = words
                .Select(word => GetEmbedding(word))
                .Where(vector => vector != null)
                .ToList();

            if (validVectors.Count == 0)
                return new double[_embeddings.First().Value.Length]; // Return zero vector if no words are found

            // Average the word embeddings to get the sentence embedding
            double[] sentenceEmbedding = new double[validVectors[0].Length];
            foreach (var vector in validVectors)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    sentenceEmbedding[i] += vector[i];
                }
            }

            for (int i = 0; i < sentenceEmbedding.Length; i++)
            {
                sentenceEmbedding[i] /= validVectors.Count;
            }

            return sentenceEmbedding;
        }
    }
}