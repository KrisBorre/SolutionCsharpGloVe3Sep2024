using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNN3Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.3;
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

            if (overallLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                double[] finalOutput = rnn.Forward(inputs[i]);
                Console.Write($"finalOutput[{i}]= ");
                for (int j = 0; j < finalOutput.Length; j++)
                {
                    Console.Write($"{finalOutput[j]} ");
                }
                Console.WriteLine();
            }

            /*
            ...
            Epoch 997/1000, Loss: 0
            Epoch 998/1000, Loss: 0
            Epoch 999/1000, Loss: 0
            Epoch 1000/1000, Loss: 0
            finalOutput[0]= 0
            finalOutput[1]= 1
            finalOutput[2]= 1
            */

            double[] generalInput1 = new double[] { 0, 1, 1 };
            double[] generalOutput1 = rnn.Forward(generalInput1);
            Console.Write($"generalInput1= ");
            for (int j = 0; j < generalInput1.Length; j++)
            {
                Console.Write($"{generalInput1[j]} ");
            }
            Console.WriteLine();

            Console.Write($"generalOutput1= ");
            for (int j = 0; j < generalOutput1.Length; j++)
            {
                Console.Write($"{generalOutput1[j]} ");
            }
            Console.WriteLine();

            /*
            generalInput1= 0 1 1
            generalOutput1= 1,0857489055647136 
            */

            double[] generalInput2 = new double[] { 1, 1, 0 };
            double[] generalOutput2 = rnn.Forward(generalInput2);
            Console.Write($"generalInput2= ");
            for (int j = 0; j < generalInput2.Length; j++)
            {
                Console.Write($"{generalInput2[j]} ");
            }
            Console.WriteLine();

            Console.Write($"generalOutput2= ");
            for (int j = 0; j < generalOutput2.Length; j++)
            {
                Console.Write($"{generalOutput2[j]} ");
            }
            Console.WriteLine();

            /*
            generalInput2= 1 1 0
            generalOutput2= 0,17688591474851406 
            */

            Console.ReadLine();
        }
    }
}
