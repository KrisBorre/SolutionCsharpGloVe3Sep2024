using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe5Oct2024
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
            Epoch 100/1000, Loss: 0,005031964344027361
            molecule
            Epoch 200/1000, Loss: 0,0012694459630869365
            molecule
            Epoch 300/1000, Loss: 6,304849509332542E-05
            molecule
            Epoch 400/1000, Loss: 4,2509614684215675E-06
            molecule
            Epoch 500/1000, Loss: 3,7718158870454097E-07
            molecule
            Epoch 600/1000, Loss: 3,4666484859186344E-08
            molecule
            Epoch 700/1000, Loss: 3,1967299001334982E-09
            molecule
            Epoch 800/1000, Loss: 2,948740165322385E-10
            molecule
            Epoch 900/1000, Loss: 2,7200977070796976E-11
            molecule
            Epoch 1000/1000, Loss: 2,5092263103673697E-12
            molecule
            */

            /*
            Epoch 100/1000, Loss: 0,004986678357426421
            molecule
            Epoch 200/1000, Loss: 0,0011996144632869885
            molecule
            Epoch 300/1000, Loss: 5,654614365755756E-05
            molecule
            Epoch 400/1000, Loss: 3,7368131316502403E-06
            molecule
            Epoch 500/1000, Loss: 3,2778248505265385E-07
            molecule
            Epoch 600/1000, Loss: 2,9807018414263806E-08
            molecule
            Epoch 700/1000, Loss: 2,719641648384878E-09
            molecule
            Epoch 800/1000, Loss: 2,482231136234123E-10
            molecule
            Epoch 900/1000, Loss: 2,2656498642676866E-11
            molecule
            Epoch 1000/1000, Loss: 2,0680145842949668E-12
            molecule 
            */

            /*
            Epoch 100/1000, Loss: 0,0051275496838817354
            molecule
            Epoch 200/1000, Loss: 0,0013849802761081728
            molecule
            Epoch 300/1000, Loss: 6,86356584196363E-05
            molecule
            Epoch 400/1000, Loss: 4,44247211098792E-06
            molecule
            Epoch 500/1000, Loss: 3,8866809785331346E-07
            molecule
            Epoch 600/1000, Loss: 3,5391806614430204E-08
            molecule
            Epoch 700/1000, Loss: 3,23488957460463E-09
            molecule
            Epoch 800/1000, Loss: 2,9577899445969406E-10
            molecule
            Epoch 900/1000, Loss: 2,704544439303205E-11
            molecule
            Epoch 1000/1000, Loss: 2,473025898710645E-12
            molecule
            */

            Console.Read();
        }
    }
}
