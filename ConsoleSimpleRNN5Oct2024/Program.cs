using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN5Oct2024
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

            // Simple sequence data with an input pattern and its subsequent target
            double[][] inputs = new double[][]
            {
                new double[] { 1, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 0, 0, 1 }
            };

            // Target output patterns, shifting the pattern right
            double[][] targets = new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 }
            };

            // Act
            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                // Training for each input-target pair
                for (int i = 0; i < inputs.Length; i++)
                {
                    // Forward pass
                    double[] output = rnn.Forward(inputs[i]);

                    // Calculate Mean Squared Error loss
                    double loss = 0;
                    for (int j = 0; j < output.Length; j++)
                    {
                        loss += Math.Pow(output[j] - targets[i][j], 2);
                    }
                    totalLoss += loss;

                    // Backward pass for adjusting weights
                    rnn.Backward(output, targets[i], inputs[i]);
                }

                // Average loss over inputs for the epoch
                totalLoss /= inputs.Length;

                if (totalLoss > lastLoss) Console.WriteLine($"Epoch {epoch + 1}: Loss did not decrease. Loss: {totalLoss}");
                lastLoss = totalLoss;

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            if (lastLoss >= initialLoss) Console.WriteLine("Loss did not decrease over 10 epochs, indicating lack of learning.");

            /*
            Epoch 1/10, Loss: 0,6664938959957104
            Epoch 2/10, Loss: 0,6664753699565654
            Epoch 3/10, Loss: 0,6664550547239552
            Epoch 4/10, Loss: 0,6664327604847938
            Epoch 5/10, Loss: 0,666408279036776
            Epoch 6/10, Loss: 0,6663813818585805
            Epoch 7/10, Loss: 0,6663518179941391
            Epoch 8/10, Loss: 0,6663193117319616
            Epoch 9/10, Loss: 0,6662835600587623
            Epoch 10/10, Loss: 0,6662442298647243 
            */

            /*
            Epoch 1/10, Loss: 0,6664904366515348
            Epoch 2/10, Loss: 0,6664662278966218
            Epoch 3/10, Loss: 0,6664401635753903
            Epoch 4/10, Loss: 0,6664119994803214
            Epoch 5/10, Loss: 0,6663814720514686
            Epoch 6/10, Loss: 0,6663482959251658
            Epoch 7/10, Loss: 0,6663121612853719
            Epoch 8/10, Loss: 0,6662727309938327
            Epoch 9/10, Loss: 0,66622963747341
            Epoch 10/10, Loss: 0,666182479316904 
            */

            Console.ReadLine();
        }
    }
}
