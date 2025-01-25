using LibraryCodeAnalysis12Nov2024;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleCodeAnalysis12Nov2024
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            MetadataReferenceManager8Nov2024 _metadataManager = new MetadataReferenceManager8Nov2024();
            CodeCompiler _compiler = new CodeCompiler(_metadataManager);
            CodeExecutor _executor = new CodeExecutor();


            #region Recurrent Neural Networks
            {
                var project9 = new Project9Oct2024 { Name = "Recurrent Neural Networks" };

                ProjectManager projectManager = new ProjectManager();

                Conversation conversation5 = projectManager.CreateNewConversation(project9);

                SourceCodeIteration sourceCodeIteration5 = projectManager.CreateNewIteration(conversation5);

                sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string gloveFilePath = ""../../../../../../../GloVe/glove.6B.300d.txt"";
        int embeddingDim = 300;
        var glove = new GloveLoader(gloveFilePath, embeddingDim);

        int hiddenSize = 10;
        double learningRate = 0.1;
        int epochs = 1000;

        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

        // Five physics-related sentences for training
        List<string[]> physicsSentences = new List<string[]>
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
                List<double[]> physicsSentenceEmbeddings = new List<double[]>();
                foreach (var word in sentence)
                {
                    physicsSentenceEmbeddings.Add(glove.GetEmbedding(word));
                }

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

            List<double[]> sentenceEmbeddings = new List<double[]>();
            foreach (var word in sentence)
            {
                sentenceEmbeddings.Add(glove.GetEmbedding(word));
            }

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

            // Replace LINQ `Skip` and `Select` with explicit loop
            var vector = new double[embeddingDim];
            for (int i = 0; i < embeddingDim; i++)
            {
                vector[i] = double.Parse(parts[i + 1], CultureInfo.InvariantCulture) / 10; // Normalize embeddings
            }

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


                var syntaxTrees1 = new[]
                {
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[0].Code),
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[1].Code),
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[2].Code)
                };

                var references4 = new List<MetadataReference>
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
                    MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                    MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
                    MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO                
                    MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
                };


                // Step 4: Compile into a single assembly
                var compilation1 = CSharpCompilation.Create(
                    "MergedAssembly",
                    syntaxTrees1,
                    references4,
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
                Compilation successful!
                Epoch 1/1000, Loss: 0,005197647664002778
                Epoch 2/1000, Loss: 0,005194768295638975
                Epoch 3/1000, Loss: 0,005171411119769476
                Epoch 4/1000, Loss: 0,00499215747850641
                Epoch 5/1000, Loss: 0,0044564441170535625
                Epoch 6/1000, Loss: 0,004214352977565691
                Epoch 7/1000, Loss: 0,004191282860911103
                Epoch 8/1000, Loss: 0,004172386030654366
                Epoch 9/1000, Loss: 0,004155003572377145
                Epoch 10/1000, Loss: 0,004139446423534059
                Epoch 11/1000, Loss: 0,00412533125666578
                Epoch 12/1000, Loss: 0,004112345558202402
                Epoch 13/1000, Loss: 0,004100268183968721
                Epoch 14/1000, Loss: 0,00408894098076076
                Epoch 15/1000, Loss: 0,004078247418777895
                Epoch 16/1000, Loss: 0,0040680979442751725
                Epoch 17/1000, Loss: 0,004058419698795508
                Epoch 18/1000, Loss: 0,00404914905744785
                Epoch 19/1000, Loss: 0,004040226001842617
                Epoch 20/1000, Loss: 0,004031589680838251
                Epoch 21/1000, Loss: 0,004023174718329605
                Epoch 22/1000, Loss: 0,004014907956567926
                Epoch 23/1000, Loss: 0,004006705409272871
                Epoch 24/1000, Loss: 0,003998469264010988
                Epoch 25/1000, Loss: 0,0039900848361865675
                Epoch 26/1000, Loss: 0,003981417454266692
                Epoch 27/1000, Loss: 0,003972309364378395
                Epoch 28/1000, Loss: 0,003962576898621161
                Epoch 29/1000, Loss: 0,003952008367845565
                Epoch 30/1000, Loss: 0,003940363417027892
                Epoch 31/1000, Loss: 0,00392737489668567
                Epoch 32/1000, Loss: 0,003912754596999531
                Epoch 33/1000, Loss: 0,0038962043575977547
                Epoch 34/1000, Loss: 0,003877433957606364
                Epoch 35/1000, Loss: 0,0038561866207313896
                Epoch 36/1000, Loss: 0,003832271705335299
                Epoch 37/1000, Loss: 0,0038056019279038124
                Epoch 38/1000, Loss: 0,003776229190929433
                Epoch 39/1000, Loss: 0,0037443693169737363
                Epoch 40/1000, Loss: 0,0037104035222227058
                Epoch 41/1000, Loss: 0,0036748461544833222
                Epoch 42/1000, Loss: 0,0036382763686447868
                Epoch 43/1000, Loss: 0,0036012450678967215
                Epoch 44/1000, Loss: 0,0035641818026281463
                Epoch 45/1000, Loss: 0,003527330953668646
                Epoch 46/1000, Loss: 0,0034907374700669435
                Epoch 47/1000, Loss: 0,0034542833625759765
                Epoch 48/1000, Loss: 0,0034177577004598212
                Epoch 49/1000, Loss: 0,003380934603429431
                Epoch 50/1000, Loss: 0,0033436375013362017
                Epoch 51/1000, Loss: 0,0033057783756413354
                Epoch 52/1000, Loss: 0,003267370873158553
                Epoch 53/1000, Loss: 0,0032285222781815773
                Epoch 54/1000, Loss: 0,0031894112762543606
                Epoch 55/1000, Loss: 0,003150258009120677
                Epoch 56/1000, Loss: 0,003111291798493627
                Epoch 57/1000, Loss: 0,0030727209313999738
                Epoch 58/1000, Loss: 0,0030347081037472657
                Epoch 59/1000, Loss: 0,002997354215047378
                Epoch 60/1000, Loss: 0,002960691953790252
                Epoch 61/1000, Loss: 0,0029246890468411666
                Epoch 62/1000, Loss: 0,002889259483765624
                Epoch 63/1000, Loss: 0,002854279886999917
                Epoch 64/1000, Loss: 0,0028196077774303723
                Epoch 65/1000, Loss: 0,0027850988177233987
                Epoch 66/1000, Loss: 0,002750620988150578
                Epoch 67/1000, Loss: 0,0027160647212939037
                Epoch 68/1000, Loss: 0,002681348976410299
                Epoch 69/1000, Loss: 0,0026464238781437623
                Epoch 70/1000, Loss: 0,002611270830136362
                Epoch 71/1000, Loss: 0,0025759010043516968
                Epoch 72/1000, Loss: 0,002540352916481995
                Epoch 73/1000, Loss: 0,00250468954324334
                Epoch 74/1000, Loss: 0,002468995205421817
                Epoch 75/1000, Loss: 0,002433372276209051
                Epoch 76/1000, Loss: 0,002397937684150484
                Epoch 77/1000, Loss: 0,0023628191453763992
                Epoch 78/1000, Loss: 0,002328151055504259
                Epoch 79/1000, Loss: 0,0022940699827306483
                Epoch 80/1000, Loss: 0,0022607097337920433
                Epoch 81/1000, Loss: 0,0022281960311708805
                Epoch 82/1000, Loss: 0,002196640958293412
                Epoch 83/1000, Loss: 0,0021661374929858412
                Epoch 84/1000, Loss: 0,0021367546196914386
                Epoch 85/1000, Loss: 0,002108533625096352
                Epoch 86/1000, Loss: 0,0020814861772163497
                Epoch 87/1000, Loss: 0,002055594632364253
                Epoch 88/1000, Loss: 0,0020308147255650943
                Epoch 89/1000, Loss: 0,0020070804437721896
                Epoch 90/1000, Loss: 0,0019843105479700187
                Epoch 91/1000, Loss: 0,0019624159813357777
                Epoch 92/1000, Loss: 0,001941307321449982
                Epoch 93/1000, Loss: 0,0019209015045977204
                Epoch 94/1000, Loss: 0,0019011272316097013
                Epoch 95/1000, Loss: 0,0018819287016212014
                Epoch 96/1000, Loss: 0,0018632675588489728
                Epoch 97/1000, Loss: 0,001845123139584557
                Epoch 98/1000, Loss: 0,001827491253042916
                Epoch 99/1000, Loss: 0,001810381817828587
                Epoch 100/1000, Loss: 0,001793815713083779
                Epoch 101/1000, Loss: 0,001777821201450068
                Epoch 102/1000, Loss: 0,0017624302513697162
                Epoch 103/1000, Loss: 0,0017476750386140598
                Epoch 104/1000, Loss: 0,001733584848678593
                Epoch 105/1000, Loss: 0,0017201835385082381
                Epoch 106/1000, Loss: 0,001707487652456896
                Epoch 107/1000, Loss: 0,0016955052272665245
                Epoch 108/1000, Loss: 0,0016842352674297345
                Epoch 109/1000, Loss: 0,001673667828218275
                Epoch 110/1000, Loss: 0,0016637846107954112
                Epoch 111/1000, Loss: 0,001654559953134999
                Epoch 112/1000, Loss: 0,0016459620918735142
                Epoch 113/1000, Loss: 0,0016379545726457328
                Epoch 114/1000, Loss: 0,0016304976979647306
                Epoch 115/1000, Loss: 0,0016235499197776334
                Epoch 116/1000, Loss: 0,0016170691056864711
                Epoch 117/1000, Loss: 0,001611013630796065
                Epoch 118/1000, Loss: 0,0016053432689602074
                Epoch 119/1000, Loss: 0,0016000198761558612
                Epoch 120/1000, Loss: 0,0015950078738020293
                Epoch 121/1000, Loss: 0,0015902745506716008
                Epoch 122/1000, Loss: 0,0015857902087635378
                Epoch 123/1000, Loss: 0,001581528181624747
                Epoch 124/1000, Loss: 0,00157746475386287
                Epoch 125/1000, Loss: 0,0015735790087683516
                Epoch 126/1000, Loss: 0,0015698526278215854
                Epoch 127/1000, Loss: 0,0015662696620442282
                Epoch 128/1000, Loss: 0,0015628162911654649
                Epoch 129/1000, Loss: 0,0015594805827690386
                Epoch 130/1000, Loss: 0,0015562522601869277
                Epoch 131/1000, Loss: 0,0015531224850231131
                Epoch 132/1000, Loss: 0,0015500836578575088
                Epoch 133/1000, Loss: 0,0015471292388722611
                Epoch 134/1000, Loss: 0,0015442535888040127
                Epoch 135/1000, Loss: 0,001541451829684442
                Epoch 136/1000, Loss: 0,0015387197242119014
                Epoch 137/1000, Loss: 0,0015360535722276575
                Epoch 138/1000, Loss: 0,001533450122588078
                Epoch 139/1000, Loss: 0,0015309064986759062
                Epoch 140/1000, Loss: 0,0015284201358370408
                Epoch 141/1000, Loss: 0,0015259887291308274
                Epoch 142/1000, Loss: 0,0015236101899171203
                Epoch 143/1000, Loss: 0,0015212826099543662
                Epoch 144/1000, Loss: 0,0015190042318374048
                Epoch 145/1000, Loss: 0,0015167734247533884
                Epoch 146/1000, Loss: 0,0015145886646741663
                Epoch 147/1000, Loss: 0,0015124485182309824
                Epoch 148/1000, Loss: 0,0015103516296312112
                Epoch 149/1000, Loss: 0,0015082967100769717
                Epoch 150/1000, Loss: 0,001506282529232515
                Epoch 151/1000, Loss: 0,0015043079083621115
                Epoch 152/1000, Loss: 0,0015023717148240506
                Epoch 153/1000, Loss: 0,0015004728576604983
                Epoch 154/1000, Loss: 0,0014986102840685801
                Epoch 155/1000, Loss: 0,001496782976576311
                Epoch 156/1000, Loss: 0,0014949899507789373
                Epoch 157/1000, Loss: 0,001493230253517844
                Epoch 158/1000, Loss: 0,0014915029614062283
                Epoch 159/1000, Loss: 0,0014898071796239686
                Epoch 160/1000, Loss: 0,001488142040919157
                Epoch 161/1000, Loss: 0,0014865067047661405
                Epoch 162/1000, Loss: 0,0014849003566400746
                Epoch 163/1000, Loss: 0,0014833222073762865
                Epoch 164/1000, Loss: 0,0014817714925895542
                Epoch 165/1000, Loss: 0,0014802474721339397
                Epoch 166/1000, Loss: 0,0014787494295882687
                Epoch 167/1000, Loss: 0,001477276671756016
                Epoch 168/1000, Loss: 0,0014758285281712689
                Epoch 169/1000, Loss: 0,0014744043506047695
                Epoch 170/1000, Loss: 0,0014730035125659407
                Epoch 171/1000, Loss: 0,0014716254087982444
                Epoch 172/1000, Loss: 0,0014702694547664094
                Epoch 173/1000, Loss: 0,0014689350861349542
                Epoch 174/1000, Loss: 0,0014676217582381278
                Epoch 175/1000, Loss: 0,0014663289455419048
                Epoch 176/1000, Loss: 0,0014650561410990393
                Epoch 177/1000, Loss: 0,0014638028559984834
                Epoch 178/1000, Loss: 0,001462568618810603
                Epoch 179/1000, Loss: 0,0014613529750297704
                Epoch 180/1000, Loss: 0,0014601554865159396
                Epoch 181/1000, Loss: 0,0014589757309368086
                Epoch 182/1000, Loss: 0,00145781330121217
                Epoch 183/1000, Loss: 0,0014566678049619472
                Epoch 184/1000, Loss: 0,0014555388639593899
                Epoch 185/1000, Loss: 0,00145442611359076
                Epoch 186/1000, Loss: 0,0014533292023227835
                Epoch 187/1000, Loss: 0,001452247791178998
                Epoch 188/1000, Loss: 0,0014511815532260457
                Epoch 189/1000, Loss: 0,0014501301730708315
                Epoch 190/1000, Loss: 0,0014490933463693753
                Epoch 191/1000, Loss: 0,0014480707793480476
                Epoch 192/1000, Loss: 0,0014470621883378224
                Epoch 193/1000, Loss: 0,0014460672993220293
                Epoch 194/1000, Loss: 0,0014450858474980336
                Epoch 195/1000, Loss: 0,0014441175768531676
                Epoch 196/1000, Loss: 0,0014431622397551385
                Epoch 197/1000, Loss: 0,0014422195965570997
                Epoch 198/1000, Loss: 0,0014412894152174504
                Epoch 199/1000, Loss: 0,0014403714709344082
                Epoch 200/1000, Loss: 0,001439465545795309
                Epoch 201/1000, Loss: 0,0014385714284405394
                Epoch 202/1000, Loss: 0,0014376889137419726
                Epoch 203/1000, Loss: 0,001436817802495709
                Epoch 204/1000, Loss: 0,001435957901128901
                Epoch 205/1000, Loss: 0,0014351090214204034
                Epoch 206/1000, Loss: 0,0014342709802349558
                Epoch 207/1000, Loss: 0,001433443599270574
                Epoch 208/1000, Loss: 0,001432626704818816
                Epoch 209/1000, Loss: 0,0014318201275375542
                Epoch 210/1000, Loss: 0,0014310237022358888
                Epoch 211/1000, Loss: 0,0014302372676707966
                Epoch 212/1000, Loss: 0,001429460666355129
                Epoch 213/1000, Loss: 0,001428693744376546
                Epoch 214/1000, Loss: 0,0014279363512269762
                Epoch 215/1000, Loss: 0,0014271883396421853
                Epoch 216/1000, Loss: 0,001426449565451045
                Epoch 217/1000, Loss: 0,0014257198874340743
                Epoch 218/1000, Loss: 0,0014249991671908577
                Epoch 219/1000, Loss: 0,00142428726901592
                Epoch 220/1000, Loss: 0,0014235840597826611
                Epoch 221/1000, Loss: 0,0014228894088349543
                Epoch 222/1000, Loss: 0,0014222031878860194
                Epoch 223/1000, Loss: 0,0014215252709241918
                Epoch 224/1000, Loss: 0,0014208555341252128
                Epoch 225/1000, Loss: 0,0014201938557706897
                Epoch 226/1000, Loss: 0,0014195401161723638
                Epoch 227/1000, Loss: 0,0014188941976018548
                Epoch 228/1000, Loss: 0,0014182559842255515
                Epoch 229/1000, Loss: 0,0014176253620443284
                Epoch 230/1000, Loss: 0,0014170022188377846
                Epoch 231/1000, Loss: 0,001416386444112717
                Epoch 232/1000, Loss: 0,0014157779290555333
                Epoch 233/1000, Loss: 0,0014151765664883467
                Epoch 234/1000, Loss: 0,0014145822508284886
                Epoch 235/1000, Loss: 0,0014139948780511895
                Epoch 236/1000, Loss: 0,0014134143456551993
                Epoch 237/1000, Loss: 0,0014128405526311239
                Epoch 238/1000, Loss: 0,0014122733994322522
                Epoch 239/1000, Loss: 0,00141171278794769
                Epoch 240/1000, Loss: 0,0014111586214775964
                Epoch 241/1000, Loss: 0,0014106108047103505
                Epoch 242/1000, Loss: 0,0014100692437014725
                Epoch 243/1000, Loss: 0,0014095338458541423
                Epoch 244/1000, Loss: 0,0014090045199011574
                Epoch 245/1000, Loss: 0,0014084811758882081
                Epoch 246/1000, Loss: 0,0014079637251583057
                Epoch 247/1000, Loss: 0,0014074520803372722
                Epoch 248/1000, Loss: 0,001406946155320152
                Epoch 249/1000, Loss: 0,001406445865258449
                Epoch 250/1000, Loss: 0,0014059511265480894
                Epoch 251/1000, Loss: 0,0014054618568180091
                Epoch 252/1000, Loss: 0,0014049779749192824
                Epoch 253/1000, Loss: 0,0014044994009147246
                Epoch 254/1000, Loss: 0,0014040260560688727
                Epoch 255/1000, Loss: 0,0014035578628382973
                Epoch 256/1000, Loss: 0,001403094744862172
                Epoch 257/1000, Loss: 0,0014026366269530484
                Epoch 258/1000, Loss: 0,0014021834350877852
                Epoch 259/1000, Loss: 0,0014017350963985863
                Epoch 260/1000, Loss: 0,0014012915391641
                Epoch 261/1000, Loss: 0,0014008526928005504
                Epoch 262/1000, Loss: 0,0014004184878528597
                Epoch 263/1000, Loss: 0,0013999888559857385
                Epoch 264/1000, Loss: 0,0013995637299747058
                Epoch 265/1000, Loss: 0,0013991430436970358
                Epoch 266/1000, Loss: 0,0013987267321225842
                Epoch 267/1000, Loss: 0,0013983147313045082
                Epoch 268/1000, Loss: 0,0013979069783698322
                Epoch 269/1000, Loss: 0,0013975034115098748
                Epoch 270/1000, Loss: 0,0013971039699705165
                Epoch 271/1000, Loss: 0,0013967085940422915
                Epoch 272/1000, Loss: 0,0013963172250503158
                Epoch 273/1000, Loss: 0,0013959298053440336
                Epoch 274/1000, Loss: 0,0013955462782867813
                Epoch 275/1000, Loss: 0,0013951665882451802
                Epoch 276/1000, Loss: 0,001394790680578336
                Epoch 277/1000, Loss: 0,0013944185016268756
                Epoch 278/1000, Loss: 0,001394049998701794
                Epoch 279/1000, Loss: 0,0013936851200731385
                Epoch 280/1000, Loss: 0,00139332381495852
                Epoch 281/1000, Loss: 0,0013929660335114708
                Epoch 282/1000, Loss: 0,0013926117268096426
                Epoch 283/1000, Loss: 0,0013922608468428502
                Epoch 284/1000, Loss: 0,0013919133465009905
                Epoch 285/1000, Loss: 0,0013915691795618136
                Epoch 286/1000, Loss: 0,0013912283006785795
                Epoch 287/1000, Loss: 0,0013908906653675923
                Epoch 288/1000, Loss: 0,001390556229995637
                Epoch 289/1000, Loss: 0,001390224951767312
                Epoch 290/1000, Loss: 0,0013898967887122785
                Epoch 291/1000, Loss: 0,0013895716996724294
                Epoch 292/1000, Loss: 0,0013892496442889921
                Epoch 293/1000, Loss: 0,0013889305829895757
                Epoch 294/1000, Loss: 0,0013886144769751656
                Epoch 295/1000, Loss: 0,0013883012882070823
                Epoch 296/1000, Loss: 0,0013879909793939123
                Epoch 297/1000, Loss: 0,0013876835139784215
                Epoch 298/1000, Loss: 0,0013873788561244512
                Epoch 299/1000, Loss: 0,0013870769707038312
                Epoch 300/1000, Loss: 0,0013867778232832845
                Epoch 301/1000, Loss: 0,0013864813801113666
                Epoch 302/1000, Loss: 0,0013861876081054218
                Epoch 303/1000, Loss: 0,0013858964748385823
                Epoch 304/1000, Loss: 0,0013856079485268138
                Epoch 305/1000, Loss: 0,0013853219980160133
                Epoch 306/1000, Loss: 0,0013850385927691665
                Epoch 307/1000, Loss: 0,0013847577028535843
                Epoch 308/1000, Loss: 0,001384479298928209
                Epoch 309/1000, Loss: 0,0013842033522310072
                Epoch 310/1000, Loss: 0,0013839298345664631
                Epoch 311/1000, Loss: 0,0013836587182931605
                Epoch 312/1000, Loss: 0,0013833899763114794
                Epoch 313/1000, Loss: 0,0013831235820514023
                Epoch 314/1000, Loss: 0,0013828595094604389
                Epoch 315/1000, Loss: 0,0013825977329916773
                Epoch 316/1000, Loss: 0,0013823382275919608
                Epoch 317/1000, Loss: 0,0013820809686902087
                Epoch 318/1000, Loss: 0,0013818259321858673
                Epoch 319/1000, Loss: 0,0013815730944375145
                Epoch 320/1000, Loss: 0,0013813224322516064
                Epoch 321/1000, Loss: 0,0013810739228713852
                Epoch 322/1000, Loss: 0,0013808275439659364
                Epoch 323/1000, Loss: 0,0013805832736194152
                Epoch 324/1000, Loss: 0,0013803410903204267
                Epoch 325/1000, Loss: 0,0013801009729515871
                Epoch 326/1000, Loss: 0,0013798629007792406
                Epoch 327/1000, Loss: 0,0013796268534433608
                Epoch 328/1000, Loss: 0,0013793928109476171
                Epoch 329/1000, Loss: 0,0013791607536496238
                Epoch 330/1000, Loss: 0,0013789306622513698
                Epoch 331/1000, Loss: 0,001378702517789819
                Epoch 332/1000, Loss: 0,0013784763016277016
                Epoch 333/1000, Loss: 0,0013782519954444843
                Epoch 334/1000, Loss: 0,0013780295812275208
                Epoch 335/1000, Loss: 0,0013778090412633914
                Epoch 336/1000, Loss: 0,001377590358129423
                Epoch 337/1000, Loss: 0,0013773735146853975
                Epoch 338/1000, Loss: 0,0013771584940654377
                Epoch 339/1000, Loss: 0,0013769452796700873
                Epoch 340/1000, Loss: 0,0013767338551585661
                Epoch 341/1000, Loss: 0,001376524204441214
                Epoch 342/1000, Loss: 0,0013763163116721155
                Epoch 343/1000, Loss: 0,0013761101612419077
                Epoch 344/1000, Loss: 0,001375905737770768
                Epoch 345/1000, Loss: 0,0013757030261015795
                Epoch 346/1000, Loss: 0,0013755020112932827
                Epoch 347/1000, Loss: 0,001375302678614394
                Epoch 348/1000, Loss: 0,0013751050135367062
                Epoch 349/1000, Loss: 0,001374909001729161
                Epoch 350/1000, Loss: 0,0013747146290518945
                Epoch 351/1000, Loss: 0,0013745218815504503
                Epoch 352/1000, Loss: 0,0013743307454501623
                Epoch 353/1000, Loss: 0,0013741412071507052
                Epoch 354/1000, Loss: 0,001373953253220803
                Epoch 355/1000, Loss: 0,0013737668703931072
                Epoch 356/1000, Loss: 0,0013735820455592275
                Epoch 357/1000, Loss: 0,0013733987657649186
                Epoch 358/1000, Loss: 0,0013732170182054295
                Epoch 359/1000, Loss: 0,0013730367902209941
                Epoch 360/1000, Loss: 0,001372858069292476
                Epoch 361/1000, Loss: 0,0013726808430371592
                Epoch 362/1000, Loss: 0,0013725050992046829
                Epoch 363/1000, Loss: 0,0013723308256731158
                Epoch 364/1000, Loss: 0,0013721580104451652
                Epoch 365/1000, Loss: 0,0013719866416445317
                Epoch 366/1000, Loss: 0,0013718167075123812
                Epoch 367/1000, Loss: 0,0013716481964039615
                Epoch 368/1000, Loss: 0,0013714810967853344
                Epoch 369/1000, Loss: 0,0013713153972302375
                Epoch 370/1000, Loss: 0,0013711510864170607
                Epoch 371/1000, Loss: 0,0013709881531259512
                Epoch 372/1000, Loss: 0,0013708265862360223
                Epoch 373/1000, Loss: 0,001370666374722679
                Epoch 374/1000, Loss: 0,001370507507655056
                Epoch 375/1000, Loss: 0,0013703499741935532
                Epoch 376/1000, Loss: 0,001370193763587485
                Epoch 377/1000, Loss: 0,0013700388651728221
                Epoch 378/1000, Loss: 0,0013698852683700353
                Epoch 379/1000, Loss: 0,0013697329626820326
                Epoch 380/1000, Loss: 0,0013695819376921866
                Epoch 381/1000, Loss: 0,0013694321830624578
                Epoch 382/1000, Loss: 0,0013692836885315957
                Epoch 383/1000, Loss: 0,0013691364439134281
                Epoch 384/1000, Loss: 0,0013689904390952337
                Epoch 385/1000, Loss: 0,0013688456640361898
                Epoch 386/1000, Loss: 0,0013687021087658978
                Epoch 387/1000, Loss: 0,0013685597633829763
                Epoch 388/1000, Loss: 0,00136841861805374
                Epoch 389/1000, Loss: 0,0013682786630109247
                Epoch 390/1000, Loss: 0,001368139888552501
                Epoch 391/1000, Loss: 0,0013680022850405351
                Epoch 392/1000, Loss: 0,0013678658429001156
                Epoch 393/1000, Loss: 0,0013677305526183458
                Epoch 394/1000, Loss: 0,0013675964047433792
                Epoch 395/1000, Loss: 0,001367463389883522
                Epoch 396/1000, Loss: 0,001367331498706381
                Epoch 397/1000, Loss: 0,0013672007219380615
                Epoch 398/1000, Loss: 0,0013670710503624168
                Epoch 399/1000, Loss: 0,0013669424748203392
                Epoch 400/1000, Loss: 0,0013668149862090973
                Epoch 401/1000, Loss: 0,0013666885754817172
                Epoch 402/1000, Loss: 0,001366563233646398
                Epoch 403/1000, Loss: 0,0013664389517659698
                Epoch 404/1000, Loss: 0,0013663157209573894
                Epoch 405/1000, Loss: 0,0013661935323912642
                Epoch 406/1000, Loss: 0,0013660723772914176
                Epoch 407/1000, Loss: 0,0013659522469344784
                Epoch 408/1000, Loss: 0,0013658331326495037
                Epoch 409/1000, Loss: 0,0013657150258176286
                Epoch 410/1000, Loss: 0,001365597917871744
                Epoch 411/1000, Loss: 0,001365481800296197
                Epoch 412/1000, Loss: 0,0013653666646265175
                Epoch 413/1000, Loss: 0,0013652525024491669
                Epoch 414/1000, Loss: 0,001365139305401304
                Epoch 415/1000, Loss: 0,0013650270651705768
                Epoch 416/1000, Loss: 0,0013649157734949297
                Epoch 417/1000, Loss: 0,0013648054221624259
                Epoch 418/1000, Loss: 0,0013646960030110882
                Epoch 419/1000, Loss: 0,0013645875079287583
                Epoch 420/1000, Loss: 0,001364479928852962
                Epoch 421/1000, Loss: 0,0013643732577707937
                Epoch 422/1000, Loss: 0,0013642674867188115
                Epoch 423/1000, Loss: 0,0013641626077829413
                Epoch 424/1000, Loss: 0,001364058613098394
                Epoch 425/1000, Loss: 0,001363955494849588
                Epoch 426/1000, Loss: 0,0013638532452700846
                Epoch 427/1000, Loss: 0,001363751856642527
                Epoch 428/1000, Loss: 0,0013636513212985882
                Epoch 429/1000, Loss: 0,0013635516316189236
                Epoch 430/1000, Loss: 0,0013634527800331312
                Epoch 431/1000, Loss: 0,001363354759019713
                Epoch 432/1000, Loss: 0,001363257561106043
                Epoch 433/1000, Loss: 0,0013631611788683378
                Epoch 434/1000, Loss: 0,0013630656049316296
                Epoch 435/1000, Loss: 0,0013629708319697423
                Epoch 436/1000, Loss: 0,0013628768527052678
                Epoch 437/1000, Loss: 0,0013627836599095459
                Epoch 438/1000, Loss: 0,0013626912464026423
                Epoch 439/1000, Loss: 0,0013625996050533272
                Epoch 440/1000, Loss: 0,0013625087287790565
                Epoch 441/1000, Loss: 0,00136241861054595
                Epoch 442/1000, Loss: 0,0013623292433687679
                Epoch 443/1000, Loss: 0,0013622406203108888
                Epoch 444/1000, Loss: 0,0013621527344842837
                Epoch 445/1000, Loss: 0,0013620655790494893
                Epoch 446/1000, Loss: 0,0013619791472155775
                Epoch 447/1000, Loss: 0,0013618934322401283
                Epoch 448/1000, Loss: 0,001361808427429188
                Epoch 449/1000, Loss: 0,0013617241261372373
                Epoch 450/1000, Loss: 0,0013616405217671494
                Epoch 451/1000, Loss: 0,0013615576077701458
                Epoch 452/1000, Loss: 0,001361475377645748
                Epoch 453/1000, Loss: 0,0013613938249417263
                Epoch 454/1000, Loss: 0,001361312943254046
                Epoch 455/1000, Loss: 0,001361232726226809
                Epoch 456/1000, Loss: 0,001361153167552186
                Epoch 457/1000, Loss: 0,0013610742609703546
                Epoch 458/1000, Loss: 0,0013609960002694227
                Epoch 459/1000, Loss: 0,001360918379285357
                Epoch 460/1000, Loss: 0,0013608413919018968
                Epoch 461/1000, Loss: 0,0013607650320504728
                Epoch 462/1000, Loss: 0,0013606892937101163
                Epoch 463/1000, Loss: 0,001360614170907363
                Epoch 464/1000, Loss: 0,0013605396577161554
                Epoch 465/1000, Loss: 0,0013604657482577391
                Epoch 466/1000, Loss: 0,0013603924367005522
                Epoch 467/1000, Loss: 0,0013603197172601122
                Epoch 468/1000, Loss: 0,0013602475841989017
                Epoch 469/1000, Loss: 0,0013601760318262394
                Epoch 470/1000, Loss: 0,0013601050544981578
                Epoch 471/1000, Loss: 0,0013600346466172693
                Epoch 472/1000, Loss: 0,0013599648026326268
                Epoch 473/1000, Loss: 0,001359895517039587
                Epoch 474/1000, Loss: 0,0013598267843796612
                Epoch 475/1000, Loss: 0,001359758599240366
                Epoch 476/1000, Loss: 0,0013596909562550684
                Epoch 477/1000, Loss: 0,0013596238501028255
                Epoch 478/1000, Loss: 0,0013595572755082214
                Epoch 479/1000, Loss: 0,0013594912272412011
                Epoch 480/1000, Loss: 0,0013594257001168968
                Epoch 481/1000, Loss: 0,0013593606889954484
                Epoch 482/1000, Loss: 0,0013592961887818303
                Epoch 483/1000, Loss: 0,0013592321944256617
                Epoch 484/1000, Loss: 0,0013591687009210196
                Epoch 485/1000, Loss: 0,001359105703306248
                Epoch 486/1000, Loss: 0,00135904319666376
                Epoch 487/1000, Loss: 0,0013589811761198411
                Epoch 488/1000, Loss: 0,001358919636844444
                Epoch 489/1000, Loss: 0,0013588585740509826
                Epoch 490/1000, Loss: 0,0013587979829961234
                Epoch 491/1000, Loss: 0,001358737858979572
                Epoch 492/1000, Loss: 0,001358678197343855
                Epoch 493/1000, Loss: 0,0013586189934741034
                Epoch 494/1000, Loss: 0,001358560242797828
                Epoch 495/1000, Loss: 0,0013585019407846954
                Epoch 496/1000, Loss: 0,0013584440829462992
                Epoch 497/1000, Loss: 0,0013583866648359299
                Epoch 498/1000, Loss: 0,001358329682048341
                Epoch 499/1000, Loss: 0,0013582731302195128
                Epoch 500/1000, Loss: 0,001358217005026414
                Epoch 501/1000, Loss: 0,0013581613021867625
                Epoch 502/1000, Loss: 0,0013581060174587805
                Epoch 503/1000, Loss: 0,0013580511466409484
                Epoch 504/1000, Loss: 0,0013579966855717638
                Epoch 505/1000, Loss: 0,001357942630129484
                Epoch 506/1000, Loss: 0,0013578889762318814
                Epoch 507/1000, Loss: 0,0013578357198359872
                Epoch 508/1000, Loss: 0,001357782856937839
                Epoch 509/1000, Loss: 0,0013577303835722238
                Epoch 510/1000, Loss: 0,0013576782958124225
                Epoch 511/1000, Loss: 0,001357626589769948
                Epoch 512/1000, Loss: 0,0013575752615942866
                Epoch 513/1000, Loss: 0,0013575243074726376
                Epoch 514/1000, Loss: 0,0013574737236296485
                Epoch 515/1000, Loss: 0,0013574235063271542
                Epoch 516/1000, Loss: 0,0013573736518639103
                Epoch 517/1000, Loss: 0,0013573241565753278
                Epoch 518/1000, Loss: 0,0013572750168332078
                Epoch 519/1000, Loss: 0,0013572262290454749
                Epoch 520/1000, Loss: 0,0013571777896559077
                Epoch 521/1000, Loss: 0,0013571296951438712
                Epoch 522/1000, Loss: 0,0013570819420240512
                Epoch 523/1000, Loss: 0,0013570345268461791
                Epoch 524/1000, Loss: 0,0013569874461947685
                Epoch 525/1000, Loss: 0,0013569406966888419
                Epoch 526/1000, Loss: 0,001356894274981662
                Epoch 527/1000, Loss: 0,0013568481777604595
                Epoch 528/1000, Loss: 0,0013568024017461679
                Epoch 529/1000, Loss: 0,0013567569436931473
                Epoch 530/1000, Loss: 0,001356711800388918
                Epoch 531/1000, Loss: 0,001356666968653889
                Epoch 532/1000, Loss: 0,0013566224453410898
                Epoch 533/1000, Loss: 0,0013565782273358986
                Epoch 534/1000, Loss: 0,0013565343115557742
                Epoch 535/1000, Loss: 0,001356490694949987
                Epoch 536/1000, Loss: 0,0013564473744993495
                Epoch 537/1000, Loss: 0,0013564043472159499
                Epoch 538/1000, Loss: 0,0013563616101428832
                Epoch 539/1000, Loss: 0,0013563191603539844
                Epoch 540/1000, Loss: 0,0013562769949535625
                Epoch 541/1000, Loss: 0,001356235111076133
                Epoch 542/1000, Loss: 0,0013561935058861564
                Epoch 543/1000, Loss: 0,0013561521765777702
                Epoch 544/1000, Loss: 0,001356111120374528
                Epoch 545/1000, Loss: 0,0013560703345291334
                Epoch 546/1000, Loss: 0,0013560298163231839
                Epoch 547/1000, Loss: 0,001355989563066905
                Epoch 548/1000, Loss: 0,0013559495720988912
                Epoch 549/1000, Loss: 0,0013559098407858502
                Epoch 550/1000, Loss: 0,001355870366522339
                Epoch 551/1000, Loss: 0,0013558311467305145
                Epoch 552/1000, Loss: 0,0013557921788598711
                Epoch 553/1000, Loss: 0,0013557534603869915
                Epoch 554/1000, Loss: 0,0013557149888152888
                Epoch 555/1000, Loss: 0,0013556767616747566
                Epoch 556/1000, Loss: 0,0013556387765217184
                Epoch 557/1000, Loss: 0,0013556010309385765
                Epoch 558/1000, Loss: 0,001355563522533563
                Epoch 559/1000, Loss: 0,001355526248940497
                Epoch 560/1000, Loss: 0,00135548920781853
                Epoch 561/1000, Loss: 0,001355452396851912
                Epoch 562/1000, Loss: 0,0013554158137497393
                Epoch 563/1000, Loss: 0,001355379456245719
                Epoch 564/1000, Loss: 0,0013553433220979253
                Epoch 565/1000, Loss: 0,001355307409088561
                Epoch 566/1000, Loss: 0,00135527171502372
                Epoch 567/1000, Loss: 0,0013552362377331538
                Epoch 568/1000, Loss: 0,001355200975070033
                Epoch 569/1000, Loss: 0,0013551659249107176
                Epoch 570/1000, Loss: 0,0013551310851545233
                Epoch 571/1000, Loss: 0,0013550964537234922
                Epoch 572/1000, Loss: 0,0013550620285621663
                Epoch 573/1000, Loss: 0,001355027807637356
                Epoch 574/1000, Loss: 0,0013549937889379196
                Epoch 575/1000, Loss: 0,0013549599704745375
                Epoch 576/1000, Loss: 0,001354926350279491
                Epoch 577/1000, Loss: 0,0013548929264064371
                Epoch 578/1000, Loss: 0,001354859696930198
                Epoch 579/1000, Loss: 0,0013548266599465366
                Epoch 580/1000, Loss: 0,0013547938135719435
                Epoch 581/1000, Loss: 0,001354761155943423
                Epoch 582/1000, Loss: 0,0013547286852182792
                Epoch 583/1000, Loss: 0,0013546963995739088
                Epoch 584/1000, Loss: 0,0013546642972075862
                Epoch 585/1000, Loss: 0,0013546323763362611
                Epoch 586/1000, Loss: 0,0013546006351963498
                Epoch 587/1000, Loss: 0,001354569072043533
                Epoch 588/1000, Loss: 0,0013545376851525483
                Epoch 589/1000, Loss: 0,0013545064728169973
                Epoch 590/1000, Loss: 0,0013544754333491386
                Epoch 591/1000, Loss: 0,0013544445650796953
                Epoch 592/1000, Loss: 0,0013544138663576547
                Epoch 593/1000, Loss: 0,0013543833355500795
                Epoch 594/1000, Loss: 0,0013543529710419088
                Epoch 595/1000, Loss: 0,0013543227712357717
                Epoch 596/1000, Loss: 0,0013542927345517966
                Epoch 597/1000, Loss: 0,0013542628594274224
                Epoch 598/1000, Loss: 0,0013542331443172138
                Epoch 599/1000, Loss: 0,0013542035876926758
                Epoch 600/1000, Loss: 0,001354174188042072
                Epoch 601/1000, Loss: 0,001354144943870242
                Epoch 602/1000, Loss: 0,001354115853698423
                Epoch 603/1000, Loss: 0,0013540869160640716
                Epoch 604/1000, Loss: 0,0013540581295206871
                Epoch 605/1000, Loss: 0,0013540294926376374
                Epoch 606/1000, Loss: 0,0013540010039999855
                Epoch 607/1000, Loss: 0,0013539726622083183
                Epoch 608/1000, Loss: 0,001353944465878576
                Epoch 609/1000, Loss: 0,001353916413641887
                Epoch 610/1000, Loss: 0,001353888504144394
                Epoch 611/1000, Loss: 0,0013538607360470982
                Epoch 612/1000, Loss: 0,0013538331080256885
                Epoch 613/1000, Loss: 0,001353805618770383
                Epoch 614/1000, Loss: 0,0013537782669857683
                Epoch 615/1000, Loss: 0,001353751051390639
                Epoch 616/1000, Loss: 0,001353723970717843
                Epoch 617/1000, Loss: 0,0013536970237141232
                Epoch 618/1000, Loss: 0,0013536702091399653
                Epoch 619/1000, Loss: 0,0013536435257694437
                Epoch 620/1000, Loss: 0,00135361697239007
                Epoch 621/1000, Loss: 0,0013535905478026466
                Epoch 622/1000, Loss: 0,0013535642508211128
                Epoch 623/1000, Loss: 0,001353538080272404
                Epoch 624/1000, Loss: 0,0013535120349963022
                Epoch 625/1000, Loss: 0,0013534861138452948
                Epoch 626/1000, Loss: 0,00135346031568443
                Epoch 627/1000, Loss: 0,0013534346393911797
                Epoch 628/1000, Loss: 0,0013534090838552945
                Epoch 629/1000, Loss: 0,001353383647978671
                Epoch 630/1000, Loss: 0,0013533583306752136
                Epoch 631/1000, Loss: 0,0013533331308706966
                Epoch 632/1000, Loss: 0,0013533080475026337
                Epoch 633/1000, Loss: 0,001353283079520146
                Epoch 634/1000, Loss: 0,0013532582258838272
                Epoch 635/1000, Loss: 0,0013532334855656189
                Epoch 636/1000, Loss: 0,0013532088575486775
                Epoch 637/1000, Loss: 0,0013531843408272526
                Epoch 638/1000, Loss: 0,0013531599344065543
                Epoch 639/1000, Loss: 0,001353135637302638
                Epoch 640/1000, Loss: 0,0013531114485422738
                Epoch 641/1000, Loss: 0,001353087367162827
                Epoch 642/1000, Loss: 0,0013530633922121415
                Epoch 643/1000, Loss: 0,0013530395227484156
                Epoch 644/1000, Loss: 0,0013530157578400877
                Epoch 645/1000, Loss: 0,0013529920965657176
                Epoch 646/1000, Loss: 0,0013529685380138732
                Epoch 647/1000, Loss: 0,001352945081283015
                Epoch 648/1000, Loss: 0,0013529217254813855
                Epoch 649/1000, Loss: 0,001352898469726894
                Epoch 650/1000, Loss: 0,0013528753131470095
                Epoch 651/1000, Loss: 0,0013528522548786515
                Epoch 652/1000, Loss: 0,00135282929406808
                Epoch 653/1000, Loss: 0,0013528064298707915
                Epoch 654/1000, Loss: 0,0013527836614514101
                Epoch 655/1000, Loss: 0,0013527609879835875
                Epoch 656/1000, Loss: 0,0013527384086498941
                Epoch 657/1000, Loss: 0,0013527159226417221
                Epoch 658/1000, Loss: 0,0013526935291591842
                Epoch 659/1000, Loss: 0,001352671227411008
                Epoch 660/1000, Loss: 0,001352649016614445
                Epoch 661/1000, Loss: 0,0013526268959951672
                Epoch 662/1000, Loss: 0,001352604864787174
                Epoch 663/1000, Loss: 0,0013525829222326947
                Epoch 664/1000, Loss: 0,0013525610675820961
                Epoch 665/1000, Loss: 0,0013525393000937862
                Epoch 666/1000, Loss: 0,0013525176190341249
                Epoch 667/1000, Loss: 0,0013524960236773326
                Epoch 668/1000, Loss: 0,001352474513305396
                Epoch 669/1000, Loss: 0,001352453087207984
                Epoch 670/1000, Loss: 0,001352431744682358
                Epoch 671/1000, Loss: 0,0013524104850332814
                Epoch 672/1000, Loss: 0,0013523893075729383
                Epoch 673/1000, Loss: 0,001352368211620846
                Epoch 674/1000, Loss: 0,0013523471965037695
                Epoch 675/1000, Loss: 0,0013523262615556405
                Epoch 676/1000, Loss: 0,0013523054061174741
                Epoch 677/1000, Loss: 0,0013522846295372874
                Epoch 678/1000, Loss: 0,0013522639311700185
                Epoch 679/1000, Loss: 0,0013522433103774472
                Epoch 680/1000, Loss: 0,0013522227665281176
                Epoch 681/1000, Loss: 0,001352202298997259
                Epoch 682/1000, Loss: 0,0013521819071667098
                Epoch 683/1000, Loss: 0,0013521615904248398
                Epoch 684/1000, Loss: 0,0013521413481664788
                Epoch 685/1000, Loss: 0,0013521211797928368
                Epoch 686/1000, Loss: 0,0013521010847114378
                Epoch 687/1000, Loss: 0,0013520810623360404
                Epoch 688/1000, Loss: 0,00135206111208657
                Epoch 689/1000, Loss: 0,0013520412333890477
                Epoch 690/1000, Loss: 0,0013520214256755182
                Epoch 691/1000, Loss: 0,0013520016883839842
                Epoch 692/1000, Loss: 0,0013519820209583334
                Epoch 693/1000, Loss: 0,001351962422848274
                Epoch 694/1000, Loss: 0,0013519428935092695
                Epoch 695/1000, Loss: 0,0013519234324024661
                Epoch 696/1000, Loss: 0,0013519040389946345
                Epoch 697/1000, Loss: 0,0013518847127581021
                Epoch 698/1000, Loss: 0,001351865453170689
                Epoch 699/1000, Loss: 0,0013518462597156447
                Epoch 700/1000, Loss: 0,001351827131881589
                Epoch 701/1000, Loss: 0,001351808069162445
                Epoch 702/1000, Loss: 0,0013517890710573856
                Epoch 703/1000, Loss: 0,001351770137070764
                Epoch 704/1000, Loss: 0,0013517512667120628
                Epoch 705/1000, Loss: 0,0013517324594958307
                Epoch 706/1000, Loss: 0,0013517137149416266
                Epoch 707/1000, Loss: 0,0013516950325739591
                Epoch 708/1000, Loss: 0,0013516764119222335
                Epoch 709/1000, Loss: 0,0013516578525206936
                Epoch 710/1000, Loss: 0,0013516393539083674
                Epoch 711/1000, Loss: 0,0013516209156290118
                Epoch 712/1000, Loss: 0,0013516025372310582
                Epoch 713/1000, Loss: 0,001351584218267559
                Epoch 714/1000, Loss: 0,0013515659582961363
                Epoch 715/1000, Loss: 0,0013515477568789285
                Epoch 716/1000, Loss: 0,001351529613582538
                Epoch 717/1000, Loss: 0,0013515115279779818
                Epoch 718/1000, Loss: 0,0013514934996406414
                Epoch 719/1000, Loss: 0,0013514755281502093
                Epoch 720/1000, Loss: 0,0013514576130906457
                Epoch 721/1000, Loss: 0,0013514397540501243
                Epoch 722/1000, Loss: 0,0013514219506209884
                Epoch 723/1000, Loss: 0,0013514042023997019
                Epoch 724/1000, Loss: 0,0013513865089868006
                Epoch 725/1000, Loss: 0,0013513688699868483
                Epoch 726/1000, Loss: 0,0013513512850083923
                Epoch 727/1000, Loss: 0,0013513337536639129
                Epoch 728/1000, Loss: 0,0013513162755697833
                Epoch 729/1000, Loss: 0,001351298850346225
                Epoch 730/1000, Loss: 0,0013512814776172597
                Epoch 731/1000, Loss: 0,0013512641570106747
                Epoch 732/1000, Loss: 0,0013512468881579699
                Epoch 733/1000, Loss: 0,0013512296706943245
                Epoch 734/1000, Loss: 0,001351212504258551
                Epoch 735/1000, Loss: 0,0013511953884930525
                Epoch 736/1000, Loss: 0,0013511783230437874
                Epoch 737/1000, Loss: 0,0013511613075602243
                Epoch 738/1000, Loss: 0,0013511443416953047
                Epoch 739/1000, Loss: 0,0013511274251054033
                Epoch 740/1000, Loss: 0,0013511105574502884
                Epoch 741/1000, Loss: 0,001351093738393086
                Epoch 742/1000, Loss: 0,0013510769676002383
                Epoch 743/1000, Loss: 0,0013510602447414693
                Epoch 744/1000, Loss: 0,0013510435694897474
                Epoch 745/1000, Loss: 0,0013510269415212468
                Epoch 746/1000, Loss: 0,0013510103605153141
                Epoch 747/1000, Loss: 0,0013509938261544326
                Epoch 748/1000, Loss: 0,0013509773381241822
                Epoch 749/1000, Loss: 0,0013509608961132123
                Epoch 750/1000, Loss: 0,0013509444998132006
                Epoch 751/1000, Loss: 0,0013509281489188234
                Epoch 752/1000, Loss: 0,0013509118431277206
                Epoch 753/1000, Loss: 0,0013508955821404614
                Epoch 754/1000, Loss: 0,0013508793656605137
                Epoch 755/1000, Loss: 0,0013508631933942094
                Epoch 756/1000, Loss: 0,0013508470650507136
                Epoch 757/1000, Loss: 0,0013508309803419948
                Epoch 758/1000, Loss: 0,0013508149389827875
                Epoch 759/1000, Loss: 0,0013507989406905702
                Epoch 760/1000, Loss: 0,0013507829851855274
                Epoch 761/1000, Loss: 0,001350767072190522
                Epoch 762/1000, Loss: 0,0013507512014310668
                Epoch 763/1000, Loss: 0,0013507353726352933
                Epoch 764/1000, Loss: 0,0013507195855339226
                Epoch 765/1000, Loss: 0,0013507038398602386
                Epoch 766/1000, Loss: 0,0013506881353500567
                Epoch 767/1000, Loss: 0,0013506724717416985
                Epoch 768/1000, Loss: 0,001350656848775961
                Epoch 769/1000, Loss: 0,0013506412661960922
                Epoch 770/1000, Loss: 0,0013506257237477623
                Epoch 771/1000, Loss: 0,0013506102211790368
                Epoch 772/1000, Loss: 0,0013505947582403508
                Epoch 773/1000, Loss: 0,001350579334684483
                Epoch 774/1000, Loss: 0,0013505639502665297
                Epoch 775/1000, Loss: 0,001350548604743877
                Epoch 776/1000, Loss: 0,0013505332978761796
                Epoch 777/1000, Loss: 0,0013505180294253326
                Epoch 778/1000, Loss: 0,0013505027991554481
                Epoch 779/1000, Loss: 0,0013504876068328312
                Epoch 780/1000, Loss: 0,0013504724522259554
                Epoch 781/1000, Loss: 0,0013504573351054397
                Epoch 782/1000, Loss: 0,001350442255244022
                Epoch 783/1000, Loss: 0,00135042721241654
                Epoch 784/1000, Loss: 0,0013504122063999063
                Epoch 785/1000, Loss: 0,0013503972369730858
                Epoch 786/1000, Loss: 0,0013503823039170732
                Epoch 787/1000, Loss: 0,0013503674070148694
                Epoch 788/1000, Loss: 0,0013503525460514647
                Epoch 789/1000, Loss: 0,001350337720813811
                Epoch 790/1000, Loss: 0,001350322931090803
                Epoch 791/1000, Loss: 0,001350308176673259
                Epoch 792/1000, Loss: 0,0013502934573538963
                Epoch 793/1000, Loss: 0,0013502787729273136
                Epoch 794/1000, Loss: 0,0013502641231899692
                Epoch 795/1000, Loss: 0,001350249507940161
                Epoch 796/1000, Loss: 0,0013502349269780064
                Epoch 797/1000, Loss: 0,0013502203801054231
                Epoch 798/1000, Loss: 0,00135020586712611
                Epoch 799/1000, Loss: 0,001350191387845526
                Epoch 800/1000, Loss: 0,001350176942070876
                Epoch 801/1000, Loss: 0,0013501625296110837
                Epoch 802/1000, Loss: 0,0013501481502767808
                Epoch 803/1000, Loss: 0,0013501338038802864
                Epoch 804/1000, Loss: 0,0013501194902355853
                Epoch 805/1000, Loss: 0,0013501052091583164
                Epoch 806/1000, Loss: 0,0013500909604657503
                Epoch 807/1000, Loss: 0,001350076743976772
                Epoch 808/1000, Loss: 0,001350062559511866
                Epoch 809/1000, Loss: 0,001350048406893099
                Epoch 810/1000, Loss: 0,0013500342859440995
                Epoch 811/1000, Loss: 0,0013500201964900462
                Epoch 812/1000, Loss: 0,001350006138357647
                Epoch 813/1000, Loss: 0,0013499921113751246
                Epoch 814/1000, Loss: 0,0013499781153722028
                Epoch 815/1000, Loss: 0,001349964150180086
                Epoch 816/1000, Loss: 0,0013499502156314458
                Epoch 817/1000, Loss: 0,0013499363115604052
                Epoch 818/1000, Loss: 0,0013499224378025246
                Epoch 819/1000, Loss: 0,0013499085941947844
                Epoch 820/1000, Loss: 0,0013498947805755707
                Epoch 821/1000, Loss: 0,0013498809967846617
                Epoch 822/1000, Loss: 0,0013498672426632105
                Epoch 823/1000, Loss: 0,0013498535180537323
                Epoch 824/1000, Loss: 0,0013498398228000913
                Epoch 825/1000, Loss: 0,001349826156747484
                Epoch 826/1000, Loss: 0,0013498125197424268
                Epoch 827/1000, Loss: 0,001349798911632739
                Epoch 828/1000, Loss: 0,0013497853322675364
                Epoch 829/1000, Loss: 0,0013497717814972097
                Epoch 830/1000, Loss: 0,0013497582591734149
                Epoch 831/1000, Loss: 0,0013497447651490608
                Epoch 832/1000, Loss: 0,001349731299278295
                Epoch 833/1000, Loss: 0,001349717861416489
                Epoch 834/1000, Loss: 0,0013497044514202295
                Epoch 835/1000, Loss: 0,0013496910691473013
                Epoch 836/1000, Loss: 0,0013496777144566784
                Epoch 837/1000, Loss: 0,0013496643872085118
                Epoch 838/1000, Loss: 0,001349651087264113
                Epoch 839/1000, Loss: 0,0013496378144859475
                Epoch 840/1000, Loss: 0,0013496245687376176
                Epoch 841/1000, Loss: 0,0013496113498838555
                Epoch 842/1000, Loss: 0,001349598157790508
                Epoch 843/1000, Loss: 0,0013495849923245268
                Epoch 844/1000, Loss: 0,0013495718533539577
                Epoch 845/1000, Loss: 0,0013495587407479268
                Epoch 846/1000, Loss: 0,0013495456543766325
                Epoch 847/1000, Loss: 0,001349532594111332
                Epoch 848/1000, Loss: 0,0013495195598243318
                Epoch 849/1000, Loss: 0,001349506551388976
                Epoch 850/1000, Loss: 0,0013494935686796385
                Epoch 851/1000, Loss: 0,0013494806115717066
                Epoch 852/1000, Loss: 0,0013494676799415775
                Epoch 853/1000, Loss: 0,0013494547736666446
                Epoch 854/1000, Loss: 0,0013494418926252854
                Epoch 855/1000, Loss: 0,0013494290366968561
                Epoch 856/1000, Loss: 0,0013494162057616776
                Epoch 857/1000, Loss: 0,0013494033997010287
                Epoch 858/1000, Loss: 0,0013493906183971352
                Epoch 859/1000, Loss: 0,0013493778617331578
                Epoch 860/1000, Loss: 0,0013493651295931892
                Epoch 861/1000, Loss: 0,0013493524218622373
                Epoch 862/1000, Loss: 0,0013493397384262199
                Epoch 863/1000, Loss: 0,001349327079171956
                Epoch 864/1000, Loss: 0,0013493144439871549
                Epoch 865/1000, Loss: 0,0013493018327604084
                Epoch 866/1000, Loss: 0,0013492892453811805
                Epoch 867/1000, Loss: 0,0013492766817398008
                Epoch 868/1000, Loss: 0,001349264141727456
                Epoch 869/1000, Loss: 0,0013492516252361774
                Epoch 870/1000, Loss: 0,0013492391321588365
                Epoch 871/1000, Loss: 0,0013492266623891355
                Epoch 872/1000, Loss: 0,0013492142158215996
                Epoch 873/1000, Loss: 0,0013492017923515658
                Epoch 874/1000, Loss: 0,0013491893918751792
                Epoch 875/1000, Loss: 0,0013491770142893826
                Epoch 876/1000, Loss: 0,0013491646594919085
                Epoch 877/1000, Loss: 0,0013491523273812713
                Epoch 878/1000, Loss: 0,0013491400178567616
                Epoch 879/1000, Loss: 0,0013491277308184352
                Epoch 880/1000, Loss: 0,0013491154661671074
                Epoch 881/1000, Loss: 0,0013491032238043474
                Epoch 882/1000, Loss: 0,0013490910036324671
                Epoch 883/1000, Loss: 0,001349078805554516
                Epoch 884/1000, Loss: 0,001349066629474274
                Epoch 885/1000, Loss: 0,001349054475296244
                Epoch 886/1000, Loss: 0,001349042342925644
                Epoch 887/1000, Loss: 0,0013490302322684013
                Epoch 888/1000, Loss: 0,0013490181432311457
                Epoch 889/1000, Loss: 0,0013490060757212008
                Epoch 890/1000, Loss: 0,00134899402964658
                Epoch 891/1000, Loss: 0,0013489820049159772
                Epoch 892/1000, Loss: 0,0013489700014387624
                Epoch 893/1000, Loss: 0,0013489580191249735
                Epoch 894/1000, Loss: 0,0013489460578853102
                Epoch 895/1000, Loss: 0,0013489341176311288
                Epoch 896/1000, Loss: 0,0013489221982744336
                Epoch 897/1000, Loss: 0,001348910299727874
                Epoch 898/1000, Loss: 0,001348898421904735
                Epoch 899/1000, Loss: 0,0013488865647189323
                Epoch 900/1000, Loss: 0,0013488747280850071
                Epoch 901/1000, Loss: 0,0013488629119181182
                Epoch 902/1000, Loss: 0,0013488511161340389
                Epoch 903/1000, Loss: 0,0013488393406491492
                Epoch 904/1000, Loss: 0,0013488275853804292
                Epoch 905/1000, Loss: 0,0013488158502454558
                Epoch 906/1000, Loss: 0,001348804135162395
                Epoch 907/1000, Loss: 0,0013487924400499968
                Epoch 908/1000, Loss: 0,0013487807648275907
                Epoch 909/1000, Loss: 0,0013487691094150806
                Epoch 910/1000, Loss: 0,0013487574737329347
                Epoch 911/1000, Loss: 0,0013487458577021876
                Epoch 912/1000, Loss: 0,0013487342612444283
                Epoch 913/1000, Loss: 0,0013487226842817993
                Epoch 914/1000, Loss: 0,0013487111267369892
                Epoch 915/1000, Loss: 0,0013486995885332287
                Epoch 916/1000, Loss: 0,0013486880695942837
                Epoch 917/1000, Loss: 0,0013486765698444536
                Epoch 918/1000, Loss: 0,0013486650892085637
                Epoch 919/1000, Loss: 0,0013486536276119594
                Epoch 920/1000, Loss: 0,0013486421849805057
                Epoch 921/1000, Loss: 0,0013486307612405768
                Epoch 922/1000, Loss: 0,0013486193563190566
                Epoch 923/1000, Loss: 0,001348607970143329
                Epoch 924/1000, Loss: 0,0013485966026412785
                Epoch 925/1000, Loss: 0,0013485852537412813
                Epoch 926/1000, Loss: 0,0013485739233722014
                Epoch 927/1000, Loss: 0,0013485626114633888
                Epoch 928/1000, Loss: 0,0013485513179446727
                Epoch 929/1000, Loss: 0,0013485400427463588
                Epoch 930/1000, Loss: 0,0013485287857992207
                Epoch 931/1000, Loss: 0,0013485175470345008
                Epoch 932/1000, Loss: 0,0013485063263839047
                Epoch 933/1000, Loss: 0,001348495123779594
                Epoch 934/1000, Loss: 0,0013484839391541866
                Epoch 935/1000, Loss: 0,001348472772440748
                Epoch 936/1000, Loss: 0,0013484616235727913
                Epoch 937/1000, Loss: 0,0013484504924842705
                Epoch 938/1000, Loss: 0,0013484393791095774
                Epoch 939/1000, Loss: 0,001348428283383537
                Epoch 940/1000, Loss: 0,001348417205241407
                Epoch 941/1000, Loss: 0,001348406144618865
                Epoch 942/1000, Loss: 0,001348395101452018
                Epoch 943/1000, Loss: 0,0013483840756773864
                Epoch 944/1000, Loss: 0,0013483730672319073
                Epoch 945/1000, Loss: 0,0013483620760529266
                Epoch 946/1000, Loss: 0,0013483511020782005
                Epoch 947/1000, Loss: 0,001348340145245886
                Epoch 948/1000, Loss: 0,0013483292054945414
                Epoch 949/1000, Loss: 0,001348318282763121
                Epoch 950/1000, Loss: 0,0013483073769909716
                Epoch 951/1000, Loss: 0,0013482964881178301
                Epoch 952/1000, Loss: 0,0013482856160838186
                Epoch 953/1000, Loss: 0,001348274760829442
                Epoch 954/1000, Loss: 0,0013482639222955851
                Epoch 955/1000, Loss: 0,0013482531004235074
                Epoch 956/1000, Loss: 0,0013482422951548402
                Epoch 957/1000, Loss: 0,0013482315064315847
                Epoch 958/1000, Loss: 0,001348220734196109
                Epoch 959/1000, Loss: 0,0013482099783911437
                Epoch 960/1000, Loss: 0,0013481992389597765
                Epoch 961/1000, Loss: 0,001348188515845455
                Epoch 962/1000, Loss: 0,001348177808991977
                Epoch 963/1000, Loss: 0,0013481671183434927
                Epoch 964/1000, Loss: 0,001348156443844499
                Epoch 965/1000, Loss: 0,0013481457854398354
                Epoch 966/1000, Loss: 0,0013481351430746844
                Epoch 967/1000, Loss: 0,0013481245166945658
                Epoch 968/1000, Loss: 0,001348113906245335
                Epoch 969/1000, Loss: 0,0013481033116731795
                Epoch 970/1000, Loss: 0,0013480927329246161
                Epoch 971/1000, Loss: 0,00134808216994649
                Epoch 972/1000, Loss: 0,0013480716226859676
                Epoch 973/1000, Loss: 0,0013480610910905385
                Epoch 974/1000, Loss: 0,0013480505751080096
                Epoch 975/1000, Loss: 0,0013480400746865035
                Epoch 976/1000, Loss: 0,0013480295897744569
                Epoch 977/1000, Loss: 0,001348019120320613
                Epoch 978/1000, Loss: 0,0013480086662740292
                Epoch 979/1000, Loss: 0,0013479982275840623
                Epoch 980/1000, Loss: 0,0013479878042003727
                Epoch 981/1000, Loss: 0,0013479773960729223
                Epoch 982/1000, Loss: 0,0013479670031519696
                Epoch 983/1000, Loss: 0,0013479566253880677
                Epoch 984/1000, Loss: 0,001347946262732062
                Epoch 985/1000, Loss: 0,0013479359151350875
                Epoch 986/1000, Loss: 0,0013479255825485676
                Epoch 987/1000, Loss: 0,0013479152649242096
                Epoch 988/1000, Loss: 0,001347904962214005
                Epoch 989/1000, Loss: 0,0013478946743702236
                Epoch 990/1000, Loss: 0,0013478844013454154
                Epoch 991/1000, Loss: 0,0013478741430924047
                Epoch 992/1000, Loss: 0,00134786389956429
                Epoch 993/1000, Loss: 0,0013478536707144386
                Epoch 994/1000, Loss: 0,0013478434564964897
                Epoch 995/1000, Loss: 0,0013478332568643472
                Epoch 996/1000, Loss: 0,0013478230717721806
                Epoch 997/1000, Loss: 0,0013478129011744214
                Epoch 998/1000, Loss: 0,0013478027450257615
                Epoch 999/1000, Loss: 0,0013477926032811486
                Epoch 1000/1000, Loss: 0,0013477824758957888
                Testing trained RNN on physics sentences:
                Input: energy is conserved in system
                Predicted: is conserved in vacuum

                Input: force equals mass times acceleration
                Predicted: equals mass times acceleration

                Input: light travels in vacuum at speed
                Predicted: travels in vacuum at speed

                Input: electron has negative charge
                Predicted: has negative charge

                Input: gravity pulls objects towards earth
                Predicted: pulls objects towards earth
                */

            }
            #endregion 


            Console.ReadLine();

            #region Recurrent Neural Networks
            {
                var project9 = new Project9Oct2024 { Name = "Recurrent Neural Networks" };

                ProjectManager projectManager = new ProjectManager();

                Conversation conversation5 = projectManager.CreateNewConversation(project9);               

                SourceCodeIteration sourceCodeIteration5 = projectManager.CreateNewIteration(conversation5);

                sourceCodeIteration5.SourceCodes.Add(new SourceCode8Nov2024(@"
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string gloveFilePath = ""../../../../../../../GloVe/glove.6B.300d.txt"";
        int embeddingDim = 300;
        var glove = new GloveLoader(gloveFilePath, embeddingDim);

        int hiddenSize = 10;
        double learningRate = 0.1;
        int epochs = 1000;

        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

        // Five physics-related sentences for training
        List<string[]> physicsSentences = new List<string[]>
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
                List<double[]> physicsSentenceEmbeddings = new List<double[]>();
                foreach (var word in sentence)
                {
                    physicsSentenceEmbeddings.Add(glove.GetEmbedding(word));
                }

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

            List<double[]> sentenceEmbeddings = new List<double[]>();
            foreach (var word in sentence)
            {
                sentenceEmbeddings.Add(glove.GetEmbedding(word));
            }

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

            // Replace LINQ `Skip` and `Select` with explicit loop
            var vector = new double[embeddingDim];
            for (int i = 0; i < embeddingDim; i++)
            {
                vector[i] = double.Parse(parts[i + 1], CultureInfo.InvariantCulture) / 10; // Normalize embeddings
            }

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


                var syntaxTrees1 = new[]
                {
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[1].Code),
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[2].Code),
                    CSharpSyntaxTree.ParseText(sourceCodeIteration5.SourceCodes[0].Code)
                };

                var references4 = new List<MetadataReference>
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System
                    MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                    MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location), // System.Collections.Generic
                    MetadataReference.CreateFromFile(typeof(File).Assembly.Location), // System.IO                
                    MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location), // System.Text.RegularExpressions
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location) // System.Runtime
                };


                // Step 4: Compile into a single assembly
                var compilation1 = CSharpCompilation.Create(
                    "MergedAssembly",
                    syntaxTrees1,
                    references4,
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
                Compilation successful!
                Epoch 1/1000, Loss: 0,005197216065723722
                Epoch 2/1000, Loss: 0,005191769010771042
                Epoch 3/1000, Loss: 0,005152508260308301
                Epoch 4/1000, Loss: 0,004916789456603688
                Epoch 5/1000, Loss: 0,0044069096040154215
                Epoch 6/1000, Loss: 0,004208682183920111
                Epoch 7/1000, Loss: 0,0041804938398681615
                Epoch 8/1000, Loss: 0,0041627916879215954
                Epoch 9/1000, Loss: 0,004147161920868977
                Epoch 10/1000, Loss: 0,004133047362915425
                Epoch 11/1000, Loss: 0,0041200987699363326
                Epoch 12/1000, Loss: 0,0041080763185463295
                Epoch 13/1000, Loss: 0,0040968113846363025
                Epoch 14/1000, Loss: 0,0040861812793743565
                Epoch 15/1000, Loss: 0,004076093159775848
                Epoch 16/1000, Loss: 0,004066472893338724
                Epoch 17/1000, Loss: 0,004057256869320735
                Epoch 18/1000, Loss: 0,004048385689537966
                Epoch 19/1000, Loss: 0,004039799075713491
                Epoch 20/1000, Loss: 0,004031431547112783
                Epoch 21/1000, Loss: 0,004023208557913732
                Epoch 22/1000, Loss: 0,0040150428808348555
                Epoch 23/1000, Loss: 0,004006831107968208
                Epoch 24/1000, Loss: 0,003998450233035191
                Epoch 25/1000, Loss: 0,003989754401990579
                Epoch 26/1000, Loss: 0,0039805720910359435
                Epoch 27/1000, Loss: 0,003970704209106048
                Epoch 28/1000, Loss: 0,003959923931116067
                Epoch 29/1000, Loss: 0,003947979429815686
                Epoch 30/1000, Loss: 0,003934601026575167
                Epoch 31/1000, Loss: 0,003919514500327922
                Epoch 32/1000, Loss: 0,003902462172873514
                Epoch 33/1000, Loss: 0,0038832326286627945
                Epoch 34/1000, Loss: 0,0038616981567737144
                Epoch 35/1000, Loss: 0,003837855909786582
                Epoch 36/1000, Loss: 0,0038118644777180863
                Epoch 37/1000, Loss: 0,0037840632453765
                Epoch 38/1000, Loss: 0,003754960186384502
                Epoch 39/1000, Loss: 0,003725178083997689
                Epoch 40/1000, Loss: 0,0036953613521918836
                Epoch 41/1000, Loss: 0,0036660626357264666
                Epoch 42/1000, Loss: 0,00363764157092238
                Epoch 43/1000, Loss: 0,003610207852503068
                Epoch 44/1000, Loss: 0,003583624516684143
                Epoch 45/1000, Loss: 0,003557563612481078
                Epoch 46/1000, Loss: 0,0035315889969147183
                Epoch 47/1000, Loss: 0,003505238191276826
                Epoch 48/1000, Loss: 0,003478084750485303
                Epoch 49/1000, Loss: 0,003449775739359993
                Epoch 50/1000, Loss: 0,0034200482957390315
                Epoch 51/1000, Loss: 0,0033887326261636424
                Epoch 52/1000, Loss: 0,003355747822211521
                Epoch 53/1000, Loss: 0,00332109434710443
                Epoch 54/1000, Loss: 0,0032848448023663985
                Epoch 55/1000, Loss: 0,0032471333042823217
                Epoch 56/1000, Loss: 0,003208143390356318
                Epoch 57/1000, Loss: 0,0031680945131871375
                Epoch 58/1000, Loss: 0,003127227556373801
                Epoch 59/1000, Loss: 0,0030857901807942118
                Epoch 60/1000, Loss: 0,0030440229999761184
                Epoch 61/1000, Loss: 0,0030021475037535364
                Epoch 62/1000, Loss: 0,002960356341134381
                Epoch 63/1000, Loss: 0,0029188061891813133
                Epoch 64/1000, Loss: 0,002877613156504572
                Epoch 65/1000, Loss: 0,0028368506011951108
                Epoch 66/1000, Loss: 0,0027965493477984456
                Epoch 67/1000, Loss: 0,002756700418909744
                Epoch 68/1000, Loss: 0,0027172603931918783
                Epoch 69/1000, Loss: 0,0026781592887355514
                Epoch 70/1000, Loss: 0,00263931049964095
                Epoch 71/1000, Loss: 0,0026006219224891753
                Epoch 72/1000, Loss: 0,0025620071444279464
                Epoch 73/1000, Loss: 0,002523395515059806
                Epoch 74/1000, Loss: 0,002484740099714836
                Epoch 75/1000, Loss: 0,002446022858588466
                Epoch 76/1000, Loss: 0,002407256829948389
                Epoch 77/1000, Loss: 0,0023684855264329598
                Epoch 78/1000, Loss: 0,0023297801017184046
                Epoch 79/1000, Loss: 0,00229123505050565
                Epoch 80/1000, Loss: 0,0022529632375254773
                Epoch 81/1000, Loss: 0,0022150909200003067
                Epoch 82/1000, Loss: 0,002177753182912353
                Epoch 83/1000, Loss: 0,0021410899268590007
                Epoch 84/1000, Loss: 0,0021052423176093358
                Epoch 85/1000, Loss: 0,0020703494838236573
                Epoch 86/1000, Loss: 0,0020365452512084306
                Epoch 87/1000, Loss: 0,0020039548010901383
                Epoch 88/1000, Loss: 0,001972691285660377
                Epoch 89/1000, Loss: 0,0019428525643163572
                Epoch 90/1000, Loss: 0,0019145183052571828
                Epoch 91/1000, Loss: 0,0018877477086449521
                Epoch 92/1000, Loss: 0,0018625780598783687
                Epoch 93/1000, Loss: 0,001839024235149428
                Epoch 94/1000, Loss: 0,001817079181033426
                Epoch 95/1000, Loss: 0,0017967152958850402
                Epoch 96/1000, Loss: 0,0017778865665715455
                Epoch 97/1000, Loss: 0,0017605312658567972
                Epoch 98/1000, Loss: 0,0017445749945223475
                Epoch 99/1000, Loss: 0,0017299338554778536
                Epoch 100/1000, Loss: 0,0017165175698098524
                Epoch 101/1000, Loss: 0,0017042323807713138
                Epoch 102/1000, Loss: 0,0016929836345250526
                Epoch 103/1000, Loss: 0,001682677969914034
                Epoch 104/1000, Loss: 0,001673225088822371
                Epoch 105/1000, Loss: 0,0016645391107538115
                Epoch 106/1000, Loss: 0,0016565395388261476
                Epoch 107/1000, Loss: 0,0016491518796589906
                Epoch 108/1000, Loss: 0,0016423079677579551
                Epoch 109/1000, Loss: 0,0016359460475307707
                Epoch 110/1000, Loss: 0,00163001066459926
                Epoch 111/1000, Loss: 0,0016244524139861567
                Epoch 112/1000, Loss: 0,0016192275871635717
                Epoch 113/1000, Loss: 0,0016142977536743586
                Epoch 114/1000, Loss: 0,0016096293066629641
                Epoch 115/1000, Loss: 0,0016051929955741742
                Epoch 116/1000, Loss: 0,0016009634637483474
                Epoch 117/1000, Loss: 0,0015969188038024793
                Epoch 118/1000, Loss: 0,0015930401395978524
                Epoch 119/1000, Loss: 0,001589311240255429
                Epoch 120/1000, Loss: 0,0015857181690450382
                Epoch 121/1000, Loss: 0,0015822489679712692
                Epoch 122/1000, Loss: 0,0015788933774203225
                Epoch 123/1000, Loss: 0,0015756425892259583
                Epoch 124/1000, Loss: 0,00157248903086964
                Epoch 125/1000, Loss: 0,0015694261781685978
                Epoch 126/1000, Loss: 0,001566448393654781
                Epoch 127/1000, Loss: 0,0015635507878481426
                Epoch 128/1000, Loss: 0,0015607291007314317
                Epoch 129/1000, Loss: 0,0015579796009032383
                Epoch 130/1000, Loss: 0,0015552990000930922
                Epoch 131/1000, Loss: 0,0015526843809463193
                Epoch 132/1000, Loss: 0,0015501331362126123
                Epoch 133/1000, Loss: 0,0015476429176911146
                Epoch 134/1000, Loss: 0,0015452115934902573
                Epoch 135/1000, Loss: 0,001542837212349122
                Epoch 136/1000, Loss: 0,0015405179739373697
                Epoch 137/1000, Loss: 0,0015382522042023622
                Epoch 138/1000, Loss: 0,001536038334965874
                Epoch 139/1000, Loss: 0,0015338748870896992
                Epoch 140/1000, Loss: 0,0015317604566310351
                Epoch 141/1000, Loss: 0,0015296937034963078
                Epoch 142/1000, Loss: 0,001527673342177508
                Epoch 143/1000, Loss: 0,0015256981342198536
                Epoch 144/1000, Loss: 0,0015237668821247352
                Epoch 145/1000, Loss: 0,001521878424439016
                Epoch 146/1000, Loss: 0,0015200316318216937
                Epoch 147/1000, Loss: 0,0015182254039128606
                Epoch 148/1000, Loss: 0,0015164586668585801
                Epoch 149/1000, Loss: 0,0015147303713695757
                Epoch 150/1000, Loss: 0,0015130394912120661
                Epoch 151/1000, Loss: 0,0015113850220463243
                Epoch 152/1000, Loss: 0,0015097659805430165
                Epoch 153/1000, Loss: 0,0015081814037195634
                Epoch 154/1000, Loss: 0,0015066303484489304
                Epoch 155/1000, Loss: 0,0015051118911018034
                Epoch 156/1000, Loss: 0,0015036251272902196
                Epoch 157/1000, Loss: 0,0015021691716866484
                Epoch 158/1000, Loss: 0,0015007431578974771
                Epoch 159/1000, Loss: 0,0014993462383739108
                Epoch 160/1000, Loss: 0,0014979775843467372
                Epoch 161/1000, Loss: 0,0014966363857741578
                Epoch 162/1000, Loss: 0,001495321851294223
                Epoch 163/1000, Loss: 0,0014940332081752987
                Epoch 164/1000, Loss: 0,0014927697022595224
                Epoch 165/1000, Loss: 0,0014915305978955042
                Epoch 166/1000, Loss: 0,0014903151778575242
                Epoch 167/1000, Loss: 0,0014891227432493334
                Epoch 168/1000, Loss: 0,0014879526133913215
                Epoch 169/1000, Loss: 0,0014868041256903652
                Epoch 170/1000, Loss: 0,0014856766354920867
                Epoch 171/1000, Loss: 0,0014845695159156026
                Epoch 172/1000, Loss: 0,0014834821576711
                Epoch 173/1000, Loss: 0,0014824139688607925
                Epoch 174/1000, Loss: 0,0014813643747639577
                Epoch 175/1000, Loss: 0,001480332817606881
                Epoch 176/1000, Loss: 0,0014793187563186248
                Epoch 177/1000, Loss: 0,0014783216662735833
                Epoch 178/1000, Loss: 0,0014773410390218477
                Epoch 179/1000, Loss: 0,001476376382008411
                Epoch 180/1000, Loss: 0,0014754272182822673
                Epoch 181/1000, Loss: 0,0014744930861964405
                Epoch 182/1000, Loss: 0,0014735735390999992
                Epoch 183/1000, Loss: 0,001472668145023056
                Epoch 184/1000, Loss: 0,0014717764863557716
                Epoch 185/1000, Loss: 0,0014708981595223216
                Epoch 186/1000, Loss: 0,001470032774650777
                Epoch 187/1000, Loss: 0,0014691799552398012
                Epoch 188/1000, Loss: 0,0014683393378230494
                Epoch 189/1000, Loss: 0,0014675105716320876
                Epoch 190/1000, Loss: 0,0014666933182586447
                Epoch 191/1000, Loss: 0,0014658872513169533
                Epoch 192/1000, Loss: 0,0014650920561068803
                Epoch 193/1000, Loss: 0,0014643074292785437
                Epoch 194/1000, Loss: 0,001463533078499032
                Epoch 195/1000, Loss: 0,0014627687221218243
                Epoch 196/1000, Loss: 0,0014620140888594556
                Epoch 197/1000, Loss: 0,0014612689174599417
                Epoch 198/1000, Loss: 0,0014605329563874222
                Epoch 199/1000, Loss: 0,00145980596350745
                Epoch 200/1000, Loss: 0,001459087705777316
                Epoch 201/1000, Loss: 0,0014583779589417499
                Epoch 202/1000, Loss: 0,0014576765072343103
                Epoch 203/1000, Loss: 0,0014569831430847394
                Epoch 204/1000, Loss: 0,0014562976668325072
                Epoch 205/1000, Loss: 0,0014556198864467628
                Epoch 206/1000, Loss: 0,0014549496172528555
                Epoch 207/1000, Loss: 0,001454286681665556
                Epoch 208/1000, Loss: 0,0014536309089291106
                Epoch 209/1000, Loss: 0,0014529821348641795
                Epoch 210/1000, Loss: 0,0014523402016217422
                Epoch 211/1000, Loss: 0,0014517049574439755
                Epoch 212/1000, Loss: 0,0014510762564321309
                Epoch 213/1000, Loss: 0,0014504539583213825
                Epoch 214/1000, Loss: 0,0014498379282626192
                Epoch 215/1000, Loss: 0,0014492280366111283
                Epoch 216/1000, Loss: 0,0014486241587220905
                Epoch 217/1000, Loss: 0,0014480261747528227
                Epoch 218/1000, Loss: 0,0014474339694716496
                Epoch 219/1000, Loss: 0,0014468474320733075
                Epoch 220/1000, Loss: 0,0014462664560007566
                Epoch 221/1000, Loss: 0,0014456909387732684
                Epoch 222/1000, Loss: 0,0014451207818206627
                Epoch 223/1000, Loss: 0,001444555890323527
                Epoch 224/1000, Loss: 0,0014439961730592994
                Epoch 225/1000, Loss: 0,0014434415422540313
                Epoch 226/1000, Loss: 0,001442891913439689
                Epoch 227/1000, Loss: 0,0014423472053168352
                Epoch 228/1000, Loss: 0,00144180733962251
                Epoch 229/1000, Loss: 0,0014412722410031748
                Epoch 230/1000, Loss: 0,0014407418368925344
                Epoch 231/1000, Loss: 0,0014402160573940826
                Epoch 232/1000, Loss: 0,0014396948351682202
                Epoch 233/1000, Loss: 0,0014391781053237586
                Epoch 234/1000, Loss: 0,001438665805313688
                Epoch 235/1000, Loss: 0,0014381578748350239
                Epoch 236/1000, Loss: 0,0014376542557325961
                Epoch 237/1000, Loss: 0,0014371548919066312
                Epoch 238/1000, Loss: 0,0014366597292239775
                Epoch 239/1000, Loss: 0,0014361687154328382
                Epoch 240/1000, Loss: 0,0014356818000808686
                Epoch 241/1000, Loss: 0,0014351989344365142
                Epoch 242/1000, Loss: 0,0014347200714134515
                Epoch 243/1000, Loss: 0,0014342451654980118
                Epoch 244/1000, Loss: 0,0014337741726794704
                Epoch 245/1000, Loss: 0,001433307050383084
                Epoch 246/1000, Loss: 0,0014328437574057612
                Epoch 247/1000, Loss: 0,0014323842538542663
                Epoch 248/1000, Loss: 0,0014319285010858534
                Epoch 249/1000, Loss: 0,0014314764616512256
                Epoch 250/1000, Loss: 0,0014310280992397366
                Epoch 251/1000, Loss: 0,0014305833786267391
                Epoch 252/1000, Loss: 0,00143014226562299
                Epoch 253/1000, Loss: 0,0014297047270260433
                Epoch 254/1000, Loss: 0,001429270730573539
                Epoch 255/1000, Loss: 0,0014288402448983258
                Epoch 256/1000, Loss: 0,0014284132394853381
                Epoch 257/1000, Loss: 0,001427989684630163
                Epoch 258/1000, Loss: 0,0014275695513992323
                Epoch 259/1000, Loss: 0,0014271528115915861
                Epoch 260/1000, Loss: 0,0014267394377021347
                Epoch 261/1000, Loss: 0,0014263294028863769
                Epoch 262/1000, Loss: 0,0014259226809265136
                Epoch 263/1000, Loss: 0,0014255192461989162
                Epoch 264/1000, Loss: 0,0014251190736428892
                Epoch 265/1000, Loss: 0,0014247221387306888
                Epoch 266/1000, Loss: 0,0014243284174387599
                Epoch 267/1000, Loss: 0,001423937886220136
                Epoch 268/1000, Loss: 0,0014235505219779723
                Epoch 269/1000, Loss: 0,0014231663020401737
                Epoch 270/1000, Loss: 0,0014227852041350762
                Epoch 271/1000, Loss: 0,0014224072063681573
                Epoch 272/1000, Loss: 0,0014220322871997253
                Epoch 273/1000, Loss: 0,0014216604254235776
                Epoch 274/1000, Loss: 0,0014212916001465745
                Epoch 275/1000, Loss: 0,0014209257907691212
                Epoch 276/1000, Loss: 0,0014205629769665092
                Epoch 277/1000, Loss: 0,0014202031386711092
                Epoch 278/1000, Loss: 0,0014198462560553707
                Epoch 279/1000, Loss: 0,0014194923095156185
                Epoch 280/1000, Loss: 0,001419141279656612
                Epoch 281/1000, Loss: 0,0014187931472768455
                Epoch 282/1000, Loss: 0,001418447893354571
                Epoch 283/1000, Loss: 0,001418105499034514
                Epoch 284/1000, Loss: 0,001417765945615264
                Epoch 285/1000, Loss: 0,0014174292145373195
                Epoch 286/1000, Loss: 0,0014170952873717676
                Epoch 287/1000, Loss: 0,0014167641458095715
                Epoch 288/1000, Loss: 0,001416435771651454
                Epoch 289/1000, Loss: 0,001416110146798356
                Epoch 290/1000, Loss: 0,001415787253242446
                Epoch 291/1000, Loss: 0,0014154670730586688
                Epoch 292/1000, Loss: 0,0014151495883968131
                Epoch 293/1000, Loss: 0,0014148347814740808
                Epoch 294/1000, Loss: 0,0014145226345681373
                Epoch 295/1000, Loss: 0,0014142131300106304
                Epoch 296/1000, Loss: 0,0014139062501811637
                Epoch 297/1000, Loss: 0,001413601977501698
                Epoch 298/1000, Loss: 0,0014133002944313757
                Epoch 299/1000, Loss: 0,001413001183461746
                Epoch 300/1000, Loss: 0,0014127046271123814
                Epoch 301/1000, Loss: 0,0014124106079268651
                Epoch 302/1000, Loss: 0,001412119108469133
                Epoch 303/1000, Loss: 0,0014118301113201699
                Epoch 304/1000, Loss: 0,0014115435990750252
                Epoch 305/1000, Loss: 0,0014112595543401534
                Epoch 306/1000, Loss: 0,001410977959731051
                Epoch 307/1000, Loss: 0,0014106987978701887
                Epoch 308/1000, Loss: 0,0014104220513852187
                Epoch 309/1000, Loss: 0,001410147702907447
                Epoch 310/1000, Loss: 0,0014098757350705575
                Epoch 311/1000, Loss: 0,001409606130509577
                Epoch 312/1000, Loss: 0,0014093388718600715
                Epoch 313/1000, Loss: 0,0014090739417575546
                Epoch 314/1000, Loss: 0,0014088113228371084
                Epoch 315/1000, Loss: 0,001408550997733195
                Epoch 316/1000, Loss: 0,0014082929490796606
                Epoch 317/1000, Loss: 0,0014080371595098999
                Epoch 318/1000, Loss: 0,0014077836116572072
                Epoch 319/1000, Loss: 0,001407532288155262
                Epoch 320/1000, Loss: 0,0014072831716387714
                Epoch 321/1000, Loss: 0,0014070362447442509
                Epoch 322/1000, Loss: 0,0014067914901109266
                Epoch 323/1000, Loss: 0,001406548890381763
                Epoch 324/1000, Loss: 0,0014063084282045966
                Epoch 325/1000, Loss: 0,00140607008623338
                Epoch 326/1000, Loss: 0,0014058338471295143
                Epoch 327/1000, Loss: 0,0014055996935632747
                Epoch 328/1000, Loss: 0,0014053676082153165
                Epoch 329/1000, Loss: 0,0014051375737782511
                Epoch 330/1000, Loss: 0,001404909572958298
                Epoch 331/1000, Loss: 0,0014046835884769866
                Epoch 332/1000, Loss: 0,0014044596030729245
                Epoch 333/1000, Loss: 0,0014042375995036037
                Epoch 334/1000, Loss: 0,0014040175605472623
                Epoch 335/1000, Loss: 0,0014037994690047693
                Epoch 336/1000, Loss: 0,0014035833077015582
                Epoch 337/1000, Loss: 0,0014033690594895732
                Epoch 338/1000, Loss: 0,0014031567072492496
                Epoch 339/1000, Loss: 0,0014029462338915064
                Epoch 340/1000, Loss: 0,001402737622359755
                Epoch 341/1000, Loss: 0,001402530855631917
                Epoch 342/1000, Loss: 0,0014023259167224487
                Epoch 343/1000, Loss: 0,0014021227886843665
                Epoch 344/1000, Loss: 0,0014019214546112733
                Epoch 345/1000, Loss: 0,0014017218976393774
                Epoch 346/1000, Loss: 0,001401524100949507
                Epoch 347/1000, Loss: 0,0014013280477691078
                Epoch 348/1000, Loss: 0,0014011337213742337
                Epoch 349/1000, Loss: 0,0014009411050915203
                Epoch 350/1000, Loss: 0,0014007501823001319
                Epoch 351/1000, Loss: 0,001400560936433698
                Epoch 352/1000, Loss: 0,0014003733509822187
                Epoch 353/1000, Loss: 0,0014001874094939492
                Epoch 354/1000, Loss: 0,0014000030955772526
                Epoch 355/1000, Loss: 0,0013998203929024313
                Epoch 356/1000, Loss: 0,0013996392852035145
                Epoch 357/1000, Loss: 0,0013994597562800306
                Epoch 358/1000, Loss: 0,0013992817899987297
                Epoch 359/1000, Loss: 0,0013991053702952837
                Epoch 360/1000, Loss: 0,0013989304811759412
                Epoch 361/1000, Loss: 0,0013987571067191524
                Epoch 362/1000, Loss: 0,00139858523107715
                Epoch 363/1000, Loss: 0,0013984148384774966
                Epoch 364/1000, Loss: 0,0013982459132245884
                Epoch 365/1000, Loss: 0,001398078439701121
                Epoch 366/1000, Loss: 0,0013979124023695106
                Epoch 367/1000, Loss: 0,0013977477857732823
                Epoch 368/1000, Loss: 0,0013975845745384063
                Epoch 369/1000, Loss: 0,0013974227533745964
                Epoch 370/1000, Loss: 0,0013972623070765706
                Epoch 371/1000, Loss: 0,0013971032205252602
                Epoch 372/1000, Loss: 0,0013969454786889857
                Epoch 373/1000, Loss: 0,0013967890666245805
                Epoch 374/1000, Loss: 0,0013966339694784793
                Epoch 375/1000, Loss: 0,0013964801724877607
                Epoch 376/1000, Loss: 0,001396327660981146
                Epoch 377/1000, Loss: 0,0013961764203799594
                Epoch 378/1000, Loss: 0,0013960264361990399
                Epoch 379/1000, Loss: 0,0013958776940476189
                Epoch 380/1000, Loss: 0,0013957301796301497
                Epoch 381/1000, Loss: 0,0013955838787470956
                Epoch 382/1000, Loss: 0,001395438777295682
                Epoch 383/1000, Loss: 0,0013952948612706048
                Epoch 384/1000, Loss: 0,0013951521167646962
                Epoch 385/1000, Loss: 0,0013950105299695534
                Epoch 386/1000, Loss: 0,0013948700871761303
                Epoch 387/1000, Loss: 0,0013947307747752833
                Epoch 388/1000, Loss: 0,001394592579258285
                Epoch 389/1000, Loss: 0,0013944554872172984
                Epoch 390/1000, Loss: 0,001394319485345813
                Epoch 391/1000, Loss: 0,0013941845604390443
                Epoch 392/1000, Loss: 0,0013940506993942979
                Epoch 393/1000, Loss: 0,0013939178892112987
                Epoch 394/1000, Loss: 0,001393786116992484
                Epoch 395/1000, Loss: 0,0013936553699432628
                Epoch 396/1000, Loss: 0,0013935256353722419
                Epoch 397/1000, Loss: 0,0013933969006914202
                Epoch 398/1000, Loss: 0,0013932691534163477
                Epoch 399/1000, Loss: 0,001393142381166258
                Epoch 400/1000, Loss: 0,0013930165716641621
                Epoch 401/1000, Loss: 0,0013928917127369249
                Epoch 402/1000, Loss: 0,0013927677923152942
                Epoch 403/1000, Loss: 0,0013926447984339205
                Epoch 404/1000, Loss: 0,0013925227192313338
                Epoch 405/1000, Loss: 0,0013924015429498992
                Epoch 406/1000, Loss: 0,0013922812579357487
                Epoch 407/1000, Loss: 0,0013921618526386785
                Epoch 408/1000, Loss: 0,0013920433156120308
                Epoch 409/1000, Loss: 0,0013919256355125415
                Epoch 410/1000, Loss: 0,0013918088011001743
                Epoch 411/1000, Loss: 0,0013916928012379219
                Epoch 412/1000, Loss: 0,001391577624891589
                Epoch 413/1000, Loss: 0,0013914632611295567
                Epoch 414/1000, Loss: 0,0013913496991225177
                Epoch 415/1000, Loss: 0,0013912369281431983
                Epoch 416/1000, Loss: 0,0013911249375660587
                Epoch 417/1000, Loss: 0,0013910137168669688
                Epoch 418/1000, Loss: 0,0013909032556228748
                Epoch 419/1000, Loss: 0,0013907935435114396
                Epoch 420/1000, Loss: 0,0013906845703106684
                Epoch 421/1000, Loss: 0,0013905763258985208
                Epoch 422/1000, Loss: 0,0013904688002525042
                Epoch 423/1000, Loss: 0,001390361983449246
                Epoch 424/1000, Loss: 0,0013902558656640662
                Epoch 425/1000, Loss: 0,0013901504371705172
                Epoch 426/1000, Loss: 0,0013900456883399238
                Epoch 427/1000, Loss: 0,001389941609640903
                Epoch 428/1000, Loss: 0,0013898381916388736
                Epoch 429/1000, Loss: 0,0013897354249955533
                Epoch 430/1000, Loss: 0,0013896333004684439
                Epoch 431/1000, Loss: 0,001389531808910307
                Epoch 432/1000, Loss: 0,0013894309412686248
                Epoch 433/1000, Loss: 0,0013893306885850577
                Epoch 434/1000, Loss: 0,0013892310419948884
                Epoch 435/1000, Loss: 0,0013891319927264536
                Epoch 436/1000, Loss: 0,0013890335321005765
                Epoch 437/1000, Loss: 0,0013889356515299814
                Epoch 438/1000, Loss: 0,0013888383425187066
                Epoch 439/1000, Loss: 0,0013887415966615105
                Epoch 440/1000, Loss: 0,001388645405643265
                Epoch 441/1000, Loss: 0,00138854976123835
                Epoch 442/1000, Loss: 0,0013884546553100375
                Epoch 443/1000, Loss: 0,0013883600798098718
                Epoch 444/1000, Loss: 0,0013882660267770432
                Epoch 445/1000, Loss: 0,0013881724883377602
                Epoch 446/1000, Loss: 0,0013880794567046142
                Epoch 447/1000, Loss: 0,0013879869241759392
                Epoch 448/1000, Loss: 0,0013878948831351756
                Epoch 449/1000, Loss: 0,001387803326050218
                Epoch 450/1000, Loss: 0,0013877122454727746
                Epoch 451/1000, Loss: 0,0013876216340377116
                Epoch 452/1000, Loss: 0,0013875314844624044
                Epoch 453/1000, Loss: 0,0013874417895460803
                Epoch 454/1000, Loss: 0,0013873525421691615
                Epoch 455/1000, Loss: 0,0013872637352926142
                Epoch 456/1000, Loss: 0,0013871753619572812
                Epoch 457/1000, Loss: 0,0013870874152832296
                Epoch 458/1000, Loss: 0,0013869998884690866
                Epoch 459/1000, Loss: 0,001386912774791384
                Epoch 460/1000, Loss: 0,001386826067603893
                Epoch 461/1000, Loss: 0,0013867397603369701
                Epoch 462/1000, Loss: 0,001386653846496893
                Epoch 463/1000, Loss: 0,0013865683196652046
                Epoch 464/1000, Loss: 0,0013864831734980584
                Epoch 465/1000, Loss: 0,0013863984017255546
                Epoch 466/1000, Loss: 0,0013863139981510927
                Epoch 467/1000, Loss: 0,0013862299566507158
                Epoch 468/1000, Loss: 0,0013861462711724564
                Epoch 469/1000, Loss: 0,0013860629357356899
                Epoch 470/1000, Loss: 0,0013859799444304864
                Epoch 471/1000, Loss: 0,001385897291416965
                Epoch 472/1000, Loss: 0,001385814970924651
                Epoch 473/1000, Loss: 0,0013857329772518367
                Epoch 474/1000, Loss: 0,0013856513047649423
                Epoch 475/1000, Loss: 0,0013855699478978836
                Epoch 476/1000, Loss: 0,0013854889011514402
                Epoch 477/1000, Loss: 0,001385408159092623
                Epoch 478/1000, Loss: 0,0013853277163540592
                Epoch 479/1000, Loss: 0,0013852475676333592
                Epoch 480/1000, Loss: 0,0013851677076925065
                Epoch 481/1000, Loss: 0,0013850881313572424
                Epoch 482/1000, Loss: 0,001385008833516452
                Epoch 483/1000, Loss: 0,0013849298091215601
                Epoch 484/1000, Loss: 0,001384851053185932
                Epoch 485/1000, Loss: 0,0013847725607842672
                Epoch 486/1000, Loss: 0,0013846943270520115
                Epoch 487/1000, Loss: 0,001384616347184764
                Epoch 488/1000, Loss: 0,001384538616437692
                Epoch 489/1000, Loss: 0,00138446113012495
                Epoch 490/1000, Loss: 0,001384383883619101
                Epoch 491/1000, Loss: 0,0013843068723505447
                Epoch 492/1000, Loss: 0,0013842300918069515
                Epoch 493/1000, Loss: 0,001384153537532696
                Epoch 494/1000, Loss: 0,0013840772051283007
                Epoch 495/1000, Loss: 0,0013840010902498813
                Epoch 496/1000, Loss: 0,0013839251886085956
                Epoch 497/1000, Loss: 0,0013838494959701026
                Epoch 498/1000, Loss: 0,001383774008154022
                Epoch 499/1000, Loss: 0,0013836987210333973
                Epoch 500/1000, Loss: 0,0013836236305341692
                Epoch 501/1000, Loss: 0,0013835487326346475
                Epoch 502/1000, Loss: 0,001383474023364995
                Epoch 503/1000, Loss: 0,0013833994988067101
                Epoch 504/1000, Loss: 0,0013833251550921175
                Epoch 505/1000, Loss: 0,0013832509884038641
                Epoch 506/1000, Loss: 0,00138317699497442
                Epoch 507/1000, Loss: 0,0013831031710855798
                Epoch 508/1000, Loss: 0,0013830295130679792
                Epoch 509/1000, Loss: 0,0013829560173006035
                Epoch 510/1000, Loss: 0,0013828826802103158
                Epoch 511/1000, Loss: 0,0013828094982713756
                Epoch 512/1000, Loss: 0,0013827364680049727
                Epoch 513/1000, Loss: 0,0013826635859787637
                Epoch 514/1000, Loss: 0,001382590848806411
                Epoch 515/1000, Loss: 0,0013825182531471292
                Epoch 516/1000, Loss: 0,0013824457957052362
                Epoch 517/1000, Loss: 0,0013823734732297088
                Epoch 518/1000, Loss: 0,001382301282513744
                Epoch 519/1000, Loss: 0,0013822292203943243
                Epoch 520/1000, Loss: 0,0013821572837517905
                Epoch 521/1000, Loss: 0,0013820854695094128
                Epoch 522/1000, Loss: 0,0013820137746329785
                Epoch 523/1000, Loss: 0,0013819421961303727
                Epoch 524/1000, Loss: 0,00138187073105117
                Epoch 525/1000, Loss: 0,0013817993764862317
                Epoch 526/1000, Loss: 0,0013817281295673033
                Epoch 527/1000, Loss: 0,0013816569874666243
                Epoch 528/1000, Loss: 0,0013815859473965324
                Epoch 529/1000, Loss: 0,0013815150066090835
                Epoch 530/1000, Loss: 0,001381444162395668
                Epoch 531/1000, Loss: 0,0013813734120866365
                Epoch 532/1000, Loss: 0,0013813027530509294
                Epoch 533/1000, Loss: 0,0013812321826957076
                Epoch 534/1000, Loss: 0,0013811616984659948
                Epoch 535/1000, Loss: 0,0013810912978443164
                Epoch 536/1000, Loss: 0,0013810209783503492
                Epoch 537/1000, Loss: 0,0013809507375405732
                Epoch 538/1000, Loss: 0,0013808805730079264
                Epoch 539/1000, Loss: 0,001380810482381468
                Epoch 540/1000, Loss: 0,001380740463326038
                Epoch 541/1000, Loss: 0,0013806705135419356
                Epoch 542/1000, Loss: 0,001380600630764585
                Epoch 543/1000, Loss: 0,001380530812764216
                Epoch 544/1000, Loss: 0,0013804610573455477
                Epoch 545/1000, Loss: 0,0013803913623474727
                Epoch 546/1000, Loss: 0,0013803217256427498
                Epoch 547/1000, Loss: 0,001380252145137696
                Epoch 548/1000, Loss: 0,0013801826187718887
                Epoch 549/1000, Loss: 0,001380113144517863
                Epoch 550/1000, Loss: 0,001380043720380824
                Epoch 551/1000, Loss: 0,0013799743443983544
                Epoch 552/1000, Loss: 0,0013799050146401295
                Epoch 553/1000, Loss: 0,0013798357292076338
                Epoch 554/1000, Loss: 0,0013797664862338858
                Epoch 555/1000, Loss: 0,0013796972838831625
                Epoch 556/1000, Loss: 0,0013796281203507271
                Epoch 557/1000, Loss: 0,001379558993862563
                Epoch 558/1000, Loss: 0,0013794899026751141
                Epoch 559/1000, Loss: 0,0013794208450750173
                Epoch 560/1000, Loss: 0,0013793518193788521
                Epoch 561/1000, Loss: 0,0013792828239328858
                Epoch 562/1000, Loss: 0,0013792138571128232
                Epoch 563/1000, Loss: 0,0013791449173235612
                Epoch 564/1000, Loss: 0,0013790760029989452
                Epoch 565/1000, Loss: 0,001379007112601529
                Epoch 566/1000, Loss: 0,0013789382446223397
                Epoch 567/1000, Loss: 0,0013788693975806416
                Epoch 568/1000, Loss: 0,0013788005700237085
                Epoch 569/1000, Loss: 0,0013787317605265957
                Epoch 570/1000, Loss: 0,001378662967691914
                Epoch 571/1000, Loss: 0,0013785941901496095
                Epoch 572/1000, Loss: 0,0013785254265567452
                Epoch 573/1000, Loss: 0,0013784566755972853
                Epoch 574/1000, Loss: 0,0013783879359818806
                Epoch 575/1000, Loss: 0,001378319206447661
                Epoch 576/1000, Loss: 0,0013782504857580266
                Epoch 577/1000, Loss: 0,0013781817727024411
                Epoch 578/1000, Loss: 0,0013781130660962331
                Epoch 579/1000, Loss: 0,001378044364780394
                Epoch 580/1000, Loss: 0,0013779756676213805
                Epoch 581/1000, Loss: 0,0013779069735109207
                Epoch 582/1000, Loss: 0,0013778382813658214
                Epoch 583/1000, Loss: 0,0013777695901277785
                Epoch 584/1000, Loss: 0,0013777008987631877
                Epoch 585/1000, Loss: 0,0013776322062629613
                Epoch 586/1000, Loss: 0,0013775635116423414
                Epoch 587/1000, Loss: 0,0013774948139407227
                Epoch 588/1000, Loss: 0,0013774261122214706
                Epoch 589/1000, Loss: 0,0013773574055717444
                Epoch 590/1000, Loss: 0,0013772886931023237
                Epoch 591/1000, Loss: 0,0013772199739474347
                Epoch 592/1000, Loss: 0,0013771512472645765
                Epoch 593/1000, Loss: 0,001377082512234356
                Epoch 594/1000, Loss: 0,0013770137680603177
                Epoch 595/1000, Loss: 0,001376945013968778
                Epoch 596/1000, Loss: 0,0013768762492086632
                Epoch 597/1000, Loss: 0,001376807473051344
                Epoch 598/1000, Loss: 0,001376738684790477
                Epoch 599/1000, Loss: 0,0013766698837418463
                Epoch 600/1000, Loss: 0,0013766010692432026
                Epoch 601/1000, Loss: 0,0013765322406541105
                Epoch 602/1000, Loss: 0,0013764633973557924
                Epoch 603/1000, Loss: 0,0013763945387509754
                Epoch 604/1000, Loss: 0,00137632566426374
                Epoch 605/1000, Loss: 0,0013762567733393676
                Epoch 606/1000, Loss: 0,0013761878654441938
                Epoch 607/1000, Loss: 0,0013761189400654595
                Epoch 608/1000, Loss: 0,0013760499967111629
                Epoch 609/1000, Loss: 0,0013759810349099166
                Epoch 610/1000, Loss: 0,0013759120542107992
                Epoch 611/1000, Loss: 0,001375843054183215
                Epoch 612/1000, Loss: 0,0013757740344167509
                Epoch 613/1000, Loss: 0,0013757049945210336
                Epoch 614/1000, Loss: 0,0013756359341255905
                Epoch 615/1000, Loss: 0,0013755668528797097
                Epoch 616/1000, Loss: 0,001375497750452301
                Epoch 617/1000, Loss: 0,0013754286265317578
                Epoch 618/1000, Loss: 0,0013753594808258221
                Epoch 619/1000, Loss: 0,0013752903130614443
                Epoch 620/1000, Loss: 0,0013752211229846516
                Epoch 621/1000, Loss: 0,0013751519103604101
                Epoch 622/1000, Loss: 0,001375082674972493
                Epoch 623/1000, Loss: 0,0013750134166233435
                Epoch 624/1000, Loss: 0,0013749441351339467
                Epoch 625/1000, Loss: 0,001374874830343692
                Epoch 626/1000, Loss: 0,0013748055021102442
                Epoch 627/1000, Loss: 0,0013747361503094117
                Epoch 628/1000, Loss: 0,0013746667748350126
                Epoch 629/1000, Loss: 0,0013745973755987485
                Epoch 630/1000, Loss: 0,0013745279525300704
                Epoch 631/1000, Loss: 0,0013744585055760514
                Epoch 632/1000, Loss: 0,0013743890347012533
                Epoch 633/1000, Loss: 0,0013743195398876024
                Epoch 634/1000, Loss: 0,0013742500211342558
                Epoch 635/1000, Loss: 0,0013741804784574755
                Epoch 636/1000, Loss: 0,0013741109118904963
                Epoch 637/1000, Loss: 0,0013740413214834014
                Epoch 638/1000, Loss: 0,0013739717073029925
                Epoch 639/1000, Loss: 0,0013739020694326569
                Epoch 640/1000, Loss: 0,0013738324079722486
                Epoch 641/1000, Loss: 0,0013737627230379513
                Epoch 642/1000, Loss: 0,0013736930147621549
                Epoch 643/1000, Loss: 0,001373623283293325
                Epoch 644/1000, Loss: 0,0013735535287958773
                Epoch 645/1000, Loss: 0,001373483751450047
                Epoch 646/1000, Loss: 0,0013734139514517624
                Epoch 647/1000, Loss: 0,0013733441290125139
                Epoch 648/1000, Loss: 0,0013732742843592279
                Epoch 649/1000, Loss: 0,0013732044177341392
                Epoch 650/1000, Loss: 0,0013731345293946573
                Epoch 651/1000, Loss: 0,001373064619613242
                Epoch 652/1000, Loss: 0,0013729946886772738
                Epoch 653/1000, Loss: 0,0013729247368889216
                Epoch 654/1000, Loss: 0,0013728547645650156
                Epoch 655/1000, Loss: 0,0013727847720369172
                Epoch 656/1000, Loss: 0,0013727147596503877
                Epoch 657/1000, Loss: 0,0013726447277654583
                Epoch 658/1000, Loss: 0,0013725746767562995
                Epoch 659/1000, Loss: 0,0013725046070110914
                Epoch 660/1000, Loss: 0,0013724345189318897
                Epoch 661/1000, Loss: 0,0013723644129344946
                Epoch 662/1000, Loss: 0,0013722942894483212
                Epoch 663/1000, Loss: 0,0013722241489162626
                Epoch 664/1000, Loss: 0,00137215399179456
                Epoch 665/1000, Loss: 0,0013720838185526708
                Epoch 666/1000, Loss: 0,0013720136296731299
                Epoch 667/1000, Loss: 0,0013719434256514204
                Epoch 668/1000, Loss: 0,0013718732069958348
                Epoch 669/1000, Loss: 0,0013718029742273454
                Epoch 670/1000, Loss: 0,0013717327278794634
                Epoch 671/1000, Loss: 0,001371662468498104
                Epoch 672/1000, Loss: 0,0013715921966414553
                Epoch 673/1000, Loss: 0,0013715219128798315
                Epoch 674/1000, Loss: 0,0013714516177955462
                Epoch 675/1000, Loss: 0,0013713813119827652
                Epoch 676/1000, Loss: 0,0013713109960473765
                Epoch 677/1000, Loss: 0,001371240670606843
                Epoch 678/1000, Loss: 0,0013711703362900708
                Epoch 679/1000, Loss: 0,0013710999937372637
                Epoch 680/1000, Loss: 0,0013710296435997858
                Epoch 681/1000, Loss: 0,0013709592865400186
                Epoch 682/1000, Loss: 0,0013708889232312228
                Epoch 683/1000, Loss: 0,0013708185543573918
                Epoch 684/1000, Loss: 0,0013707481806131132
                Epoch 685/1000, Loss: 0,0013706778027034222
                Epoch 686/1000, Loss: 0,001370607421343662
                Epoch 687/1000, Loss: 0,0013705370372593365
                Epoch 688/1000, Loss: 0,0013704666511859685
                Epoch 689/1000, Loss: 0,0013703962638689517
                Epoch 690/1000, Loss: 0,001370325876063407
                Epoch 691/1000, Loss: 0,0013702554885340365
                Epoch 692/1000, Loss: 0,0013701851020549761
                Epoch 693/1000, Loss: 0,001370114717409649
                Epoch 694/1000, Loss: 0,001370044335390618
                Epoch 695/1000, Loss: 0,0013699739567994372
                Epoch 696/1000, Loss: 0,001369903582446505
                Epoch 697/1000, Loss: 0,0013698332131509123
                Epoch 698/1000, Loss: 0,001369762849740296
                Epoch 699/1000, Loss: 0,0013696924930506871
                Epoch 700/1000, Loss: 0,0013696221439263617
                Epoch 701/1000, Loss: 0,0013695518032196883
                Epoch 702/1000, Loss: 0,0013694814717909803
                Epoch 703/1000, Loss: 0,001369411150508341
                Epoch 704/1000, Loss: 0,0013693408402475123
                Epoch 705/1000, Loss: 0,001369270541891723
                Epoch 706/1000, Loss: 0,0013692002563315364
                Epoch 707/1000, Loss: 0,0013691299844646953
                Epoch 708/1000, Loss: 0,0013690597271959722
                Epoch 709/1000, Loss: 0,0013689894854370106
                Epoch 710/1000, Loss: 0,0013689192601061744
                Epoch 711/1000, Loss: 0,0013688490521283942
                Epoch 712/1000, Loss: 0,0013687788624350066
                Epoch 713/1000, Loss: 0,0013687086919636075
                Epoch 714/1000, Loss: 0,0013686385416578916
                Epoch 715/1000, Loss: 0,0013685684124674954
                Epoch 716/1000, Loss: 0,0013684983053478482
                Epoch 717/1000, Loss: 0,001368428221260008
                Epoch 718/1000, Loss: 0,0013683581611705115
                Epoch 719/1000, Loss: 0,0013682881260512149
                Epoch 720/1000, Loss: 0,0013682181168791377
                Epoch 721/1000, Loss: 0,001368148134636306
                Epoch 722/1000, Loss: 0,001368078180309596
                Epoch 723/1000, Loss: 0,0013680082548905782
                Epoch 724/1000, Loss: 0,0013679383593753571
                Epoch 725/1000, Loss: 0,0013678684947644169
                Epoch 726/1000, Loss: 0,0013677986620624645
                Epoch 727/1000, Loss: 0,0013677288622782704
                Epoch 728/1000, Loss: 0,0013676590964245142
                Epoch 729/1000, Loss: 0,0013675893655176253
                Epoch 730/1000, Loss: 0,0013675196705776236
                Epoch 731/1000, Loss: 0,0013674500126279715
                Epoch 732/1000, Loss: 0,0013673803926954043
                Epoch 733/1000, Loss: 0,001367310811809784
                Epoch 734/1000, Loss: 0,001367241271003935
                Epoch 735/1000, Loss: 0,0013671717713134948
                Epoch 736/1000, Loss: 0,0013671023137767498
                Epoch 737/1000, Loss: 0,0013670328994344846
                Epoch 738/1000, Loss: 0,0013669635293298247
                Epoch 739/1000, Loss: 0,001366894204508078
                Epoch 740/1000, Loss: 0,0013668249260165841
                Epoch 741/1000, Loss: 0,0013667556949045544
                Epoch 742/1000, Loss: 0,0013666865122229197
                Epoch 743/1000, Loss: 0,0013666173790241756
                Epoch 744/1000, Loss: 0,0013665482963622264
                Epoch 745/1000, Loss: 0,0013664792652922336
                Epoch 746/1000, Loss: 0,0013664102868704614
                Epoch 747/1000, Loss: 0,0013663413621541219
                Epoch 748/1000, Loss: 0,0013662724922012253
                Epoch 749/1000, Loss: 0,0013662036780704265
                Epoch 750/1000, Loss: 0,001366134920820873
                Epoch 751/1000, Loss: 0,0013660662215120556
                Epoch 752/1000, Loss: 0,0013659975812036545
                Epoch 753/1000, Loss: 0,0013659290009553926
                Epoch 754/1000, Loss: 0,0013658604818268844
                Epoch 755/1000, Loss: 0,0013657920248774883
                Epoch 756/1000, Loss: 0,0013657236311661566
                Epoch 757/1000, Loss: 0,0013656553017512899
                Epoch 758/1000, Loss: 0,0013655870376905886
                Epoch 759/1000, Loss: 0,0013655188400409097
                Epoch 760/1000, Loss: 0,0013654507098581183
                Epoch 761/1000, Loss: 0,0013653826481969445
                Epoch 762/1000, Loss: 0,001365314656110839
                Epoch 763/1000, Loss: 0,0013652467346518301
                Epoch 764/1000, Loss: 0,0013651788848703847
                Epoch 765/1000, Loss: 0,001365111107815262
                Epoch 766/1000, Loss: 0,001365043404533375
                Epoch 767/1000, Loss: 0,0013649757760696517
                Epoch 768/1000, Loss: 0,001364908223466899
                Epoch 769/1000, Loss: 0,001364840747765658
                Epoch 770/1000, Loss: 0,0013647733500040744
                Epoch 771/1000, Loss: 0,001364706031217759
                Epoch 772/1000, Loss: 0,0013646387924396547
                Epoch 773/1000, Loss: 0,0013645716346999015
                Epoch 774/1000, Loss: 0,0013645045590257054
                Epoch 775/1000, Loss: 0,0013644375664412056
                Epoch 776/1000, Loss: 0,0013643706579673457
                Epoch 777/1000, Loss: 0,001364303834621742
                Epoch 778/1000, Loss: 0,0013642370974185586
                Epoch 779/1000, Loss: 0,0013641704473683779
                Epoch 780/1000, Loss: 0,0013641038854780752
                Epoch 781/1000, Loss: 0,001364037412750694
                Epoch 782/1000, Loss: 0,0013639710301853254
                Epoch 783/1000, Loss: 0,001363904738776983
                Epoch 784/1000, Loss: 0,0013638385395164806
                Epoch 785/1000, Loss: 0,001363772433390318
                Epoch 786/1000, Loss: 0,0013637064213805575
                Epoch 787/1000, Loss: 0,0013636405044647093
                Epoch 788/1000, Loss: 0,0013635746836156166
                Epoch 789/1000, Loss: 0,0013635089598013394
                Epoch 790/1000, Loss: 0,0013634433339850423
                Epoch 791/1000, Loss: 0,001363377807124883
                Epoch 792/1000, Loss: 0,0013633123801739034
                Epoch 793/1000, Loss: 0,0013632470540799174
                Epoch 794/1000, Loss: 0,001363181829785408
                Epoch 795/1000, Loss: 0,001363116708227418
                Epoch 796/1000, Loss: 0,0013630516903374477
                Epoch 797/1000, Loss: 0,0013629867770413502
                Epoch 798/1000, Loss: 0,0013629219692592308
                Epoch 799/1000, Loss: 0,001362857267905347
                Epoch 800/1000, Loss: 0,0013627926738880114
                Epoch 801/1000, Loss: 0,0013627281881094908
                Epoch 802/1000, Loss: 0,001362663811465916
                Epoch 803/1000, Loss: 0,0013625995448471827
                Epoch 804/1000, Loss: 0,0013625353891368649
                Epoch 805/1000, Loss: 0,0013624713452121168
                Epoch 806/1000, Loss: 0,0013624074139435918
                Epoch 807/1000, Loss: 0,0013623435961953472
                Epoch 808/1000, Loss: 0,0013622798928247658
                Epoch 809/1000, Loss: 0,0013622163046824642
                Epoch 810/1000, Loss: 0,001362152832612214
                Epoch 811/1000, Loss: 0,001362089477450861
                Epoch 812/1000, Loss: 0,0013620262400282446
                Epoch 813/1000, Loss: 0,0013619631211671175
                Epoch 814/1000, Loss: 0,0013619001216830756
                Epoch 815/1000, Loss: 0,001361837242384476
                Epoch 816/1000, Loss: 0,0013617744840723678
                Epoch 817/1000, Loss: 0,001361711847540423
                Epoch 818/1000, Loss: 0,0013616493335748623
                Epoch 819/1000, Loss: 0,0013615869429543885
                Epoch 820/1000, Loss: 0,0013615246764501223
                Epoch 821/1000, Loss: 0,001361462534825534
                Epoch 822/1000, Loss: 0,0013614005188363845
                Epoch 823/1000, Loss: 0,001361338629230661
                Epoch 824/1000, Loss: 0,0013612768667485163
                Epoch 825/1000, Loss: 0,0013612152321222154
                Epoch 826/1000, Loss: 0,0013611537260760766
                Epoch 827/1000, Loss: 0,0013610923493264134
                Epoch 828/1000, Loss: 0,001361031102581488
                Epoch 829/1000, Loss: 0,0013609699865414558
                Epoch 830/1000, Loss: 0,0013609090018983174
                Epoch 831/1000, Loss: 0,0013608481493358685
                Epoch 832/1000, Loss: 0,0013607874295296575
                Epoch 833/1000, Loss: 0,0013607268431469369
                Epoch 834/1000, Loss: 0,0013606663908466224
                Epoch 835/1000, Loss: 0,0013606060732792523
                Epoch 836/1000, Loss: 0,0013605458910869463
                Epoch 837/1000, Loss: 0,0013604858449033676
                Epoch 838/1000, Loss: 0,0013604259353536888
                Epoch 839/1000, Loss: 0,001360366163054554
                Epoch 840/1000, Loss: 0,0013603065286140487
                Epoch 841/1000, Loss: 0,0013602470326316667
                Epoch 842/1000, Loss: 0,0013601876756982813
                Epoch 843/1000, Loss: 0,0013601284583961155
                Epoch 844/1000, Loss: 0,0013600693812987185
                Epoch 845/1000, Loss: 0,001360010444970939
                Epoch 846/1000, Loss: 0,0013599516499689002
                Epoch 847/1000, Loss: 0,001359892996839982
                Epoch 848/1000, Loss: 0,0013598344861227981
                Epoch 849/1000, Loss: 0,001359776118347178
                Epoch 850/1000, Loss: 0,0013597178940341513
                Epoch 851/1000, Loss: 0,0013596598136959297
                Epoch 852/1000, Loss: 0,0013596018778358958
                Epoch 853/1000, Loss: 0,0013595440869485903
                Epoch 854/1000, Loss: 0,0013594864415196997
                Epoch 855/1000, Loss: 0,0013594289420260498
                Epoch 856/1000, Loss: 0,0013593715889355933
                Epoch 857/1000, Loss: 0,0013593143827074095
                Epoch 858/1000, Loss: 0,0013592573237916953
                Epoch 859/1000, Loss: 0,0013592004126297627
                Epoch 860/1000, Loss: 0,0013591436496540386
                Epoch 861/1000, Loss: 0,0013590870352880621
                Epoch 862/1000, Loss: 0,001359030569946488
                Epoch 863/1000, Loss: 0,0013589742540350872
                Epoch 864/1000, Loss: 0,001358918087950751
                Epoch 865/1000, Loss: 0,001358862072081498
                Epoch 866/1000, Loss: 0,0013588062068064792
                Epoch 867/1000, Loss: 0,0013587504924959862
                Epoch 868/1000, Loss: 0,0013586949295114603
                Epoch 869/1000, Loss: 0,0013586395182055054
                Epoch 870/1000, Loss: 0,0013585842589218958
                Epoch 871/1000, Loss: 0,0013585291519955947
                Epoch 872/1000, Loss: 0,0013584741977527661
                Epoch 873/1000, Loss: 0,0013584193965107889
                Epoch 874/1000, Loss: 0,001358364748578277
                Epoch 875/1000, Loss: 0,001358310254255099
                Epoch 876/1000, Loss: 0,0013582559138323926
                Epoch 877/1000, Loss: 0,0013582017275925917
                Epoch 878/1000, Loss: 0,0013581476958094436
                Epoch 879/1000, Loss: 0,0013580938187480337
                Epoch 880/1000, Loss: 0,0013580400966648112
                Epoch 881/1000, Loss: 0,0013579865298076156
                Epoch 882/1000, Loss: 0,0013579331184156976
                Epoch 883/1000, Loss: 0,0013578798627197556
                Epoch 884/1000, Loss: 0,001357826762941957
                Epoch 885/1000, Loss: 0,0013577738192959723
                Epoch 886/1000, Loss: 0,0013577210319870047
                Epoch 887/1000, Loss: 0,001357668401211824
                Epoch 888/1000, Loss: 0,001357615927158796
                Epoch 889/1000, Loss: 0,001357563610007923
                Epoch 890/1000, Loss: 0,0013575114499308713
                Epoch 891/1000, Loss: 0,0013574594470910125
                Epoch 892/1000, Loss: 0,0013574076016434605
                Epoch 893/1000, Loss: 0,001357355913735106
                Epoch 894/1000, Loss: 0,0013573043835046603
                Epoch 895/1000, Loss: 0,0013572530110826896
                Epoch 896/1000, Loss: 0,001357201796591662
                Epoch 897/1000, Loss: 0,0013571507401459835
                Epoch 898/1000, Loss: 0,0013570998418520447
                Epoch 899/1000, Loss: 0,0013570491018082603
                Epoch 900/1000, Loss: 0,0013569985201051168
                Epoch 901/1000, Loss: 0,0013569480968252153
                Epoch 902/1000, Loss: 0,0013568978320433178
                Epoch 903/1000, Loss: 0,0013568477258263946
                Epoch 904/1000, Loss: 0,0013567977782336683
                Epoch 905/1000, Loss: 0,0013567479893166656
                Epoch 906/1000, Loss: 0,0013566983591192646
                Epoch 907/1000, Loss: 0,0013566488876777427
                Epoch 908/1000, Loss: 0,0013565995750208291
                Epoch 909/1000, Loss: 0,001356550421169753
                Epoch 910/1000, Loss: 0,0013565014261382965
                Epoch 911/1000, Loss: 0,0013564525899328462
                Epoch 912/1000, Loss: 0,0013564039125524448
                Epoch 913/1000, Loss: 0,0013563553939888462
                Epoch 914/1000, Loss: 0,0013563070342265668
                Epoch 915/1000, Loss: 0,0013562588332429423
                Epoch 916/1000, Loss: 0,0013562107910081796
                Epoch 917/1000, Loss: 0,0013561629074854136
                Epoch 918/1000, Loss: 0,001356115182630764
                Epoch 919/1000, Loss: 0,0013560676163933898
                Epoch 920/1000, Loss: 0,0013560202087155456
                Epoch 921/1000, Loss: 0,001355972959532641
                Epoch 922/1000, Loss: 0,0013559258687732972
                Epoch 923/1000, Loss: 0,0013558789363594032
                Epoch 924/1000, Loss: 0,001355832162206178
                Epoch 925/1000, Loss: 0,0013557855462222241
                Epoch 926/1000, Loss: 0,0013557390883095922
                Epoch 927/1000, Loss: 0,0013556927883638377
                Epoch 928/1000, Loss: 0,0013556466462740799
                Epoch 929/1000, Loss: 0,0013556006619230639
                Epoch 930/1000, Loss: 0,0013555548351872194
                Epoch 931/1000, Loss: 0,0013555091659367248
                Epoch 932/1000, Loss: 0,0013554636540355634
                Epoch 933/1000, Loss: 0,001355418299341587
                Epoch 934/1000, Loss: 0,001355373101706578
                Epoch 935/1000, Loss: 0,0013553280609763113
                Epoch 936/1000, Loss: 0,0013552831769906134
                Epoch 937/1000, Loss: 0,0013552384495834286
                Epoch 938/1000, Loss: 0,0013551938785828766
                Epoch 939/1000, Loss: 0,0013551494638113202
                Epoch 940/1000, Loss: 0,0013551052050854236
                Epoch 941/1000, Loss: 0,0013550611022162162
                Epoch 942/1000, Loss: 0,001355017155009158
                Epoch 943/1000, Loss: 0,0013549733632641982
                Epoch 944/1000, Loss: 0,0013549297267758426
                Epoch 945/1000, Loss: 0,0013548862453332138
                Epoch 946/1000, Loss: 0,0013548429187201154
                Epoch 947/1000, Loss: 0,0013547997467150946
                Epoch 948/1000, Loss: 0,001354756729091508
                Epoch 949/1000, Loss: 0,0013547138656175804
                Epoch 950/1000, Loss: 0,0013546711560564737
                Epoch 951/1000, Loss: 0,001354628600166346
                Epoch 952/1000, Loss: 0,0013545861977004143
                Epoch 953/1000, Loss: 0,001354543948407025
                Epoch 954/1000, Loss: 0,0013545018520297076
                Epoch 955/1000, Loss: 0,0013544599083072431
                Epoch 956/1000, Loss: 0,0013544181169737291
                Epoch 957/1000, Loss: 0,0013543764777586378
                Epoch 958/1000, Loss: 0,0013543349903868825
                Epoch 959/1000, Loss: 0,0013542936545788795
                Epoch 960/1000, Loss: 0,0013542524700506128
                Epoch 961/1000, Loss: 0,0013542114365136934
                Epoch 962/1000, Loss: 0,001354170553675424
                Epoch 963/1000, Loss: 0,0013541298212388629
                Epoch 964/1000, Loss: 0,0013540892389028824
                Epoch 965/1000, Loss: 0,0013540488063622368
                Epoch 966/1000, Loss: 0,0013540085233076187
                Epoch 967/1000, Loss: 0,001353968389425724
                Epoch 968/1000, Loss: 0,0013539284043993132
                Epoch 969/1000, Loss: 0,001353888567907273
                Epoch 970/1000, Loss: 0,0013538488796246766
                Epoch 971/1000, Loss: 0,0013538093392228472
                Epoch 972/1000, Loss: 0,0013537699463694157
                Epoch 973/1000, Loss: 0,0013537307007283844
                Epoch 974/1000, Loss: 0,0013536916019601854
                Epoch 975/1000, Loss: 0,0013536526497217433
                Epoch 976/1000, Loss: 0,0013536138436665308
                Epoch 977/1000, Loss: 0,0013535751834446344
                Epoch 978/1000, Loss: 0,0013535366687028082
                Epoch 979/1000, Loss: 0,0013534982990845376
                Epoch 980/1000, Loss: 0,0013534600742300944
                Epoch 981/1000, Loss: 0,0013534219937766001
                Epoch 982/1000, Loss: 0,0013533840573580792
                Epoch 983/1000, Loss: 0,0013533462646055212
                Epoch 984/1000, Loss: 0,0013533086151469363
                Epoch 985/1000, Loss: 0,0013532711086074135
                Epoch 986/1000, Loss: 0,0013532337446091785
                Epoch 987/1000, Loss: 0,0013531965227716492
                Epoch 988/1000, Loss: 0,001353159442711493
                Epoch 989/1000, Loss: 0,001353122504042684
                Epoch 990/1000, Loss: 0,0013530857063765555
                Epoch 991/1000, Loss: 0,0013530490493218607
                Epoch 992/1000, Loss: 0,0013530125324848236
                Epoch 993/1000, Loss: 0,0013529761554691956
                Epoch 994/1000, Loss: 0,00135293991787631
                Epoch 995/1000, Loss: 0,0013529038193051345
                Epoch 996/1000, Loss: 0,001352867859352329
                Epoch 997/1000, Loss: 0,0013528320376122941
                Epoch 998/1000, Loss: 0,0013527963536772267
                Epoch 999/1000, Loss: 0,0013527608071371739
                Epoch 1000/1000, Loss: 0,001352725397580082
                Testing trained RNN on physics sentences:
                Input: energy is conserved in system
                Predicted: is conserved in vacuum

                Input: force equals mass times acceleration
                Predicted: equals mass times acceleration

                Input: light travels in vacuum at speed
                Predicted: travels in vacuum at speed

                Input: electron has negative charge
                Predicted: has negative charge

                Input: gravity pulls objects towards earth
                Predicted: pulls objects towards earth
                */

            }
            #endregion 


            Console.ReadLine();


            #region Temperature

            // See ConsoleCodeAnalysis24Oct2024
            var project10 = new Project9Oct2024 { Name = "Temperature" };

            ProjectManager projectManager1 = new ProjectManager();

            Conversation conversation10 = projectManager1.CreateNewConversation(project10);
            
            SourceCodeIteration sourceCodeIteration10 = projectManager1.CreateNewIteration(conversation10);
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

            CodeProcessing codeProcessing = new CodeProcessing();

            foreach (var sourceCode in sourceCodeIteration10.SourceCodes)
            {
                string modifiedCode = codeProcessing.RemoveConsoleReadKey(sourceCode.Code);
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

            projectManager1.Print(project10);

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


            #region Linq
            {
                var project7 = new Project9Oct2024 { Name = "Linq" };
                ProjectManager projectManager = new ProjectManager();

                Conversation conversation1 = projectManager.CreateNewConversation(project7);
                SourceCodeIteration sourceCodeIteration1 = projectManager.CreateNewIteration(conversation1);

                sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"using System;
            using System.Collections.Generic;
            using System.Globalization;
            using System.IO;
            using System.Linq;

            class VectorEmbedding
        {
            private Dictionary<string, float[]> wordVectors;

            public VectorEmbedding()
            {
                wordVectors = new Dictionary<string, float[]>();
            }

            public void LoadEmbedding(string path)
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        string word = values[0];
                        float[] vector = values.Skip(1).Select(t => float.Parse(t, CultureInfo.InvariantCulture)).ToArray();
                        wordVectors[word] = vector;
                    }
                }
            }

            public float[] GetVector(string word)
            {
                if (wordVectors.TryGetValue(word, out float[] vector))
                {
                    return vector;
                }
                else
                {
                    throw new ArgumentException($""Word '{word}' not found in embeddings."");
                }
            }
        }

        class Program
        {
            static void Main()
            {
                string gloveFilePath = ""../../../../../../../GloVe/glove.6B.300d.txt"";
                VectorEmbedding embedding = new VectorEmbedding();
                embedding.LoadEmbedding(gloveFilePath);

                try
                {
                    float[] vector = embedding.GetVector(""computer"");
                    Console.WriteLine($""Vector for 'computer': [{string.Join("", "", vector)}]"");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }"));

                CodeProcessing codeProcessing1 = new CodeProcessing();

                foreach (var sourceCode in sourceCodeIteration1.SourceCodes)
                {
                    string modifiedCode = codeProcessing1.RemoveConsoleReadKey(sourceCode.Code);
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


                projectManager1.Print(project7);

                /*
                Project: Linq
                conversation #1
                iteration #1
                Code: using System;
                            using System.Collections.Generic;
                            using System.Globalization;
                            using System.IO;
                            using System.Linq;

                            class VectorEmbedding
                        {
                            private Dictionary<string, float[]> wordVectors;

                            public VectorEmbedding()
                            {
                                wordVectors = new Dictionary<string, float[]>();
                            }

                            public void LoadEmbedding(string path)
                            {
                                using (StreamReader sr = File.OpenText(path))
                                {
                                    string line;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                        string word = values[0];
                                        float[] vector = values.Skip(1).Select(t => float.Parse(t, CultureInfo.InvariantCulture)).ToArray();
                                        wordVectors[word] = vector;
                                    }
                                }
                            }

                            public float[] GetVector(string word)
                            {
                                if (wordVectors.TryGetValue(word, out float[] vector))
                                {
                                    return vector;
                                }
                                else
                                {
                                    throw new ArgumentException($"Word '{word}' not found in embeddings.");
                                }
                            }
                        }

                        class Program
                        {
                            static void Main()
                            {
                                string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
                                VectorEmbedding embedding = new VectorEmbedding();
                                embedding.LoadEmbedding(gloveFilePath);

                                try
                                {
                                    float[] vector = embedding.GetVector("computer");
                                    Console.WriteLine($"Vector for 'computer': [{string.Join(", ", vector)}]");
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                Execution Output: Vector for 'computer': [-0,27628, 0,13999, 0,098519, -0,64019, 0,031988, 0,10066, -0,18673, -0,37129, 0,5974, -2,0405, 0,22368, -0,026314, 0,72408, -0,43829, 0,48886, -0,0035486, -0,10006, -0,30587, -0,15621, -0,068136, 0,21104, 0,29287, -0,088861, -0,20462, -0,57602, 0,34526, 0,4139, 0,17917, 0,25143, -0,22678, -0,10103, 0,14576, 0,20127, 0,3181, -0,78907, -0,22194, -0,24833, -0,015103, -0,2005, -0,026441, 0,18551, 0,33782, -0,33543, 0,86117, -0,047083, -0,17009, 0,30438, 0,094119, 0,32435, -0,81171, 0,88966, -0,39149, 0,16828, 0,14316, 0,0036339, -0,064557, 0,045777, -0,32248, 0,048943, 0,16817, 0,068344, 0,54227, 0,12493, 0,69742, -0,037194, 0,3308, -0,42194, 0,3397, 0,27646, -0,016003, -0,21827, 0,44535, 0,35379, -0,022089, 0,21375, 0,43267, -0,32897, 0,096165, 0,31265, -0,30528, 0,26126, -0,65364, -0,78014, -0,23154, 0,12113, 0,34896, -0,55444, 0,46619, -0,1652, 0,11611, -0,76676, 0,69502, -0,15698, -0,1249, 0,56505, 0,64499, -0,57403, -0,033549, 0,32898, -1,4025, -0,31143, 0,64549, -0,061534, -0,69295, 0,00060894, -0,56544, 0,19181, -0,19208, -0,62673, -0,0097473, -0,5504, -0,56128, -0,19603, 0,29254, 0,098576, -0,059395, 0,0033616, 0,19515, -0,60703, 0,34262, 0,095211, -0,079411, 0,14305, -0,56569, -0,065887, 0,15167, -0,13505, 0,19571, 0,22812, 0,035346, -0,22509, 0,1891, -0,37348, 0,12505, 0,46249, -0,32219, 0,90643, 0,11595, 0,11628, 0,22961, 0,2401, -0,061609, 0,39325, -0,065066, 0,42257, 0,5688, 0,49804, -0,61308, 0,41468, -0,13448, 0,6043, -0,065462, -0,085376, 0,19115, 0,39925, 0,37495, -0,18492, 0,061751, -0,38747, -0,30335, -0,38211, 0,28221, -0,10286, -0,5866, 0,82922, 0,25131, 0,24772, 0,87482, -0,31359, 0,81621, -0,90081, -0,77933, -1,009, 0,36472, -0,11562, -0,24841, 0,094527, -0,42266, 0,060392, -0,15365, -0,069604, 0,0051292, 0,39572, -0,15692, 0,35708, -0,35165, 0,35296, -0,5222, 0,514, -0,17764, -0,10272, -0,3964, 0,30418, 0,073659, -0,11685, 0,14299, -0,3681, 0,27642, -0,46683, -0,32633, 0,51107, 0,023945, 0,11723, 0,21761, -0,17389, -0,61193, -0,59449, 0,47749, -0,59008, -0,36092, -0,099574, -0,043098, -0,15106, -0,14336, -0,031135, 0,17887, -0,64221, 0,17242, 0,33916, 0,87181, -0,7723, 0,53195, -0,52763, 0,1751, 0,31043, -0,15177, -0,22706, 0,10803, 0,44919, 0,070016, 0,20851, 0,21517, -0,61712, -0,09997, 0,005502, 0,076786, 0,28046, 0,42331, -0,58925, 0,070554, 0,39923, 0,090201, 0,17139, -0,17282, -0,53675, -0,46439, -0,5785, -0,68311, 0,059383, 0,12427, -0,14558, 0,57687, -0,57499, -0,051645, 0,3841, 0,13047, 0,33786, 0,33204, 0,40119, 0,26389, -0,36953, -0,29797, -0,66816, -0,11883, 0,50133, 0,20603, -0,32558, -0,12242, 0,50666, 0,16353, -0,10672, 0,22364, 0,23915, -0,55509, -0,48432, -0,012165, -1,7992, 0,3231, -0,26309, -0,32538, -0,5827, 0,15099, 0,33838, 0,12007, 0,41395, -0,15553, -0,19301, 0,05886, -0,5242, -0,3717, 0,56205, -0,65801, -0,49796, 0,24347, 0,12873, 0,33665, -0,072609, -0,15686, -0,14187, -0,26488]
                */
            }
            #endregion



            Console.ReadLine();
        }

    }
}