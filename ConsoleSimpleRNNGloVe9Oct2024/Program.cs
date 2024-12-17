using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe9Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 3;
            double learningRate = 0.1;
            int epochs = 300;

            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            var inputWord1 = "atom";
            var targetWord1 = "molecule";

            double[] inputEmbedding1 = glove.GetEmbedding(inputWord1);
            double[] targetEmbedding1 = glove.GetEmbedding(targetWord1);

            var inputWord2 = "king";
            var targetWord2 = "queen";

            double[] inputEmbedding2 = glove.GetEmbedding(inputWord2);
            double[] targetEmbedding2 = glove.GetEmbedding(targetWord2);

            var inputWord3 = "electron";
            var targetWord3 = "proton";

            double[] inputEmbedding3 = glove.GetEmbedding(inputWord3);
            double[] targetEmbedding3 = glove.GetEmbedding(targetWord3);

            // Simple sequence data with an input pattern and its subsequent target
            double[][] inputs = new double[][]
            {
                inputEmbedding1,
                inputEmbedding2,
                inputEmbedding3
            };

            // Target output patterns, shifting the pattern right
            double[][] targets = new double[][]
            {
                targetEmbedding1,
                targetEmbedding2,
                targetEmbedding3
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
                    string inputWord = glove.FindClosestWord(inputs[i]);
                    string predictedWord = glove.FindClosestWord(output);
                    Console.WriteLine(inputWord + " " + predictedWord);

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

                // Assert that the loss decreases (at least generally)
                if (totalLoss > lastLoss)
                {
                    Console.WriteLine($"Epoch {epoch + 1}: Loss did not decrease. Loss: {totalLoss}");
                }

                lastLoss = totalLoss;

                // Display loss for early epochs to track improvement
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");

            }

            // Assert that learning took place by checking for an overall loss reduction compared to the initial loss
            if (lastLoss >= initialLoss)
            {
                Console.WriteLine("Loss did not decrease over 10 epochs, indicating lack of learning.");
            }

            /*
            Epoch 298/300, Loss: 0,006424621908672258
            atom molecule
            king queen
            electron proton
            Epoch 299/300, Loss: 0,006050136647008583
            atom molecule
            king queen
            electron proton
            Epoch 300/300, Loss: 0,005692082705613347
            */

            /*
            Epoch 297/300, Loss: 0,012182480580437055
            atom molecule
            king queen
            electron proton
            Epoch 298/300, Loss: 0,011622270680772711
            atom molecule
            king queen
            electron proton
            Epoch 299/300, Loss: 0,011075412121487738
            atom molecule
            king queen
            electron proton
            Epoch 300/300, Loss: 0,010542450520722134
            */

            Console.Read();
        }
    }
}
