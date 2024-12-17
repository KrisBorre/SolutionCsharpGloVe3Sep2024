using LibraryPerceptron1Sep2024;

namespace UnitTestsPerceptron1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class PerceptronTests
    {
        [Fact]
        public void Perceptron_InitializesWithCorrectNumberOfWeights()
        {
            int inputSize = 2;
            Perceptron perceptron = new Perceptron(inputSize);

            Assert.Equal(inputSize + 1, perceptron.Weights.Length); // Input weights plus one bias weight
        }

        [Fact]
        public void Train_PredictsCorrectlyForANDGate()
        {
            // Arrange
            Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);
            double[][] inputs = {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            int[] expectedOutputs = { -1, -1, -1, 1 }; // Expected outputs for AND gate

            // Act - Train for a sufficient number of epochs
            int epochs = 20;
            for (int e = 0; e < epochs; e++)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    perceptron.Train(inputs[i], expectedOutputs[i]);
                }
            }

            // Assert - Verify that predictions match expected outputs
            for (int i = 0; i < inputs.Length; i++)
            {
                int prediction = perceptron.Predict(inputs[i]);
                Assert.Equal(expectedOutputs[i], prediction);
            }
        }
             
        [Theory]
        [InlineData(new double[] { 1, 1 }, 1)]
        [InlineData(new double[] { 1, 0 }, -1)]
        [InlineData(new double[] { 0, 1 }, -1)]
        [InlineData(new double[] { 0, 0 }, -1)]
        public void Predict_ReturnsExpectedOutputForTrainedANDGate(double[] inputs, int expectedOutput)
        {
            // Arrange
            Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);
            double[][] trainingInputs = {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 }
            };
            int[] trainingOutputs = { -1, -1, -1, 1 };

            // Act - Train for a sufficient number of epochs
            int epochs = 20;
            for (int e = 0; e < epochs; e++)
            {
                for (int i = 0; i < trainingInputs.Length; i++)
                {
                    perceptron.Train(trainingInputs[i], trainingOutputs[i]);
                }
            }

            // Assert - Test prediction after training
            int prediction = perceptron.Predict(inputs);
            Assert.Equal(expectedOutput, prediction);
        }

        [Fact]
        public void Perceptron_WeightsAdjustWithTraining()
        {
            // Sometimes the test passes, sometimes it does not.

            // Arrange
            Perceptron perceptron = new Perceptron(inputSize: 2, learningRate: 0.1);
            double[] initialWeights = (double[])perceptron.Weights.Clone();
            double[] inputs = { 1, 1 };
            int expectedOutput = 1;

            // Act - Train once
            perceptron.Train(inputs, expectedOutput);

            // Assert - Weights should have changed after training
            for (int i = 0; i < initialWeights.Length; i++)
            {
                Assert.NotEqual(initialWeights[i], perceptron.Weights[i]);
            }
        }

    }
}