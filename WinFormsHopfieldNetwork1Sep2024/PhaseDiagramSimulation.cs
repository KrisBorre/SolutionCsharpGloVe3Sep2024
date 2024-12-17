using LibraryHopfieldNetwork1Sep2024;

namespace WinFormsHopfieldNetwork1Sep2024
{
    public class PhaseDiagramSimulation
    {
        public static double[,] RunSimulation(int size, int maxPatterns, double[] temperatures)
        {
            double[,] accuracyResults = new double[maxPatterns, temperatures.Length];
            List<int[]> patterns = GenerateRandomPatterns(size, maxPatterns);

            for (int numPatterns = 1; numPatterns <= maxPatterns; numPatterns++)
            {
                HopfieldNetwork network = new HopfieldNetwork(size);
                network.Train(patterns.Take(numPatterns).ToList());

                for (int tempIndex = 0; tempIndex < temperatures.Length; tempIndex++)
                {
                    double temperature = temperatures[tempIndex];
                    double totalAccuracy = 0;

                    foreach (var pattern in patterns.Take(numPatterns))
                    {
                        int[] noisyPattern = HopfieldNetwork.AddNoise(pattern, temperature);
                        int[] retrievedPattern = network.Retrieve(noisyPattern);
                        totalAccuracy += HopfieldNetwork.CalculateAccuracy(pattern, retrievedPattern);
                    }
                    accuracyResults[numPatterns - 1, tempIndex] = totalAccuracy / numPatterns;
                }
            }
            return accuracyResults;
        }

        private static List<int[]> GenerateRandomPatterns(int size, int count)
        {
            Random rand = new Random();
            List<int[]> patterns = new List<int[]>();
            for (int i = 0; i < count; i++)
            {
                int[] pattern = new int[size];
                for (int j = 0; j < size; j++)
                {
                    pattern[j] = rand.Next(2) == 0 ? -1 : 1;
                }
                patterns.Add(pattern);
            }
            return patterns;
        }
    }

}
