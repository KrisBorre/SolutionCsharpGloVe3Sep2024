using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe7Sep2024
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
            Epoch 5/100, Loss: 0,001050886046649792
            molecule
            Epoch 10/100, Loss: 0,0001124141271815726
            molecule
            Epoch 15/100, Loss: 1,2026784207612764E-05
            molecule
            Epoch 20/100, Loss: 1,286700850010409E-06
            molecule
            Epoch 25/100, Loss: 1,376593099116774E-07
            molecule
            Epoch 30/100, Loss: 1,4727654282482796E-08
            molecule
            Epoch 35/100, Loss: 1,5756566004135879E-09
            molecule
            Epoch 40/100, Loss: 1,685736013575215E-10
            molecule
            Epoch 45/100, Loss: 1,803505856959801E-11
            molecule
            Epoch 50/100, Loss: 1,9295034037878488E-12
            molecule
            Epoch 55/100, Loss: 2,064303462526651E-13
            molecule
            Epoch 60/100, Loss: 2,2085209992410465E-14
            molecule
            Epoch 65/100, Loss: 2,3628139432149126E-15
            molecule
            Epoch 70/100, Loss: 2,5278861874001234E-16
            molecule
            Epoch 75/100, Loss: 2,7044908011860668E-17
            molecule
            Epoch 80/100, Loss: 2,893433470513619E-18
            molecule
            Epoch 85/100, Loss: 3,0955761545617473E-19
            molecule
            Epoch 90/100, Loss: 3,3118411361164046E-20
            molecule
            Epoch 95/100, Loss: 3,543214808158848E-21
            molecule
            Epoch 100/100, Loss: 3,790752180916639E-22
            molecule
            */


            /*           
            Epoch 5/100, Loss: 0,0010345194844711262
            molecule
            Epoch 10/100, Loss: 0,00011057818678593196
            molecule
            Epoch 15/100, Loss: 1,1836958601337518E-05
            molecule
            Epoch 20/100, Loss: 1,267318606487657E-06
            molecule
            Epoch 25/100, Loss: 1,3568787592300766E-07
            molecule
            Epoch 30/100, Loss: 1,4527718169786845E-08
            molecule
            Epoch 35/100, Loss: 1,5554423212695847E-09
            molecule
            Epoch 40/100, Loss: 1,6653688356849248E-10
            molecule
            Epoch 45/100, Loss: 1,783064106334139E-11
            molecule
            Epoch 50/100, Loss: 1,9090771611936057E-12
            molecule
            Epoch 55/100, Loss: 2,0439958354552841E-13
            molecule
            Epoch 60/100, Loss: 2,1884495085974576E-14
            molecule
            Epoch 65/100, Loss: 2,3431120401684648E-15
            molecule
            Epoch 70/100, Loss: 2,508704912712918E-16
            molecule
            Epoch 75/100, Loss: 2,686000595097056E-17
            molecule
            Epoch 80/100, Loss: 2,8758261473830687E-18
            molecule
            Epoch 85/100, Loss: 3,079067110593367E-19
            molecule
            Epoch 90/100, Loss: 3,2966715274419617E-20
            molecule
            Epoch 95/100, Loss: 3,52965456624409E-21
            molecule
            Epoch 100/100, Loss: 3,779103690191553E-22
            molecule
            */


            Console.Read();
        }
    }
}
