using LibrarySimpleRNN1Oct2024;

namespace ConsoleSimpleRNN1Oct2024_ReducesLossAfterTraining
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            double learningRate = 0.5; // 0.001;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize, learningRate);
            double[] input = { 0.5, 0.1, -0.2 };
            double[] target = { 0.3, 0.7 };

            double[] initialOutput = rnn.Forward(input);

            double initialLoss = CalculateLoss(initialOutput, target);

            // Train for a few epochs
            for (int epoch = 0; epoch < 10; epoch++)
            {
                var output = rnn.Forward(input);
                rnn.Backward(output, target, input);
            }

            double[] finalOutput = rnn.Forward(input);

            double finalLoss = CalculateLoss(finalOutput, target);

            Console.WriteLine($"Final loss ({finalLoss}) is less than initial loss ({initialLoss})");

            // double learningRate = 0.001;
            // Final loss (0,5799232979328705) is less than initial loss (0,5799291908208358)
            // Final loss (0,579840270983692) is less than initial loss (0,5798497385940697)
            // Final loss (0,5798336442861626) is less than initial loss (0,5798429445616059)

            // double learningRate = 0.5;
            // Final loss (1,9644583122050297E-05) is less than initial loss (0,5799571906612168)
            // Final loss (9,083990512637309E-06) is less than initial loss (0,579943167308828)
            // Final loss (2,2895517686102655E-05) is less than initial loss (0,5798974755606429)
        }

        private static double CalculateLoss(double[] output, double[] target)
        {
            double loss = 0.0;
            for (int i = 0; i < output.Length; i++)
            {
                loss += Math.Pow(output[i] - target[i], 2);
            }
            return loss;
        }
    }
}
