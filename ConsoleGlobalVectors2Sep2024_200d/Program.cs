using System.Globalization;

namespace ConsoleGlobalVectors2Sep2024_200d
{
    class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";

            var targetWords = new List<string> { "ice", "steam" };
            var contextWords = new List<string> { "solid", "gas", "water", "fashion" };

            // Read embeddings from the GloVe file
            var embeddings = LoadGloveEmbeddings(gloveFilePath, targetWords.Concat(contextWords).ToList());

            if (embeddings.Count == 0)
            {
                Console.WriteLine("No embeddings found. Ensure the file path and target/context words are correct.");
                return;
            }

            // Compute probabilities (cosine similarity) and ratios
            Console.WriteLine($"{"Context Word",-10} {"P(k|ice)",-10} {"P(k|steam)",-10} {"Ratio",-10}");
            foreach (var contextWord in contextWords)
            {
                if (embeddings.ContainsKey("ice") && embeddings.ContainsKey("steam") && embeddings.ContainsKey(contextWord))
                {
                    double pkIce = CosineSimilarity(embeddings["ice"], embeddings[contextWord]);
                    double pkSteam = CosineSimilarity(embeddings["steam"], embeddings[contextWord]);
                    double ratio = pkIce / pkSteam;

                    Console.WriteLine($"{contextWord,-10} {pkIce,-10:E2} {pkSteam,-10:E2} {ratio,-10:F2}");
                }
                else
                {
                    Console.WriteLine($"{contextWord,-10} Missing embeddings for one or more words.");
                }
            }

            /*
            Context Word P(k|ice)   P(k|steam) Ratio
            solid      3,20E-001  2,28E-001  1,40
            gas        2,74E-001  4,01E-001  0,68
            water      4,76E-001  4,76E-001  1,00
            fashion    1,55E-001  7,50E-002  2,06
            */

            Console.ReadLine();
        }

        // Method to load GloVe embeddings for specified words
        static Dictionary<string, List<double>> LoadGloveEmbeddings(string filePath, List<string> wordsToFind)
        {
            var embeddings = new Dictionary<string, List<double>>();
            var wordsToFindSet = new HashSet<string>(wordsToFind);

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return embeddings;
            }

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var tokens = line.Split(' ');
                    string word = tokens[0];
                    if (wordsToFindSet.Contains(word))
                    {
                        var vector = tokens.Skip(1).Select(token => double.Parse(token, CultureInfo.InvariantCulture)).ToList();
                        //var vector = tokens.Skip(1).Select(double.Parse).ToList();
                        embeddings[word] = vector;

                        // Stop if all words are found
                        if (embeddings.Count == wordsToFind.Count)
                            break;
                    }
                }
            }
            return embeddings;
        }

        // Method to calculate cosine similarity between two vectors
        static double CosineSimilarity(List<double> vec1, List<double> vec2)
        {
            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < vec1.Count; i++)
            {
                dotProduct += vec1[i] * vec2[i];
                magnitude1 += vec1[i] * vec1[i];
                magnitude2 += vec2[i] * vec2[i];
            }

            return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
        }
    }

}
