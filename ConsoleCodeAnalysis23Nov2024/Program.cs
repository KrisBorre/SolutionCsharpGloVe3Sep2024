using LibraryCodeAnalysis23Nov2024;

// see ConsoleCodeAnalysis7Oct2024
namespace ConsoleCodeAnalysis23Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var projectRNN = new Project9Oct2024 { Name = "RNN" };

            SourceCode8Nov2024 sourceCode1 = new SourceCode8Nov2024(@"using System;
using System.Linq;

namespace RNN
{
    public class RNN
    {
        private readonly int inputSize;
        private readonly int hiddenSize;
        private readonly int outputSize;
        private double[,] Wxh;
        private double[,] Whh;
        private double[,] Why;
        private double[] bh;
        private double[] by;

        public RNN(int inputSize, int hiddenSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;

            // Initialize weights and biases
            Wxh = Matrix.Random(hiddenSize, inputSize);
            Whh = Matrix.Random(hiddenSize, hiddenSize);
            Why = Matrix.Random(outputSize, hiddenSize);
            bh = Matrix.Zero(hiddenSize, 1);
            by = Matrix.Zero(outputSize, 1);
        }

        public double[] Forward(double[] xs)
        {
            int seqLength = xs.Length;
            double[] hs = new double[seqLength];
            double[] ys = new double[seqLength];
            double[] prevH = Matrix.Zero(hiddenSize, 1);

            for (int i = 0; i < seqLength; i++)
            {
                double[] x = Matrix.FromArray(new double[] { xs[i] });
                double[] h = Activations.Tanh(Matrix.Add(Matrix.Dot(Wxh, x), Matrix.Dot(Whh, prevH), bh));
                double[] y = Activations.Softmax(Matrix.Dot(Why, h), by);

                hs[i] = h.Sum();
                ys[i] = y.Sum();
                prevH = h;
            }

            return ys;
        }

        public void Train(double[] xs, double[] ys, double learningRate)
        {
            int seqLength = xs.Length;
            double[] prevH = Matrix.Zero(hiddenSize, 1);
            double[,] dLdWhy = Matrix.Zero(outputSize, hiddenSize);
            double[,] dLdWhh = Matrix.Zero(hiddenSize, hiddenSize);
            double[,] dLdWxh = Matrix.Zero(hiddenSize, inputSize);
            double[] dLdbh = Matrix.Zero(hiddenSize, 1);
            double[] dLdbh = Matrix.Zero(outputSize, 1);

            for (int i = 0; i < seqLength; i++)
            {
                // Forward pass
                double[] x = Matrix.FromArray(new double[] { xs[i] });
                double[] h = Activations.Tanh(Matrix.Add(Matrix.Dot(Wxh, x), Matrix.Dot(Whh, prevH), bh));
                double[] y = Activations.Softmax(Matrix.Dot(Why, h), by);

                // Backward pass (BPTT)
                double[] dLdWhyT = Activations.SoftmaxCrossEntropy(ys[i], y);
                double[] dLdWhhT = Matrix.Dot(dLdWhyT, Why);
                double[] dLdWxhT = Matrix.Dot(dLdWhhT, prevH);
                double[] dLdbhT = dLdWhyT;
                double[] dLdbhT = Activations.TanhPrime(h) * Matrix.Dot(Matrix.Transpose(Why), dLdWhyT);

                // Accumulate gradients
                dLdWhy += Matrix.OuterProduct(dLdWhyT, h);
                dLdWhh += Matrix.OuterProduct(dLdWhhT, prevH);
                dLdWxh += Matrix.OuterProduct(dLdWxhT, x);
                dLdbh += dLdbhT;
                dLdbh += dLdbhT;

                prevH = h;
            }

            // Update weights and biases
            Why = Matrix.Subtract(Why, Matrix.Scale(learningRate, dLdWhy));
            Whh = Matrix.Subtract(Whh, Matrix.Scale(learningRate, dLdWhh));
            Wxh = Matrix.Subtract(Wxh, Matrix.Scale(learningRate, dLdWxh));
            by = Matrix.Subtract(by, Matrix.Scale(learningRate, dLdby));
            bh = Matrix.Subtract(bh, Matrix.Scale(learningRate, dLdbh));
        }
    }

    public class Matrix
    {
        // Implement matrix operations here...
    }

    public class Activations
    {
        // Implement activation functions here...
    }
}");

            SourceCode8Nov2024 sourceCode2 = new SourceCode8Nov2024(@"public static double[,] Dot(double[,] a, double[,] b)
{
    int rowsA = a.GetLength(0);
    int colsA = a.GetLength(1);
    int colsB = b.GetLength(1);

    double[,] result = new double[rowsA, colsB];

    for (int i = 0; i < rowsA; i++)
    {
        for (int j = 0; j < colsB; j++)
        {
            double sum = 0;
            for (int k = 0; k < colsA; k++)
            {
                sum += a[i, k] * b[k, j];
            }
            result[i, j] = sum;
        }
    }

    return result;
}

public static double[,] Add(double[,] a, double[,] b)
{
    int rows = a.GetLength(0);
    int cols = a.GetLength(1);

    double[,] result = new double[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            result[i, j] = a[i, j] + b[i, j];
        }
    }

    return result;
}

public static double[,] Subtract(double[,] a, double[,] b)
{
    int rows = a.GetLength(0);
    int cols = a.GetLength(1);

    double[,] result = new double[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            result[i, j] = a[i, j] - b[i, j];
        }
    }

    return result;
}

