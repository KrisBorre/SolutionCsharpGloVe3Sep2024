using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe11Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            //int hiddenSize = 1;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,004660185110841957
            //Epoch 10000 / 10000, Loss: 0,004660180668407635
            //Predicted sentence:
            //the quick brown a a a a a

            //int hiddenSize = 2;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 10000 / 10000, Loss: 0,003298832222268828
            //Predicted sentence:
            //the quick brown fox fox is is a

            int hiddenSize = 3;
            double learningRate = 0.1;
            int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,002092384827888379
            //Epoch 10000 / 10000, Loss: 0,002092383988303989
            //Predicted sentence:
            //the quick brown fox fox is is a

            //int hiddenSize = 4;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,0011002563789741999
            //Epoch 10000 / 10000, Loss: 0,0011002557048908135
            //Predicted sentence:
            //the quick brown fox fox fox fox fox


            //int hiddenSize = 6;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,00023811005700156775
            //Epoch 10000 / 10000, Loss: 0,00023810963480382636
            //Predicted sentence:
            //the quick brown fox fox a fox fox

            //int hiddenSize = 8;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 10000 / 10000, Loss: 0,00023817739421416444
            //Predicted sentence:
            //the quick brown fox fox fox fox fox

            //int hiddenSize = 10;
            //double learningRate = 0.1;
            //int epochs = 10000;
            /*
            Epoch 9998/10000, Loss: 0,0002381269433272637
            Epoch 9999/10000, Loss: 0,00023812652267525842
            Epoch 10000/10000, Loss: 0,0002381261020898171
            Predicted sentence:
            the quick brown fox fox a fox fox
            */

            //int hiddenSize = 10;
            //double learningRate = 0.05;
            //int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,0002360079575785809
            //Epoch 10000 / 10000, Loss: 0,0002360076497875368
            //Predicted sentence:
            //the quick brown fox fox fox fox fox

            //int hiddenSize = 10;
            //double learningRate = 0.02;
            //int epochs = 10000;
            //Epoch 10000 / 10000, Loss: 0,00023141524540152615
            //Predicted sentence:
            //the quick brown fox fox fox fox fox

            //int hiddenSize = 10;
            //double learningRate = 0.01;
            //int epochs = 10000;
            //Epoch 9999 / 10000, Loss: 0,0002288873967514789
            //Epoch 10000 / 10000, Loss: 0,0002288873879882145
            //Predicted sentence:
            //the quick brown fox fox fox fox fox

            //int hiddenSize = 10;
            //double learningRate = 0.005;
            //int epochs = 10000;
            //Epoch 10000 / 10000, Loss: 0,00023202744145594978
            //Predicted sentence:
            //the quick brown fox fox fox fox fox

            //int hiddenSize = 10;
            //double learningRate = 0.001;
            //int epochs = 10000;
            //Epoch 10000 / 10000, Loss: 0,0038165734349849185
            //Predicted sentence:
            //the quick brown a a a a a


            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            // Example training sentence sequences (words to be converted into GloVe embeddings)
            List<string[]> trainingSentences = new List<string[]>
                {
                    new string[] { "the", "quick", "brown", "fox" },
                    new string[] { "king", "is", "a", "man" },
                    new string[] { "queen", "is", "a", "woman" }
                };

            // Training the RNN to predict the next word in a sentence
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                foreach (var sentence in trainingSentences)
                {
                    // Convert words to embeddings
                    List<double[]> sentenceEmbeddings = sentence
                        .Select(word => glove.GetEmbedding(word))
                        .ToList();

                    // Forward and Backward pass for each pair in the sentence
                    for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                    {
                        double[] inputEmbedding = sentenceEmbeddings[i];
                        double[] targetEmbedding = sentenceEmbeddings[i + 1];

                        // Forward pass
                        double[] outputEmbedding = rnn.Forward(inputEmbedding);

                        // Calculate Mean Squared Error loss for the current word pair
                        double loss = 0;
                        for (int j = 0; j < outputEmbedding.Length; j++)
                        {
                            loss += Math.Pow(outputEmbedding[j] - targetEmbedding[j], 2);
                        }
                        loss /= embeddingDim;  // Normalize loss by embedding dimension
                        totalLoss += loss;

                        // Backward pass
                        rnn.Backward(outputEmbedding, targetEmbedding, inputEmbedding);
                    }
                }

                // Average loss over sentences
                totalLoss /= trainingSentences.Count;

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            // Inference: Predict the next word in a sentence
            var testSentence = new string[] { "the", "quick", "brown" };
            List<double[]> testEmbeddings = testSentence.Select(word => glove.GetEmbedding(word)).ToList();

            Console.WriteLine("Predicted sentence:");
            foreach (var embedding in testEmbeddings)
            {
                string word = glove.FindClosestWord(embedding);
                Console.Write(word + " ");
            }

            // Predict the next word in sequence
            double[] currentEmbedding = testEmbeddings.Last();
            for (int i = 0; i < 5; i++) // Generate 5 words
            {
                currentEmbedding = rnn.Forward(currentEmbedding);
                string nextWord = glove.FindClosestWord(currentEmbedding);
                Console.Write(nextWord + " ");
            }



            Console.Read();
        }
    }
}

