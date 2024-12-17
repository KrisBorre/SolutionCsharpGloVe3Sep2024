using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe5Oct2024_200d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.200d.txt";
            int embeddingDim = 200;

            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 50;
            double learningRate = 0.01;
            int epochs = 1000;

            var rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            var inputWord = "atom";
            var targetWord = "molecule";

            double[] inputEmbedding = glove.GetEmbedding(inputWord);
            double[] targetEmbedding = glove.GetEmbedding(targetWord);

            // Track loss over epochs
            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                // Forward pass
                double[] output = rnn.Forward(inputEmbedding);

                // Calculate mean squared error loss
                double loss = 0;
                for (int j = 0; j < embeddingDim; j++)
                {
                    loss += Math.Pow(output[j] - targetEmbedding[j], 2);
                }
                loss /= embeddingDim;

                // Backward pass to update weights
                rnn.Backward(output, targetEmbedding, inputEmbedding);

                // Check for decreasing loss
                if (epoch == 0)
                {
                    initialLoss = loss;
                }

                // Log progress at regular intervals
                if ((epoch + 1) % 100 == 0)
                {
                    Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {loss}");
                    string predictedWord = glove.FindClosestWord(output);
                    Console.WriteLine(predictedWord);
                }

                lastLoss = loss;
            }

            // Assert that loss decreased over time, indicating learning
            if (lastLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            /*
            Epoch 800/1000, Loss: 6,980134148968751E-14
            molecule
            Epoch 900/1000, Loss: 2,4340947836726086E-15
            molecule
            Epoch 1000/1000, Loss: 8,549089559739435E-17
            molecule
            */

            Console.Read();
        }
    }
}
