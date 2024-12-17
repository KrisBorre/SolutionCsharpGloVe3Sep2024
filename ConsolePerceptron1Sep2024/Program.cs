using LibraryPerceptron1Sep2024;

namespace ConsolePerceptron1Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a Perceptron with 2 inputs (for a 2D point)
            Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);

            // Training data (AND gate example)
            // Each row represents [x1, x2, expected_output]
            double[][] inputs = {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            int[] outputs = { -1, -1, -1, 1 }; // Expected outputs for AND gate (-1 and 1 for binary class labels)

            // Train the perceptron
            int epochs = 20;
            for (int e = 0; e < epochs; e++)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    perceptron.Train(inputs[i], outputs[i]);
                }
            }

            // Display weights
            perceptron.DisplayWeights();

            // Test the perceptron on new data
            Console.WriteLine("Testing perceptron:");
            double[][] testInputs = {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };

            foreach (var input in testInputs)
            {
                int prediction = perceptron.Predict(input);
                Console.WriteLine($"Input: [{input[0]}, {input[1]}], Prediction: {prediction}");
            }

            /*
            Weights:
            w[0] = -0,3661460139754797
            w[1] = 0,15740090648752025
            w[2] = 0,27945300043194826
            Testing perceptron:
            Input: [0, 0], Prediction: -1
            Input: [0, 1], Prediction: -1
            Input: [1, 0], Prediction: -1
            Input: [1, 1], Prediction: 1
            */

            /*
            Weights:
            w[0] = -0,4913767399168931
            w[1] = 0,3499971896004952
            w[2] = 0,15590227343644325
            Testing perceptron:
            Input: [0, 0], Prediction: -1
            Input: [0, 1], Prediction: -1
            Input: [1, 0], Prediction: -1
            Input: [1, 1], Prediction: 1
            */

            /*
            Weights:
            w[0] = -0,508342766292986
            w[1] = 0,5053305038188253
            w[2] = 0,14678874798269187
            Testing perceptron:
            Input: [0, 0], Prediction: -1
            Input: [0, 1], Prediction: -1
            Input: [1, 0], Prediction: -1
            Input: [1, 1], Prediction: 1
            */

            /*
            Weights:
            w[0] = -0,8939649341369262
            w[1] = 0,5752011562765198
            w[2] = 0,41951675803589344
            Testing perceptron:
            Input: [0, 0], Prediction: -1
            Input: [0, 1], Prediction: -1
            Input: [1, 0], Prediction: -1
            Input: [1, 1], Prediction: 1
            */

            Console.ReadLine();
        }
    }
}
