using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe14Sep2024_300d
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
            string[] inputWords = { "atom", "king", "electron", "salsa", "genetics" };
            string[] targetWords = { "molecule", "queen", "proton", "samba", "evolution" };

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
            Input: atom, Target: molecule, Predicted: kprc-tv
            Input: king, Target: queen, Predicted: sobibor
            Input: electron, Target: proton, Predicted: 12-story
            Input: salsa, Target: samba, Predicted: atatürk
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 1/300, Average Loss: 0,09412725124086754
            Input: atom, Target: molecule, Predicted: samba
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 2/300, Average Loss: 0,003910210838532687
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 3/300, Average Loss: 0,002800069346494479
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 4: Loss did not decrease. Loss: 0,002800999576872276
            Epoch 4/300, Average Loss: 0,002800999576872276
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 5/300, Average Loss: 0,0028009880088846863
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 6/300, Average Loss: 0,0028009879926091034
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 7: Loss did not decrease. Loss: 0,0028009881598838435
            Epoch 7/300, Average Loss: 0,0028009881598838435
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 8: Loss did not decrease. Loss: 0,002800988214516102
            Epoch 8/300, Average Loss: 0,002800988214516102
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 9: Loss did not decrease. Loss: 0,0028009882762069565
            Epoch 9/300, Average Loss: 0,0028009882762069565
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 10: Loss did not decrease. Loss: 0,0028009883195565556
            Epoch 10/300, Average Loss: 0,0028009883195565556
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 11: Loss did not decrease. Loss: 0,002800988341139053
            Epoch 11/300, Average Loss: 0,002800988341139053
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 12: Loss did not decrease. Loss: 0,0028009883704291664
            Epoch 12/300, Average Loss: 0,0028009883704291664
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 13: Loss did not decrease. Loss: 0,002800988376780978
            Epoch 13/300, Average Loss: 0,002800988376780978
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0028009883950579255
            Epoch 14/300, Average Loss: 0,0028009883950579255
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 15: Loss did not decrease. Loss: 0,0028009883961042396
            Epoch 15/300, Average Loss: 0,0028009883961042396
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0028009884069761393
            Epoch 16/300, Average Loss: 0,0028009884069761393
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 17/300, Average Loss: 0,0028009884064768287
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 18: Loss did not decrease. Loss: 0,002800988412740761
            Epoch 18/300, Average Loss: 0,002800988412740761
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 19/300, Average Loss: 0,002800988411999742
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 20: Loss did not decrease. Loss: 0,002800988415527522
            Epoch 20/300, Average Loss: 0,002800988415527522
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 21/300, Average Loss: 0,0028009884149206563
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0028009884168739277
            Epoch 22/300, Average Loss: 0,0028009884168739277
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 23/300, Average Loss: 0,0028009884164566894
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 24: Loss did not decrease. Loss: 0,002800988417524017
            Epoch 24/300, Average Loss: 0,002800988417524017
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 25/300, Average Loss: 0,0028009884172605463
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0028009884178376823
            Epoch 26/300, Average Loss: 0,0028009884178376823
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 27/300, Average Loss: 0,002800988417679482
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0028009884179889064
            Epoch 28/300, Average Loss: 0,0028009884179889064
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 29/300, Average Loss: 0,002800988417897024
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 30: Loss did not decrease. Loss: 0,002800988418061754
            Epoch 30/300, Average Loss: 0,002800988418061754
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 31/300, Average Loss: 0,002800988418009632
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 32: Loss did not decrease. Loss: 0,002800988418096814
            Epoch 32/300, Average Loss: 0,002800988418096814
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 33/300, Average Loss: 0,0028009884180677596
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0028009884181136703
            Epoch 34/300, Average Loss: 0,0028009884181136703
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 35/300, Average Loss: 0,0028009884180976935
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 36: Loss did not decrease. Loss: 0,002800988418121766
            Epoch 36/300, Average Loss: 0,002800988418121766
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 37/300, Average Loss: 0,002800988418113075
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0028009884181256486
            Epoch 38/300, Average Loss: 0,0028009884181256486
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 39/300, Average Loss: 0,002800988418120962
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0028009884181275082
            Epoch 40/300, Average Loss: 0,0028009884181275082
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 41/300, Average Loss: 0,0028009884181249994
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 42: Loss did not decrease. Loss: 0,002800988418128398
            Epoch 42/300, Average Loss: 0,002800988418128398
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 43/300, Average Loss: 0,002800988418127061
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 44: Loss did not decrease. Loss: 0,002800988418128822
            Epoch 44/300, Average Loss: 0,002800988418128822
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 45/300, Average Loss: 0,002800988418128116
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 46: Loss did not decrease. Loss: 0,002800988418129025
            Epoch 46/300, Average Loss: 0,002800988418129025
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 47/300, Average Loss: 0,0028009884181286527
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0028009884181291215
            Epoch 48/300, Average Loss: 0,0028009884181291215
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 49/300, Average Loss: 0,002800988418128926
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 50: Loss did not decrease. Loss: 0,002800988418129169
            Epoch 50/300, Average Loss: 0,002800988418129169
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 51/300, Average Loss: 0,0028009884181290656
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0028009884181291905
            Epoch 52/300, Average Loss: 0,0028009884181291905
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 53/300, Average Loss: 0,0028009884181291354
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 54: Loss did not decrease. Loss: 0,0028009884181291996
            Epoch 54/300, Average Loss: 0,0028009884181291996
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 55/300, Average Loss: 0,002800988418129171
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0028009884181292044
            Epoch 56/300, Average Loss: 0,0028009884181292044
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 57/300, Average Loss: 0,002800988418129191
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 58: Loss did not decrease. Loss: 0,002800988418129206
            Epoch 58/300, Average Loss: 0,002800988418129206
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 59/300, Average Loss: 0,002800988418129199
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 60: Loss did not decrease. Loss: 0,002800988418129207
            Epoch 60/300, Average Loss: 0,002800988418129207
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 61/300, Average Loss: 0,002800988418129203
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 62: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 62/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 63/300, Average Loss: 0,0028009884181292052
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0028009884181292096
            Epoch 64/300, Average Loss: 0,0028009884181292096
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 65/300, Average Loss: 0,0028009884181292057
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0028009884181292096
            Epoch 66/300, Average Loss: 0,0028009884181292096
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 67/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 68: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 68/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 69/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 70/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 71: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 71/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 72/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 73/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 74/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 75: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 75/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 76: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 76/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 77/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 78: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 78/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 79/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 80: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 80/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 81/300, Average Loss: 0,0028009884181292074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 82/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 83/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 84: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 84/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 85/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 86/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 87/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 88/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 89/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 90: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 90/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 91/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 92: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 92/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 93/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 94: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 94/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 95/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 96/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 97/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 98: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 98/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 99/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 100/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 101/300, Average Loss: 0,0028009884181292074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 102: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 102/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 103/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 104: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 104/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 105/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 106: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 106/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 107/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 108: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 108/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 109/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 110: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 110/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 111/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 112: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 112/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 113/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 114: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 114/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 115/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 116: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 116/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 117/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 118/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 119/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 120: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 120/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 121/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 122/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 123/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 124: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 124/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 125/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 126: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 126/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 127/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 128: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 128/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 129/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 130: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 130/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 131/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 132: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 132/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 133/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 134/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 135/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 136/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 137/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 138/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 139/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 140/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 141/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 142: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 142/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 143/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 144: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 144/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 145/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 146: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 146/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 147/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 148/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 149/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 150: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 150/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 151/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 152/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 153/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 154: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 154/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 155/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 156: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 156/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 157/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 158: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 158/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 159/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 160: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 160/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 161/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 162: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 162/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 163/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 164: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 164/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 165/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 166: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 166/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 167/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 168: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 168/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 169/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 170: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 170/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 171/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 172: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 172/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 173/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 174: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 174/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 175/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 176: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 176/300, Average Loss: 0,0028009884181292087
            */


            /*
            Input: atom, Target: molecule, Predicted: 5.56
            Input: king, Target: queen, Predicted: indians
            Input: electron, Target: proton, Predicted: vlast
            Input: salsa, Target: samba, Predicted: ebenezer
            Input: genetics, Target: evolution, Predicted: pastor
            Epoch 1/300, Average Loss: 0,08523194029895077
            Input: atom, Target: molecule, Predicted: thurles
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 2/300, Average Loss: 0,0043609432471079235
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 3/300, Average Loss: 0,0027995343866896315
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 4: Loss did not decrease. Loss: 0,0028009960780452253
            Epoch 4/300, Average Loss: 0,0028009960780452253
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 5/300, Average Loss: 0,0028009887338063663
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 6/300, Average Loss: 0,002800988406185503
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 7: Loss did not decrease. Loss: 0,002800988413211212
            Epoch 7/300, Average Loss: 0,002800988413211212
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 8: Loss did not decrease. Loss: 0,0028009884173321826
            Epoch 8/300, Average Loss: 0,0028009884173321826
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 9/300, Average Loss: 0,0028009884154078456
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 10: Loss did not decrease. Loss: 0,002800988417731481
            Epoch 10/300, Average Loss: 0,002800988417731481
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 11/300, Average Loss: 0,0028009884166819467
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 12: Loss did not decrease. Loss: 0,0028009884179296396
            Epoch 12/300, Average Loss: 0,0028009884179296396
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 13/300, Average Loss: 0,002800988417365813
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 14: Loss did not decrease. Loss: 0,002800988418028588
            Epoch 14/300, Average Loss: 0,002800988418028588
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 15/300, Average Loss: 0,002800988417729252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0028009884180782508
            Epoch 16/300, Average Loss: 0,0028009884180782508
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 17/300, Average Loss: 0,002800988417920846
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0028009884181032967
            Epoch 18/300, Average Loss: 0,0028009884181032967
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 19/300, Average Loss: 0,0028009884180211753
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 20: Loss did not decrease. Loss: 0,002800988418115985
            Epoch 20/300, Average Loss: 0,002800988418115985
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 21/300, Average Loss: 0,0028009884180734213
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0028009884181224394
            Epoch 22/300, Average Loss: 0,0028009884181224394
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 23/300, Average Loss: 0,0028009884181005016
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 24: Loss did not decrease. Loss: 0,002800988418125732
            Epoch 24/300, Average Loss: 0,002800988418125732
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 25/300, Average Loss: 0,0028009884181144805
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0028009884181274198
            Epoch 26/300, Average Loss: 0,0028009884181274198
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 27/300, Average Loss: 0,002800988418121671
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0028009884181282854
            Epoch 28/300, Average Loss: 0,0028009884181282854
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 29/300, Average Loss: 0,002800988418125361
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0028009884181287325
            Epoch 30/300, Average Loss: 0,0028009884181287325
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 31/300, Average Loss: 0,0028009884181272476
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 32: Loss did not decrease. Loss: 0,002800988418128963
            Epoch 32/300, Average Loss: 0,002800988418128963
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 33/300, Average Loss: 0,002800988418128212
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 34: Loss did not decrease. Loss: 0,002800988418129081
            Epoch 34/300, Average Loss: 0,002800988418129081
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 35/300, Average Loss: 0,0028009884181287026
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 36: Loss did not decrease. Loss: 0,002800988418129142
            Epoch 36/300, Average Loss: 0,002800988418129142
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 37/300, Average Loss: 0,0028009884181289515
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0028009884181291736
            Epoch 38/300, Average Loss: 0,0028009884181291736
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 39/300, Average Loss: 0,002800988418129078
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 40: Loss did not decrease. Loss: 0,002800988418129191
            Epoch 40/300, Average Loss: 0,002800988418129191
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 41/300, Average Loss: 0,0028009884181291406
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 42: Loss did not decrease. Loss: 0,002800988418129199
            Epoch 42/300, Average Loss: 0,002800988418129199
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 43/300, Average Loss: 0,002800988418129175
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0028009884181292044
            Epoch 44/300, Average Loss: 0,0028009884181292044
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 45/300, Average Loss: 0,0028009884181291918
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 46: Loss did not decrease. Loss: 0,0028009884181292074
            Epoch 46/300, Average Loss: 0,0028009884181292074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 47/300, Average Loss: 0,0028009884181291996
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 48/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 49/300, Average Loss: 0,0028009884181292035
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 50: Loss did not decrease. Loss: 0,002800988418129208
            Epoch 50/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 51/300, Average Loss: 0,0028009884181292065
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 52: Loss did not decrease. Loss: 0,002800988418129208
            Epoch 52/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 53/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 54: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 54/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 55/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 56/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 57: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 57/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 58/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 59/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 60/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 61: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 61/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 62/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 63/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 64/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 65: Loss did not decrease. Loss: 0,0028009884181292096
            Epoch 65/300, Average Loss: 0,0028009884181292096
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 66/300, Average Loss: 0,0028009884181292074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 67: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 67/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 68/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 69: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 69/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 70/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 71: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 71/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 72/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 73/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 74/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 75: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 75/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 76/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 77/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 78/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 79: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 79/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 80/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 81: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 81/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 82/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 83: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 83/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 84/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 85: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 85/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 86/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 87: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 87/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 88/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 89: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 89/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 90/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 91: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 91/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 92/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 93: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 93/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 94/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 95: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 95/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 96/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 97: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 97/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 98/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 99: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 99/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 100/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 101: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 101/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 102/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 103: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 103/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 104/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 105: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 105/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 106/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 107/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 108/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 109: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 109/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 110/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 111: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 111/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 112/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 113: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 113/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 114/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 115: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 115/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 116/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 117/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 118/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 119/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 120/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 121: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 121/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 122/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 123: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 123/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 124/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 125/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 126/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 127: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 127/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 128/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 129: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 129/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 130/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 131/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 132/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 133: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 133/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 134/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 135: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 135/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 136/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 137/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 138/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 139: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 139/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 140/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 141: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 141/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 142/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 143/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 144/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 145: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 145/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 146/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 147: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 147/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 148: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 148/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 149/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 150/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 151: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 151/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 152/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 153: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 153/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 154/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 155: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 155/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 156/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 157: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 157/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 158/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 159: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 159/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 160/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 161: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 161/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 162/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 163: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 163/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 164/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 165: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 165/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 166/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 167/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 168/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 169: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 169/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 170/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 171: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 171/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 172/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 173/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 174/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 175: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 175/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 176/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 177: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 177/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 178/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 179/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 180/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 181: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 181/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 182/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 183: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 183/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 184/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 185/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 186/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 187: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 187/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 188/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 189: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 189/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 190/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 191/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 192/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 193: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 193/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 194/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 195: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 195/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 196/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 197/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 198/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 199: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 199/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 200/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 201: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 201/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 202/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 203/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 204/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 205: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 205/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 206/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 207: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 207/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 208/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 209/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 210/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 211: Loss did not decrease. Loss: 0,0028009884181292083
            Epoch 211/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 212/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 213: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 213/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 214/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 215: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 215/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 216/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 217: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 217/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 218/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 219: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 219/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 220/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 221/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 222/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 223: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 223/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 224/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 225: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 225/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 226/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 227: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 227/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 228/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 229: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 229/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 230/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 231: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 231/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 232/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 233: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 233/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 234/300, Average Loss: 0,002800988418129208
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 235: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 235/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule 
            */


            /*
            Input: atom, Target: molecule, Predicted: sphacteria
            Input: king, Target: queen, Predicted: partnership
            Input: electron, Target: proton, Predicted: sweetin
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: accelerator
            Epoch 1/300, Average Loss: 0,09912055848927301
            Input: atom, Target: molecule, Predicted: samba
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 2/300, Average Loss: 0,004891991056549727
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 3/300, Average Loss: 0,0027989692856172452
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 4: Loss did not decrease. Loss: 0,0028009935185905124
            Epoch 4/300, Average Loss: 0,0028009935185905124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 5/300, Average Loss: 0,0028009889722006276
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 6/300, Average Loss: 0,002800988404831916
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 7: Loss did not decrease. Loss: 0,0028009884165904565
            Epoch 7/300, Average Loss: 0,0028009884165904565
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 8: Loss did not decrease. Loss: 0,002800988419037055
            Epoch 8/300, Average Loss: 0,002800988419037055
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 9/300, Average Loss: 0,002800988417174363
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 10: Loss did not decrease. Loss: 0,002800988418574636
            Epoch 10/300, Average Loss: 0,002800988418574636
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 11/300, Average Loss: 0,002800988417606498
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 12: Loss did not decrease. Loss: 0,002800988418348102
            Epoch 12/300, Average Loss: 0,002800988418348102
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 13/300, Average Loss: 0,0028009884178450393
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0028009884182369263
            Epoch 14/300, Average Loss: 0,0028009884182369263
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 15/300, Average Loss: 0,0028009884179756076
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0028009884181822925
            Epoch 16/300, Average Loss: 0,0028009884181822925
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 17/300, Average Loss: 0,0028009884180465747
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0028009884181554082
            Epoch 18/300, Average Loss: 0,0028009884181554082
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 19/300, Average Loss: 0,002800988418084926
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 20: Loss did not decrease. Loss: 0,002800988418142157
            Epoch 20/300, Average Loss: 0,002800988418142157
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 21/300, Average Loss: 0,0028009884181055566
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0028009884181356185
            Epoch 22/300, Average Loss: 0,0028009884181356185
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 23/300, Average Loss: 0,0028009884181166094
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 24: Loss did not decrease. Loss: 0,002800988418132387
            Epoch 24/300, Average Loss: 0,002800988418132387
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 25/300, Average Loss: 0,0028009884181225122
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 26: Loss did not decrease. Loss: 0,002800988418130787
            Epoch 26/300, Average Loss: 0,002800988418130787
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 27/300, Average Loss: 0,0028009884181256573
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0028009884181299924
            Epoch 28/300, Average Loss: 0,0028009884181299924
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 29/300, Average Loss: 0,0028009884181273256
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0028009884181296007
            Epoch 30/300, Average Loss: 0,0028009884181296007
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 31/300, Average Loss: 0,0028009884181282134
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0028009884181294034
            Epoch 32/300, Average Loss: 0,0028009884181294034
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 33/300, Average Loss: 0,002800988418128682
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 34: Loss did not decrease. Loss: 0,002800988418129307
            Epoch 34/300, Average Loss: 0,002800988418129307
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 35/300, Average Loss: 0,0028009884181289307
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 36: Loss did not decrease. Loss: 0,002800988418129258
            Epoch 36/300, Average Loss: 0,002800988418129258
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 37/300, Average Loss: 0,002800988418129062
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0028009884181292334
            Epoch 38/300, Average Loss: 0,0028009884181292334
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 39/300, Average Loss: 0,0028009884181291315
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0028009884181292204
            Epoch 40/300, Average Loss: 0,0028009884181292204
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 41/300, Average Loss: 0,002800988418129167
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 42: Loss did not decrease. Loss: 0,002800988418129215
            Epoch 42/300, Average Loss: 0,002800988418129215
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 43/300, Average Loss: 0,002800988418129186
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 44: Loss did not decrease. Loss: 0,002800988418129212
            Epoch 44/300, Average Loss: 0,002800988418129212
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 45/300, Average Loss: 0,0028009884181291974
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 46: Loss did not decrease. Loss: 0,0028009884181292104
            Epoch 46/300, Average Loss: 0,0028009884181292104
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 47/300, Average Loss: 0,0028009884181292026
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 48: Loss did not decrease. Loss: 0,00280098841812921
            Epoch 48/300, Average Loss: 0,00280098841812921
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 49/300, Average Loss: 0,0028009884181292057
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0028009884181292096
            Epoch 50/300, Average Loss: 0,0028009884181292096
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 51/300, Average Loss: 0,002800988418129207
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 52: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 52/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 53/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 54: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 54/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 55/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 56: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 56/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 57/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 58/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 59/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 60/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 61/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 62: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 62/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 63/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 64/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 65/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 66/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 67: Loss did not decrease. Loss: 0,002800988418129209
            Epoch 67/300, Average Loss: 0,002800988418129209
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 68/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 69/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 70/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 71/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 72/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 73/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 74/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 75/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 76/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 77/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 78/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 79/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 80/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 81/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 82/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 83/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 84/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 85: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 85/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 86/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 87/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 88/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 89/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 90: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 90/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 91/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 92/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 93/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 94/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 95/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 96/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 97/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 98/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 99/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 100/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 101/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 102: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 102/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 103/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 104/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 105/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 106/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 107/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 108/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 109/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 110/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 111/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 112/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 113/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 114/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 115/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 116/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 117/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 118/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 119/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 120/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 121/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 122/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 123/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 124/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 125/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 126/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 127/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 128/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 129/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 130/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 131/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 132/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 133/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 134/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 135/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 136/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 137/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 138: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 138/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 139/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 140/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 141/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 142/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 143/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 144/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 145/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 146/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 147/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 148/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 149/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 150/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 151/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 152/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 153/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 154/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 155/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 156/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 157/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 158/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 159/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 160/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 161/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 162: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 162/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 163/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 164/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 165/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 166/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 167/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 168/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 169/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 170/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 171/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 172/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 173/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 174: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 174/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 175/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 176/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 177/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 178/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 179/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 180/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 181/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 182/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 183/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 184/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 185/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 186/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 187/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 188/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 189/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 190/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 191/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 192/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 193/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 194/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 195/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 196/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 197/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 198: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 198/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 199/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 200/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 201/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 202/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 203/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 204/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 205/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 206/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 207/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 208/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 209/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 210/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 211/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 212/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 213/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 214/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 215/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 216/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 217/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 218/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 219/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 220/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 221/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 222/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 223/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 224/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 225/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 226/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 227/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 228/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 229/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 230/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 231/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 232/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 233/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 234: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 234/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 235/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 236/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 237/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 238/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 239/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 240/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 241/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 242/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 243/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 244/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 245/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 246: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 246/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 247/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 248/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 249/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 250/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 251/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 252/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 253/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 254/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 255/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 256/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 257/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 258/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 259/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 260/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 261/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 262/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 263/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 264/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 265/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 266/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 267/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 268/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 269/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Warning: Epoch 270: Loss did not decrease. Loss: 0,0028009884181292087
            Epoch 270/300, Average Loss: 0,0028009884181292087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 271/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 272/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 273/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 274/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 275/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 276/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 277/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 278/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 279/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 280/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 281/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 282/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 283/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 284/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 285/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 286/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 287/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 288/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 289/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 290/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 291/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 292/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 293/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 294/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 295/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 296/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 297/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 298/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 299/300, Average Loss: 0,0028009884181292083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Input: genetics, Target: evolution, Predicted: proton
            Epoch 300/300, Average Loss: 0,0028009884181292083
            Model successfully decreased loss, indicating learning over epochs. 
            */


            Console.Read();
        }
    }
}
