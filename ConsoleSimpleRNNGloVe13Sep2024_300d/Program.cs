using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe13Sep2024_300d
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
            string[] inputWords = { "atom", "king", "electron", "salsa" };
            string[] targetWords = { "molecule", "queen", "proton", "samba" };

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
            Input: atom, Target: molecule, Predicted: ionia
            Input: king, Target: queen, Predicted: madang
            Input: electron, Target: proton, Predicted: kplc
            Input: salsa, Target: samba, Predicted: walthall
            Epoch 1/300, Average Loss: 0,096710942739402
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 2/300, Average Loss: 0,005483422274194773
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 3/300, Average Loss: 0,0021279750255033866
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 4/300, Average Loss: 0,002111937662788358
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 5: Loss did not decrease. Loss: 0,0021123889846090317
            Epoch 5/300, Average Loss: 0,0021123889846090317
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 6: Loss did not decrease. Loss: 0,0021123941437419503
            Epoch 6/300, Average Loss: 0,0021123941437419503
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 7/300, Average Loss: 0,002112393491675773
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 8: Loss did not decrease. Loss: 0,0021123936783186193
            Epoch 8/300, Average Loss: 0,0021123936783186193
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 9: Loss did not decrease. Loss: 0,002112393825765585
            Epoch 9/300, Average Loss: 0,002112393825765585
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 10: Loss did not decrease. Loss: 0,002112393947840337
            Epoch 10/300, Average Loss: 0,002112393947840337
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 11: Loss did not decrease. Loss: 0,002112394049686201
            Epoch 11/300, Average Loss: 0,002112394049686201
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 12: Loss did not decrease. Loss: 0,0021123941345718477
            Epoch 12/300, Average Loss: 0,0021123941345718477
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 13: Loss did not decrease. Loss: 0,002112394205277834
            Epoch 13/300, Average Loss: 0,002112394205277834
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 14: Loss did not decrease. Loss: 0,002112394264138876
            Epoch 14/300, Average Loss: 0,002112394264138876
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 15: Loss did not decrease. Loss: 0,002112394313112484
            Epoch 15/300, Average Loss: 0,002112394313112484
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0021123943538383784
            Epoch 16/300, Average Loss: 0,0021123943538383784
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 17: Loss did not decrease. Loss: 0,002112394387688851
            Epoch 17/300, Average Loss: 0,002112394387688851
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 18: Loss did not decrease. Loss: 0,0021123944158114123
            Epoch 18/300, Average Loss: 0,0021123944158114123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 19: Loss did not decrease. Loss: 0,002112394439164846
            Epoch 19/300, Average Loss: 0,002112394439164846
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 20: Loss did not decrease. Loss: 0,002112394458549654
            Epoch 20/300, Average Loss: 0,002112394458549654
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 21: Loss did not decrease. Loss: 0,0021123944746337144
            Epoch 21/300, Average Loss: 0,0021123944746337144
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 22: Loss did not decrease. Loss: 0,002112394487973881
            Epoch 22/300, Average Loss: 0,002112394487973881
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 23: Loss did not decrease. Loss: 0,0021123944990341513
            Epoch 23/300, Average Loss: 0,0021123944990341513
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 24: Loss did not decrease. Loss: 0,002112394508200918
            Epoch 24/300, Average Loss: 0,002112394508200918
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 25: Loss did not decrease. Loss: 0,002112394515795768
            Epoch 25/300, Average Loss: 0,002112394515795768
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0021123945220862094
            Epoch 26/300, Average Loss: 0,0021123945220862094
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 27: Loss did not decrease. Loss: 0,0021123945272946525
            Epoch 27/300, Average Loss: 0,0021123945272946525
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0021123945316059195
            Epoch 28/300, Average Loss: 0,0021123945316059195
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 29: Loss did not decrease. Loss: 0,0021123945351735327
            Epoch 29/300, Average Loss: 0,0021123945351735327
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0021123945381249544
            Epoch 30/300, Average Loss: 0,0021123945381249544
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 31: Loss did not decrease. Loss: 0,0021123945405659658
            Epoch 31/300, Average Loss: 0,0021123945405659658
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0021123945425843248
            Epoch 32/300, Average Loss: 0,0021123945425843248
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 33: Loss did not decrease. Loss: 0,0021123945442528052
            Epoch 33/300, Average Loss: 0,0021123945442528052
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 34: Loss did not decrease. Loss: 0,002112394545631735
            Epoch 34/300, Average Loss: 0,002112394545631735
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 35: Loss did not decrease. Loss: 0,0021123945467711063
            Epoch 35/300, Average Loss: 0,0021123945467711063
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0021123945477123304
            Epoch 36/300, Average Loss: 0,0021123945477123304
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 37: Loss did not decrease. Loss: 0,002112394548489706
            Epoch 37/300, Average Loss: 0,002112394548489706
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0021123945491316274
            Epoch 38/300, Average Loss: 0,0021123945491316274
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 39: Loss did not decrease. Loss: 0,0021123945496615924
            Epoch 39/300, Average Loss: 0,0021123945496615924
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 40: Loss did not decrease. Loss: 0,002112394550099044
            Epoch 40/300, Average Loss: 0,002112394550099044
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 41: Loss did not decrease. Loss: 0,002112394550460069
            Epoch 41/300, Average Loss: 0,002112394550460069
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 42: Loss did not decrease. Loss: 0,0021123945507579662
            Epoch 42/300, Average Loss: 0,0021123945507579662
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 43: Loss did not decrease. Loss: 0,0021123945510037345
            Epoch 43/300, Average Loss: 0,0021123945510037345
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0021123945512064608
            Epoch 44/300, Average Loss: 0,0021123945512064608
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 45: Loss did not decrease. Loss: 0,0021123945513736577
            Epoch 45/300, Average Loss: 0,0021123945513736577
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 46: Loss did not decrease. Loss: 0,0021123945515115305
            Epoch 46/300, Average Loss: 0,0021123945515115305
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 47: Loss did not decrease. Loss: 0,0021123945516252083
            Epoch 47/300, Average Loss: 0,0021123945516252083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0021123945517189193
            Epoch 48/300, Average Loss: 0,0021123945517189193
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 49: Loss did not decrease. Loss: 0,0021123945517961635
            Epoch 49/300, Average Loss: 0,0021123945517961635
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0021123945518598235
            Epoch 50/300, Average Loss: 0,0021123945518598235
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 51: Loss did not decrease. Loss: 0,002112394551912284
            Epoch 51/300, Average Loss: 0,002112394551912284
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0021123945519555096
            Epoch 52/300, Average Loss: 0,0021123945519555096
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 53: Loss did not decrease. Loss: 0,0021123945519911187
            Epoch 53/300, Average Loss: 0,0021123945519911187
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 54: Loss did not decrease. Loss: 0,002112394552020452
            Epoch 54/300, Average Loss: 0,002112394552020452
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 55: Loss did not decrease. Loss: 0,002112394552044614
            Epoch 55/300, Average Loss: 0,002112394552044614
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0021123945520645127
            Epoch 56/300, Average Loss: 0,0021123945520645127
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 57: Loss did not decrease. Loss: 0,0021123945520809
            Epoch 57/300, Average Loss: 0,0021123945520809
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 58: Loss did not decrease. Loss: 0,002112394552094392
            Epoch 58/300, Average Loss: 0,002112394552094392
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 59: Loss did not decrease. Loss: 0,0021123945521055003
            Epoch 59/300, Average Loss: 0,0021123945521055003
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0021123945521146453
            Epoch 60/300, Average Loss: 0,0021123945521146453
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 61: Loss did not decrease. Loss: 0,002112394552122172
            Epoch 61/300, Average Loss: 0,002112394552122172
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 62: Loss did not decrease. Loss: 0,002112394552128369
            Epoch 62/300, Average Loss: 0,002112394552128369
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 63: Loss did not decrease. Loss: 0,0021123945521334684
            Epoch 63/300, Average Loss: 0,0021123945521334684
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 64: Loss did not decrease. Loss: 0,002112394552137664
            Epoch 64/300, Average Loss: 0,002112394552137664
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 65: Loss did not decrease. Loss: 0,002112394552141118
            Epoch 65/300, Average Loss: 0,002112394552141118
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0021123945521439595
            Epoch 66/300, Average Loss: 0,0021123945521439595
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 67: Loss did not decrease. Loss: 0,0021123945521462966
            Epoch 67/300, Average Loss: 0,0021123945521462966
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 68: Loss did not decrease. Loss: 0,0021123945521482196
            Epoch 68/300, Average Loss: 0,0021123945521482196
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 69: Loss did not decrease. Loss: 0,002112394552149802
            Epoch 69/300, Average Loss: 0,002112394552149802
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 70: Loss did not decrease. Loss: 0,0021123945521511027
            Epoch 70/300, Average Loss: 0,0021123945521511027
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 71: Loss did not decrease. Loss: 0,0021123945521521748
            Epoch 71/300, Average Loss: 0,0021123945521521748
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 72: Loss did not decrease. Loss: 0,0021123945521530543
            Epoch 72/300, Average Loss: 0,0021123945521530543
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 73: Loss did not decrease. Loss: 0,0021123945521537794
            Epoch 73/300, Average Loss: 0,0021123945521537794
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 74: Loss did not decrease. Loss: 0,0021123945521543735
            Epoch 74/300, Average Loss: 0,0021123945521543735
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 75: Loss did not decrease. Loss: 0,002112394552154864
            Epoch 75/300, Average Loss: 0,002112394552154864
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 76: Loss did not decrease. Loss: 0,0021123945521552665
            Epoch 76/300, Average Loss: 0,0021123945521552665
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 77: Loss did not decrease. Loss: 0,002112394552155597
            Epoch 77/300, Average Loss: 0,002112394552155597
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 78: Loss did not decrease. Loss: 0,002112394552155869
            Epoch 78/300, Average Loss: 0,002112394552155869
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 79: Loss did not decrease. Loss: 0,002112394552156093
            Epoch 79/300, Average Loss: 0,002112394552156093
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 80: Loss did not decrease. Loss: 0,002112394552156277
            Epoch 80/300, Average Loss: 0,002112394552156277
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 81: Loss did not decrease. Loss: 0,0021123945521564283
            Epoch 81/300, Average Loss: 0,0021123945521564283
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0021123945521565528
            Epoch 82/300, Average Loss: 0,0021123945521565528
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 83: Loss did not decrease. Loss: 0,0021123945521566555
            Epoch 83/300, Average Loss: 0,0021123945521566555
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 84: Loss did not decrease. Loss: 0,002112394552156738
            Epoch 84/300, Average Loss: 0,002112394552156738
            */


            /*
            Input: atom, Target: molecule, Predicted: habakkuk
            Input: king, Target: queen, Predicted: emilia-romagna
            Input: electron, Target: proton, Predicted: scheels
            Input: salsa, Target: samba, Predicted: retreat
            Epoch 1/300, Average Loss: 0,1038413395101776
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 2/300, Average Loss: 0,00446251570493339
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 3/300, Average Loss: 0,0021209395946334103
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 4/300, Average Loss: 0,0021119195704866305
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 5: Loss did not decrease. Loss: 0,0021123996205732985
            Epoch 5/300, Average Loss: 0,0021123996205732985
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 6/300, Average Loss: 0,002112395149507079
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 7/300, Average Loss: 0,0021123944758369134
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 8: Loss did not decrease. Loss: 0,0021123945056655794
            Epoch 8/300, Average Loss: 0,0021123945056655794
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 9: Loss did not decrease. Loss: 0,0021123945127455744
            Epoch 9/300, Average Loss: 0,0021123945127455744
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 10: Loss did not decrease. Loss: 0,002112394518363748
            Epoch 10/300, Average Loss: 0,002112394518363748
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 11: Loss did not decrease. Loss: 0,0021123945232888472
            Epoch 11/300, Average Loss: 0,0021123945232888472
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 12: Loss did not decrease. Loss: 0,002112394527551441
            Epoch 12/300, Average Loss: 0,002112394527551441
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 13: Loss did not decrease. Loss: 0,0021123945312274263
            Epoch 13/300, Average Loss: 0,0021123945312274263
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 14: Loss did not decrease. Loss: 0,0021123945343874996
            Epoch 14/300, Average Loss: 0,0021123945343874996
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 15: Loss did not decrease. Loss: 0,0021123945370963215
            Epoch 15/300, Average Loss: 0,0021123945370963215
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 16: Loss did not decrease. Loss: 0,0021123945394123708
            Epoch 16/300, Average Loss: 0,0021123945394123708
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 17: Loss did not decrease. Loss: 0,002112394541387998
            Epoch 17/300, Average Loss: 0,002112394541387998
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 18: Loss did not decrease. Loss: 0,002112394543069679
            Epoch 18/300, Average Loss: 0,002112394543069679
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 19: Loss did not decrease. Loss: 0,002112394544498391
            Epoch 19/300, Average Loss: 0,002112394544498391
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 20: Loss did not decrease. Loss: 0,002112394545710039
            Epoch 20/300, Average Loss: 0,002112394545710039
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 21: Loss did not decrease. Loss: 0,002112394546735931
            Epoch 21/300, Average Loss: 0,002112394546735931
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 22: Loss did not decrease. Loss: 0,002112394547603242
            Epoch 22/300, Average Loss: 0,002112394547603242
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 23: Loss did not decrease. Loss: 0,0021123945483354626
            Epoch 23/300, Average Loss: 0,0021123945483354626
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0021123945489528407
            Epoch 24/300, Average Loss: 0,0021123945489528407
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 25: Loss did not decrease. Loss: 0,00211239454947276
            Epoch 25/300, Average Loss: 0,00211239454947276
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 26: Loss did not decrease. Loss: 0,002112394549910117
            Epoch 26/300, Average Loss: 0,002112394549910117
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 27: Loss did not decrease. Loss: 0,002112394550277637
            Epoch 27/300, Average Loss: 0,002112394550277637
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0021123945505861683
            Epoch 28/300, Average Loss: 0,0021123945505861683
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 29: Loss did not decrease. Loss: 0,0021123945508449388
            Epoch 29/300, Average Loss: 0,0021123945508449388
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0021123945510617866
            Epoch 30/300, Average Loss: 0,0021123945510617866
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 31: Loss did not decrease. Loss: 0,0021123945512433566
            Epoch 31/300, Average Loss: 0,0021123945512433566
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 32: Loss did not decrease. Loss: 0,0021123945513952707
            Epoch 32/300, Average Loss: 0,0021123945513952707
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 33: Loss did not decrease. Loss: 0,0021123945515222767
            Epoch 33/300, Average Loss: 0,0021123945515222767
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0021123945516283884
            Epoch 34/300, Average Loss: 0,0021123945516283884
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 35: Loss did not decrease. Loss: 0,0021123945517169855
            Epoch 35/300, Average Loss: 0,0021123945517169855
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 36: Loss did not decrease. Loss: 0,0021123945517909112
            Epoch 36/300, Average Loss: 0,0021123945517909112
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 37: Loss did not decrease. Loss: 0,0021123945518525603
            Epoch 37/300, Average Loss: 0,0021123945518525603
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 38: Loss did not decrease. Loss: 0,00211239455190394
            Epoch 38/300, Average Loss: 0,00211239455190394
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 39: Loss did not decrease. Loss: 0,0021123945519467414
            Epoch 39/300, Average Loss: 0,0021123945519467414
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 40: Loss did not decrease. Loss: 0,002112394551982378
            Epoch 40/300, Average Loss: 0,002112394551982378
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 41: Loss did not decrease. Loss: 0,0021123945520120343
            Epoch 41/300, Average Loss: 0,0021123945520120343
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 42: Loss did not decrease. Loss: 0,0021123945520367
            Epoch 42/300, Average Loss: 0,0021123945520367
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 43: Loss did not decrease. Loss: 0,0021123945520572104
            Epoch 43/300, Average Loss: 0,0021123945520572104
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 44: Loss did not decrease. Loss: 0,002112394552074254
            Epoch 44/300, Average Loss: 0,002112394552074254
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 45: Loss did not decrease. Loss: 0,002112394552088414
            Epoch 45/300, Average Loss: 0,002112394552088414
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 46: Loss did not decrease. Loss: 0,0021123945521001703
            Epoch 46/300, Average Loss: 0,0021123945521001703
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 47: Loss did not decrease. Loss: 0,0021123945521099316
            Epoch 47/300, Average Loss: 0,0021123945521099316
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0021123945521180306
            Epoch 48/300, Average Loss: 0,0021123945521180306
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 49: Loss did not decrease. Loss: 0,002112394552124749
            Epoch 49/300, Average Loss: 0,002112394552124749
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0021123945521303203
            Epoch 50/300, Average Loss: 0,0021123945521303203
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 51: Loss did not decrease. Loss: 0,002112394552134939
            Epoch 51/300, Average Loss: 0,002112394552134939
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0021123945521387653
            Epoch 52/300, Average Loss: 0,0021123945521387653
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 53: Loss did not decrease. Loss: 0,0021123945521419364
            Epoch 53/300, Average Loss: 0,0021123945521419364
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 54: Loss did not decrease. Loss: 0,002112394552144562
            Epoch 54/300, Average Loss: 0,002112394552144562
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 55: Loss did not decrease. Loss: 0,0021123945521467373
            Epoch 55/300, Average Loss: 0,0021123945521467373
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 56: Loss did not decrease. Loss: 0,002112394552148536
            Epoch 56/300, Average Loss: 0,002112394552148536
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 57: Loss did not decrease. Loss: 0,0021123945521500267
            Epoch 57/300, Average Loss: 0,0021123945521500267
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 58: Loss did not decrease. Loss: 0,002112394552151259
            Epoch 58/300, Average Loss: 0,002112394552151259
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 59: Loss did not decrease. Loss: 0,002112394552152278
            Epoch 59/300, Average Loss: 0,002112394552152278
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 60: Loss did not decrease. Loss: 0,002112394552153121
            Epoch 60/300, Average Loss: 0,002112394552153121
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 61: Loss did not decrease. Loss: 0,0021123945521538193
            Epoch 61/300, Average Loss: 0,0021123945521538193
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 62: Loss did not decrease. Loss: 0,0021123945521543935
            Epoch 62/300, Average Loss: 0,0021123945521543935
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 63: Loss did not decrease. Loss: 0,0021123945521548714
            Epoch 63/300, Average Loss: 0,0021123945521548714
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 64: Loss did not decrease. Loss: 0,002112394552155264
            Epoch 64/300, Average Loss: 0,002112394552155264
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 65: Loss did not decrease. Loss: 0,002112394552155589
            Epoch 65/300, Average Loss: 0,002112394552155589
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 66: Loss did not decrease. Loss: 0,0021123945521558576
            Epoch 66/300, Average Loss: 0,0021123945521558576
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 67: Loss did not decrease. Loss: 0,002112394552156079
            Epoch 67/300, Average Loss: 0,002112394552156079
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 68: Loss did not decrease. Loss: 0,0021123945521562635
            Epoch 68/300, Average Loss: 0,0021123945521562635
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 69: Loss did not decrease. Loss: 0,0021123945521564136
            Epoch 69/300, Average Loss: 0,0021123945521564136
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 70: Loss did not decrease. Loss: 0,002112394552156539
            Epoch 70/300, Average Loss: 0,002112394552156539
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 71: Loss did not decrease. Loss: 0,0021123945521566412
            Epoch 71/300, Average Loss: 0,0021123945521566412
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 72: Loss did not decrease. Loss: 0,0021123945521567267
            Epoch 72/300, Average Loss: 0,0021123945521567267
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 73: Loss did not decrease. Loss: 0,0021123945521567943
            Epoch 73/300, Average Loss: 0,0021123945521567943
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 74: Loss did not decrease. Loss: 0,002112394552156855
            Epoch 74/300, Average Loss: 0,002112394552156855
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 75: Loss did not decrease. Loss: 0,002112394552156902
            Epoch 75/300, Average Loss: 0,002112394552156902
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 76: Loss did not decrease. Loss: 0,002112394552156941
            Epoch 76/300, Average Loss: 0,002112394552156941
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 77: Loss did not decrease. Loss: 0,0021123945521569734
            Epoch 77/300, Average Loss: 0,0021123945521569734
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 78: Loss did not decrease. Loss: 0,002112394552157001
            Epoch 78/300, Average Loss: 0,002112394552157001
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 79: Loss did not decrease. Loss: 0,002112394552157022
            Epoch 79/300, Average Loss: 0,002112394552157022
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 80: Loss did not decrease. Loss: 0,002112394552157041
            Epoch 80/300, Average Loss: 0,002112394552157041
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 81: Loss did not decrease. Loss: 0,0021123945521570537
            Epoch 81/300, Average Loss: 0,0021123945521570537
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 82: Loss did not decrease. Loss: 0,002112394552157068
            Epoch 82/300, Average Loss: 0,002112394552157068
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 83: Loss did not decrease. Loss: 0,0021123945521570775
            Epoch 83/300, Average Loss: 0,0021123945521570775
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 84: Loss did not decrease. Loss: 0,0021123945521570858
            Epoch 84/300, Average Loss: 0,0021123945521570858
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 85: Loss did not decrease. Loss: 0,002112394552157093
            Epoch 85/300, Average Loss: 0,002112394552157093
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 86: Loss did not decrease. Loss: 0,0021123945521570988
            Epoch 86/300, Average Loss: 0,0021123945521570988
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 87: Loss did not decrease. Loss: 0,002112394552157103
            Epoch 87/300, Average Loss: 0,002112394552157103
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 88: Loss did not decrease. Loss: 0,0021123945521571074
            Epoch 88/300, Average Loss: 0,0021123945521571074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 89: Loss did not decrease. Loss: 0,0021123945521571105
            Epoch 89/300, Average Loss: 0,0021123945521571105
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 90: Loss did not decrease. Loss: 0,002112394552157113
            Epoch 90/300, Average Loss: 0,002112394552157113
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 91: Loss did not decrease. Loss: 0,0021123945521571157
            Epoch 91/300, Average Loss: 0,0021123945521571157
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 92: Loss did not decrease. Loss: 0,002112394552157117
            Epoch 92/300, Average Loss: 0,002112394552157117
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 93: Loss did not decrease. Loss: 0,0021123945521571187
            Epoch 93/300, Average Loss: 0,0021123945521571187
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 94: Loss did not decrease. Loss: 0,0021123945521571196
            Epoch 94/300, Average Loss: 0,0021123945521571196
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 95: Loss did not decrease. Loss: 0,002112394552157121
            Epoch 95/300, Average Loss: 0,002112394552157121
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 96: Loss did not decrease. Loss: 0,0021123945521571218
            Epoch 96/300, Average Loss: 0,0021123945521571218
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 97: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 97/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 98/300, Average Loss: 0,002112394552157122
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 99/300, Average Loss: 0,002112394552157122
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 100: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 100/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 101/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 102: Loss did not decrease. Loss: 0,002112394552157124
            Epoch 102/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 103/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 104: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 104/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 105/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 106: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 106/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 107: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 107/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 108/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 109: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 109/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 110/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 111: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 111/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 112/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 113/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 114/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 115/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 116/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 117/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 118: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 118/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 119/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 120/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 121: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 121/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 122/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 123: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 123/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 124/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 125/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 126: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 126/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 127: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 127/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 128/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 129: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 129/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 130/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 131/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 132/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 133/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 134: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 134/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 135/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 136/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 137/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 138/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 139/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 140/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 141/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 142/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 143/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 144/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 145/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 146/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 147/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 148/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 149/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 150/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 151/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 152/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 153/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 154/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 155/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 156/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 157/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 158/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 159/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 160/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 161/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 162/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 163/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 164/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 165/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 166/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 167/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 168/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 169/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 170/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 171/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 172/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 173/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 174/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 175/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 176/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 177/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 178/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 179/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 180/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 181/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 182/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 183: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 183/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 184/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 185: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 185/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 186/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 187/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 188/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 189/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 190/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 191/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 192/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 193/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 194/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 195/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 196/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 197/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 198/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 199/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 200/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 201/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 202/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 203/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 204/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 205/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 206/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 207: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 207/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 208/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 209/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 210/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 211/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 212/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 213/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 214/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 215/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 216/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 217/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 218/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 219/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 220/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 221/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 222/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 223/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 224/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 225/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 226/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 227/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 228/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 229/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 230/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 231/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 232/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 233/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 234/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 235/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 236/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 237/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 238/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 239/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 240/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 241/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 242/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 243: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 243/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 244/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 245: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 245/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 246/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 247/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 248/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 249/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 250/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 251/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 252/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 253/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 254/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 255/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 256/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 257/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 258/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 259/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 260/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 261/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 262/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 263/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 264/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 265/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 266/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 267: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 267/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 268/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 269/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 270/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 271/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 272/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 273/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 274/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 275/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 276/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 277/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 278/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 279/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 280/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 281/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 282/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 283/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 284/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 285/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 286/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 287/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 288/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 289/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 290/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 291: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 291/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 292/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 293/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 294/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 295/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 296/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 297/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 298/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 299/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 300/300, Average Loss: 0,002112394552157125
            Model successfully decreased loss, indicating learning over epochs. 
            */


            /*
            Input: atom, Target: molecule, Predicted: vursh
            Input: king, Target: queen, Predicted: zugdidi
            Input: electron, Target: proton, Predicted: linafelter
            Input: salsa, Target: samba, Predicted: antiguan
            Epoch 1/300, Average Loss: 0,116220371266843
            Input: atom, Target: molecule, Predicted: crtc
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 2/300, Average Loss: 0,010529588322414721
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 3/300, Average Loss: 0,002120825090869756
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 4/300, Average Loss: 0,002112057621707783
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 5: Loss did not decrease. Loss: 0,002112405995913624
            Epoch 5/300, Average Loss: 0,002112405995913624
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 6/300, Average Loss: 0,0021123948857447094
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 7/300, Average Loss: 0,00211239451283928
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 8: Loss did not decrease. Loss: 0,0021123945415471583
            Epoch 8/300, Average Loss: 0,0021123945415471583
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 9: Loss did not decrease. Loss: 0,0021123945435911826
            Epoch 9/300, Average Loss: 0,0021123945435911826
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 10: Loss did not decrease. Loss: 0,002112394545188016
            Epoch 10/300, Average Loss: 0,002112394545188016
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 11: Loss did not decrease. Loss: 0,0021123945465132913
            Epoch 11/300, Average Loss: 0,0021123945465132913
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 12: Loss did not decrease. Loss: 0,00211239454758381
            Epoch 12/300, Average Loss: 0,00211239454758381
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 13: Loss did not decrease. Loss: 0,0021123945484494144
            Epoch 13/300, Average Loss: 0,0021123945484494144
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 14: Loss did not decrease. Loss: 0,002112394549149658
            Epoch 14/300, Average Loss: 0,002112394549149658
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 15: Loss did not decrease. Loss: 0,002112394549716375
            Epoch 15/300, Average Loss: 0,002112394549716375
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 16: Loss did not decrease. Loss: 0,002112394550175234
            Epoch 16/300, Average Loss: 0,002112394550175234
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 17: Loss did not decrease. Loss: 0,0021123945505469362
            Epoch 17/300, Average Loss: 0,0021123945505469362
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 18: Loss did not decrease. Loss: 0,002112394550848185
            Epoch 18/300, Average Loss: 0,002112394550848185
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 19: Loss did not decrease. Loss: 0,0021123945510924547
            Epoch 19/300, Average Loss: 0,0021123945510924547
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 20: Loss did not decrease. Loss: 0,0021123945512906265
            Epoch 20/300, Average Loss: 0,0021123945512906265
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 21: Loss did not decrease. Loss: 0,0021123945514514835
            Epoch 21/300, Average Loss: 0,0021123945514514835
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 22: Loss did not decrease. Loss: 0,0021123945515821273
            Epoch 22/300, Average Loss: 0,0021123945515821273
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 23: Loss did not decrease. Loss: 0,0021123945516882893
            Epoch 23/300, Average Loss: 0,0021123945516882893
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 24: Loss did not decrease. Loss: 0,0021123945517746087
            Epoch 24/300, Average Loss: 0,0021123945517746087
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 25: Loss did not decrease. Loss: 0,002112394551844835
            Epoch 25/300, Average Loss: 0,002112394551844835
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 26: Loss did not decrease. Loss: 0,0021123945519020025
            Epoch 26/300, Average Loss: 0,0021123945519020025
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 27: Loss did not decrease. Loss: 0,002112394551948569
            Epoch 27/300, Average Loss: 0,002112394551948569
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 28: Loss did not decrease. Loss: 0,0021123945519865234
            Epoch 28/300, Average Loss: 0,0021123945519865234
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 29: Loss did not decrease. Loss: 0,00211239455201748
            Epoch 29/300, Average Loss: 0,00211239455201748
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 30: Loss did not decrease. Loss: 0,0021123945520427415
            Epoch 30/300, Average Loss: 0,0021123945520427415
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 31: Loss did not decrease. Loss: 0,0021123945520633704
            Epoch 31/300, Average Loss: 0,0021123945520633704
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 32: Loss did not decrease. Loss: 0,002112394552080228
            Epoch 32/300, Average Loss: 0,002112394552080228
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 33: Loss did not decrease. Loss: 0,002112394552094014
            Epoch 33/300, Average Loss: 0,002112394552094014
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 34: Loss did not decrease. Loss: 0,0021123945521052943
            Epoch 34/300, Average Loss: 0,0021123945521052943
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 35: Loss did not decrease. Loss: 0,0021123945521145287
            Epoch 35/300, Average Loss: 0,0021123945521145287
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 36: Loss did not decrease. Loss: 0,002112394552122097
            Epoch 36/300, Average Loss: 0,002112394552122097
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 37: Loss did not decrease. Loss: 0,0021123945521283006
            Epoch 37/300, Average Loss: 0,0021123945521283006
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 38: Loss did not decrease. Loss: 0,0021123945521333916
            Epoch 38/300, Average Loss: 0,0021123945521333916
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 39: Loss did not decrease. Loss: 0,00211239455213757
            Epoch 39/300, Average Loss: 0,00211239455213757
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 40: Loss did not decrease. Loss: 0,0021123945521410036
            Epoch 40/300, Average Loss: 0,0021123945521410036
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 41: Loss did not decrease. Loss: 0,002112394552143826
            Epoch 41/300, Average Loss: 0,002112394552143826
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 42: Loss did not decrease. Loss: 0,0021123945521461488
            Epoch 42/300, Average Loss: 0,0021123945521461488
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 43: Loss did not decrease. Loss: 0,0021123945521480587
            Epoch 43/300, Average Loss: 0,0021123945521480587
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 44: Loss did not decrease. Loss: 0,0021123945521496325
            Epoch 44/300, Average Loss: 0,0021123945521496325
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 45: Loss did not decrease. Loss: 0,00211239455215093
            Epoch 45/300, Average Loss: 0,00211239455215093
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 46: Loss did not decrease. Loss: 0,002112394552152001
            Epoch 46/300, Average Loss: 0,002112394552152001
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 47: Loss did not decrease. Loss: 0,002112394552152883
            Epoch 47/300, Average Loss: 0,002112394552152883
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 48: Loss did not decrease. Loss: 0,0021123945521536115
            Epoch 48/300, Average Loss: 0,0021123945521536115
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 49: Loss did not decrease. Loss: 0,002112394552154214
            Epoch 49/300, Average Loss: 0,002112394552154214
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 50: Loss did not decrease. Loss: 0,0021123945521547127
            Epoch 50/300, Average Loss: 0,0021123945521547127
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 51: Loss did not decrease. Loss: 0,0021123945521551247
            Epoch 51/300, Average Loss: 0,0021123945521551247
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 52: Loss did not decrease. Loss: 0,0021123945521554647
            Epoch 52/300, Average Loss: 0,0021123945521554647
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 53: Loss did not decrease. Loss: 0,0021123945521557465
            Epoch 53/300, Average Loss: 0,0021123945521557465
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 54: Loss did not decrease. Loss: 0,0021123945521559803
            Epoch 54/300, Average Loss: 0,0021123945521559803
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 55: Loss did not decrease. Loss: 0,002112394552156174
            Epoch 55/300, Average Loss: 0,002112394552156174
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 56: Loss did not decrease. Loss: 0,0021123945521563355
            Epoch 56/300, Average Loss: 0,0021123945521563355
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 57: Loss did not decrease. Loss: 0,002112394552156468
            Epoch 57/300, Average Loss: 0,002112394552156468
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 58: Loss did not decrease. Loss: 0,002112394552156579
            Epoch 58/300, Average Loss: 0,002112394552156579
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 59: Loss did not decrease. Loss: 0,002112394552156671
            Epoch 59/300, Average Loss: 0,002112394552156671
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 60: Loss did not decrease. Loss: 0,0021123945521567466
            Epoch 60/300, Average Loss: 0,0021123945521567466
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 61: Loss did not decrease. Loss: 0,00211239455215681
            Epoch 61/300, Average Loss: 0,00211239455215681
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 62: Loss did not decrease. Loss: 0,0021123945521568633
            Epoch 62/300, Average Loss: 0,0021123945521568633
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 63: Loss did not decrease. Loss: 0,0021123945521569075
            Epoch 63/300, Average Loss: 0,0021123945521569075
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 64: Loss did not decrease. Loss: 0,0021123945521569426
            Epoch 64/300, Average Loss: 0,0021123945521569426
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 65: Loss did not decrease. Loss: 0,0021123945521569726
            Epoch 65/300, Average Loss: 0,0021123945521569726
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 66: Loss did not decrease. Loss: 0,002112394552156998
            Epoch 66/300, Average Loss: 0,002112394552156998
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 67: Loss did not decrease. Loss: 0,0021123945521570207
            Epoch 67/300, Average Loss: 0,0021123945521570207
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 68: Loss did not decrease. Loss: 0,002112394552157037
            Epoch 68/300, Average Loss: 0,002112394552157037
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 69: Loss did not decrease. Loss: 0,002112394552157051
            Epoch 69/300, Average Loss: 0,002112394552157051
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 70: Loss did not decrease. Loss: 0,002112394552157065
            Epoch 70/300, Average Loss: 0,002112394552157065
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 71: Loss did not decrease. Loss: 0,0021123945521570745
            Epoch 71/300, Average Loss: 0,0021123945521570745
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 72: Loss did not decrease. Loss: 0,002112394552157083
            Epoch 72/300, Average Loss: 0,002112394552157083
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 73: Loss did not decrease. Loss: 0,0021123945521570897
            Epoch 73/300, Average Loss: 0,0021123945521570897
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 74: Loss did not decrease. Loss: 0,0021123945521570953
            Epoch 74/300, Average Loss: 0,0021123945521570953
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 75: Loss did not decrease. Loss: 0,0021123945521571005
            Epoch 75/300, Average Loss: 0,0021123945521571005
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 76: Loss did not decrease. Loss: 0,002112394552157103
            Epoch 76/300, Average Loss: 0,002112394552157103
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 77: Loss did not decrease. Loss: 0,0021123945521571074
            Epoch 77/300, Average Loss: 0,0021123945521571074
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 78: Loss did not decrease. Loss: 0,00211239455215711
            Epoch 78/300, Average Loss: 0,00211239455215711
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 79: Loss did not decrease. Loss: 0,0021123945521571118
            Epoch 79/300, Average Loss: 0,0021123945521571118
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 80: Loss did not decrease. Loss: 0,0021123945521571144
            Epoch 80/300, Average Loss: 0,0021123945521571144
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 81: Loss did not decrease. Loss: 0,002112394552157116
            Epoch 81/300, Average Loss: 0,002112394552157116
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 82: Loss did not decrease. Loss: 0,0021123945521571187
            Epoch 82/300, Average Loss: 0,0021123945521571187
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 83: Loss did not decrease. Loss: 0,002112394552157119
            Epoch 83/300, Average Loss: 0,002112394552157119
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 84: Loss did not decrease. Loss: 0,00211239455215712
            Epoch 84/300, Average Loss: 0,00211239455215712
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 85: Loss did not decrease. Loss: 0,0021123945521571205
            Epoch 85/300, Average Loss: 0,0021123945521571205
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 86: Loss did not decrease. Loss: 0,0021123945521571213
            Epoch 86/300, Average Loss: 0,0021123945521571213
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 87: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 87/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 88/300, Average Loss: 0,0021123945521571226
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 89: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 89/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 90/300, Average Loss: 0,002112394552157122
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 91: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 91/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 92/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 93: Loss did not decrease. Loss: 0,0021123945521571244
            Epoch 93/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 94/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 95/300, Average Loss: 0,0021123945521571226
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 96: Loss did not decrease. Loss: 0,002112394552157123
            Epoch 96/300, Average Loss: 0,002112394552157123
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 97: Loss did not decrease. Loss: 0,002112394552157124
            Epoch 97/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 98/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 99/300, Average Loss: 0,002112394552157124
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 100: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 100/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 101/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 102: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 102/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 103/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 104/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 105: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 105/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 106: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 106/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 107/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 108: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 108/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 109/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 110/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 111: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 111/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 112/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 113/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 114: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 114/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 115: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 115/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 116/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 117: Loss did not decrease. Loss: 0,0021123945521571257
            Epoch 117/300, Average Loss: 0,0021123945521571257
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 118/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 119/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 120/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 121: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 121/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 122/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 123/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 124/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 125/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 126/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 127/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 128/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 129/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 130/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 131/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 132/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 133/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 134/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 135/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 136/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 137/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 138/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 139/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 140/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 141/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 142/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 143: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 143/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 144/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 145/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 146/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 147: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 147/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 148/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 149/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 150/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 151/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 152/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 153/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 154/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 155/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 156/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 157/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 158/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 159/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 160/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 161/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 162/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 163/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 164/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 165/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 166/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 167/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 168/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 169/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 170/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 171/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 172/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 173/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 174/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 175/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 176/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 177/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 178/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 179/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 180/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 181/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 182/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 183: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 183/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 184/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 185/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 186/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 187/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 188/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 189/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 190/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 191/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 192/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 193/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 194/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 195/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 196/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 197/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 198/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 199/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 200/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 201/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 202/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 203/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 204/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 205/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 206/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 207/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 208/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 209/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 210/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 211/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 212/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 213/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 214/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 215/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 216/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 217/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 218/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 219: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 219/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 220/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 221/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 222/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 223/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 224/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 225/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 226/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 227/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 228/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 229/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 230/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 231/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 232/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 233/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 234/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 235/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 236/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 237/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 238/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 239/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 240/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 241/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 242/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 243/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 244/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 245/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 246/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 247/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 248/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 249/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 250/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 251/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 252/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 253/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 254/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 255/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 256/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 257/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 258/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 259/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 260/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 261/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 262/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 263/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 264/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 265/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 266/300, Average Loss: 0,0021123945521571244
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 267: Loss did not decrease. Loss: 0,002112394552157125
            Epoch 267/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 268/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 269/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 270/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 271/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 272/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 273/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 274/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 275/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 276/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 277/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 278/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 279/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 280/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 281/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 282/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Warning: Epoch 283: Loss did not decrease. Loss: 0,0021123945521571252
            Epoch 283/300, Average Loss: 0,0021123945521571252
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 284/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 285/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 286/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 287/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 288/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 289/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 290/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 291/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 292/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 293/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 294/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 295/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 296/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 297/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 298/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 299/300, Average Loss: 0,002112394552157125
            Input: atom, Target: molecule, Predicted: proton
            Input: king, Target: queen, Predicted: samba
            Input: electron, Target: proton, Predicted: molecule
            Input: salsa, Target: samba, Predicted: queen
            Epoch 300/300, Average Loss: 0,002112394552157125
            Model successfully decreased loss, indicating learning over epochs. 
            */


            Console.Read();
        }
    }
}
