using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNN2Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 5;
            int outputSize = 1;
            double learningRate = 0.3; // 0.5; // 0.8; // 0.01; 
            int epochs = 10;

            SimpleRNN rnn = new SimpleRNN(inputSize, hiddenSize, outputSize, learningRate);

            double[][] inputs = new double[][]
            {
                new double[] { 1, 0, 0 },
                new double[] { 0, 1, 0 },
                new double[] { 0, 0, 1 }
            };

            double[][] targets = new double[][]
            {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 }
            };

            // Track initial and final losses
            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;
            double overallLoss = initialLoss;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] output = rnn.Forward(inputs[i]);
                    double loss = 0;

                    for (int j = 0; j < output.Length; j++)
                    {
                        loss += Math.Pow(output[j] - targets[i][j], 2);
                    }

                    totalLoss += loss;
                    rnn.Backward(output, targets[i], inputs[i]);
                }

                totalLoss /= inputs.Length;

                // Track progress by calculating total loss and checking for an overall loss decrease
                if (epoch == 0) initialLoss = totalLoss;
                overallLoss = totalLoss;
                lastLoss = totalLoss;

                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
            }

            if (overallLoss >= initialLoss)
            {
                Console.WriteLine("Overall loss did not decrease, indicating lack of learning.");
            }

            // double learningRate = 0.01;
            /*
            Epoch 1/10, Loss: 0,5826275398746659
            Epoch 2/10, Loss: 0,503091009285829
            Epoch 3/10, Loss: 0,4409128247770176
            Epoch 4/10, Loss: 0,38784451000106684
            Epoch 5/10, Loss: 0,3424166725641018
            Epoch 6/10, Loss: 0,30353250637855406
            Epoch 7/10, Loss: 0,27026452284959857
            Epoch 8/10, Loss: 0,24181795606832687
            Epoch 9/10, Loss: 0,2175041280202287
            Epoch 10/10, Loss: 0,19672397708987374
            finalOutput[0]= 0,17404711490129623
            finalOutput[1]= 0,4430784250349733
            finalOutput[2]= 0,5541121413315737
            */

            //double learningRate = 0.8;
            /*
            Epoch 1/10, Loss: 1,0248232899323135
            Epoch 2/10, Loss: 426,4190249268233
            Epoch 3/10, Loss: 872828,0118357181
            Epoch 4/10, Loss: 128860256,13453953
            Epoch 5/10, Loss: 3369730568746,9614
            Epoch 6/10, Loss: 30056618998279572
            Epoch 7/10, Loss: 2,450313172747696E+20
            Epoch 8/10, Loss: 1,9838466838998546E+24
            Epoch 9/10, Loss: 1,6053127191231533E+28
            Epoch 10/10, Loss: 1,2989508543140973E+32
            Overall loss did not decrease, indicating lack of learning.
            finalOutput[0]= 86223869994516000
            finalOutput[1]= -1,654959399857283E+17
            finalOutput[2]= 86223869994516000
            */

            // double learningRate = 0.5;
            /*
            Epoch 1/10, Loss: 0,5717819826910246
            Epoch 2/10, Loss: 1,0767671032730846
            Epoch 3/10, Loss: 0,7490684845056016
            Epoch 4/10, Loss: 1,3878982505628172
            Epoch 5/10, Loss: 191,23770806222248
            Epoch 6/10, Loss: 53228,731029736
            Epoch 7/10, Loss: 876239,5653958448
            Epoch 8/10, Loss: 996607037,5752574
            Epoch 9/10, Loss: 7278874588,769147
            Epoch 10/10, Loss: 14496599287553,803
            Overall loss did not decrease, indicating lack of learning.
            finalOutput[0]= 13582562,078128057
            finalOutput[1]= -30911453,604034975
            finalOutput[2]= 13582562,078128057
            */

            for (int i = 0; i < inputs.Length; i++)
            {
                double[] finalOutput = rnn.Forward(inputs[i]);
                Console.Write($"finalOutput[{i}]= ");
                for (int j = 0; j < finalOutput.Length; j++)
                {
                    Console.Write($"{finalOutput[j]} ");
                }
                Console.WriteLine();
            }

            // double learningRate = 0.3;
            /*
            Epoch 1/10, Loss: 0,1986092866967958
            Epoch 2/10, Loss: 0,13091912495055844
            Epoch 3/10, Loss: 0,12160687265864338
            Epoch 4/10, Loss: 0,045288585189677294
            Epoch 5/10, Loss: 0,07005271403828155
            Epoch 6/10, Loss: 0,04218012913907653
            Epoch 7/10, Loss: 0,017381911631147818
            Epoch 8/10, Loss: 0,010148760136291382
            Epoch 9/10, Loss: 0,005417128048717391
            Epoch 10/10, Loss: 0,002399375774119894
            finalOutput[0]= -0,0523180007990795
            finalOutput[1]= 0,9929965199599254
            finalOutput[2]= 0,9854621265210138 
            */

            Console.ReadLine();
        }
    }
}
