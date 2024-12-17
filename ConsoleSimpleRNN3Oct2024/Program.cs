using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN3Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.01;
            int epochs = 1000;

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

            // Assert that learning occurred by checking the overall loss reduction from initial to final epoch
            if (overallLoss >= initialLoss) Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");

            /*
            ....
            Epoch 997/1000, Loss: 2,249137217985157E-17
            Epoch 998/1000, Loss: 2,1835033207507885E-17
            Epoch 999/1000, Loss: 2,1197847980604842E-17
            Epoch 1000/1000, Loss: 2,057925602192353E-17 
            */

            /*
            ...
            Epoch 998/1000, Loss: 3,5309284169140917E-17
            Epoch 999/1000, Loss: 3,427960060956636E-17
            Epoch 1000/1000, Loss: 3,327994506065769E-17 
            */

            Console.ReadLine();
        }
    }
}
