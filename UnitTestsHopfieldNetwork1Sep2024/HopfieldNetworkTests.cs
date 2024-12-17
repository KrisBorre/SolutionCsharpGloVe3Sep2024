using LibraryHopfieldNetwork1Sep2024;

namespace UnitTestsHopfieldNetwork1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class HopfieldNetworkTests
    {       
        [Fact]
        public void Retrieve_ShouldStabilizeToTrainedPattern()
        {
            // Arrange
            int size = 5;
            HopfieldNetwork network = new HopfieldNetwork(size);
            int[] pattern = new int[] { 1, -1, 1, -1, 1 };
            network.Train(new List<int[]> { pattern });

            // Act
            int[] noisyPattern = HopfieldNetwork.AddNoise(pattern, 0.2);
            int[] retrievedPattern = network.Retrieve(noisyPattern);

            // Assert
            Assert.Equal(pattern, retrievedPattern);
        }

        [Fact]
        public void AddNoise_ShouldIntroduceExpectedNoise()
        {
            // Arrange
            int[] pattern = new int[] { 1, -1, 1, -1, 1 };
            double temperature = 0.5;
            int iterations = 1000;
            double expectedNoiseLevel = temperature;

            // Act
            int flips = 0;
            for (int i = 0; i < iterations; i++)
            {
                int[] noisyPattern = HopfieldNetwork.AddNoise(pattern, temperature);
                flips += pattern.Zip(noisyPattern, (original, noisy) => original != noisy ? 1 : 0).Sum();
            }

            double actualNoiseLevel = (double)flips / (pattern.Length * iterations);

            // Assert
            Assert.InRange(actualNoiseLevel, expectedNoiseLevel - 0.05, expectedNoiseLevel + 0.05);
        }

        [Fact]
        public void CalculateAccuracy_ShouldReturnCorrectProportion()
        {
            // Arrange
            int[] original = new int[] { 1, -1, 1, -1, 1 };
            int[] retrieved = new int[] { 1, -1, -1, -1, 1 };

            // Act
            double accuracy = HopfieldNetwork.CalculateAccuracy(original, retrieved);

            // Assert
            Assert.Equal(0.8, accuracy, 1);
        }      
    }
}
