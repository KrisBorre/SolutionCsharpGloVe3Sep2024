using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe1Sep2024_200d
{
    internal class Program
    {
        /// <summary>
        /// ChatGPT
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.200d.txt";
            int embeddingDim = 200;

            // Initialize GloVe embeddings
            var glove = new GloveEmbeddings(filePath, embeddingDim);

            // Initialize perceptron for sentence classification
            var perceptron = new Perceptron(inputSize: embeddingDim, learningRate: 0.1);

            // Training data: sentences and their labels (1 for physics-related, -1 for non-physics)
            var trainingData = new List<(string sentence, int label)>
            {
                ("electrons orbit the nucleus", 1),
                ("gravity attracts objects", 1),
                ("stars are distant suns", 1),
                ("cats are popular pets", -1),
                ("computers run software", -1)
            };

            foreach (var (sentence, label) in trainingData)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                perceptron.Train(sentenceEmbedding, label);
            }

            // Test the perceptron with new sentences
            var testSentences = new List<(string sentence, int expected)>
            {
                ("planets revolve around stars", 1),
                ("dogs are loyal animals", -1)
            };

            foreach (var (sentence, expected) in testSentences)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                int prediction = perceptron.Predict(sentenceEmbedding);
                Console.WriteLine($"Sentence: \"{sentence}\" - Predicted: {prediction}, Expected: {expected}");
            }

            /*
            Sentence: "planets revolve around stars" - Predicted: 1, Expected: 1
            Sentence: "dogs are loyal animals" - Predicted: -1, Expected: -1 
            */

            Console.Read();
        }
    }
}
