using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe6Sep2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50; // Assuming GloVe's 50-dimensional embeddings

            // Initialize GloVe embeddings
            var glove = new GloveEmbeddings(filePath, embeddingDim);

            // Initialize perceptron for sentence classification
            var perceptron = new Perceptron(inputSize: embeddingDim, learningRate: 0.1);

            // Training data: sentences and their labels (1 for star schema-related, -1 for non-related)
            var trainingData = new List<(string sentence, int label)>
            {
                // Star Schema & Dimensional Modeling sentences
                ("a fact table contains measures and foreign keys", 1),
                ("dimension tables store descriptive attributes", 1),
                ("star schema uses denormalized data", 1),
                ("snowflake schema normalizes dimensions", 1),
                ("data warehouses aggregate transactional data", 1),
                ("ETL processes extract, transform, and load data", 1),
                ("fact tables support analytic queries", 1),
                ("dimensions describe the context of facts", 1),
                ("time dimension is commonly used in star schema", 1),
                ("dimensional modeling improves query performance", 1),
                ("surrogate keys are used in dimensional modeling", 1),
                ("slowly changing dimensions track changes over time", 1),
                ("data marts are subsets of data warehouses", 1),
                ("hierarchies in dimensions simplify reporting", 1),
                ("a conformed dimension is shared across schemas", 1),

                // Non-related sentences
                ("dogs are loyal companions", -1),
                ("trees produce oxygen", -1),
                ("books can transport readers to imaginary worlds", -1),
                ("baking bread requires yeast", -1),
                ("the sky is blue on a clear day", -1),
                ("mountains are formed by tectonic activity", -1),
                ("rivers flow into oceans", -1),
                ("cats are popular pets", -1),
                ("the piano is a popular musical instrument", -1),
                ("hiking is a popular outdoor activity", -1),
            };

            // Training loop
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
                // Star Schema & Dimensional Modeling
                ("star schema optimizes query performance", 1),
                ("ETL processes ensure data consistency", 1),
                ("fact tables are connected to multiple dimensions", 1),
                ("dimensional modeling supports business intelligence", 1),
                ("data warehouses enable historical analysis", 1),

                // Non-related sentences
                ("roses are often given as gifts on Valentine's Day", -1),
                ("sunflowers turn toward the sun", -1),
                ("chocolate is made from cocoa beans", -1),
                ("traveling by airplane is faster than by train", -1),
                ("elephants are the largest land animals", -1)
            };

            foreach (var (sentence, expected) in testSentences)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                int prediction = perceptron.Predict(sentenceEmbedding);
                Console.WriteLine($"Sentence: \"{sentence}\" - Predicted: {prediction}, Expected: {expected}");
            }

            /*
            Sentence: "star schema optimizes query performance" - Predicted: 1, Expected: 1
            Sentence: "ETL processes ensure data consistency" - Predicted: 1, Expected: 1
            Sentence: "fact tables are connected to multiple dimensions" - Predicted: 1, Expected: 1
            Sentence: "dimensional modeling supports business intelligence" - Predicted: 1, Expected: 1
            Sentence: "data warehouses enable historical analysis" - Predicted: 1, Expected: 1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: -1, Expected: -1
            Sentence: "sunflowers turn toward the sun" - Predicted: -1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "traveling by airplane is faster than by train" - Predicted: -1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            */

            Console.Read();
        }
    }
}
