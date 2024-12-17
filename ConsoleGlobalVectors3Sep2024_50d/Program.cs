using System.Globalization;

namespace ConsoleGlobalVectors3Sep2024_50d
{
    internal class Program
    {
        /// <summary>
        /// ChatGPT
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";

            // Load embeddings from the GloVe file
            var embeddings = LoadEmbeddings(gloveFilePath);

            // Words to compare
            string word1 = "ice";
            string word2 = "steam";

            // Check if words exist in the embeddings
            if (!embeddings.ContainsKey(word1) || !embeddings.ContainsKey(word2))
            {
                Console.WriteLine("One or both words not found in the embeddings.");
                return;
            }

            // Calculate semantic relationships
            var semanticRatios = CalculateSemanticRatios(embeddings, word1, word2);

            Console.WriteLine($"Top 10 highest semantic ratios for words '{word1}' and '{word2}':");
            foreach (var item in semanticRatios.OrderByDescending(x => x.Value).Take(10))
            {
                Console.WriteLine($"Word: {item.Key}, Ratio: {item.Value:F4}");
            }

            Console.WriteLine($"\nTop 10 lowest semantic ratios for words '{word1}' and '{word2}':");
            foreach (var item in semanticRatios.OrderBy(x => x.Value).Take(10))
            {
                Console.WriteLine($"Word: {item.Key}, Ratio: {item.Value:F4}");
            }

            /*
            Top 10 highest semantic ratios for words 'ice' and 'steam':
            Word: digamma, Ratio: 44867,2082
            Word: herry, Ratio: 43089,9397
            Word: janjigian, Ratio: 29235,9038
            Word: congener, Ratio: 15578,2441
            Word: merieux, Ratio: 15003,6797
            Word: 50-kilometer, Ratio: 14710,3616
            Word: wycherley, Ratio: 11740,1484
            Word: cheney, Ratio: 11452,0458
            Word: 116.04, Ratio: 10764,0969
            Word: inconvenienced, Ratio: 10608,0960

            Top 10 lowest semantic ratios for words 'ice' and 'steam':
            Word: economizing, Ratio: -160611,5203
            Word: strang, Ratio: -88446,3026
            Word: defreitas, Ratio: -24245,4861
            Word: luffy, Ratio: -15945,8914
            Word: leguen, Ratio: -15785,6434
            Word: z4, Ratio: -11992,1667
            Word: negated, Ratio: -10567,4311
            Word: 63-foot, Ratio: -9780,8840
            Word: semlow, Ratio: -9592,6998
            Word: 0200gmt, Ratio: -8623,4115
            */

            Console.WriteLine($"\nFirst 10 semantic ratios between 0.99 and 1.01 for words '{word1}' and '{word2}':");
            foreach (var item in semanticRatios.Where(x => x.Value >= 0.99 && x.Value <= 1.01).Take(10))
            {
                Console.WriteLine($"Word: {item.Key}, Ratio: {item.Value:F4}");
            }

            /*
            First 10 semantic ratios between 0.99 and 1.01 for words 'ice' and 'steam':
            Word: war, Ratio: 0,9953
            Word: major, Ratio: 1,0027
            Word: economic, Ratio: 0,9937
            Word: office, Ratio: 0,9919
            Word: small, Ratio: 0,9906
            Word: program, Ratio: 1,0029
            Word: april, Ratio: 0,9915
            Word: bill, Ratio: 0,9934
            Word: move, Ratio: 1,0014
            Word: land, Ratio: 1,0017
            */

            // Display ratios for specific words
            string[] specificWords = { "solid", "gas", "water", "fashion" };
            Console.WriteLine($"\nSemantic ratios for specific words ('solid', 'gas', 'water', 'fashion') with respect to '{word1}' and '{word2}':");
            foreach (var word in specificWords)
            {
                if (semanticRatios.ContainsKey(word))
                {
                    Console.WriteLine($"Word: {word}, Ratio: {semanticRatios[word]:F4}");
                }
                else
                {
                    Console.WriteLine($"Word: {word} not found in the embeddings.");
                }
            }

            /*
            Semantic ratios for specific words ('solid', 'gas', 'water', 'fashion') with respect to 'ice' and 'steam':
            Word: solid, Ratio: 1,1566
            Word: gas, Ratio: 0,9054
            Word: water, Ratio: 0,9673
            Word: fashion, Ratio: 4,4905
            */

            // Volgens the tabel is
            // solid, ratio = 8.9
            // gas, ratio = 8.5E-2
            // water, ratio = 1.36
            // fashion, ratio = 0.96                

            Console.ReadLine();
        }

        // Method to load word embeddings from a GloVe file
        static Dictionary<string, float[]> LoadEmbeddings(string filePath)
        {
            var embeddings = new Dictionary<string, float[]>();

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');
                string word = parts[0];
                float[] vector = parts.Skip(1).Select(token => float.Parse(token, CultureInfo.InvariantCulture)).ToArray();
                embeddings[word] = vector;
            }

            return embeddings;
        }

        // Method to calculate semantic ratios
        static Dictionary<string, double> CalculateSemanticRatios(
            Dictionary<string, float[]> embeddings,
            string word1,
            string word2)
        {
            var ratios = new Dictionary<string, double>();

            foreach (var entry in embeddings)
            {
                string word = entry.Key;
                float[] vectorK = entry.Value;

                // Skip if the word is the same as the target words
                if (word == word1 || word == word2) continue;

                // Compute dot products and probabilities
                double dot1 = DotProduct(embeddings[word1], vectorK);
                double dot2 = DotProduct(embeddings[word2], vectorK);

                // Avoid division by zero
                if (dot2 == 0) continue;

                // Calculate the ratio
                double ratio = dot1 / dot2;
                ratios[word] = ratio;
            }

            return ratios;
        }

        // Helper method to calculate dot product
        static double DotProduct(float[] vec1, float[] vec2)
        {
            return vec1.Zip(vec2, (a, b) => a * b).Sum();
        }
    }
}