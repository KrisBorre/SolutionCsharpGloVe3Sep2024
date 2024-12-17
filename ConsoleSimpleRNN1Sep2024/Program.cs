namespace ConsoleSimpleRNN1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // Simple example with input size = 2, hidden size = 5, output size = 1
            var rnn = new SimpleRNN(inputSize: 2, hiddenSize: 5, outputSize: 1, learningRate: 0.01);

            // Toy data: sequence of inputs and targets
            double[][] inputs = {
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 },
                new double[] { 0, 0 }
            };

            double[][] targets = {
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 0 }
            };

            rnn.Train(inputs, targets, epochs: 1000);

            // Test predictions
            foreach (var input in inputs)
            {
                var output = rnn.Forward(input);
                Console.WriteLine($"Input: {string.Join(",", input)}, Output: {string.Join(",", output)}");
            }

            /*
            Epoch 995/1000, Loss: 2,478998196908533E-09
            Epoch 996/1000, Loss: 2,444368436450347E-09
            Epoch 997/1000, Loss: 2,410222754066172E-09
            Epoch 998/1000, Loss: 2,376554375538811E-09
            Epoch 999/1000, Loss: 2,3433566216430713E-09
            Epoch 1000/1000, Loss: 2,310622906841264E-09
            Input: 0,1, Output: 1,0000286325855152
            Input: 1,0, Output: 2,2170002023882973E-05
            Input: 1,1, Output: 0,999974071711262
            Input: 0,0, Output: -1,0536037601663328E-05
            */

            /*
            Epoch 996/1000, Loss: 1,7926031011538282E-05
            Epoch 997/1000, Loss: 1,7897798183308133E-05
            Epoch 998/1000, Loss: 1,7869610934442953E-05
            Epoch 999/1000, Loss: 1,7841469177529964E-05
            Epoch 1000/1000, Loss: 1,781337282553445E-05
            Input: 0,1, Output: 1,0023277098332826
            Input: 1,0, Output: 0,001984844622104999
            Input: 1,1, Output: 0,9980862195485308
            Input: 0,0, Output: -0,0016979191338942975
            */

            /*
            Epoch 995/1000, Loss: 4,289197340829616E-06
            Epoch 996/1000, Loss: 4,272840504262611E-06
            Epoch 997/1000, Loss: 4,25654574587892E-06
            Epoch 998/1000, Loss: 4,240312832403011E-06
            Epoch 999/1000, Loss: 4,224141531406679E-06
            Epoch 1000/1000, Loss: 4,208031611313799E-06
            Input: 0,1, Output: 0,9988464038137881
            Input: 1,0, Output: -0,0009737537283778202
            Input: 1,1, Output: 1,0009281456910895
            Input: 0,0, Output: 0,0008054607043177825 
            */

            Console.ReadLine();
        }
    }
}
