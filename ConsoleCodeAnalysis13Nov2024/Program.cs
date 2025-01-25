using LibraryCodeAnalysis12Nov2024;

namespace ConsoleCodeAnalysis13Nov2024
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CodeProcessing codeProcessing = new CodeProcessing();

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

                codeProcessing.ProcessCodeBlock(sourceCodeIteration5, sourceCodeIteration5.SourceCodes[0].Code, sourceCodeIteration5.SourceCodes[1].Code, sourceCodeIteration5.SourceCodes[2].Code);

                projectManager.Print(project9);

                /*
                Project: Recurrent Neural Networks
                conversation #1
                iteration #1
                Epoch 1/1000, Loss: 0,005197569978298553
                Epoch 2/1000, Loss: 0,005193104543260862
                Epoch 3/1000, Loss: 0,0051555430021315825
                Epoch 4/1000, Loss: 0,0048980789977872575
                Epoch 5/1000, Loss: 0,004358487342907679
                Epoch 6/1000, Loss: 0,004206463016929083
                Epoch 7/1000, Loss: 0,004187331607532164
                Epoch 8/1000, Loss: 0,004168826489595127
                Epoch 9/1000, Loss: 0,004152023229683165
                Epoch 10/1000, Loss: 0,004136936234837061
                Epoch 11/1000, Loss: 0,0041231925129620265
                Epoch 12/1000, Loss: 0,004110505709380212
                Epoch 13/1000, Loss: 0,004098673470830031
                Epoch 14/1000, Loss: 0,004087550340198173
                Epoch 15/1000, Loss: 0,0040770282572841375
                Epoch 16/1000, Loss: 0,0040670231606311485
                Epoch 17/1000, Loss: 0,004057465503286997
                Epoch 18/1000, Loss: 0,004048293294537485
                Epoch 19/1000, Loss: 0,004039446789382708
                Epoch 20/1000, Loss: 0,004030864248964398
                Epoch 21/1000, Loss: 0,004022478379018368
                Epoch 22/1000, Loss: 0,004014213170271773
                Epoch 23/1000, Loss: 0,004005980945224871
                Epoch 24/1000, Loss: 0,003997679481314511
                Epoch 25/1000, Loss: 0,003989189148376416
                Epoch 26/1000, Loss: 0,003980370084784251
                Epoch 27/1000, Loss: 0,003971059557844641
                Epoch 28/1000, Loss: 0,003961069825154141
                Epoch 29/1000, Loss: 0,003950187044292388
                Epoch 30/1000, Loss: 0,003938172062673774
                Epoch 31/1000, Loss: 0,0039247642205930515
                Epoch 32/1000, Loss: 0,003909689530960752
                Epoch 33/1000, Loss: 0,0038926746074616943
                Epoch 34/1000, Loss: 0,0038734672913143574
                Epoch 35/1000, Loss: 0,003851863856028353
                Epoch 36/1000, Loss: 0,0038277408054350017
                Epoch 37/1000, Loss: 0,0038010866889919815
                Epoch 38/1000, Loss: 0,003772026515488975
                Epoch 39/1000, Loss: 0,0037408293682409943
                Epoch 40/1000, Loss: 0,0037078904806525552
                Epoch 41/1000, Loss: 0,003673684035574082
                Epoch 42/1000, Loss: 0,003638692350076373
                Epoch 43/1000, Loss: 0,003603327740603826
                Epoch 44/1000, Loss: 0,003567869433035861
                Epoch 45/1000, Loss: 0,0035324345428147377
                Epoch 46/1000, Loss: 0,0034969897959786617
                Epoch 47/1000, Loss: 0,003461395619813416
                Epoch 48/1000, Loss: 0,0034254644227570893
                Epoch 49/1000, Loss: 0,003389014224294174
                Epoch 50/1000, Loss: 0,003351905369325936
                Epoch 51/1000, Loss: 0,003314056858302692
                Epoch 52/1000, Loss: 0,003275445634345341
                Epoch 53/1000, Loss: 0,003236095404334307
                Epoch 54/1000, Loss: 0,003196061740097807
                Epoch 55/1000, Loss: 0,0031554185773167687
                Epoch 56/1000, Loss: 0,0031142489360024206
                Epoch 57/1000, Loss: 0,0030726404769809345
                Epoch 58/1000, Loss: 0,003030684826156584
                Epoch 59/1000, Loss: 0,002988478674595136
                Epoch 60/1000, Loss: 0,0029461245488932508
                Epoch 61/1000, Loss: 0,002903729715743268
                Epoch 62/1000, Loss: 0,0028614026615275726
                Epoch 63/1000, Loss: 0,0028192476215825077
                Epoch 64/1000, Loss: 0,0027773583979453964
                Epoch 65/1000, Loss: 0,0027358129900409414
                Epoch 66/1000, Loss: 0,0026946703349038166
                Epoch 67/1000, Loss: 0,0026539698480699415
                Epoch 68/1000, Loss: 0,0026137337158791954
                Epoch 69/1000, Loss: 0,0025739712645916394
                Epoch 70/1000, Loss: 0,0025346843879059154
                Epoch 71/1000, Loss: 0,002495872991818314
                Epoch 72/1000, Loss: 0,002457539642413882
                Epoch 73/1000, Loss: 0,0024196929494174507
                Epoch 74/1000, Loss: 0,0023823495610241544
                Epoch 75/1000, Loss: 0,0023455349019754746
                Epoch 76/1000, Loss: 0,0023092829293241313
                Epoch 77/1000, Loss: 0,0022736352228216556
                Epoch 78/1000, Loss: 0,002238639702817385
                Epoch 79/1000, Loss: 0,0022043492105392115
                Epoch 80/1000, Loss: 0,0021708201141102394
                Epoch 81/1000, Loss: 0,002138111026764484
                Epoch 82/1000, Loss: 0,0021062816446579336
                Epoch 83/1000, Loss: 0,0020753916374585644
                Epoch 84/1000, Loss: 0,0020454994702488994
                Epoch 85/1000, Loss: 0,0020166610187459257
                Epoch 86/1000, Loss: 0,0019889278745217837
                Epoch 87/1000, Loss: 0,001962345321274116
                Epoch 88/1000, Loss: 0,001936950077867888
                Epoch 89/1000, Loss: 0,0019127680165960247
                Epoch 90/1000, Loss: 0,0018898121418406185
                Epoch 91/1000, Loss: 0,0018680811312402276
                Epoch 92/1000, Loss: 0,001847558692665245
                Epoch 93/1000, Loss: 0,0018282138883341657
                Epoch 94/1000, Loss: 0,0018100024474630625
                Epoch 95/1000, Loss: 0,0017928689597290957
                Epoch 96/1000, Loss: 0,001776749737613751
                Epoch 97/1000, Loss: 0,0017615760707441652
                Epoch 98/1000, Loss: 0,00174727757344378
                Epoch 99/1000, Loss: 0,0017337853431726009
                Epoch 100/1000, Loss: 0,0017210346928016999
                Epoch 101/1000, Loss: 0,0017089672826103648
                Epoch 102/1000, Loss: 0,0016975325482781914
                Epoch 103/1000, Loss: 0,001686688390851641
                Epoch 104/1000, Loss: 0,0016764011579800588
                Epoch 105/1000, Loss: 0,0016666449989858518
                Epoch 106/1000, Loss: 0,0016574007173640044
                Epoch 107/1000, Loss: 0,001648654271799958
                Epoch 108/1000, Loss: 0,0016403950900268694
                Epoch 109/1000, Loss: 0,0016326143586465793
                Epoch 110/1000, Loss: 0,001625303437040569
                Epoch 111/1000, Loss: 0,0016184525164308805
                Epoch 112/1000, Loss: 0,001612049609030659
                Epoch 113/1000, Loss: 0,0016060799112160768
                Epoch 114/1000, Loss: 0,0016005255435892895
                Epoch 115/1000, Loss: 0,0015953656344360255
                Epoch 116/1000, Loss: 0,0015905766852655425
                Epoch 117/1000, Loss: 0,001586133140170602
                Epoch 118/1000, Loss: 0,0015820080751449586
                Epoch 119/1000, Loss: 0,001578173928015019
                Epoch 120/1000, Loss: 0,001574603201793925
                Epoch 121/1000, Loss: 0,0015712690909501878
                Epoch 122/1000, Loss: 0,0015681459982293793
                Epoch 123/1000, Loss: 0,0015652099267353195
                Epoch 124/1000, Loss: 0,0015624387462305216
                Epoch 125/1000, Loss: 0,0015598123431774897
                Epoch 126/1000, Loss: 0,0015573126707788547
                Epoch 127/1000, Loss: 0,0015549237185852507
                Epoch 128/1000, Loss: 0,0015526314218275903
                Epoch 129/1000, Loss: 0,001550423529297946
                Epoch 130/1000, Loss: 0,0015482894461086353
                Epoch 131/1000, Loss: 0,0015462200646299638
                Epoch 132/1000, Loss: 0,0015442075938078836
                Epoch 133/1000, Loss: 0,0015422453941976058
                Epoch 134/1000, Loss: 0,0015403278235866544
                Epoch 135/1000, Loss: 0,0015384500960864565
                Epoch 136/1000, Loss: 0,0015366081560415153
                Epoch 137/1000, Loss: 0,0015347985669935096
                Epoch 138/1000, Loss: 0,0015330184151785289
                Epoch 139/1000, Loss: 0,0015312652265575626
                Epoch 140/1000, Loss: 0,0015295368961159412
                Epoch 141/1000, Loss: 0,0015278316280576598
                Epoch 142/1000, Loss: 0,001526147885517129
                Epoch 143/1000, Loss: 0,0015244843484761143
                Epoch 144/1000, Loss: 0,001522839878679031
                Epoch 145/1000, Loss: 0,0015212134904651555
                Epoch 146/1000, Loss: 0,0015196043265676357
                Epoch 147/1000, Loss: 0,001518011638057472
                Epoch 148/1000, Loss: 0,001516434767730307
                Epoch 149/1000, Loss: 0,0015148731363421463
                Epoch 150/1000, Loss: 0,0015133262311957938
                Epoch 151/1000, Loss: 0,0015117935966627485
                Epoch 152/1000, Loss: 0,0015102748262964174
                Epoch 153/1000, Loss: 0,0015087695562525795
                Epoch 154/1000, Loss: 0,0015072774597835073
                Epoch 155/1000, Loss: 0,001505798242614128
                Epoch 156/1000, Loss: 0,0015043316390433567
                Epoch 157/1000, Loss: 0,0015028774086423796
                Epoch 158/1000, Loss: 0,0015014353334451293
                Epoch 159/1000, Loss: 0,0015000052155453999
                Epoch 160/1000, Loss: 0,0014985868750307413
                Epoch 161/1000, Loss: 0,001497180148196001
                Epoch 162/1000, Loss: 0,0014957848859897837
                Epoch 163/1000, Loss: 0,0014944009526555234
                Epoch 164/1000, Loss: 0,0014930282245357311
                Epoch 165/1000, Loss: 0,00149166658901354
                Epoch 166/1000, Loss: 0,0014903159435702238
                Epoch 167/1000, Loss: 0,0014889761949410392
                Epoch 168/1000, Loss: 0,0014876472583547754
                Epoch 169/1000, Loss: 0,0014863290568448424
                Epoch 170/1000, Loss: 0,0014850215206217662
                Epoch 171/1000, Loss: 0,0014837245864986004
                Epoch 172/1000, Loss: 0,0014824381973621535
                Epoch 173/1000, Loss: 0,0014811623016840478
                Epoch 174/1000, Loss: 0,001479896853066575
                Epoch 175/1000, Loss: 0,0014786418098190917
                Epoch 176/1000, Loss: 0,0014773971345613634
                Epoch 177/1000, Loss: 0,0014761627938508126
                Epoch 178/1000, Loss: 0,0014749387578310786
                Epoch 179/1000, Loss: 0,0014737249998997162
                Epoch 180/1000, Loss: 0,00147252149639317
                Epoch 181/1000, Loss: 0,0014713282262874576
                Epoch 182/1000, Loss: 0,001470145170913238
                Epoch 183/1000, Loss: 0,0014689723136841327
                Epoch 184/1000, Loss: 0,0014678096398373715
                Epoch 185/1000, Loss: 0,0014666571361859643
                Epoch 186/1000, Loss: 0,0014655147908817277
                Epoch 187/1000, Loss: 0,0014643825931886325
                Epoch 188/1000, Loss: 0,001463260533265992
                Epoch 189/1000, Loss: 0,0014621486019611373
                Epoch 190/1000, Loss: 0,0014610467906112376
                Epoch 191/1000, Loss: 0,0014599550908540398
                Epoch 192/1000, Loss: 0,0014588734944472903
                Epoch 193/1000, Loss: 0,0014578019930966914
                Epoch 194/1000, Loss: 0,0014567405782922311
                Epoch 195/1000, Loss: 0,0014556892411527785
                Epoch 196/1000, Loss: 0,0014546479722788448
                Epoch 197/1000, Loss: 0,0014536167616134182
                Epoch 198/1000, Loss: 0,0014525955983107945
                Epoch 199/1000, Loss: 0,0014515844706133414
                Epoch 200/1000, Loss: 0,0014505833657361064
                Epoch 201/1000, Loss: 0,0014495922697592058
                Epoch 202/1000, Loss: 0,0014486111675279226
                Epoch 203/1000, Loss: 0,001447640042560414
                Epoch 204/1000, Loss: 0,0014466788769629554
                Epoch 205/1000, Loss: 0,001445727651352615
                Epoch 206/1000, Loss: 0,001444786344787249
                Epoch 207/1000, Loss: 0,0014438549347027068
                Epoch 208/1000, Loss: 0,001442933396857106
                Epoch 209/1000, Loss: 0,001442021705282046
                Epoch 210/1000, Loss: 0,00144111983224061
                Epoch 211/1000, Loss: 0,0014402277481919846
                Epoch 212/1000, Loss: 0,0014393454217625317
                Epoch 213/1000, Loss: 0,0014384728197231254
                Epoch 214/1000, Loss: 0,0014376099069725663
                Epoch 215/1000, Loss: 0,0014367566465268598
                Epoch 216/1000, Loss: 0,001435912999514162
                Epoch 217/1000, Loss: 0,0014350789251751635
                Epoch 218/1000, Loss: 0,0014342543808686901
                Epoch 219/1000, Loss: 0,0014334393220822872
                Epoch 220/1000, Loss: 0,0014326337024475524
                Epoch 221/1000, Loss: 0,0014318374737599744
                Epoch 222/1000, Loss: 0,0014310505860030329
                Epoch 223/1000, Loss: 0,0014302729873763155
                Epoch 224/1000, Loss: 0,0014295046243273982
                Epoch 225/1000, Loss: 0,001428745441587245
                Epoch 226/1000, Loss: 0,0014279953822088774
                Epoch 227/1000, Loss: 0,0014272543876090622
                Epoch 228/1000, Loss: 0,0014265223976127786
                Epoch 229/1000, Loss: 0,0014257993505002143
                Epoch 230/1000, Loss: 0,0014250851830560603
                Epoch 231/1000, Loss: 0,001424379830620863
                Epoch 232/1000, Loss: 0,0014236832271442002
                Epoch 233/1000, Loss: 0,0014229953052394696
                Epoch 234/1000, Loss: 0,0014223159962400607
                Epoch 235/1000, Loss: 0,0014216452302566972
                Epoch 236/1000, Loss: 0,0014209829362357535
                Epoch 237/1000, Loss: 0,0014203290420183408
                Epoch 238/1000, Loss: 0,0014196834743999789
                Epoch 239/1000, Loss: 0,0014190461591906635
                Epoch 240/1000, Loss: 0,0014184170212751645
                Epoch 241/1000, Loss: 0,0014177959846733898
                Epoch 242/1000, Loss: 0,001417182972600651
                Epoch 243/1000, Loss: 0,0014165779075276928
                Epoch 244/1000, Loss: 0,0014159807112403386
                Epoch 245/1000, Loss: 0,0014153913048986322
                Epoch 246/1000, Loss: 0,0014148096090953475
                Epoch 247/1000, Loss: 0,0014142355439137556
                Epoch 248/1000, Loss: 0,0014136690289845472
                Epoch 249/1000, Loss: 0,0014131099835418112
                Epoch 250/1000, Loss: 0,0014125583264779874
                Epoch 251/1000, Loss: 0,0014120139763977144
                Epoch 252/1000, Loss: 0,0014114768516704916
                Epoch 253/1000, Loss: 0,0014109468704821071
                Epoch 254/1000, Loss: 0,0014104239508847675
                Epoch 255/1000, Loss: 0,0014099080108458746
                Epoch 256/1000, Loss: 0,0014093989682954244
                Epoch 257/1000, Loss: 0,001408896741171971
                Epoch 258/1000, Loss: 0,0014084012474671445
                Epoch 259/1000, Loss: 0,0014079124052686957
                Epoch 260/1000, Loss: 0,0014074301328020347
                Epoch 261/1000, Loss: 0,0014069543484702765
                Epoch 262/1000, Loss: 0,0014064849708927646
                Epoch 263/1000, Loss: 0,0014060219189420803
                Epoch 264/1000, Loss: 0,0014055651117795395
                Epoch 265/1000, Loss: 0,0014051144688891778
                Epoch 266/1000, Loss: 0,0014046699101102358
                Epoch 267/1000, Loss: 0,0014042313556681593
                Epoch 268/1000, Loss: 0,0014037987262041317
                Epoch 269/1000, Loss: 0,0014033719428031468
                Epoch 270/1000, Loss: 0,0014029509270206586
                Epoch 271/1000, Loss: 0,001402535600907825
                Epoch 272/1000, Loss: 0,001402125887035363
                Epoch 273/1000, Loss: 0,0014017217085160656
                Epoch 274/1000, Loss: 0,0014013229890259828
                Epoch 275/1000, Loss: 0,0014009296528243198
                Epoch 276/1000, Loss: 0,0014005416247720671
                Epoch 277/1000, Loss: 0,001400158830349416
                Epoch 278/1000, Loss: 0,001399781195671969
                Epoch 279/1000, Loss: 0,0013994086475058
                Epoch 280/1000, Loss: 0,0013990411132813903
                Epoch 281/1000, Loss: 0,001398678521106474
                Epoch 282/1000, Loss: 0,0013983207997778375
                Epoch 283/1000, Loss: 0,0013979678787920974
                Epoch 284/1000, Loss: 0,0013976196883555008
                Epoch 285/1000, Loss: 0,0013972761593927813
                Epoch 286/1000, Loss: 0,0013969372235551054
                Epoch 287/1000, Loss: 0,0013966028132271434
                Epoch 288/1000, Loss: 0,0013962728615333011
                Epoch 289/1000, Loss: 0,0013959473023431469
                Epoch 290/1000, Loss: 0,001395626070276065
                Epoch 291/1000, Loss: 0,001395309100705173
                Epoch 292/1000, Loss: 0,0013949963297605252
                Epoch 293/1000, Loss: 0,0013946876943316505
                Epoch 294/1000, Loss: 0,0013943831320694393
                Epoch 295/1000, Loss: 0,0013940825813874172
                Epoch 296/1000, Loss: 0,001393785981462434
                Epoch 297/1000, Loss: 0,0013934932722348004
                Epoch 298/1000, Loss: 0,0013932043944078888
                Epoch 299/1000, Loss: 0,0013929192894472397
                Epoch 300/1000, Loss: 0,0013926378995791858
                Epoch 301/1000, Loss: 0,0013923601677890327
                Epoch 302/1000, Loss: 0,0013920860378188074
                Epoch 303/1000, Loss: 0,0013918154541646096
                Epoch 304/1000, Loss: 0,001391548362073578
                Epoch 305/1000, Loss: 0,0013912847075405047
                Epoch 306/1000, Loss: 0,0013910244373041092
                Epoch 307/1000, Loss: 0,0013907674988429953
                Epoch 308/1000, Loss: 0,0013905138403713178
                Epoch 309/1000, Loss: 0,0013902634108341615
                Epoch 310/1000, Loss: 0,0013900161599026692
                Epoch 311/1000, Loss: 0,0013897720379689217
                Epoch 312/1000, Loss: 0,0013895309961405923
                Epoch 313/1000, Loss: 0,0013892929862353964
                Epoch 314/1000, Loss: 0,0013890579607753358
                Epoch 315/1000, Loss: 0,001388825872980776
                Epoch 316/1000, Loss: 0,0013885966767643438
                Epoch 317/1000, Loss: 0,0013883703267246808
                Epoch 318/1000, Loss: 0,0013881467781400511
                Epoch 319/1000, Loss: 0,0013879259869618269
                Epoch 320/1000, Loss: 0,0013877079098078473
                Epoch 321/1000, Loss: 0,0013874925039556812
                Epoch 322/1000, Loss: 0,001387279727335789
                Epoch 323/1000, Loss: 0,0013870695385246007
                Epoch 324/1000, Loss: 0,0013868618967375185
                Epoch 325/1000, Loss: 0,0013866567618218517
                Epoch 326/1000, Loss: 0,0013864540942496906
                Epoch 327/1000, Loss: 0,0013862538551107399
                Epoch 328/1000, Loss: 0,0013860560061050974
                Epoch 329/1000, Loss: 0,0013858605095360076
                Epoch 330/1000, Loss: 0,0013856673283025801
                Epoch 331/1000, Loss: 0,0013854764258924917
                Epoch 332/1000, Loss: 0,0013852877663746753
                Epoch 333/1000, Loss: 0,0013851013143919874
                Epoch 334/1000, Loss: 0,0013849170351538916
                Epoch 335/1000, Loss: 0,0013847348944291253
                Epoch 336/1000, Loss: 0,0013845548585383856
                Epoch 337/1000, Loss: 0,0013843768943470228
                Epoch 338/1000, Loss: 0,0013842009692577467
                Epoch 339/1000, Loss: 0,0013840270512033611
                Epoch 340/1000, Loss: 0,0013838551086395156
                Epoch 341/1000, Loss: 0,001383685110537488
                Epoch 342/1000, Loss: 0,0013835170263770008
                Epoch 343/1000, Loss: 0,0013833508261390682
                Epoch 344/1000, Loss: 0,0013831864802988849
                Epoch 345/1000, Loss: 0,0013830239598187563
                Epoch 346/1000, Loss: 0,001382863236141066
                Epoch 347/1000, Loss: 0,0013827042811812996
                Epoch 348/1000, Loss: 0,0013825470673211114
                Epoch 349/1000, Loss: 0,0013823915674014411
                Epoch 350/1000, Loss: 0,001382237754715685
                Epoch 351/1000, Loss: 0,0013820856030029261
                Epoch 352/1000, Loss: 0,0013819350864412146
                Epoch 353/1000, Loss: 0,0013817861796409099
                Epoch 354/1000, Loss: 0,0013816388576380857
                Epoch 355/1000, Loss: 0,0013814930958879874
                Epoch 356/1000, Loss: 0,0013813488702585643
                Epoch 357/1000, Loss: 0,0013812061570240542
                Epoch 358/1000, Loss: 0,001381064932858641
                Epoch 359/1000, Loss: 0,0013809251748301745
                Epoch 360/1000, Loss: 0,0013807868603939572
                Epoch 361/1000, Loss: 0,0013806499673865995
                Epoch 362/1000, Loss: 0,0013805144740199422
                Epoch 363/1000, Loss: 0,0013803803588750476
                Epoch 364/1000, Loss: 0,001380247600896262
                Epoch 365/1000, Loss: 0,0013801161793853462
                Epoch 366/1000, Loss: 0,001379986073995674
                Epoch 367/1000, Loss: 0,0013798572647265091
                Epoch 368/1000, Loss: 0,0013797297319173393
                Epoch 369/1000, Loss: 0,0013796034562422973
                Epoch 370/1000, Loss: 0,0013794784187046397
                Epoch 371/1000, Loss: 0,0013793546006313044
                Epoch 372/1000, Loss: 0,0013792319836675349
                Epoch 373/1000, Loss: 0,0013791105497715785
                Epoch 374/1000, Loss: 0,0013789902812094558
                Epoch 375/1000, Loss: 0,0013788711605497955
                Epoch 376/1000, Loss: 0,0013787531706587492
                Epoch 377/1000, Loss: 0,0013786362946949678
                Epoch 378/1000, Loss: 0,0013785205161046513
                Epoch 379/1000, Loss: 0,0013784058186166726
                Epoch 380/1000, Loss: 0,0013782921862377615
                Epoch 381/1000, Loss: 0,0013781796032477697
                Epoch 382/1000, Loss: 0,001378068054194994
                Epoch 383/1000, Loss: 0,0013779575238915716
                Epoch 384/1000, Loss: 0,0013778479974089499
                Epoch 385/1000, Loss: 0,0013777394600734093
                Epoch 386/1000, Loss: 0,0013776318974616716
                Epoch 387/1000, Loss: 0,0013775252953965554
                Epoch 388/1000, Loss: 0,0013774196399427131
                Epoch 389/1000, Loss: 0,0013773149174024259
                Epoch 390/1000, Loss: 0,001377211114311463
                Epoch 391/1000, Loss: 0,0013771082174350098
                Epoch 392/1000, Loss: 0,001377006213763655
                Epoch 393/1000, Loss: 0,0013769050905094438
                Epoch 394/1000, Loss: 0,0013768048351019907
                Epoch 395/1000, Loss: 0,0013767054351846591
                Epoch 396/1000, Loss: 0,0013766068786107955
                Epoch 397/1000, Loss: 0,00137650915344003
                Epoch 398/1000, Loss: 0,0013764122479346312
                Epoch 399/1000, Loss: 0,0013763161505559282
                Epoch 400/1000, Loss: 0,0013762208499607793
                Epoch 401/1000, Loss: 0,0013761263349981114
                Epoch 402/1000, Loss: 0,0013760325947055068
                Epoch 403/1000, Loss: 0,001375939618305852
                Epoch 404/1000, Loss: 0,0013758473952040393
                Epoch 405/1000, Loss: 0,001375755914983724
                Epoch 406/1000, Loss: 0,0013756651674041386
                Epoch 407/1000, Loss: 0,0013755751423969544
                Epoch 408/1000, Loss: 0,0013754858300632053
                Epoch 409/1000, Loss: 0,0013753972206702548
                Epoch 410/1000, Loss: 0,0013753093046488189
                Epoch 411/1000, Loss: 0,0013752220725900398
                Epoch 412/1000, Loss: 0,0013751355152426107
                Epoch 413/1000, Loss: 0,0013750496235099467
                Epoch 414/1000, Loss: 0,0013749643884474066
                Epoch 415/1000, Loss: 0,0013748798012595633
                Epoch 416/1000, Loss: 0,0013747958532975208
                Epoch 417/1000, Loss: 0,0013747125360562772
                Epoch 418/1000, Loss: 0,0013746298411721355
                Epoch 419/1000, Loss: 0,0013745477604201574
                Epoch 420/1000, Loss: 0,0013744662857116654
                Epoch 421/1000, Loss: 0,0013743854090917838
                Epoch 422/1000, Loss: 0,0013743051227370275
                Epoch 423/1000, Loss: 0,0013742254189529311
                Epoch 424/1000, Loss: 0,0013741462901717206
                Epoch 425/1000, Loss: 0,0013740677289500287
                Epoch 426/1000, Loss: 0,0013739897279666459
                Epoch 427/1000, Loss: 0,0013739122800203167
                Epoch 428/1000, Loss: 0,00137383537802757
                Epoch 429/1000, Loss: 0,0013737590150205963
                Epoch 430/1000, Loss: 0,0013736831841451543
                Epoch 431/1000, Loss: 0,0013736078786585196
                Epoch 432/1000, Loss: 0,001373533091927473
                Epoch 433/1000, Loss: 0,0013734588174263184
                Epoch 434/1000, Loss: 0,0013733850487349405
                Epoch 435/1000, Loss: 0,001373311779536902
                Epoch 436/1000, Loss: 0,0013732390036175649
                Epoch 437/1000, Loss: 0,0013731667148622548
                Epoch 438/1000, Loss: 0,0013730949072544567
                Epoch 439/1000, Loss: 0,0013730235748740409
                Epoch 440/1000, Loss: 0,0013729527118955247
                Epoch 441/1000, Loss: 0,0013728823125863634
                Epoch 442/1000, Loss: 0,0013728123713052745
                Epoch 443/1000, Loss: 0,0013727428825005918
                Epoch 444/1000, Loss: 0,0013726738407086494
                Epoch 445/1000, Loss: 0,0013726052405521965
                Epoch 446/1000, Loss: 0,0013725370767388406
                Epoch 447/1000, Loss: 0,0013724693440595194
                Epoch 448/1000, Loss: 0,001372402037387002
                Epoch 449/1000, Loss: 0,0013723351516744162
                Epoch 450/1000, Loss: 0,001372268681953805
                Epoch 451/1000, Loss: 0,0013722026233347067
                Epoch 452/1000, Loss: 0,001372136971002765
                Epoch 453/1000, Loss: 0,0013720717202183653
                Epoch 454/1000, Loss: 0,0013720068663152895
                Epoch 455/1000, Loss: 0,0013719424046994032
                Epoch 456/1000, Loss: 0,0013718783308473684
                Epoch 457/1000, Loss: 0,0013718146403053704
                Epoch 458/1000, Loss: 0,0013717513286878782
                Epoch 459/1000, Loss: 0,0013716883916764261
                Epoch 460/1000, Loss: 0,0013716258250184117
                Epoch 461/1000, Loss: 0,001371563624525926
                Epoch 462/1000, Loss: 0,0013715017860745982
                Epoch 463/1000, Loss: 0,0013714403056024655
                Epoch 464/1000, Loss: 0,0013713791791088621
                Epoch 465/1000, Loss: 0,00137131840265333
                Epoch 466/1000, Loss: 0,001371257972354553
                Epoch 467/1000, Loss: 0,0013711978843893044
                Epoch 468/1000, Loss: 0,0013711381349914191
                Epoch 469/1000, Loss: 0,0013710787204507858
                Epoch 470/1000, Loss: 0,0013710196371123536
                Epoch 471/1000, Loss: 0,0013709608813751615
                Epoch 472/1000, Loss: 0,0013709024496913846
                Epoch 473/1000, Loss: 0,0013708443385653977
                Epoch 474/1000, Loss: 0,001370786544552857
                Epoch 475/1000, Loss: 0,0013707290642597993
                Epoch 476/1000, Loss: 0,0013706718943417592
                Epoch 477/1000, Loss: 0,0013706150315028985
                Epoch 478/1000, Loss: 0,0013705584724951598
                Epoch 479/1000, Loss: 0,0013705022141174265
                Epoch 480/1000, Loss: 0,0013704462532147083
                Epoch 481/1000, Loss: 0,0013703905866773321
                Epoch 482/1000, Loss: 0,0013703352114401589
                Epoch 483/1000, Loss: 0,001370280124481807
                Epoch 484/1000, Loss: 0,0013702253228238905
                Epoch 485/1000, Loss: 0,0013701708035302796
                Epoch 486/1000, Loss: 0,001370116563706366
                Epoch 487/1000, Loss: 0,0013700626004983466
                Epoch 488/1000, Loss: 0,0013700089110925194
                Epoch 489/1000, Loss: 0,0013699554927145943
                Epoch 490/1000, Loss: 0,0013699023426290142
                Epoch 491/1000, Loss: 0,0013698494581382925
                Epoch 492/1000, Loss: 0,001369796836582357
                Epoch 493/1000, Loss: 0,0013697444753379153
                Epoch 494/1000, Loss: 0,001369692371817823
                Epoch 495/1000, Loss: 0,001369640523470468
                Epoch 496/1000, Loss: 0,0013695889277791703
                Epoch 497/1000, Loss: 0,0013695375822615804
                Epoch 498/1000, Loss: 0,0013694864844691072
                Epoch 499/1000, Loss: 0,0013694356319863394
                Epoch 500/1000, Loss: 0,0013693850224304892
                Epoch 501/1000, Loss: 0,0013693346534508414
                Epoch 502/1000, Loss: 0,0013692845227282133
                Epoch 503/1000, Loss: 0,0013692346279744266
                Epoch 504/1000, Loss: 0,0013691849669317862
                Epoch 505/1000, Loss: 0,0013691355373725725
                Epoch 506/1000, Loss: 0,001369086337098541
                Epoch 507/1000, Loss: 0,0013690373639404275
                Epoch 508/1000, Loss: 0,0013689886157574737
                Epoch 509/1000, Loss: 0,001368940090436947
                Epoch 510/1000, Loss: 0,0013688917858936825
                Epoch 511/1000, Loss: 0,0013688437000696232
                Epoch 512/1000, Loss: 0,001368795830933378
                Epoch 513/1000, Loss: 0,0013687481764797782
                Epoch 514/1000, Loss: 0,0013687007347294527
                Epoch 515/1000, Loss: 0,0013686535037283998
                Epoch 516/1000, Loss: 0,001368606481547578
                Epoch 517/1000, Loss: 0,0013685596662824974
                Epoch 518/1000, Loss: 0,0013685130560528187
                Epoch 519/1000, Loss: 0,0013684666490019653
                Epoch 520/1000, Loss: 0,0013684204432967335
                Epoch 521/1000, Loss: 0,0013683744371269224
                Epoch 522/1000, Loss: 0,0013683286287049555
                Epoch 523/1000, Loss: 0,0013682830162655237
                Epoch 524/1000, Loss: 0,0013682375980652237
                Epoch 525/1000, Loss: 0,0013681923723822106
                Epoch 526/1000, Loss: 0,0013681473375158535
                Epoch 527/1000, Loss: 0,0013681024917863971
                Epoch 528/1000, Loss: 0,0013680578335346309
                Epoch 529/1000, Loss: 0,0013680133611215645
                Epoch 530/1000, Loss: 0,001367969072928106
                Epoch 531/1000, Loss: 0,0013679249673547525
                Epoch 532/1000, Loss: 0,0013678810428212804
                Epoch 533/1000, Loss: 0,0013678372977664398
                Epoch 534/1000, Loss: 0,0013677937306476652
                Epoch 535/1000, Loss: 0,0013677503399407785
                Epoch 536/1000, Loss: 0,0013677071241397052
                Epoch 537/1000, Loss: 0,0013676640817561927
                Epoch 538/1000, Loss: 0,0013676212113195375
                Epoch 539/1000, Loss: 0,0013675785113763106
                Epoch 540/1000, Loss: 0,0013675359804900952
                Epoch 541/1000, Loss: 0,001367493617241224
                Epoch 542/1000, Loss: 0,0013674514202265228
                Epoch 543/1000, Loss: 0,0013674093880590612
                Epoch 544/1000, Loss: 0,0013673675193679022
                Epoch 545/1000, Loss: 0,0013673258127978624
                Epoch 546/1000, Loss: 0,0013672842670092718
                Epoch 547/1000, Loss: 0,0013672428806777415
                Epoch 548/1000, Loss: 0,001367201652493934
                Epoch 549/1000, Loss: 0,0013671605811633341
                Epoch 550/1000, Loss: 0,0013671196654060328
                Epoch 551/1000, Loss: 0,0013670789039565037
                Epoch 552/1000, Loss: 0,001367038295563394
                Epoch 553/1000, Loss: 0,0013669978389893123
                Epoch 554/1000, Loss: 0,0013669575330106216
                Epoch 555/1000, Loss: 0,0013669173764172386
                Epoch 556/1000, Loss: 0,0013668773680124324
                Epoch 557/1000, Loss: 0,0013668375066126317
                Epoch 558/1000, Loss: 0,0013667977910472287
                Epoch 559/1000, Loss: 0,0013667582201583955
                Epoch 560/1000, Loss: 0,0013667187928008927
                Epoch 561/1000, Loss: 0,0013666795078418937
                Epoch 562/1000, Loss: 0,0013666403641607995
                Epoch 563/1000, Loss: 0,0013666013606490677
                Epoch 564/1000, Loss: 0,0013665624962100364
                Epoch 565/1000, Loss: 0,001366523769758756
                Epoch 566/1000, Loss: 0,001366485180221822
                Epoch 567/1000, Loss: 0,0013664467265372122
                Epoch 568/1000, Loss: 0,001366408407654122
                Epoch 569/1000, Loss: 0,0013663702225328099
                Epoch 570/1000, Loss: 0,0013663321701444405
                Epoch 571/1000, Loss: 0,0013662942494709314
                Epoch 572/1000, Loss: 0,001366256459504803
                Epoch 573/1000, Loss: 0,0013662187992490298
                Epoch 574/1000, Loss: 0,001366181267716897
                Epoch 575/1000, Loss: 0,001366143863931857
                Epoch 576/1000, Loss: 0,001366106586927388
                Epoch 577/1000, Loss: 0,0013660694357468574
                Epoch 578/1000, Loss: 0,001366032409443387
                Epoch 579/1000, Loss: 0,0013659955070797164
                Epoch 580/1000, Loss: 0,0013659587277280762
                Epoch 581/1000, Loss: 0,0013659220704700542
                Epoch 582/1000, Loss: 0,0013658855343964763
                Epoch 583/1000, Loss: 0,0013658491186072722
                Epoch 584/1000, Loss: 0,0013658128222113614
                Epoch 585/1000, Loss: 0,0013657766443265279
                Epoch 586/1000, Loss: 0,0013657405840793036
                Epoch 587/1000, Loss: 0,0013657046406048523
                Epoch 588/1000, Loss: 0,001365668813046853
                Epoch 589/1000, Loss: 0,0013656331005573897
                Epoch 590/1000, Loss: 0,0013655975022968416
                Epoch 591/1000, Loss: 0,0013655620174337683
                Epoch 592/1000, Loss: 0,001365526645144811
                Epoch 593/1000, Loss: 0,001365491384614579
                Epoch 594/1000, Loss: 0,0013654562350355532
                Epoch 595/1000, Loss: 0,0013654211956079796
                Epoch 596/1000, Loss: 0,0013653862655397695
                Epoch 597/1000, Loss: 0,0013653514440464048
                Epoch 598/1000, Loss: 0,0013653167303508333
                Epoch 599/1000, Loss: 0,0013652821236833815
                Epoch 600/1000, Loss: 0,0013652476232816559
                Epoch 601/1000, Loss: 0,0013652132283904523
                Epoch 602/1000, Loss: 0,0013651789382616623
                Epoch 603/1000, Loss: 0,001365144752154188
                Epoch 604/1000, Loss: 0,0013651106693338508
                Epoch 605/1000, Loss: 0,0013650766890733056
                Epoch 606/1000, Loss: 0,0013650428106519567
                Epoch 607/1000, Loss: 0,0013650090333558718
                Epoch 608/1000, Loss: 0,0013649753564777018
                Epoch 609/1000, Loss: 0,0013649417793165969
                Epoch 610/1000, Loss: 0,0013649083011781288
                Epoch 611/1000, Loss: 0,001364874921374212
                Epoch 612/1000, Loss: 0,0013648416392230242
                Epoch 613/1000, Loss: 0,0013648084540489314
                Epoch 614/1000, Loss: 0,0013647753651824146
                Epoch 615/1000, Loss: 0,001364742371959992
                Epoch 616/1000, Loss: 0,0013647094737241502
                Epoch 617/1000, Loss: 0,0013646766698232685
                Epoch 618/1000, Loss: 0,0013646439596115539
                Epoch 619/1000, Loss: 0,001364611342448965
                Epoch 620/1000, Loss: 0,0013645788177011487
                Epoch 621/1000, Loss: 0,0013645463847393722
                Epoch 622/1000, Loss: 0,0013645140429404535
                Epoch 623/1000, Loss: 0,0013644817916866991
                Epoch 624/1000, Loss: 0,0013644496303658405
                Epoch 625/1000, Loss: 0,0013644175583709677
                Epoch 626/1000, Loss: 0,0013643855751004695
                Epoch 627/1000, Loss: 0,001364353679957971
                Epoch 628/1000, Loss: 0,0013643218723522724
                Epoch 629/1000, Loss: 0,001364290151697291
                Epoch 630/1000, Loss: 0,001364258517412002
                Epoch 631/1000, Loss: 0,0013642269689203802
                Epoch 632/1000, Loss: 0,0013641955056513443
                Epoch 633/1000, Loss: 0,0013641641270386992
                Epoch 634/1000, Loss: 0,00136413283252108
                Epoch 635/1000, Loss: 0,0013641016215419036
                Epoch 636/1000, Loss: 0,0013640704935493045
                Epoch 637/1000, Loss: 0,0013640394479960919
                Epoch 638/1000, Loss: 0,0013640084843396937
                Epoch 639/1000, Loss: 0,001363977602042103
                Epoch 640/1000, Loss: 0,0013639468005698318
                Epoch 641/1000, Loss: 0,0013639160793938578
                Epoch 642/1000, Loss: 0,001363885437989578
                Epoch 643/1000, Loss: 0,0013638548758367563
                Epoch 644/1000, Loss: 0,0013638243924194813
                Epoch 645/1000, Loss: 0,001363793987226116
                Epoch 646/1000, Loss: 0,0013637636597492503
                Epoch 647/1000, Loss: 0,001363733409485658
                Epoch 648/1000, Loss: 0,00136370323593625
                Epoch 649/1000, Loss: 0,001363673138606033
                Epoch 650/1000, Loss: 0,0013636431170040602
                Epoch 651/1000, Loss: 0,0013636131706433927
                Epoch 652/1000, Loss: 0,0013635832990410574
                Epoch 653/1000, Loss: 0,0013635535017180002
                Epoch 654/1000, Loss: 0,0013635237781990514
                Epoch 655/1000, Loss: 0,001363494128012877
                Epoch 656/1000, Loss: 0,0013634645506919471
                Epoch 657/1000, Loss: 0,0013634350457724907
                Epoch 658/1000, Loss: 0,001363405612794457
                Epoch 659/1000, Loss: 0,001363376251301479
                Epoch 660/1000, Loss: 0,0013633469608408336
                Epoch 661/1000, Loss: 0,001363317740963407
                Epoch 662/1000, Loss: 0,0013632885912236521
                Epoch 663/1000, Loss: 0,0013632595111795593
                Epoch 664/1000, Loss: 0,0013632305003926145
                Epoch 665/1000, Loss: 0,0013632015584277677
                Epoch 666/1000, Loss: 0,001363172684853395
                Epoch 667/1000, Loss: 0,0013631438792412662
                Epoch 668/1000, Loss: 0,0013631151411665095
                Epoch 669/1000, Loss: 0,0013630864702075792
                Epoch 670/1000, Loss: 0,0013630578659462205
                Epoch 671/1000, Loss: 0,0013630293279674399
                Epoch 672/1000, Loss: 0,0013630008558594693
                Epoch 673/1000, Loss: 0,0013629724492137383
                Epoch 674/1000, Loss: 0,0013629441076248373
                Epoch 675/1000, Loss: 0,0013629158306904922
                Epoch 676/1000, Loss: 0,0013628876180115323
                Epoch 677/1000, Loss: 0,001362859469191856
                Epoch 678/1000, Loss: 0,0013628313838384067
                Epoch 679/1000, Loss: 0,0013628033615611394
                Epoch 680/1000, Loss: 0,0013627754019729951
                Epoch 681/1000, Loss: 0,0013627475046898685
                Epoch 682/1000, Loss: 0,001362719669330583
                Epoch 683/1000, Loss: 0,001362691895516859
                Epoch 684/1000, Loss: 0,001362664182873292
                Epoch 685/1000, Loss: 0,0013626365310273204
                Epoch 686/1000, Loss: 0,0013626089396091999
                Epoch 687/1000, Loss: 0,0013625814082519787
                Epoch 688/1000, Loss: 0,0013625539365914705
                Epoch 689/1000, Loss: 0,0013625265242662259
                Epoch 690/1000, Loss: 0,0013624991709175129
                Epoch 691/1000, Loss: 0,001362471876189286
                Epoch 692/1000, Loss: 0,001362444639728164
                Epoch 693/1000, Loss: 0,0013624174611834054
                Epoch 694/1000, Loss: 0,001362390340206883
                Epoch 695/1000, Loss: 0,0013623632764530624
                Epoch 696/1000, Loss: 0,0013623362695789763
                Epoch 697/1000, Loss: 0,0013623093192442002
                Epoch 698/1000, Loss: 0,0013622824251108324
                Epoch 699/1000, Loss: 0,0013622555868434694
                Epoch 700/1000, Loss: 0,0013622288041091825
                Epoch 701/1000, Loss: 0,001362202076577499
                Epoch 702/1000, Loss: 0,001362175403920376
                Epoch 703/1000, Loss: 0,0013621487858121828
                Epoch 704/1000, Loss: 0,0013621222219296744
                Epoch 705/1000, Loss: 0,001362095711951977
                Epoch 706/1000, Loss: 0,001362069255560561
                Epoch 707/1000, Loss: 0,0013620428524392246
                Epoch 708/1000, Loss: 0,0013620165022740706
                Epoch 709/1000, Loss: 0,001361990204753489
                Epoch 710/1000, Loss: 0,0013619639595681323
                Epoch 711/1000, Loss: 0,001361937766410903
                Epoch 712/1000, Loss: 0,0013619116249769278
                Epoch 713/1000, Loss: 0,0013618855349635422
                Epoch 714/1000, Loss: 0,0013618594960702682
                Epoch 715/1000, Loss: 0,0013618335079988004
                Epoch 716/1000, Loss: 0,0013618075704529815
                Epoch 717/1000, Loss: 0,0013617816831387897
                Epoch 718/1000, Loss: 0,0013617558457643176
                Epoch 719/1000, Loss: 0,001361730058039752
                Epoch 720/1000, Loss: 0,0013617043196773634
                Epoch 721/1000, Loss: 0,0013616786303914804
                Epoch 722/1000, Loss: 0,0013616529898984784
                Epoch 723/1000, Loss: 0,0013616273979167584
                Epoch 724/1000, Loss: 0,0013616018541667337
                Epoch 725/1000, Loss: 0,00136157635837081
                Epoch 726/1000, Loss: 0,0013615509102533725
                Epoch 727/1000, Loss: 0,001361525509540765
                Epoch 728/1000, Loss: 0,0013615001559612778
                Epoch 729/1000, Loss: 0,0013614748492451294
                Epoch 730/1000, Loss: 0,0013614495891244532
                Epoch 731/1000, Loss: 0,001361424375333279
                Epoch 732/1000, Loss: 0,0013613992076075185
                Epoch 733/1000, Loss: 0,0013613740856849518
                Epoch 734/1000, Loss: 0,0013613490093052098
                Epoch 735/1000, Loss: 0,0013613239782097615
                Epoch 736/1000, Loss: 0,0013612989921418985
                Epoch 737/1000, Loss: 0,0013612740508467188
                Epoch 738/1000, Loss: 0,001361249154071115
                Epoch 739/1000, Loss: 0,0013612243015637584
                Epoch 740/1000, Loss: 0,001361199493075086
                Epoch 741/1000, Loss: 0,001361174728357285
                Epoch 742/1000, Loss: 0,0013611500071642815
                Epoch 743/1000, Loss: 0,0013611253292517232
                Epoch 744/1000, Loss: 0,0013611006943769698
                Epoch 745/1000, Loss: 0,0013610761022990769
                Epoch 746/1000, Loss: 0,0013610515527787848
                Epoch 747/1000, Loss: 0,0013610270455785029
                Epoch 748/1000, Loss: 0,0013610025804623004
                Epoch 749/1000, Loss: 0,001360978157195889
                Epoch 750/1000, Loss: 0,0013609537755466148
                Epoch 751/1000, Loss: 0,0013609294352834426
                Epoch 752/1000, Loss: 0,0013609051361769455
                Epoch 753/1000, Loss: 0,0013608808779992895
                Epoch 754/1000, Loss: 0,0013608566605242257
                Epoch 755/1000, Loss: 0,0013608324835270769
                Epoch 756/1000, Loss: 0,0013608083467847236
                Epoch 757/1000, Loss: 0,0013607842500755925
                Epoch 758/1000, Loss: 0,0013607601931796482
                Epoch 759/1000, Loss: 0,0013607361758783802
                Epoch 760/1000, Loss: 0,001360712197954788
                Epoch 761/1000, Loss: 0,0013606882591933758
                Epoch 762/1000, Loss: 0,0013606643593801358
                Epoch 763/1000, Loss: 0,0013606404983025403
                Epoch 764/1000, Loss: 0,0013606166757495312
                Epoch 765/1000, Loss: 0,0013605928915115084
                Epoch 766/1000, Loss: 0,0013605691453803153
                Epoch 767/1000, Loss: 0,0013605454371492353
                Epoch 768/1000, Loss: 0,0013605217666129766
                Epoch 769/1000, Loss: 0,0013604981335676627
                Epoch 770/1000, Loss: 0,0013604745378108223
                Epoch 771/1000, Loss: 0,0013604509791413785
                Epoch 772/1000, Loss: 0,0013604274573596404
                Epoch 773/1000, Loss: 0,001360403972267292
                Epoch 774/1000, Loss: 0,0013603805236673816
                Epoch 775/1000, Loss: 0,0013603571113643127
                Epoch 776/1000, Loss: 0,0013603337351638365
                Epoch 777/1000, Loss: 0,0013603103948730364
                Epoch 778/1000, Loss: 0,0013602870903003258
                Epoch 779/1000, Loss: 0,001360263821255434
                Epoch 780/1000, Loss: 0,0013602405875493963
                Epoch 781/1000, Loss: 0,0013602173889945496
                Epoch 782/1000, Loss: 0,0013601942254045179
                Epoch 783/1000, Loss: 0,0013601710965942067
                Epoch 784/1000, Loss: 0,001360148002379792
                Epoch 785/1000, Loss: 0,0013601249425787124
                Epoch 786/1000, Loss: 0,001360101917009662
                Epoch 787/1000, Loss: 0,0013600789254925765
                Epoch 788/1000, Loss: 0,001360055967848631
                Epoch 789/1000, Loss: 0,0013600330439002255
                Epoch 790/1000, Loss: 0,0013600101534709827
                Epoch 791/1000, Loss: 0,0013599872963857339
                Epoch 792/1000, Loss: 0,0013599644724705123
                Epoch 793/1000, Loss: 0,001359941681552548
                Epoch 794/1000, Loss: 0,0013599189234602548
                Epoch 795/1000, Loss: 0,0013598961980232263
                Epoch 796/1000, Loss: 0,0013598735050722262
                Epoch 797/1000, Loss: 0,0013598508444391771
                Epoch 798/1000, Loss: 0,0013598282159571614
                Epoch 799/1000, Loss: 0,0013598056194604035
                Epoch 800/1000, Loss: 0,0013597830547842679
                Epoch 801/1000, Loss: 0,001359760521765251
                Epoch 802/1000, Loss: 0,0013597380202409709
                Epoch 803/1000, Loss: 0,0013597155500501637
                Epoch 804/1000, Loss: 0,0013596931110326728
                Epoch 805/1000, Loss: 0,001359670703029442
                Epoch 806/1000, Loss: 0,0013596483258825117
                Epoch 807/1000, Loss: 0,0013596259794350055
                Epoch 808/1000, Loss: 0,001359603663531129
                Epoch 809/1000, Loss: 0,0013595813780161582
                Epoch 810/1000, Loss: 0,0013595591227364343
                Epoch 811/1000, Loss: 0,0013595368975393588
                Epoch 812/1000, Loss: 0,0013595147022733808
                Epoch 813/1000, Loss: 0,0013594925367879974
                Epoch 814/1000, Loss: 0,0013594704009337413
                Epoch 815/1000, Loss: 0,001359448294562176
                Epoch 816/1000, Loss: 0,0013594262175258887
                Epoch 817/1000, Loss: 0,0013594041696784848
                Epoch 818/1000, Loss: 0,00135938215087458
                Epoch 819/1000, Loss: 0,001359360160969794
                Epoch 820/1000, Loss: 0,0013593381998207449
                Epoch 821/1000, Loss: 0,0013593162672850413
                Epoch 822/1000, Loss: 0,0013592943632212769
                Epoch 823/1000, Loss: 0,0013592724874890239
                Epoch 824/1000, Loss: 0,0013592506399488279
                Epoch 825/1000, Loss: 0,0013592288204621985
                Epoch 826/1000, Loss: 0,0013592070288916073
                Epoch 827/1000, Loss: 0,0013591852651004795
                Epoch 828/1000, Loss: 0,001359163528953186
                Epoch 829/1000, Loss: 0,0013591418203150413
                Epoch 830/1000, Loss: 0,0013591201390522939
                Epoch 831/1000, Loss: 0,0013590984850321258
                Epoch 832/1000, Loss: 0,0013590768581226381
                Epoch 833/1000, Loss: 0,001359055258192854
                Epoch 834/1000, Loss: 0,001359033685112706
                Epoch 835/1000, Loss: 0,001359012138753036
                Epoch 836/1000, Loss: 0,0013589906189855837
                Epoch 837/1000, Loss: 0,0013589691256829873
                Epoch 838/1000, Loss: 0,0013589476587187728
                Epoch 839/1000, Loss: 0,0013589262179673498
                Epoch 840/1000, Loss: 0,0013589048033040077
                Epoch 841/1000, Loss: 0,0013588834146049083
                Epoch 842/1000, Loss: 0,0013588620517470831
                Epoch 843/1000, Loss: 0,0013588407146084224
                Epoch 844/1000, Loss: 0,0013588194030676765
                Epoch 845/1000, Loss: 0,0013587981170044452
                Epoch 846/1000, Loss: 0,001358776856299176
                Epoch 847/1000, Loss: 0,0013587556208331574
                Epoch 848/1000, Loss: 0,0013587344104885144
                Epoch 849/1000, Loss: 0,0013587132251482016
                Epoch 850/1000, Loss: 0,0013586920646959993
                Epoch 851/1000, Loss: 0,0013586709290165108
                Epoch 852/1000, Loss: 0,0013586498179951516
                Epoch 853/1000, Loss: 0,0013586287315181514
                Epoch 854/1000, Loss: 0,0013586076694725422
                Epoch 855/1000, Loss: 0,0013585866317461605
                Epoch 856/1000, Loss: 0,0013585656182276353
                Epoch 857/1000, Loss: 0,0013585446288063886
                Epoch 858/1000, Loss: 0,0013585236633726276
                Epoch 859/1000, Loss: 0,0013585027218173422
                Epoch 860/1000, Loss: 0,0013584818040322981
                Epoch 861/1000, Loss: 0,001358460909910033
                Epoch 862/1000, Loss: 0,0013584400393438535
                Epoch 863/1000, Loss: 0,0013584191922278266
                Epoch 864/1000, Loss: 0,0013583983684567781
                Epoch 865/1000, Loss: 0,0013583775679262891
                Epoch 866/1000, Loss: 0,001358356790532688
                Epoch 867/1000, Loss: 0,0013583360361730471
                Epoch 868/1000, Loss: 0,0013583153047451806
                Epoch 869/1000, Loss: 0,001358294596147637
                Epoch 870/1000, Loss: 0,001358273910279697
                Epoch 871/1000, Loss: 0,001358253247041367
                Epoch 872/1000, Loss: 0,0013582326063333759
                Epoch 873/1000, Loss: 0,001358211988057171
                Epoch 874/1000, Loss: 0,0013581913921149136
                Epoch 875/1000, Loss: 0,0013581708184094753
                Epoch 876/1000, Loss: 0,0013581502668444303
                Epoch 877/1000, Loss: 0,0013581297373240566
                Epoch 878/1000, Loss: 0,0013581092297533278
                Epoch 879/1000, Loss: 0,0013580887440379115
                Epoch 880/1000, Loss: 0,0013580682800841616
                Epoch 881/1000, Loss: 0,0013580478377991187
                Epoch 882/1000, Loss: 0,0013580274170905038
                Epoch 883/1000, Loss: 0,0013580070178667125
                Epoch 884/1000, Loss: 0,0013579866400368146
                Epoch 885/1000, Loss: 0,0013579662835105483
                Epoch 886/1000, Loss: 0,001357945948198314
                Epoch 887/1000, Loss: 0,0013579256340111754
                Epoch 888/1000, Loss: 0,0013579053408608517
                Epoch 889/1000, Loss: 0,0013578850686597151
                Epoch 890/1000, Loss: 0,0013578648173207856
                Epoch 891/1000, Loss: 0,0013578445867577292
                Epoch 892/1000, Loss: 0,0013578243768848526
                Epoch 893/1000, Loss: 0,0013578041876171018
                Epoch 894/1000, Loss: 0,0013577840188700526
                Epoch 895/1000, Loss: 0,0013577638705599146
                Epoch 896/1000, Loss: 0,0013577437426035219
                Epoch 897/1000, Loss: 0,001357723634918331
                Epoch 898/1000, Loss: 0,0013577035474224165
                Epoch 899/1000, Loss: 0,0013576834800344702
                Epoch 900/1000, Loss: 0,0013576634326737942
                Epoch 901/1000, Loss: 0,001357643405260299
                Epoch 902/1000, Loss: 0,0013576233977144988
                Epoch 903/1000, Loss: 0,0013576034099575089
                Epoch 904/1000, Loss: 0,001357583441911042
                Epoch 905/1000, Loss: 0,0013575634934974048
                Epoch 906/1000, Loss: 0,0013575435646394946
                Epoch 907/1000, Loss: 0,0013575236552607942
                Epoch 908/1000, Loss: 0,0013575037652853712
                Epoch 909/1000, Loss: 0,0013574838946378712
                Epoch 910/1000, Loss: 0,0013574640432435184
                Epoch 911/1000, Loss: 0,0013574442110281086
                Epoch 912/1000, Loss: 0,0013574243979180083
                Epoch 913/1000, Loss: 0,0013574046038401502
                Epoch 914/1000, Loss: 0,0013573848287220284
                Epoch 915/1000, Loss: 0,0013573650724916998
                Epoch 916/1000, Loss: 0,001357345335077775
                Epoch 917/1000, Loss: 0,0013573256164094195
                Epoch 918/1000, Loss: 0,0013573059164163482
                Epoch 919/1000, Loss: 0,0013572862350288214
                Epoch 920/1000, Loss: 0,0013572665721776444
                Epoch 921/1000, Loss: 0,0013572469277941626
                Epoch 922/1000, Loss: 0,0013572273018102575
                Epoch 923/1000, Loss: 0,0013572076941583459
                Epoch 924/1000, Loss: 0,001357188104771374
                Epoch 925/1000, Loss: 0,001357168533582816
                Epoch 926/1000, Loss: 0,0013571489805266706
                Epoch 927/1000, Loss: 0,001357129445537459
                Epoch 928/1000, Loss: 0,0013571099285502191
                Epoch 929/1000, Loss: 0,001357090429500505
                Epoch 930/1000, Loss: 0,0013570709483243827
                Epoch 931/1000, Loss: 0,0013570514849584273
                Epoch 932/1000, Loss: 0,0013570320393397205
                Epoch 933/1000, Loss: 0,001357012611405847
                Epoch 934/1000, Loss: 0,001356993201094891
                Epoch 935/1000, Loss: 0,0013569738083454358
                Epoch 936/1000, Loss: 0,0013569544330965574
                Epoch 937/1000, Loss: 0,0013569350752878234
                Epoch 938/1000, Loss: 0,0013569157348592897
                Epoch 939/1000, Loss: 0,0013568964117514992
                Epoch 940/1000, Loss: 0,0013568771059054758
                Epoch 941/1000, Loss: 0,0013568578172627234
                Epoch 942/1000, Loss: 0,0013568385457652236
                Epoch 943/1000, Loss: 0,0013568192913554322
                Epoch 944/1000, Loss: 0,001356800053976275
                Epoch 945/1000, Loss: 0,0013567808335711487
                Epoch 946/1000, Loss: 0,0013567616300839123
                Epoch 947/1000, Loss: 0,0013567424434588918
                Epoch 948/1000, Loss: 0,0013567232736408689
                Epoch 949/1000, Loss: 0,0013567041205750862
                Epoch 950/1000, Loss: 0,001356684984207239
                Epoch 951/1000, Loss: 0,0013566658644834765
                Epoch 952/1000, Loss: 0,0013566467613503944
                Epoch 953/1000, Loss: 0,0013566276747550374
                Epoch 954/1000, Loss: 0,0013566086046448928
                Epoch 955/1000, Loss: 0,0013565895509678886
                Epoch 956/1000, Loss: 0,0013565705136723931
                Epoch 957/1000, Loss: 0,0013565514927072091
                Epoch 958/1000, Loss: 0,0013565324880215734
                Epoch 959/1000, Loss: 0,0013565134995651522
                Epoch 960/1000, Loss: 0,0013564945272880413
                Epoch 961/1000, Loss: 0,0013564755711407612
                Epoch 962/1000, Loss: 0,0013564566310742554
                Epoch 963/1000, Loss: 0,0013564377070398876
                Epoch 964/1000, Loss: 0,0013564187989894391
                Epoch 965/1000, Loss: 0,0013563999068751074
                Epoch 966/1000, Loss: 0,001356381030649501
                Epoch 967/1000, Loss: 0,0013563621702656406
                Epoch 968/1000, Loss: 0,0013563433256769532
                Epoch 969/1000, Loss: 0,0013563244968372713
                Epoch 970/1000, Loss: 0,0013563056837008302
                Epoch 971/1000, Loss: 0,0013562868862222654
                Epoch 972/1000, Loss: 0,0013562681043566107
                Epoch 973/1000, Loss: 0,0013562493380592949
                Epoch 974/1000, Loss: 0,0013562305872861394
                Epoch 975/1000, Loss: 0,0013562118519933558
                Epoch 976/1000, Loss: 0,001356193132137546
                Epoch 977/1000, Loss: 0,001356174427675695
                Epoch 978/1000, Loss: 0,0013561557385651727
                Epoch 979/1000, Loss: 0,001356137064763729
                Epoch 980/1000, Loss: 0,001356118406229494
                Epoch 981/1000, Loss: 0,0013560997629209718
                Epoch 982/1000, Loss: 0,0013560811347970415
                Epoch 983/1000, Loss: 0,0013560625218169554
                Epoch 984/1000, Loss: 0,001356043923940332
                Epoch 985/1000, Loss: 0,0013560253411271585
                Epoch 986/1000, Loss: 0,0013560067733377873
                Epoch 987/1000, Loss: 0,0013559882205329327
                Epoch 988/1000, Loss: 0,0013559696826736683
                Epoch 989/1000, Loss: 0,0013559511597214266
                Epoch 990/1000, Loss: 0,0013559326516379949
                Epoch 991/1000, Loss: 0,001355914158385515
                Epoch 992/1000, Loss: 0,0013558956799264786
                Epoch 993/1000, Loss: 0,0013558772162237268
                Epoch 994/1000, Loss: 0,0013558587672404478
                Epoch 995/1000, Loss: 0,001355840332940173
                Epoch 996/1000, Loss: 0,0013558219132867783
                Epoch 997/1000, Loss: 0,0013558035082444782
                Epoch 998/1000, Loss: 0,0013557851177778246
                Epoch 999/1000, Loss: 0,001355766741851707
                Epoch 1000/1000, Loss: 0,0013557483804313476
                Testing trained RNN on physics sentences:
                Input: energy is conserved in system
                Predicted: is conserved in vacuum

                Input: force equals mass times acceleration
                Predicted: equals mass times acceleration

                Input: light travels in vacuum at speed
                Predicted: travels in vacuum at speed

                Input: electron has negative charge
                Predicted: has negative at

                Input: gravity pulls objects towards earth
                Predicted: pulls objects towards earth


                Code:
                using System;
                using System.Collections.Generic;
                using System.Globalization;
                using System.IO;

                public class Program
                {
                    public static void Main(string[] args)
                    {
                        string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
                        int embeddingDim = 300;
                        var glove = new GloveLoader(gloveFilePath, embeddingDim);

                        int hiddenSize = 10;
                        double learningRate = 0.1;
                        int epochs = 1000;

                        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

                        // Five physics-related sentences for training
                        List<string[]> physicsSentences = new List<string[]>
                        {
                            new string[] { "energy", "is", "conserved", "in", "system" },
                            new string[] { "force", "equals", "mass", "times", "acceleration" },
                            new string[] { "light", "travels", "in", "vacuum", "at", "speed" },
                            new string[] { "electron", "has", "negative", "charge" },
                            new string[] { "gravity", "pulls", "objects", "towards", "earth" }
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
                            Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
                        }

                        // Testing the RNN to see if it can recall physics sentences
                        Console.WriteLine("Testing trained RNN on physics sentences:");
                        foreach (var sentence in physicsSentences)
                        {
                            Console.Write("Input: ");
                            foreach (var word in sentence)
                            {
                                Console.Write(word + " ");
                            }
                            Console.Write("\nPredicted: ");

                            List<double[]> sentenceEmbeddings = new List<double[]>();
                            foreach (var word in sentence)
                            {
                                sentenceEmbeddings.Add(glove.GetEmbedding(word));
                            }

                            for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                            {
                                double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                                string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                                Console.Write(predictedWord + " ");
                            }
                            Console.WriteLine("\n");
                        }

                        Console.ReadKey();
                    }
                }

                Code:
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


                Code:
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

                Code:
                using System;
                using System.Collections.Generic;
                using System.Globalization;
                using System.IO;

                public class Program
                {
                    public static void Main(string[] args)
                    {
                        string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
                        int embeddingDim = 300;
                        var glove = new GloveLoader(gloveFilePath, embeddingDim);

                        int hiddenSize = 10;
                        double learningRate = 0.1;
                        int epochs = 1000;

                        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

                        // Five physics-related sentences for training
                        List<string[]> physicsSentences = new List<string[]>
                        {
                            new string[] { "energy", "is", "conserved", "in", "system" },
                            new string[] { "force", "equals", "mass", "times", "acceleration" },
                            new string[] { "light", "travels", "in", "vacuum", "at", "speed" },
                            new string[] { "electron", "has", "negative", "charge" },
                            new string[] { "gravity", "pulls", "objects", "towards", "earth" }
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
                            Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
                        }

                        // Testing the RNN to see if it can recall physics sentences
                        Console.WriteLine("Testing trained RNN on physics sentences:");
                        foreach (var sentence in physicsSentences)
                        {
                            Console.Write("Input: ");
                            foreach (var word in sentence)
                            {
                                Console.Write(word + " ");
                            }
                            Console.Write("\nPredicted: ");

                            List<double[]> sentenceEmbeddings = new List<double[]>();
                            foreach (var word in sentence)
                            {
                                sentenceEmbeddings.Add(glove.GetEmbedding(word));
                            }

                            for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                            {
                                double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                                string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                                Console.Write(predictedWord + " ");
                            }
                            Console.WriteLine("\n");
                        }

                        Console.ReadKey();
                    }
                }

                Code:
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


                Code:
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
                */

            }
            #endregion 

            Console.WriteLine();

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

                codeProcessing.ProcessCodeBlock(sourceCodeIteration5, sourceCodeIteration5.SourceCodes[1].Code, sourceCodeIteration5.SourceCodes[2].Code, sourceCodeIteration5.SourceCodes[0].Code);

                projectManager.Print(project9);

                /*

                Project: Recurrent Neural Networks
                conversation #1
                iteration #1
                Epoch 1/1000, Loss: 0,005197035768004216
                Epoch 2/1000, Loss: 0,005189737125899973
                Epoch 3/1000, Loss: 0,005130898172389646
                Epoch 4/1000, Loss: 0,004796283885091109
                Epoch 5/1000, Loss: 0,004300924308824893
                Epoch 6/1000, Loss: 0,004200182773337406
                Epoch 7/1000, Loss: 0,004180951559614272
                Epoch 8/1000, Loss: 0,0041630349208414495
                Epoch 9/1000, Loss: 0,004146931933508101
                Epoch 10/1000, Loss: 0,004132422153373596
                Epoch 11/1000, Loss: 0,004119156139617861
                Epoch 12/1000, Loss: 0,004106876822721405
                Epoch 13/1000, Loss: 0,004095401848618444
                Epoch 14/1000, Loss: 0,004084599046777138
                Epoch 15/1000, Loss: 0,004074369307434687
                Epoch 16/1000, Loss: 0,004064634717367431
                Epoch 17/1000, Loss: 0,004055330060857154
                Epoch 18/1000, Loss: 0,0040463965064708105
                Epoch 19/1000, Loss: 0,0040377767255439356
                Epoch 20/1000, Loss: 0,00402941093916465
                Epoch 21/1000, Loss: 0,004021233543901571
                Epoch 22/1000, Loss: 0,004013170063997037
                Epoch 23/1000, Loss: 0,004005134244727841
                Epoch 24/1000, Loss: 0,003997025156361673
                Epoch 25/1000, Loss: 0,003988724236081575
                Epoch 26/1000, Loss: 0,003980092271764576
                Epoch 27/1000, Loss: 0,003970966443202231
                Epoch 28/1000, Loss: 0,003961157700763372
                Epoch 29/1000, Loss: 0,003950448993508257
                Epoch 30/1000, Loss: 0,003938595161708974
                Epoch 31/1000, Loss: 0,003925325658156588
                Epoch 32/1000, Loss: 0,003910351583499567
                Epoch 33/1000, Loss: 0,0038933786656574374
                Epoch 34/1000, Loss: 0,0038741275573586443
                Epoch 35/1000, Loss: 0,0038523618993304256
                Epoch 36/1000, Loss: 0,0038279227670766696
                Epoch 37/1000, Loss: 0,0038007653318845916
                Epoch 38/1000, Loss: 0,003770990170632962
                Epoch 39/1000, Loss: 0,003738858723608255
                Epoch 40/1000, Loss: 0,0037047819046246004
                Epoch 41/1000, Loss: 0,003669275160249976
                Epoch 42/1000, Loss: 0,003632883285813631
                Epoch 43/1000, Loss: 0,0035960913002986636
                Epoch 44/1000, Loss: 0,0035592469987969617
                Epoch 45/1000, Loss: 0,0035225195708798148
                Epoch 46/1000, Loss: 0,003485905620359191
                Epoch 47/1000, Loss: 0,003449275687596995
                Epoch 48/1000, Loss: 0,003412441048224745
                Epoch 49/1000, Loss: 0,0033752181700158835
                Epoch 50/1000, Loss: 0,00333747524747502
                Epoch 51/1000, Loss: 0,0032991555708851497
                Epoch 52/1000, Loss: 0,003260280596436687
                Epoch 53/1000, Loss: 0,0032209393355473828
                Epoch 54/1000, Loss: 0,0031812708113333928
                Epoch 55/1000, Loss: 0,0031414446052125205
                Epoch 56/1000, Loss: 0,0031016424205242577
                Epoch 57/1000, Loss: 0,0030620419424972787
                Epoch 58/1000, Loss: 0,003022803355125493
                Epoch 59/1000, Loss: 0,0029840586279967405
                Epoch 60/1000, Loss: 0,002945903851983238
                Epoch 61/1000, Loss: 0,0029083951354811166
                Epoch 62/1000, Loss: 0,0028715485687448444
                Epoch 63/1000, Loss: 0,002835344387995625
                Epoch 64/1000, Loss: 0,0027997348121454055
                Epoch 65/1000, Loss: 0,0027646543374692956
                Epoch 66/1000, Loss: 0,0027300308381308343
                Epoch 67/1000, Loss: 0,002695795795424067
                Epoch 68/1000, Loss: 0,0026618923487625665
                Epoch 69/1000, Loss: 0,0026282804658675552
                Epoch 70/1000, Loss: 0,002594939158415963
                Epoch 71/1000, Loss: 0,0025618661555779155
                Epoch 72/1000, Loss: 0,0025290757137567802
                Epoch 73/1000, Loss: 0,0024965952943562693
                Epoch 74/1000, Loss: 0,0024644617427760016
                Epoch 75/1000, Loss: 0,00243271742682404
                Epoch 76/1000, Loss: 0,0024014066079356194
                Epoch 77/1000, Loss: 0,0023705721714234783
                Epoch 78/1000, Loss: 0,0023402527593469656
                Epoch 79/1000, Loss: 0,002310480338267215
                Epoch 80/1000, Loss: 0,0022812782804429495
                Epoch 81/1000, Loss: 0,0022526601079579135
                Epoch 82/1000, Loss: 0,0022246290999203863
                Epoch 83/1000, Loss: 0,002197178950906606
                Epoch 84/1000, Loss: 0,0021702955719826835
                Epoch 85/1000, Loss: 0,0021439599537596042
                Epoch 86/1000, Loss: 0,002118151805363006
                Epoch 87/1000, Loss: 0,002092853502797364
                Epoch 88/1000, Loss: 0,00206805378000311
                Epoch 89/1000, Loss: 0,0020437506080598676
                Epoch 90/1000, Loss: 0,002019952832298081
                Epoch 91/1000, Loss: 0,0019966803436166566
                Epoch 92/1000, Loss: 0,001973962800800419
                Epoch 93/1000, Loss: 0,0019518371432477061
                Epoch 94/1000, Loss: 0,0019303442959520956
                Epoch 95/1000, Loss: 0,0019095255465401823
                Epoch 96/1000, Loss: 0,001889419064224691
                Epoch 97/1000, Loss: 0,0018700569477700592
                Epoch 98/1000, Loss: 0,0018514630612842052
                Epoch 99/1000, Loss: 0,0018336517741154943
                Epoch 100/1000, Loss: 0,0018166275917324746
                Epoch 101/1000, Loss: 0,0018003855663137926
                Epoch 102/1000, Loss: 0,001784912315952221
                Epoch 103/1000, Loss: 0,001770187457516574
                Epoch 104/1000, Loss: 0,0017561852621802393
                Epoch 105/1000, Loss: 0,0017428763645041933
                Epoch 106/1000, Loss: 0,0017302293870524216
                Epoch 107/1000, Loss: 0,0017182123767093254
                Epoch 108/1000, Loss: 0,001706793982628978
                Epoch 109/1000, Loss: 0,0016959443373148167
                Epoch 110/1000, Loss: 0,0016856356307802824
                Epoch 111/1000, Loss: 0,001675842392335419
                Epoch 112/1000, Loss: 0,0016665415144885049
                Epoch 113/1000, Loss: 0,0016577120679829509
                Epoch 114/1000, Loss: 0,0016493349655990273
                Epoch 115/1000, Loss: 0,001641392534981583
                Epoch 116/1000, Loss: 0,001633868057893698
                Epoch 117/1000, Loss: 0,0016267453259231634
                Epoch 118/1000, Loss: 0,0016200082521166683
                Epoch 119/1000, Loss: 0,0016136405657745357
                Epoch 120/1000, Loss: 0,0016076256051538271
                Epoch 121/1000, Loss: 0,0016019462113385271
                Epoch 122/1000, Loss: 0,0015965847169616368
                Epoch 123/1000, Loss: 0,001591523016353139
                Epoch 124/1000, Loss: 0,0015867426992225205
                Epoch 125/1000, Loss: 0,0015822252280331805
                Epoch 126/1000, Loss: 0,0015779521394244174
                Epoch 127/1000, Loss: 0,0015739052518806585
                Epoch 128/1000, Loss: 0,0015700668647837628
                Epoch 129/1000, Loss: 0,0015664199374836544
                Epoch 130/1000, Loss: 0,0015629482406341513
                Epoch 131/1000, Loss: 0,001559636475420063
                Epoch 132/1000, Loss: 0,0015564703592186508
                Epoch 133/1000, Loss: 0,0015534366785705475
                Epoch 134/1000, Loss: 0,0015505233120476658
                Epoch 135/1000, Loss: 0,0015477192267298353
                Epoch 136/1000, Loss: 0,0015450144526116026
                Epoch 137/1000, Loss: 0,001542400039451748
                Epoch 138/1000, Loss: 0,0015398680004525594
                Epoch 139/1000, Loss: 0,0015374112468104965
                Epoch 140/1000, Loss: 0,001535023516698925
                Epoch 141/1000, Loss: 0,0015326993016961184
                Epoch 142/1000, Loss: 0,0015304337731100951
                Epoch 143/1000, Loss: 0,0015282227101136446
                Epoch 144/1000, Loss: 0,0015260624311122279
                Epoch 145/1000, Loss: 0,0015239497293380055
                Epoch 146/1000, Loss: 0,001521881813300594
                Epoch 147/1000, Loss: 0,001519856252428971
                Epoch 148/1000, Loss: 0,0015178709280051956
                Epoch 149/1000, Loss: 0,0015159239893127298
                Epoch 150/1000, Loss: 0,001514013814792759
                Epoch 151/1000, Loss: 0,0015121389779129799
                Epoch 152/1000, Loss: 0,0015102982173975182
                Epoch 153/1000, Loss: 0,0015084904114368376
                Epoch 154/1000, Loss: 0,0015067145554867806
                Epoch 155/1000, Loss: 0,0015049697432709486
                Epoch 156/1000, Loss: 0,0015032551506161983
                Epoch 157/1000, Loss: 0,001501570021773622
                Epoch 158/1000, Loss: 0,0014999136579041945
                Epoch 159/1000, Loss: 0,0014982854074372762
                Epoch 160/1000, Loss: 0,0014966846580396628
                Epoch 161/1000, Loss: 0,0014951108299618653
                Epoch 162/1000, Loss: 0,0014935633705559005
                Epoch 163/1000, Loss: 0,0014920417497846154
                Epoch 164/1000, Loss: 0,0014905454565662273
                Epoch 165/1000, Loss: 0,0014890739958190592
                Epoch 166/1000, Loss: 0,001487626886090583
                Epoch 167/1000, Loss: 0,0014862036576717356
                Epoch 168/1000, Loss: 0,0014848038511122825
                Epoch 169/1000, Loss: 0,0014834270160659058
                Epoch 170/1000, Loss: 0,001482072710404837
                Epoch 171/1000, Loss: 0,0014807404995534493
                Epoch 172/1000, Loss: 0,0014794299559984319
                Epoch 173/1000, Loss: 0,0014781406589401609
                Epoch 174/1000, Loss: 0,0014768721940558252
                Epoch 175/1000, Loss: 0,0014756241533498862
                Epoch 176/1000, Loss: 0,0014743961350716765
                Epoch 177/1000, Loss: 0,0014731877436835318
                Epoch 178/1000, Loss: 0,0014719985898658215
                Epoch 179/1000, Loss: 0,0014708282905477707
                Epoch 180/1000, Loss: 0,0014696764689550571
                Epoch 181/1000, Loss: 0,0014685427546669272
                Epoch 182/1000, Loss: 0,0014674267836770412
                Epoch 183/1000, Loss: 0,0014663281984534798
                Epoch 184/1000, Loss: 0,0014652466479943632
                Epoch 185/1000, Loss: 0,0014641817878763787
                Epoch 186/1000, Loss: 0,0014631332802942146
                Epoch 187/1000, Loss: 0,0014621007940894934
                Epoch 188/1000, Loss: 0,0014610840047682484
                Epoch 189/1000, Loss: 0,0014600825945064208
                Epoch 190/1000, Loss: 0,0014590962521431451
                Epoch 191/1000, Loss: 0,001458124673161871
                Epoch 192/1000, Loss: 0,001457167559659576
                Epoch 193/1000, Loss: 0,0014562246203044824
                Epoch 194/1000, Loss: 0,0014552955702828402
                Epoch 195/1000, Loss: 0,0014543801312354165
                Epoch 196/1000, Loss: 0,0014534780311844168
                Epoch 197/1000, Loss: 0,0014525890044516068
                Epoch 198/1000, Loss: 0,0014517127915684397
                Epoch 199/1000, Loss: 0,0014508491391790044
                Epoch 200/1000, Loss: 0,0014499977999366078
                Epoch 201/1000, Loss: 0,0014491585323948052
                Epoch 202/1000, Loss: 0,001448331100893662
                Epoch 203/1000, Loss: 0,0014475152754420118
                Epoch 204/1000, Loss: 0,0014467108315964368
                Epoch 205/1000, Loss: 0,0014459175503376707
                Epoch 206/1000, Loss: 0,0014451352179450615
                Epoch 207/1000, Loss: 0,00144436362586972
                Epoch 208/1000, Loss: 0,0014436025706069096
                Epoch 209/1000, Loss: 0,001442851853568196
                Epoch 210/1000, Loss: 0,0014421112809538397
                Epoch 211/1000, Loss: 0,0014413806636258517
                Epoch 212/1000, Loss: 0,0014406598169821054
                Epoch 213/1000, Loss: 0,0014399485608318475
                Epoch 214/1000, Loss: 0,0014392467192729072
                Epoch 215/1000, Loss: 0,0014385541205708722
                Epoch 216/1000, Loss: 0,0014378705970404598
                Epoch 217/1000, Loss: 0,0014371959849292628
                Epoch 218/1000, Loss: 0,0014365301243040559
                Epoch 219/1000, Loss: 0,001435872858939759
                Epoch 220/1000, Loss: 0,001435224036211192
                Epoch 221/1000, Loss: 0,0014345835069876764
                Epoch 222/1000, Loss: 0,0014339511255305436
                Epoch 223/1000, Loss: 0,0014333267493935873
                Epoch 224/1000, Loss: 0,0014327102393264708
                Epoch 225/1000, Loss: 0,0014321014591810804
                Epoch 226/1000, Loss: 0,0014315002758208252
                Epoch 227/1000, Loss: 0,00143090655903283
                Epoch 228/1000, Loss: 0,0014303201814430072
                Epoch 229/1000, Loss: 0,0014297410184339302
                Epoch 230/1000, Loss: 0,0014291689480654761
                Epoch 231/1000, Loss: 0,0014286038509981494
                Epoch 232/1000, Loss: 0,0014280456104190404
                Epoch 233/1000, Loss: 0,0014274941119703224
                Epoch 234/1000, Loss: 0,001426949243680226
                Epoch 235/1000, Loss: 0,00142641089589641
                Epoch 236/1000, Loss: 0,0014258789612216333
                Epoch 237/1000, Loss: 0,001425353334451674
                Epoch 238/1000, Loss: 0,0014248339125153844
                Epoch 239/1000, Loss: 0,0014243205944168308
                Epoch 240/1000, Loss: 0,0014238132811794121
                Epoch 241/1000, Loss: 0,0014233118757918995
                Epoch 242/1000, Loss: 0,0014228162831563123
                Epoch 243/1000, Loss: 0,0014223264100375569
                Epoch 244/1000, Loss: 0,0014218421650147573
                Epoch 245/1000, Loss: 0,0014213634584342094
                Epoch 246/1000, Loss: 0,001420890202363891
                Epoch 247/1000, Loss: 0,0014204223105494665
                Epoch 248/1000, Loss: 0,0014199596983717178
                Epoch 249/1000, Loss: 0,0014195022828053549
                Epoch 250/1000, Loss: 0,001419049982379134
                Epoch 251/1000, Loss: 0,00141860271713725
                Epoch 252/1000, Loss: 0,0014181604086019289
                Epoch 253/1000, Loss: 0,0014177229797372026
                Epoch 254/1000, Loss: 0,0014172903549137849
                Epoch 255/1000, Loss: 0,001416862459875035
                Epoch 256/1000, Loss: 0,0014164392217039575
                Epoch 257/1000, Loss: 0,001416020568791191
                Epoch 258/1000, Loss: 0,0014156064308039672
                Epoch 259/1000, Loss: 0,0014151967386559865
                Epoch 260/1000, Loss: 0,0014147914244781953
                Epoch 261/1000, Loss: 0,0014143904215904192
                Epoch 262/1000, Loss: 0,0014139936644738365
                Epoch 263/1000, Loss: 0,0014136010887442455
                Epoch 264/1000, Loss: 0,0014132126311261254
                Epoch 265/1000, Loss: 0,001412828229427437
                Epoch 266/1000, Loss: 0,0014124478225151674
                Epoch 267/1000, Loss: 0,001412071350291569
                Epoch 268/1000, Loss: 0,0014116987536710948
                Epoch 269/1000, Loss: 0,0014113299745579966
                Epoch 270/1000, Loss: 0,0014109649558245669
                Epoch 271/1000, Loss: 0,0014106036412900066
                Epoch 272/1000, Loss: 0,0014102459756999116
                Epoch 273/1000, Loss: 0,0014098919047063388
                Epoch 274/1000, Loss: 0,001409541374848452
                Epoch 275/1000, Loss: 0,0014091943335337324
                Epoch 276/1000, Loss: 0,00140885072901972
                Epoch 277/1000, Loss: 0,0014085105103962953
                Epoch 278/1000, Loss: 0,0014081736275684653
                Epoch 279/1000, Loss: 0,001407840031239654
                Epoch 280/1000, Loss: 0,0014075096728954745
                Epoch 281/1000, Loss: 0,0014071825047879774
                Epoch 282/1000, Loss: 0,0014068584799203493
                Epoch 283/1000, Loss: 0,0014065375520320674
                Epoch 284/1000, Loss: 0,0014062196755844746
                Epoch 285/1000, Loss: 0,001405904805746788
                Epoch 286/1000, Loss: 0,0014055928983825053
                Epoch 287/1000, Loss: 0,0014052839100362098
                Epoch 288/1000, Loss: 0,0014049777979207672
                Epoch 289/1000, Loss: 0,0014046745199048835
                Epoch 290/1000, Loss: 0,0014043740345010352
                Epoch 291/1000, Loss: 0,0014040763008537445
                Epoch 292/1000, Loss: 0,0014037812787281976
                Epoch 293/1000, Loss: 0,0014034889284991908
                Epoch 294/1000, Loss: 0,0014031992111403972
                Epoch 295/1000, Loss: 0,0014029120882139375
                Epoch 296/1000, Loss: 0,0014026275218602597
                Epoch 297/1000, Loss: 0,0014023454747882935
                Epoch 298/1000, Loss: 0,0014020659102658988
                Epoch 299/1000, Loss: 0,001401788792110572
                Epoch 300/1000, Loss: 0,0014015140846804193
                Epoch 301/1000, Loss: 0,0014012417528653802
                Epoch 302/1000, Loss: 0,001400971762078694
                Epoch 303/1000, Loss: 0,001400704078248598
                Epoch 304/1000, Loss: 0,0014004386678102537
                Epoch 305/1000, Loss: 0,0014001754976978903
                Epoch 306/1000, Loss: 0,0013999145353371543
                Epoch 307/1000, Loss: 0,001399655748637666
                Epoch 308/1000, Loss: 0,0013993991059857648
                Epoch 309/1000, Loss: 0,001399144576237443
                Epoch 310/1000, Loss: 0,0013988921287114583
                Epoch 311/1000, Loss: 0,0013986417331826183
                Epoch 312/1000, Loss: 0,0013983933598752337
                Epoch 313/1000, Loss: 0,0013981469794567233
                Epoch 314/1000, Loss: 0,0013979025630313762
                Epoch 315/1000, Loss: 0,0013976600821342623
                Epoch 316/1000, Loss: 0,0013974195087252759
                Epoch 317/1000, Loss: 0,0013971808151833217
                Epoch 318/1000, Loss: 0,0013969439743006231
                Epoch 319/1000, Loss: 0,0013967089592771573
                Epoch 320/1000, Loss: 0,00139647574371521
                Epoch 321/1000, Loss: 0,0013962443016140402
                Epoch 322/1000, Loss: 0,0013960146073646543
                Epoch 323/1000, Loss: 0,0013957866357446854
                Epoch 324/1000, Loss: 0,0013955603619133722
                Epoch 325/1000, Loss: 0,0013953357614066304
                Epoch 326/1000, Loss: 0,0013951128101322204
                Epoch 327/1000, Loss: 0,0013948914843649957
                Epoch 328/1000, Loss: 0,0013946717607422393
                Epoch 329/1000, Loss: 0,0013944536162590804
                Epoch 330/1000, Loss: 0,0013942370282639838
                Epoch 331/1000, Loss: 0,0013940219744543168
                Epoch 332/1000, Loss: 0,0013938084328719842
                Epoch 333/1000, Loss: 0,0013935963818991306
                Epoch 334/1000, Loss: 0,0013933858002539066
                Epoch 335/1000, Loss: 0,0013931766669862998
                Epoch 336/1000, Loss: 0,001392968961474021
                Epoch 337/1000, Loss: 0,0013927626634184487
                Epoch 338/1000, Loss: 0,0013925577528406312
                Epoch 339/1000, Loss: 0,0013923542100773363
                Epoch 340/1000, Loss: 0,0013921520157771541
                Epoch 341/1000, Loss: 0,0013919511508966472
                Epoch 342/1000, Loss: 0,0013917515966965497
                Epoch 343/1000, Loss: 0,0013915533347380095
                Epoch 344/1000, Loss: 0,0013913563468788688
                Epoch 345/1000, Loss: 0,0013911606152700005
                Epoch 346/1000, Loss: 0,001390966122351667
                Epoch 347/1000, Loss: 0,001390772850849933
                Epoch 348/1000, Loss: 0,0013905807837731056
                Epoch 349/1000, Loss: 0,0013903899044082184
                Epoch 350/1000, Loss: 0,0013902001963175463
                Epoch 351/1000, Loss: 0,0013900116433351558
                Epoch 352/1000, Loss: 0,0013898242295634915
                Epoch 353/1000, Loss: 0,001389637939369988
                Epoch 354/1000, Loss: 0,0013894527573837254
                Epoch 355/1000, Loss: 0,0013892686684921008
                Epoch 356/1000, Loss: 0,0013890856578375437
                Epoch 357/1000, Loss: 0,0013889037108142522
                Epoch 358/1000, Loss: 0,00138872281306496
                Epoch 359/1000, Loss: 0,0013885429504777351
                Epoch 360/1000, Loss: 0,001388364109182801
                Epoch 361/1000, Loss: 0,0013881862755493895
                Epoch 362/1000, Loss: 0,0013880094361826164
                Epoch 363/1000, Loss: 0,0013878335779203882
                Epoch 364/1000, Loss: 0,001387658687830331
                Epoch 365/1000, Loss: 0,001387484753206745
                Epoch 366/1000, Loss: 0,0013873117615675872
                Epoch 367/1000, Loss: 0,0013871397006514764
                Epoch 368/1000, Loss: 0,001386968558414724
                Epoch 369/1000, Loss: 0,001386798323028389
                Epoch 370/1000, Loss: 0,001386628982875358
                Epoch 371/1000, Loss: 0,001386460526547447
                Epoch 372/1000, Loss: 0,0013862929428425292
                Epoch 373/1000, Loss: 0,001386126220761687
                Epoch 374/1000, Loss: 0,0013859603495063838
                Epoch 375/1000, Loss: 0,001385795318475662
                Epoch 376/1000, Loss: 0,0013856311172633661
                Epoch 377/1000, Loss: 0,0013854677356553828
                Epoch 378/1000, Loss: 0,0013853051636269118
                Epoch 379/1000, Loss: 0,0013851433913397518
                Epoch 380/1000, Loss: 0,001384982409139614
                Epoch 381/1000, Loss: 0,0013848222075534588
                Epoch 382/1000, Loss: 0,0013846627772868525
                Epoch 383/1000, Loss: 0,0013845041092213468
                Epoch 384/1000, Loss: 0,0013843461944118836
                Epoch 385/1000, Loss: 0,0013841890240842186
                Epoch 386/1000, Loss: 0,0013840325896323697
                Epoch 387/1000, Loss: 0,0013838768826160883
                Epoch 388/1000, Loss: 0,0013837218947583489
                Epoch 389/1000, Loss: 0,001383567617942867
                Epoch 390/1000, Loss: 0,0013834140442116364
                Epoch 391/1000, Loss: 0,0013832611657624847
                Epoch 392/1000, Loss: 0,0013831089749466576
                Epoch 393/1000, Loss: 0,0013829574642664224
                Epoch 394/1000, Loss: 0,0013828066263726945
                Epoch 395/1000, Loss: 0,0013826564540626834
                Epoch 396/1000, Loss: 0,0013825069402775632
                Epoch 397/1000, Loss: 0,0013823580781001659
                Epoch 398/1000, Loss: 0,001382209860752694
                Epoch 399/1000, Loss: 0,0013820622815944577
                Epoch 400/1000, Loss: 0,0013819153341196326
                Epoch 401/1000, Loss: 0,0013817690119550395
                Epoch 402/1000, Loss: 0,0013816233088579457
                Epoch 403/1000, Loss: 0,0013814782187138888
                Epoch 404/1000, Loss: 0,0013813337355345263
                Epoch 405/1000, Loss: 0,0013811898534554952
                Epoch 406/1000, Loss: 0,0013810465667343091
                Epoch 407/1000, Loss: 0,0013809038697482634
                Epoch 408/1000, Loss: 0,0013807617569923704
                Epoch 409/1000, Loss: 0,0013806202230773148
                Epoch 410/1000, Loss: 0,0013804792627274257
                Epoch 411/1000, Loss: 0,0013803388707786788
                Epoch 412/1000, Loss: 0,0013801990421767108
                Epoch 413/1000, Loss: 0,0013800597719748637
                Epoch 414/1000, Loss: 0,0013799210553322428
                Epoch 415/1000, Loss: 0,0013797828875118045
                Epoch 416/1000, Loss: 0,0013796452638784563
                Epoch 417/1000, Loss: 0,0013795081798971857
                Epoch 418/1000, Loss: 0,0013793716311312051
                Epoch 419/1000, Loss: 0,0013792356132401214
                Epoch 420/1000, Loss: 0,0013791001219781245
                Epoch 421/1000, Loss: 0,001378965153192199
                Epoch 422/1000, Loss: 0,0013788307028203507
                Epoch 423/1000, Loss: 0,001378696766889866
                Epoch 424/1000, Loss: 0,0013785633415155777
                Epoch 425/1000, Loss: 0,0013784304228981622
                Epoch 426/1000, Loss: 0,0013782980073224547
                Epoch 427/1000, Loss: 0,00137816609115578
                Epoch 428/1000, Loss: 0,0013780346708463114
                Epoch 429/1000, Loss: 0,0013779037429214461
                Epoch 430/1000, Loss: 0,0013777733039861983
                Epoch 431/1000, Loss: 0,0013776433507216206
                Epoch 432/1000, Loss: 0,0013775138798832354
                Epoch 433/1000, Loss: 0,0013773848882994942
                Epoch 434/1000, Loss: 0,0013772563728702547
                Epoch 435/1000, Loss: 0,001377128330565276
                Epoch 436/1000, Loss: 0,0013770007584227354
                Epoch 437/1000, Loss: 0,0013768736535477648
                Epoch 438/1000, Loss: 0,0013767470131110058
                Epoch 439/1000, Loss: 0,0013766208343471854
                Epoch 440/1000, Loss: 0,0013764951145537106
                Epoch 441/1000, Loss: 0,0013763698510892825
                Epoch 442/1000, Loss: 0,0013762450413725286
                Epoch 443/1000, Loss: 0,0013761206828806565
                Epoch 444/1000, Loss: 0,001375996773148125
                Epoch 445/1000, Loss: 0,001375873309765333
                Epoch 446/1000, Loss: 0,0013757502903773305
                Epoch 447/1000, Loss: 0,0013756277126825444
                Epoch 448/1000, Loss: 0,001375505574431527
                Epoch 449/1000, Loss: 0,0013753838734257187
                Epoch 450/1000, Loss: 0,0013752626075162317
                Epoch 451/1000, Loss: 0,0013751417746026536
                Epoch 452/1000, Loss: 0,0013750213726318624
                Epoch 453/1000, Loss: 0,0013749013995968686
                Epoch 454/1000, Loss: 0,001374781853535668
                Epoch 455/1000, Loss: 0,0013746627325301143
                Epoch 456/1000, Loss: 0,0013745440347048116
                Epoch 457/1000, Loss: 0,001374425758226023
                Epoch 458/1000, Loss: 0,001374307901300593
                Epoch 459/1000, Loss: 0,0013741904621748942
                Epoch 460/1000, Loss: 0,0013740734391337844
                Epoch 461/1000, Loss: 0,0013739568304995864
                Epoch 462/1000, Loss: 0,0013738406346310791
                Epoch 463/1000, Loss: 0,0013737248499225093
                Epoch 464/1000, Loss: 0,0013736094748026204
                Epoch 465/1000, Loss: 0,0013734945077336935
                Epoch 466/1000, Loss: 0,0013733799472106093
                Epoch 467/1000, Loss: 0,001373265791759924
                Epoch 468/1000, Loss: 0,0013731520399389602
                Epoch 469/1000, Loss: 0,0013730386903349188
                Epoch 470/1000, Loss: 0,001372925741563999
                Epoch 471/1000, Loss: 0,0013728131922705398
                Epoch 472/1000, Loss: 0,001372701041126177
                Epoch 473/1000, Loss: 0,0013725892868290104
                Epoch 474/1000, Loss: 0,0013724779281027934
                Epoch 475/1000, Loss: 0,0013723669636961301
                Epoch 476/1000, Loss: 0,001372256392381694
                Epoch 477/1000, Loss: 0,0013721462129554606
                Epoch 478/1000, Loss: 0,0013720364242359477
                Epoch 479/1000, Loss: 0,0013719270250634793
                Epoch 480/1000, Loss: 0,0013718180142994589
                Epoch 481/1000, Loss: 0,0013717093908256585
                Epoch 482/1000, Loss: 0,001371601153543522
                Epoch 483/1000, Loss: 0,001371493301373479
                Epoch 484/1000, Loss: 0,001371385833254279
                Epoch 485/1000, Loss: 0,001371278748142335
                Epoch 486/1000, Loss: 0,0013711720450110794
                Epoch 487/1000, Loss: 0,0013710657228503357
                Epoch 488/1000, Loss: 0,0013709597806657037
                Epoch 489/1000, Loss: 0,0013708542174779585
                Epoch 490/1000, Loss: 0,001370749032322456
                Epoch 491/1000, Loss: 0,0013706442242485608
                Epoch 492/1000, Loss: 0,0013705397923190797
                Epoch 493/1000, Loss: 0,0013704357356097115
                Epoch 494/1000, Loss: 0,0013703320532085057
                Epoch 495/1000, Loss: 0,001370228744215337
                Epoch 496/1000, Loss: 0,0013701258077413894
                Epoch 497/1000, Loss: 0,0013700232429086534
                Epoch 498/1000, Loss: 0,001369921048849434
                Epoch 499/1000, Loss: 0,0013698192247058702
                Epoch 500/1000, Loss: 0,0013697177696294703
                Epoch 501/1000, Loss: 0,0013696166827806507
                Epoch 502/1000, Loss: 0,0013695159633282906
                Epoch 503/1000, Loss: 0,0013694156104492974
                Epoch 504/1000, Loss: 0,0013693156233281858
                Epoch 505/1000, Loss: 0,0013692160011566591
                Epoch 506/1000, Loss: 0,0013691167431332117
                Epoch 507/1000, Loss: 0,0013690178484627312
                Epoch 508/1000, Loss: 0,0013689193163561213
                Epoch 509/1000, Loss: 0,0013688211460299287
                Epoch 510/1000, Loss: 0,0013687233367059774
                Epoch 511/1000, Loss: 0,0013686258876110234
                Epoch 512/1000, Loss: 0,001368528797976405
                Epoch 513/1000, Loss: 0,0013684320670377156
                Epoch 514/1000, Loss: 0,0013683356940344786
                Epoch 515/1000, Loss: 0,0013682396782098304
                Epoch 516/1000, Loss: 0,0013681440188102198
                Epoch 517/1000, Loss: 0,0013680487150851098
                Epoch 518/1000, Loss: 0,0013679537662866891
                Epoch 519/1000, Loss: 0,001367859171669597
                Epoch 520/1000, Loss: 0,0013677649304906492
                Epoch 521/1000, Loss: 0,0013676710420085802
                Epoch 522/1000, Loss: 0,0013675775054837879
                Epoch 523/1000, Loss: 0,0013674843201780882
                Epoch 524/1000, Loss: 0,0013673914853544805
                Epoch 525/1000, Loss: 0,0013672990002769143
                Epoch 526/1000, Loss: 0,0013672068642100706
                Epoch 527/1000, Loss: 0,0013671150764191496
                Epoch 528/1000, Loss: 0,001367023636169659
                Epoch 529/1000, Loss: 0,001366932542727219
                Epoch 530/1000, Loss: 0,001366841795357372
                Epoch 531/1000, Loss: 0,0013667513933253894
                Epoch 532/1000, Loss: 0,0013666613358961051
                Epoch 533/1000, Loss: 0,0013665716223337366
                Epoch 534/1000, Loss: 0,001366482251901721
                Epoch 535/1000, Loss: 0,001366393223862562
                Epoch 536/1000, Loss: 0,0013663045374776735
                Epoch 537/1000, Loss: 0,0013662161920072362
                Epoch 538/1000, Loss: 0,0013661281867100603
                Epoch 539/1000, Loss: 0,001366040520843449
                Epoch 540/1000, Loss: 0,0013659531936630743
                Epoch 541/1000, Loss: 0,001365866204422856
                Epoch 542/1000, Loss: 0,0013657795523748446
                Epoch 543/1000, Loss: 0,0013656932367691111
                Epoch 544/1000, Loss: 0,0013656072568536464
                Epoch 545/1000, Loss: 0,001365521611874256
                Epoch 546/1000, Loss: 0,0013654363010744716
                Epoch 547/1000, Loss: 0,0013653513236954588
                Epoch 548/1000, Loss: 0,0013652666789759364
                Epoch 549/1000, Loss: 0,0013651823661520925
                Epoch 550/1000, Loss: 0,0013650983844575163
                Epoch 551/1000, Loss: 0,0013650147331231242
                Epoch 552/1000, Loss: 0,0013649314113770975
                Epoch 553/1000, Loss: 0,0013648484184448193
                Epoch 554/1000, Loss: 0,001364765753548821
                Epoch 555/1000, Loss: 0,0013646834159087291
                Epoch 556/1000, Loss: 0,0013646014047412152
                Epoch 557/1000, Loss: 0,0013645197192599574
                Epoch 558/1000, Loss: 0,0013644383586755948
                Epoch 559/1000, Loss: 0,0013643573221956964
                Epoch 560/1000, Loss: 0,001364276609024723
                Epoch 561/1000, Loss: 0,0013641962183640055
                Epoch 562/1000, Loss: 0,0013641161494117129
                Epoch 563/1000, Loss: 0,0013640364013628357
                Epoch 564/1000, Loss: 0,0013639569734091644
                Epoch 565/1000, Loss: 0,0013638778647392768
                Epoch 566/1000, Loss: 0,0013637990745385224
                Epoch 567/1000, Loss: 0,0013637206019890171
                Epoch 568/1000, Loss: 0,0013636424462696367
                Epoch 569/1000, Loss: 0,0013635646065560105
                Epoch 570/1000, Loss: 0,001363487082020527
                Epoch 571/1000, Loss: 0,0013634098718323289
                Epoch 572/1000, Loss: 0,0013633329751573267
                Epoch 573/1000, Loss: 0,0013632563911581983
                Epoch 574/1000, Loss: 0,0013631801189944047
                Epoch 575/1000, Loss: 0,0013631041578222012
                Epoch 576/1000, Loss: 0,0013630285067946512
                Epoch 577/1000, Loss: 0,0013629531650616437
                Epoch 578/1000, Loss: 0,0013628781317699134
                Epoch 579/1000, Loss: 0,0013628034060630618
                Epoch 580/1000, Loss: 0,0013627289870815798
                Epoch 581/1000, Loss: 0,0013626548739628748
                Epoch 582/1000, Loss: 0,0013625810658412958
                Epoch 583/1000, Loss: 0,0013625075618481654
                Epoch 584/1000, Loss: 0,0013624343611118067
                Epoch 585/1000, Loss: 0,00136236146275758
                Epoch 586/1000, Loss: 0,001362288865907914
                Epoch 587/1000, Loss: 0,0013622165696823426
                Epoch 588/1000, Loss: 0,0013621445731975413
                Epoch 589/1000, Loss: 0,001362072875567368
                Epoch 590/1000, Loss: 0,0013620014759028979
                Epoch 591/1000, Loss: 0,0013619303733124729
                Epoch 592/1000, Loss: 0,001361859566901736
                Epoch 593/1000, Loss: 0,001361789055773679
                Epoch 594/1000, Loss: 0,0013617188390286884
                Epoch 595/1000, Loss: 0,001361648915764587
                Epoch 596/1000, Loss: 0,001361579285076685
                Epoch 597/1000, Loss: 0,0013615099460578262
                Epoch 598/1000, Loss: 0,0013614408977984372
                Epoch 599/1000, Loss: 0,0013613721393865763
                Epoch 600/1000, Loss: 0,0013613036699079847
                Epoch 601/1000, Loss: 0,0013612354884461387
                Epoch 602/1000, Loss: 0,0013611675940822985
                Epoch 603/1000, Loss: 0,0013610999858955659
                Epoch 604/1000, Loss: 0,0013610326629629333
                Epoch 605/1000, Loss: 0,0013609656243593392
                Epoch 606/1000, Loss: 0,0013608988691577234
                Epoch 607/1000, Loss: 0,0013608323964290812
                Epoch 608/1000, Loss: 0,0013607662052425187
                Epoch 609/1000, Loss: 0,0013607002946653087
                Epoch 610/1000, Loss: 0,0013606346637629497
                Epoch 611/1000, Loss: 0,0013605693115992172
                Epoch 612/1000, Loss: 0,0013605042372362246
                Epoch 613/1000, Loss: 0,0013604394397344812
                Epoch 614/1000, Loss: 0,001360374918152946
                Epoch 615/1000, Loss: 0,0013603106715490878
                Epoch 616/1000, Loss: 0,0013602466989789434
                Epoch 617/1000, Loss: 0,001360182999497175
                Epoch 618/1000, Loss: 0,0013601195721571284
                Epoch 619/1000, Loss: 0,0013600564160108906
                Epoch 620/1000, Loss: 0,0013599935301093493
                Epoch 621/1000, Loss: 0,0013599309135022503
                Epoch 622/1000, Loss: 0,0013598685652382563
                Epoch 623/1000, Loss: 0,0013598064843650063
                Epoch 624/1000, Loss: 0,0013597446699291707
                Epoch 625/1000, Loss: 0,001359683120976513
                Epoch 626/1000, Loss: 0,0013596218365519454
                Epoch 627/1000, Loss: 0,0013595608156995896
                Epoch 628/1000, Loss: 0,0013595000574628299
                Epoch 629/1000, Loss: 0,0013594395608843774
                Epoch 630/1000, Loss: 0,0013593793250063207
                Epoch 631/1000, Loss: 0,0013593193488701872
                Epoch 632/1000, Loss: 0,0013592596315170006
                Epoch 633/1000, Loss: 0,0013592001719873342
                Epoch 634/1000, Loss: 0,0013591409693213719
                Epoch 635/1000, Loss: 0,0013590820225589596
                Epoch 636/1000, Loss: 0,0013590233307396653
                Epoch 637/1000, Loss: 0,0013589648929028327
                Epoch 638/1000, Loss: 0,0013589067080876356
                Epoch 639/1000, Loss: 0,0013588487753331366
                Epoch 640/1000, Loss: 0,0013587910936783353
                Epoch 641/1000, Loss: 0,0013587336621622291
                Epoch 642/1000, Loss: 0,0013586764798238639
                Epoch 643/1000, Loss: 0,001358619545702386
                Epoch 644/1000, Loss: 0,0013585628588370985
                Epoch 645/1000, Loss: 0,0013585064182675113
                Epoch 646/1000, Loss: 0,0013584502230333936
                Epoch 647/1000, Loss: 0,0013583942721748273
                Epoch 648/1000, Loss: 0,001358338564732255
                Epoch 649/1000, Loss: 0,001358283099746534
                Epoch 650/1000, Loss: 0,0013582278762589836
                Epoch 651/1000, Loss: 0,0013581728933114378
                Epoch 652/1000, Loss: 0,001358118149946293
                Epoch 653/1000, Loss: 0,001358063645206554
                Epoch 654/1000, Loss: 0,00135800937813589
                Epoch 655/1000, Loss: 0,0013579553477786723
                Epoch 656/1000, Loss: 0,0013579015531800292
                Epoch 657/1000, Loss: 0,0013578479933858889
                Epoch 658/1000, Loss: 0,0013577946674430284
                Epoch 659/1000, Loss: 0,0013577415743991155
                Epoch 660/1000, Loss: 0,0013576887133027557
                Epoch 661/1000, Loss: 0,0013576360832035386
                Epoch 662/1000, Loss: 0,0013575836831520769
                Epoch 663/1000, Loss: 0,001357531512200055
                Epoch 664/1000, Loss: 0,001357479569400269
                Epoch 665/1000, Loss: 0,0013574278538066697
                Epoch 666/1000, Loss: 0,0013573763644744035
                Epoch 667/1000, Loss: 0,0013573251004598541
                Epoch 668/1000, Loss: 0,001357274060820683
                Epoch 669/1000, Loss: 0,00135722324461587
                Epoch 670/1000, Loss: 0,0013571726509057518
                Epoch 671/1000, Loss: 0,0013571222787520602
                Epoch 672/1000, Loss: 0,001357072127217962
                Epoch 673/1000, Loss: 0,0013570221953680957
                Epoch 674/1000, Loss: 0,0013569724822686086
                Epoch 675/1000, Loss: 0,0013569229869871938
                Epoch 676/1000, Loss: 0,0013568737085931236
                Epoch 677/1000, Loss: 0,00135682464615729
                Epoch 678/1000, Loss: 0,0013567757987522347
                Epoch 679/1000, Loss: 0,001356727165452184
                Epoch 680/1000, Loss: 0,0013566787453330852
                Epoch 681/1000, Loss: 0,0013566305374726373
                Epoch 682/1000, Loss: 0,0013565825409503226
                Epoch 683/1000, Loss: 0,0013565347548474423
                Epoch 684/1000, Loss: 0,0013564871782471438
                Epoch 685/1000, Loss: 0,0013564398102344534
                Epoch 686/1000, Loss: 0,0013563926498963061
                Epoch 687/1000, Loss: 0,0013563456963215756
                Epoch 688/1000, Loss: 0,0013562989486011036
                Epoch 689/1000, Loss: 0,0013562524058277274
                Epoch 690/1000, Loss: 0,0013562060670963085
                Epoch 691/1000, Loss: 0,0013561599315037585
                Epoch 692/1000, Loss: 0,0013561139981490694
                Epoch 693/1000, Loss: 0,001356068266133336
                Epoch 694/1000, Loss: 0,0013560227345597842
                Epoch 695/1000, Loss: 0,001355977402533797
                Epoch 696/1000, Loss: 0,0013559322691629346
                Epoch 697/1000, Loss: 0,0013558873335569634
                Epoch 698/1000, Loss: 0,0013558425948278785
                Epoch 699/1000, Loss: 0,001355798052089926
                Epoch 700/1000, Loss: 0,001355753704459625
                Epoch 701/1000, Loss: 0,0013557095510557903
                Epoch 702/1000, Loss: 0,001355665590999556
                Epoch 703/1000, Loss: 0,001355621823414394
                Epoch 704/1000, Loss: 0,0013555782474261348
                Epoch 705/1000, Loss: 0,001355534862162988
                Epoch 706/1000, Loss: 0,0013554916667555636
                Epoch 707/1000, Loss: 0,0013554486603368885
                Epoch 708/1000, Loss: 0,0013554058420424263
                Epoch 709/1000, Loss: 0,0013553632110100947
                Epoch 710/1000, Loss: 0,0013553207663802853
                Epoch 711/1000, Loss: 0,0013552785072958773
                Epoch 712/1000, Loss: 0,0013552364329022589
                Epoch 713/1000, Loss: 0,0013551945423473381
                Epoch 714/1000, Loss: 0,001355152834781563
                Epoch 715/1000, Loss: 0,0013551113093579347
                Epoch 716/1000, Loss: 0,001355069965232023
                Epoch 717/1000, Loss: 0,0013550288015619807
                Epoch 718/1000, Loss: 0,0013549878175085587
                Epoch 719/1000, Loss: 0,0013549470122351185
                Epoch 720/1000, Loss: 0,0013549063849076445
                Epoch 721/1000, Loss: 0,0013548659346947582
                Epoch 722/1000, Loss: 0,0013548256607677314
                Epoch 723/1000, Loss: 0,0013547855623004964
                Epoch 724/1000, Loss: 0,001354745638469658
                Epoch 725/1000, Loss: 0,0013547058884545026
                Epoch 726/1000, Loss: 0,0013546663114370155
                Epoch 727/1000, Loss: 0,0013546269066018837
                Epoch 728/1000, Loss: 0,00135458767313651
                Epoch 729/1000, Loss: 0,0013545486102310214
                Epoch 730/1000, Loss: 0,001354509717078278
                Epoch 731/1000, Loss: 0,0013544709928738833
                Epoch 732/1000, Loss: 0,0013544324368161928
                Epoch 733/1000, Loss: 0,001354394048106318
                Epoch 734/1000, Loss: 0,0013543558259481406
                Epoch 735/1000, Loss: 0,0013543177695483147
                Epoch 736/1000, Loss: 0,0013542798781162762
                Epoch 737/1000, Loss: 0,001354242150864249
                Epoch 738/1000, Loss: 0,0013542045870072524
                Epoch 739/1000, Loss: 0,0013541671857631049
                Epoch 740/1000, Loss: 0,0013541299463524334
                Epoch 741/1000, Loss: 0,0013540928679986746
                Epoch 742/1000, Loss: 0,0013540559499280835
                Epoch 743/1000, Loss: 0,001354019191369737
                Epoch 744/1000, Loss: 0,0013539825915555369
                Epoch 745/1000, Loss: 0,0013539461497202175
                Epoch 746/1000, Loss: 0,001353909865101345
                Epoch 747/1000, Loss: 0,0013538737369393262
                Epoch 748/1000, Loss: 0,0013538377644774077
                Epoch 749/1000, Loss: 0,0013538019469616813
                Epoch 750/1000, Loss: 0,001353766283641086
                Epoch 751/1000, Loss: 0,00135373077376741
                Epoch 752/1000, Loss: 0,0013536954165952939
                Epoch 753/1000, Loss: 0,0013536602113822325
                Epoch 754/1000, Loss: 0,001353625157388576
                Epoch 755/1000, Loss: 0,0013535902538775318
                Epoch 756/1000, Loss: 0,001353555500115166
                Epoch 757/1000, Loss: 0,0013535208953704039
                Epoch 758/1000, Loss: 0,0013534864389150296
                Epoch 759/1000, Loss: 0,001353452130023691
                Epoch 760/1000, Loss: 0,0013534179679738942
                Epoch 761/1000, Loss: 0,0013533839520460072
                Epoch 762/1000, Loss: 0,0013533500815232579
                Epoch 763/1000, Loss: 0,0013533163556917354
                Epoch 764/1000, Loss: 0,0013532827738403897
                Epoch 765/1000, Loss: 0,0013532493352610268
                Epoch 766/1000, Loss: 0,0013532160392483133
                Epoch 767/1000, Loss: 0,0013531828850997703
                Epoch 768/1000, Loss: 0,001353149872115776
                Epoch 769/1000, Loss: 0,0013531169995995604
                Epoch 770/1000, Loss: 0,0013530842668572078
                Epoch 771/1000, Loss: 0,0013530516731976483
                Epoch 772/1000, Loss: 0,0013530192179326632
                Epoch 773/1000, Loss: 0,0013529869003768773
                Epoch 774/1000, Loss: 0,0013529547198477582
                Epoch 775/1000, Loss: 0,0013529226756656128
                Epoch 776/1000, Loss: 0,0013528907671535861
                Epoch 777/1000, Loss: 0,0013528589936376563
                Epoch 778/1000, Loss: 0,0013528273544466324
                Epoch 779/1000, Loss: 0,0013527958489121508
                Epoch 780/1000, Loss: 0,0013527644763686722
                Epoch 781/1000, Loss: 0,0013527332361534757
                Epoch 782/1000, Loss: 0,0013527021276066586
                Epoch 783/1000, Loss: 0,0013526711500711284
                Epoch 784/1000, Loss: 0,0013526403028926034
                Epoch 785/1000, Loss: 0,0013526095854196015
                Epoch 786/1000, Loss: 0,0013525789970034456
                Epoch 787/1000, Loss: 0,001352548536998249
                Epoch 788/1000, Loss: 0,0013525182047609168
                Epoch 789/1000, Loss: 0,0013524879996511386
                Epoch 790/1000, Loss: 0,0013524579210313866
                Epoch 791/1000, Loss: 0,001352427968266907
                Epoch 792/1000, Loss: 0,0013523981407257157
                Epoch 793/1000, Loss: 0,0013523684377785945
                Epoch 794/1000, Loss: 0,001352338858799085
                Epoch 795/1000, Loss: 0,0013523094031634817
                Epoch 796/1000, Loss: 0,001352280070250829
                Epoch 797/1000, Loss: 0,0013522508594429126
                Epoch 798/1000, Loss: 0,0013522217701242565
                Epoch 799/1000, Loss: 0,0013521928016821155
                Epoch 800/1000, Loss: 0,0013521639535064678
                Epoch 801/1000, Loss: 0,001352135224990013
                Epoch 802/1000, Loss: 0,0013521066155281615
                Epoch 803/1000, Loss: 0,0013520781245190312
                Epoch 804/1000, Loss: 0,0013520497513634388
                Epoch 805/1000, Loss: 0,001352021495464897
                Epoch 806/1000, Loss: 0,0013519933562296021
                Epoch 807/1000, Loss: 0,0013519653330664337
                Epoch 808/1000, Loss: 0,0013519374253869437
                Epoch 809/1000, Loss: 0,0013519096326053504
                Epoch 810/1000, Loss: 0,0013518819541385337
                Epoch 811/1000, Loss: 0,0013518543894060256
                Epoch 812/1000, Loss: 0,001351826937830005
                Epoch 813/1000, Loss: 0,0013517995988352885
                Epoch 814/1000, Loss: 0,001351772371849325
                Epoch 815/1000, Loss: 0,0013517452563021897
                Epoch 816/1000, Loss: 0,001351718251626574
                Epoch 817/1000, Loss: 0,0013516913572577777
                Epoch 818/1000, Loss: 0,0013516645726337074
                Epoch 819/1000, Loss: 0,0013516378971948617
                Epoch 820/1000, Loss: 0,001351611330384329
                Epoch 821/1000, Loss: 0,0013515848716477762
                Epoch 822/1000, Loss: 0,0013515585204334445
                Epoch 823/1000, Loss: 0,0013515322761921402
                Epoch 824/1000, Loss: 0,0013515061383772256
                Epoch 825/1000, Loss: 0,0013514801064446135
                Epoch 826/1000, Loss: 0,0013514541798527598
                Epoch 827/1000, Loss: 0,001351428358062652
                Epoch 828/1000, Loss: 0,0013514026405378062
                Epoch 829/1000, Loss: 0,0013513770267442556
                Epoch 830/1000, Loss: 0,0013513515161505429
                Epoch 831/1000, Loss: 0,0013513261082277153
                Epoch 832/1000, Loss: 0,0013513008024493144
                Epoch 833/1000, Loss: 0,0013512755982913653
                Epoch 834/1000, Loss: 0,0013512504952323749
                Epoch 835/1000, Loss: 0,0013512254927533173
                Epoch 836/1000, Loss: 0,0013512005903376309
                Epoch 837/1000, Loss: 0,0013511757874712072
                Epoch 838/1000, Loss: 0,0013511510836423837
                Epoch 839/1000, Loss: 0,0013511264783419344
                Epoch 840/1000, Loss: 0,0013511019710630632
                Epoch 841/1000, Loss: 0,0013510775613013951
                Epoch 842/1000, Loss: 0,001351053248554969
                Epoch 843/1000, Loss: 0,0013510290323242257
                Epoch 844/1000, Loss: 0,0013510049121120034
                Epoch 845/1000, Loss: 0,0013509808874235286
                Epoch 846/1000, Loss: 0,0013509569577664063
                Epoch 847/1000, Loss: 0,0013509331226506135
                Epoch 848/1000, Loss: 0,0013509093815884882
                Epoch 849/1000, Loss: 0,0013508857340947234
                Epoch 850/1000, Loss: 0,0013508621796863587
                Epoch 851/1000, Loss: 0,001350838717882771
                Epoch 852/1000, Loss: 0,001350815348205664
                Epoch 853/1000, Loss: 0,001350792070179064
                Epoch 854/1000, Loss: 0,0013507688833293092
                Epoch 855/1000, Loss: 0,0013507457871850402
                Epoch 856/1000, Loss: 0,001350722781277193
                Epoch 857/1000, Loss: 0,0013506998651389914
                Epoch 858/1000, Loss: 0,001350677038305935
                Epoch 859/1000, Loss: 0,001350654300315795
                Epoch 860/1000, Loss: 0,0013506316507086026
                Epoch 861/1000, Loss: 0,0013506090890266426
                Epoch 862/1000, Loss: 0,0013505866148144417
                Epoch 863/1000, Loss: 0,0013505642276187646
                Epoch 864/1000, Loss: 0,0013505419269886016
                Epoch 865/1000, Loss: 0,0013505197124751614
                Epoch 866/1000, Loss: 0,0013504975836318646
                Epoch 867/1000, Loss: 0,0013504755400143304
                Epoch 868/1000, Loss: 0,0013504535811803726
                Epoch 869/1000, Loss: 0,00135043170668999
                Epoch 870/1000, Loss: 0,0013504099161053562
                Epoch 871/1000, Loss: 0,0013503882089908128
                Epoch 872/1000, Loss: 0,0013503665849128594
                Epoch 873/1000, Loss: 0,0013503450434401486
                Epoch 874/1000, Loss: 0,0013503235841434724
                Epoch 875/1000, Loss: 0,0013503022065957568
                Epoch 876/1000, Loss: 0,0013502809103720535
                Epoch 877/1000, Loss: 0,0013502596950495313
                Epoch 878/1000, Loss: 0,0013502385602074645
                Epoch 879/1000, Loss: 0,00135021750542723
                Epoch 880/1000, Loss: 0,0013501965302922944
                Epoch 881/1000, Loss: 0,0013501756343882077
                Epoch 882/1000, Loss: 0,0013501548173025927
                Epoch 883/1000, Loss: 0,0013501340786251394
                Epoch 884/1000, Loss: 0,0013501134179475956
                Epoch 885/1000, Loss: 0,001350092834863758
                Epoch 886/1000, Loss: 0,0013500723289694618
                Epoch 887/1000, Loss: 0,0013500518998625782
                Epoch 888/1000, Loss: 0,0013500315471429995
                Epoch 889/1000, Loss: 0,0013500112704126353
                Epoch 890/1000, Loss: 0,001349991069275401
                Epoch 891/1000, Loss: 0,0013499709433372122
                Epoch 892/1000, Loss: 0,0013499508922059744
                Epoch 893/1000, Loss: 0,0013499309154915758
                Epoch 894/1000, Loss: 0,0013499110128058784
                Epoch 895/1000, Loss: 0,0013498911837627105
                Epoch 896/1000, Loss: 0,0013498714279778566
                Epoch 897/1000, Loss: 0,0013498517450690522
                Epoch 898/1000, Loss: 0,0013498321346559724
                Epoch 899/1000, Loss: 0,0013498125963602265
                Epoch 900/1000, Loss: 0,0013497931298053476
                Epoch 901/1000, Loss: 0,0013497737346167844
                Epoch 902/1000, Loss: 0,0013497544104218964
                Epoch 903/1000, Loss: 0,0013497351568499405
                Epoch 904/1000, Loss: 0,001349715973532068
                Epoch 905/1000, Loss: 0,0013496968601013127
                Epoch 906/1000, Loss: 0,001349677816192585
                Epoch 907/1000, Loss: 0,0013496588414426626
                Epoch 908/1000, Loss: 0,0013496399354901822
                Epoch 909/1000, Loss: 0,0013496210979756337
                Epoch 910/1000, Loss: 0,0013496023285413505
                Epoch 911/1000, Loss: 0,0013495836268315009
                Epoch 912/1000, Loss: 0,0013495649924920804
                Epoch 913/1000, Loss: 0,0013495464251709062
                Epoch 914/1000, Loss: 0,0013495279245176058
                Epoch 915/1000, Loss: 0,0013495094901836108
                Epoch 916/1000, Loss: 0,0013494911218221485
                Epoch 917/1000, Loss: 0,0013494728190882352
                Epoch 918/1000, Loss: 0,001349454581638667
                Epoch 919/1000, Loss: 0,001349436409132012
                Epoch 920/1000, Loss: 0,001349418301228603
                Epoch 921/1000, Loss: 0,0013494002575905298
                Epoch 922/1000, Loss: 0,0013493822778816306
                Epoch 923/1000, Loss: 0,0013493643617674867
                Epoch 924/1000, Loss: 0,0013493465089154109
                Epoch 925/1000, Loss: 0,0013493287189944414
                Epoch 926/1000, Loss: 0,001349310991675337
                Epoch 927/1000, Loss: 0,0013492933266305638
                Epoch 928/1000, Loss: 0,001349275723534294
                Epoch 929/1000, Loss: 0,0013492581820623915
                Epoch 930/1000, Loss: 0,0013492407018924101
                Epoch 931/1000, Loss: 0,0013492232827035833
                Epoch 932/1000, Loss: 0,0013492059241768154
                Epoch 933/1000, Loss: 0,0013491886259946772
                Epoch 934/1000, Loss: 0,001349171387841396
                Epoch 935/1000, Loss: 0,0013491542094028492
                Epoch 936/1000, Loss: 0,001349137090366556
                Epoch 937/1000, Loss: 0,0013491200304216711
                Epoch 938/1000, Loss: 0,0013491030292589765
                Epoch 939/1000, Loss: 0,0013490860865708744
                Epoch 940/1000, Loss: 0,001349069202051378
                Epoch 941/1000, Loss: 0,0013490523753961095
                Epoch 942/1000, Loss: 0,0013490356063022842
                Epoch 943/1000, Loss: 0,001349018894468711
                Epoch 944/1000, Loss: 0,001349002239595783
                Epoch 945/1000, Loss: 0,0013489856413854671
                Epoch 946/1000, Loss: 0,0013489690995413003
                Epoch 947/1000, Loss: 0,0013489526137683804
                Epoch 948/1000, Loss: 0,0013489361837733594
                Epoch 949/1000, Loss: 0,001348919809264438
                Epoch 950/1000, Loss: 0,0013489034899513553
                Epoch 951/1000, Loss: 0,001348887225545384
                Epoch 952/1000, Loss: 0,0013488710157593229
                Epoch 953/1000, Loss: 0,0013488548603074896
                Epoch 954/1000, Loss: 0,001348838758905712
                Epoch 955/1000, Loss: 0,001348822711271324
                Epoch 956/1000, Loss: 0,0013488067171231566
                Epoch 957/1000, Loss: 0,0013487907761815313
                Epoch 958/1000, Loss: 0,0013487748881682534
                Epoch 959/1000, Loss: 0,0013487590528066056
                Epoch 960/1000, Loss: 0,001348743269821338
                Epoch 961/1000, Loss: 0,0013487275389386668
                Epoch 962/1000, Loss: 0,0013487118598862616
                Epoch 963/1000, Loss: 0,0013486962323932422
                Epoch 964/1000, Loss: 0,0013486806561901704
                Epoch 965/1000, Loss: 0,001348665131009045
                Epoch 966/1000, Loss: 0,0013486496565832903
                Epoch 967/1000, Loss: 0,0013486342326477557
                Epoch 968/1000, Loss: 0,0013486188589387042
                Epoch 969/1000, Loss: 0,001348603535193808
                Epoch 970/1000, Loss: 0,0013485882611521407
                Epoch 971/1000, Loss: 0,0013485730365541703
                Epoch 972/1000, Loss: 0,0013485578611417558
                Epoch 973/1000, Loss: 0,0013485427346581354
                Epoch 974/1000, Loss: 0,0013485276568479245
                Epoch 975/1000, Loss: 0,0013485126274571059
                Epoch 976/1000, Loss: 0,0013484976462330256
                Epoch 977/1000, Loss: 0,0013484827129243845
                Epoch 978/1000, Loss: 0,0013484678272812332
                Epoch 979/1000, Loss: 0,0013484529890549649
                Epoch 980/1000, Loss: 0,0013484381979983091
                Epoch 981/1000, Loss: 0,001348423453865325
                Epoch 982/1000, Loss: 0,0013484087564113946
                Epoch 983/1000, Loss: 0,0013483941053932197
                Epoch 984/1000, Loss: 0,0013483795005688084
                Epoch 985/1000, Loss: 0,0013483649416974764
                Epoch 986/1000, Loss: 0,0013483504285398365
                Epoch 987/1000, Loss: 0,001348335960857793
                Epoch 988/1000, Loss: 0,001348321538414536
                Epoch 989/1000, Loss: 0,0013483071609745353
                Epoch 990/1000, Loss: 0,0013482928283035317
                Epoch 991/1000, Loss: 0,0013482785401685358
                Epoch 992/1000, Loss: 0,0013482642963378165
                Epoch 993/1000, Loss: 0,001348250096580899
                Epoch 994/1000, Loss: 0,0013482359406685553
                Epoch 995/1000, Loss: 0,0013482218283728006
                Epoch 996/1000, Loss: 0,0013482077594668858
                Epoch 997/1000, Loss: 0,001348193733725293
                Epoch 998/1000, Loss: 0,0013481797509237268
                Epoch 999/1000, Loss: 0,0013481658108391117
                Epoch 1000/1000, Loss: 0,0013481519132495824
                Testing trained RNN on physics sentences:
                Input: energy is conserved in system
                Predicted: is conserved in vacuum

                Input: force equals mass times acceleration
                Predicted: equals mass times acceleration

                Input: light travels in vacuum at speed
                Predicted: travels in vacuum at speed

                Input: electron has negative charge
                Predicted: has negative at

                Input: gravity pulls objects towards earth
                Predicted: pulls objects towards earth


                Code:
                using System;
                using System.Collections.Generic;
                using System.Globalization;
                using System.IO;

                public class Program
                {
                    public static void Main(string[] args)
                    {
                        string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
                        int embeddingDim = 300;
                        var glove = new GloveLoader(gloveFilePath, embeddingDim);

                        int hiddenSize = 10;
                        double learningRate = 0.1;
                        int epochs = 1000;

                        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

                        // Five physics-related sentences for training
                        List<string[]> physicsSentences = new List<string[]>
                        {
                            new string[] { "energy", "is", "conserved", "in", "system" },
                            new string[] { "force", "equals", "mass", "times", "acceleration" },
                            new string[] { "light", "travels", "in", "vacuum", "at", "speed" },
                            new string[] { "electron", "has", "negative", "charge" },
                            new string[] { "gravity", "pulls", "objects", "towards", "earth" }
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
                            Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
                        }

                        // Testing the RNN to see if it can recall physics sentences
                        Console.WriteLine("Testing trained RNN on physics sentences:");
                        foreach (var sentence in physicsSentences)
                        {
                            Console.Write("Input: ");
                            foreach (var word in sentence)
                            {
                                Console.Write(word + " ");
                            }
                            Console.Write("\nPredicted: ");

                            List<double[]> sentenceEmbeddings = new List<double[]>();
                            foreach (var word in sentence)
                            {
                                sentenceEmbeddings.Add(glove.GetEmbedding(word));
                            }

                            for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                            {
                                double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                                string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                                Console.Write(predictedWord + " ");
                            }
                            Console.WriteLine("\n");
                        }

                        Console.ReadKey();
                    }
                }

                Code:
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


                Code:
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

                Code:
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


                Code:
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

                Code:
                using System;
                using System.Collections.Generic;
                using System.Globalization;
                using System.IO;

                public class Program
                {
                    public static void Main(string[] args)
                    {
                        string gloveFilePath = "../../../../../../../GloVe/glove.6B.300d.txt";
                        int embeddingDim = 300;
                        var glove = new GloveLoader(gloveFilePath, embeddingDim);

                        int hiddenSize = 10;
                        double learningRate = 0.1;
                        int epochs = 1000;

                        SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

                        // Five physics-related sentences for training
                        List<string[]> physicsSentences = new List<string[]>
                        {
                            new string[] { "energy", "is", "conserved", "in", "system" },
                            new string[] { "force", "equals", "mass", "times", "acceleration" },
                            new string[] { "light", "travels", "in", "vacuum", "at", "speed" },
                            new string[] { "electron", "has", "negative", "charge" },
                            new string[] { "gravity", "pulls", "objects", "towards", "earth" }
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
                            Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");
                        }

                        // Testing the RNN to see if it can recall physics sentences
                        Console.WriteLine("Testing trained RNN on physics sentences:");
                        foreach (var sentence in physicsSentences)
                        {
                            Console.Write("Input: ");
                            foreach (var word in sentence)
                            {
                                Console.Write(word + " ");
                            }
                            Console.Write("\nPredicted: ");

                            List<double[]> sentenceEmbeddings = new List<double[]>();
                            foreach (var word in sentence)
                            {
                                sentenceEmbeddings.Add(glove.GetEmbedding(word));
                            }

                            for (int i = 0; i < sentenceEmbeddings.Count - 1; i++)
                            {
                                double[] physicsOutputEmbedding = rnn.Forward(sentenceEmbeddings[i]);
                                string predictedWord = glove.FindClosestWord(physicsOutputEmbedding);
                                Console.Write(predictedWord + " ");
                            }
                            Console.WriteLine("\n");
                        }

                        Console.ReadKey();
                    }
                }
                */

            }
            #endregion 

            Console.WriteLine();

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

            codeProcessing.ProcessCodeBlock(sourceCodeIteration10, sourceCodeIteration10.SourceCodes[1].Code, sourceCodeIteration10.SourceCodes[2].Code, sourceCodeIteration10.SourceCodes[0].Code);

            projectManager1.Print(project10);

            /*
            Project: Temperature
            conversation #1
            iteration #1
            Temperature too low
            Temperature: 35
            Temperature: 40

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

                        */

            #endregion

            Console.WriteLine();

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

                codeProcessing1.ProcessCodeBlock(sourceCodeIteration1, sourceCodeIteration1.SourceCodes[0].Code);

                projectManager1.Print(project7);

                /*                 
                Project: Linq
                conversation #1
                iteration #1
                Vector for 'computer': [-0,27628, 0,13999, 0,098519, -0,64019, 0,031988, 0,10066, -0,18673, -0,37129, 0,5974, -2,0405, 0,22368, -0,026314, 0,72408, -0,43829, 0,48886, -0,0035486, -0,10006, -0,30587, -0,15621, -0,068136, 0,21104, 0,29287, -0,088861, -0,20462, -0,57602, 0,34526, 0,4139, 0,17917, 0,25143, -0,22678, -0,10103, 0,14576, 0,20127, 0,3181, -0,78907, -0,22194, -0,24833, -0,015103, -0,2005, -0,026441, 0,18551, 0,33782, -0,33543, 0,86117, -0,047083, -0,17009, 0,30438, 0,094119, 0,32435, -0,81171, 0,88966, -0,39149, 0,16828, 0,14316, 0,0036339, -0,064557, 0,045777, -0,32248, 0,048943, 0,16817, 0,068344, 0,54227, 0,12493, 0,69742, -0,037194, 0,3308, -0,42194, 0,3397, 0,27646, -0,016003, -0,21827, 0,44535, 0,35379, -0,022089, 0,21375, 0,43267, -0,32897, 0,096165, 0,31265, -0,30528, 0,26126, -0,65364, -0,78014, -0,23154, 0,12113, 0,34896, -0,55444, 0,46619, -0,1652, 0,11611, -0,76676, 0,69502, -0,15698, -0,1249, 0,56505, 0,64499, -0,57403, -0,033549, 0,32898, -1,4025, -0,31143, 0,64549, -0,061534, -0,69295, 0,00060894, -0,56544, 0,19181, -0,19208, -0,62673, -0,0097473, -0,5504, -0,56128, -0,19603, 0,29254, 0,098576, -0,059395, 0,0033616, 0,19515, -0,60703, 0,34262, 0,095211, -0,079411, 0,14305, -0,56569, -0,065887, 0,15167, -0,13505, 0,19571, 0,22812, 0,035346, -0,22509, 0,1891, -0,37348, 0,12505, 0,46249, -0,32219, 0,90643, 0,11595, 0,11628, 0,22961, 0,2401, -0,061609, 0,39325, -0,065066, 0,42257, 0,5688, 0,49804, -0,61308, 0,41468, -0,13448, 0,6043, -0,065462, -0,085376, 0,19115, 0,39925, 0,37495, -0,18492, 0,061751, -0,38747, -0,30335, -0,38211, 0,28221, -0,10286, -0,5866, 0,82922, 0,25131, 0,24772, 0,87482, -0,31359, 0,81621, -0,90081, -0,77933, -1,009, 0,36472, -0,11562, -0,24841, 0,094527, -0,42266, 0,060392, -0,15365, -0,069604, 0,0051292, 0,39572, -0,15692, 0,35708, -0,35165, 0,35296, -0,5222, 0,514, -0,17764, -0,10272, -0,3964, 0,30418, 0,073659, -0,11685, 0,14299, -0,3681, 0,27642, -0,46683, -0,32633, 0,51107, 0,023945, 0,11723, 0,21761, -0,17389, -0,61193, -0,59449, 0,47749, -0,59008, -0,36092, -0,099574, -0,043098, -0,15106, -0,14336, -0,031135, 0,17887, -0,64221, 0,17242, 0,33916, 0,87181, -0,7723, 0,53195, -0,52763, 0,1751, 0,31043, -0,15177, -0,22706, 0,10803, 0,44919, 0,070016, 0,20851, 0,21517, -0,61712, -0,09997, 0,005502, 0,076786, 0,28046, 0,42331, -0,58925, 0,070554, 0,39923, 0,090201, 0,17139, -0,17282, -0,53675, -0,46439, -0,5785, -0,68311, 0,059383, 0,12427, -0,14558, 0,57687, -0,57499, -0,051645, 0,3841, 0,13047, 0,33786, 0,33204, 0,40119, 0,26389, -0,36953, -0,29797, -0,66816, -0,11883, 0,50133, 0,20603, -0,32558, -0,12242, 0,50666, 0,16353, -0,10672, 0,22364, 0,23915, -0,55509, -0,48432, -0,012165, -1,7992, 0,3231, -0,26309, -0,32538, -0,5827, 0,15099, 0,33838, 0,12007, 0,41395, -0,15553, -0,19301, 0,05886, -0,5242, -0,3717, 0,56205, -0,65801, -0,49796, 0,24347, 0,12873, 0,33665, -0,072609, -0,15686, -0,14187, -0,26488]

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
                    */
            }
            #endregion

            Console.ReadLine();
        }

    }
}