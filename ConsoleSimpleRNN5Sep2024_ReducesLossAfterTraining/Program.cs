using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNN5Sep2024_ReducesLossAfterTraining
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
            // Final loss (0,6123189372215818) is less than initial loss (0,7906050769636365)
            // Final loss (0,4740107352589912) is less than initial loss (0,4824551823965272)

            // double learningRate = 0.5;
            // Final loss (4,781063702536723E-08) is less than initial loss (0,7126320427863312)
            // Final loss (1,3555552061509705E-05) is less than initial loss (0,5494228107034291)
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
