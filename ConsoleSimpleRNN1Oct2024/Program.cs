﻿using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN1Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.01;
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

            if (overallLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            Console.ReadLine();
        }
    }
}