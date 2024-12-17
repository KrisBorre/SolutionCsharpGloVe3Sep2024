using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe2Sep2024
{
    internal class Program
    {
        /// <summary>
        /// ChatGPT
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50; // Assuming GloVe's 50-dimensional embeddings

            // Initialize GloVe embeddings
            var glove = new GloveEmbeddings(filePath, embeddingDim);

            // Initialize perceptron for sentence classification
            var perceptron = new Perceptron(inputSize: embeddingDim, learningRate: 0.1);

            // Training data: sentences and their labels (1 for physics-related, -1 for non-physics)
            var trainingData = new List<(string sentence, int label)>
            {
                // Physics-related sentences
                ("electrons orbit the nucleus", 1),
                ("gravity attracts objects", 1),
                ("stars are distant suns", 1),
                ("light travels in waves", 1),
                ("force equals mass times acceleration", 1),
                ("energy can neither be created nor destroyed", 1),
                ("a black hole traps light within its event horizon", 1),
                ("the speed of light is approximately 300,000 km per second", 1),
                ("an object in motion stays in motion unless acted upon by an external force", 1),
                ("thermal energy is transferred through conduction, convection, or radiation", 1),
                ("atoms are the building blocks of matter", 1),
                ("the second law of thermodynamics states that entropy always increases", 1),
                ("magnetic fields are produced by moving charges", 1),
                ("quarks combine to form protons and neutrons", 1),
                ("the Doppler effect describes the change in frequency due to relative motion", 1),
                ("plasma is a state of matter found in stars", 1),
                ("momentum is conserved in a closed system", 1),
                ("the Higgs boson gives particles their mass", 1),
                ("sound requires a medium to travel through", 1),
                ("radio waves are part of the electromagnetic spectrum", 1),

                // Non-physics-related sentences
                ("cats are popular pets", -1),
                ("computers run software", -1),
                ("the sky is blue on a clear day", -1),
                ("trees produce oxygen", -1),
                ("baking bread requires yeast", -1),
                ("books can transport readers to imaginary worlds", -1),
                ("dogs are loyal companions", -1),
                ("the ocean is vast and full of life", -1),
                ("the capital of France is Paris", -1),
                ("pasta is a staple food in Italian cuisine", -1),
                ("basketball is played with a hoop and a ball", -1),
                ("smartphones have touchscreens", -1),
                ("paintings can evoke powerful emotions", -1),
                ("the seasons change throughout the year", -1),
                ("coffee contains caffeine", -1),
                ("jazz is a genre of music", -1),
                ("sunflowers turn toward the sun", -1),
                ("traveling by airplane is faster than by train", -1),
                ("clouds are made of tiny water droplets", -1),
                ("mountains are formed by tectonic activity", -1)
            };

            for (int i = 0; i < 50; i++)
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
                // Physics-related sentences
                ("light bends when passing through a prism", 1),
                ("the sun generates energy through nuclear fusion", 1),
                ("electromagnetic waves do not require a medium", 1),
                ("the law of conservation of momentum applies in collisions", 1),
                ("a vacuum contains no air or matter", 1),
                ("satellites stay in orbit due to gravity and velocity balance", 1),
                ("neutrons have no electrical charge", 1),
                ("laser light is coherent and monochromatic", 1),
                ("the universe is expanding over time", 1),
                ("heat flows from hot objects to cold objects", 1),

                // Non-physics-related sentences
                ("birds build nests to lay their eggs", -1),
                ("the piano is a popular musical instrument", -1),
                ("chocolate is made from cocoa beans", -1),
                ("a rainy day is perfect for reading books", -1),
                ("rivers flow into oceans", -1),
                ("modern cars often have automatic transmission", -1),
                ("roses are often given as gifts on Valentine's Day", -1),
                ("elephants are the largest land animals", -1),
                ("a balanced diet is important for health", -1),
                ("hiking is a popular outdoor activity", -1)
            };

            foreach (var (sentence, expected) in testSentences)
            {
                double[] sentenceEmbedding = glove.ComputeSentenceEmbedding(sentence);
                int prediction = perceptron.Predict(sentenceEmbedding);
                Console.WriteLine($"Sentence: \"{sentence}\" - Predicted: {prediction}, Expected: {expected}");
            }

            /*
            Sentence: "light bends when passing through a prism" - Predicted: 1, Expected: 1
            Sentence: "the sun generates energy through nuclear fusion" - Predicted: 1, Expected: 1
            Sentence: "electromagnetic waves do not require a medium" - Predicted: 1, Expected: 1
            Sentence: "the law of conservation of momentum applies in collisions" - Predicted: 1, Expected: 1
            Sentence: "a vacuum contains no air or matter" - Predicted: 1, Expected: 1
            Sentence: "satellites stay in orbit due to gravity and velocity balance" - Predicted: 1, Expected: 1
            Sentence: "neutrons have no electrical charge" - Predicted: 1, Expected: 1
            Sentence: "laser light is coherent and monochromatic" - Predicted: 1, Expected: 1
            Sentence: "the universe is expanding over time" - Predicted: 1, Expected: 1
            Sentence: "heat flows from hot objects to cold objects" - Predicted: 1, Expected: 1
            Sentence: "birds build nests to lay their eggs" - Predicted: -1, Expected: -1
            Sentence: "the piano is a popular musical instrument" - Predicted: -1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "a rainy day is perfect for reading books" - Predicted: -1, Expected: -1
            Sentence: "rivers flow into oceans" - Predicted: 1, Expected: -1
            Sentence: "modern cars often have automatic transmission" - Predicted: -1, Expected: -1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: -1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "a balanced diet is important for health" - Predicted: -1, Expected: -1
            Sentence: "hiking is a popular outdoor activity" - Predicted: -1, Expected: -1
            */

            Console.Read();
        }
    }
}
