namespace LibraryImprovedRNN1Nov2024
{
    public class ImprovedRNN
    {
        private int inputSize;
        private int hiddenSize;
        private int outputSize;
        private double learningRate;
        private double maxGradientNorm = 1.0; // For gradient clipping

        private double[,] Wx;
        private double[,] Wh;
        private double[] Bh;
        private double[,] Wo;
        private double[] Bo;
        private double[] h;

        public double[,] WeightInputToHidden => Wx;
        public double[,] WeightHiddenToHidden => Wh;
        public double[,] WeightHiddenToOutput => Wo;
        public double[] BiasHidden => Bh;
        public double[] BiasOutput => Bo;

        public ImprovedRNN(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.001)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;
            this.learningRate = learningRate;

            Wx = InitializeMatrix(inputSize, hiddenSize);
            Wh = InitializeMatrix(hiddenSize, hiddenSize);
            Bh = new double[hiddenSize];
            Wo = InitializeMatrix(hiddenSize, outputSize);
            Bo = new double[outputSize];
            h = new double[hiddenSize];
        }

        private double[,] InitializeMatrix(int rows, int cols)
        {
            var rand = new Random();
            double[,] matrix = new double[rows, cols];
            double scale = Math.Sqrt(2.0 / (rows + cols)); // Xavier initialization

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = (rand.NextDouble() * 2 - 1) * scale;
            return matrix;
        }

        public double[] Forward(double[] input)
        {
            double[] newH = new double[hiddenSize];

            for (int j = 0; j < hiddenSize; j++)
            {
                for (int i = 0; i < inputSize; i++)
                    newH[j] += input[i] * Wx[i, j];
                for (int k = 0; k < hiddenSize; k++)
                    newH[j] += h[k] * Wh[k, j];

                newH[j] = Math.Tanh(newH[j] + Bh[j]);
            }

            h = newH;

            var output = new double[outputSize];
            for (int k = 0; k < outputSize; k++)
            {
                for (int j = 0; j < hiddenSize; j++)
                    output[k] += h[j] * Wo[j, k];
                output[k] += Bo[k];
            }

            return output;
        }

        public void Backward(double[] output, double[] target, double[] input)
        {
            double[] dOutput = new double[output.Length];
            for (int i = 0; i < output.Length; i++)
                dOutput[i] = 2 * (output[i] - target[i]);

            double[] dH = new double[hiddenSize];
            for (int j = 0; j < hiddenSize; j++)
            {
                for (int k = 0; k < outputSize; k++)
                {
                    dH[j] += dOutput[k] * Wo[j, k];
                    Wo[j, k] -= learningRate * dOutput[k] * h[j];
                }
                dH[j] *= (1 - h[j] * h[j]); // Derivative of tanh
            }

            for (int j = 0; j < hiddenSize; j++)
            {
                for (int i = 0; i < inputSize; i++)
                    Wx[i, j] -= learningRate * dH[j] * input[i];
                Bh[j] -= learningRate * dH[j];
            }

            for (int j = 0; j < hiddenSize; j++)
                for (int k = 0; k < hiddenSize; k++)
                    Wh[j, k] -= learningRate * dH[k] * h[j];
        }
    }
}
