namespace LibrarySimpleRNN1Sep2024
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

        private double[,] Wx; // Input weight matrix
        private double[,] Wh; // Hidden weight matrix
        private double[,] Wy; // Output weight matrix
        private double[] bh; // Hidden bias
        private double[] by; // Output bias

        private double[] hPrev; // Previous hidden state

        public SimpleRNN(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.01)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;
            this.learningRate = learningRate;

            // Initialize weights and biases
            Wx = InitializeMatrix(hiddenSize, inputSize);
            Wh = InitializeMatrix(hiddenSize, hiddenSize);
            Wy = InitializeMatrix(outputSize, hiddenSize);
            bh = new double[hiddenSize];
            by = new double[outputSize];

            hPrev = new double[hiddenSize];
        }

        // Forward pass for a single time step
        public double[] Forward(double[] x)
        {
            // Hidden state calculation: h(t) = tanh(Wx * x + Wh * h_prev + bh)
            double[] hCurrent = AddVectors(AddVectors(MatrixVectorMultiply(Wx, x), MatrixVectorMultiply(Wh, hPrev)), bh);
            hCurrent = ApplyActivation(hCurrent, Math.Tanh);

            // Output calculation: y(t) = Wy * h(t) + by
            double[] y = AddVectors(MatrixVectorMultiply(Wy, hCurrent), by);

            hPrev = hCurrent; // Store current hidden state as previous for the next time step
            return y;
        }

        // Train the RNN using a basic gradient descent approach
        public void Train(double[][] inputs, double[][] targets, int epochs)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0.0;

                for (int t = 0; t < inputs.Length; t++)
                {
                    double[] x = inputs[t];
                    double[] target = targets[t];

                    // Forward pass
                    double[] y = Forward(x);

                    // Compute loss (mean squared error)
                    double loss = 0;
                    for (int i = 0; i < outputSize; i++)
                    {
                        loss += Math.Pow(target[i] - y[i], 2);
                    }
                    totalLoss += loss;

                    // Backward pass (using simple gradient descent for this example)
                    Backward(y, target, x);
                }

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }
        }

        // Backpropagation through time (BPTT) for updating weights
        public void Backward(double[] output, double[] target, double[] x)
        {
            // Compute output layer error
            double[] outputError = new double[outputSize];
            for (int i = 0; i < outputSize; i++)
            {
                outputError[i] = 2 * (output[i] - target[i]);
            }

            // Gradient for output weights and biases
            double[] hCurrentGrad = MatrixVectorMultiply(Transpose(Wy), outputError);
            for (int i = 0; i < outputSize; i++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    Wy[i, j] -= learningRate * outputError[i] * hPrev[j];
                }
                by[i] -= learningRate * outputError[i];
            }

            // Gradient for hidden layer (backpropagated through tanh)
            double[] hGrad = ApplyActivationDerivative(hPrev, Math.Tanh);
            for (int i = 0; i < hiddenSize; i++)
            {
                hGrad[i] *= hCurrentGrad[i];
            }

            // Update hidden weights and biases
            for (int i = 0; i < hiddenSize; i++)
            {
                for (int j = 0; j < inputSize; j++)
                {
                    Wx[i, j] -= learningRate * hGrad[i] * x[j];
                }
                for (int j = 0; j < hiddenSize; j++)
                {
                    Wh[i, j] -= learningRate * hGrad[i] * hPrev[j];
                }
                bh[i] -= learningRate * hGrad[i];
            }
        }

        // Utility functions

        private double[,] InitializeMatrix(int rows, int cols)
        {
            var rand = new Random();
            double[,] matrix = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = rand.NextDouble() - 0.5;
            return matrix;
        }

        private double[] MatrixVectorMultiply(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[] result = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < cols; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }
            return result;
        }

        private double[,] Transpose(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[,] transposed = new double[cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    transposed[j, i] = matrix[i, j];
            return transposed;
        }

        private double[] AddVectors(double[] a, double[] b)
        {
            double[] result = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] + b[i];
            }
            return result;
        }

        private double[] ApplyActivation(double[] vector, Func<double, double> activation)
        {
            double[] result = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = activation(vector[i]);
            }
            return result;
        }

        private double[] ApplyActivationDerivative(double[] vector, Func<double, double> activationDerivative)
        {
            double[] result = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = 1 - Math.Pow(Math.Tanh(vector[i]), 2); // Derivative of tanh
            }
            return result;
        }
    }

}
