using LibraryGlobalVectors1Sep2024;

namespace ConsoleGloVeNearestNeighbors3Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// GloVe is an unsupervised learning algorithm for obtaining vector representations for words. 
        /// See: https://nlp.stanford.edu/projects/glove/
        /// </summary>
        static void Main(string[] args)
        {
            // Path to the GloVe embedding file
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50; // Dimensionality of the GloVe embeddings

            // Load pretrained embeddings
            EmbeddingLoader embeddingLoader = new(filePath, embeddingDim);
            var wordEmbeddings = new WordEmbeddings(embeddingLoader.Embeddings);

            // Define tokens to test
            string[] tokens = {
                "comparative", "city", "strong", "paris", "man", "Honolulu", "toad", "queen", "IBM", "litoria",
                "french", "hello", "world", "clear", "this", "soft", "dark", "is", "a", "test", "france",
                "belgium", "clearer", "softer", "darker", "frog", "frogs", "lizard", "Nashville", "uncle",
                "woman", "sir", "United", "stronger", "superlative", "company", "brother", "sister",
                "observation", "elevator", "blood", "pressure", "town", "phone", "hamburger", "solid",
                "gas", "water", "fashion", "ice", "steam"
            };

            // Perform analogies
            TestAnalogies(wordEmbeddings);

            // Find neighbors for each token
            FindAndDisplayNeighbors(wordEmbeddings, tokens, 1);

            /*
            Loading embeddings...
            Embeddings loaded successfully!
            'king' - 'man' + 'woman' = 'girl'
            'man' - 'king' + 'woman' = 'queen'
            'ice' - 'steam' + 'liquid' = 'condensing'
            'uncle' - 'man' + 'woman' = 'girl'
            'strong' - 'dark' + 'light' = 'shades'
            'electron' - 'proton' + 'atom' = 'qatargas'
            'man' - 'woman' + 'lord' = 'earl'

            Neighbors of 'comparative' using cosine similarity:
            anthropology
            Neighbors of 'comparative' using Euclidean distance:
            sociological

            Neighbors of 'city' using cosine similarity:
            town
            Neighbors of 'city' using Euclidean distance:
            town

            Neighbors of 'strong' using cosine similarity:
            stronger
            Neighbors of 'strong' using Euclidean distance:
            stronger

            Neighbors of 'paris' using cosine similarity:
            prohertrib
            Neighbors of 'paris' using Euclidean distance:
            prohertrib

            Neighbors of 'man' using cosine similarity:
            woman
            Neighbors of 'man' using Euclidean distance:
            woman

            Word not found in embeddings.
            Neighbors of 'Honolulu' using cosine similarity:
            No neighbors found.
            Word not found in embeddings.
            Neighbors of 'Honolulu' using Euclidean distance:
            No neighbors found.

            Neighbors of 'toad' using cosine similarity:
            toads
            Neighbors of 'toad' using Euclidean distance:
            toads

            Neighbors of 'queen' using cosine similarity:
            princess
            Neighbors of 'queen' using Euclidean distance:
            princess

            Word not found in embeddings.
            Neighbors of 'IBM' using cosine similarity:
            No neighbors found.
            Word not found in embeddings.
            Neighbors of 'IBM' using Euclidean distance:
            No neighbors found.

            Neighbors of 'litoria' using cosine similarity:
            limnonectes
            Neighbors of 'litoria' using Euclidean distance:
            limnonectes

            Neighbors of 'french' using cosine similarity:
            france
            Neighbors of 'french' using Euclidean distance:
            france

            Neighbors of 'hello' using cosine similarity:
            goodbye
            Neighbors of 'hello' using Euclidean distance:
            goodbye

            Neighbors of 'world' using cosine similarity:
            europe
            Neighbors of 'world' using Euclidean distance:
            europe

            Neighbors of 'clear' using cosine similarity:
            but
            Neighbors of 'clear' using Euclidean distance:
            yet

            Neighbors of 'this' using cosine similarity:
            it
            Neighbors of 'this' using Euclidean distance:
            it

            Neighbors of 'soft' using cosine similarity:
            thin
            Neighbors of 'soft' using Euclidean distance:
            thin

            Neighbors of 'dark' using cosine similarity:
            shadows
            Neighbors of 'dark' using Euclidean distance:
            shadows

            Neighbors of 'is' using cosine similarity:
            this
            Neighbors of 'is' using Euclidean distance:
            this

            Neighbors of 'a' using cosine similarity:
            another
            Neighbors of 'a' using Euclidean distance:
            an

            Neighbors of 'test' using cosine similarity:
            tests
            Neighbors of 'test' using Euclidean distance:
            tests

            Neighbors of 'france' using cosine similarity:
            french
            Neighbors of 'france' using Euclidean distance:
            french

            Neighbors of 'belgium' using cosine similarity:
            netherlands
            Neighbors of 'belgium' using Euclidean distance:
            netherlands

            Neighbors of 'clearer' using cosine similarity:
            reassuring
            Neighbors of 'clearer' using Euclidean distance:
            reassuring

            Neighbors of 'softer' using cosine similarity:
            slightly
            Neighbors of 'softer' using Euclidean distance:
            softening

            Neighbors of 'darker' using cosine similarity:
            dark
            Neighbors of 'darker' using Euclidean distance:
            dark

            Neighbors of 'frog' using cosine similarity:
            snake
            Neighbors of 'frog' using Euclidean distance:
            snake

            Neighbors of 'frogs' using cosine similarity:
            horned
            Neighbors of 'frogs' using Euclidean distance:
            salamanders

            Neighbors of 'lizard' using cosine similarity:
            parrot
            Neighbors of 'lizard' using Euclidean distance:
            parrot

            Word not found in embeddings.
            Neighbors of 'Nashville' using cosine similarity:
            No neighbors found.
            Word not found in embeddings.
            Neighbors of 'Nashville' using Euclidean distance:
            No neighbors found.

            Neighbors of 'uncle' using cosine similarity:
            son
            Neighbors of 'uncle' using Euclidean distance:
            cousin

            Neighbors of 'woman' using cosine similarity:
            girl
            Neighbors of 'woman' using Euclidean distance:
            girl

            Neighbors of 'sir' using cosine similarity:
            edward
            Neighbors of 'sir' using Euclidean distance:
            edward

            Word not found in embeddings.
            Neighbors of 'United' using cosine similarity:
            No neighbors found.
            Word not found in embeddings.
            Neighbors of 'United' using Euclidean distance:
            No neighbors found.

            Neighbors of 'stronger' using cosine similarity:
            weaker
            Neighbors of 'stronger' using Euclidean distance:
            weaker

            Neighbors of 'superlative' using cosine similarity:
            masterly
            Neighbors of 'superlative' using Euclidean distance:
            exemplifying

            Neighbors of 'company' using cosine similarity:
            firm
            Neighbors of 'company' using Euclidean distance:
            firm

            Neighbors of 'brother' using cosine similarity:
            son
            Neighbors of 'brother' using Euclidean distance:
            cousin

            Neighbors of 'sister' using cosine similarity:
            wife
            Neighbors of 'sister' using Euclidean distance:
            wife

            Neighbors of 'observation' using cosine similarity:
            reconnaissance
            Neighbors of 'observation' using Euclidean distance:
            observing

            Neighbors of 'elevator' using cosine similarity:
            escalator
            Neighbors of 'elevator' using Euclidean distance:
            escalator

            Neighbors of 'blood' using cosine similarity:
            causes
            Neighbors of 'blood' using Euclidean distance:
            causes

            Neighbors of 'pressure' using cosine similarity:
            ease
            Neighbors of 'pressure' using Euclidean distance:
            ease

            Neighbors of 'town' using cosine similarity:
            village
            Neighbors of 'town' using Euclidean distance:
            village

            Neighbors of 'phone' using cosine similarity:
            telephone
            Neighbors of 'phone' using Euclidean distance:
            telephone

            Neighbors of 'hamburger' using cosine similarity:
            abendblatt
            Neighbors of 'hamburger' using Euclidean distance:
            burger

            Neighbors of 'solid' using cosine similarity:
            consistent
            Neighbors of 'solid' using Euclidean distance:
            consistent

            Neighbors of 'gas' using cosine similarity:
            fuel
            Neighbors of 'gas' using Euclidean distance:
            fuel

            Neighbors of 'water' using cosine similarity:
            dry
            Neighbors of 'water' using Euclidean distance:
            dry

            Neighbors of 'fashion' using cosine similarity:
            style
            Neighbors of 'fashion' using Euclidean distance:
            fashions

            Neighbors of 'ice' using cosine similarity:
            hot
            Neighbors of 'ice' using Euclidean distance:
            hot

            Neighbors of 'steam' using cosine similarity:
            engines
            Neighbors of 'steam' using Euclidean distance:
            boiler
            */

            Console.ReadLine();
        }

        /// <summary>
        /// Test a set of word analogies.
        /// </summary>
        static void TestAnalogies(WordEmbeddings wordEmbeddings)
        {
            var analogies = new List<(string a, string b, string c)>
            {
                ("king", "man", "woman"),
                ("man", "king", "woman"),
                ("ice", "steam", "liquid"),
                ("uncle", "man", "woman"),
                ("strong", "dark", "light"),
                ("electron", "proton", "atom"),
                ("man", "woman", "lord")
            };

            foreach (var (a, b, c) in analogies)
            {
                string result = wordEmbeddings.FindAnalogy(a, b, c);
                Console.WriteLine($"'{a}' - '{b}' + '{c}' = '{result}'");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Find and display nearest neighbors for a list of tokens.
        /// </summary>
        static void FindAndDisplayNeighbors(WordEmbeddings wordEmbeddings, string[] tokens, int n)
        {
            foreach (var token in tokens)
            {
                // Find neighbors using cosine similarity
                var cosineNeighbors = wordEmbeddings.FindNearestNeighbors(token, n);
                Console.WriteLine($"Neighbors of '{token}' using cosine similarity:");
                DisplayNeighbors(cosineNeighbors);

                // Find neighbors using Euclidean distance
                var euclideanNeighbors = wordEmbeddings.FindNearestNeighbors(token, n, useCosineSimilarity: false);
                Console.WriteLine($"Neighbors of '{token}' using Euclidean distance:");
                DisplayNeighbors(euclideanNeighbors);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Display a list of neighbors.
        /// </summary>
        static void DisplayNeighbors(IEnumerable<string> neighbors)
        {
            if (neighbors == null || !neighbors.Any())
            {
                Console.WriteLine("No neighbors found.");
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

