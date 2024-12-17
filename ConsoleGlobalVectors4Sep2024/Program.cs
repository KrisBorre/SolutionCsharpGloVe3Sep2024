using System.Globalization;

namespace ConsoleGlobalVectors4Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            string gloveFile = "../../../../../../../GloVe/glove.6B.50d.txt";

            var embeddings = LoadEmbeddings(gloveFile);

            #region Sun versus Moon
            {
                // Word pairs to test
                var wordPair = ("sun", "moon");

                // Context words for comparisons
                var contextWords = new[] { "bright", "dark", "light", "fashion" };

                // Ensure the word pair and context words exist in the embeddings
                if (!embeddings.ContainsKey(wordPair.Item1) || !embeddings.ContainsKey(wordPair.Item2))
                {
                    Console.WriteLine($"Error: One or both of the words '{wordPair.Item1}' and '{wordPair.Item2}' are not in the embeddings.");
                    return;
                }

                foreach (var word in contextWords)
                {
                    if (!embeddings.ContainsKey(word))
                    {
                        Console.WriteLine($"Error: Context word '{word}' is not in the embeddings.");
                        return;
                    }
                }

                // Calculate semantic ratios
                var semanticRatios = new Dictionary<string, double>();
                foreach (var contextWord in contextWords)
                {
                    double dotProduct1 = DotProduct(embeddings[wordPair.Item1], embeddings[contextWord]);
                    double dotProduct2 = DotProduct(embeddings[wordPair.Item2], embeddings[contextWord]);

                    if (dotProduct2 != 0)
                    {
                        double ratio = dotProduct1 / dotProduct2;
                        semanticRatios[contextWord] = ratio;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Dot product for '{contextWord}' with '{wordPair.Item2}' is zero, skipping...");
                    }
                }

                // Display results
                Console.WriteLine($"Semantic ratios for words '{wordPair.Item1}' and '{wordPair.Item2}':");
                foreach (var item in semanticRatios)
                {
                    Console.WriteLine($"Context Word: {item.Key}, Ratio: {item.Value:F4}");
                }

            }
            #endregion

            #region Winter versus Summer
            {
                // Word pairs to test
                var wordPair = ("winter", "summer");

                // Context words for comparisons
                var contextWords = new[] { "cold", "hot", "weather", "style" };

                // Ensure the word pair and context words exist in the embeddings
                if (!embeddings.ContainsKey(wordPair.Item1) || !embeddings.ContainsKey(wordPair.Item2))
                {
                    Console.WriteLine($"Error: One or both of the words '{wordPair.Item1}' and '{wordPair.Item2}' are not in the embeddings.");
                    return;
                }

                foreach (var word in contextWords)
                {
                    if (!embeddings.ContainsKey(word))
                    {
                        Console.WriteLine($"Error: Context word '{word}' is not in the embeddings.");
                        return;
                    }
                }

                // Calculate semantic ratios
                var semanticRatios = new Dictionary<string, double>();
                foreach (var contextWord in contextWords)
                {
                    double dotProduct1 = DotProduct(embeddings[wordPair.Item1], embeddings[contextWord]);
                    double dotProduct2 = DotProduct(embeddings[wordPair.Item2], embeddings[contextWord]);

                    if (dotProduct2 != 0)
                    {
                        double ratio = dotProduct1 / dotProduct2;
                        semanticRatios[contextWord] = ratio;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Dot product for '{contextWord}' with '{wordPair.Item2}' is zero, skipping...");
                    }
                }

                // Display results
                Console.WriteLine($"Semantic ratios for words '{wordPair.Item1}' and '{wordPair.Item2}':");
                foreach (var item in semanticRatios)
                {
                    Console.WriteLine($"Context Word: {item.Key}, Ratio: {item.Value:F4}");
                }

            }
            #endregion

            #region Dog versus Cat
            {
                // Word pairs to test
                var wordPair = ("dog", "cat");

                // Context words for comparisons
                var contextWords = new[] { "bark", "meow", "pet", "fashion" };

                // Ensure the word pair and context words exist in the embeddings
                if (!embeddings.ContainsKey(wordPair.Item1) || !embeddings.ContainsKey(wordPair.Item2))
                {
                    Console.WriteLine($"Error: One or both of the words '{wordPair.Item1}' and '{wordPair.Item2}' are not in the embeddings.");
                    return;
                }

                foreach (var word in contextWords)
                {
                    if (!embeddings.ContainsKey(word))
                    {
                        Console.WriteLine($"Error: Context word '{word}' is not in the embeddings.");
                        return;
                    }
                }

                // Calculate semantic ratios
                var semanticRatios = new Dictionary<string, double>();
                foreach (var contextWord in contextWords)
                {
                    double dotProduct1 = DotProduct(embeddings[wordPair.Item1], embeddings[contextWord]);
                    double dotProduct2 = DotProduct(embeddings[wordPair.Item2], embeddings[contextWord]);

                    if (dotProduct2 != 0)
                    {
                        double ratio = dotProduct1 / dotProduct2;
                        semanticRatios[contextWord] = ratio;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Dot product for '{contextWord}' with '{wordPair.Item2}' is zero, skipping...");
                    }
                }

                // Display results
                Console.WriteLine($"Semantic ratios for words '{wordPair.Item1}' and '{wordPair.Item2}':");
                foreach (var item in semanticRatios)
                {
                    Console.WriteLine($"Context Word: {item.Key}, Ratio: {item.Value:F4}");
                }

            }
            #endregion

            #region Coffee versus Tea
            {
                // Word pairs to test
                var wordPair = ("coffee", "tea");

                // Context words for comparisons
                var contextWords = new[] { "caffeine", "herbal", "drink", "style" };

                // Ensure the word pair and context words exist in the embeddings
                if (!embeddings.ContainsKey(wordPair.Item1) || !embeddings.ContainsKey(wordPair.Item2))
                {
                    Console.WriteLine($"Error: One or both of the words '{wordPair.Item1}' and '{wordPair.Item2}' are not in the embeddings.");
                    return;
                }

                foreach (var word in contextWords)
                {
                    if (!embeddings.ContainsKey(word))
                    {
                        Console.WriteLine($"Error: Context word '{word}' is not in the embeddings.");
                        return;
                    }
                }

                // Calculate semantic ratios
                var semanticRatios = new Dictionary<string, double>();
                foreach (var contextWord in contextWords)
                {
                    double dotProduct1 = DotProduct(embeddings[wordPair.Item1], embeddings[contextWord]);
                    double dotProduct2 = DotProduct(embeddings[wordPair.Item2], embeddings[contextWord]);

                    if (dotProduct2 != 0)
                    {
                        double ratio = dotProduct1 / dotProduct2;
                        semanticRatios[contextWord] = ratio;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Dot product for '{contextWord}' with '{wordPair.Item2}' is zero, skipping...");
                    }
                }

                // Display results
                Console.WriteLine($"Semantic ratios for words '{wordPair.Item1}' and '{wordPair.Item2}':");
                foreach (var item in semanticRatios)
                {
                    Console.WriteLine($"Context Word: {item.Key}, Ratio: {item.Value:F4}");
                }

            }
            #endregion


            #region Car versus Bicycle
            {
                // Word pairs to test
                var wordPair = ("car", "bicycle");

                // Context words for comparisons
                var contextWords = new[] { "fuel", "pedal", "transport", "fashion" };

                // Ensure the word pair and context words exist in the embeddings
                if (!embeddings.ContainsKey(wordPair.Item1) || !embeddings.ContainsKey(wordPair.Item2))
                {
                    Console.WriteLine($"Error: One or both of the words '{wordPair.Item1}' and '{wordPair.Item2}' are not in the embeddings.");
                    return;
                }

                foreach (var word in contextWords)
                {
                    if (!embeddings.ContainsKey(word))
                    {
                        Console.WriteLine($"Error: Context word '{word}' is not in the embeddings.");
                        return;
                    }
                }

                // Calculate semantic ratios
                var semanticRatios = new Dictionary<string, double>();
                foreach (var contextWord in contextWords)
                {
                    double dotProduct1 = DotProduct(embeddings[wordPair.Item1], embeddings[contextWord]);
                    double dotProduct2 = DotProduct(embeddings[wordPair.Item2], embeddings[contextWord]);

                    if (dotProduct2 != 0)
                    {
                        double ratio = dotProduct1 / dotProduct2;
                        semanticRatios[contextWord] = ratio;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Dot product for '{contextWord}' with '{wordPair.Item2}' is zero, skipping...");
                    }
                }

                // Display results
                Console.WriteLine($"Semantic ratios for words '{wordPair.Item1}' and '{wordPair.Item2}':");
                foreach (var item in semanticRatios)
                {
                    Console.WriteLine($"Context Word: {item.Key}, Ratio: {item.Value:F4}");
                }

            }
            #endregion

            /*
            Semantic ratios for words 'sun' and 'moon':
            Context Word: bright, Ratio: 1,2133
            Context Word: dark, Ratio: 1,0733
            Context Word: light, Ratio: 1,3396
            Context Word: fashion, Ratio: 1,8498
            Semantic ratios for words 'winter' and 'summer':
            Context Word: cold, Ratio: 1,0660
            Context Word: hot, Ratio: 0,9842
            Context Word: weather, Ratio: 1,1469
            Context Word: style, Ratio: 0,7942
            Semantic ratios for words 'dog' and 'cat':
            Context Word: bark, Ratio: 0,9819
            Context Word: meow, Ratio: 0,8583
            Context Word: pet, Ratio: 1,0926
            Context Word: fashion, Ratio: 1,1754
            Semantic ratios for words 'coffee' and 'tea':
            Context Word: caffeine, Ratio: 1,1450
            Context Word: herbal, Ratio: 0,8520
            Context Word: drink, Ratio: 1,1079
            Context Word: style, Ratio: 0,9578
            Semantic ratios for words 'car' and 'bicycle':
            Context Word: fuel, Ratio: 1,7498
            Context Word: pedal, Ratio: 0,9606
            Context Word: transport, Ratio: 1,2134
            Context Word: fashion, Ratio: 1,3257
            */

            Console.ReadLine();
        }

        // Function to load embeddings from the GloVe file
        static Dictionary<string, double[]> LoadEmbeddings(string filePath)
        {
            var embeddings = new Dictionary<string, double[]>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var tokens = line.Split(' ');
                    var word = tokens[0];
                    var vector = tokens.Skip(1).Select(t => double.Parse(t, CultureInfo.InvariantCulture)).ToArray();
                    embeddings[word] = vector;
                }
            }
            return embeddings;
        }

        // Function to calculate dot product between two vectors
        static double DotProduct(double[] vector1, double[] vector2)
        {
            return vector1.Zip(vector2, (x, y) => x * y).Sum();
        }
    }

}