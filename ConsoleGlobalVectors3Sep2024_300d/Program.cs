using System.Globalization;

namespace ConsoleGlobalVectors3Sep2024_300d
{
    internal class Program
    {
        /// <summary>
        /// ChatGPT
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";

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
            Word: 86.87, Ratio: 636068,5940
            Word: untuned, Ratio: 117064,7787
            Word: eagleton, Ratio: 72086,0774
            Word: noc, Ratio: 68306,5174
            Word: arteriole, Ratio: 64538,6776
            Word: shilkret, Ratio: 59396,3667
            Word: wildenstein, Ratio: 38131,0362
            Word: 6b, Ratio: 25907,3220
            Word: guimard, Ratio: 19470,3991
            Word: piette, Ratio: 17048,6533

            Top 10 lowest semantic ratios for words 'ice' and 'steam':
            Word: independant, Ratio: -45872,5805
            Word: z-rock, Ratio: -29957,1910
            Word: dungey, Ratio: -18978,3857
            Word: tuas, Ratio: -15850,7184
            Word: bhavna, Ratio: -14323,4049
            Word: wildwoods, Ratio: -14223,2105
            Word: fubu, Ratio: -13191,9875
            Word: departements, Ratio: -12635,7649
            Word: unformatted, Ratio: -11577,7444
            Word: quinns, Ratio: -11273,7791 
            */

            Console.WriteLine($"\nFirst 10 semantic ratios between 0.99 and 1.01 for words '{word1}' and '{word2}':");
            foreach (var item in semanticRatios.Where(x => x.Value >= 0.99 && x.Value <= 1.01).Take(10))
            {
                Console.WriteLine($"Word: {item.Key}, Ratio: {item.Value:F4}");
            }

            /*
            First 10 semantic ratios between 0.99 and 1.01 for words 'ice' and 'steam':
            Word: military, Ratio: 0,9996
            Word: six, Ratio: 0,9951
            Word: town, Ratio: 1,0036
            Word: april, Ratio: 1,0088
            Word: need, Ratio: 0,9931
            Word: water, Ratio: 1,0068
            Word: administration, Ratio: 0,9959
            Word: board, Ratio: 1,0033
            Word: opening, Ratio: 0,9904
            Word: society, Ratio: 1,0075 
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
            Word: solid, Ratio: 1,4176
            Word: gas, Ratio: 0,6140
            Word: water, Ratio: 1,0068
            Word: fashion, Ratio: -31,8903 
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