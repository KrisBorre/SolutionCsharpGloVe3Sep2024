﻿using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe2Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool verbose = false;

            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;

            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 100;

            double learningRate = 0.1;
            for (int experiment = 0; experiment < 10; experiment++)
            {
                var rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate / 2.0);

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

                int epochs = 100;
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

                    if (verbose) Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
                }

                string testSentence = "the quick brown";
                var testWords = testSentence.Split(' ');

                if (verbose) Console.Write("Input: ");
                foreach (var word in testWords)
                {
                    if (verbose) Console.Write($"{word} ");
                }
                if (verbose) Console.WriteLine();

                string lastWord = testWords.Last();
                if (verbose) Console.WriteLine("lastWord = " + lastWord);
                double[] currentEmbedding = glove.GetEmbedding(lastWord);

                double[] foxEmbedding_ = null;

                for (int i = 0; i < 5; i++)
                {

                    var outputEmbedding = rnn.Forward(currentEmbedding);
                    if (i == 0)
                    {
                        foxEmbedding_ = outputEmbedding;
                    }
                    string predictedWord = glove.FindClosestWord(outputEmbedding);

                    if (verbose) Console.Write(predictedWord + " ");
                    currentEmbedding = outputEmbedding;
                }
                if (verbose) Console.WriteLine();

                /*
                Epoch 98/100, Loss: 1,5917399115957527
                Epoch 99/100, Loss: 1,5917374978562286
                Epoch 100/100, Loss: 1,5917350840314997
                Input: the quick brown
                klan klan flag touch touch
                */

                var embeddingFox = glove.GetEmbedding("fox");

                // Chebychev distance
                double maxVerschil = double.MinValue;
                if (verbose) Console.WriteLine("{0,-10} {1,-10} {2,-10}", "Fox", "Output", "Verschil");
                int k = 0;
                foreach (double x in embeddingFox)
                {
                    double y = foxEmbedding_[k];
                    double verschil = Math.Abs(x - y);
                    if (verbose) Console.WriteLine("{0,-10:0.0000} {1,-10:0.0000} {2,-10:0.0000}", x, y, verschil);
                    if (maxVerschil < verschil)
                    {
                        maxVerschil = verschil;
                    }
                    k++;
                }
                Console.Write("maxVerschil = " + maxVerschil);
                Console.WriteLine();
            }

            /*
            maxVerschil = 0,11740651194127348
            maxVerschil = 0,1175438816268394
            maxVerschil = 0,11783490227374634
            maxVerschil = 0,11848825072986123
            maxVerschil = 0,11738623650948662
            maxVerschil = 0,11720518827133139
            maxVerschil = 0,11790171304065916
            maxVerschil = 0,11769662355965768
            maxVerschil = 0,11764269393779551
            maxVerschil = 0,11790223304456973
            */

            Console.ReadLine();
        }
    }
}
