using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe12Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            //int hiddenSize = 10;
            //double learningRate = 0.1;
            //int epochs = 10000;
            /*
            Epoch 9999/10000, Loss: 0,001994326994346442
            Epoch 10000/10000, Loss: 0,001994326593968964
            Testing trained RNN on physics sentences:
            Input: energy is conserved in system
            Predicted: is conserved in system

            Input: force equals mass times acceleration
            Predicted: equals mass times acceleration

            Input: light travels in vacuum at speed
            Predicted: travels in system at speed

            Input: electron has negative charge
            Predicted: has negative charge

            Input: gravity pulls objects towards earth
            Predicted: pulls objects towards earth
            */

            //int hiddenSize = 20;
            //double learningRate = 0.1;
            //int epochs = 50000;
            //Epoch 49999 / 50000, Loss: 0,0004973796822719372
            //Epoch 50000 / 50000, Loss: 0,0004973796791757903
            //Testing trained RNN on physics sentences:
            //Input: energy is conserved in system
            //Predicted: is conserved in system

            //Input: force equals mass times acceleration
            //Predicted: equals mass times acceleration

            //Input: light travels in vacuum at speed
            //Predicted: travels in system at speed

            //Input: electron has negative charge
            //Predicted: has negative charge

            //Input: gravity pulls objects towards earth
            //Predicted: pulls objects towards earth

            //Input: ice age
            //Predicted: uses where speed earth

            //int hiddenSize = 50;
            //double learningRate = 0.1;
            //int epochs = 50000;
            /*
            Epoch 49999/50000, Loss: 0,0004977466350442091
            Epoch 50000/50000, Loss: 0,0004977466348559134
            Testing trained RNN on physics sentences:
            Input: energy is conserved in system
            Predicted: is conserved in system

            Input: force equals mass times acceleration
            Predicted: equals mass times acceleration

            Input: light travels in vacuum at speed
            Predicted: travels in system at speed

            Input: electron has negative charge
            Predicted: has negative charge

            Input: gravity pulls objects towards earth
            Predicted: pulls objects towards earth

            Input: ice age
            Predicted: uses where speed earth
            */

            //int hiddenSize = 50;
            //double learningRate = 0.1;
            //int epochs = 500000;
            //Epoch 499999 / 500000, Loss: 0,000494131538465192
            //Epoch 500000 / 500000, Loss: 0,0004941312527248951
            //Testing trained RNN on physics sentences:
            //Input: energy is conserved in system
            //Predicted: is conserved in system

            //Input: force equals mass times acceleration
            //Predicted: equals mass times acceleration

            //Input: light travels in vacuum at speed
            //Predicted: travels in system at speed

            //Input: electron has negative charge
            //Predicted: has negative charge

            //Input: gravity pulls objects towards earth
            //Predicted: pulls objects towards earth

            //Input: ice age
            //Predicted: uses where speed earth

            //Input: proton has
            //Predicted: times negative charge charge

            int hiddenSize = 150;
            double learningRate = 0.1;
            int epochs = 1000000;
            /*
            Epoch 999999/1000000, Loss: 0,0004940404247415689
            Epoch 1000000/1000000, Loss: 0,0004940404246033493
            Testing trained RNN on physics sentences:
            Input: energy is conserved in system
            Predicted: is conserved in system

            Input: force equals mass times acceleration
            Predicted: equals mass times acceleration

            Input: light travels in vacuum at speed
            Predicted: travels in system at speed

            Input: electron has negative charge
            Predicted: has negative charge

            Input: gravity pulls objects towards earth
            Predicted: pulls objects towards earth

            Input: ice age
            Predicted: uses where speed earth

            Input: proton has
            Predicted: times negative charge charge
            */

            /*
            Epoch 999998/1000000, Loss: 0,0004940289986579893
            Epoch 999999/1000000, Loss: 0,0004940289985564167
            Epoch 1000000/1000000, Loss: 0,0004940289984548453
            Testing trained RNN on physics sentences:
            Input: energy is conserved in system
            Predicted: is conserved in system

            Input: force equals mass times acceleration
            Predicted: equals mass times acceleration

            Input: light travels in vacuum at speed
            Predicted: travels in system at speed

            Input: electron has negative charge
            Predicted: has negative charge

            Input: gravity pulls objects towards earth
            Predicted: pulls objects towards earth

            Input: ice age
            Predicted: uses where speed earth

            Input: proton has
            Predicted: times negative charge charge 
            */


            //int hiddenSize = 50;
            //double learningRate = 0.01;
            //int epochs = 500000;
            //Epoch 499999 / 500000, Loss: 0,0004746301225323586
            //Epoch 500000 / 500000, Loss: 0,0004746301202361378
            //Testing trained RNN on physics sentences:
            //Input: energy is conserved in system
            //Predicted: is conserved in system

            //Input: force equals mass times acceleration
            //Predicted: equals mass times acceleration

            //Input: light travels in vacuum at speed
            //Predicted: travels in system at speed

            //Input: electron has negative charge
            //Predicted: has negative charge

            //Input: gravity pulls objects towards earth
            //Predicted: pulls objects towards earth

            //Input: ice age
            //Predicted: uses situated speed earth

            //int hiddenSize = 50;
            //double learningRate = 0.001;
            //int epochs = 5000000;
            /*
            Epoch 4999999 / 5000000, Loss: 0,00047195554080461277
            Epoch 5000000 / 5000000, Loss: 0,000471955540755209
            Testing trained RNN on physics sentences:
            Input: energy is conserved in system
            Predicted: is conserved in system

            Input: force equals mass times acceleration
            Predicted: equals mass times acceleration

            Input: light travels in vacuum at speed
            Predicted: travels in system at speed

            Input: electron has negative charge
            Predicted: has negative charge

            Input: gravity pulls objects towards earth
            Predicted: pulls objects towards earth

            Input: ice age
            Predicted: uses situated speed earth

            Input: proton has
            Predicted: times negative charge any
            */

            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            // Five physics-related sentences for training
            List<string[]> physicsSentences = new List<string[]>
                {
                    new string[] { "energy", "is", "conserved", "in", "system" },
                    new string[] { "force", "equals", "mass", "times", "acceleration" },
                    new string[] { "light", "travels", "in", "vacuum", "at", "speed" },
                    new string[] { "electron", "has", "negative", "charge" },
                    new string[] { "gravity", "pulls", "objects", "towards", "earth" }
                };

            // Training the RNN on these sentences
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                foreach (var sentence in physicsSentences)
                {
                    List<double[]> physicsSentenceEmbeddings = sentence
                        .Select(word => glove.GetEmbedding(word))
                        .ToList();

                    for (int i = 0; i < physicsSentenceEmbeddings.Count - 1; i++)
                    {
                        double[] inputEmbedding = physicsSentenceEmbeddings[i];
                        double[] targetEmbedding = physicsSentenceEmbeddings[i + 1];

                        // Forward pass
                        double[] trainOutputEmbedding = rnn.Forward(inputEmbedding);

                        // Calculate Mean Squared Error loss for the current word pair
                        double loss = 0;
                        for (int j = 0; j < trainOutputEmbedding.Length; j++)
                        {
                            loss += Math.Pow(trainOutputEmbedding[j] - targetEmbedding[j], 2);
                        }
                        loss /= embeddingDim;
                        totalLoss += loss;

                        // Backward pass
                        rnn.Backward(trainOutputEmbedding, targetEmbedding, inputEmbedding);
                    }
                }

                totalLoss /= physicsSentences.Count;
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            // Testing the RNN to see if it can recall physics sentences
            Console.WriteLine("Testing trained RNN on physics sentences:");
            foreach (var sentence in physicsSentences)
            {
                Console.Write("Input: ");
                foreach (var word in sentence)
                {
                    Console.Write(word + " ");
                }
                Console.Write("\nPredicted: ");

                List<double[]> sentenceEmbeddings = sentence
                    .Select(word => glove.GetEmbedding(word))
                    .ToList();

                for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                {
                    double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                    string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                    Console.Write(predictedWord + " ");
                }
                Console.WriteLine("\n");
            }


            var sentenceIceAge = new string[] { "ice", "age" };
            Console.Write("Input: ");
            foreach (var word in sentenceIceAge)
            {
                Console.Write(word + " ");
            }
            Console.Write("\nPredicted: ");

            List<double[]> iceAgeSentenceEmbeddings = sentenceIceAge
                .Select(word => glove.GetEmbedding(word))
                .ToList();

            double[] outputEmbedding1 = null;
            for (int i = 0; i < iceAgeSentenceEmbeddings.Count - 1; i++)
            {
                outputEmbedding1 = rnn.Forward(iceAgeSentenceEmbeddings[i]);
                string predictedWord = glove.FindClosestWord(outputEmbedding1);
                Console.Write(predictedWord + " ");
            }
            for (int i = 0; i < 3; i++)
            {
                outputEmbedding1 = rnn.Forward(outputEmbedding1);
                string predictedWord = glove.FindClosestWord(outputEmbedding1);
                Console.Write(predictedWord + " ");
            }
            Console.WriteLine("\n");


            var sentenceProton = new string[] { "proton", "has" };
            Console.Write("Input: ");
            foreach (var word in sentenceProton)
            {
                Console.Write(word + " ");
            }
            Console.Write("\nPredicted: ");

            List<double[]> protonSentenceEmbeddings = sentenceProton
                .Select(word => glove.GetEmbedding(word))
                .ToList();

            double[] outputEmbedding2 = null;
            for (int i = 0; i < protonSentenceEmbeddings.Count - 1; i++)
            {
                outputEmbedding2 = rnn.Forward(protonSentenceEmbeddings[i]);
                string predictedWord = glove.FindClosestWord(outputEmbedding2);
                Console.Write(predictedWord + " ");
            }
            for (int i = 0; i < 3; i++)
            {
                outputEmbedding2 = rnn.Forward(outputEmbedding2);
                string predictedWord = glove.FindClosestWord(outputEmbedding2);
                Console.Write(predictedWord + " ");
            }
            Console.WriteLine("\n");


            Console.Read();
        }
    }
}
