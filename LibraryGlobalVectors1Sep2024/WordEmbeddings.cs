namespace LibraryGlobalVectors1Sep2024
{
    /// <summary>
    /// ChatGPT-4o
    /// </summary>
    public class WordEmbeddings
    {
        private Dictionary<string, double[]> embeddings;

        public WordEmbeddings(Dictionary<string, double[]> embeddings)
        {
            this.embeddings = embeddings;
        }

        public string FindAnalogy(string wordA, string wordB, string wordC)
        {
            if (!embeddings.ContainsKey(wordA) || !embeddings.ContainsKey(wordB) || !embeddings.ContainsKey(wordC))
                throw new ArgumentException("One or more words not found in embeddings.");

            double[] vectorA = embeddings[wordA];
            double[] vectorB = embeddings[wordB];
            double[] vectorC = embeddings[wordC];

            double[] resultVector = new double[vectorA.Length];
            for (int i = 0; i < vectorA.Length; i++)
            {
                resultVector[i] = vectorB[i] - vectorA[i] + vectorC[i];
            }

            return FindNearestWord(resultVector, new List<string> { wordA, wordB, wordC });
        }

        public string FindNearestWord(double[] resultVector, List<string> excludedWords = null)
        {
            excludedWords = excludedWords ?? new List<string>();

            string nearestWord = null;
            double bestSimilarity = double.MinValue;

            foreach (var kvp in embeddings)
            {
                string word = kvp.Key;
                double[] wordVector = kvp.Value;

                // Skip excluded words
                if (excludedWords.Contains(word))
                    continue;

                // Calculate cosine similarity
                double similarity = CosineSimilarity(resultVector, wordVector);

                // Update if a closer word is found
                if (similarity > bestSimilarity)
                {
                    bestSimilarity = similarity;
                    nearestWord = word;
                }
            }

            return nearestWord;
        }

        // Method to calculate cosine similarity
        public static double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            double dotProduct = 0;
            double normA = 0;
            double normB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += vectorA[i] * vectorA[i];
                normB += vectorB[i] * vectorB[i];
            }

            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        // Method to calculate Euclidean distance
        private static double EuclideanDistance(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException("Vectors must be of the same length.");
            }

            double sum = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                double difference = vectorA[i] - vectorB[i];
                sum += difference * difference;
            }

            return Math.Sqrt(sum);
        }

        public string FindNearestWord(string word)
        {
            return FindNearestNeighbors(word, 1, false)[0];
        }

        // Method to find nearest neighbors using Euclidean distance
        public List<string> FindNearestNeighbors(string word, int n = 10, bool useCosineSimilarity = true)
        {
            if (!embeddings.ContainsKey(word))
            {
                Console.WriteLine("Word not found in embeddings.");
                return new List<string>();
            }
            else
            {
                double[] wordVector = embeddings[word];
                var distances = new List<(string, double)>();

                foreach (var kvp in embeddings)
                {
                    if (kvp.Key != word) // Skip the word itself
                    {
                        double similarity = useCosineSimilarity
                            ? CosineSimilarity(wordVector, kvp.Value)
                            : -EuclideanDistance(wordVector, kvp.Value); // Use negative Euclidean distance to get nearest

                        distances.Add((kvp.Key, similarity));
                    }
                }

                // Sort by similarity (or distance) in descending order and take top n
                return distances.OrderByDescending(x => x.Item2)
                                .Take(n)
                                .Select(x => x.Item1)
                                .ToList();
            }
        }

        // Sentence Embeddings: By averaging the word vectors in a sentence, you can get a rough representation for the sentence as a whole.
        // This can be used for comparing sentence similarity, clustering sentences, or measuring sentence-level semantics.


        public double[] ComputeSentenceEmbedding(string[] sentence)
        {
            double[] sentenceEmbedding = new double[embeddings.First().Value.Length];
            int validWordCount = 0;

            foreach (string word in sentence)
            {
                if (embeddings.ContainsKey(word))
                {
                    double[] wordEmbedding = embeddings[word];
                    for (int i = 0; i < sentenceEmbedding.Length; i++)
                    {
                        sentenceEmbedding[i] += wordEmbedding[i];
                    }
                    validWordCount++;
                }
            }

            if (validWordCount > 0)
            {
                for (int i = 0; i < sentenceEmbedding.Length; i++)
                {
                    sentenceEmbedding[i] /= validWordCount;
                }
            }

            return sentenceEmbedding;
        }

    }
}
