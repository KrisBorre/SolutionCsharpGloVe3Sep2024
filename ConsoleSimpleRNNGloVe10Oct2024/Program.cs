using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNNGloVe10Oct2024
{
    internal class Program
    {
        /// <summary>
        /// ChatGPT-4o
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Load Glove embeddings
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
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
            Input: atom, Target: molecule, Predicted: monooxygenase
            Input: king, Target: queen, Predicted: mesnel
            Input: electron, Target: proton, Predicted: monooxygenase
            Epoch 1/300, Average Loss: 0,0056945404392114785
            Input: atom, Target: molecule, Predicted: monooxygenase
            Input: king, Target: queen, Predicted: monooxygenase
            Input: electron, Target: proton, Predicted: monooxygenase
            Epoch 2/300, Average Loss: 0,005690125128082137
            Input: atom, Target: molecule, Predicted: monooxygenase
            Input: king, Target: queen, Predicted: monooxygenase
            Input: electron, Target: proton, Predicted: potassium
            Epoch 3/300, Average Loss: 0,005684164858417807
            ...
            Epoch 298/300, Average Loss: 6,215719459703083E-05
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 299/300, Average Loss: 5,764866829301166E-05
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 300/300, Average Loss: 5,3414175369772666E-05
            Model successfully decreased loss, indicating learning over epochs.
            */

            /*
            Epoch 2/300, Average Loss: 0,005691237965206645
            Input: atom, Target: molecule, Predicted: monooxygenase
            Input: king, Target: queen, Predicted: potassium
            Input: electron, Target: proton, Predicted: potassium
            Epoch 3/300, Average Loss: 0,005686187407773213
            Input: atom, Target: molecule, Predicted: potassium
            Input: king, Target: queen, Predicted: potassium
            Input: electron, Target: proton, Predicted: potassium
            Epoch 4/300, Average Loss: 0,0056788638342439406
            ....
            Epoch 158/300, Average Loss: 0,0009975577804058422
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: molecule
            Epoch 159/300, Average Loss: 0,000996129770390041
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: molecule
            Epoch 160/300, Average Loss: 0,0009946459113438198
            ...
            Epoch 296/300, Average Loss: 7,384524946352347E-06
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 297/300, Average Loss: 6,740641396188478E-06
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 298/300, Average Loss: 6,151590449217995E-06
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 299/300, Average Loss: 5,612957629348214E-06
            Input: atom, Target: molecule, Predicted: molecule
            Input: king, Target: queen, Predicted: queen
            Input: electron, Target: proton, Predicted: proton
            Epoch 300/300, Average Loss: 5,120644497280135E-06
            Model successfully decreased loss, indicating learning over epochs. 
            */

            Console.Read();
        }
    }
}
