using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN2Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.000001;
            int epochs = 10;

            SimpleRNN rnn = new SimpleRNN(inputSize, hiddenSize, outputSize, learningRate);

            double[][] inputs = new double[][]
            {
                new double[] { 1, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 0, 0, 1 }
            };

            double[][] targets = new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 }
            };

            // Track initial and final losses
            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;
            double overallLoss = initialLoss;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] output = rnn.Forward(inputs[i]);
                    double loss = 0;

                    for (int j = 0; j < output.Length; j++)
                    {
                        loss += Math.Pow(output[j] - targets[i][j], 2);
                    }

                    totalLoss += loss;
                    rnn.Backward(output, targets[i], inputs[i]);
                }

                totalLoss /= inputs.Length;

                // Track progress by calculating total loss and checking for an overall loss decrease
                if (epoch == 0) initialLoss = totalLoss;
                overallLoss = totalLoss;
                lastLoss = totalLoss;

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            if (overallLoss >= initialLoss) Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");

            /*
            Epoch 1/10, Loss: 0,6664899398606922
            Epoch 2/10, Loss: 0,6664899378995811
            Epoch 3/10, Loss: 0,6664899359384534
            Epoch 4/10, Loss: 0,6664899339773086
            Epoch 5/10, Loss: 0,6664899320161468
            Epoch 6/10, Loss: 0,6664899300549681
            Epoch 7/10, Loss: 0,6664899280937727
            Epoch 8/10, Loss: 0,6664899261325602
            Epoch 9/10, Loss: 0,6664899241713306
            Epoch 10/10, Loss: 0,6664899222100841
            */

            Console.ReadLine();
        }
    }
}
