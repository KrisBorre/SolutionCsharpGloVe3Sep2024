using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe3Sep2024
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

            // Training data: sentences and their labels (1 for genetics-related, -1 for non-genetics-related)
            var trainingData = new List<(string sentence, int label)>
            {
                // Genetics-related sentences
                ("DNA is the blueprint of life", 1),
                ("genes are inherited from parents", 1),
                ("mutations can lead to genetic disorders", 1),
                ("RNA is transcribed from DNA", 1),
                ("chromosomes carry genetic information", 1),
                ("genetic engineering allows modification of organisms", 1),
                ("the human genome consists of 23 pairs of chromosomes", 1),
                ("heredity explains the passing of traits", 1),
                ("CRISPR is a powerful gene-editing tool", 1),
                ("Mendel's experiments laid the foundation of genetics", 1),

                // Non-genetics-related sentences
                ("computers can process vast amounts of data", -1),
                ("a healthy diet includes vegetables", -1),
                ("the Eiffel Tower is in Paris", -1),
                ("clouds are made of condensed water vapor", -1),
                ("mountains are formed by tectonic activity", -1),
                ("dogs are loyal animals", -1),
                ("a piano has black and white keys", -1),
                ("the ocean is vast and mysterious", -1),
                ("the sky is blue on sunny days", -1),
                ("birds build nests in trees", -1)
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
                // Genetics-related sentences
                ("genetic mutations can be beneficial or harmful", 1),
                ("inheritance patterns determine how traits are passed", 1),
                ("gene therapy is a promising field in medicine", 1),
                ("proteins are synthesized based on RNA templates", 1),
                ("epigenetics studies changes in gene expression", 1),

                // Non-genetics-related sentences
                ("cats are domesticated animals", -1),
                ("the sun sets in the west", -1),
                ("coffee is brewed from roasted beans", -1),
                ("bicycles have two wheels", -1),
                ("flowers bloom in the spring", -1)
            };

            foreach (var (sentence, expected) in testSentences)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                int prediction = perceptron.Predict(sentenceEmbedding);
                Console.WriteLine($"Sentence: \"{sentence}\" - Predicted: {prediction}, Expected: {expected}");
            }

            /*
            Sentence: "genetic mutations can be beneficial or harmful" - Predicted: 1, Expected: 1
            Sentence: "inheritance patterns determine how traits are passed" - Predicted: 1, Expected: 1
            Sentence: "gene therapy is a promising field in medicine" - Predicted: 1, Expected: 1
            Sentence: "proteins are synthesized based on RNA templates" - Predicted: 1, Expected: 1
            Sentence: "epigenetics studies changes in gene expression" - Predicted: 1, Expected: 1
            Sentence: "cats are domesticated animals" - Predicted: 1, Expected: -1
            Sentence: "the sun sets in the west" - Predicted: -1, Expected: -1
            Sentence: "coffee is brewed from roasted beans" - Predicted: -1, Expected: -1
            Sentence: "bicycles have two wheels" - Predicted: -1, Expected: -1
            Sentence: "flowers bloom in the spring" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "genetic mutations can be beneficial or harmful" - Predicted: 1, Expected: 1
            Sentence: "inheritance patterns determine how traits are passed" - Predicted: 1, Expected: 1
            Sentence: "gene therapy is a promising field in medicine" - Predicted: 1, Expected: 1
            Sentence: "proteins are synthesized based on RNA templates" - Predicted: -1, Expected: 1
            Sentence: "epigenetics studies changes in gene expression" - Predicted: 1, Expected: 1
            Sentence: "cats are domesticated animals" - Predicted: -1, Expected: -1
            Sentence: "the sun sets in the west" - Predicted: -1, Expected: -1
            Sentence: "coffee is brewed from roasted beans" - Predicted: -1, Expected: -1
            Sentence: "bicycles have two wheels" - Predicted: -1, Expected: -1
            Sentence: "flowers bloom in the spring" - Predicted: -1, Expected: -1 
            */

            /*
            Sentence: "genetic mutations can be beneficial or harmful" - Predicted: 1, Expected: 1
            Sentence: "inheritance patterns determine how traits are passed" - Predicted: 1, Expected: 1
            Sentence: "gene therapy is a promising field in medicine" - Predicted: 1, Expected: 1
            Sentence: "proteins are synthesized based on RNA templates" - Predicted: 1, Expected: 1
            Sentence: "epigenetics studies changes in gene expression" - Predicted: 1, Expected: 1
            Sentence: "cats are domesticated animals" - Predicted: -1, Expected: -1
            Sentence: "the sun sets in the west" - Predicted: -1, Expected: -1
            Sentence: "coffee is brewed from roasted beans" - Predicted: -1, Expected: -1
            Sentence: "bicycles have two wheels" - Predicted: -1, Expected: -1
            Sentence: "flowers bloom in the spring" - Predicted: -1, Expected: -1
            */

            Console.Read();
        }
    }
}
