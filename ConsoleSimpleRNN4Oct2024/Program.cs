using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN4Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.5; // 0.0001;
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

            if (overallLoss >= initialLoss) Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");

            // double learningRate = 0.0001;
            /*
            Epoch 994/1000, Loss: 0,6663766184543072
            Epoch 995/1000, Loss: 0,6663763082406894
            Epoch 996/1000, Loss: 0,6663759977488747
            Epoch 997/1000, Loss: 0,666375686978566
            Epoch 998/1000, Loss: 0,6663753759294658
            Epoch 999/1000, Loss: 0,6663750646012766
            Epoch 1000/1000, Loss: 0,6663747529937004
            */

            // double learningRate = 0.5;
            /*
            ...
            Epoch 997/1000, Loss: 5,70901017154315E-24
            Epoch 998/1000, Loss: 1,1943105552564492E-23
            Epoch 999/1000, Loss: 1,951114276116245E-24
            Epoch 1000/1000, Loss: 8,88337495322235E-24 
            */

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
            finalOutput[0]= -1,0567414998607916E-13
            finalOutput[1]= 0,9999999999958905
            finalOutput[2]= 0,9999999999958216
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
            generalOutput1= 1,3029049845721723
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
            generalOutput2= 0,577404759669695 
            */

            Console.ReadLine();
        }
    }
}
