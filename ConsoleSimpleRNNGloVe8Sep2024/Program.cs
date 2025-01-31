using LibrarySimpleRNN1Sep2024;
using LibraryGlobalVectors1Sep2024;

namespace ConsoleSimpleRNNGloVe8Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrange GloVe loader and RNN parameters
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 1;
            double learningRate = 0.1;
            int epochs = 15;

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

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {loss}");
                string predictedWord = glove.FindClosestWord(output);
                Console.WriteLine(predictedWord);

                lastLoss = loss;
            }

            // Assert that loss decreased over time, indicating learning
            if (lastLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            /*
            Epoch 1/15, Loss: 0,0063124555642087255
            staunchly
            Epoch 2/15, Loss: 0,00400642170182355
            molecule
            Epoch 3/15, Loss: 0,002605304078549106
            molecule
            Epoch 4/15, Loss: 0,001702530090039788
            molecule
            Epoch 5/15, Loss: 0,0011519357370613887
            molecule
            Epoch 6/15, Loss: 0,0008252371913821953
            molecule
            Epoch 7/15, Loss: 0,0006594640975105126
            molecule
            Epoch 8/15, Loss: 0,000615381794499664
            molecule
            Epoch 9/15, Loss: 0,0006790830470069856
            molecule
            Epoch 10/15, Loss: 0,0008586729928861048
            molecule
            Epoch 11/15, Loss: 0,001177362924578907
            molecule
            Epoch 12/15, Loss: 0,0016982574881982066
            molecule
            Epoch 13/15, Loss: 0,0025012724669143003
            molecule
            Epoch 14/15, Loss: 0,003772531784827086
            molecule
            Epoch 15/15, Loss: 0,00573469315143158
            binds
            */


            /*
            Epoch 1/15, Loss: 0,020425529459146153
            hikers
            Epoch 2/15, Loss: 0,0048993157381844385
            collagen
            Epoch 3/15, Loss: 0,0025692415551675624
            molecule
            Epoch 4/15, Loss: 0,0015345651447577263
            molecule
            Epoch 5/15, Loss: 0,0009590186782943058
            molecule
            Epoch 6/15, Loss: 0,0006088682743529735
            molecule
            Epoch 7/15, Loss: 0,0003885945734418796
            molecule
            Epoch 8/15, Loss: 0,00024844405424739516
            molecule
            Epoch 9/15, Loss: 0,0001589323807948152
            molecule
            Epoch 10/15, Loss: 0,00010169002322108167
            molecule
            Epoch 11/15, Loss: 6,506864863223861E-05
            molecule
            Epoch 12/15, Loss: 4,163648418219015E-05
            molecule
            Epoch 13/15, Loss: 2,6642765670739068E-05
            molecule
            Epoch 14/15, Loss: 1,704847331615923E-05
            molecule
            Epoch 15/15, Loss: 1,0909177498451106E-05
            molecule
            */


            /*
            Epoch 1/15, Loss: 0,006867565393167232
            bulimia
            Epoch 2/15, Loss: 0,003965364622119389
            molecule
            Epoch 3/15, Loss: 0,002607408080655065
            molecule
            Epoch 4/15, Loss: 0,0016354996377813832
            molecule
            Epoch 5/15, Loss: 0,001038985532479005
            molecule
            Epoch 6/15, Loss: 0,0006671580393185999
            molecule
            Epoch 7/15, Loss: 0,0004237972008950777
            molecule
            Epoch 8/15, Loss: 0,00027180128739038515
            molecule
            Epoch 9/15, Loss: 0,0001735279995263876
            molecule
            Epoch 10/15, Loss: 0,00011101036968414165
            molecule
            Epoch 11/15, Loss: 7,102150065452039E-05
            molecule
            Epoch 12/15, Loss: 4,541679653638289E-05
            molecule
            Epoch 13/15, Loss: 2,906074901252855E-05
            molecule
            Epoch 14/15, Loss: 1,858839183546644E-05
            molecule
            Epoch 15/15, Loss: 1,189228602716713E-05
            molecule
            */


            /*
            Epoch 1/15, Loss: 0,006715073093483868
            palace
            Epoch 2/15, Loss: 0,003775991816299171
            molecule
            Epoch 3/15, Loss: 0,002404247142981917
            molecule
            Epoch 4/15, Loss: 0,001529398382577806
            molecule
            Epoch 5/15, Loss: 0,0009763567958691243
            molecule
            Epoch 6/15, Loss: 0,0006235788887258655
            molecule
            Epoch 7/15, Loss: 0,00039835902635371477
            molecule
            Epoch 8/15, Loss: 0,00025449048597818885
            molecule
            Epoch 9/15, Loss: 0,00016258364609138463
            molecule
            Epoch 10/15, Loss: 0,00010386805286813129
            molecule
            Epoch 11/15, Loss: 6,635714814567196E-05
            molecule
            Epoch 12/15, Loss: 4,23928926438071E-05
            molecule
            Epoch 13/15, Loss: 2,7083097712558217E-05
            molecule
            Epoch 14/15, Loss: 1,730228531869497E-05
            molecule
            Epoch 15/15, Loss: 1,1053722169098435E-05
            molecule 
            */


            /*
            Epoch 1/15, Loss: 0,007676078612727523
            depressed
            Epoch 2/15, Loss: 0,004539417684286774
            helix
            Epoch 3/15, Loss: 0,0029663192379243875
            molecules
            Epoch 4/15, Loss: 0,0018929680331670298
            molecule
            Epoch 5/15, Loss: 0,0012197919131963603
            molecule
            Epoch 6/15, Loss: 0,000784356781916654
            molecule
            Epoch 7/15, Loss: 0,0005056839848633312
            molecule
            Epoch 8/15, Loss: 0,0003258807888235435
            molecule
            Epoch 9/15, Loss: 0,00021035721900314582
            molecule
            Epoch 10/15, Loss: 0,00013575992088596834
            molecule
            Epoch 11/15, Loss: 8,773334130375808E-05
            molecule
            Epoch 12/15, Loss: 5,669382314724619E-05
            molecule
            Epoch 13/15, Loss: 3,6678066333298944E-05
            molecule
            Epoch 14/15, Loss: 2,3730953723690085E-05
            molecule
            Epoch 15/15, Loss: 1,536994716721908E-05
            molecule 
            */

            Console.Read();
        }
    }
}
