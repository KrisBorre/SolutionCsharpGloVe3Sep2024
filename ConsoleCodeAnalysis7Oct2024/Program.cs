namespace ConsoleCodeAnalysis7Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var run = new Run();

            var projectRNN = new Project { Name = "RNN" };
            run.Projects.Add(projectRNN);

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 1,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Rnn
{
    public Matrix<float> Weight;
    public Vector<float> Bias;

    public Rnn(int inputSize, int hiddenSize)
    {
        // Initialize weights and bias
        Weight = Matrix<float>.Build.Random(inputSize, hiddenSize);
        Bias = Vector<float>.Build.Random(hiddenSize);
    }

    public Vector<float> Forward(Vector<float> input)
    {
        // Simple tanh activation function
        return Vector<float>.Tanh(Weight * input + Bias);
    }

    public float Loss(Vector<float> target, Vector<float> output)
    {
        // Mean Squared Error loss
        return (float)(0.5 * Vector.Dot(target - output, target - output));
    }

    public void Backward(Vector<float> target, Vector<float> output, float learningRate)
    {
        // Gradient of loss wrt output
        var dLoss_dOutput = output - target;

        // Gradient of loss wrt weights and bias
        var dLoss_dWeight = Matrix<float>.Build.DenseOfRowVector(dLoss_dOutput * output.Lift(1 - output.ToArray().Select(Math.Tanh).ToArray()));
        var dLoss_dBias = dLoss_dOutput;

        // Update weights and bias
        Weight -= dLoss_dWeight * learningRate;
        Bias -= dLoss_dBias * learningRate;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create an RNN with 1 input size and 1 hidden size
        var rnn = new Rnn(1, 1);

        // Define a simple training loop
        for (int i = 0; i < 1000; i++)
        {
            // Define a simple training example
            var input = new Vector<float>(new float[] { 0.5f });
            var target = new Vector<float>(new float[] { 0.8f });

            // Forward pass
            var output = rnn.Forward(input);

            // Compute loss
            var loss = rnn.Loss(target, output);

            // Backward pass
            rnn.Backward(target, output, 0.1f);

            // Print loss
            Console.WriteLine($""Loss at iteration {i}: {loss}"");
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 2,
                SourceCodes = {
                    new SourceCode(@"public class RNN
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
    }

    // Input Embedding
    private double[] EmbedInput(int input)
    {
        // Implement embedding logic here
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
            }
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 3,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

public class RNN
{
    private int inputSize;
    private int hiddenSize;
    private int outputSize;

    private Matrix<float> Whx; // Input to hidden weights
    private Matrix<float> Whh; // Hidden to hidden weights
    private Matrix<float> Why; // Hidden to output weights

    private Matrix<float> bh; // Hidden bias
    private Matrix<float> by; // Output bias

    private ActivationFunction activation;

    public RNN(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        Whx = Matrix<float>.Build.Random(hiddenSize, inputSize);
        Whh = Matrix<float>.Build.Random(hiddenSize, hiddenSize);
        Why = Matrix<float>.Build.Random(outputSize, hiddenSize);

        bh = Matrix<float>.Build.DenseOfSize(hiddenSize, 1);
        by = Matrix<float>.Build.DenseOfSize(outputSize, 1);

        activation = Tanh;
    }

    public Matrix<float> Forward(IEnumerable<int> inputs)
    {
        var hidden = Matrix<float>.Build.DenseOfSize(hiddenSize, 1);
        var outputs = new List<Matrix<float>>();

        foreach (var input in inputs)
        {
            var x = Vector<float>.Build.DenseOfArray(Enumerable.Repeat(input, hiddenSize).ToArray());
            var h = activation(Whx * x + Whh * hidden + bh);
            var y = Why * h + by;

            outputs.Add(y);
            hidden = h;
        }

        return outputs.SelectMany(o => o.Enumerate()).ToArray();
    }

    private Matrix<float> ActivationDerivative(Matrix<float> z)
    {
        return (1 - z.PointwiseMultiply(z)).AsMatrix();
    }

    private Matrix<float> Softmax(Matrix<float> z)
    {
        var expZ = z.PointwiseExp();
        return expZ / expZ.ColumnSums().AsVector().ToColumnMatrix();
    }

    private Matrix<float> Tanh(Matrix<float> z)
    {
        return (1 - z.PointwiseExp().PointwiseMultiply(-2)).PointwiseMultiply(z.PointwiseExp() - 1).AsMatrix();
    }

    // ... (BPTT and training loop)
}")
                }
            });



            projectRNN.Attempts.Add(new Attempt
            {
                Number = 6,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Linq;

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
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 7,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
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
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 8,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class SimpleRNN
{
    private Matrix<float> W; // Weight matrix
    private Vector<float> b; // Bias vector
    private ActivationFunction activation; // Activation function
    private LossFunction loss; // Loss function

    public SimpleRNN(int inputSize, int hiddenSize, ActivationFunction activation, LossFunction loss)
    {
        this.activation = activation;
        this.loss = loss;

        // Initialize weights and bias
        W = Matrix<float>.Build.Random(hiddenSize, inputSize);
        b = Vector<float>.Build.Random(hiddenSize);
    }

    public Vector<float> Forward(Vector<float> x)
    {
        // Compute the output of the RNN for a single input
        return activation(W * x + b);
    }

    public Tuple<Vector<float>, float> Backward(Vector<float> x, Vector<float> y)
    {
        // Compute gradients for the weights and biases
        // and the loss function
        var y_pred = Forward(x);
        var dL_dy_pred = loss.Gradient(y_pred, y);
        var dy_pred_dz = activation.Gradient(y_pred);
        var dL_dz = dL_dy_pred * dy_pred_dz;
        var dL_dW = dL_dz * x.Transpose();
        var dL_db = dL_dz;

        // Update the parameters
        W -= dL_dW;
        b -= dL_db;

        return new Tuple<Vector<float>, float>(y_pred, loss(y_pred, y));
    }

    public void Train(List<Tuple<Vector<float>, Vector<float>>> trainingSet, int epochs)
    {
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            float totalLoss = 0;
            foreach (var data in trainingSet)
            {
                var (y_pred, loss) = Backward(data.Item1, data.Item2);
                totalLoss += loss;
            }

            Console.WriteLine($""Epoch {epoch + 1}, Loss: {totalLoss / trainingSet.Count}"");
        }
    }
}

public delegate Vector<float> ActivationFunction(Vector<float> x);
public delegate float LossFunction(Vector<float> y_pred, Vector<float> y);

public static class ActivationFunctions
{
    public static Vector<float> Tanh(Vector<float> x)
    {
        var result = new Vector<float>(x.Count);
        for (int i = 0; i < x.Count; i++)
        {
            result[i] = (float)Math.Tanh(x[i]);
        }
        return result;
    }
}

public static class LossFunctions
{
    public static float MSE(Vector<float> y_pred, Vector<float> y)
    {
        var diff = y_pred - y;
        return (float)diff.L2Norm();
    }
}")
                }
            });



            projectRNN.Attempts.Add(new Attempt
            {
                Number = 10,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Linq;
using System.Numerics;

public class RNN
{
    private float[] weightsInputToHidden;
    private float[] weightsHiddenToOutput;
    private float[] hiddenBias;
    private float[] outputBias;
    private float learningRate;

    public RNN(int inputSize, int hiddenSize, float learningRate)
    {
        this.learningRate = learningRate;

        // Initialize weights and biases
        weightsInputToHidden = new float[inputSize * hiddenSize];
        weightsHiddenToOutput = new float[hiddenSize * 1];
        hiddenBias = new float[hiddenSize];
        outputBias = new float[1];

        // Initialize weights and biases to small random values
        var rnd = new Random();
        for (int i = 0; i < weightsInputToHidden.Length; i++)
        {
            weightsInputToHidden[i] = (float)(rnd.NextDouble() * 2 - 1);
        }

        for (int i = 0; i < weightsHiddenToOutput.Length; i++)
        {
            weightsHiddenToOutput[i] = (float)(rnd.NextDouble() * 2 - 1);
        }

        for (int i = 0; i < hiddenBias.Length; i++)
        {
            hiddenBias[i] = (float)(rnd.NextDouble() * 2 - 1);
        }

        for (int i = 0; i < outputBias.Length; i++)
        {
            outputBias[i] = (float)(rnd.NextDouble() * 2 - 1);
        }
    }

    public float[] Forward(float[] inputs)
    {
        int hiddenSize = hiddenBias.Length;
        float[] hiddenLayer = new float[hiddenSize];

        // Compute the hidden layer
        for (int i = 0; i < hiddenSize; i++)
        {
            float sum = 0;
            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weightsInputToHidden[i * inputs.Length + j];
            }
            sum += hiddenBias[i];
            hiddenLayer[i] = Sigmoid(sum);
        }

        // Compute the output
        float[] output = new float[1];
        for (int i = 0; i < 1; i++)
        {
            float sum = 0;
            for (int j = 0; j < hiddenSize; j++)
            {
                sum += hiddenLayer[j] * weightsHiddenToOutput[i * hiddenSize + j];
            }
            sum += outputBias[i];
            output[i] = Sigmoid(sum);
        }

        return output;
    }

    public void Backward(float[] inputs, float[] targetOutputs)
    {
        int inputSize = weightsInputToHidden.Length / hiddenBias.Length;
        int hiddenSize = hiddenBias.Length;

        // Compute output error
        float[] outputError = new float[1];
        outputError[0] = targetOutputs[0] - Forward(inputs)[0];

        // Compute gradients for output layer
        float[] dOutput = new float[1];
        dOutput[0] = outputError[0] * dSigmoid(Forward(inputs)[0]);

        // Compute gradients for hidden layer
        float[] dHidden = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            float sum = 0;
            for (int j = 0; j < 1; j++)
            {
                sum += dOutput[j] * weightsHiddenToOutput[i * 1 + j];
            }
            dHidden[i] = sum * dSigmoid(HiddenActivations(inputs)[i]);
        }

        // Update weights and biases
        for (int i = 0; i < hiddenSize; i++)
        {
            for (int j = 0; j < inputSize; j++)
            {
                weightsInputToHidden[i * inputSize + j] += learningRate * dHidden[i] * inputs[j];
            }
            hiddenBias[i] += learningRate * dHidden[i];
        }

        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                weightsHiddenToOutput[i * hiddenSize + j] += learningRate * dOutput[i] * HiddenActivations(inputs)[j];
            }
            outputBias[i] += learningRate * dOutput[i];
        }
    }

    private float[] HiddenActivations(float[] inputs)
    {
        float[] hiddenLayer = new float[hiddenBias.Length];

        for (int i = 0; i < hiddenLayer.Length; i++)
        {
            hiddenLayer[i] = Sigmoid(Vector.Dot(new Vector(inputs), new Vector(weightsInputToHidden, i * inputs.Length)));
        }

        return hiddenLayer;
    }

    private float Sigmoid(float x)
    {
        return 1 / (1 + (float)Math.Exp(-x));
    }

    private float dSigmoid(float x)
    {
        return x * (1 - x);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Simple dataset: predict the next number in a sequence (0 -> 1 -> 2 -> ...)
        int[] inputSequence = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] targetSequence = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Convert sequences to float arrays
        float[] inputs = inputSequence.Select(i => (float)i).ToArray();
        float[] targets = targetSequence.Select(i => (float)i).ToArray();

        // Initialize RNN with 1 input size, 2 hidden size, and learning rate 0.5
        RNN rnn = new RNN(1, 2, 0.5f);

        // Training loop
        for (int epoch = 0; epoch < 1000; epoch++)
        {
            float loss = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                // Forward pass
                float[] output = rnn.Forward(new float[] { inputs[i] });

                // Compute loss (mean squared error)
                loss += (output[0] - targets[i]) * (output[0] - targets[i]);

                // Backward pass
                rnn.Backward(new float[] { inputs[i] }, new float[] { targets[i] });
            }

            loss /= inputs.Length;
            Console.WriteLine($""Epoch {epoch + 1}, Loss: {loss}"");
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 11,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

