namespace LibrarySimpleRNN1Oct2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class SimpleRNN
    {
        private int inputSize;
        private int hiddenSize;
        private int outputSize;
        private double learningRate;

        public double[,] Wx;
        public double[,] Wh;
        public double[] Bh;
        public double[] h;

        public SimpleRNN(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.001)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;
            this.learningRate = learningRate;

            Wx = InitializeMatrix(inputSize, hiddenSize);
            Wh = InitializeMatrix(hiddenSize, outputSize);
            Bh = new double[hiddenSize];
            h = new double[hiddenSize];
        }

        private double[,] InitializeMatrix(int rows, int cols)
        {
            var rand = new Random();
            double[,] matrix = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = rand.NextDouble() * 0.01; // Small random values
            return matrix;
        }

        public double[] Forward(double[] input)
        {
            h = new double[hiddenSize];
            for (int j = 0; j < hiddenSize; j++)
            {
                for (int i = 0; i < inputSize; i++)
                {
                    h[j] += input[i] * Wx[i, j];
                }
                h[j] = Math.Tanh(h[j] + Bh[j]);
            }

            var output = new double[outputSize];
            for (int k = 0; k < outputSize; k++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    output[k] += h[j] * Wh[j, k];
                }
            }
            return output;
        }

        public void Backward(double[] output, double[] target, double[] input)
        {
            double[] dOutput = new double[output.Length];
            for (int i = 0; i < output.Length; i++)
            {
                dOutput[i] = 2 * (output[i] - target[i]);
            }

            double[] dH = new double[hiddenSize];
            for (int j = 0; j < hiddenSize; j++)
            {
                for (int k = 0; k < outputSize; k++)
                {
                    dH[j] += dOutput[k] * Wh[j, k];
                    Wh[j, k] -= learningRate * dOutput[k] * h[j];
                }
                dH[j] *= (1 - h[j] * h[j]); // Derivative of tanh
            }

            for (int j = 0; j < hiddenSize; j++)
            {
                for (int i = 0; i < inputSize; i++)
                {
                    Wx[i, j] -= learningRate * dH[j] * input[i];
                }
                Bh[j] -= learningRate * dH[j];
            }
        }
    }
}
