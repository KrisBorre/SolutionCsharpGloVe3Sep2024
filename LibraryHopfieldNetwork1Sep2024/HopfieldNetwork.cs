namespace LibraryHopfieldNetwork1Sep2024
{
    // ChatGPT, adapted
    // Represents a Hopfield Network, a type of recurrent neural network used for pattern recognition
    public class HopfieldNetwork
    {
        private int _size; // Number of neurons in the network
        private double[,] _weights; // Weight matrix to store connections between neurons
        private bool _verbose;

        // Constructor to initialize the network with a specific number of neurons
        public HopfieldNetwork(int size, bool verbose = false)
        {
            _size = size;
            _weights = new double[_size, _size]; // Square matrix of weights
            _verbose = verbose;
            if (_verbose) Console.WriteLine($"Hopfield Network initialized with {_size} neurons.");
        }

        // Train the network with binary patterns (-1, +1) using Hebbian learning
        public void Train(List<int[]> patterns)
        {
            if (_verbose) Console.WriteLine("Starting training...");
            Array.Clear(_weights, 0, _weights.Length); // Clear the weight matrix
            if (_verbose) Console.WriteLine($"Weight matrix cleared. Training on {patterns.Count} patterns.");
            foreach (var pattern in patterns)
            {
                if (_verbose) Console.WriteLine($"Training with pattern: [{string.Join(", ", pattern)}]");
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (i != j) // Skip diagonal elements (self-connections)
                        {
                            _weights[i, j] += pattern[i] * pattern[j]; // Update weights
                        }
                    }
                }
            }
            if (_verbose) Console.WriteLine("Training completed. Weight matrix updated.");
        }

        // Retrieve a pattern using asynchronous updates to find a stable state (attractor)
        public int[] Retrieve(int[] pattern)
        {
            if (_verbose) Console.WriteLine($"Retrieving pattern: [{string.Join(", ", pattern)}]");
            int[] retrievedPattern = (int[])pattern.Clone(); // Copy the input pattern
            for (int iteration = 0; iteration < 10; iteration++) // Perform up to 10 updates
            {
                if (_verbose) Console.WriteLine($"Iteration {iteration + 1}...");
                for (int i = 0; i < _size; i++) // Update each neuron asynchronously
                {
                    double sum = 0;
                    for (int j = 0; j < _size; j++) // Compute weighted input for neuron i
                    {
                        sum += _weights[i, j] * retrievedPattern[j];
                    }
                    // Apply activation function: +1 if sum >= 0, otherwise -1
                    retrievedPattern[i] = sum >= 0 ? 1 : -1;
                }
                if (_verbose) Console.WriteLine($"Updated pattern: [{string.Join(", ", retrievedPattern)}]");
            }
            if (_verbose) Console.WriteLine("Retrieval completed.");
            return retrievedPattern; // Return the stabilized pattern
        }

        // Introduces noise into a binary pattern by flipping bits with probability = temperature
        public static int[] AddNoise(int[] pattern, double temperature, bool verbose = false)
        {
            if (verbose) Console.WriteLine($"Adding noise to pattern: [{string.Join(", ", pattern)}] with temperature {temperature}");
            Random rand = new Random();
            int[] noisyPattern = (int[])pattern.Clone(); // Copy the original pattern
            for (int i = 0; i < noisyPattern.Length; i++)
            {
                if (rand.NextDouble() < temperature) // Flip bit with probability = temperature
                {
                    noisyPattern[i] = noisyPattern[i] == 1 ? -1 : 1;
                }
            }
            if (verbose) Console.WriteLine($"Noisy pattern: [{string.Join(", ", noisyPattern)}]");
            return noisyPattern;
        }

        // Calculates accuracy as the proportion of bits that match the original pattern
        public static double CalculateAccuracy(int[] original, int[] retrieved, bool verbose = false)
        {
            if (verbose) Console.WriteLine($"Calculating accuracy between patterns:");
            if (verbose) Console.WriteLine($"Original:  [{string.Join(", ", original)}]");
            if (verbose) Console.WriteLine($"Retrieved: [{string.Join(", ", retrieved)}]");
            int matches = 0; // Count matching bits
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] == retrieved[i])
                {
                    matches++;
                }
            }
            double accuracy = (double)matches / original.Length; // Return proportion of matching bits
            if (verbose) Console.WriteLine($"Accuracy: {accuracy:F2}");
            return accuracy;
        }
    }
}


