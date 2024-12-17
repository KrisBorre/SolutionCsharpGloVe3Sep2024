using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe1Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double learningRate = 0.00001; // 0.0001; // 0.001; // 0.01;

            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50; // Assuming GloVe's 50-dimensional embeddings

            // Load GloVe embeddings
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            // Define a simple RNN
            int hiddenSize = 100;
            var rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            // Training data: sentences broken into word sequences
            var sentences = new List<string[]>
            {
                new string[] { "the", "quick", "brown", "fox" },
                new string[] { "jumps", "over", "the", "lazy", "dog" }
            };

            // Prepare training data as sequences of word embeddings
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

            // Train the RNN on these sequences
            int epochs = 1000;
            for (int epoch = 0; epoch <= epochs; epoch++)
            {
                double totalLoss = 0;
                for (int t = 0; t < inputs.Count; t++)
                {
                    var input = inputs[t];
                    var target = targets[t];

                    var output = rnn.Forward(input);

                    // Compute the loss (MSE)
                    double loss = 0;
                    for (int i = 0; i < embeddingDim; i++)
                    {
                        loss += Math.Pow(output[i] - target[i], 2);
                    }
                    totalLoss += loss;

                    // Backpropagate
                    rnn.Backward(output, target, input);
                }

                Console.WriteLine($"Epoch {epoch}/{epochs}, Loss: {totalLoss}");
            }

            {
                // Test prediction
                string testSentence = "the quick brown";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();

                /*
                Epoch 94/100, Loss: 909406372,5177114
                Epoch 95/100, Loss: 654455534,9964719
                Epoch 96/100, Loss: 1172861399,9153728
                Epoch 97/100, Loss: 954879718,5039252
                Epoch 98/100, Loss: 1386834509,710786
                Epoch 99/100, Loss: 1485827499,7905033
                Epoch 100/100, Loss: 1554897949,4559417
                Input: the quick brown
                capitol capitol capitol capitol capitol
                */


            }

            var embeddingFox = glove.GetEmbedding("fox");
            Console.Write("embeddingFox = ");
            foreach (double x in embeddingFox)
            {
                Console.Write(x + " ");
            }
            Console.WriteLine();
            var embeddingCapitol = glove.GetEmbedding("capitol");
            Console.Write("embeddingCapitol = ");
            foreach (double x in embeddingCapitol)
            {
                Console.Write(x + " ");
            }
            Console.WriteLine();

            Console.WriteLine("Fox    Capitol     verschil");
            int k = 0;
            foreach (double x in embeddingFox)
            {
                double y = embeddingCapitol[k];
                Console.WriteLine(x + " " + y + " " + (x - y));
                k++;
            }

            Console.WriteLine("{0,-10} {1,-10} {2,-10}", "Fox", "Capitol", "Verschil");
            int k2 = 0;
            foreach (double x in embeddingFox)
            {
                double y = embeddingCapitol[k2];
                Console.WriteLine("{0,-10:0.0000} {1,-10:0.0000} {2,-10:0.0000}", x, y, x - y);
                k2++;
            }


            {
                string testSentence = "the charge of the electron is";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }


            {
                string testSentence = "the charge of the electron";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            {
                string testSentence = "the charge";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            /*
            Input: the quick brown
            litwack feeder litwack feeder litwack
            Input: the charge of the electron is
            feeder litwack feeder litwack feeder
            Input: the charge of the electron
            litwack feeder litwack feeder litwack
            Input: the charge
            feeder litwack feeder litwack feeder
            */

            {
                string testSentence = "jumps over the lazy";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            {
                string testSentence = "jumps over the";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            {
                string testSentence = "jumps over";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            {
                string testSentence = "jumps";
                var testWords = testSentence.Split(' ');

                Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    Console.Write($"{word} ");
                }
                Console.WriteLine();

                double[] currentEmbedding = glove.GetEmbedding(testWords.Last());
                for (int i = 0; i < 5; i++) // Predict the next 5 words
                {
                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    Console.Write(predictedWord + " ");

                    // Update the input with the current prediction
                    currentEmbedding = outputEmbedding;
                }
                Console.WriteLine();
            }

            /*
            Input: jumps over the lazy
            rated mammoliti rated mammoliti rated
            Input: jumps over the
            mammoliti rated mammoliti rated mammoliti
            Input: jumps over
            rated mammoliti rated mammoliti rated
            Input: jumps
            mammoliti rated mammoliti rated mammoliti
            */

            /*
            Input: the charge of the electron is
            gener pfr agents lego difficile
            Input: the charge of the electron
            subchapter classless deserted ah-1w gaels
            Input: the charge
            perilous seibold bedevil rochlin schuylerville
            Input: jumps over the lazy
            verdura dooming cheddar diatonic udara
            Input: jumps over the
            frond sanmina corned strapping stettiner
            Input: jumps over
            harteveldt madoffs zagel ndamukong kampaku
            Input: jumps
            hardwoods wanjin holdup hanns 10,000-acre
            */


            Console.ReadLine();
        }
    }
}