public class RNN
{
    private Matrix<float> weights;
    private Vector<float> bias;

    public RNN(int inputSize, int hiddenSize)
    {
        weights = Matrix<float>.Build.Random(hiddenSize, inputSize);
        bias = Vector<float>.Build.Random(hiddenSize);
    }

    public Vector<float> Forward(Vector<float> input)
    {
        Vector<float> hidden = Vector<float>.Zero;
        hidden = Vector.Tanh(Vector.Add(Vector.Dot(weights, input), bias));
        return hidden;
    }

    public void Backward(Vector<float> input, Vector<float> gradOutput, float learningRate)
    {
        // Compute gradients
        var gradInput = Vector<float>.Zero;
        var gradWeights = Matrix<float>.Build.DenseOfRows(new List<Vector<float>>());
        var gradBias = Vector<float>.Build.Zeroes(bias.Count);

        // Update parameters
        weights -= Matrix<float>.Build.DenseOfColumnVectors(gradWeights) * learningRate;
        bias -= gradBias * learningRate;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var inputSize = 10;
        var hiddenSize = 5;
        var rnn = new RNN(inputSize, hiddenSize);

        var input = Vector<float>.Build.Random(inputSize);
        var output = rnn.Forward(input);

        // Train the RNN
        var learningRate = 0.01f;
        var epochs = 100;
        for (var i = 0; i < epochs; i++)
        {
            var target = Vector<float>.Build.Random(hiddenSize);
            var loss = Vector.Subtract(output, target);
            rnn.Backward(input, loss, learningRate);
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 12,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Linq;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        // Initialize parameters for the RNN
        var weights = new Matrix<float>(3, 2); // Input size = 3, hidden size = 2
        var biases = new Vector<float>(2);

        // Randomly initialize weights and biases
        var rnd = new Random();
        for (int i = 0; i < weights.RowCount; i++)
        {
            for (int j = 0; j < weights.ColumnCount; j++)
            {
                weights[i, j] = (float)rnd.NextDouble();
            }
        }
        for (int i = 0; i < biases.Count; i++)
        {
            biases[i] = (float)rnd.NextDouble();
        }

        // Simple dataset: input = [1, 0, 0] and [0, 1, 0]
        var inputs = new Matrix<float>(2, 3);
        inputs[0, 0] = 1; inputs[0, 1] = 0; inputs[0, 2] = 0;
        inputs[1, 0] = 0; inputs[1, 1] = 1; inputs[1, 2] = 0;

        // Labels for the dataset: output = [1, 0] and [0, 1]
        var labels = new Matrix<float>(2, 2);
        labels[0, 0] = 1; labels[0, 1] = 0;
        labels[1, 0] = 0; labels[1, 1] = 1;

        // Hyperparameters
        var learningRate = 0.1f;
        var epochs = 1000;

        // Training loop
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            // Forward propagation
            var hiddenLayer = inputs * weights + biases;
            var output = hiddenLayer.Tanh();

            // Compute loss
            var loss = (output - labels).L2Norm();

            // Backpropagation
            var dLoss_dOutput = output - labels;
            var dOutput_dHiddenLayer = output.TanhDerivative();
            var dLoss_dHiddenLayer = dLoss_dOutput * dOutput_dHiddenLayer;
            var dLoss_dWeights = inputs.Transpose() * dLoss_dHiddenLayer;
            var dLoss_dBias = dLoss_dHiddenLayer;

            // Update parameters
            weights -= dLoss_dWeights * learningRate;
            biases -= dLoss_dBias * learningRate;

            // Log loss every 100 epochs
            if (epoch % 100 == 0)
            {
                Console.WriteLine($""Epoch {epoch}, Loss: {loss}"");
            }
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 13,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class SimpleRNN
{
    private Matrix<float> Wx;
    private Matrix<float> Wh;
    private Vector<float> b;
    private float learningRate;

    public SimpleRNN(int inputSize, int hiddenSize, float learningRate)
    {
        this.learningRate = learningRate;

        // Initialize weights and bias for input to hidden layer
        Wx = Matrix<float>.Build.Random(hiddenSize, inputSize);
        Wh = Matrix<float>.Build.Random(hiddenSize, hiddenSize);
        b = Vector<float>.Build.Random(hiddenSize);
    }

    public Vector<float> Forward(Vector<float> x, Vector<float> h_prev)
    {
        var h_tilde = Wx * x + Wh * h_prev + b;
        var h_t = 1 / (1 + (float)Math.Exp(-h_tilde.Sum())) * h_tilde;
        return h_t;
    }

    public void Backward(Vector<float> x, Vector<float> h_prev, Vector<float> h_t, Vector<float> next_h_t, Vector<float> y)
    {
        var delta = h_t - y;
        var dWx = delta * x.Transpose();
        var dWh = delta * h_prev.Transpose();
        var db = delta;

        // Update weights and bias
        Wx -= learningRate * dWx;
        Wh -= learningRate * dWh;
        b -= learningRate * db;
    }

    public float Train(List<Vector<float>> inputs, List<Vector<float>> targets)
    {
        // Initialize previous hidden state
        var h_prev = Vector<float>.Build.Dense(Wx.RowCount, 0f);
        var losses = new List<float>();

        for (int i = 0; i < inputs.Count; i++)
        {
            // Forward pass
            var h_t = Forward(inputs[i], h_prev);

            // Compute loss
            var loss = (h_t - targets[i]).L2Norm() / 2;
            losses.Add(loss);

            // Backward pass
            if (i > 0)
            {
                Backward(inputs[i - 1], h_prev, h_t, h_prev, targets[i]);
            }

            // Update previous hidden state
            h_prev = h_t;
        }

        return losses.Average();
    }
}

public class Program
{
    public static void Main()
    {
        // Simple dataset
        var inputs = new List<Vector<float>> { Vector<float>.Build.Dense(new float[] { 0, 1 }), Vector<float>.Build.Dense(new float[] { 1, 0 }) };
        var targets = new List<Vector<float>> { Vector<float>.Build.Dense(new float[] { 1, 0 }), Vector<float>.Build.Dense(new float[] { 0, 1 }) };

        var rnn = new SimpleRNN(2, 2, 0.1f);

        for (int epoch = 0; epoch < 1000; epoch++)
        {
            var loss = rnn.Train(inputs, targets);
            Console.WriteLine($""Epoch {epoch}, Loss: {loss}"");
        }
    }
}")
                }
            });



            projectRNN.Attempts.Add(new Attempt
            {
                Number = 15,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Numerics;

public class RNN
{
    private Matrix<float> Wx, Wh, b;
    private float learningRate;

    public RNN(int inputSize, int hiddenSize, float learningRate = 0.1f)
    {
        Wx = Matrix<float>.Build.Random(hiddenSize, inputSize);
        Wh = Matrix<float>.Build.Random(hiddenSize, hiddenSize);
        b = Matrix<float>.Build.Dense(hiddenSize, 1, 0.1f);
        this.learningRate = learningRate;
    }

    public float[] Forward(float[] inputs)
    {
        var output = new float[inputs.Length];
        var hiddens = new Matrix<float>(new float[1, 1], 1, 1);

        for (int i = 0; i < inputs.Length; i++)
        {
            var x = Matrix<float>.Build.DenseOfArray(new float[] { inputs[i] });
            hiddens = tanh(Wx * x + Wh * hiddens + b);
            output[i] = hiddens[0, 0];
        }

        return output;
    }

    private Matrix<float> tanh(Matrix<float> x)
    {
        return x.PointwiseApply(Math.Tanh);
    }

    public void Backward(float[] inputs, float[] targets, float[] output)
    {
        var dWh = Matrix<float>.Build.DenseOfRows(hiddenSize, hiddenSize);
        var dWx = Matrix<float>.Build.DenseOfRows(hiddenSize, inputSize);
        var db = Matrix<float>.Build.DenseOfRows(hiddenSize, 1);
        var dhiddens = Matrix<float>.Build.DenseOfRows(hiddenSize, 1);

        for (int i = inputs.Length - 1; i >= 0; i--)
        {
            var x = Matrix<float>.Build.DenseOfArray(new float[] { inputs[i] });
            var target = Matrix<float>.Build.DenseOfArray(new float[] { targets[i] });
            var hidden = Matrix<float>.Build.DenseOfArray(new float[] { output[i] });

            var dloss = 2 * (target - hidden);
            var dtanh = 1 - hidden * hidden;

            dWx += dloss * dtanh * x.Transpose();
            dWh += dloss * dtanh * hidden.Transpose();
            db += dloss * dtanh;
            dhiddens = Wh.Transpose() * dloss * dtanh + dhiddens;
        }

        Wx -= learningRate * dWx;
        Wh -= learningRate * dWh;
        b -= learningRate * db;
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 16,
                SourceCodes = {
                    new SourceCode(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class RNN
{
    private Matrix<float> Wxh;
    private Matrix<float> Whh;
    private Matrix<float> Why;
    private Vector<float> bh;
    private Vector<float> by;

    private Random rand = new Random();

    public RNN(int nInput, int nHidden, int nOutput)
    {
        Wxh = Matrix<float>.Build.Random(nHidden, nInput, Normal, rand);
        Whh = Matrix<float>.Build.Random(nHidden, nHidden, Normal, rand);
        Why = Matrix<float>.Build.Random(nOutput, nHidden, Normal, rand);
        bh = Vector<float>.Build.Random(nHidden, Normal, rand);
        by = Vector<float>.Build.Random(nOutput, Normal, rand);
    }

    public Vector<float> Forward(Vector<float> xs)
    {
        var hprev = Vector<float>.Zero;
        var ys = new List<float>();

        foreach (var x in xs)
        {
            var h = (Wxh * x + Whh * hprev + bh).Tanh();
            var y = Why * h + by;
            ys.Add(y.Sum());

            hprev = h;
        }

        return DenseVector.OfArray(ys.ToArray());
    }

    public (Matrix<float> dWxh, Matrix<float> dWhh, Matrix<float> dWhy, Vector<float> dbh, Vector<float> dby) Backward(Vector<float> xs, Vector<float> ys, float learningRate)
    {
        var dWxh = Matrix<float>.Build.Dense(Wxh.RowCount, Wxh.ColumnCount);
        var dWhh = Matrix<float>.Build.Dense(Whh.RowCount, Whh.ColumnCount);
        var dWhy = Matrix<float>.Build.Dense(Why.RowCount, Why.ColumnCount);
        var dbh = Vector<float>.Build.Dense(bh.Count);
        var dby = Vector<float>.Build.Dense(by.Count);

        var hprev = Vector<float>.Zero;

        for (int t = xs.Count - 1; t >= 0; t--)
        {
            var x = xs[t];
            var y = ys[t];

            var h = Wxh * x + Whh * hprev + bh;
            var yHat = Why * h + by;

            var dWhyT = dWhy.Transpose();
            var dbyT = dby.Transpose();

            var dLdyHat = yHat - y;
            var dyHat = dLdyHat * dWhyT;
            var dh = dyHat * Why;

            var dLdh = dh;
            var dWhhT = Whh.Transpose();
            var dWxhT = Wxh.Transpose();

            dWhh += dLdh * hprev.Transpose();
            dWxh += dLdh * x.Transpose();
            dbh += dLdh.Sum(i => i).ToArray();

            var dLdhPrev = Whh * dLdh;
            hprev = dLdhPrev.Select(x => (float)Math.Tanh(x)).ToArray();
        }

        dWxh /= xs.Count;
        dWhh /= xs.Count;
        dWhy /= xs.Count;
        dbh /= xs.Count;
        dby /= xs.Count;

        Wxh -= dWxh * learningRate;
        Whh -= dWhh * learningRate;
        Why -= dWhy * learningRate;
        bh -= dbh * learningRate;
        by -= dby * learningRate;

        return (dWxh, dWhh, dWhy, dbh, dby);
    }

    private float Normal(Random r)
    {
        return (float)(r.NextDouble() - 0.5);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rnn = new RNN(1, 2, 1);
        var xs = new List<float> { 0, 0 };
        var ys = new List<float> { 0 };

        for (int i = 0; i < 1000; i++)
        {
            var yHat = rnn.Forward(DenseVector.OfArray(xs.ToArray()));
            rnn.Backward(DenseVector.OfArray(xs.ToArray()), DenseVector.OfArray(ys.ToArray()), 0.01f);
        }
    }
}")
                }
            });

            projectRNN.Attempts.Add(new Attempt
            {
                Number = 17,
                SourceCodes = {
                    new SourceCode(@"using System;
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
}"),
                    new SourceCode(@"public static double[,] Dot(double[,] a, double[,] b)
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
}")
                }
            });



            // Process and execute the projects
            var compiler = new CodeCompiler();
            compiler.ProcessAttempts(run);

            // Display results
            foreach (var project in run.Projects)
            {
                Console.WriteLine($"Project: {project.Name}");
                foreach (var attempt in project.Attempts)
                {
                    Console.WriteLine($"  Attempt #{attempt.Number}:");
                    Console.WriteLine($"    Compiled: {attempt.CompilationSuccess}");
                    Console.WriteLine($"    Diagnostics: {attempt.DiagnosticResults}");
                    Console.WriteLine($"    Execution Output: {attempt.ExecutionOutput}");
                }
            }


            /*
            Project: RNN
              Attempt #1:
                Compiled: False
                Diagnostics: (3,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (8,12): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (14,18): error CS0103: The name 'Matrix' does not exist in the current context
            (15,30): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (21,30): error CS0117: 'Vector<float>' does not contain a definition for 'Tanh'
            (36,29): error CS0103: The name 'Matrix' does not exist in the current context
            (36,105): error CS1061: 'Vector<float>' does not contain a definition for 'ToArray' and no accessible extension method 'ToArray' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (36,89): error CS1061: 'Vector<float>' does not contain a definition for 'Lift' and no accessible extension method 'Lift' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
                Execution Output:
              Attempt #2:
                Compiled: False
                Diagnostics: error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (9,22): warning CS0169: The field 'RNN.hiddenBiases' is never used
            (10,22): warning CS0169: The field 'RNN.outputBiases' is never used
            (8,23): warning CS0169: The field 'RNN.weightsHiddenOutput' is never used
            (7,23): warning CS0169: The field 'RNN.weightsInputHidden' is never used
            (6,23): warning CS0169: The field 'RNN.inputEmbeddings' is never used
                Execution Output:
              Attempt #3:
                Compiled: False
                Diagnostics: (2,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (37,12): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (55,48): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (55,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (60,35): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (60,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (66,32): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (66,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (12,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (13,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (14,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (16,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (17,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (19,13): error CS0246: The type or namespace name 'ActivationFunction' could not be found (are you missing a using directive or an assembly reference?)
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (27,15): error CS0103: The name 'Matrix' does not exist in the current context
            (28,15): error CS0103: The name 'Matrix' does not exist in the current context
            (29,15): error CS0103: The name 'Matrix' does not exist in the current context
            (31,14): error CS0103: The name 'Matrix' does not exist in the current context
            (32,14): error CS0103: The name 'Matrix' does not exist in the current context
            (39,22): error CS0103: The name 'Matrix' does not exist in the current context
            (40,32): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (44,35): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (44,54): error CS0103: The name 'Enumerable' does not exist in the current context
            (45,21): error CS1955: Non-invocable member 'RNN.activation' cannot be used like a method.
            (52,24): error CS1061: 'List<Matrix<float>>' does not contain a definition for 'SelectMany' and no accessible extension method 'SelectMany' accepting a first argument of type 'List<Matrix<float>>' could be found (are you missing a using directive or an assembly reference?)
                Execution Output:
              Attempt #6:
                Compiled: False
                Diagnostics: (3,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
                Execution Output:
              Attempt #7:
                Compiled: False
                Diagnostics: (49,22): error CS0443: Syntax error; value expected
            (49,36): error CS0443: Syntax error; value expected
            (49,50): error CS0443: Syntax error; value expected
            (63,25): error CS0443: Syntax error; value expected
            (63,34): error CS0443: Syntax error; value expected
            (63,48): error CS0443: Syntax error; value expected
            (64,25): error CS0443: Syntax error; value expected
            (64,34): error CS0443: Syntax error; value expected
            (64,48): error CS0443: Syntax error; value expected
            (65,29): error CS0443: Syntax error; value expected
            (66,19): error CS0443: Syntax error; value expected
            (66,33): error CS0443: Syntax error; value expected
            (66,61): error CS0443: Syntax error; value expected
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (58,36): error CS0103: The name 'hiddenSize' does not exist in the current context
            (58,48): error CS0103: The name 'inputSize' does not exist in the current context
            (59,36): error CS0103: The name 'hiddenSize' does not exist in the current context
            (59,48): error CS0103: The name 'hiddenSize' does not exist in the current context
            (60,35): error CS0103: The name 'hiddenSize' does not exist in the current context
            (66,67): error CS0019: Operator '+' cannot be applied to operands of type 'double[*,*]' and 'double[*,*]'
            (68,15): error CS0019: Operator '*' cannot be applied to operands of type 'double' and 'double[*,*]'
            (69,15): error CS0019: Operator '*' cannot be applied to operands of type 'double' and 'double[*,*]'
            (70,14): error CS0019: Operator '*' cannot be applied to operands of type 'double' and 'double[*,*]'
                Execution Output:
              Attempt #8:
                Compiled: False
                Diagnostics: (3,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (8,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (19,13): error CS0103: The name 'Matrix' does not exist in the current context
            (20,27): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (84,28): error CS1061: 'Vector<float>' does not contain a definition for 'L2Norm' and no accessible extension method 'L2Norm' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (70,40): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (71,29): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (73,13): error CS0200: Property or indexer 'Vector<float>.this[int]' cannot be assigned to -- it is read only
            (34,31): error CS1061: 'LossFunction' does not contain a definition for 'Gradient' and no accessible extension method 'Gradient' accepting a first argument of type 'LossFunction' could be found (are you missing a using directive or an assembly reference?)
            (35,37): error CS1061: 'ActivationFunction' does not contain a definition for 'Gradient' and no accessible extension method 'Gradient' accepting a first argument of type 'ActivationFunction' could be found (are you missing a using directive or an assembly reference?)
            (37,31): error CS1061: 'Vector<float>' does not contain a definition for 'Transpose' and no accessible extension method 'Transpose' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
                Execution Output:
              Attempt #10:
                Compiled: False
                Diagnostics: (2,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (156,40): error CS1061: 'int[]' does not contain a definition for 'Select' and no accessible extension method 'Select' accepting a first argument of type 'int[]' could be found (are you missing a using directive or an assembly reference?)
            (157,42): error CS1061: 'int[]' does not contain a definition for 'Select' and no accessible extension method 'Select' accepting a first argument of type 'int[]' could be found (are you missing a using directive or an assembly reference?)
            (130,49): error CS0712: Cannot create an instance of the static class 'Vector'
            (130,69): error CS0712: Cannot create an instance of the static class 'Vector'
                Execution Output:
              Attempt #11:
                Compiled: False
                Diagnostics: (4,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (8,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (13,19): error CS0103: The name 'Matrix' does not exist in the current context
            (14,30): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (20,25): error CS0117: 'Vector' does not contain a definition for 'Tanh'
            (45,35): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (53,40): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (28,27): error CS0103: The name 'Matrix' does not exist in the current context
            (29,38): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (29,51): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (32,20): error CS0103: The name 'Matrix' does not exist in the current context
                Execution Output:
              Attempt #12:
                Compiled: False
                Diagnostics: (2,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (10,27): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (22,29): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (24,13): error CS0200: Property or indexer 'Vector<float>.this[int]' cannot be assigned to -- it is read only
            (28,26): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (33,26): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
                Execution Output:
              Attempt #13:
                Compiled: False
                Diagnostics: (3,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (8,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (18,14): error CS0103: The name 'Matrix' does not exist in the current context
            (19,14): error CS0103: The name 'Matrix' does not exist in the current context
            (20,27): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (77,62): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (77,111): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (78,63): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (78,112): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (33,29): error CS1061: 'Vector<float>' does not contain a definition for 'Transpose' and no accessible extension method 'Transpose' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (34,34): error CS1061: 'Vector<float>' does not contain a definition for 'Transpose' and no accessible extension method 'Transpose' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (46,36): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (55,43): error CS1061: 'Vector<float>' does not contain a definition for 'L2Norm' and no accessible extension method 'L2Norm' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (68,23): error CS1061: 'List<float>' does not contain a definition for 'Average' and no accessible extension method 'Average' accepting a first argument of type 'List<float>' could be found (are you missing a using directive or an assembly reference?)
                Execution Output:
              Attempt #15:
                Compiled: False
                Diagnostics: (33,32): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (33,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (7,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            (12,14): error CS0103: The name 'Matrix' does not exist in the current context
            (13,14): error CS0103: The name 'Matrix' does not exist in the current context
            (14,13): error CS0103: The name 'Matrix' does not exist in the current context
            (21,27): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (25,21): error CS0103: The name 'Matrix' does not exist in the current context
            (40,19): error CS0103: The name 'Matrix' does not exist in the current context
            (40,51): error CS0103: The name 'hiddenSize' does not exist in the current context
            (40,63): error CS0103: The name 'hiddenSize' does not exist in the current context
            (41,19): error CS0103: The name 'Matrix' does not exist in the current context
            (41,51): error CS0103: The name 'hiddenSize' does not exist in the current context
            (41,63): error CS0103: The name 'inputSize' does not exist in the current context
            (42,18): error CS0103: The name 'Matrix' does not exist in the current context
            (42,50): error CS0103: The name 'hiddenSize' does not exist in the current context
            (43,24): error CS0103: The name 'Matrix' does not exist in the current context
            (43,56): error CS0103: The name 'hiddenSize' does not exist in the current context
            (47,21): error CS0103: The name 'Matrix' does not exist in the current context
            (48,26): error CS0103: The name 'Matrix' does not exist in the current context
            (49,26): error CS0103: The name 'Matrix' does not exist in the current context
                Execution Output:
              Attempt #16:
                Compiled: False
                Diagnostics: (3,14): error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (are you missing an assembly reference?)
            (42,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (42,33): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (42,53): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (8,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (9,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (10,13): error CS0246: The type or namespace name 'Matrix<>' could not be found (are you missing a using directive or an assembly reference?)
            (18,15): error CS0103: The name 'Matrix' does not exist in the current context
            (19,15): error CS0103: The name 'Matrix' does not exist in the current context
            (20,15): error CS0103: The name 'Matrix' does not exist in the current context
            (21,28): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (22,28): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (110,36): error CS0103: The name 'DenseVector' does not exist in the current context
            (111,26): error CS0103: The name 'DenseVector' does not exist in the current context
            (111,61): error CS0103: The name 'DenseVector' does not exist in the current context
            (30,27): error CS1579: foreach statement cannot operate on variables of type 'Vector<float>' because 'Vector<float>' does not contain a public instance or extension definition for 'GetEnumerator'
            (39,16): error CS0103: The name 'DenseVector' does not exist in the current context
            (44,20): error CS0103: The name 'Matrix' does not exist in the current context
            (45,20): error CS0103: The name 'Matrix' does not exist in the current context
            (46,20): error CS0103: The name 'Matrix' does not exist in the current context
            (47,33): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (47,45): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (48,33): error CS0117: 'Vector<float>' does not contain a definition for 'Build'
            (48,45): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (52,22): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (71,34): error CS1061: 'Vector<float>' does not contain a definition for 'Transpose' and no accessible extension method 'Transpose' accepting a first argument of type 'Vector<float>' could be found (are you missing a using directive or an assembly reference?)
            (72,30): error CS1061: 'float' does not contain a definition for 'Transpose' and no accessible extension method 'Transpose' accepting a first argument of type 'float' could be found (are you missing a using directive or an assembly reference?)
            (79,17): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (80,17): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (81,17): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (82,16): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
            (83,16): error CS0176: Member 'Vector<float>.Count' cannot be accessed with an instance reference; qualify it with a type name instead
                Execution Output:
              Attempt #17:
                Compiled: False
                Diagnostics: (1,1): error CS0106: The modifier 'public' is not valid for this item
            (25,1): error CS0106: The modifier 'public' is not valid for this item
            (43,1): error CS0106: The modifier 'public' is not valid for this item
            (61,1): error CS0106: The modifier 'public' is not valid for this item
            (79,1): error CS0106: The modifier 'public' is not valid for this item
            (1,25): warning CS8321: The local function 'Dot' is declared but never used
            (25,25): warning CS8321: The local function 'Add' is declared but never used
            (43,25): warning CS8321: The local function 'Subtract' is declared but never used
            (61,25): warning CS8321: The local function 'Scale' is declared but never used
            (79,25): warning CS8321: The local function 'OuterProduct' is declared but never used
                Execution Output:
            */

            Console.ReadLine();
        }
    }
}
