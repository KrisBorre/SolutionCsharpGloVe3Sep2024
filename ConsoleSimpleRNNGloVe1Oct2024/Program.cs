﻿using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe1Oct2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;

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

            int epochs = 100; // 100000; // 100;
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
            Epoch 98/100, Loss: 1,5917399115957527
            Epoch 99/100, Loss: 1,5917374978562286
            Epoch 100/100, Loss: 1,5917350840314997
            Input: the quick brown
            klan klan flag touch touch
            */

            /*
            Epoch 99999/100000, Loss: 0,5449772964945327
            Epoch 100000/100000, Loss: 0,5449705504389316
            Input: the quick brown
            lastWord = brown
            fox but little little little
            */

            var embeddingFox = glove.GetEmbedding("fox");
            var embeddingKlan = glove.GetEmbedding("klan");

            Console.WriteLine("{0,-10} {1,-10} {2,-10}", "Fox", "Klan", "Verschil");
            int k2 = 0;
            foreach (double x in embeddingFox)
            {
                double y = embeddingKlan[k2];
                Console.WriteLine("{0,-10:0.0000} {1,-10:0.0000} {2,-10:0.0000}", x, y, x - y);
                k2++;
            }


            Console.ReadLine();
        }
    }
}