
using LibraryGlobalVectors1Sep2024;
using LibraryPerceptron1Sep2024;

namespace ConsolePerceptronGloVe5Sep2024
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

            // Training data: sentences and their labels (1 for Python-related, -1 for non-Python)
            var trainingData = new List<(string sentence, int label)>
            {
                // Python-related sentences
                ("Python is an interpreted language", 1),
                ("Python supports multiple programming paradigms", 1),
                ("Python has a simple and easy-to-read syntax", 1),
                ("Python is widely used in data science and machine learning", 1),
                ("Python's standard library includes modules for various tasks", 1),
                ("You can use Python for web development with frameworks like Django", 1),
                ("Python has dynamic typing and automatic memory management", 1),
                ("Python is often used for automation tasks", 1),
                ("List comprehensions in Python provide a concise way to create lists", 1),
                ("Python supports object-oriented programming", 1),
                ("The 'for' loop in Python is used to iterate over elements", 1),
                ("Python is known for its large community and vast documentation", 1),
                ("In Python, indentation is used to define code blocks", 1),
                ("Python supports both procedural and functional programming", 1),
                ("You can use Python to build web applications with Flask", 1),

                // Non-Python-related sentences
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

            for (int i = 0; i < 30; i++)
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
                // Python-related sentences
                ("Python is a versatile programming language", 1),
                ("Python can be used for web scraping with libraries like BeautifulSoup", 1),
                ("You can use Python to perform data analysis with pandas", 1),
                ("Python's syntax allows for rapid development", 1),
                ("Python supports regular expressions for text pattern matching", 1),
                ("Python can be used for system scripting and automation", 1),
                ("Flask is a lightweight web framework for Python", 1),
                ("In Python, you can define a function using the 'def' keyword", 1),
                ("Python supports decorators to modify the behavior of functions", 1),
                ("Python is often used in the scientific computing community", 1),

                // Non-Python-related sentences
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
            Sentence: "Python is a versatile programming language" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for web scraping with libraries like BeautifulSoup" - Predicted: 1, Expected: 1
            Sentence: "You can use Python to perform data analysis with pandas" - Predicted: 1, Expected: 1
            Sentence: "Python's syntax allows for rapid development" - Predicted: 1, Expected: 1
            Sentence: "Python supports regular expressions for text pattern matching" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for system scripting and automation" - Predicted: 1, Expected: 1
            Sentence: "Flask is a lightweight web framework for Python" - Predicted: 1, Expected: 1
            Sentence: "In Python, you can define a function using the 'def' keyword" - Predicted: 1, Expected: 1
            Sentence: "Python supports decorators to modify the behavior of functions" - Predicted: 1, Expected: 1
            Sentence: "Python is often used in the scientific computing community" - Predicted: 1, Expected: 1
            Sentence: "birds build nests to lay their eggs" - Predicted: -1, Expected: -1
            Sentence: "the piano is a popular musical instrument" - Predicted: 1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "a rainy day is perfect for reading books" - Predicted: -1, Expected: -1
            Sentence: "rivers flow into oceans" - Predicted: -1, Expected: -1
            Sentence: "modern cars often have automatic transmission" - Predicted: 1, Expected: -1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: -1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "a balanced diet is important for health" - Predicted: 1, Expected: -1
            Sentence: "hiking is a popular outdoor activity" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "Python is a versatile programming language" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for web scraping with libraries like BeautifulSoup" - Predicted: -1, Expected: 1
            Sentence: "You can use Python to perform data analysis with pandas" - Predicted: 1, Expected: 1
            Sentence: "Python's syntax allows for rapid development" - Predicted: 1, Expected: 1
            Sentence: "Python supports regular expressions for text pattern matching" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for system scripting and automation" - Predicted: 1, Expected: 1
            Sentence: "Flask is a lightweight web framework for Python" - Predicted: 1, Expected: 1
            Sentence: "In Python, you can define a function using the 'def' keyword" - Predicted: 1, Expected: 1
            Sentence: "Python supports decorators to modify the behavior of functions" - Predicted: 1, Expected: 1
            Sentence: "Python is often used in the scientific computing community" - Predicted: 1, Expected: 1
            Sentence: "birds build nests to lay their eggs" - Predicted: -1, Expected: -1
            Sentence: "the piano is a popular musical instrument" - Predicted: -1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "a rainy day is perfect for reading books" - Predicted: -1, Expected: -1
            Sentence: "rivers flow into oceans" - Predicted: -1, Expected: -1
            Sentence: "modern cars often have automatic transmission" - Predicted: 1, Expected: -1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: -1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "a balanced diet is important for health" - Predicted: 1, Expected: -1
            Sentence: "hiking is a popular outdoor activity" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "Python is a versatile programming language" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for web scraping with libraries like BeautifulSoup" - Predicted: -1, Expected: 1
            Sentence: "You can use Python to perform data analysis with pandas" - Predicted: 1, Expected: 1
            Sentence: "Python's syntax allows for rapid development" - Predicted: 1, Expected: 1
            Sentence: "Python supports regular expressions for text pattern matching" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for system scripting and automation" - Predicted: 1, Expected: 1
            Sentence: "Flask is a lightweight web framework for Python" - Predicted: 1, Expected: 1
            Sentence: "In Python, you can define a function using the 'def' keyword" - Predicted: 1, Expected: 1
            Sentence: "Python supports decorators to modify the behavior of functions" - Predicted: 1, Expected: 1
            Sentence: "Python is often used in the scientific computing community" - Predicted: 1, Expected: 1
            Sentence: "birds build nests to lay their eggs" - Predicted: -1, Expected: -1
            Sentence: "the piano is a popular musical instrument" - Predicted: 1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "a rainy day is perfect for reading books" - Predicted: -1, Expected: -1
            Sentence: "rivers flow into oceans" - Predicted: -1, Expected: -1
            Sentence: "modern cars often have automatic transmission" - Predicted: -1, Expected: -1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: -1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "a balanced diet is important for health" - Predicted: 1, Expected: -1
            Sentence: "hiking is a popular outdoor activity" - Predicted: -1, Expected: -1
            */

            /*
            Sentence: "Python is a versatile programming language" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for web scraping with libraries like BeautifulSoup" - Predicted: 1, Expected: 1
            Sentence: "You can use Python to perform data analysis with pandas" - Predicted: 1, Expected: 1
            Sentence: "Python's syntax allows for rapid development" - Predicted: 1, Expected: 1
            Sentence: "Python supports regular expressions for text pattern matching" - Predicted: 1, Expected: 1
            Sentence: "Python can be used for system scripting and automation" - Predicted: 1, Expected: 1
            Sentence: "Flask is a lightweight web framework for Python" - Predicted: 1, Expected: 1
            Sentence: "In Python, you can define a function using the 'def' keyword" - Predicted: 1, Expected: 1
            Sentence: "Python supports decorators to modify the behavior of functions" - Predicted: 1, Expected: 1
            Sentence: "Python is often used in the scientific computing community" - Predicted: 1, Expected: 1
            Sentence: "birds build nests to lay their eggs" - Predicted: -1, Expected: -1
            Sentence: "the piano is a popular musical instrument" - Predicted: 1, Expected: -1
            Sentence: "chocolate is made from cocoa beans" - Predicted: -1, Expected: -1
            Sentence: "a rainy day is perfect for reading books" - Predicted: -1, Expected: -1
            Sentence: "rivers flow into oceans" - Predicted: -1, Expected: -1
            Sentence: "modern cars often have automatic transmission" - Predicted: 1, Expected: -1
            Sentence: "roses are often given as gifts on Valentine's Day" - Predicted: 1, Expected: -1
            Sentence: "elephants are the largest land animals" - Predicted: -1, Expected: -1
            Sentence: "a balanced diet is important for health" - Predicted: 1, Expected: -1
            Sentence: "hiking is a popular outdoor activity" - Predicted: -1, Expected: -1
            */

            Console.Read();
        }
    }
}
