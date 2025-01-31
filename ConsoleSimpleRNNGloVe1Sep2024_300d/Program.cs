using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe1Sep2024_300d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
            int embeddingDim = 300;

            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 100;
            var rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: 0.0001);

            var sentences = new List<string[]>
            {
                new string[] { "the", "quick", "brown", "fox" },
                new string[] { "jumps", "over", "the", "lazy", "dog" }
            };

            var inputs = new List<double[]>();
            var targets = new List<double[]>();

            foreach (var sentence in sentences)
            {
                for (int i = 0; i < sentence.Length - 1; i++)
                {
                    var wordEmbedding = glove.GetEmbedding(sentence[i]);
                    var nextWordEmbedding = glove.GetEmbedding(sentence[i + 1]);

                    inputs.Add(wordEmbedding);
                    targets.Add(nextWordEmbedding);
                }
            }

            int epochs = 100000;
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;
                for (int t = 0; t < inputs.Count; t++)
                {
                    var input = inputs[t];
                    var target = targets[t];

                    var output = rnn.Forward(input);

                    double loss = 0;
                    for (int i = 0; i < embeddingDim; i++)
                    {
                        loss += Math.Pow(output[i] - target[i], 2);
                    }
                    totalLoss += loss;

                    rnn.Backward(output, target, input);
                }

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            string testSentence = "the quick brown";
            var testWords = testSentence.Split(' ');

            Console.Write("Input: ");
            foreach (var word in testWords)
            {
                Console.Write($"{word} ");
            }
            Console.WriteLine();

            string lastWord = testWords.Last();
            Console.WriteLine("lastWord = " + lastWord);
            double[] currentEmbedding = glove.GetEmbedding(lastWord);

            for (int i = 0; i < 5; i++)
            {
                var outputEmbedding = rnn.Forward(currentEmbedding);
                string predictedWord = glove.FindClosestWord(outputEmbedding);

                Console.Write(predictedWord + " ");
                currentEmbedding = outputEmbedding;
            }
            Console.WriteLine();

            /*
            Epoch 99997/100000, Loss: 9,932667874199556
            Epoch 99998/100000, Loss: 16,421930295493045
            Epoch 99999/100000, Loss: 8,429411263994199
            Epoch 100000/100000, Loss: 7,464308342493434
            Input: the quick brown
            lastWord = brown
            fox thin brown over dakota
            */

            /*
            Epoch 99997/100000, Loss: 30,224603429889186
            Epoch 99998/100000, Loss: 19,42419437561515
            Epoch 99999/100000, Loss: 18,285691421944172
            Epoch 100000/100000, Loss: 6,214301286075354
            Input: the quick brown
            lastWord = brown
            either quick fox n't place 
            */

            /*
            Epoch 99997/100000, Loss: 4,980170769853851
            Epoch 99998/100000, Loss: 3,956753094267339
            Epoch 99999/100000, Loss: 3,7055885920027105
            Epoch 100000/100000, Loss: 5,511702149105716
            Input: the quick brown
            lastWord = brown
            dog dog brown fox white
            */


            /*
            Epoch 99998/100000, Loss: 6,4131729617156905
            Epoch 99999/100000, Loss: 11,347880646987809
            Epoch 100000/100000, Loss: 10,270554445833207
            Input: the quick brown
            lastWord = brown
            each n't big webb this
            */


            /*
            Epoch 99998/100000, Loss: 3,45016305119264
            Epoch 99999/100000, Loss: 5,214723556956765
            Epoch 100000/100000, Loss: 5,290071450967071
            Input: the quick brown
            lastWord = brown
            quick brown quick brown fox
            */



            Console.ReadLine();
        }
    }
}
