using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN1Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.5; // 0.8; // 0.01;
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

            // double learningRate = 0.01;
            /*
            Epoch 1/10, Loss: 0,6665199542563286
            Epoch 2/10, Loss: 0,6665006369806541
            Epoch 3/10, Loss: 0,6664797804578236
            Epoch 4/10, Loss: 0,6664571895733603
            Epoch 5/10, Loss: 0,6664326531989397
            Epoch 6/10, Loss: 0,66640594222728
            Epoch 7/10, Loss: 0,6663768074433105
            Epoch 8/10, Loss: 0,6663449772123395
            Epoch 9/10, Loss: 0,6663101549643946
            Epoch 10/10, Loss: 0,6662720164521856
            */

            //double learningRate = 0.8;
            /*
            Epoch 1/10, Loss: 0,6660817349422722
            Epoch 2/10, Loss: 0,6284239370151219
            Epoch 3/10, Loss: 0,28353890857630465
            Epoch 4/10, Loss: 0,325262904760428
            Epoch 5/10, Loss: 1,3808543766511674
            Epoch 6/10, Loss: 68,95178548206965
            Epoch 7/10, Loss: 1258298,6803786822
            Epoch 8/10, Loss: 19232315058,254795
            Epoch 9/10, Loss: 894429724471061,5
            Epoch 10/10, Loss: 1,0522876199586026E+20
            Overall loss did not decrease, indicating lack of learning.
            */

            // double learningRate = 0.5;
            /*
            Epoch 1/10, Loss: 0,6660570938327356
            Epoch 2/10, Loss: 0,6529387919795863
            Epoch 3/10, Loss: 0,4567870996729462
            Epoch 4/10, Loss: 0,35936064201209383
            Epoch 5/10, Loss: 0,007652511554930359
            Epoch 6/10, Loss: 0,009800496135828061
            Epoch 7/10, Loss: 0,018255780852045333
            Epoch 8/10, Loss: 0,005604363669116389
            Epoch 9/10, Loss: 0,009592864668409674
            Epoch 10/10, Loss: 0,01731253174103683
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
            finalOutput[0]= 0,06719373484619091
            finalOutput[1]= 1,0397249209083854
            finalOutput[2]= 1,0695221501933159
            */

            /*
            finalOutput[0]= -0,051815460231724376
            finalOutput[1]= 0,9926117267861032
            finalOutput[2]= 1,1689057580645486
            */

            Console.ReadLine();
        }
    }
}
