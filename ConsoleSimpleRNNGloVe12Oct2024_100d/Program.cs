using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe12Oct2024_100d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.100d.txt";
            int embeddingDim = 100;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 10;
            double learningRate = 0.1;
            int epochs = 10000;
            /*
            Epoch 9998/10000, Loss: 0,001960920117329368
            Epoch 9999/10000, Loss: 0,0019609194332464107
            Epoch 10000/10000, Loss: 0,0019609187492441492
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
            Predicted: toward earth earth way

            Input: proton has
            Predicted: it negative this same
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