public static double[,] Scale(double scale, double[,] matrix)
{
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);

    double[,] result = new double[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            result[i, j] = scale * matrix[i, j];
        }
    }

    return result;
}

public static double[,] OuterProduct(double[] a, double[] b)
{
    int rowsA = a.Length;
    int colsB = b.Length;

    double[,] result = new double[rowsA, colsB];

    for (int i = 0; i < rowsA; i++)
    {
        for (int j = 0; j < colsB; j++)
        {
            result[i, j] = a[i] * b[j];
        }
    }

    return result;
}");

            projectRNN.Conversations.Add(new Conversation
            {
                Number = 17,
                SourceCodeIterations = {
                    new SourceCodeIteration {
                        Number = 1,
                        SourceCodes = {sourceCode1, sourceCode2 }
                    }
                }
            });


            CodeAnalyzer23Nov2024 codeAnalyzer = new CodeAnalyzer23Nov2024();
            codeAnalyzer.AnalyzeCode(sourceCode1.Code);
            codeAnalyzer.AnalyzeCode(sourceCode2.Code);

            /*
            Code Analysis Result:
            Namespace: RNN
              Class: RNN
                Method: Forward()
                Method: Train()
              Class: Matrix
              Class: Activations
            Code Analysis Result: 
            */

            var project1 = new Project9Oct2024 { Name = "RNN" };

            #region conversation 2

            var conversation2 = new Conversation();
            conversation2.Number = 2;
            project1.Conversations.Add(conversation2);

            var sourceCodeIteration2_2 = new SourceCodeIteration();
            sourceCodeIteration2_2.Number = 2;
            conversation2.SourceCodeIterations.Add(sourceCodeIteration2_2);

            SourceCode8Nov2024 sourceCode2_2 = new SourceCode8Nov2024(@"public class RNN
            {
                private int inputSize;
                private int hiddenSize;
                private int outputSize;
                private double[,] inputEmbeddings;
                private double[,] weightsInputHidden;
                private double[,] weightsHiddenOutput;
                private double[] hiddenBiases;
                private double[] outputBiases;

                // Constructor
                public RNN(int inputSize, int hiddenSize, int outputSize)
                {
                    this.inputSize = inputSize;
                    this.hiddenSize = hiddenSize;
                    this.outputSize = outputSize;

                    // Initialize embeddings, weights, and biases
                    // For example:
                    inputEmbeddings = new double[inputSize, hiddenSize];
                    weightsInputHidden = new double[inputSize, hiddenSize];
                    weightsHiddenOutput = new double[hiddenSize, outputSize];
                    hiddenBiases = new double[hiddenSize];
                    outputBiases = new double[outputSize];
                }

                // Input Embedding
                private double[] EmbedInput(int input)
                {
                    // For example:
                    // return inputEmbeddings[input, *];
                    return new double[0];
                }

                // Forward Propagation
                private double[] Forward(double[] input)
                {
                    // Implement forward propagation logic here
                    return new double[0];
                }

                // Backpropagation Through Time
                private double[] Backpropagate(double[] output, double[] target)
                {
                    // Implement backpropagation logic here
                    return new double[0];
                }

                // Training Loop
                public void Train(double[][] trainingData, double[][] targets, int epochs)
                {
                    for(int epoch = 0; epoch < epochs; epoch++)
                    {
                        for(int i = 0; i < trainingData.Length; i++)
                        {
                            double[] input = trainingData[i];
                            double[] target = targets[i];

                            double[] output = Forward(input);
                            double[] gradients = Backpropagate(output, target);

                            // Update parameters using gradients
                            // For example:
                            // weightsInputHidden -= learningRate * gradients[weightsInputHidden];
                        }
                    }
                }

                public static void Main(string[] args)
                {
                    // Create an instance of RNN and train
                }
            }");
            sourceCodeIteration2_2.SourceCodes.Add(sourceCode2_2);

            codeAnalyzer.AnalyzeCode(sourceCode2_2.Code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: RNN
                Method: EmbedInput()
                Method: Forward()
                Method: Backpropagate()
                Method: Train()
                Method: Main() 
            */

            #endregion


            #region conversation 6

            var conversation6 = new Conversation();
            conversation6.Number = 6;
            project1.Conversations.Add(conversation6);

            var sourceCodeIteration6_2 = new SourceCodeIteration();
            sourceCodeIteration6_2.Number = 2;
            conversation6.SourceCodeIterations.Add(sourceCodeIteration6_2);

            SourceCode8Nov2024 sourceCode6_2 = new SourceCode8Nov2024(@"using System;
            using System.Collections.Generic;

            class Program
            {
                static void Main(string[] args)
                {
                    // Initialize a small dataset for sequence prediction.
                    List<List<double>> trainingData = new List<List<double>>
                    {
                        new List<double> { 0, 0 },
                        new List<double> { 0, 1 },
                        new List<double> { 1, 0 },
                        new List<double> { 1, 1 }
                    };
                    List<double> labels = new List<double> { 0, 1, 1, 0 };

                    // Initialize the RNN with a single layer with 2 inputs and 1 output.
                    Rnn rnn = new Rnn(2, 1);

                    // Train the RNN.
                    for (int epoch = 0; epoch < 1000; epoch++)
                    {
                        double loss = 0;
                        for (int i = 0; i < trainingData.Count; i++)
                        {
                            double output = rnn.Forward(trainingData[i]);
                            double error = output - labels[i];
                            loss += error * error;
                            rnn.Backward(error);
                        }
                        loss /= trainingData.Count;
                        Console.WriteLine($""Epoch {epoch}, Loss: {loss}"");
                    }
                }
            }

            class Rnn
            {
                public double[,] weights;
                public double[] biases;

                public Rnn(int inputSize, int outputSize)
                {
                    weights = new double[inputSize, outputSize];
                    biases = new double[outputSize];

                    Random rng = new Random();
                    for (int i = 0; i < inputSize; i++)
                        for (int j = 0; j < outputSize; j++)
                            weights[i, j] = (rng.NextDouble() * 2 - 1) / Math.Sqrt(inputSize);
                    for (int j = 0; j < outputSize; j++)
                        biases[j] = (rng.NextDouble() * 2 - 1) / Math.Sqrt(inputSize);
                }

                public double Forward(IList<double> inputs)
                {
                    double activation = 0;
                    for (int i = 0; i < inputs.Count; i++)
                        activation += inputs[i] * weights[i, 0] + biases[0];
                    return Sigmoid(activation);
                }

                public void Backward(double error)
                {
                    double gradient = error * DerivativeSigmoid(weights[0, 0]);
                    for (int i = 0; i < weights.GetLength(0); i++)
                    {
                        double delta = gradient * weights[i, 0];
                        weights[i, 0] -= delta;
                        biases[0] -= gradient;
                    }
                }

                static double Sigmoid(double x) => 1 / (1 + Math.Exp(-x));
                static double DerivativeSigmoid(double x) => x * (1 - x);
            }");
            sourceCodeIteration6_2.SourceCodes.Add(sourceCode6_2);

            codeAnalyzer.AnalyzeCode(sourceCode6_2.Code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: Program
                Method: Main()
              Class: Rnn
                Method: Forward()
                Method: Backward()
                Method: Sigmoid()
                Method: DerivativeSigmoid() 
            */

            #endregion


            #region conversation 7

            var conversation7 = new Conversation();
            conversation7.Number = 7;
            project1.Conversations.Add(conversation7);

            var sourceCodeIteration7_2 = new SourceCodeIteration();
            sourceCodeIteration7_2.Number = 2;
            conversation7.SourceCodeIterations.Add(sourceCodeIteration7_2);

            SourceCode8Nov2024 sourceCode7_2 = new SourceCode8Nov2024(@"using System;
            using System.Numerics;
            using static System.Math;

            public class RNN
            {
                private double[,] Wx, Wh, b;
                private double learningRate = 0.1;

                public RNN(int inputSize, int hiddenSize)
                {
                    // Initialize weights and bias
                    Random random = new Random();
                    Wx = new double[hiddenSize, inputSize];
                    Wh = new double[hiddenSize, hiddenSize];
                    b = new double[hiddenSize, 1];
                    for (int i = 0; i < hiddenSize; i++)
                    {
                        for (int j = 0; j < inputSize; j++)
                        {
                            Wx[i, j] = (random.NextDouble() - 0.5) * 0.1;
                        }
                        for (int j = 0; j < hiddenSize; j++)
                        {
                            Wh[i, j] = (random.NextDouble() - 0.5) * 0.1;
                        }
                        b[i, 0] = (random.NextDouble() - 0.5) * 0.1;
                    }
                }

                public double Tanh(double x)
                {
                    return (Exp(2.0 * x) - 1) / (Exp(2.0 * x) + 1);
                }

                public double TanhDerivative(double x)
                {
                    return 1 - Pow(Tanh(x), 2);
                }

                public double[,] Forward(double[,] X)
                {
                    int timeSteps = X.GetLength(0);
                    int batchSize = X.GetLength(1);
                    double[,] H = new double[timeSteps + 1, batchSize];
                    for (int t = 0; t < timeSteps; t++)
                    {
                        H[t + 1, ] = Tanh(H[t, ] * Wx + H[t, ] * Wh + b);
                    }
                    return H;
                }

                public void Backward(double[,] X, double[,] H, double[,] dH)
                {
                    int timeSteps = X.GetLength(0);
                    int batchSize = X.GetLength(1);
                    double[,] dWx = new double[hiddenSize, inputSize];
                    double[,] dWh = new double[hiddenSize, hiddenSize];
                    double[,] db = new double[hiddenSize, 1];
                    for (int t = timeSteps - 1; t >= 0; t--)
                    {
                        dWx += H[t, ] * X[t, ] * dH[t + 1, ];
                        dWh += H[t, ] * H[t, ] * dH[t + 1, ];
                        db += dH[t + 1, ];
                        dH[t, ] += H[t + 1, ] * TanhDerivative(H[t + 1, ]) * (Wx + Wh + b);
                    }
                    Wx -= learningRate * dWx;
                    Wh -= learningRate * dWh;
                    b -= learningRate * db;
                }
            }");
            sourceCodeIteration7_2.SourceCodes.Add(sourceCode7_2);

            codeAnalyzer.AnalyzeCode(sourceCode7_2.Code);

            /*
            Code Analysis Result:
            Namespace: <global namespace>
              Class: RNN
                Method: Tanh()
                Method: TanhDerivative()
                Method: Forward()
                Method: Backward()
            */

            #endregion


            Console.ReadLine();
        }
    }
}
