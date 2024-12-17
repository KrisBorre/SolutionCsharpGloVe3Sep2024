using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors5Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// GloVe is an unsupervised learning algorithm for obtaining vector representations for words. 
        /// https://nlp.stanford.edu/projects/glove/
        /// </summary>
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;

            // Initialize embeddings
            var embeddingLoader = new EmbeddingLoader(filePath, embeddingDim);
            var wordEmbeddings = new WordEmbeddings(embeddingLoader.Embeddings);

            // Run analogy tests
            RunAnalogyTests(wordEmbeddings);

            // Find nearest neighbors for tokens
            string[] tokens = GetTestTokens();
            int numNeighbors = 1;
            FindNeighborsForTokens(wordEmbeddings, tokens, numNeighbors);

            /*
            Loading embeddings...
            Embeddings loaded successfully!
            ### Analogy Tests ###
            king - man + woman = girl
            man - king + woman = queen
            man - woman + king = queen
            ice - steam + liquid = condensing
            gas - water + steam = dry
            queen - man + woman = victim
            dark - darker + soft = softer
            man - woman + lord = earl
            velocity - acceleration + position = reforms
            electron - proton + atom = qatargas

            ### Nearest Neighbors ###
            Neighbors of 'comparative' using cosine:
            anthropology
            Neighbors of 'comparative' using Euclidean:
            sociological

            Neighbors of 'city' using cosine:
            town
            Neighbors of 'city' using Euclidean:
            town

            Neighbors of 'strong' using cosine:
            stronger
            Neighbors of 'strong' using Euclidean:
            stronger

            Neighbors of 'paris' using cosine:
            prohertrib
            Neighbors of 'paris' using Euclidean:
            prohertrib

            Neighbors of 'man' using cosine:
            woman
            Neighbors of 'man' using Euclidean:
            woman

            Neighbors of 'Honolulu' using cosine:
            Word not found in embeddings.
            'Honolulu' not found in embeddings.
            Neighbors of 'Honolulu' using Euclidean:
            Word not found in embeddings.
            'Honolulu' not found in embeddings.

            Neighbors of 'toad' using cosine:
            toads
            Neighbors of 'toad' using Euclidean:
            toads

            Neighbors of 'queen' using cosine:
            princess
            Neighbors of 'queen' using Euclidean:
            princess

            Neighbors of 'IBM' using cosine:
            Word not found in embeddings.
            'IBM' not found in embeddings.
            Neighbors of 'IBM' using Euclidean:
            Word not found in embeddings.
            'IBM' not found in embeddings.

            Neighbors of 'litoria' using cosine:
            limnonectes
            Neighbors of 'litoria' using Euclidean:
            limnonectes

            Neighbors of 'french' using cosine:
            france
            Neighbors of 'french' using Euclidean:
            france

            Neighbors of 'hello' using cosine:
            goodbye
            Neighbors of 'hello' using Euclidean:
            goodbye

            Neighbors of 'world' using cosine:
            europe
            Neighbors of 'world' using Euclidean:
            europe

            Neighbors of 'clear' using cosine:
            but
            Neighbors of 'clear' using Euclidean:
            yet

            Neighbors of 'this' using cosine:
            it
            Neighbors of 'this' using Euclidean:
            it

            Neighbors of 'soft' using cosine:
            thin
            Neighbors of 'soft' using Euclidean:
            thin

            Neighbors of 'dark' using cosine:
            shadows
            Neighbors of 'dark' using Euclidean:
            shadows

            Neighbors of 'is' using cosine:
            this
            Neighbors of 'is' using Euclidean:
            this

            Neighbors of 'a' using cosine:
            another
            Neighbors of 'a' using Euclidean:
            an

            Neighbors of 'test' using cosine:
            tests
            Neighbors of 'test' using Euclidean:
            tests

            Neighbors of 'france' using cosine:
            french
            Neighbors of 'france' using Euclidean:
            french

            Neighbors of 'belgium' using cosine:
            netherlands
            Neighbors of 'belgium' using Euclidean:
            netherlands

            Neighbors of 'clearer' using cosine:
            reassuring
            Neighbors of 'clearer' using Euclidean:
            reassuring

            Neighbors of 'softer' using cosine:
            slightly
            Neighbors of 'softer' using Euclidean:
            softening

            Neighbors of 'darker' using cosine:
            dark
            Neighbors of 'darker' using Euclidean:
            dark

            Neighbors of 'frog' using cosine:
            snake
            Neighbors of 'frog' using Euclidean:
            snake

            Neighbors of 'frogs' using cosine:
            horned
            Neighbors of 'frogs' using Euclidean:
            salamanders

            Neighbors of 'lizard' using cosine:
            parrot
            Neighbors of 'lizard' using Euclidean:
            parrot

            Neighbors of 'Nashville' using cosine:
            Word not found in embeddings.
            'Nashville' not found in embeddings.
            Neighbors of 'Nashville' using Euclidean:
            Word not found in embeddings.
            'Nashville' not found in embeddings.

            Neighbors of 'uncle' using cosine:
            son
            Neighbors of 'uncle' using Euclidean:
            cousin

            Neighbors of 'woman' using cosine:
            girl
            Neighbors of 'woman' using Euclidean:
            girl

            Neighbors of 'sir' using cosine:
            edward
            Neighbors of 'sir' using Euclidean:
            edward

            Neighbors of 'United' using cosine:
            Word not found in embeddings.
            'United' not found in embeddings.
            Neighbors of 'United' using Euclidean:
            Word not found in embeddings.
            'United' not found in embeddings.

            Neighbors of 'stronger' using cosine:
            weaker
            Neighbors of 'stronger' using Euclidean:
            weaker

            Neighbors of 'superlative' using cosine:
            masterly
            Neighbors of 'superlative' using Euclidean:
            exemplifying

            Neighbors of 'company' using cosine:
            firm
            Neighbors of 'company' using Euclidean:
            firm

            Neighbors of 'brother' using cosine:
            son
            Neighbors of 'brother' using Euclidean:
            cousin

            Neighbors of 'sister' using cosine:
            wife
            Neighbors of 'sister' using Euclidean:
            wife

            Neighbors of 'observation' using cosine:
            reconnaissance
            Neighbors of 'observation' using Euclidean:
            observing

            Neighbors of 'elevator' using cosine:
            escalator
            Neighbors of 'elevator' using Euclidean:
            escalator

            Neighbors of 'blood' using cosine:
            causes
            Neighbors of 'blood' using Euclidean:
            causes

            Neighbors of 'pressure' using cosine:
            ease
            Neighbors of 'pressure' using Euclidean:
            ease

            Neighbors of 'town' using cosine:
            village
            Neighbors of 'town' using Euclidean:
            village

            Neighbors of 'phone' using cosine:
            telephone
            Neighbors of 'phone' using Euclidean:
            telephone

            Neighbors of 'hamburger' using cosine:
            abendblatt
            Neighbors of 'hamburger' using Euclidean:
            burger

            Neighbors of 'solid' using cosine:
            consistent
            Neighbors of 'solid' using Euclidean:
            consistent

            Neighbors of 'gas' using cosine:
            fuel
            Neighbors of 'gas' using Euclidean:
            fuel

            Neighbors of 'water' using cosine:
            dry
            Neighbors of 'water' using Euclidean:
            dry

            Neighbors of 'fashion' using cosine:
            style
            Neighbors of 'fashion' using Euclidean:
            fashions

            Neighbors of 'ice' using cosine:
            hot
            Neighbors of 'ice' using Euclidean:
            hot

            Neighbors of 'steam' using cosine:
            engines
            Neighbors of 'steam' using Euclidean:
            boiler
            */

            Console.ReadLine();
        }

        /// <summary>
        /// Runs a set of analogy tests to demonstrate the GloVe model's capabilities.
        /// </summary>
        /// <param name="wordEmbeddings">The word embeddings instance.</param>
        static void RunAnalogyTests(WordEmbeddings wordEmbeddings)
        {
            Console.WriteLine("### Analogy Tests ###");
            var analogyTests = new (string Word1, string Word2, string Word3)[]
            {
                ("king", "man", "woman"),
                ("man", "king", "woman"),
                ("man", "woman", "king"),
                ("ice", "steam", "liquid"),
                ("gas", "water", "steam"),
                ("queen", "man", "woman"),
                ("dark", "darker", "soft"),
                ("man", "woman", "lord"),
                ("velocity", "acceleration", "position"),
                ("electron", "proton", "atom"),
                // Add more analogy tests here as needed
            };

            foreach (var (word1, word2, word3) in analogyTests)
            {
                string result = wordEmbeddings.FindAnalogy(word1, word2, word3);
                Console.WriteLine($"{word1} - {word2} + {word3} = {result}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Returns a predefined list of test tokens.
        /// </summary>
        static string[] GetTestTokens()
        {
            return new string[]
            {
                "comparative", "city", "strong", "paris", "man", "Honolulu", "toad", "queen",
                "IBM", "litoria", "french", "hello", "world", "clear", "this", "soft", "dark",
                "is", "a", "test", "france", "belgium", "clearer", "softer", "darker", "frog",
                "frogs", "lizard", "Nashville", "uncle", "woman", "sir", "United", "stronger",
                "superlative", "company", "brother", "sister", "observation", "elevator",
                "blood", "pressure", "town", "phone", "hamburger", "solid", "gas", "water",
                "fashion", "ice", "steam"
            };
        }

        /// <summary>
        /// Finds and displays the nearest neighbors for each token in the list.
        /// </summary>
        /// <param name="wordEmbeddings">The word embeddings instance.</param>
        /// <param name="tokens">The tokens to process.</param>
        /// <param name="numNeighbors">Number of nearest neighbors to retrieve.</param>
        static void FindNeighborsForTokens(WordEmbeddings wordEmbeddings, string[] tokens, int numNeighbors)
        {
            Console.WriteLine("### Nearest Neighbors ###");

            foreach (var token in tokens)
            {
                DisplayNeighbors(wordEmbeddings, token, numNeighbors, true);  // Cosine similarity
                DisplayNeighbors(wordEmbeddings, token, numNeighbors, false); // Euclidean distance
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays the nearest neighbors for a single token.
        /// </summary>
        /// <param name="wordEmbeddings">The word embeddings instance.</param>
        /// <param name="token">The token to analyze.</param>
        /// <param name="numNeighbors">Number of neighbors to retrieve.</param>
        /// <param name="useCosine">Whether to use cosine similarity (true) or Euclidean distance (false).</param>
        static void DisplayNeighbors(WordEmbeddings wordEmbeddings, string token, int numNeighbors, bool useCosine)
        {
            string similarityType = useCosine ? "cosine" : "Euclidean";
            Console.WriteLine($"Neighbors of '{token}' using {similarityType}:");

            var neighbors = wordEmbeddings.FindNearestNeighbors(token, numNeighbors, useCosine);

            if (neighbors == null || neighbors.Count == 0)
            {
                Console.WriteLine($"'{token}' not found in embeddings.");
            }
            else
            {
                foreach (var neighbor in neighbors)
                {
                    Console.WriteLine(neighbor);
                }
            }
        }
    }
}
