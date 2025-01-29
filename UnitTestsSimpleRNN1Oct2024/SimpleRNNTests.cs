using LibrarySimpleRNN1Oct2024;

namespace UnitTestsSimpleRNN1Oct2024
{
    public class SimpleRNNTests
    {
        [Fact]
        public void Constructor_InitializesMatricesWithExpectedDimensions()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;

            // Act
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize);

            // Assert
            Assert.Equal(inputSize, rnn.Wx.GetLength(0));
            Assert.Equal(hiddenSize, rnn.Wx.GetLength(1));
            Assert.Equal(hiddenSize, rnn.Wh.GetLength(0));
            Assert.Equal(outputSize, rnn.Wh.GetLength(1));
            Assert.Equal(hiddenSize, rnn.Bh.Length);
            Assert.Equal(hiddenSize, rnn.h.Length);
        }

        [Fact]
        public void Forward_ReturnsOutputOfExpectedLength()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize);
            double[] input = { 0.5, 0.1, -0.2 };

            // Act
            var output = rnn.Forward(input);

            // Assert
            Assert.Equal(outputSize, output.Length);
        }

        [Fact]
        public void Forward_GeneratesNonZeroOutput()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize);
            double[] input = { 0.5, 0.1, -0.2 };

            // Act
            var output = rnn.Forward(input);

            // Assert
            Assert.All(output, value => Assert.NotEqual(0, value));
        }

        [Fact]
        public void Backward_UpdatesWeights()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            double learningRate = 0.01;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize, learningRate);
            double[] input = { 0.5, 0.1, -0.2 };
            double[] target = { 0.3, 0.7 };
            double[] initialOutput = rnn.Forward(input);

            // Capture initial weights
            var initialWh = (double[,])rnn.Wh.Clone();
            var initialWx = (double[,])rnn.Wx.Clone();

            // Act
            rnn.Backward(initialOutput, target, input);

            // Assert: Check weights are updated
            bool weightsUpdated = false;
            for (int j = 0; j < hiddenSize; j++)
            {
                for (int k = 0; k < outputSize; k++)
                {
                    if (initialWh[j, k] != rnn.Wh[j, k])
                    {
                        weightsUpdated = true;
                        break;
                    }
                }
            }
            Assert.True(weightsUpdated, "Weights in Wh should be updated after Backward.");

            weightsUpdated = false;
            for (int i = 0; i < inputSize; i++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    if (initialWx[i, j] != rnn.Wx[i, j])
                    {
                        weightsUpdated = true;
                        break;
                    }
                }
            }
            Assert.True(weightsUpdated, "Weights in Wx should be updated after Backward.");
        }

        [Fact]
        public void Backward_UpdatesBiases()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            double learningRate = 0.01;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize, learningRate);
            double[] input = { 0.5, 0.1, -0.2 };
            double[] target = { 0.3, 0.7 };
            double[] initialOutput = rnn.Forward(input);

            var initialBh = (double[])rnn.Bh.Clone();            

            // Act
            rnn.Backward(initialOutput, target, input);

            // Assert: Check biases are updated
            bool biasesUpdated = false;
            for (int j = 0; j < hiddenSize; j++)
            {
                if (initialBh[j] != rnn.Bh[j])
                {
                    biasesUpdated = true;
                    break;
                }
            }
            Assert.True(biasesUpdated, "Biases in Bh should be updated after Backward.");
        }

        [Fact]
        public void Backward_ReducesLossAfterTraining()
        {
            // Arrange
            int inputSize = 3;
            int hiddenSize = 4;
            int outputSize = 2;
            var rnn = new SimpleRNN(inputSize, hiddenSize, outputSize);
            double[] input = { 0.5, 0.1, -0.2 };
            double[] target = { 0.3, 0.7 };

            double[] initialOutput = rnn.Forward(input);

            // Act
            double initialLoss = CalculateLoss(initialOutput, target);

            // Train for a few epochs
            for (int epoch = 0; epoch < 10; epoch++)
            {
                var output = rnn.Forward(input);
                rnn.Backward(output, target, input);
            }

            double[] finalOutput = rnn.Forward(input);

            double finalLoss = CalculateLoss(finalOutput, target);

            // Assert
            Assert.True(finalLoss < initialLoss, $"Expected final loss ({finalLoss}) to be less than initial loss ({initialLoss})");
        }

        private double CalculateLoss(double[] output, double[] target)
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