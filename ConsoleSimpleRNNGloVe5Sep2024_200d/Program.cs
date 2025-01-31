using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe5Sep2024_200d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.200d.txt";
            int embeddingDim = 200;

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
            Epoch 100/1000, Loss: 0,49738920461704694
            gene
            Epoch 200/1000, Loss: 0,17050557250794618
            high-wing
            Epoch 300/1000, Loss: 0,0829947131530822
            recessive
            Epoch 400/1000, Loss: 0,12019796308651282
            dubhda
            Epoch 500/1000, Loss: 0,08920417859758087
            characterises
            Epoch 600/1000, Loss: 0,033838846918186276
            peptides
            Epoch 700/1000, Loss: 0,10882802666203524
            stacks
            Epoch 800/1000, Loss: 0,028339464174812326
            molecule
            Epoch 900/1000, Loss: 0,09629584391292978
            gomory
            Epoch 1000/1000, Loss: 0,02962228086770638
            inhibits
            */

            /*
            Epoch 100/1000, Loss: 3,556972311803074E-06
            molecule
            Epoch 200/1000, Loss: 8,523351140328949E-10
            molecule
            Epoch 300/1000, Loss: 2,0425822514366446E-13
            molecule
            Epoch 400/1000, Loss: 4,89535446923422E-17
            molecule
            Epoch 500/1000, Loss: 1,1733232629292797E-20
            molecule
            Epoch 600/1000, Loss: 2,812310114611496E-24
            molecule
            Epoch 700/1000, Loss: 6,74825254501251E-28
            molecule
            Epoch 800/1000, Loss: 1,3437197225095793E-31
            molecule
            Epoch 900/1000, Loss: 2,642719024849589E-32
            molecule
            Epoch 1000/1000, Loss: 2,7116938713678727E-32
            molecule
            */

            /*
            Epoch 100/1000, Loss: 3,88558778186648E-06
            molecule
            Epoch 200/1000, Loss: 9,280240403791245E-10
            molecule
            Epoch 300/1000, Loss: 2,2165141681173996E-13
            molecule
            Epoch 400/1000, Loss: 5,29410038143438E-17
            molecule
            Epoch 500/1000, Loss: 1,2645148373127853E-20
            molecule
            Epoch 600/1000, Loss: 3,0202477202369898E-24
            molecule
            Epoch 700/1000, Loss: 7,216022691870943E-28
            molecule
            Epoch 800/1000, Loss: 1,4325261611601806E-31
            molecule
            Epoch 900/1000, Loss: 2,5899452906245307E-32
            molecule
            Epoch 1000/1000, Loss: 2,3930901807789074E-32
            molecule
            */

            /*
            Epoch 100/1000, Loss: 0,4323421440522274
            insubstantial
            Epoch 200/1000, Loss: 0,076927416759662
            dingli
            Epoch 300/1000, Loss: 0,13501457272881126
            equational
            Epoch 400/1000, Loss: 0,27702684539140887
            versace
            Epoch 500/1000, Loss: 0,14342465543285493
            c-130s
            Epoch 600/1000, Loss: 0,11183907854851016
            rite
            Epoch 700/1000, Loss: 0,020530660437535656
            molecule
            Epoch 800/1000, Loss: 0,020321871118700856
            molecule
            Epoch 900/1000, Loss: 0,01151548869515855
            molecule
            Epoch 1000/1000, Loss: 0,03469234854614884
            grollo
            */

            /*
            Epoch 100/1000, Loss: 4,118879557667392E-06
            molecule
            Epoch 200/1000, Loss: 9,928667857623536E-10
            molecule
            Epoch 300/1000, Loss: 2,3940902401378476E-13
            molecule
            Epoch 400/1000, Loss: 5,775384499139927E-17
            molecule
            Epoch 500/1000, Loss: 1,3939643151529096E-20
            molecule
            Epoch 600/1000, Loss: 3,366992508870139E-24
            molecule
            Epoch 700/1000, Loss: 8,164113644811807E-28
            molecule
            Epoch 800/1000, Loss: 1,9076200526200806E-31
            molecule
            Epoch 900/1000, Loss: 2,7337103065570763E-31
            molecule
            Epoch 1000/1000, Loss: 1,5772863094493196E-31
            molecule
            */

            /*
            Epoch 100/1000, Loss: 4,29309146168008E-06
            molecule
            Epoch 200/1000, Loss: 1,0312242642757336E-09
            molecule
            Epoch 300/1000, Loss: 2,477102880095971E-13
            molecule
            Epoch 400/1000, Loss: 5,950290253906067E-17
            molecule
            Epoch 500/1000, Loss: 1,429336953585385E-20
            molecule
            Epoch 600/1000, Loss: 3,4333064211327206E-24
            molecule
            Epoch 700/1000, Loss: 8,26061359503676E-28
            molecule
            Epoch 800/1000, Loss: 1,2467876682248986E-31
            molecule
            Epoch 900/1000, Loss: 2,60009398053714E-32
            molecule
            Epoch 1000/1000, Loss: 3,056141622394691E-32
            molecule
            */

            /*
            Epoch 100/1000, Loss: 3,8323292891473486E-06
            molecule
            Epoch 200/1000, Loss: 9,154837171120239E-10
            molecule
            Epoch 300/1000, Loss: 2,1870700192629505E-13
            molecule
            Epoch 400/1000, Loss: 5,22518966004966E-17
            molecule
            Epoch 500/1000, Loss: 1,2484382543948277E-20
            molecule
            Epoch 600/1000, Loss: 2,9831501403416128E-24
            molecule
            Epoch 700/1000, Loss: 7,106411382309402E-28
            molecule
            Epoch 800/1000, Loss: 1,6076395278170627E-31
            molecule
            Epoch 900/1000, Loss: 3,0508460574457774E-32
            molecule
            Epoch 1000/1000, Loss: 3,2319685875371095E-32
            molecule
            */

            Console.Read();
        }
    }
}
