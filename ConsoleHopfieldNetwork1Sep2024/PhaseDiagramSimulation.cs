using LibraryHopfieldNetwork1Sep2024;

namespace ConsoleHopfieldNetwork1Sep2024
{
    // ChatGPT, adapted    
    // Simulates the Hopfield Network's behavior under different noise levels and pattern counts
    public class PhaseDiagramSimulation
    {
        public static void Run()
        {
            int size = 100; // Number of neurons in the network
            int maxPatterns = 15; // Maximum patterns to store in the network
            double[] temperatures = { 0.0, 0.1, 0.2, 0.3 }; // Noise levels for testing

            PrintMessage("Generating random patterns for the simulation...", ConsoleColor.White);
            List<int[]> patterns = GenerateRandomPatterns(size, maxPatterns);
            PrintMessage($"Generated {patterns.Count} patterns of size {size}.", ConsoleColor.White);

            bool verbose = false;

            // Test network performance for different numbers of stored patterns
            foreach (int numPatterns in Enumerable.Range(1, maxPatterns))
            {
                if (numPatterns == maxPatterns) verbose = true;
                else verbose = false;

                if (verbose) PrintMessage($"\nTesting with {numPatterns} patterns...", ConsoleColor.Green);
                HopfieldNetwork network = new HopfieldNetwork(size, verbose);
                network.Train(patterns.Take(numPatterns).ToList()); // Train on a subset of patterns
                if (verbose) PrintMessage($"Network trained with {numPatterns} patterns.", ConsoleColor.Green);

                // Evaluate performance for each noise level
                foreach (double temperature in temperatures)
                {
                    if (numPatterns == maxPatterns && temperature == 0.3) verbose = true;
                    else verbose = false;

                    if (verbose) PrintMessage($"\nEvaluating performance at temperature {temperature:F1}...", ConsoleColor.Blue);
                    double averageAccuracy = 0; // Average accuracy across patterns
                    int patternIndex = 0;

                    foreach (var pattern in patterns.Take(numPatterns))
                    {
                        patternIndex++;
                        if (verbose) PrintMessage($"\nProcessing pattern {patternIndex}/{numPatterns}...", ConsoleColor.Yellow);
                        int[] noisyPattern = HopfieldNetwork.AddNoise(pattern, temperature); // Add noise
                        int[] retrievedPattern = network.Retrieve(noisyPattern); // Retrieve pattern
                        double accuracy = HopfieldNetwork.CalculateAccuracy(pattern, retrievedPattern); // Calculate accuracy
                        if (verbose) PrintMessage($"Pattern accuracy: {accuracy:F2}", ConsoleColor.Yellow);
                        averageAccuracy += accuracy;
                    }

                    averageAccuracy /= numPatterns; // Compute mean accuracy
                    if (verbose) PrintMessage($"Average accuracy for {numPatterns} patterns at temperature {temperature:F1}: {averageAccuracy:F2}", ConsoleColor.Magenta);
                }
            }
            PrintMessage("Simulation completed.", ConsoleColor.Green);
        }

        // Generates a set of random binary patterns (-1, +1) of specified size and count
        private static List<int[]> GenerateRandomPatterns(int size, int count, bool verbose = false)
        {
            Random rand = new Random();
            List<int[]> patterns = new List<int[]>();
            if (verbose) PrintMessage($"Generating {count} patterns of size {size}...");
            for (int i = 0; i < count; i++)
            {
                int[] pattern = new int[size];
                for (int j = 0; j < size; j++) // Randomly assign -1 or +1 to each bit
                {
                    pattern[j] = rand.Next(2) == 0 ? -1 : 1;
                }
                patterns.Add(pattern);
                if (verbose) PrintMessage($"Generated pattern {i + 1}: [{string.Join(", ", pattern)}]");
            }
            if (verbose) PrintMessage("Pattern generation completed.");
            return patterns;
        }

        // Helper function to print messages in a specific color
        private static void PrintMessage(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
