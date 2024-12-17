namespace LibraryPerceptron1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class Perceptron
    {
        public double[] Weights;
        private double learningRate;

        public Perceptron(int inputSize, double learningRate = 0.1)
        {
            this.Weights = new double[inputSize + 1]; // Including one weight for the bias term
            this.learningRate = learningRate;
            InitializeWeights();
        }

        // Initialize weights to small random values
        private void InitializeWeights()
        {
            Random rand = new Random();
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rand.NextDouble() * 2 - 1; // Random value between -1 and 1
            }
        }

        // Activation function (sign function)
        private int Activate(double sum)
        {
            return sum >= 0 ? 1 : -1;
        }

        // Train the perceptron with given inputs and expected output
        public void Train(double[] inputs, int expectedOutput)
        {
            double sum = Weights[0]; // Start with bias
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += Weights[i + 1] * inputs[i];
            }
            int prediction = Activate(sum);
            int error = expectedOutput - prediction;

            // Update weights if there's an error
            Weights[0] += learningRate * error; // Update bias
            for (int i = 0; i < inputs.Length; i++)
            {
                Weights[i + 1] += learningRate * error * inputs[i];
            }
        }

        // Predict the output for a given input
        public int Predict(double[] inputs)
        {
            double sum = Weights[0]; // Start with bias
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += Weights[i + 1] * inputs[i];
            }
            return Activate(sum);
        }

        // Display weights for debugging purposes
        public void DisplayWeights()
        {
            Console.WriteLine("Weights:");
            for (int i = 0; i < Weights.Length; i++)
            {
                Console.WriteLine($"w[{i}] = {Weights[i]}");
            }
        }
    }
}
