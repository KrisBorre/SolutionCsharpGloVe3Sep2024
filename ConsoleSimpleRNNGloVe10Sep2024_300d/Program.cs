using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe10Sep2024_300d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load Glove embeddings
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
            int embeddingDim = 300;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 5;  // Adjusted hidden size for learning capacity
            double learningRate = 0.1;  // Adjusted learning rate for gradual learning
            int epochs = 300;  // Increased epochs for better training over time

            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            // Define input-target pairs using GloVe embeddings
            string[] inputWords = { "atom", "king", "electron" };
            string[] targetWords = { "molecule", "queen", "proton" };

            double[][] inputs = inputWords.Select(w => glove.GetEmbedding(w)).ToArray();
            double[][] targets = targetWords.Select(w => glove.GetEmbedding(w)).ToArray();

            // Validate that embeddings were successfully loaded
            if (inputs.Contains(null) || targets.Contains(null))
            {
                Console.WriteLine("Error: One or more words are not in the GloVe dictionary.");
                return;
            }

            // Track initial and last loss
            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;

            // Training loop over epochs
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                // Process each input-target pair
                for (int i = 0; i < inputs.Length; i++)
                {
                    // Forward pass
                    double[] output = rnn.Forward(inputs[i]);

                    // Calculate Mean Squared Error loss for the current pair
                    double loss = 0;
                    for (int j = 0; j < output.Length; j++)
                    {
                        loss += Math.Pow(output[j] - targets[i][j], 2);
                    }
                    loss /= embeddingDim;  // Normalize loss by the embedding dimension
                    totalLoss += loss;

                    // Backward pass
                    rnn.Backward(output, targets[i], inputs[i]);

                    // Display closest words for input and predicted output
                    string inputWord = glove.FindClosestWord(inputs[i]);
                    string predictedWord = glove.FindClosestWord(output);
                    string targetWord = glove.FindClosestWord(targets[i]);
                    Console.WriteLine($"Input: {inputWord}, Target: {targetWord}, Predicted: {predictedWord}");
                }

                // Average loss for the epoch
                totalLoss /= inputs.Length;

                // Check if the loss decreases
                if (totalLoss > lastLoss)
                {
                    Console.WriteLine($"Warning: Epoch {epoch + 1}: Loss did not decrease. Loss: {totalLoss}");
                }

                lastLoss = totalLoss;

                // Display progress at regular intervals
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Average Loss: {totalLoss}");

                // Set initial loss after the first epoch to gauge improvement
                if (epoch == 0) initialLoss = lastLoss;
            }

            // Verify that overall learning occurred
            if (lastLoss < initialLoss)
            {
                Console.WriteLine("Model successfully decreased loss, indicating learning over epochs.");
            }
            else
            {
                Console.WriteLine("Loss did not decrease significantly, consider revising parameters.");
            }

            /*
            Input: atom, Target: molecule, Predicted: 39-32
            Input: king, Target: queen, Predicted: hemmings
            Input: electron, Target: proton, Predicted: ccia
            Epoch 1/300, Average Loss: 0,14112596078328618
            Input: atom, Target: molecule, Predicted: houses
            Input: king, Target: queen, Predicted: le
            Input: electron, Target: proton, Predicted: proton
            Epoch 2/300, Average Loss: 0,011813088275819159
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 3/300, Average Loss: 0,002080744476116616
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 4/300, Average Loss: 0,0013853431897755293
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 5: Loss did not decrease. Loss: 0,0013856976850519162
            Epoch 5/300, Average Loss: 0,0013856976850519162
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 6: Loss did not decrease. Loss: 0,0013858117606342735
            Epoch 6/300, Average Loss: 0,0013858117606342735
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 7/300, Average Loss: 0,001385809562668801
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 8/300, Average Loss: 0,0013858082840492306
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 9/300, Average Loss: 0,0013858082448903436
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 10: Loss did not decrease. Loss: 0,0013858082739178562
            Epoch 10/300, Average Loss: 0,0013858082739178562
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 11/300, Average Loss: 0,0013858082568771613
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 12: Loss did not decrease. Loss: 0,001385808277992415
            Epoch 12/300, Average Loss: 0,001385808277992415
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 13/300, Average Loss: 0,0013858082589584993
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0013858082805695896
            Epoch 14/300, Average Loss: 0,0013858082805695896
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 15/300, Average Loss: 0,0013858082611793356
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 16: Loss did not decrease. Loss: 0,001385808282365331
            Epoch 16/300, Average Loss: 0,001385808282365331
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 17/300, Average Loss: 0,0013858082633744486
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0013858082835749505
            Epoch 18/300, Average Loss: 0,0013858082835749505
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 19/300, Average Loss: 0,0013858082654794777
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 20: Loss did not decrease. Loss: 0,0013858082843524126
            Epoch 20/300, Average Loss: 0,0013858082843524126
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 21/300, Average Loss: 0,0013858082674542396
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0013858082848149714
            Epoch 22/300, Average Loss: 0,0013858082848149714
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 23/300, Average Loss: 0,001385808269276264
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0013858082850514836
            Epoch 24/300, Average Loss: 0,0013858082850514836
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 25/300, Average Loss: 0,0013858082709356492
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 26: Loss did not decrease. Loss: 0,001385808285128873
            Epoch 26/300, Average Loss: 0,001385808285128873
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 27/300, Average Loss: 0,0013858082724312055
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0013858082850971775
            Epoch 28/300, Average Loss: 0,0013858082850971775
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 29/300, Average Loss: 0,0013858082737675987
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 30: Loss did not decrease. Loss: 0,001385808284993486
            Epoch 30/300, Average Loss: 0,001385808284993486
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 31/300, Average Loss: 0,0013858082749532488
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0013858082848449947
            Epoch 32/300, Average Loss: 0,0013858082848449947
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 33/300, Average Loss: 0,0013858082759987962
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0013858082846713877
            Epoch 34/300, Average Loss: 0,0013858082846713877
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 35/300, Average Loss: 0,0013858082769160117
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0013858082844866657
            Epoch 36/300, Average Loss: 0,0013858082844866657
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 37/300, Average Loss: 0,0013858082777170242
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0013858082843005717
            Epoch 38/300, Average Loss: 0,0013858082843005717
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 39/300, Average Loss: 0,0013858082784138047
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0013858082841196712
            Epoch 40/300, Average Loss: 0,0013858082841196712
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 41/300, Average Loss: 0,0013858082790178092
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 42: Loss did not decrease. Loss: 0,001385808283948193
            Epoch 42/300, Average Loss: 0,001385808283948193
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 43/300, Average Loss: 0,0013858082795397781
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0013858082837886537
            Epoch 44/300, Average Loss: 0,0013858082837886537
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 45/300, Average Loss: 0,0013858082799896104
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 46: Loss did not decrease. Loss: 0,00138580828364235
            Epoch 46/300, Average Loss: 0,00138580828364235
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 47/300, Average Loss: 0,0013858082803763154
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0013858082835097151
            Epoch 48/300, Average Loss: 0,0013858082835097151
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 49/300, Average Loss: 0,0013858082807080118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 50: Loss did not decrease. Loss: 0,001385808283390587
            Epoch 50/300, Average Loss: 0,001385808283390587
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 51/300, Average Loss: 0,0013858082809919453
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0013858082832844162
            Epoch 52/300, Average Loss: 0,0013858082832844162
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 53/300, Average Loss: 0,0013858082812345472
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 54: Loss did not decrease. Loss: 0,001385808283190409
            Epoch 54/300, Average Loss: 0,001385808283190409
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 55/300, Average Loss: 0,0013858082814414824
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 56: Loss did not decrease. Loss: 0,001385808283107634
            Epoch 56/300, Average Loss: 0,001385808283107634
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 57/300, Average Loss: 0,0013858082816177238
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 58: Loss did not decrease. Loss: 0,0013858082830350959
            Epoch 58/300, Average Loss: 0,0013858082830350959
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 59/300, Average Loss: 0,0013858082817676098
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0013858082829717965
            Epoch 60/300, Average Loss: 0,0013858082829717965
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 61/300, Average Loss: 0,0013858082818949148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 62: Loss did not decrease. Loss: 0,001385808282916761
            Epoch 62/300, Average Loss: 0,001385808282916761
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 63/300, Average Loss: 0,0013858082820029087
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 64: Loss did not decrease. Loss: 0,001385808282869067
            Epoch 64/300, Average Loss: 0,001385808282869067
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 65/300, Average Loss: 0,0013858082820944184
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0013858082828278552
            Epoch 66/300, Average Loss: 0,0013858082828278552
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 67/300, Average Loss: 0,001385808282171879
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 68: Loss did not decrease. Loss: 0,001385808282792336
            Epoch 68/300, Average Loss: 0,001385808282792336
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 69/300, Average Loss: 0,0013858082822373826
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0013858082827617948
            Epoch 70/300, Average Loss: 0,0013858082827617948
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 71/300, Average Loss: 0,0013858082822927268
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 72: Loss did not decrease. Loss: 0,0013858082827355898
            Epoch 72/300, Average Loss: 0,0013858082827355898
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 73/300, Average Loss: 0,0013858082823394458
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 74: Loss did not decrease. Loss: 0,0013858082827131494
            Epoch 74/300, Average Loss: 0,0013858082827131494
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 75/300, Average Loss: 0,0013858082823788532
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 76: Loss did not decrease. Loss: 0,0013858082826939653
            Epoch 76/300, Average Loss: 0,0013858082826939653
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 77/300, Average Loss: 0,0013858082824120689
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 78: Loss did not decrease. Loss: 0,0013858082826775934
            Epoch 78/300, Average Loss: 0,0013858082826775934
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 79/300, Average Loss: 0,0013858082824400452
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 80: Loss did not decrease. Loss: 0,0013858082826636398
            Epoch 80/300, Average Loss: 0,0013858082826636398
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 81/300, Average Loss: 0,0013858082824635936
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0013858082826517651
            Epoch 82/300, Average Loss: 0,0013858082826517651
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 83/300, Average Loss: 0,0013858082824834026
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 84: Loss did not decrease. Loss: 0,001385808282641671
            Epoch 84/300, Average Loss: 0,001385808282641671
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 85/300, Average Loss: 0,0013858082825000575
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 86: Loss did not decrease. Loss: 0,0013858082826331021
            Epoch 86/300, Average Loss: 0,0013858082826331021
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 87/300, Average Loss: 0,0013858082825140494
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 88: Loss did not decrease. Loss: 0,001385808282625835
            Epoch 88/300, Average Loss: 0,001385808282625835
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 89/300, Average Loss: 0,0013858082825258025
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 90: Loss did not decrease. Loss: 0,0013858082826196775
            Epoch 90/300, Average Loss: 0,0013858082826196775
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 91/300, Average Loss: 0,0013858082825356673
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 92: Loss did not decrease. Loss: 0,001385808282614467
            Epoch 92/300, Average Loss: 0,001385808282614467
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 93/300, Average Loss: 0,0013858082825439434
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 94: Loss did not decrease. Loss: 0,0013858082826100602
            Epoch 94/300, Average Loss: 0,0013858082826100602
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 95/300, Average Loss: 0,001385808282550885
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 96: Loss did not decrease. Loss: 0,0013858082826063364
            Epoch 96/300, Average Loss: 0,0013858082826063364
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 97/300, Average Loss: 0,0013858082825567045
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 98: Loss did not decrease. Loss: 0,001385808282603193
            Epoch 98/300, Average Loss: 0,001385808282603193
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 99/300, Average Loss: 0,0013858082825615788
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 100: Loss did not decrease. Loss: 0,0013858082826005396
            Epoch 100/300, Average Loss: 0,0013858082826005396
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 101/300, Average Loss: 0,0013858082825656639
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 102: Loss did not decrease. Loss: 0,001385808282598302
            Epoch 102/300, Average Loss: 0,001385808282598302
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 103/300, Average Loss: 0,001385808282569084
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 104: Loss did not decrease. Loss: 0,0013858082825964192
            Epoch 104/300, Average Loss: 0,0013858082825964192
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 105/300, Average Loss: 0,0013858082825719466
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 106: Loss did not decrease. Loss: 0,001385808282594831
            Epoch 106/300, Average Loss: 0,001385808282594831
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 107/300, Average Loss: 0,001385808282574343
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 108: Loss did not decrease. Loss: 0,001385808282593496
            Epoch 108/300, Average Loss: 0,001385808282593496
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 109/300, Average Loss: 0,0013858082825763465
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 110: Loss did not decrease. Loss: 0,0013858082825923721
            Epoch 110/300, Average Loss: 0,0013858082825923721
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 111/300, Average Loss: 0,0013858082825780223
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 112: Loss did not decrease. Loss: 0,0013858082825914271
            Epoch 112/300, Average Loss: 0,0013858082825914271
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 113/300, Average Loss: 0,0013858082825794233
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 114: Loss did not decrease. Loss: 0,001385808282590634
            Epoch 114/300, Average Loss: 0,001385808282590634
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 115/300, Average Loss: 0,0013858082825805942
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 116: Loss did not decrease. Loss: 0,001385808282589967
            Epoch 116/300, Average Loss: 0,001385808282589967
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 117/300, Average Loss: 0,0013858082825815733
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0013858082825894062
            Epoch 118/300, Average Loss: 0,0013858082825894062
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 119/300, Average Loss: 0,001385808282582392
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 120: Loss did not decrease. Loss: 0,0013858082825889372
            Epoch 120/300, Average Loss: 0,0013858082825889372
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 121/300, Average Loss: 0,0013858082825830736
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 122: Loss did not decrease. Loss: 0,0013858082825885436
            Epoch 122/300, Average Loss: 0,0013858082825885436
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 123/300, Average Loss: 0,0013858082825836445
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 124: Loss did not decrease. Loss: 0,0013858082825882127
            Epoch 124/300, Average Loss: 0,0013858082825882127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 125/300, Average Loss: 0,0013858082825841205
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 126: Loss did not decrease. Loss: 0,0013858082825879356
            Epoch 126/300, Average Loss: 0,0013858082825879356
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 127/300, Average Loss: 0,0013858082825845182
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 128: Loss did not decrease. Loss: 0,0013858082825877035
            Epoch 128/300, Average Loss: 0,0013858082825877035
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 129/300, Average Loss: 0,0013858082825848501
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 130: Loss did not decrease. Loss: 0,0013858082825875088
            Epoch 130/300, Average Loss: 0,0013858082825875088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 131/300, Average Loss: 0,0013858082825851262
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 132: Loss did not decrease. Loss: 0,0013858082825873464
            Epoch 132/300, Average Loss: 0,0013858082825873464
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 133/300, Average Loss: 0,001385808282585357
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 134: Loss did not decrease. Loss: 0,0013858082825872087
            Epoch 134/300, Average Loss: 0,0013858082825872087
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 135/300, Average Loss: 0,0013858082825855492
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 136: Loss did not decrease. Loss: 0,0013858082825870957
            Epoch 136/300, Average Loss: 0,0013858082825870957
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 137/300, Average Loss: 0,00138580828258571
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0013858082825869995
            Epoch 138/300, Average Loss: 0,0013858082825869995
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 139/300, Average Loss: 0,0013858082825858435
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 140: Loss did not decrease. Loss: 0,001385808282586919
            Epoch 140/300, Average Loss: 0,001385808282586919
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 141/300, Average Loss: 0,0013858082825859552
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 142: Loss did not decrease. Loss: 0,0013858082825868522
            Epoch 142/300, Average Loss: 0,0013858082825868522
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 143/300, Average Loss: 0,001385808282586049
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 144: Loss did not decrease. Loss: 0,0013858082825867967
            Epoch 144/300, Average Loss: 0,0013858082825867967
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 145/300, Average Loss: 0,001385808282586127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 146: Loss did not decrease. Loss: 0,0013858082825867507
            Epoch 146/300, Average Loss: 0,0013858082825867507
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 147/300, Average Loss: 0,0013858082825861904
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 148: Loss did not decrease. Loss: 0,0013858082825867115
            Epoch 148/300, Average Loss: 0,0013858082825867115
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 149/300, Average Loss: 0,0013858082825862446
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 150: Loss did not decrease. Loss: 0,0013858082825866788
            Epoch 150/300, Average Loss: 0,0013858082825866788
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 151/300, Average Loss: 0,0013858082825862893
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 152: Loss did not decrease. Loss: 0,001385808282586651
            Epoch 152/300, Average Loss: 0,001385808282586651
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 153/300, Average Loss: 0,0013858082825863275
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 154: Loss did not decrease. Loss: 0,0013858082825866285
            Epoch 154/300, Average Loss: 0,0013858082825866285
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 155/300, Average Loss: 0,0013858082825863583
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 156: Loss did not decrease. Loss: 0,0013858082825866085
            Epoch 156/300, Average Loss: 0,0013858082825866085
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 157/300, Average Loss: 0,0013858082825863843
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 158: Loss did not decrease. Loss: 0,0013858082825865935
            Epoch 158/300, Average Loss: 0,0013858082825865935
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 159/300, Average Loss: 0,0013858082825864055
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 160: Loss did not decrease. Loss: 0,0013858082825865799
            Epoch 160/300, Average Loss: 0,0013858082825865799
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 161/300, Average Loss: 0,0013858082825864235
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 162: Loss did not decrease. Loss: 0,0013858082825865686
            Epoch 162/300, Average Loss: 0,0013858082825865686
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 163/300, Average Loss: 0,001385808282586439
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 164: Loss did not decrease. Loss: 0,00138580828258656
            Epoch 164/300, Average Loss: 0,00138580828258656
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 165/300, Average Loss: 0,0013858082825864506
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 166: Loss did not decrease. Loss: 0,001385808282586552
            Epoch 166/300, Average Loss: 0,001385808282586552
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 167/300, Average Loss: 0,0013858082825864615
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 168: Loss did not decrease. Loss: 0,0013858082825865456
            Epoch 168/300, Average Loss: 0,0013858082825865456
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 169/300, Average Loss: 0,0013858082825864704
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 170: Loss did not decrease. Loss: 0,0013858082825865404
            Epoch 170/300, Average Loss: 0,0013858082825865404
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 171/300, Average Loss: 0,0013858082825864773
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 172: Loss did not decrease. Loss: 0,0013858082825865354
            Epoch 172/300, Average Loss: 0,0013858082825865354
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 173/300, Average Loss: 0,0013858082825864834
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 174: Loss did not decrease. Loss: 0,0013858082825865322
            Epoch 174/300, Average Loss: 0,0013858082825865322
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 175/300, Average Loss: 0,0013858082825864877
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 176: Loss did not decrease. Loss: 0,0013858082825865283
            Epoch 176/300, Average Loss: 0,0013858082825865283
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 177/300, Average Loss: 0,0013858082825864923
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 178: Loss did not decrease. Loss: 0,0013858082825865252
            Epoch 178/300, Average Loss: 0,0013858082825865252
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 179/300, Average Loss: 0,0013858082825864964
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 180: Loss did not decrease. Loss: 0,0013858082825865235
            Epoch 180/300, Average Loss: 0,0013858082825865235
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 181/300, Average Loss: 0,0013858082825864983
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 182: Loss did not decrease. Loss: 0,0013858082825865218
            Epoch 182/300, Average Loss: 0,0013858082825865218
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 183/300, Average Loss: 0,001385808282586501
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 184: Loss did not decrease. Loss: 0,0013858082825865207
            Epoch 184/300, Average Loss: 0,0013858082825865207
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 185/300, Average Loss: 0,0013858082825865027
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 186: Loss did not decrease. Loss: 0,0013858082825865196
            Epoch 186/300, Average Loss: 0,0013858082825865196
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 187/300, Average Loss: 0,0013858082825865049
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 188: Loss did not decrease. Loss: 0,0013858082825865183
            Epoch 188/300, Average Loss: 0,0013858082825865183
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 189/300, Average Loss: 0,001385808282586506
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 190: Loss did not decrease. Loss: 0,001385808282586517
            Epoch 190/300, Average Loss: 0,001385808282586517
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 191/300, Average Loss: 0,0013858082825865079
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 192: Loss did not decrease. Loss: 0,0013858082825865166
            Epoch 192/300, Average Loss: 0,0013858082825865166
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 193/300, Average Loss: 0,0013858082825865088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 194: Loss did not decrease. Loss: 0,0013858082825865161
            Epoch 194/300, Average Loss: 0,0013858082825865161
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 195/300, Average Loss: 0,0013858082825865088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 196: Loss did not decrease. Loss: 0,0013858082825865161
            Epoch 196/300, Average Loss: 0,0013858082825865161
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 197/300, Average Loss: 0,0013858082825865088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 198: Loss did not decrease. Loss: 0,0013858082825865153
            Epoch 198/300, Average Loss: 0,0013858082825865153
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 199/300, Average Loss: 0,0013858082825865096
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 200: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 200/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 201/300, Average Loss: 0,0013858082825865105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 202: Loss did not decrease. Loss: 0,0013858082825865146
            Epoch 202/300, Average Loss: 0,0013858082825865146
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 203/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 204: Loss did not decrease. Loss: 0,0013858082825865146
            Epoch 204/300, Average Loss: 0,0013858082825865146
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 205/300, Average Loss: 0,0013858082825865118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 206: Loss did not decrease. Loss: 0,0013858082825865144
            Epoch 206/300, Average Loss: 0,0013858082825865144
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 207/300, Average Loss: 0,0013858082825865111
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 208: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 208/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 209/300, Average Loss: 0,0013858082825865118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 210: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 210/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 211/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 212: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 212/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 213/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 214: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 214/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 215/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 216: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 216/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 217/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 218/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 219: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 219/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 220/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 221: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 221/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 222/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 223: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 223/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 224/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 225: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 225/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 226/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 227/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 228: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 228/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 229/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 230/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 231/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 232: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 232/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 233/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 234: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 234/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 235/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 236: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 236/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 237/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 238/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 239/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 240: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 240/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 241/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 242/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 243/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 244/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 245/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 246/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 247: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 247/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 248/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 249/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 250: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 250/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 251/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 252/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 253/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 254: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 254/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 255/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 256/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 257/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 258: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 258/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 259/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 260/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 261/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 262: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 262/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 263/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 264/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 265/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 266/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 267/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 268/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 269/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 270: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 270/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 271/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 272/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 273/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 274/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 275/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 276/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 277: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 277/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 278/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 279/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 280/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 281/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 282: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 282/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 283/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 284/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 285/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 286/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 287/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 288/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 289/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 290/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 291/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 292/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 293/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 294/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 295/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 296/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 297/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 298/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 299/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 300/300, Average Loss: 0,0013858082825865127
            Model successfully decreased loss, indicating learning over epochs.
            */


            /*
            Input: atom, Target: molecule, Predicted: bra
            Input: king, Target: queen, Predicted: crossbench
            Input: electron, Target: proton, Predicted: prevented
            Epoch 1/300, Average Loss: 0,15485427887273853
            Input: atom, Target: molecule, Predicted: witmer
            Input: king, Target: queen, Predicted: nataly
            Input: electron, Target: proton, Predicted: molecule
            Epoch 2/300, Average Loss: 0,02069730447807828
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 3/300, Average Loss: 0,001894807388223422
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 4/300, Average Loss: 0,0013800166400616866
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 5: Loss did not decrease. Loss: 0,0013860874204954376
            Epoch 5/300, Average Loss: 0,0013860874204954376
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 6/300, Average Loss: 0,0013858784463506547
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 7/300, Average Loss: 0,0013858081776198562
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 8: Loss did not decrease. Loss: 0,0013858111089904424
            Epoch 8/300, Average Loss: 0,0013858111089904424
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 9/300, Average Loss: 0,0013858080784568353
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 10: Loss did not decrease. Loss: 0,001385810994889979
            Epoch 10/300, Average Loss: 0,001385810994889979
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 11/300, Average Loss: 0,001385808131047804
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 12: Loss did not decrease. Loss: 0,0013858104731276112
            Epoch 12/300, Average Loss: 0,0013858104731276112
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 13/300, Average Loss: 0,0013858081589854163
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0013858100541463668
            Epoch 14/300, Average Loss: 0,0013858100541463668
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 15/300, Average Loss: 0,0013858081819562121
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 16: Loss did not decrease. Loss: 0,001385809715298991
            Epoch 16/300, Average Loss: 0,001385809715298991
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 17/300, Average Loss: 0,0013858082006625912
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0013858094412697117
            Epoch 18/300, Average Loss: 0,0013858094412697117
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 19/300, Average Loss: 0,001385808215896691
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 20: Loss did not decrease. Loss: 0,0013858092196573195
            Epoch 20/300, Average Loss: 0,0013858092196573195
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 21/300, Average Loss: 0,0013858082283019397
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0013858090404339988
            Epoch 22/300, Average Loss: 0,0013858090404339988
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 23/300, Average Loss: 0,0013858082384027946
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0013858088954908107
            Epoch 24/300, Average Loss: 0,0013858088954908107
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 25/300, Average Loss: 0,0013858082466266612
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 26: Loss did not decrease. Loss: 0,001385808778270343
            Epoch 26/300, Average Loss: 0,001385808778270343
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 27/300, Average Loss: 0,001385808253321814
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0013858086834697542
            Epoch 28/300, Average Loss: 0,0013858086834697542
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 29/300, Average Loss: 0,0013858082587720163
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0013858086068006701
            Epoch 30/300, Average Loss: 0,0013858086068006701
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 31/300, Average Loss: 0,0013858082632084423
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0013858085447950716
            Epoch 32/300, Average Loss: 0,0013858085447950716
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 33/300, Average Loss: 0,0013858082668194063
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0013858084946483287
            Epoch 34/300, Average Loss: 0,0013858084946483287
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 35/300, Average Loss: 0,001385808269758295
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0013858084540922845
            Epoch 36/300, Average Loss: 0,0013858084540922845
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 37/300, Average Loss: 0,001385808272150035
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 38: Loss did not decrease. Loss: 0,001385808421292626
            Epoch 38/300, Average Loss: 0,001385808421292626
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 39/300, Average Loss: 0,0013858082740963648
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 40: Loss did not decrease. Loss: 0,001385808394765886
            Epoch 40/300, Average Loss: 0,001385808394765886
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 41/300, Average Loss: 0,001385808275680128
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 42: Loss did not decrease. Loss: 0,001385808373312337
            Epoch 42/300, Average Loss: 0,001385808373312337
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 43/300, Average Loss: 0,0013858082769687833
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0013858083559617179
            Epoch 44/300, Average Loss: 0,0013858083559617179
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 45/300, Average Loss: 0,0013858082780172536
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 46: Loss did not decrease. Loss: 0,001385808341929332
            Epoch 46/300, Average Loss: 0,001385808341929332
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 47/300, Average Loss: 0,001385808278870252
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0013858083305805697
            Epoch 48/300, Average Loss: 0,0013858083305805697
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 49/300, Average Loss: 0,001385808279564182
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0013858083214021912
            Epoch 50/300, Average Loss: 0,0013858083214021912
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 51/300, Average Loss: 0,0013858082801286717
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0013858083139791143
            Epoch 52/300, Average Loss: 0,0013858083139791143
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 53/300, Average Loss: 0,0013858082805878389
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 54: Loss did not decrease. Loss: 0,0013858083079756444
            Epoch 54/300, Average Loss: 0,0013858083079756444
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 55/300, Average Loss: 0,0013858082809613131
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0013858083031202861
            Epoch 56/300, Average Loss: 0,0013858083031202861
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 57/300, Average Loss: 0,0013858082812650701
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 58: Loss did not decrease. Loss: 0,0013858082991934702
            Epoch 58/300, Average Loss: 0,0013858082991934702
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 59/300, Average Loss: 0,0013858082815121104
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0013858082960176187
            Epoch 60/300, Average Loss: 0,0013858082960176187
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 61/300, Average Loss: 0,001385808281713012
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 62: Loss did not decrease. Loss: 0,0013858082934491145
            Epoch 62/300, Average Loss: 0,0013858082934491145
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 63/300, Average Loss: 0,0013858082818763832
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0013858082913718068
            Epoch 64/300, Average Loss: 0,0013858082913718068
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 65/300, Average Loss: 0,0013858082820092296
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0013858082896917587
            Epoch 66/300, Average Loss: 0,0013858082896917587
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 67/300, Average Loss: 0,0013858082821172459
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 68: Loss did not decrease. Loss: 0,0013858082883329986
            Epoch 68/300, Average Loss: 0,0013858082883329986
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 69/300, Average Loss: 0,0013858082822050713
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0013858082872340836
            Epoch 70/300, Average Loss: 0,0013858082872340836
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 71/300, Average Loss: 0,0013858082822764733
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 72: Loss did not decrease. Loss: 0,0013858082863453193
            Epoch 72/300, Average Loss: 0,0013858082863453193
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 73/300, Average Loss: 0,0013858082823345228
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 74: Loss did not decrease. Loss: 0,001385808285626518
            Epoch 74/300, Average Loss: 0,001385808285626518
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 75/300, Average Loss: 0,0013858082823817138
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 76: Loss did not decrease. Loss: 0,0013858082850451757
            Epoch 76/300, Average Loss: 0,0013858082850451757
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 77/300, Average Loss: 0,0013858082824200748
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 78: Loss did not decrease. Loss: 0,001385808284575007
            Epoch 78/300, Average Loss: 0,001385808284575007
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 79/300, Average Loss: 0,0013858082824512563
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 80: Loss did not decrease. Loss: 0,0013858082841947488
            Epoch 80/300, Average Loss: 0,0013858082841947488
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 81/300, Average Loss: 0,0013858082824766017
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0013858082838872079
            Epoch 82/300, Average Loss: 0,0013858082838872079
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 83/300, Average Loss: 0,0013858082824972015
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 84: Loss did not decrease. Loss: 0,001385808283638479
            Epoch 84/300, Average Loss: 0,001385808283638479
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 85/300, Average Loss: 0,001385808282513943
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 86: Loss did not decrease. Loss: 0,0013858082834373156
            Epoch 86/300, Average Loss: 0,0013858082834373156
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 87/300, Average Loss: 0,0013858082825275494
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 88: Loss did not decrease. Loss: 0,0013858082832746193
            Epoch 88/300, Average Loss: 0,0013858082832746193
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 89/300, Average Loss: 0,0013858082825386078
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 90: Loss did not decrease. Loss: 0,0013858082831430356
            Epoch 90/300, Average Loss: 0,0013858082831430356
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 91/300, Average Loss: 0,0013858082825475928
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 92: Loss did not decrease. Loss: 0,0013858082830366146
            Epoch 92/300, Average Loss: 0,0013858082830366146
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 93/300, Average Loss: 0,001385808282554895
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 94: Loss did not decrease. Loss: 0,0013858082829505448
            Epoch 94/300, Average Loss: 0,0013858082829505448
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 95/300, Average Loss: 0,001385808282560827
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 96: Loss did not decrease. Loss: 0,0013858082828809334
            Epoch 96/300, Average Loss: 0,0013858082828809334
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 97/300, Average Loss: 0,0013858082825656474
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 98: Loss did not decrease. Loss: 0,001385808282824634
            Epoch 98/300, Average Loss: 0,001385808282824634
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 99/300, Average Loss: 0,0013858082825695644
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 100: Loss did not decrease. Loss: 0,0013858082827791
            Epoch 100/300, Average Loss: 0,0013858082827791
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 101/300, Average Loss: 0,0013858082825727463
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 102: Loss did not decrease. Loss: 0,0013858082827422728
            Epoch 102/300, Average Loss: 0,0013858082827422728
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 103/300, Average Loss: 0,0013858082825753317
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 104: Loss did not decrease. Loss: 0,0013858082827124883
            Epoch 104/300, Average Loss: 0,0013858082827124883
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 105/300, Average Loss: 0,0013858082825774316
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 106: Loss did not decrease. Loss: 0,0013858082826883993
            Epoch 106/300, Average Loss: 0,0013858082826883993
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 107/300, Average Loss: 0,001385808282579137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 108: Loss did not decrease. Loss: 0,0013858082826689168
            Epoch 108/300, Average Loss: 0,0013858082826689168
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 109/300, Average Loss: 0,0013858082825805227
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 110: Loss did not decrease. Loss: 0,0013858082826531594
            Epoch 110/300, Average Loss: 0,0013858082826531594
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 111/300, Average Loss: 0,0013858082825816487
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 112: Loss did not decrease. Loss: 0,0013858082826404157
            Epoch 112/300, Average Loss: 0,0013858082826404157
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 113/300, Average Loss: 0,001385808282582562
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 114: Loss did not decrease. Loss: 0,001385808282630108
            Epoch 114/300, Average Loss: 0,001385808282630108
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 115/300, Average Loss: 0,0013858082825833056
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 116: Loss did not decrease. Loss: 0,001385808282621773
            Epoch 116/300, Average Loss: 0,001385808282621773
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 117/300, Average Loss: 0,001385808282583908
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0013858082826150306
            Epoch 118/300, Average Loss: 0,0013858082826150306
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 119/300, Average Loss: 0,0013858082825843976
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 120: Loss did not decrease. Loss: 0,0013858082826095775
            Epoch 120/300, Average Loss: 0,0013858082826095775
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 121/300, Average Loss: 0,0013858082825847957
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 122: Loss did not decrease. Loss: 0,001385808282605167
            Epoch 122/300, Average Loss: 0,001385808282605167
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 123/300, Average Loss: 0,0013858082825851184
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 124: Loss did not decrease. Loss: 0,0013858082826016004
            Epoch 124/300, Average Loss: 0,0013858082826016004
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 125/300, Average Loss: 0,001385808282585381
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 126: Loss did not decrease. Loss: 0,0013858082825987158
            Epoch 126/300, Average Loss: 0,0013858082825987158
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 127/300, Average Loss: 0,0013858082825855946
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 128: Loss did not decrease. Loss: 0,0013858082825963824
            Epoch 128/300, Average Loss: 0,0013858082825963824
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 129/300, Average Loss: 0,0013858082825857672
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 130: Loss did not decrease. Loss: 0,001385808282594495
            Epoch 130/300, Average Loss: 0,001385808282594495
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 131/300, Average Loss: 0,0013858082825859072
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 132: Loss did not decrease. Loss: 0,0013858082825929686
            Epoch 132/300, Average Loss: 0,0013858082825929686
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 133/300, Average Loss: 0,001385808282586021
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 134: Loss did not decrease. Loss: 0,0013858082825917335
            Epoch 134/300, Average Loss: 0,0013858082825917335
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 135/300, Average Loss: 0,001385808282586113
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 136: Loss did not decrease. Loss: 0,0013858082825907367
            Epoch 136/300, Average Loss: 0,0013858082825907367
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 137/300, Average Loss: 0,00138580828258619
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0013858082825899288
            Epoch 138/300, Average Loss: 0,0013858082825899288
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 139/300, Average Loss: 0,0013858082825862503
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 140: Loss did not decrease. Loss: 0,0013858082825892754
            Epoch 140/300, Average Loss: 0,0013858082825892754
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 141/300, Average Loss: 0,0013858082825862997
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 142: Loss did not decrease. Loss: 0,001385808282588747
            Epoch 142/300, Average Loss: 0,001385808282588747
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 143/300, Average Loss: 0,0013858082825863396
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 144: Loss did not decrease. Loss: 0,0013858082825883202
            Epoch 144/300, Average Loss: 0,0013858082825883202
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 145/300, Average Loss: 0,0013858082825863721
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 146: Loss did not decrease. Loss: 0,0013858082825879744
            Epoch 146/300, Average Loss: 0,0013858082825879744
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 147/300, Average Loss: 0,0013858082825863986
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 148: Loss did not decrease. Loss: 0,0013858082825876944
            Epoch 148/300, Average Loss: 0,0013858082825876944
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 149/300, Average Loss: 0,00138580828258642
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 150: Loss did not decrease. Loss: 0,0013858082825874694
            Epoch 150/300, Average Loss: 0,0013858082825874694
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 151/300, Average Loss: 0,0013858082825864376
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 152: Loss did not decrease. Loss: 0,001385808282587286
            Epoch 152/300, Average Loss: 0,001385808282587286
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 153/300, Average Loss: 0,0013858082825864513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 154: Loss did not decrease. Loss: 0,001385808282587138
            Epoch 154/300, Average Loss: 0,001385808282587138
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 155/300, Average Loss: 0,0013858082825864634
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 156: Loss did not decrease. Loss: 0,0013858082825870188
            Epoch 156/300, Average Loss: 0,0013858082825870188
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 157/300, Average Loss: 0,0013858082825864728
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 158: Loss did not decrease. Loss: 0,001385808282586922
            Epoch 158/300, Average Loss: 0,001385808282586922
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 159/300, Average Loss: 0,0013858082825864806
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 160: Loss did not decrease. Loss: 0,0013858082825868442
            Epoch 160/300, Average Loss: 0,0013858082825868442
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 161/300, Average Loss: 0,0013858082825864866
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 162: Loss did not decrease. Loss: 0,0013858082825867807
            Epoch 162/300, Average Loss: 0,0013858082825867807
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 163/300, Average Loss: 0,0013858082825864918
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 164: Loss did not decrease. Loss: 0,001385808282586729
            Epoch 164/300, Average Loss: 0,001385808282586729
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 165/300, Average Loss: 0,0013858082825864957
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 166: Loss did not decrease. Loss: 0,001385808282586689
            Epoch 166/300, Average Loss: 0,001385808282586689
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 167/300, Average Loss: 0,0013858082825864992
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 168: Loss did not decrease. Loss: 0,001385808282586655
            Epoch 168/300, Average Loss: 0,001385808282586655
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 169/300, Average Loss: 0,0013858082825865016
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 170: Loss did not decrease. Loss: 0,0013858082825866276
            Epoch 170/300, Average Loss: 0,0013858082825866276
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 171/300, Average Loss: 0,001385808282586504
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 172: Loss did not decrease. Loss: 0,0013858082825866057
            Epoch 172/300, Average Loss: 0,0013858082825866057
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 173/300, Average Loss: 0,0013858082825865057
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 174: Loss did not decrease. Loss: 0,0013858082825865881
            Epoch 174/300, Average Loss: 0,0013858082825865881
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 175/300, Average Loss: 0,0013858082825865066
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 176: Loss did not decrease. Loss: 0,0013858082825865745
            Epoch 176/300, Average Loss: 0,0013858082825865745
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 177/300, Average Loss: 0,0013858082825865075
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 178: Loss did not decrease. Loss: 0,001385808282586562
            Epoch 178/300, Average Loss: 0,001385808282586562
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 179/300, Average Loss: 0,0013858082825865088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 180: Loss did not decrease. Loss: 0,0013858082825865534
            Epoch 180/300, Average Loss: 0,0013858082825865534
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 181/300, Average Loss: 0,0013858082825865103
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 182: Loss did not decrease. Loss: 0,0013858082825865458
            Epoch 182/300, Average Loss: 0,0013858082825865458
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 183/300, Average Loss: 0,0013858082825865105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 184: Loss did not decrease. Loss: 0,0013858082825865391
            Epoch 184/300, Average Loss: 0,0013858082825865391
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 185/300, Average Loss: 0,0013858082825865103
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 186: Loss did not decrease. Loss: 0,0013858082825865335
            Epoch 186/300, Average Loss: 0,0013858082825865335
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 187/300, Average Loss: 0,0013858082825865111
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 188: Loss did not decrease. Loss: 0,0013858082825865296
            Epoch 188/300, Average Loss: 0,0013858082825865296
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 189/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 190: Loss did not decrease. Loss: 0,0013858082825865265
            Epoch 190/300, Average Loss: 0,0013858082825865265
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 191/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 192: Loss did not decrease. Loss: 0,001385808282586524
            Epoch 192/300, Average Loss: 0,001385808282586524
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 193/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 194: Loss did not decrease. Loss: 0,0013858082825865215
            Epoch 194/300, Average Loss: 0,0013858082825865215
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 195/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 196: Loss did not decrease. Loss: 0,0013858082825865196
            Epoch 196/300, Average Loss: 0,0013858082825865196
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 197/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 198: Loss did not decrease. Loss: 0,0013858082825865187
            Epoch 198/300, Average Loss: 0,0013858082825865187
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 199/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 200: Loss did not decrease. Loss: 0,0013858082825865187
            Epoch 200/300, Average Loss: 0,0013858082825865187
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 201/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 202: Loss did not decrease. Loss: 0,0013858082825865172
            Epoch 202/300, Average Loss: 0,0013858082825865172
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 203/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 204: Loss did not decrease. Loss: 0,0013858082825865161
            Epoch 204/300, Average Loss: 0,0013858082825865161
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 205/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 206: Loss did not decrease. Loss: 0,0013858082825865161
            Epoch 206/300, Average Loss: 0,0013858082825865161
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 207/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 208: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 208/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 209/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 210: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 210/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 211/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 212: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 212/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 213/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 214: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 214/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 215/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 216: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 216/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 217/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 218/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 219: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 219/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 220/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 221: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 221/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 222/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 223/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 224: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 224/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 225/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 226/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 227: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 227/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 228/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 229/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 230/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 231: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 231/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 232/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 233: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 233/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 234/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 235: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 235/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 236/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 237: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 237/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 238/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 239/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 240: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 240/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 241/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 242/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 243/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 244: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 244/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 245/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 246: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 246/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 247/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 248/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 249: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 249/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 250/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 251/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 252/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 253: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 253/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 254/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 255: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 255/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 256/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 257/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 258/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 259: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 259/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 260/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 261: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 261/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 262/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 263/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 264/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 265/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 266/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 267: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 267/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 268/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 269/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 270/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 271/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 272/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 273: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 273/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 274/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 275/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 276/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 277: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 277/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 278/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 279/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 280: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 280/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 281/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 282/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 283/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 284/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 285: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 285/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 286/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 287/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 288: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 288/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 289/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 290/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 291/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 292: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 292/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 293/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 294/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 295/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 296/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 297/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 298/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 299/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 300/300, Average Loss: 0,0013858082825865127
            Model successfully decreased loss, indicating learning over epochs. 
            */


            /*
            Input: atom, Target: molecule, Predicted: síochána
            Input: king, Target: queen, Predicted: easwaran
            Input: electron, Target: proton, Predicted: euronext
            Epoch 1/300, Average Loss: 0,14453140094504208
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: persephone
            Input: electron, Target: proton, Predicted: molecule
            Epoch 2/300, Average Loss: 0,010914684700023112
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 3/300, Average Loss: 0,0019386470787844017
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 4/300, Average Loss: 0,001381351210582871
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 5: Loss did not decrease. Loss: 0,001386023473909972
            Epoch 5/300, Average Loss: 0,001386023473909972
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 6/300, Average Loss: 0,0013858549510985217
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 7/300, Average Loss: 0,001385808364444972
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 8/300, Average Loss: 0,0013858080513118832
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 9: Loss did not decrease. Loss: 0,0013858083061552035
            Epoch 9/300, Average Loss: 0,0013858083061552035
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 10: Loss did not decrease. Loss: 0,0013858084032912327
            Epoch 10/300, Average Loss: 0,0013858084032912327
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 11/300, Average Loss: 0,0013858083089809744
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 12: Loss did not decrease. Loss: 0,001385808383060917
            Epoch 12/300, Average Loss: 0,001385808383060917
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 13/300, Average Loss: 0,0013858083014917557
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0013858083680705461
            Epoch 14/300, Average Loss: 0,0013858083680705461
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 15/300, Average Loss: 0,0013858082958826982
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0013858083553073177
            Epoch 16/300, Average Loss: 0,0013858083553073177
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 17/300, Average Loss: 0,0013858082916066966
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 18: Loss did not decrease. Loss: 0,001385808344451067
            Epoch 18/300, Average Loss: 0,001385808344451067
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 19/300, Average Loss: 0,0013858082883772603
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 20: Loss did not decrease. Loss: 0,0013858083352166727
            Epoch 20/300, Average Loss: 0,0013858083352166727
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 21/300, Average Loss: 0,0013858082859650419
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0013858083273617058
            Epoch 22/300, Average Loss: 0,0013858083273617058
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 23/300, Average Loss: 0,0013858082841880957
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0013858083206799892
            Epoch 24/300, Average Loss: 0,0013858083206799892
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 25/300, Average Loss: 0,0013858082829024166
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0013858083149961753
            Epoch 26/300, Average Loss: 0,0013858083149961753
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 27/300, Average Loss: 0,0013858082819943236
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 28: Loss did not decrease. Loss: 0,001385808310161136
            Epoch 28/300, Average Loss: 0,001385808310161136
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 29/300, Average Loss: 0,0013858082813743419
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0013858083060480357
            Epoch 30/300, Average Loss: 0,0013858083060480357
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 31/300, Average Loss: 0,0013858082809722902
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0013858083025490057
            Epoch 32/300, Average Loss: 0,0013858083025490057
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 33/300, Average Loss: 0,001385808280733326
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0013858082995722996
            Epoch 34/300, Average Loss: 0,0013858082995722996
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 35/300, Average Loss: 0,0013858082806147826
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0013858082970398882
            Epoch 36/300, Average Loss: 0,0013858082970398882
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 37/300, Average Loss: 0,0013858082805836384
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 38: Loss did not decrease. Loss: 0,001385808294885404
            Epoch 38/300, Average Loss: 0,001385808294885404
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 39/300, Average Loss: 0,0013858082806144825
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0013858082930523986
            Epoch 40/300, Average Loss: 0,0013858082930523986
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 41/300, Average Loss: 0,0013858082806879047
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 42: Loss did not decrease. Loss: 0,0013858082914928634
            Epoch 42/300, Average Loss: 0,0013858082914928634
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 43/300, Average Loss: 0,0013858082807891997
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 44: Loss did not decrease. Loss: 0,001385808290165961
            Epoch 44/300, Average Loss: 0,001385808290165961
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 45/300, Average Loss: 0,0013858082809073422
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 46: Loss did not decrease. Loss: 0,0013858082890369574
            Epoch 46/300, Average Loss: 0,0013858082890369574
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 47/300, Average Loss: 0,001385808281034173
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 48: Loss did not decrease. Loss: 0,001385808288076311
            Epoch 48/300, Average Loss: 0,001385808288076311
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 49/300, Average Loss: 0,0013858082811637532
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0013858082872588884
            Epoch 50/300, Average Loss: 0,0013858082872588884
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 51/300, Average Loss: 0,0013858082812918543
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0013858082865633185
            Epoch 52/300, Average Loss: 0,0013858082865633185
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 53/300, Average Loss: 0,001385808281415552
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 54: Loss did not decrease. Loss: 0,0013858082859714148
            Epoch 54/300, Average Loss: 0,0013858082859714148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 55/300, Average Loss: 0,0013858082815329197
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0013858082854677114
            Epoch 56/300, Average Loss: 0,0013858082854677114
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 57/300, Average Loss: 0,0013858082816427706
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 58: Loss did not decrease. Loss: 0,0013858082850390526
            Epoch 58/300, Average Loss: 0,0013858082850390526
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 59/300, Average Loss: 0,0013858082817444746
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0013858082846742437
            Epoch 60/300, Average Loss: 0,0013858082846742437
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 61/300, Average Loss: 0,0013858082818378028
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 62: Loss did not decrease. Loss: 0,001385808284363764
            Epoch 62/300, Average Loss: 0,001385808284363764
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 63/300, Average Loss: 0,0013858082819228146
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0013858082840995129
            Epoch 64/300, Average Loss: 0,0013858082840995129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 65/300, Average Loss: 0,0013858082819997715
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0013858082838746004
            Epoch 66/300, Average Loss: 0,0013858082838746004
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 67/300, Average Loss: 0,001385808282069068
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 68: Loss did not decrease. Loss: 0,0013858082836831617
            Epoch 68/300, Average Loss: 0,0013858082836831617
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 69/300, Average Loss: 0,0013858082821311803
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0013858082835202098
            Epoch 70/300, Average Loss: 0,0013858082835202098
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 71/300, Average Loss: 0,0013858082821866314
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 72: Loss did not decrease. Loss: 0,001385808283381501
            Epoch 72/300, Average Loss: 0,001385808283381501
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 73/300, Average Loss: 0,0013858082822359623
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 74: Loss did not decrease. Loss: 0,0013858082832634237
            Epoch 74/300, Average Loss: 0,0013858082832634237
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 75/300, Average Loss: 0,0013858082822797116
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 76: Loss did not decrease. Loss: 0,0013858082831629058
            Epoch 76/300, Average Loss: 0,0013858082831629058
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 77/300, Average Loss: 0,0013858082823184042
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 78: Loss did not decrease. Loss: 0,0013858082830773318
            Epoch 78/300, Average Loss: 0,0013858082830773318
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 79/300, Average Loss: 0,0013858082823525394
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 80: Loss did not decrease. Loss: 0,0013858082830044787
            Epoch 80/300, Average Loss: 0,0013858082830044787
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 81/300, Average Loss: 0,0013858082823825877
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0013858082829424515
            Epoch 82/300, Average Loss: 0,0013858082829424515
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 83/300, Average Loss: 0,0013858082824089854
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 84: Loss did not decrease. Loss: 0,0013858082828896404
            Epoch 84/300, Average Loss: 0,0013858082828896404
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 85/300, Average Loss: 0,0013858082824321331
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 86: Loss did not decrease. Loss: 0,001385808282844676
            Epoch 86/300, Average Loss: 0,001385808282844676
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 87/300, Average Loss: 0,0013858082824523962
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 88: Loss did not decrease. Loss: 0,0013858082828063882
            Epoch 88/300, Average Loss: 0,0013858082828063882
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 89/300, Average Loss: 0,0013858082824701086
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 90: Loss did not decrease. Loss: 0,001385808282773787
            Epoch 90/300, Average Loss: 0,001385808282773787
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 91/300, Average Loss: 0,0013858082824855695
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 92: Loss did not decrease. Loss: 0,0013858082827460242
            Epoch 92/300, Average Loss: 0,0013858082827460242
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 93/300, Average Loss: 0,0013858082824990473
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 94: Loss did not decrease. Loss: 0,0013858082827223816
            Epoch 94/300, Average Loss: 0,0013858082827223816
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 95/300, Average Loss: 0,0013858082825107833
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 96: Loss did not decrease. Loss: 0,0013858082827022484
            Epoch 96/300, Average Loss: 0,0013858082827022484
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 97/300, Average Loss: 0,0013858082825209906
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 98: Loss did not decrease. Loss: 0,0013858082826851018
            Epoch 98/300, Average Loss: 0,0013858082826851018
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 99/300, Average Loss: 0,0013858082825298592
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 100: Loss did not decrease. Loss: 0,001385808282670498
            Epoch 100/300, Average Loss: 0,001385808282670498
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 101/300, Average Loss: 0,0013858082825375575
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 102: Loss did not decrease. Loss: 0,0013858082826580605
            Epoch 102/300, Average Loss: 0,0013858082826580605
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 103/300, Average Loss: 0,0013858082825442353
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 104: Loss did not decrease. Loss: 0,001385808282647466
            Epoch 104/300, Average Loss: 0,001385808282647466
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 105/300, Average Loss: 0,0013858082825500214
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 106: Loss did not decrease. Loss: 0,001385808282638443
            Epoch 106/300, Average Loss: 0,001385808282638443
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 107/300, Average Loss: 0,0013858082825550322
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 108: Loss did not decrease. Loss: 0,0013858082826307572
            Epoch 108/300, Average Loss: 0,0013858082826307572
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 109/300, Average Loss: 0,0013858082825593688
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 110: Loss did not decrease. Loss: 0,00138580828262421
            Epoch 110/300, Average Loss: 0,00138580828262421
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 111/300, Average Loss: 0,0013858082825631173
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 112: Loss did not decrease. Loss: 0,0013858082826186321
            Epoch 112/300, Average Loss: 0,0013858082826186321
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 113/300, Average Loss: 0,0013858082825663582
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 114: Loss did not decrease. Loss: 0,0013858082826138814
            Epoch 114/300, Average Loss: 0,0013858082826138814
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 115/300, Average Loss: 0,0013858082825691568
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 116: Loss did not decrease. Loss: 0,001385808282609833
            Epoch 116/300, Average Loss: 0,001385808282609833
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 117/300, Average Loss: 0,0013858082825715726
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0013858082826063857
            Epoch 118/300, Average Loss: 0,0013858082826063857
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 119/300, Average Loss: 0,0013858082825736568
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 120: Loss did not decrease. Loss: 0,001385808282603448
            Epoch 120/300, Average Loss: 0,001385808282603448
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 121/300, Average Loss: 0,0013858082825754542
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 122: Loss did not decrease. Loss: 0,0013858082826009434
            Epoch 122/300, Average Loss: 0,0013858082826009434
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 123/300, Average Loss: 0,0013858082825770038
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 124: Loss did not decrease. Loss: 0,0013858082825988105
            Epoch 124/300, Average Loss: 0,0013858082825988105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 125/300, Average Loss: 0,0013858082825783378
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 126: Loss did not decrease. Loss: 0,001385808282596994
            Epoch 126/300, Average Loss: 0,001385808282596994
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 127/300, Average Loss: 0,0013858082825794875
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 128: Loss did not decrease. Loss: 0,0013858082825954456
            Epoch 128/300, Average Loss: 0,0013858082825954456
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 129/300, Average Loss: 0,001385808282580477
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 130: Loss did not decrease. Loss: 0,0013858082825941257
            Epoch 130/300, Average Loss: 0,0013858082825941257
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 131/300, Average Loss: 0,001385808282581328
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 132: Loss did not decrease. Loss: 0,001385808282593001
            Epoch 132/300, Average Loss: 0,001385808282593001
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 133/300, Average Loss: 0,001385808282582061
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 134: Loss did not decrease. Loss: 0,0013858082825920432
            Epoch 134/300, Average Loss: 0,0013858082825920432
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 135/300, Average Loss: 0,0013858082825826913
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 136: Loss did not decrease. Loss: 0,001385808282591227
            Epoch 136/300, Average Loss: 0,001385808282591227
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 137/300, Average Loss: 0,0013858082825832325
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0013858082825905311
            Epoch 138/300, Average Loss: 0,0013858082825905311
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 139/300, Average Loss: 0,001385808282583699
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 140: Loss did not decrease. Loss: 0,0013858082825899383
            Epoch 140/300, Average Loss: 0,0013858082825899383
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 141/300, Average Loss: 0,0013858082825840981
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 142: Loss did not decrease. Loss: 0,0013858082825894333
            Epoch 142/300, Average Loss: 0,0013858082825894333
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 143/300, Average Loss: 0,0013858082825844416
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 144: Loss did not decrease. Loss: 0,0013858082825890011
            Epoch 144/300, Average Loss: 0,0013858082825890011
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 145/300, Average Loss: 0,0013858082825847372
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 146: Loss did not decrease. Loss: 0,0013858082825886342
            Epoch 146/300, Average Loss: 0,0013858082825886342
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 147/300, Average Loss: 0,0013858082825849904
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 148: Loss did not decrease. Loss: 0,0013858082825883213
            Epoch 148/300, Average Loss: 0,0013858082825883213
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 149/300, Average Loss: 0,0013858082825852075
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 150: Loss did not decrease. Loss: 0,001385808282588055
            Epoch 150/300, Average Loss: 0,001385808282588055
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 151/300, Average Loss: 0,0013858082825853938
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 152: Loss did not decrease. Loss: 0,0013858082825878276
            Epoch 152/300, Average Loss: 0,0013858082825878276
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 153/300, Average Loss: 0,0013858082825855538
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 154: Loss did not decrease. Loss: 0,0013858082825876337
            Epoch 154/300, Average Loss: 0,0013858082825876337
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 155/300, Average Loss: 0,0013858082825856915
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 156: Loss did not decrease. Loss: 0,0013858082825874685
            Epoch 156/300, Average Loss: 0,0013858082825874685
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 157/300, Average Loss: 0,0013858082825858094
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 158: Loss did not decrease. Loss: 0,0013858082825873267
            Epoch 158/300, Average Loss: 0,0013858082825873267
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 159/300, Average Loss: 0,00138580828258591
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 160: Loss did not decrease. Loss: 0,0013858082825872076
            Epoch 160/300, Average Loss: 0,0013858082825872076
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 161/300, Average Loss: 0,0013858082825859966
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 162: Loss did not decrease. Loss: 0,001385808282587105
            Epoch 162/300, Average Loss: 0,001385808282587105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 163/300, Average Loss: 0,0013858082825860703
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 164: Loss did not decrease. Loss: 0,0013858082825870175
            Epoch 164/300, Average Loss: 0,0013858082825870175
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 165/300, Average Loss: 0,0013858082825861345
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 166: Loss did not decrease. Loss: 0,0013858082825869433
            Epoch 166/300, Average Loss: 0,0013858082825869433
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 167/300, Average Loss: 0,0013858082825861885
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 168: Loss did not decrease. Loss: 0,00138580828258688
            Epoch 168/300, Average Loss: 0,00138580828258688
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 169/300, Average Loss: 0,0013858082825862353
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 170: Loss did not decrease. Loss: 0,001385808282586826
            Epoch 170/300, Average Loss: 0,001385808282586826
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 171/300, Average Loss: 0,001385808282586275
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 172: Loss did not decrease. Loss: 0,0013858082825867802
            Epoch 172/300, Average Loss: 0,0013858082825867802
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 173/300, Average Loss: 0,0013858082825863097
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 174: Loss did not decrease. Loss: 0,00138580828258674
            Epoch 174/300, Average Loss: 0,00138580828258674
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 175/300, Average Loss: 0,0013858082825863388
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 176: Loss did not decrease. Loss: 0,0013858082825867065
            Epoch 176/300, Average Loss: 0,0013858082825867065
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 177/300, Average Loss: 0,0013858082825863643
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 178: Loss did not decrease. Loss: 0,0013858082825866794
            Epoch 178/300, Average Loss: 0,0013858082825866794
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 179/300, Average Loss: 0,0013858082825863865
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 180: Loss did not decrease. Loss: 0,0013858082825866543
            Epoch 180/300, Average Loss: 0,0013858082825866543
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 181/300, Average Loss: 0,0013858082825864042
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 182: Loss did not decrease. Loss: 0,0013858082825866334
            Epoch 182/300, Average Loss: 0,0013858082825866334
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 183/300, Average Loss: 0,0013858082825864194
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 184: Loss did not decrease. Loss: 0,0013858082825866154
            Epoch 184/300, Average Loss: 0,0013858082825866154
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 185/300, Average Loss: 0,0013858082825864329
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 186: Loss did not decrease. Loss: 0,0013858082825866005
            Epoch 186/300, Average Loss: 0,0013858082825866005
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 187/300, Average Loss: 0,0013858082825864446
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 188: Loss did not decrease. Loss: 0,0013858082825865877
            Epoch 188/300, Average Loss: 0,0013858082825865877
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 189/300, Average Loss: 0,0013858082825864541
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 190: Loss did not decrease. Loss: 0,0013858082825865773
            Epoch 190/300, Average Loss: 0,0013858082825865773
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 191/300, Average Loss: 0,0013858082825864628
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 192: Loss did not decrease. Loss: 0,0013858082825865673
            Epoch 192/300, Average Loss: 0,0013858082825865673
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 193/300, Average Loss: 0,0013858082825864704
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 194: Loss did not decrease. Loss: 0,001385808282586559
            Epoch 194/300, Average Loss: 0,001385808282586559
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 195/300, Average Loss: 0,0013858082825864767
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 196: Loss did not decrease. Loss: 0,0013858082825865526
            Epoch 196/300, Average Loss: 0,0013858082825865526
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 197/300, Average Loss: 0,0013858082825864814
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 198: Loss did not decrease. Loss: 0,0013858082825865465
            Epoch 198/300, Average Loss: 0,0013858082825865465
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 199/300, Average Loss: 0,0013858082825864862
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 200: Loss did not decrease. Loss: 0,0013858082825865413
            Epoch 200/300, Average Loss: 0,0013858082825865413
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 201/300, Average Loss: 0,0013858082825864895
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 202: Loss did not decrease. Loss: 0,0013858082825865365
            Epoch 202/300, Average Loss: 0,0013858082825865365
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 203/300, Average Loss: 0,0013858082825864936
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 204: Loss did not decrease. Loss: 0,0013858082825865337
            Epoch 204/300, Average Loss: 0,0013858082825865337
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 205/300, Average Loss: 0,0013858082825864964
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 206: Loss did not decrease. Loss: 0,0013858082825865309
            Epoch 206/300, Average Loss: 0,0013858082825865309
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 207/300, Average Loss: 0,0013858082825864988
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 208: Loss did not decrease. Loss: 0,0013858082825865283
            Epoch 208/300, Average Loss: 0,0013858082825865283
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 209/300, Average Loss: 0,001385808282586501
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 210: Loss did not decrease. Loss: 0,0013858082825865259
            Epoch 210/300, Average Loss: 0,0013858082825865259
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 211/300, Average Loss: 0,0013858082825865027
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 212: Loss did not decrease. Loss: 0,0013858082825865242
            Epoch 212/300, Average Loss: 0,0013858082825865242
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 213/300, Average Loss: 0,0013858082825865044
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 214: Loss did not decrease. Loss: 0,0013858082825865224
            Epoch 214/300, Average Loss: 0,0013858082825865224
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 215/300, Average Loss: 0,0013858082825865057
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 216: Loss did not decrease. Loss: 0,0013858082825865213
            Epoch 216/300, Average Loss: 0,0013858082825865213
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 217/300, Average Loss: 0,0013858082825865062
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 218: Loss did not decrease. Loss: 0,001385808282586519
            Epoch 218/300, Average Loss: 0,001385808282586519
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 219/300, Average Loss: 0,0013858082825865075
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 220: Loss did not decrease. Loss: 0,001385808282586518
            Epoch 220/300, Average Loss: 0,001385808282586518
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 221/300, Average Loss: 0,0013858082825865085
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 222: Loss did not decrease. Loss: 0,0013858082825865179
            Epoch 222/300, Average Loss: 0,0013858082825865179
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 223/300, Average Loss: 0,0013858082825865092
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 224: Loss did not decrease. Loss: 0,0013858082825865172
            Epoch 224/300, Average Loss: 0,0013858082825865172
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 225/300, Average Loss: 0,0013858082825865096
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 226: Loss did not decrease. Loss: 0,0013858082825865166
            Epoch 226/300, Average Loss: 0,0013858082825865166
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 227/300, Average Loss: 0,0013858082825865105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 228: Loss did not decrease. Loss: 0,0013858082825865161
            Epoch 228/300, Average Loss: 0,0013858082825865161
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 229/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 230: Loss did not decrease. Loss: 0,0013858082825865155
            Epoch 230/300, Average Loss: 0,0013858082825865155
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 231/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 232: Loss did not decrease. Loss: 0,0013858082825865153
            Epoch 232/300, Average Loss: 0,0013858082825865153
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 233/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 234: Loss did not decrease. Loss: 0,0013858082825865155
            Epoch 234/300, Average Loss: 0,0013858082825865155
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 235/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 236: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 236/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 237/300, Average Loss: 0,0013858082825865114
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 238: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 238/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 239/300, Average Loss: 0,0013858082825865111
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 240: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 240/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 241/300, Average Loss: 0,0013858082825865118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 242: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 242/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 243/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 244: Loss did not decrease. Loss: 0,001385808282586514
            Epoch 244/300, Average Loss: 0,001385808282586514
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 245/300, Average Loss: 0,0013858082825865118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 246: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 246/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 247/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 248: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 248/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 249/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 250: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 250/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 251/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 252: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 252/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 253/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 254: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 254/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 255/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 256: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 256/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 257/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 258: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 258/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 259/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 260: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 260/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 261/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 262: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 262/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 263/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 264/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 265/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 266: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 266/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 267/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 268: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 268/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 269/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 270: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 270/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 271/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 272: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 272/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 273/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 274: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 274/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 275/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 276: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 276/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 277/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 278: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 278/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 279/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 280: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 280/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 281/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 282: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 282/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 283/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 284: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 284/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 285/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 286: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 286/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 287/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 288: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 288/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 289/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 290: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 290/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 291/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 292: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 292/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 293/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 294: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 294/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 295/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 296: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 296/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 297/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 298: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 298/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 299/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 300: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 300/300, Average Loss: 0,001385808282586513
            Model successfully decreased loss, indicating learning over epochs. 
            */


            /*
            Input: atom, Target: molecule, Predicted: supernumerary
            Input: king, Target: queen, Predicted: brobeck
            Input: electron, Target: proton, Predicted: polyethylene
            Epoch 1/300, Average Loss: 0,16028843325991357
            Input: atom, Target: molecule, Predicted: supernumerary
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 2/300, Average Loss: 0,01059711588701151
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 3/300, Average Loss: 0,001489000253534273
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 4/300, Average Loss: 0,0013842328944202638
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 5: Loss did not decrease. Loss: 0,0013856049759954725
            Epoch 5/300, Average Loss: 0,0013856049759954725
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 6: Loss did not decrease. Loss: 0,0013858171480032567
            Epoch 6/300, Average Loss: 0,0013858171480032567
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 7/300, Average Loss: 0,0013858102455102106
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 8/300, Average Loss: 0,0013858082757293114
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 9/300, Average Loss: 0,0013858082659214388
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 10: Loss did not decrease. Loss: 0,0013858082830601043
            Epoch 10/300, Average Loss: 0,0013858082830601043
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 11/300, Average Loss: 0,0013858082819812679
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 12: Loss did not decrease. Loss: 0,0013858082833675626
            Epoch 12/300, Average Loss: 0,0013858082833675626
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 13/300, Average Loss: 0,0013858082818846908
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0013858082832370533
            Epoch 14/300, Average Loss: 0,0013858082832370533
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 15/300, Average Loss: 0,0013858082819220288
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 16: Loss did not decrease. Loss: 0,001385808283133668
            Epoch 16/300, Average Loss: 0,001385808283133668
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 17/300, Average Loss: 0,0013858082819698128
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0013858082830467602
            Epoch 18/300, Average Loss: 0,0013858082830467602
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 19/300, Average Loss: 0,001385808282023101
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 20: Loss did not decrease. Loss: 0,0013858082829737344
            Epoch 20/300, Average Loss: 0,0013858082829737344
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 21/300, Average Loss: 0,0013858082820780333
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0013858082829123555
            Epoch 22/300, Average Loss: 0,0013858082829123555
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 23/300, Average Loss: 0,0013858082821320507
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0013858082828607516
            Epoch 24/300, Average Loss: 0,0013858082828607516
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 25/300, Average Loss: 0,001385808282183523
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0013858082828173527
            Epoch 26/300, Average Loss: 0,0013858082828173527
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 27/300, Average Loss: 0,001385808282231487
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 28: Loss did not decrease. Loss: 0,001385808282780848
            Epoch 28/300, Average Loss: 0,001385808282780848
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 29/300, Average Loss: 0,0013858082822754405
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0013858082827501361
            Epoch 30/300, Average Loss: 0,0013858082827501361
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 31/300, Average Loss: 0,001385808282315204
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0013858082827242916
            Epoch 32/300, Average Loss: 0,0013858082827242916
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 33/300, Average Loss: 0,0013858082823508127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0013858082827025405
            Epoch 34/300, Average Loss: 0,0013858082827025405
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 35/300, Average Loss: 0,0013858082823824383
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0013858082826842318
            Epoch 36/300, Average Loss: 0,0013858082826842318
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 37/300, Average Loss: 0,001385808282410337
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0013858082826688177
            Epoch 38/300, Average Loss: 0,0013858082826688177
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 39/300, Average Loss: 0,0013858082824348087
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0013858082826558402
            Epoch 40/300, Average Loss: 0,0013858082826558402
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 41/300, Average Loss: 0,0013858082824561736
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 42: Loss did not decrease. Loss: 0,0013858082826449121
            Epoch 42/300, Average Loss: 0,0013858082826449121
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 43/300, Average Loss: 0,0013858082824747488
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0013858082826357085
            Epoch 44/300, Average Loss: 0,0013858082826357085
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 45/300, Average Loss: 0,0013858082824908437
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 46: Loss did not decrease. Loss: 0,001385808282627958
            Epoch 46/300, Average Loss: 0,001385808282627958
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 47/300, Average Loss: 0,0013858082825047452
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0013858082826214303
            Epoch 48/300, Average Loss: 0,0013858082826214303
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 49/300, Average Loss: 0,0013858082825167243
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0013858082826159312
            Epoch 50/300, Average Loss: 0,0013858082826159312
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 51/300, Average Loss: 0,0013858082825270205
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0013858082826112995
            Epoch 52/300, Average Loss: 0,0013858082826112995
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 53/300, Average Loss: 0,0013858082825358527
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 54: Loss did not decrease. Loss: 0,0013858082826073966
            Epoch 54/300, Average Loss: 0,0013858082826073966
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 55/300, Average Loss: 0,0013858082825434156
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0013858082826041084
            Epoch 56/300, Average Loss: 0,0013858082826041084
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 57/300, Average Loss: 0,0013858082825498822
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 58: Loss did not decrease. Loss: 0,0013858082826013387
            Epoch 58/300, Average Loss: 0,0013858082826013387
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 59/300, Average Loss: 0,001385808282555403
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0013858082825990055
            Epoch 60/300, Average Loss: 0,0013858082825990055
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 61/300, Average Loss: 0,0013858082825601106
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 62: Loss did not decrease. Loss: 0,00138580828259704
            Epoch 62/300, Average Loss: 0,00138580828259704
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 63/300, Average Loss: 0,0013858082825641208
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0013858082825953823
            Epoch 64/300, Average Loss: 0,0013858082825953823
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 65/300, Average Loss: 0,0013858082825675324
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 66: Loss did not decrease. Loss: 0,001385808282593987
            Epoch 66/300, Average Loss: 0,001385808282593987
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 67/300, Average Loss: 0,0013858082825704337
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 68: Loss did not decrease. Loss: 0,001385808282592811
            Epoch 68/300, Average Loss: 0,001385808282592811
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 69/300, Average Loss: 0,0013858082825728962
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0013858082825918196
            Epoch 70/300, Average Loss: 0,0013858082825918196
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 71/300, Average Loss: 0,0013858082825749882
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 72: Loss did not decrease. Loss: 0,0013858082825909852
            Epoch 72/300, Average Loss: 0,0013858082825909852
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 73/300, Average Loss: 0,001385808282576762
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 74: Loss did not decrease. Loss: 0,0013858082825902813
            Epoch 74/300, Average Loss: 0,0013858082825902813
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 75/300, Average Loss: 0,001385808282578266
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 76: Loss did not decrease. Loss: 0,001385808282589688
            Epoch 76/300, Average Loss: 0,001385808282589688
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 77/300, Average Loss: 0,00138580828257954
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 78: Loss did not decrease. Loss: 0,0013858082825891887
            Epoch 78/300, Average Loss: 0,0013858082825891887
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 79/300, Average Loss: 0,0013858082825806198
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 80: Loss did not decrease. Loss: 0,0013858082825887671
            Epoch 80/300, Average Loss: 0,0013858082825887671
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 81/300, Average Loss: 0,0013858082825815327
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0013858082825884126
            Epoch 82/300, Average Loss: 0,0013858082825884126
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 83/300, Average Loss: 0,001385808282582306
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 84: Loss did not decrease. Loss: 0,0013858082825881134
            Epoch 84/300, Average Loss: 0,0013858082825881134
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 85/300, Average Loss: 0,00138580828258296
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 86: Loss did not decrease. Loss: 0,0013858082825878623
            Epoch 86/300, Average Loss: 0,0013858082825878623
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 87/300, Average Loss: 0,0013858082825835118
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 88: Loss did not decrease. Loss: 0,00138580828258765
            Epoch 88/300, Average Loss: 0,00138580828258765
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 89/300, Average Loss: 0,0013858082825839804
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 90: Loss did not decrease. Loss: 0,001385808282587471
            Epoch 90/300, Average Loss: 0,001385808282587471
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 91/300, Average Loss: 0,001385808282584375
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 92: Loss did not decrease. Loss: 0,0013858082825873202
            Epoch 92/300, Average Loss: 0,0013858082825873202
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 93/300, Average Loss: 0,0013858082825847088
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 94: Loss did not decrease. Loss: 0,001385808282587193
            Epoch 94/300, Average Loss: 0,001385808282587193
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 95/300, Average Loss: 0,00138580828258499
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 96: Loss did not decrease. Loss: 0,001385808282587086
            Epoch 96/300, Average Loss: 0,001385808282587086
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 97/300, Average Loss: 0,0013858082825852283
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 98: Loss did not decrease. Loss: 0,0013858082825869958
            Epoch 98/300, Average Loss: 0,0013858082825869958
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 99/300, Average Loss: 0,0013858082825854287
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 100: Loss did not decrease. Loss: 0,0013858082825869197
            Epoch 100/300, Average Loss: 0,0013858082825869197
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 101/300, Average Loss: 0,001385808282585598
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 102: Loss did not decrease. Loss: 0,0013858082825868566
            Epoch 102/300, Average Loss: 0,0013858082825868566
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 103/300, Average Loss: 0,0013858082825857427
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 104: Loss did not decrease. Loss: 0,0013858082825868024
            Epoch 104/300, Average Loss: 0,0013858082825868024
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 105/300, Average Loss: 0,0013858082825858626
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 106: Loss did not decrease. Loss: 0,0013858082825867564
            Epoch 106/300, Average Loss: 0,0013858082825867564
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 107/300, Average Loss: 0,0013858082825859647
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 108: Loss did not decrease. Loss: 0,0013858082825867182
            Epoch 108/300, Average Loss: 0,0013858082825867182
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 109/300, Average Loss: 0,0013858082825860512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 110: Loss did not decrease. Loss: 0,0013858082825866853
            Epoch 110/300, Average Loss: 0,0013858082825866853
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 111/300, Average Loss: 0,0013858082825861228
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 112: Loss did not decrease. Loss: 0,0013858082825866586
            Epoch 112/300, Average Loss: 0,0013858082825866586
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 113/300, Average Loss: 0,0013858082825861848
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 114: Loss did not decrease. Loss: 0,0013858082825866358
            Epoch 114/300, Average Loss: 0,0013858082825866358
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 115/300, Average Loss: 0,0013858082825862362
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 116: Loss did not decrease. Loss: 0,0013858082825866159
            Epoch 116/300, Average Loss: 0,0013858082825866159
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 117/300, Average Loss: 0,0013858082825862802
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0013858082825866005
            Epoch 118/300, Average Loss: 0,0013858082825866005
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 119/300, Average Loss: 0,001385808282586316
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 120: Loss did not decrease. Loss: 0,0013858082825865864
            Epoch 120/300, Average Loss: 0,0013858082825865864
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 121/300, Average Loss: 0,0013858082825863472
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 122: Loss did not decrease. Loss: 0,0013858082825865747
            Epoch 122/300, Average Loss: 0,0013858082825865747
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 123/300, Average Loss: 0,001385808282586373
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 124: Loss did not decrease. Loss: 0,0013858082825865643
            Epoch 124/300, Average Loss: 0,0013858082825865643
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 125/300, Average Loss: 0,0013858082825863951
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 126: Loss did not decrease. Loss: 0,0013858082825865565
            Epoch 126/300, Average Loss: 0,0013858082825865565
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 127/300, Average Loss: 0,0013858082825864142
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 128: Loss did not decrease. Loss: 0,0013858082825865508
            Epoch 128/300, Average Loss: 0,0013858082825865508
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 129/300, Average Loss: 0,001385808282586429
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 130: Loss did not decrease. Loss: 0,0013858082825865439
            Epoch 130/300, Average Loss: 0,0013858082825865439
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 131/300, Average Loss: 0,001385808282586442
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 132: Loss did not decrease. Loss: 0,0013858082825865398
            Epoch 132/300, Average Loss: 0,0013858082825865398
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 133/300, Average Loss: 0,001385808282586454
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 134: Loss did not decrease. Loss: 0,0013858082825865352
            Epoch 134/300, Average Loss: 0,0013858082825865352
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 135/300, Average Loss: 0,0013858082825864628
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 136: Loss did not decrease. Loss: 0,001385808282586532
            Epoch 136/300, Average Loss: 0,001385808282586532
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 137/300, Average Loss: 0,001385808282586471
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0013858082825865287
            Epoch 138/300, Average Loss: 0,0013858082825865287
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 139/300, Average Loss: 0,0013858082825864775
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 140: Loss did not decrease. Loss: 0,0013858082825865252
            Epoch 140/300, Average Loss: 0,0013858082825865252
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 141/300, Average Loss: 0,0013858082825864832
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 142: Loss did not decrease. Loss: 0,001385808282586525
            Epoch 142/300, Average Loss: 0,001385808282586525
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 143/300, Average Loss: 0,001385808282586487
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 144: Loss did not decrease. Loss: 0,001385808282586523
            Epoch 144/300, Average Loss: 0,001385808282586523
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 145/300, Average Loss: 0,0013858082825864912
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 146: Loss did not decrease. Loss: 0,0013858082825865213
            Epoch 146/300, Average Loss: 0,0013858082825865213
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 147/300, Average Loss: 0,0013858082825864947
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 148: Loss did not decrease. Loss: 0,0013858082825865196
            Epoch 148/300, Average Loss: 0,0013858082825865196
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 149/300, Average Loss: 0,0013858082825864981
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 150: Loss did not decrease. Loss: 0,0013858082825865183
            Epoch 150/300, Average Loss: 0,0013858082825865183
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 151/300, Average Loss: 0,0013858082825864992
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 152: Loss did not decrease. Loss: 0,0013858082825865179
            Epoch 152/300, Average Loss: 0,0013858082825865179
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 153/300, Average Loss: 0,0013858082825865023
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 154: Loss did not decrease. Loss: 0,001385808282586517
            Epoch 154/300, Average Loss: 0,001385808282586517
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 155/300, Average Loss: 0,0013858082825865033
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 156: Loss did not decrease. Loss: 0,0013858082825865166
            Epoch 156/300, Average Loss: 0,0013858082825865166
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 157/300, Average Loss: 0,001385808282586505
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 158: Loss did not decrease. Loss: 0,0013858082825865157
            Epoch 158/300, Average Loss: 0,0013858082825865157
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 159/300, Average Loss: 0,001385808282586507
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 160: Loss did not decrease. Loss: 0,0013858082825865155
            Epoch 160/300, Average Loss: 0,0013858082825865155
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 161/300, Average Loss: 0,0013858082825865075
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 162: Loss did not decrease. Loss: 0,0013858082825865148
            Epoch 162/300, Average Loss: 0,0013858082825865148
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 163/300, Average Loss: 0,0013858082825865083
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 164: Loss did not decrease. Loss: 0,0013858082825865153
            Epoch 164/300, Average Loss: 0,0013858082825865153
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 165/300, Average Loss: 0,0013858082825865092
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 166: Loss did not decrease. Loss: 0,0013858082825865146
            Epoch 166/300, Average Loss: 0,0013858082825865146
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 167/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 168: Loss did not decrease. Loss: 0,0013858082825865144
            Epoch 168/300, Average Loss: 0,0013858082825865144
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 169/300, Average Loss: 0,0013858082825865103
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 170: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 170/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 171/300, Average Loss: 0,0013858082825865105
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 172: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 172/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 173/300, Average Loss: 0,001385808282586511
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 174: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 174/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 175/300, Average Loss: 0,0013858082825865114
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 176: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 176/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 177/300, Average Loss: 0,0013858082825865114
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 178: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 178/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 179/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 180: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 180/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 181/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 182: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 182/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 183/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 184: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 184/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 185/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 186: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 186/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 187/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 188: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 188/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 189/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 190: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 190/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 191/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 192: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 192/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 193/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 194: Loss did not decrease. Loss: 0,0013858082825865137
            Epoch 194/300, Average Loss: 0,0013858082825865137
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 195/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 196: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 196/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 197: Loss did not decrease. Loss: 0,0013858082825865135
            Epoch 197/300, Average Loss: 0,0013858082825865135
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 198/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 199: Loss did not decrease. Loss: 0,001385808282586513
            Epoch 199/300, Average Loss: 0,001385808282586513
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 200/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 201: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 201/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 202/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 203/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 204/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 205/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 206: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 206/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 207/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 208/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 209/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 210/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 211/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 212/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 213/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 214: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 214/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 215/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 216/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 217/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 218/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 219/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 220/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 221: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 221/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 222/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 223/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 224: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 224/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 225/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 226/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 227: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 227/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 228/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 229/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 230/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 231/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 232/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 233/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 234: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 234/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 235/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 236/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 237/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 238: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 238/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 239/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 240/300, Average Loss: 0,001385808282586512
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 241: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 241/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 242/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 243/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 244/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 245: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 245/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 246/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 247/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 248/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 249/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 250: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 250/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 251/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 252/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 253/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 254: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 254/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 255/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 256/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 257: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 257/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 258/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 259/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 260: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 260/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 261/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 262/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 263/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 264/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 265/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 266/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 267/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 268/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 269: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 269/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 270/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 271/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 272: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 272/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 273/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 274/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 275/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 276: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 276/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 277/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 278: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 278/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 279/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 280/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 281/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 282: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 282/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 283/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 284: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 284/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 285/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 286/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 287/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 288/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 289: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 289/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 290: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 290/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 291/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 292/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 293/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 294: Loss did not decrease. Loss: 0,0013858082825865129
            Epoch 294/300, Average Loss: 0,0013858082825865129
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 295/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 296/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Warning: Epoch 297: Loss did not decrease. Loss: 0,0013858082825865127
            Epoch 297/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 298/300, Average Loss: 0,0013858082825865127
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 299/300, Average Loss: 0,0013858082825865122
            Input: atom, Target: molecule, Predicted: queen
            Input: king, Target: queen, Predicted: proton
            Input: electron, Target: proton, Predicted: molecule
            Epoch 300/300, Average Loss: 0,0013858082825865122
            Model successfully decreased loss, indicating learning over epochs. 
            */

            Console.Read();
        }
    }
}
