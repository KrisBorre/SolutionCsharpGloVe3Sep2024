using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors2Sep2024_200d
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filePath = "../../../../../../../GloVe/glove.6B.200d.txt";
            const int embeddingDimension = 200;

            // Load pretrained embeddings
            var embeddingLoader = new EmbeddingLoader(filePath, embeddingDimension);
            var wordEmbeddings = LoadWordEmbeddings(embeddingLoader);

            // Define target words and process them
            string[] targetWords = { "france", "queen", "IBM", "clear" };
            const int numberOfNeighbors = 4;

            foreach (string targetWord in targetWords)
            {
                DisplayNeighbors(targetWord, wordEmbeddings, numberOfNeighbors);
            }

            /*
            Loading embeddings...
            Embeddings loaded successfully!
            Processing neighbors for 'france'...
            Neighbors of 'france' using cosine similarity:
            french
            paris
            belgium
            world

            Neighbors of 'france' using Euclidean distance:
            french
            paris
            belgium
            Honolulu

            Processing neighbors for 'queen'...
            Neighbors of 'queen' using cosine similarity:
            sister
            woman
            brother
            uncle

            Neighbors of 'queen' using Euclidean distance:
            sister
            Honolulu
            IBM
            Nashville

            Processing neighbors for 'IBM'...
            Neighbors of 'IBM' using cosine similarity:
            comparative
            city
            strong
            paris

            Neighbors of 'IBM' using Euclidean distance:
            Honolulu
            Nashville
            United
            superlative

            Processing neighbors for 'clear'...
            Neighbors of 'clear' using cosine similarity:
            this
            is
            strong
            pressure

            Neighbors of 'clear' using Euclidean distance:
            this
            Honolulu
            IBM
            Nashville
            */

            Console.ReadLine();
        }

        private static WordEmbeddings LoadWordEmbeddings(EmbeddingLoader embeddingLoader)
        {
            string[] tokens =
            {
            "comparative", "city", "strong", "paris", "man", "Honolulu", "toad", "queen", "IBM", "litoria", "french",
            "hello", "world", "clear", "this", "soft", "dark", "is", "a", "test", "france", "belgium", "clearer",
            "softer", "darker", "frog", "frogs", "lizard", "Nashville", "uncle", "woman", "sir", "United",
            "stronger", "superlative", "company", "brother", "sister", "observation", "elevator", "blood",
            "pressure", "town", "phone", "hamburger", "solid", "gas", "water", "fashion", "ice", "steam"
        };

            var embeddings = new Dictionary<string, double[]>();

            foreach (string token in tokens)
            {
                var embedding = embeddingLoader.GetEmbedding(token);
                if (embedding == null)
                {
                    Console.WriteLine($"Warning: Embedding for token '{token}' not found.");
                    continue;
                }
                embeddings[token] = embedding;
            }

            return new WordEmbeddings(embeddings);
        }

        private static void DisplayNeighbors(string targetWord, WordEmbeddings wordEmbeddings, int numberOfNeighbors)
        {
            Console.WriteLine($"Processing neighbors for '{targetWord}'...");

            var cosineNeighbors = wordEmbeddings.FindNearestNeighbors(targetWord, numberOfNeighbors);
            Console.WriteLine($"Neighbors of '{targetWord}' using cosine similarity:");
            DisplayNeighborList(cosineNeighbors);

            var euclideanNeighbors = wordEmbeddings.FindNearestNeighbors(targetWord, numberOfNeighbors, useCosineSimilarity: false);
            Console.WriteLine($"Neighbors of '{targetWord}' using Euclidean distance:");
            DisplayNeighborList(euclideanNeighbors);
        }

        private static void DisplayNeighborList(IEnumerable<string> neighbors)
        {
            foreach (var neighbor in neighbors)
            {
                Console.WriteLine(neighbor);
            }
            Console.WriteLine();
        }
    }
}