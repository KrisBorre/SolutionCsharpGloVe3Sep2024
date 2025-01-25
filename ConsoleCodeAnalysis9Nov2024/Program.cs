using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleCodeAnalysis9Nov2024
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var apiClient = new ApiClient("http://localhost:8080", 4096);


            MetadataReferenceManager8Nov2024 _metadataManager = new MetadataReferenceManager8Nov2024();
            CodeCompiler _compiler = new CodeCompiler(_metadataManager);
            CodeExecutor _executor = new CodeExecutor();

            var apiClientHandler = new ApiClientHandler(apiClient);
            var projectPromptHandler = new ProjectPromptHandler8Nov2024(apiClientHandler);


            #region Temperature

            // See ConsoleCodeAnalysis24Oct2024
            var project10 = new Project9Oct2024 { Name = "Temperature" };

            Conversation conversation10 = CreateNewConversation(project10);
            SourceCodeIteration sourceCodeIteration10 = projectPromptHandler.CreateNewIteration(conversation10);
            string code4 = @"using System;
            using System.Collections.Generic;

            public class TemperatureSensor : IObservable<int>
            {
                private List<IObserver<int>> observers = new List<IObserver<int>>();

                public IDisposable Subscribe(IObserver<int> observer)
                {
                    if (!observers.Contains(observer))
                        observers.Add(observer);

                    return new Unsubscriber(observers, observer);
                }

                public void PublishTemperature(int temperature)
                {
                    foreach (var observer in observers)
                    {
                        if (temperature >= 30)
                            observer.OnNext(temperature);
                        else
                            observer.OnError(new Exception(""Temperature too low""));
                    }
                }

                private class Unsubscriber : IDisposable
                {
                    private List<IObserver<int>> _observers;
                    private IObserver<int> _observer;

                    public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
                    {
                        this._observers = observers;
                        this._observer = observer;
                    }

                    public void Dispose()
                    {
                        if (_observer != null && _observers.Contains(_observer))
                            _observers.Remove(_observer);
                    }
                }
            }";
            string code5 = @"using System;

            public class TemperatureDisplay : IObserver<int>
            {
                public void OnNext(int temperature)
                {
                    Console.WriteLine($""Temperature: {temperature}"");
                }

                public void OnError(Exception error)
                {
                    Console.WriteLine(error.Message);
                }

                public void OnCompleted()
                {
                    Console.WriteLine(""Sensor readings complete"");
                }
            }";
            string code6 = @"using System;

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var sensor = new TemperatureSensor();
                    var display = new TemperatureDisplay();

                    using (sensor.Subscribe(display))
                    {
                        sensor.PublishTemperature(25);
                        sensor.PublishTemperature(35);
                        sensor.PublishTemperature(40);
                    }
                }
            }";
            sourceCodeIteration10.SourceCodes.Add(new SourceCode8Nov2024(code4));
            sourceCodeIteration10.SourceCodes.Add(new SourceCode8Nov2024(code5));
            sourceCodeIteration10.SourceCodes.Add(new SourceCode8Nov2024(code6));

            foreach (var sourceCode in sourceCodeIteration10.SourceCodes)
            {
                string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                sourceCode.IsCompiled = true;
                sourceCode.CompilationSuccess = compilationResult.Success;

                if (compilationResult.Success)
                {
                    ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                    sourceCode.ExecutionOutput = executionResult.Output;
                }
                else
                {
                    sourceCode.CompilationErrors = compilationResult.Diagnostics;
                }
            }

            Print(project10);


            var syntaxTrees10 = new[]
            {
                CSharpSyntaxTree.ParseText(code4),
                CSharpSyntaxTree.ParseText(code5),
                CSharpSyntaxTree.ParseText(code6)
            };

            // Step 3: Add references
            var references10 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
            };

            // Step 4: Compile into a single assembly
            var compilation10 = CSharpCompilation.Create(
                "MergedAssembly",
                syntaxTrees10,
                references10,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Step 5: Emit the assembly to memory or disk
            using (var ms = new MemoryStream())
            {
                var result = compilation10.Emit(ms);

                if (result.Success)
                {
                    Console.WriteLine("Compilation successful!");

                    // Load and execute the assembly
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var programType = assembly.GetType("Program");
                    var mainMethod = programType.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                    mainMethod.Invoke(null, new object[] { new string[0] });
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                }
            }

            /*
             
             Project: Temperature
conversation #1
iteration #1
Code: using System;
            using System.Collections.Generic;

            public class TemperatureSensor : IObservable<int>
            {
                private List<IObserver<int>> observers = new List<IObserver<int>>();

                public IDisposable Subscribe(IObserver<int> observer)
                {
                    if (!observers.Contains(observer))
                        observers.Add(observer);

                    return new Unsubscriber(observers, observer);
                }

                public void PublishTemperature(int temperature)
                {
                    foreach (var observer in observers)
                    {
                        if (temperature >= 30)
                            observer.OnNext(temperature);
                        else
                            observer.OnError(new Exception("Temperature too low"));
                    }
                }

                private class Unsubscriber : IDisposable
                {
                    private List<IObserver<int>> _observers;
                    private IObserver<int> _observer;

                    public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
                    {
                        this._observers = observers;
                        this._observer = observer;
                    }

                    public void Dispose()
                    {
                        if (_observer != null && _observers.Contains(_observer))
                            _observers.Remove(_observer);
                    }
                }
            }
Compilation Errors: error CS5001: Program does not contain a static 'Main' method suitable for an entry point
Code: using System;

            public class TemperatureDisplay : IObserver<int>
            {
                public void OnNext(int temperature)
                {
                    Console.WriteLine($"Temperature: {temperature}");
                }

                public void OnError(Exception error)
                {
                    Console.WriteLine(error.Message);
                }

                public void OnCompleted()
                {
                    Console.WriteLine("Sensor readings complete");
                }
            }
Compilation Errors: error CS5001: Program does not contain a static 'Main' method suitable for an entry point
Code: using System;

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var sensor = new TemperatureSensor();
                    var display = new TemperatureDisplay();

                    using (sensor.Subscribe(display))
                    {
                        sensor.PublishTemperature(25);
                        sensor.PublishTemperature(35);
                        sensor.PublishTemperature(40);
                    }
                }
            }
Compilation Errors: (7,38): error CS0246: The type or namespace name 'TemperatureSensor' could not be found (are you missing a using directive or an assembly reference?)
(8,39): error CS0246: The type or namespace name 'TemperatureDisplay' could not be found (are you missing a using directive or an assembly reference?)
Compilation successful!
Temperature too low
Temperature: 35
Temperature: 4
             */

            #endregion

            Console.ReadLine();

            #region Recurrent Neural Networks
            var project9 = new Project9Oct2024 { Name = "Recurrent Neural Networks" };
            Conversation conversation5 = CreateNewConversation(project9);
            SourceCodeIteration sourceCodeIteration5 = projectPromptHandler.CreateNewIteration(conversation5);

            #region old
            //sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
            //using System;
            //using System.Collections.Generic;
            //using System.IO;
            //using System.Linq;

            //    internal class Program
            //    {
            //        static void Main(string[] args)
            //        {
            //            string gloveFilePath = ""../../../../../../../GloVe/glove.6B.300d.txt"";
            //            int embeddingDim = 300;
            //            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            //            int hiddenSize = 10;
            //            double learningRate = 0.1;
            //            int epochs = 1000;

            //            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            //            // Five physics-related sentences for training
            //            List<string[]> physicsSentences = new List<string[]>
            //                {
            //                    new string[] { ""energy"", ""is"", ""conserved"", ""in"", ""system"" },
            //                    new string[] { ""force"", ""equals"", ""mass"", ""times"", ""acceleration"" },
            //                    new string[] { ""light"", ""travels"", ""in"", ""vacuum"", ""at"", ""speed"" },
            //                    new string[] { ""electron"", ""has"", ""negative"", ""charge"" },
            //                    new string[] { ""gravity"", ""pulls"", ""objects"", ""towards"", ""earth"" }
            //                };

            //            // Training the RNN on these sentences
            //            for (int epoch = 0; epoch < epochs; epoch++)
            //            {
            //                double totalLoss = 0;

            //                foreach (var sentence in physicsSentences)
            //                {
            //                    List<double[]> physicsSentenceEmbeddings = sentence
            //                        .Select(word => glove.GetEmbedding(word))
            //                        .ToList();

            //                    for (int i = 0; i < physicsSentenceEmbeddings.Count - 1; i++)
            //                    {
            //                        double[] inputEmbedding = physicsSentenceEmbeddings[i];
            //                        double[] targetEmbedding = physicsSentenceEmbeddings[i + 1];

            //                        // Forward pass
            //                        double[] trainOutputEmbedding = rnn.Forward(inputEmbedding);

            //                        // Calculate Mean Squared Error loss for the current word pair
            //                        double loss = 0;
            //                        for (int j = 0; j < trainOutputEmbedding.Length; j++)
            //                        {
            //                            loss += Math.Pow(trainOutputEmbedding[j] - targetEmbedding[j], 2);
            //                        }
            //                        loss /= embeddingDim;
            //                        totalLoss += loss;

            //                        // Backward pass
            //                        rnn.Backward(trainOutputEmbedding, targetEmbedding, inputEmbedding);
            //                    }
            //                }

            //                totalLoss /= physicsSentences.Count;
            //                Console.WriteLine($""Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}"");
            //            }

            //            // Testing the RNN to see if it can recall physics sentences
            //            Console.WriteLine(""Testing trained RNN on physics sentences:"");
            //            foreach (var sentence in physicsSentences)
            //            {
            //                Console.Write(""Input: "");
            //                foreach (var word in sentence)
            //                {
            //                    Console.Write(word + "" "");
            //                }
            //                Console.Write(""\nPredicted: "");

            //                List<double[]> sentenceEmbeddings = sentence
            //                    .Select(word => glove.GetEmbedding(word))
            //                    .ToList();

            //                for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
            //                {
            //                    double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
            //                    string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
            //                    Console.Write(predictedWord + "" "");
            //                }
            //                Console.WriteLine(""\n"");
            //            }

            //            Console.ReadKey();
            //        }
            //    }
            //"));
            //sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"using System.Globalization;
            //using System;
            //using System.Collections.Generic;
            //using System.IO;
            //using System.Linq;

            //    public class GloveLoader
            //    {
            //        private Dictionary<string, double[]> embeddings;
            //        private int embeddingDim;

            //        public GloveLoader(string filePath, int embeddingDim)
            //        {
            //            this.embeddingDim = embeddingDim;
            //            this.embeddings = LoadEmbeddings(filePath);
            //        }

            //        private Dictionary<string, double[]> LoadEmbeddings(string filePath)
            //        {
            //            var embeddings = new Dictionary<string, double[]>();

            //            foreach (var line in File.ReadLines(filePath))
            //            {
            //                var parts = line.Split(' ');
            //                var word = parts[0];
            //                var vector = parts.Skip(1).Select(s => double.Parse(s, CultureInfo.InvariantCulture) / 10).ToArray(); // Normalize embeddings

            //                embeddings[word] = vector;
            //            }

            //            return embeddings;
            //        }

            //        public double[] GetEmbedding(string word)
            //        {
            //            if (embeddings.ContainsKey(word))
            //            {
            //                return embeddings[word];
            //            }
            //            else
            //            {
            //                return new double[embeddingDim];
            //            }
            //        }

            //        public string FindClosestWord(double[] embedding)
            //        {
            //            double bestSimilarity = double.MinValue;
            //            string bestWord = null;

            //            foreach (var kvp in embeddings)
            //            {
            //                var word = kvp.Key;
            //                var vector = kvp.Value;
            //                double similarity = CosineSimilarity(vector, embedding);

            //                if (similarity > bestSimilarity)
            //                {
            //                    bestSimilarity = similarity;
            //                    bestWord = word;
            //                }
            //            }

            //            return bestWord;
            //        }

            //        private double CosineSimilarity(double[] vectorA, double[] vectorB)
            //        {
            //            double dotProduct = 0, normA = 0, normB = 0;
            //            for (int i = 0; i < vectorA.Length; i++)
            //            {
            //                dotProduct += vectorA[i] * vectorB[i];
            //                normA += vectorA[i] * vectorA[i];
            //                normB += vectorB[i] * vectorB[i];
            //            }
            //            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
            //        }
            //    }"));
            //sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
            //using System;
            //using System.Collections.Generic;
            //using System.IO;
            //using System.Linq;

            //    public class SimpleRNN
            //    {
            //        private int inputSize;
            //        private int hiddenSize;
            //        private int outputSize;
            //        private double learningRate;

            //        public double[,] Wx;
            //        public double[,] Wh;
            //        public double[] Bh;
            //        public double[] h;

            //        public SimpleRNN(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.001)
            //        {
            //            this.inputSize = inputSize;
            //            this.hiddenSize = hiddenSize;
            //            this.outputSize = outputSize;
            //            this.learningRate = learningRate;

            //            Wx = InitializeMatrix(inputSize, hiddenSize);
            //            Wh = InitializeMatrix(hiddenSize, outputSize);
            //            Bh = new double[hiddenSize];
            //            h = new double[hiddenSize];
            //        }

            //        private double[,] InitializeMatrix(int rows, int cols)
            //        {
            //            var rand = new Random();
            //            double[,] matrix = new double[rows, cols];
            //            for (int i = 0; i < rows; i++)
            //                for (int j = 0; j < cols; j++)
            //                    matrix[i, j] = rand.NextDouble() * 0.01; // Small random values
            //            return matrix;
            //        }

            //        public double[] Forward(double[] input)
            //        {
            //            h = new double[hiddenSize];
            //            for (int j = 0; j < hiddenSize; j++)
            //            {
            //                for (int i = 0; i < inputSize; i++)
            //                {
            //                    h[j] += input[i] * Wx[i, j];
            //                }
            //                h[j] = Math.Tanh(h[j] + Bh[j]);
            //            }

            //            var output = new double[outputSize];
            //            for (int k = 0; k < outputSize; k++)
            //            {
            //                for (int j = 0; j < hiddenSize; j++)
            //                {
            //                    output[k] += h[j] * Wh[j, k];
            //                }
            //            }
            //            return output;
            //        }

            //        public void Backward(double[] output, double[] target, double[] input)
            //        {
            //            double[] dOutput = new double[output.Length];
            //            for (int i = 0; i < output.Length; i++)
            //            {
            //                dOutput[i] = 2 * (output[i] - target[i]);
            //            }

            //            double[] dH = new double[hiddenSize];
            //            for (int j = 0; j < hiddenSize; j++)
            //            {
            //                for (int k = 0; k < outputSize; k++)
            //                {
            //                    dH[j] += dOutput[k] * Wh[j, k];
            //                    Wh[j, k] -= learningRate * dOutput[k] * h[j];
            //                }
            //                dH[j] *= (1 - h[j] * h[j]); // Derivative of tanh
            //            }

            //            for (int j = 0; j < hiddenSize; j++)
            //            {
            //                for (int i = 0; i < inputSize; i++)
            //                {
            //                    Wx[i, j] -= learningRate * dH[j] * input[i];
            //                }
            //                Bh[j] -= learningRate * dH[j];
            //            }
            //        }
            //    }"));
            #endregion

            #region new
            sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
            using System;
            using System.Collections.Generic;
            using System.Globalization;
            using System.IO;
            using System.Linq;

                internal class Program
                {
                    static void Main(string[] args)
                    {
                        string gloveFilePath = ""../../../../../../../GloVe/glove.6B.300d.txt"";
                        int embeddingDim = 300;
                        var glove = new GloveLoader(gloveFilePath, embeddingDim);

                        int hiddenSize = 10;
                        double learningRate = 0.1;
                        int epochs = 1000;

                        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

                        // Five physics-related sentences for training
                        System.Collections.Generic.List<string[]> physicsSentences = new System.Collections.Generic.List<string[]>
                            {
                                new string[] { ""energy"", ""is"", ""conserved"", ""in"", ""system"" },
                                new string[] { ""force"", ""equals"", ""mass"", ""times"", ""acceleration"" },
                                new string[] { ""light"", ""travels"", ""in"", ""vacuum"", ""at"", ""speed"" },
                                new string[] { ""electron"", ""has"", ""negative"", ""charge"" },
                                new string[] { ""gravity"", ""pulls"", ""objects"", ""towards"", ""earth"" }
                            };

                        // Training the RNN on these sentences
                        for (int epoch = 0; epoch < epochs; epoch++)
                        {
                            double totalLoss = 0;

                            foreach (var sentence in physicsSentences)
                            {
                                System.Collections.Generic.List<double[]> physicsSentenceEmbeddings = sentence
                                    .Select(word => glove.GetEmbedding(word))
                                    .ToList();

                                for (int i = 0; i < physicsSentenceEmbeddings.Count - 1; i++)
                                {
                                    double[] inputEmbedding = physicsSentenceEmbeddings[i];
                                    double[] targetEmbedding = physicsSentenceEmbeddings[i + 1];

                                    // Forward pass
                                    double[] trainOutputEmbedding = rnn.Forward(inputEmbedding);

                                    // Calculate Mean Squared Error loss for the current word pair
                                    double loss = 0;
                                    for (int j = 0; j < trainOutputEmbedding.Length; j++)
                                    {
                                        loss += Math.Pow(trainOutputEmbedding[j] - targetEmbedding[j], 2);
                                    }
                                    loss /= embeddingDim;
                                    totalLoss += loss;

                                    // Backward pass
                                    rnn.Backward(trainOutputEmbedding, targetEmbedding, inputEmbedding);
                                }
                            }

                            totalLoss /= physicsSentences.Count;
                            Console.WriteLine($""Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}"");
                        }

                        // Testing the RNN to see if it can recall physics sentences
                        Console.WriteLine(""Testing trained RNN on physics sentences:"");
                        foreach (var sentence in physicsSentences)
                        {
                            Console.Write(""Input: "");
                            foreach (var word in sentence)
                            {
                                Console.Write(word + "" "");
                            }
                            Console.Write(""\nPredicted: "");

                            System.Collections.Generic.List<double[]> sentenceEmbeddings = sentence
                                .Select(word => glove.GetEmbedding(word))
                                .ToList();

                            for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                            {
                                double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                                string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                                Console.Write(predictedWord + "" "");
                            }
                            Console.WriteLine(""\n"");
                        }

                        Console.ReadKey();
                    }
                }
            "));
            sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
            using System;
            using System.Collections.Generic;
            using System.Globalization;
            using System.IO;
            using System.Linq;

                public class SimpleRNN
                {
                    private int inputSize;
                    private int hiddenSize;
                    private int outputSize;
                    private double learningRate;

                    public double[,] Wx;
                    public double[,] Wh;
                    public double[] Bh;
                    public double[] h;

                    public SimpleRNN(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.001)
                    {
                        this.inputSize = inputSize;
                        this.hiddenSize = hiddenSize;
                        this.outputSize = outputSize;
                        this.learningRate = learningRate;

                        Wx = InitializeMatrix(inputSize, hiddenSize);
                        Wh = InitializeMatrix(hiddenSize, outputSize);
                        Bh = new double[hiddenSize];
                        h = new double[hiddenSize];
                    }

                    private double[,] InitializeMatrix(int rows, int cols)
                    {
                        var rand = new Random();
                        double[,] matrix = new double[rows, cols];
                        for (int i = 0; i < rows; i++)
                            for (int j = 0; j < cols; j++)
                                matrix[i, j] = rand.NextDouble() * 0.01; // Small random values
                        return matrix;
                    }

                    public double[] Forward(double[] input)
                    {
                        h = new double[hiddenSize];
                        for (int j = 0; j < hiddenSize; j++)
                        {
                            for (int i = 0; i < inputSize; i++)
                            {
                                h[j] += input[i] * Wx[i, j];
                            }
                            h[j] = Math.Tanh(h[j] + Bh[j]);
                        }

                        var output = new double[outputSize];
                        for (int k = 0; k < outputSize; k++)
                        {
                            for (int j = 0; j < hiddenSize; j++)
                            {
                                output[k] += h[j] * Wh[j, k];
                            }
                        }
                        return output;
                    }

                    public void Backward(double[] output, double[] target, double[] input)
                    {
                        double[] dOutput = new double[output.Length];
                        for (int i = 0; i < output.Length; i++)
                        {
                            dOutput[i] = 2 * (output[i] - target[i]);
                        }

                        double[] dH = new double[hiddenSize];
                        for (int j = 0; j < hiddenSize; j++)
                        {
                            for (int k = 0; k < outputSize; k++)
                            {
                                dH[j] += dOutput[k] * Wh[j, k];
                                Wh[j, k] -= learningRate * dOutput[k] * h[j];
                            }
                            dH[j] *= (1 - h[j] * h[j]); // Derivative of tanh
                        }

                        for (int j = 0; j < hiddenSize; j++)
                        {
                            for (int i = 0; i < inputSize; i++)
                            {
                                Wx[i, j] -= learningRate * dH[j] * input[i];
                            }
                            Bh[j] -= learningRate * dH[j];
                        }
                    }
                }
            
            "));
            sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
            using System;
            using System.Collections.Generic;
            using System.Globalization;
            using System.IO;
            using System.Linq;
     
                public class GloveLoader
                {
                    private Dictionary<string, double[]> embeddings;
                    private int embeddingDim;

                    public GloveLoader(string filePath, int embeddingDim)
                    {
                        this.embeddingDim = embeddingDim;
                        this.embeddings = LoadEmbeddings(filePath);
                    }

                    private Dictionary<string, double[]> LoadEmbeddings(string filePath)
                    {
                        var embeddings = new Dictionary<string, double[]>();

                        foreach (var line in File.ReadLines(filePath))
                        {
                            var parts = line.Split(' ');
                            var word = parts[0];
                            var vector = parts.Skip(1).Select(s => double.Parse(s, CultureInfo.InvariantCulture) / 10).ToArray(); // Normalize embeddings

                            embeddings[word] = vector;
                        }

                        return embeddings;
                    }

                    public double[] GetEmbedding(string word)
                    {
                        if (embeddings.ContainsKey(word))
                        {
                            return embeddings[word];
                        }
                        else
                        {
                            return new double[embeddingDim];
                        }
                    }

                    public string FindClosestWord(double[] embedding)
                    {
                        double bestSimilarity = double.MinValue;
                        string bestWord = null;

                        foreach (var kvp in embeddings)
                        {
                            var word = kvp.Key;
                            var vector = kvp.Value;
                            double similarity = CosineSimilarity(vector, embedding);

                            if (similarity > bestSimilarity)
                            {
                                bestSimilarity = similarity;
                                bestWord = word;
                            }
                        }

                        return bestWord;
                    }

                    private double CosineSimilarity(double[] vectorA, double[] vectorB)
                    {
                        double dotProduct = 0, normA = 0, normB = 0;
                        for (int i = 0; i < vectorA.Length; i++)
                        {
                            dotProduct += vectorA[i] * vectorB[i];
                            normA += vectorA[i] * vectorA[i];
                            normB += vectorB[i] * vectorB[i];
                        }
                        return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
                    }
                }

            
            "));
            #endregion

            var syntaxTrees1 = new[]
            {
                CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[1].Code),
                CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[2].Code),
                CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[0].Code)
            };


            var references0 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
            };


            var references1 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location), // System.Collections.Generic
                MetadataReference.CreateFromFile(typeof(Random).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
                MetadataReference.CreateFromFile(typeof(Math).Assembly.Location), // System.Runtime.Extensions               
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(Decimal).Assembly.Location),      // System.Private.CoreLib or System.Runtime
            };

            var references3 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime                
                MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location), // System.Collections.Generic
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
            };


            var references4 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
                MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
                MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
              };

            var references5 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
                MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), // System.Linq
                MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location) // System.Collections
              };


            // Step 4: Compile into a single assembly
            var compilation1 = CSharpCompilation.Create(
                "MergedAssembly",
                syntaxTrees1,
                references5,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Step 5: Emit the assembly to memory or disk
            using (var ms = new MemoryStream())
            {
                var result = compilation1.Emit(ms);

                if (result.Success)
                {
                    Console.WriteLine("Compilation successful!");

                    // Load and execute the assembly
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var programType = assembly.GetType("Program");
                    var mainMethod = programType.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                    mainMethod.Invoke(null, new object[] { new string[0] });
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                }
            }

            /*
Compilation failed:
(39,103): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
(80,92): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
             */

            /*
Compilation failed:
(39,103): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
(80,92): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
            */

            #endregion 

            Console.ReadLine();

            #region Temperature

            // See ConsoleCodeAnalysis24Oct2024
            var project8 = new Project9Oct2024 { Name = "Temperature" };

            Conversation conversation4 = CreateNewConversation(project8);
            SourceCodeIteration sourceCodeIteration4 = projectPromptHandler.CreateNewIteration(conversation4);
            string code1 = @"using System;
            using System.Collections.Generic;

            public class TemperatureSensor : IObservable<int>
            {
                private List<IObserver<int>> observers = new List<IObserver<int>>();

                public IDisposable Subscribe(IObserver<int> observer)
                {
                    if (!observers.Contains(observer))
                        observers.Add(observer);

                    return new Unsubscriber(observers, observer);
                }

                public void PublishTemperature(int temperature)
                {
                    foreach (var observer in observers)
                    {
                        if (temperature >= 30)
                            observer.OnNext(temperature);
                        else
                            observer.OnError(new Exception(""Temperature too low""));
                    }
                }

                private class Unsubscriber : IDisposable
                {
                    private List<IObserver<int>> _observers;
                    private IObserver<int> _observer;

                    public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
                    {
                        this._observers = observers;
                        this._observer = observer;
                    }

                    public void Dispose()
                    {
                        if (_observer != null && _observers.Contains(_observer))
                            _observers.Remove(_observer);
                    }
                }
            }";
            string code2 = @"using System;

            public class TemperatureDisplay : IObserver<int>
            {
                public void OnNext(int temperature)
                {
                    Console.WriteLine($""Temperature: {temperature}"");
                }

                public void OnError(Exception error)
                {
                    Console.WriteLine(error.Message);
                }

                public void OnCompleted()
                {
                    Console.WriteLine(""Sensor readings complete"");
                }
            }";
            string code3 = @"using System;

            public static class Program
            {
                public static void Main(string[] args)
                {
                    var sensor = new TemperatureSensor();
                    var display = new TemperatureDisplay();

                    using (sensor.Subscribe(display))
                    {
                        sensor.PublishTemperature(25);
                        sensor.PublishTemperature(35);
                        sensor.PublishTemperature(40);
                    }
                }
            }";
            sourceCodeIteration4.SourceCodes.Add(new SourceCode8Nov2024(code1));
            sourceCodeIteration4.SourceCodes.Add(new SourceCode8Nov2024(code2));
            sourceCodeIteration4.SourceCodes.Add(new SourceCode8Nov2024(code3));

            foreach (var sourceCode in sourceCodeIteration4.SourceCodes)
            {
                string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                sourceCode.IsCompiled = true;
                sourceCode.CompilationSuccess = compilationResult.Success;

                if (compilationResult.Success)
                {
                    ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                    sourceCode.ExecutionOutput = executionResult.Output;
                }
                else
                {
                    sourceCode.CompilationErrors = compilationResult.Diagnostics;
                }
            }

            Print(project8);

            /*
            Project: Temperature
            conversation #1
            iteration #1
            Code: using System;
                        using System.Collections.Generic;

                        public class TemperatureSensor : IObservable<int>
                        {
                            private List<IObserver<int>> observers = new List<IObserver<int>>();

                            public IDisposable Subscribe(IObserver<int> observer)
                            {
                                if (!observers.Contains(observer))
                                    observers.Add(observer);

                                return new Unsubscriber(observers, observer);
                            }

                            public void PublishTemperature(int temperature)
                            {
                                foreach (var observer in observers)
                                {
                                    if (temperature >= 30)
                                        observer.OnNext(temperature);
                                    else
                                        observer.OnError(new Exception("Temperature too low"));
                                }
                            }

                            private class Unsubscriber : IDisposable
                            {
                                private List<IObserver<int>> _observers;
                                private IObserver<int> _observer;

                                public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
                                {
                                    this._observers = observers;
                                    this._observer = observer;
                                }

                                public void Dispose()
                                {
                                    if (_observer != null && _observers.Contains(_observer))
                                        _observers.Remove(_observer);
                                }
                            }
                        }
            Compilation Errors: error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            Code: using System;

                        public class TemperatureDisplay : IObserver<int>
                        {
                            public void OnNext(int temperature)
                            {
                                Console.WriteLine($"Temperature: {temperature}");
                            }

                            public void OnError(Exception error)
                            {
                                Console.WriteLine(error.Message);
                            }

                            public void OnCompleted()
                            {
                                Console.WriteLine("Sensor readings complete");
                            }
                        }
            Compilation Errors: error CS5001: Program does not contain a static 'Main' method suitable for an entry point
            Code: using System;

                        public static class Program
                        {
                            public static void Main(string[] args)
                            {
                                var sensor = new TemperatureSensor();
                                var display = new TemperatureDisplay();

                                using (sensor.Subscribe(display))
                                {
                                    sensor.PublishTemperature(25);
                                    sensor.PublishTemperature(35);
                                    sensor.PublishTemperature(40);
                                }
                            }
                        }
            Compilation Errors: (7,38): error CS0246: The type or namespace name 'TemperatureSensor' could not be found (are you missing a using directive or an assembly reference?)
            (8,39): error CS0246: The type or namespace name 'TemperatureDisplay' could not be found (are you missing a using directive or an assembly reference?) 
            */

            var syntaxTrees2 = new[]
            {
                CSharpSyntaxTree.ParseText(code1),
                CSharpSyntaxTree.ParseText(code2),
                CSharpSyntaxTree.ParseText(code3)
            };

            // Step 3: Add references
            var references2 = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Private.CoreLib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(typeof(IObserver<>).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location), // System.Collections
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location), // System.Runtime
            };

            // Step 4: Compile into a single assembly
            var compilation2 = CSharpCompilation.Create(
                "MergedAssembly",
                syntaxTrees2,
                references2,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            // Step 5: Emit the assembly to memory or disk
            using (var ms = new MemoryStream())
            {
                var result = compilation2.Emit(ms);

                if (result.Success)
                {
                    Console.WriteLine("Compilation successful!");

                    // Load and execute the assembly
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var programType = assembly.GetType("Program");
                    var mainMethod = programType.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
                    mainMethod.Invoke(null, new object[] { new string[0] });
                }
                else
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic.ToString());
                    }
                }
            }

            #endregion

            Console.ReadLine();

            #region IFormattable

            var project7 = new Project9Oct2024 { Name = "IFormattable" };

            project7.Prompts.Add(@"Write a C# console application that demonstrates the IFormattable interface for custom string formatting.

Write a Person class to implement IFormattable.
Override the ToString method to provide custom formats for displaying Person information (e.g., Name only, Age only, or both).
Write a Main method to demonstrate formatting Person objects using different formats.");

            Conversation conversation1 = CreateNewConversation(project7);
            SourceCodeIteration sourceCodeIteration1 = projectPromptHandler.CreateNewIteration(conversation1);

            sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"            public static void Main(string[] args)
            {
                Person person = new Person(""John Doe"", 30);

                Console.WriteLine(person.ToString(""F"", null));  // Outputs: John Doe (30 years old)
                Console.WriteLine(person.ToString(""N"", null));  // Outputs: John Doe
                Console.WriteLine(person.ToString(""A"", null));  // Outputs: 30
            }"));
            sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"using System;

            public class Person : IFormattable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                public string ToString(string format, IFormatProvider formatProvider)
                {
                    if (string.IsNullOrEmpty(format))
                        format = ""F"";

                    switch (format.ToUpper())
                    {
                        case ""F"":
                            return $""{Name} ({Age} years old)"";
                        case ""N"":
                            return Name;
                        case ""A"":
                            return Age.ToString();
                        default:
                            throw new FormatException(string.Format(""The {0} format string is not supported."", format));
                    }
                }

                public override string ToString()
                {
                    return ToString(null, null);
                }
            }

            public class Program
            {
                public static void Main(string[] args)
                {
                    Person person = new Person(""John Doe"", 30);

                    Console.WriteLine(person.ToString(""F"", null));  // Outputs: John Doe (30 years old)
                    Console.WriteLine(person.ToString(""N"", null));  // Outputs: John Doe
                    Console.WriteLine(person.ToString(""A"", null));  // Outputs: 30
                }
            }"));

            foreach (var sourceCode in sourceCodeIteration1.SourceCodes)
            {
                string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                sourceCode.IsCompiled = true;
                sourceCode.CompilationSuccess = compilationResult.Success;

                if (compilationResult.Success)
                {
                    ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                    sourceCode.ExecutionOutput = executionResult.Output;
                }
                else
                {
                    sourceCode.CompilationErrors = compilationResult.Diagnostics;
                }
            }


            Conversation conversation2 = CreateNewConversation(project7);
            SourceCodeIteration sourceCodeIteration2 = projectPromptHandler.CreateNewIteration(conversation2);

            sourceCodeIteration2.SourceCodes.Add(new SourceCode8Nov2024(@"            public string ToString(string format, IFormatProvider formatProvider)
            {
                // ...
            }"));
            sourceCodeIteration2.SourceCodes.Add(new SourceCode8Nov2024(@" Console.WriteLine(person1.ToString(""N"", null));
            Console.WriteLine(person1.ToString(""A"", null));
            Console.WriteLine(person1.ToString(""F"", null));"));
            sourceCodeIteration2.SourceCodes.Add(new SourceCode8Nov2024(@"using System;

            public class Person : IFormattable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                public string ToString(string format, IFormatProvider formatProvider)
                {
                    switch (format.ToUpper())
                    {
                        case ""N"":
                            return Name;
                        case ""A"":
                            return Age.ToString();
                        case ""F"":
                            return string.Format(""{0} is {1} years old"", Name, Age);
                        default:
                            throw new FormatException(""Format not supported"");
                    }
                }

                public override string ToString()
                {
                    return ToString(""F"", null);
                }
            }

            class Program
            {
                static void Main(string[] args)
                {
                    Person person1 = new Person(""John Doe"", 30);
                    Console.WriteLine(person1.ToString(""N"", null)); // John Doe
                    Console.WriteLine(person1.ToString(""A"", null)); // 30
                    Console.WriteLine(person1.ToString(""F"", null)); // John Doe is 30 years old

                    Person person2 = new Person(""Jane Smith"", 25);
                    Console.WriteLine(string.Format(""{0:N}"", person2)); // Jane Smith
                    Console.WriteLine(string.Format(""{0:A}"", person2)); // 25
                    Console.WriteLine(string.Format(""{0}"", person2)); // Jane Smith is 25 years old
                }
            }"));

            foreach (var sourceCode in sourceCodeIteration2.SourceCodes)
            {
                string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                sourceCode.IsCompiled = true;
                sourceCode.CompilationSuccess = compilationResult.Success;

                if (compilationResult.Success)
                {
                    ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                    sourceCode.ExecutionOutput = executionResult.Output;
                }
                else
                {
                    sourceCode.CompilationErrors = compilationResult.Diagnostics;
                }
            }



            Conversation conversation3 = CreateNewConversation(project7);
            SourceCodeIteration sourceCodeIteration3 = projectPromptHandler.CreateNewIteration(conversation3);

            sourceCodeIteration3.SourceCodes.Add(new SourceCode8Nov2024(@"using System;

            public class Person : IFormattable
            {
                public string Name { get; set; }
                public int Age { get; set; }

                public Person(string name, int age)
                {
                    Name = name;
                    Age = age;
                }

                public string ToString(string format, IFormatProvider formatProvider)
                {
                    if (string.IsNullOrEmpty(format))
                    {
                        format = ""G"";
                    }

                    switch (format.ToUpperInvariant())
                    {
                        case ""N"":
                            return Name;
                        case ""A"":
                            return Age.ToString();
                        case ""G"":
                        default:
                            return $""{Name} ({Age})"";
                    }
                }

                public override string ToString()
                {
                    return ToString(null, null);
                }
            }

            class Program
            {
                static void Main(string[] args)
                {
                    Person person1 = new Person(""John Doe"", 30);
                    Console.WriteLine(person1.ToString(""N"", null)); // John Doe
                    Console.WriteLine(person1.ToString(""A"", null)); // 30
                    Console.WriteLine(person1.ToString(""G"", null)); // John Doe (30)

                    Console.ReadKey();
                }
            }"));
            sourceCodeIteration3.SourceCodes.Add(new SourceCode8Nov2024(@" public string ToString(string format, IFormatProvider formatProvider)
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = ""G"";
                }

                var culture = formatProvider.GetFormat(typeof(CultureInfo)) as CultureInfo;

                switch (format.ToUpperInvariant())
                {
                    case ""N"":
                        return Name;
                    case ""A"":
                        return Age.ToString(culture);
                    case ""G"":
                    default:
                        return $""{Name} ({Age.ToString(culture)})"";
                }
            }"));

            foreach (var sourceCode in sourceCodeIteration3.SourceCodes)
            {
                string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                sourceCode.IsCompiled = true;
                sourceCode.CompilationSuccess = compilationResult.Success;

                if (compilationResult.Success)
                {
                    ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                    sourceCode.ExecutionOutput = executionResult.Output;
                }
                else
                {
                    sourceCode.CompilationErrors = compilationResult.Diagnostics;
                }
            }


            Print(project7);




            /*
            Project: IFormattable
            conversation #1
            iteration #1
            Code:             public static void Main(string[] args)
                        {
                            Person person = new Person("John Doe", 30);

                            Console.WriteLine(person.ToString("F", null));  // Outputs: John Doe (30 years old)
                            Console.WriteLine(person.ToString("N", null));  // Outputs: John Doe
                            Console.WriteLine(person.ToString("A", null));  // Outputs: 30
                        }
            Compilation Errors: (1,13): error CS0106: The modifier 'public' is not valid for this item
            (3,17): error CS0246: The type or namespace name 'Person' could not be found (are you missing a using directive or an assembly reference?)
            (3,37): error CS0246: The type or namespace name 'Person' could not be found (are you missing a using directive or an assembly reference?)
            (5,17): error CS0103: The name 'Console' does not exist in the current context
            (6,17): error CS0103: The name 'Console' does not exist in the current context
            (7,17): error CS0103: The name 'Console' does not exist in the current context
            (1,32): warning CS7022: The entry point of the program is global code; ignoring 'Main(string[])' entry point.
            (1,32): warning CS8321: The local function 'Main' is declared but never used
            Code: using System;

                        public class Person : IFormattable
                        {
                            public string Name { get; set; }
                            public int Age { get; set; }

                            public Person(string name, int age)
                            {
                                Name = name;
                                Age = age;
                            }

                            public string ToString(string format, IFormatProvider formatProvider)
                            {
                                if (string.IsNullOrEmpty(format))
                                    format = "F";

                                switch (format.ToUpper())
                                {
                                    case "F":
                                        return $"{Name} ({Age} years old)";
                                    case "N":
                                        return Name;
                                    case "A":
                                        return Age.ToString();
                                    default:
                                        throw new FormatException(string.Format("The {0} format string is not supported.", format));
                                }
                            }

                            public override string ToString()
                            {
                                return ToString(null, null);
                            }
                        }

                        public class Program
                        {
                            public static void Main(string[] args)
                            {
                                Person person = new Person("John Doe", 30);

                                Console.WriteLine(person.ToString("F", null));  // Outputs: John Doe (30 years old)
                                Console.WriteLine(person.ToString("N", null));  // Outputs: John Doe
                                Console.WriteLine(person.ToString("A", null));  // Outputs: 30
                            }
                        }
            Execution Output: John Doe (30 years old)
            John Doe
            30

            conversation #2
            iteration #1
            Code:             public string ToString(string format, IFormatProvider formatProvider)
                        {
                            // ...
                        }
            Compilation Errors: (1,13): error CS0106: The modifier 'public' is not valid for this item
            (1,27): error CS0161: 'ToString(string, IFormatProvider)': not all code paths return a value
            (1,51): error CS0246: The type or namespace name 'IFormatProvider' could not be found (are you missing a using directive or an assembly reference?)
            (1,27): warning CS8321: The local function 'ToString' is declared but never used
            Code:  Console.WriteLine(person1.ToString("N", null));
                        Console.WriteLine(person1.ToString("A", null));
                        Console.WriteLine(person1.ToString("F", null));
            Compilation Errors: (1,2): error CS0103: The name 'Console' does not exist in the current context
            (1,20): error CS0103: The name 'person1' does not exist in the current context
            (2,13): error CS0103: The name 'Console' does not exist in the current context
            (2,31): error CS0103: The name 'person1' does not exist in the current context
            (3,13): error CS0103: The name 'Console' does not exist in the current context
            (3,31): error CS0103: The name 'person1' does not exist in the current context
            Code: using System;

                        public class Person : IFormattable
                        {
                            public string Name { get; set; }
                            public int Age { get; set; }

                            public Person(string name, int age)
                            {
                                Name = name;
                                Age = age;
                            }

                            public string ToString(string format, IFormatProvider formatProvider)
                            {
                                switch (format.ToUpper())
                                {
                                    case "N":
                                        return Name;
                                    case "A":
                                        return Age.ToString();
                                    case "F":
                                        return string.Format("{0} is {1} years old", Name, Age);
                                    default:
                                        throw new FormatException("Format not supported");
                                }
                            }

                            public override string ToString()
                            {
                                return ToString("F", null);
                            }
                        }

                        class Program
                        {
                            static void Main(string[] args)
                            {
                                Person person1 = new Person("John Doe", 30);
                                Console.WriteLine(person1.ToString("N", null)); // John Doe
                                Console.WriteLine(person1.ToString("A", null)); // 30
                                Console.WriteLine(person1.ToString("F", null)); // John Doe is 30 years old

                                Person person2 = new Person("Jane Smith", 25);
                                Console.WriteLine(string.Format("{0:N}", person2)); // Jane Smith
                                Console.WriteLine(string.Format("{0:A}", person2)); // 25
                                Console.WriteLine(string.Format("{0}", person2)); // Jane Smith is 25 years old
                            }
                        }
            Execution Output: Error during execution: Object reference not set to an instance of an object.
            conversation #3
            iteration #1
            Code: using System;

                        public class Person : IFormattable
                        {
                            public string Name { get; set; }
                            public int Age { get; set; }

                            public Person(string name, int age)
                            {
                                Name = name;
                                Age = age;
                            }

                            public string ToString(string format, IFormatProvider formatProvider)
                            {
                                if (string.IsNullOrEmpty(format))
                                {
                                    format = "G";
                                }

                                switch (format.ToUpperInvariant())
                                {
                                    case "N":
                                        return Name;
                                    case "A":
                                        return Age.ToString();
                                    case "G":
                                    default:
                                        return $"{Name} ({Age})";
                                }
                            }

                            public override string ToString()
                            {
                                return ToString(null, null);
                            }
                        }

                        class Program
                        {
                            static void Main(string[] args)
                            {
                                Person person1 = new Person("John Doe", 30);
                                Console.WriteLine(person1.ToString("N", null)); // John Doe
                                Console.WriteLine(person1.ToString("A", null)); // 30
                                Console.WriteLine(person1.ToString("G", null)); // John Doe (30)

                                Console.ReadKey();
                            }
                        }
            Execution Output: John Doe
            30
            John Doe (30)

            Code:  public string ToString(string format, IFormatProvider formatProvider)
                        {
                            if (string.IsNullOrEmpty(format))
                            {
                                format = "G";
                            }

                            var culture = formatProvider.GetFormat(typeof(CultureInfo)) as CultureInfo;

                            switch (format.ToUpperInvariant())
                            {
                                case "N":
                                    return Name;
                                case "A":
                                    return Age.ToString(culture);
                                case "G":
                                default:
                                    return $"{Name} ({Age.ToString(culture)})";
                            }
                        }
            Compilation Errors: (1,2): error CS0106: The modifier 'public' is not valid for this item
            (8,63): error CS0246: The type or namespace name 'CultureInfo' could not be found (are you missing a using directive or an assembly reference?)
            (8,80): error CS0246: The type or namespace name 'CultureInfo' could not be found (are you missing a using directive or an assembly reference?)
            (13,32): error CS0103: The name 'Name' does not exist in the current context
            (15,32): error CS0103: The name 'Age' does not exist in the current context
            (18,35): error CS0103: The name 'Name' does not exist in the current context
            (18,43): error CS0103: The name 'Age' does not exist in the current context
            (1,40): error CS0246: The type or namespace name 'IFormatProvider' could not be found (are you missing a using directive or an assembly reference?)
            (1,16): warning CS8321: The local function 'ToString' is declared but never used
            */
            #endregion

            Console.ReadLine();
        }

        private static void Print(Project9Oct2024 project)
        {
            Console.WriteLine($"Project: {project.Name}");
            foreach (var conversation in project.Conversations)
            {
                Console.WriteLine($"conversation #{conversation.Number}");
                foreach (var sourceCodeIteration in conversation.SourceCodeIterations)
                {
                    Console.WriteLine($"iteration #{sourceCodeIteration.Number}");
                    foreach (var sourceCode in sourceCodeIteration.SourceCodes)
                    {
                        Console.WriteLine($"Code: {sourceCode.Code}");
                        if (sourceCode.CompilationSuccess)
                        {
                            Console.WriteLine($"Execution Output: {sourceCode.ExecutionOutput}");
                        }
                        else
                        {
                            Console.WriteLine($"Compilation Errors: {sourceCode.CompilationErrors}");
                        }
                    }
                }
            }
        }

        private static Conversation CreateNewConversation(Project9Oct2024 project)
        {
            var conversation = new Conversation
            {
                Number = project.Conversations.Count + 1
            };
            project.Conversations.Add(conversation);
            return conversation;
        }
    }
}