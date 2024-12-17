using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe4Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;

            // Initialize GloVe embeddings
            var glove = new GloveEmbeddings(filePath, embeddingDim);

            // Initialize perceptron for sentence classification
            var perceptron = new Perceptron(inputSize: embeddingDim, learningRate: 0.1);

            // Training data: sentences and their labels (1 for neural networks-related, -1 for non-neural networks-related)
            var trainingData = new List<(string sentence, int label)>
            {
                // Neural networks-related sentences
                ("a neural network consists of layers of nodes", 1),
                ("backpropagation is used to train neural networks", 1),
                ("activation functions introduce non-linearity in models", 1),
                ("deep learning involves multiple hidden layers", 1),
                ("a convolutional neural network processes images effectively", 1),
                ("recurrent neural networks are used for sequential data", 1),
                ("dropout helps prevent overfitting in neural networks", 1),
                ("gradient descent optimizes the network weights", 1),
                ("transformers are widely used in natural language processing", 1),
                ("neural networks are inspired by the human brain", 1),

                // Non-neural networks-related sentences
                ("the sun rises in the east", -1),
                ("water boils at 100 degrees Celsius", -1),
                ("trees produce oxygen during photosynthesis", -1),
                ("birds build nests to lay eggs", -1),
                ("the Great Wall of China is a famous landmark", -1),
                ("bicycles are powered by pedaling", -1),
                ("a piano has black and white keys", -1),
                ("chocolate is made from cocoa beans", -1),
                ("cars have engines that generate power", -1),
                ("mountains are formed by tectonic activity", -1)
            };

            // Train the perceptron
            for (int i = 0; i < 60; i++)
            {
                foreach (var (sentence, label) in trainingData)
                {
                    double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                    perceptron.Train(sentenceEmbedding, label);
                }
            }

            // Test the perceptron with new sentences
            var testSentences = new List<(string sentence, int expected)>
            {
                // Neural networks-related sentences
                ("feedforward networks are the simplest type of neural networks", 1),
                ("long short-term memory networks handle vanishing gradients", 1),
                ("an autoencoder compresses data into a latent representation", 1),
                ("loss functions evaluate the performance of a model", 1),
                ("hyperparameter tuning improves neural network accuracy", 1),

                // Non-neural networks-related sentences
                ("elephants are the largest land animals", -1),
                ("the Eiffel Tower is located in Paris", -1),
                ("jazz is a popular genre of music", -1),
                ("clouds are formed by water vapor condensation", -1),
                ("baking requires flour and water", -1)
            };

            foreach (var (sentence, expected) in testSentences)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                int prediction = perceptron.Predict(sentenceEmbedding);
                Console.WriteLine($"Sentence: \"{sentence}\" - Predicted: {prediction}, Expected: {expected}");
            }

            /*
            Sentence: "feedforward networks are the simplest type of neural networks" - Predicted: 1, Expected: 1
            Sentence: "long short-term memory networks handle vanishing gradients" - Predicted: 1, Expected: 1
            Sentence: "an autoencoder compresses data into a latent representation" - Predicted: 1, Expected: 1
            Sentence: "loss functions evaluate the performance of a model" - Predicted: 1, Expected: 1
            Sentence: "hyperparameter tuning improves neural network accuracy" - Predicted: 1, Expected: 1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "the Eiffel Tower is located in Paris" - Predicted: -1, Expected: -1
            Sentence: "jazz is a popular genre of music" - Predicted: 1, Expected: -1
            Sentence: "clouds are formed by water vapor condensation" - Predicted: -1, Expected: -1
            Sentence: "baking requires flour and water" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "feedforward networks are the simplest type of neural networks" - Predicted: 1, Expected: 1
            Sentence: "long short-term memory networks handle vanishing gradients" - Predicted: 1, Expected: 1
            Sentence: "an autoencoder compresses data into a latent representation" - Predicted: 1, Expected: 1
            Sentence: "loss functions evaluate the performance of a model" - Predicted: 1, Expected: 1
            Sentence: "hyperparameter tuning improves neural network accuracy" - Predicted: 1, Expected: 1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "the Eiffel Tower is located in Paris" - Predicted: 1, Expected: -1
            Sentence: "jazz is a popular genre of music" - Predicted: 1, Expected: -1
            Sentence: "clouds are formed by water vapor condensation" - Predicted: -1, Expected: -1
            Sentence: "baking requires flour and water" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "feedforward networks are the simplest type of neural networks" - Predicted: 1, Expected: 1
            Sentence: "long short-term memory networks handle vanishing gradients" - Predicted: 1, Expected: 1
            Sentence: "an autoencoder compresses data into a latent representation" - Predicted: 1, Expected: 1
            Sentence: "loss functions evaluate the performance of a model" - Predicted: -1, Expected: 1
            Sentence: "hyperparameter tuning improves neural network accuracy" - Predicted: 1, Expected: 1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "the Eiffel Tower is located in Paris" - Predicted: -1, Expected: -1
            Sentence: "jazz is a popular genre of music" - Predicted: 1, Expected: -1
            Sentence: "clouds are formed by water vapor condensation" - Predicted: -1, Expected: -1
            Sentence: "baking requires flour and water" - Predicted: -1, Expected: -1
            */

            Console.Read();
        }
    }
}
