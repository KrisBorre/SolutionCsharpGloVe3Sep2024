using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe7Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 1; //5; //50;
            double learningRate = 0.1; //0.01;
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
                if ((epoch + 1) % 5 == 0)
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
            Epoch 95/100, Loss: 3,9947229149924034E-10
            molecule
            Epoch 100/100, Loss: 1,2712619234091507E-10
            molecule
            */

            /*
            molecule
            Epoch 35/100, Loss: 0,002688729390451943
            molecule
            Epoch 40/100, Loss: 0,001084698573172363
            molecule
            Epoch 45/100, Loss: 0,00032337467950223185
            molecule
            Epoch 50/100, Loss: 8,156908082584315E-05
            molecule
            Epoch 55/100, Loss: 1,9020439595927792E-05
            molecule
            Epoch 60/100, Loss: 4,293839390764017E-06
            molecule
            Epoch 65/100, Loss: 9,619266275375197E-07
            molecule
            Epoch 70/100, Loss: 2,1736763744271294E-07
            molecule
            Epoch 75/100, Loss: 5,028058924933561E-08
            molecule
            Epoch 80/100, Loss: 1,2100275382582309E-08
            molecule
            Epoch 85/100, Loss: 3,0826548056666477E-09
            molecule
            Epoch 90/100, Loss: 8,436123762405742E-10
            molecule
            Epoch 95/100, Loss: 2,495328782475803E-10
            molecule
            Epoch 100/100, Loss: 7,936171402118144E-11
            molecule
            */

            Console.Read();
        }
    }
}
