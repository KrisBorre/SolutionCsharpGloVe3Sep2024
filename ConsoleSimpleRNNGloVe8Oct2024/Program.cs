using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe8Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 1;
            double learningRate = 0.1;
            int epochs = 15;

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

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {loss}");
                string predictedWord = glove.FindClosestWord(output);
                Console.WriteLine(predictedWord);

                lastLoss = loss;
            }

            // Assert that loss decreased over time, indicating learning
            if (lastLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            /*
            Epoch 14/15, Loss: 0,0062327031009388025
            molecule
            Epoch 15/15, Loss: 0,006212327737959545
            molecule
            */

            /*
            Epoch 1/15, Loss: 0,006307302845823598
            surfactant
            Epoch 2/15, Loss: 0,00630648611078336
            surfactant
            Epoch 3/15, Loss: 0,006305498838600948
            surfactant
            Epoch 4/15, Loss: 0,006304283724892044
            surfactant
            Epoch 5/15, Loss: 0,006302770895166308
            surfactant
            Epoch 6/15, Loss: 0,00630087392232245
            surfactant
            Epoch 7/15, Loss: 0,006298484959496198
            surfactant
            Epoch 8/15, Loss: 0,006295468732123768
            surfactant
            Epoch 9/15, Loss: 0,006291655084146611
            surfactant
            Epoch 10/15, Loss: 0,006286829715722002
            surfactant
            Epoch 11/15, Loss: 0,0062807226856391295
            surfactant
            Epoch 12/15, Loss: 0,006272994185466556
            molecule
            Epoch 13/15, Loss: 0,006263217033273327
            molecule
            Epoch 14/15, Loss: 0,006250855298782081
            molecule
            Epoch 15/15, Loss: 0,006235238486593025
            molecule 
            */

            /*
            Epoch 1/15, Loss: 0,006307704288584712
            electrolyte
            Epoch 2/15, Loss: 0,006306877613125817
            electrolyte
            Epoch 3/15, Loss: 0,006305905856344136
            electrolyte
            Epoch 4/15, Loss: 0,006304731887152121
            electrolyte
            Epoch 5/15, Loss: 0,006303287633483331
            electrolyte
            Epoch 6/15, Loss: 0,00630149015561405
            electrolyte
            Epoch 7/15, Loss: 0,006299236936250102
            electrolyte
            Epoch 8/15, Loss: 0,006296400134944333
            electrolyte
            Epoch 9/15, Loss: 0,006292819510219434
            electrolyte
            Epoch 10/15, Loss: 0,006288293659215841
            electrolyte
            Epoch 11/15, Loss: 0,006282569163421717
            surfactant
            Epoch 12/15, Loss: 0,006275327163886894
            surfactant
            Epoch 13/15, Loss: 0,0062661668278719655
            membrane
            Epoch 14/15, Loss: 0,006254585125010551
            molecule
            Epoch 15/15, Loss: 0,006239952328642357
            molecule 
            */

            Console.Read();
        }
    }
}
