using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe6Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 50;
            double learningRate = 0.01;
            int epochs = 100;

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
                if ((epoch + 1) % 10 == 0)
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
            Epoch 90/100, Loss: 0,00528871567736884
            molecule
            Epoch 100/100, Loss: 0,005056068628987154
            molecule
            */

            /*
            Epoch 10/100, Loss: 0,006166530375408093
            monooxygenase
            Epoch 20/100, Loss: 0,006118314461708246
            potassium
            Epoch 30/100, Loss: 0,006060294128263513
            potassium
            Epoch 40/100, Loss: 0,0059896269754249176
            potassium
            Epoch 50/100, Loss: 0,005903065217714073
            potassium
            Epoch 60/100, Loss: 0,005796928189336308
            potassium
            Epoch 70/100, Loss: 0,005667127902440168
            potassium
            Epoch 80/100, Loss: 0,0055092767419408105
            molecule
            Epoch 90/100, Loss: 0,005318913506617554
            molecule
            Epoch 100/100, Loss: 0,0050918857225510025
            molecule 
            */

            Console.Read();
        }
    }
}
