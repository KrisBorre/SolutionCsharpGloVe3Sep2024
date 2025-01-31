using LibraryGlobalVectors1Sep2024;
using LibrarySimpleRNN1Sep2024;

namespace ConsoleSimpleRNNGloVe9Sep2024_100d
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.100d.txt";
            int embeddingDim = 100;

            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            int hiddenSize = 3;
            double learningRate = 0.1;
            int epochs = 300;

            SimpleRNN rnn = new SimpleRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

            var inputWord1 = "atom";
            var targetWord1 = "molecule";

            double[] inputEmbedding1 = glove.GetEmbedding(inputWord1);
            double[] targetEmbedding1 = glove.GetEmbedding(targetWord1);

            var inputWord2 = "king";
            var targetWord2 = "queen";

            double[] inputEmbedding2 = glove.GetEmbedding(inputWord2);
            double[] targetEmbedding2 = glove.GetEmbedding(targetWord2);

            var inputWord3 = "electron";
            var targetWord3 = "proton";

            double[] inputEmbedding3 = glove.GetEmbedding(inputWord3);
            double[] targetEmbedding3 = glove.GetEmbedding(targetWord3);

            // Simple sequence data with an input pattern and its subsequent target
            double[][] inputs = new double[][]
            {
                inputEmbedding1,
                inputEmbedding2,
                inputEmbedding3
            };

            // Target output patterns, shifting the pattern right
            double[][] targets = new double[][]
            {
                targetEmbedding1,
                targetEmbedding2,
                targetEmbedding3
            };


            double initialLoss = double.MaxValue;
            double lastLoss = initialLoss;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;

                // Training for each input-target pair
                for (int i = 0; i < inputs.Length; i++)
                {
                    // Forward pass
                    double[] output = rnn.Forward(inputs[i]);
                    string inputWord = glove.FindClosestWord(inputs[i]);
                    string predictedWord = glove.FindClosestWord(output);
                    Console.WriteLine(inputWord + " " + predictedWord);

                    // Calculate Mean Squared Error loss
                    double loss = 0;
                    for (int j = 0; j < output.Length; j++)
                    {
                        loss += Math.Pow(output[j] - targets[i][j], 2);
                    }
                    totalLoss += loss;

                    // Backward pass for adjusting weights
                    rnn.Backward(output, targets[i], inputs[i]);
                }

                // Average loss over inputs for the epoch
                totalLoss /= inputs.Length;

                // Assert that the loss decreases (at least generally)
                if (totalLoss > lastLoss)
                {
                    Console.WriteLine($"Epoch {epoch + 1}: Loss did not decrease. Loss: {totalLoss}");
                }

                lastLoss = totalLoss;

                // Display loss for early epochs to track improvement
                Console.WriteLine($"Epoch {epoch + 1}/{epochs}, Loss: {totalLoss}");

            }

            // Assert that learning took place by checking for an overall loss reduction compared to the initial loss
            if (lastLoss >= initialLoss)
            {
                Console.WriteLine("Loss did not decrease over 10 epochs, indicating lack of learning.");
            }

            /*
            atom vnd
            king aas
            electron bravely
            Epoch 1/300, Loss: 1,5547758475615325
            atom thd
            king k-pop
            electron harburg
            Epoch 2: Loss did not decrease. Loss: 6,679515932773147
            Epoch 2/300, Loss: 6,679515932773147
            atom earths
            king psa
            electron molecule
            Epoch 3/300, Loss: 3,77213277521146
            atom queen
            king proton
            electron molecule
            Epoch 4/300, Loss: 0,3311359655769902
            atom queen
            king proton
            electron molecule
            Epoch 5/300, Loss: 0,3275408649720699
            atom queen
            king proton
            electron molecule
            Epoch 6/300, Loss: 0,3237758891974803
            atom queen
            king proton
            electron molecule
            Epoch 7: Loss did not decrease. Loss: 0,32656506017306636
            Epoch 7/300, Loss: 0,32656506017306636
            atom queen
            king proton
            electron molecule
            Epoch 8/300, Loss: 0,32343909332884374
            atom queen
            king proton
            electron molecule
            Epoch 9: Loss did not decrease. Loss: 0,3258745137333281
            Epoch 9/300, Loss: 0,3258745137333281
            atom queen
            king proton
            electron molecule
            Epoch 10/300, Loss: 0,32318118588835926
            atom queen
            king proton
            electron molecule
            Epoch 11: Loss did not decrease. Loss: 0,32532863958477554
            Epoch 11/300, Loss: 0,32532863958477554
            atom queen
            king proton
            electron molecule
            Epoch 12/300, Loss: 0,322978931167648
            atom queen
            king proton
            electron molecule
            Epoch 13: Loss did not decrease. Loss: 0,3248854430136569
            Epoch 13/300, Loss: 0,3248854430136569
            atom queen
            king proton
            electron molecule
            Epoch 14/300, Loss: 0,32281882484519864
            atom queen
            king proton
            electron molecule
            Epoch 15: Loss did not decrease. Loss: 0,32451893770870116
            Epoch 15/300, Loss: 0,32451893770870116
            atom queen
            king proton
            electron molecule
            Epoch 16/300, Loss: 0,3226897987529033
            atom queen
            king proton
            electron molecule
            Epoch 17: Loss did not decrease. Loss: 0,3242111712608948
            Epoch 17/300, Loss: 0,3242111712608948
            atom queen
            king proton
            electron molecule
            Epoch 18/300, Loss: 0,3225840782110792
            atom queen
            king proton
            electron molecule
            Epoch 19: Loss did not decrease. Loss: 0,3239494360073902
            Epoch 19/300, Loss: 0,3239494360073902
            atom queen
            king proton
            electron molecule
            Epoch 20/300, Loss: 0,3224962166026762
            atom queen
            king proton
            electron molecule
            Epoch 21: Loss did not decrease. Loss: 0,32372451912987116
            Epoch 21/300, Loss: 0,32372451912987116
            atom queen
            king proton
            electron molecule
            Epoch 22/300, Loss: 0,32242232291480405
            atom queen
            king proton
            electron molecule
            Epoch 23: Loss did not decrease. Loss: 0,32352958095526807
            Epoch 23/300, Loss: 0,32352958095526807
            atom queen
            king proton
            electron molecule
            Epoch 24/300, Loss: 0,3223595559887937
            atom queen
            king proton
            electron molecule
            Epoch 25: Loss did not decrease. Loss: 0,32335942950834484
            Epoch 25/300, Loss: 0,32335942950834484
            atom queen
            king proton
            electron molecule
            Epoch 26/300, Loss: 0,3223057971632472
            atom queen
            king proton
            electron molecule
            Epoch 27: Loss did not decrease. Loss: 0,32321004258282227
            Epoch 27/300, Loss: 0,32321004258282227
            atom queen
            king proton
            electron molecule
            Epoch 28/300, Loss: 0,32225943515033834
            atom queen
            king proton
            electron molecule
            Epoch 29: Loss did not decrease. Loss: 0,3230782463013144
            Epoch 29/300, Loss: 0,3230782463013144
            atom queen
            king proton
            electron molecule
            Epoch 30/300, Loss: 0,3222192219953578
            atom queen
            king proton
            electron molecule
            Epoch 31: Loss did not decrease. Loss: 0,32296149447384365
            Epoch 31/300, Loss: 0,32296149447384365
            atom queen
            king proton
            electron molecule
            Epoch 32/300, Loss: 0,3221841748314653
            atom queen
            king proton
            electron molecule
            Epoch 33: Loss did not decrease. Loss: 0,322857714157423
            Epoch 33/300, Loss: 0,322857714157423
            atom queen
            king proton
            electron molecule
            Epoch 34/300, Loss: 0,322153507671469
            atom queen
            king proton
            electron molecule
            Epoch 35: Loss did not decrease. Loss: 0,322765195521414
            Epoch 35/300, Loss: 0,322765195521414
            atom queen
            king proton
            electron molecule
            Epoch 36/300, Loss: 0,32212658324300963
            atom queen
            king proton
            electron molecule
            Epoch 37: Loss did not decrease. Loss: 0,32268251190414776
            Epoch 37/300, Loss: 0,32268251190414776
            atom queen
            king proton
            electron molecule
            Epoch 38/300, Loss: 0,3221028784189308
            atom queen
            king proton
            electron molecule
            Epoch 39: Loss did not decrease. Loss: 0,322608460799565
            Epoch 39/300, Loss: 0,322608460799565
            atom queen
            king proton
            electron molecule
            Epoch 40/300, Loss: 0,3220819590136229
            atom queen
            king proton
            electron molecule
            Epoch 41: Loss did not decrease. Loss: 0,32254201959368656
            Epoch 41/300, Loss: 0,32254201959368656
            atom queen
            king proton
            electron molecule
            Epoch 42/300, Loss: 0,322063461128373
            atom queen
            king proton
            electron molecule
            Epoch 43: Loss did not decrease. Loss: 0,3224823118599021
            Epoch 43/300, Loss: 0,3224823118599021
            atom queen
            king proton
            electron molecule
            Epoch 44/300, Loss: 0,3220470771418185
            atom queen
            king proton
            electron molecule
            Epoch 45: Loss did not decrease. Loss: 0,32242858132703545
            Epoch 45/300, Loss: 0,32242858132703545
            atom queen
            king proton
            electron molecule
            Epoch 46/300, Loss: 0,3220325450407542
            atom queen
            king proton
            electron molecule
            Epoch 47: Loss did not decrease. Loss: 0,32238017150341663
            Epoch 47/300, Loss: 0,32238017150341663
            atom queen
            king proton
            electron molecule
            Epoch 48/300, Loss: 0,3220196401852165
            atom queen
            king proton
            electron molecule
            Epoch 49: Loss did not decrease. Loss: 0,322336509527706
            Epoch 49/300, Loss: 0,322336509527706
            atom queen
            king proton
            electron molecule
            Epoch 50/300, Loss: 0,3220081688706134
            atom queen
            king proton
            electron molecule
            Epoch 51: Loss did not decrease. Loss: 0,32229709321986294
            Epoch 51/300, Loss: 0,32229709321986294
            atom queen
            king proton
            electron molecule
            Epoch 52/300, Loss: 0,32199796323321456
            atom queen
            king proton
            electron molecule
            Epoch 53: Loss did not decrease. Loss: 0,3222614805852925
            Epoch 53/300, Loss: 0,3222614805852925
            atom queen
            king proton
            electron molecule
            Epoch 54/300, Loss: 0,32198887717217134
            atom queen
            king proton
            electron molecule
            Epoch 55: Loss did not decrease. Loss: 0,32222928122189703
            Epoch 55/300, Loss: 0,32222928122189703
            atom queen
            king proton
            electron molecule
            Epoch 56/300, Loss: 0,3219807830498988
            atom queen
            king proton
            electron molecule
            Epoch 57: Loss did not decrease. Loss: 0,32220014921982093
            Epoch 57/300, Loss: 0,32220014921982093
            atom queen
            king proton
            electron molecule
            Epoch 58/300, Loss: 0,3219735689953161
            atom queen
            king proton
            electron molecule
            Epoch 59: Loss did not decrease. Loss: 0,32217377724458557
            Epoch 59/300, Loss: 0,32217377724458557
            atom queen
            king proton
            electron molecule
            Epoch 60/300, Loss: 0,3219671366792027
            atom queen
            king proton
            electron molecule
            Epoch 61: Loss did not decrease. Loss: 0,32214989156783175
            Epoch 61/300, Loss: 0,32214989156783175
            atom queen
            king proton
            electron molecule
            Epoch 62/300, Loss: 0,3219613994632298
            atom queen
            king proton
            electron molecule
            Epoch 63: Loss did not decrease. Loss: 0,3221282478640462
            Epoch 63/300, Loss: 0,3221282478640462
            atom queen
            king proton
            electron molecule
            Epoch 64/300, Loss: 0,321956280847775
            atom queen
            king proton
            electron molecule
            Epoch 65: Loss did not decrease. Loss: 0,3221086276319641
            Epoch 65/300, Loss: 0,3221086276319641
            atom queen
            king proton
            electron molecule
            Epoch 66/300, Loss: 0,32195171316095483
            atom queen
            king proton
            electron molecule
            Epoch 67: Loss did not decrease. Loss: 0,32209083512963915
            Epoch 67/300, Loss: 0,32209083512963915
            atom queen
            king proton
            electron molecule
            Epoch 68/300, Loss: 0,3219476364441971
            atom queen
            king proton
            electron molecule
            Epoch 69: Loss did not decrease. Loss: 0,32207469473519246
            Epoch 69/300, Loss: 0,32207469473519246
            atom queen
            king proton
            electron molecule
            Epoch 70/300, Loss: 0,32194399749933217
            atom queen
            king proton
            electron molecule
            Epoch 71: Loss did not decrease. Loss: 0,3220600486628751
            Epoch 71/300, Loss: 0,3220600486628751
            atom queen
            king proton
            electron molecule
            Epoch 72/300, Loss: 0,32194074906949594
            atom queen
            king proton
            electron molecule
            Epoch 73: Loss did not decrease. Loss: 0,32204675497771396
            Epoch 73/300, Loss: 0,32204675497771396
            atom queen
            king proton
            electron molecule
            Epoch 74/300, Loss: 0,32193784913172613
            atom queen
            king proton
            electron molecule
            Epoch 75: Loss did not decrease. Loss: 0,32203468586264444
            Epoch 75/300, Loss: 0,32203468586264444
            atom queen
            king proton
            electron molecule
            Epoch 76/300, Loss: 0,3219352602834387
            atom queen
            king proton
            electron molecule
            Epoch 77: Loss did not decrease. Loss: 0,32202372610038865
            Epoch 77/300, Loss: 0,32202372610038865
            atom queen
            king proton
            electron molecule
            Epoch 78/300, Loss: 0,3219329492083181
            atom queen
            king proton
            electron molecule
            Epoch 79: Loss did not decrease. Loss: 0,32201377173896556
            Epoch 79/300, Loss: 0,32201377173896556
            atom queen
            king proton
            electron molecule
            Epoch 80/300, Loss: 0,3219308862097794
            atom queen
            king proton
            electron molecule
            Epoch 81: Loss did not decrease. Loss: 0,32200472891501114
            Epoch 81/300, Loss: 0,32200472891501114
            atom queen
            king proton
            electron molecule
            Epoch 82/300, Loss: 0,32192904480223494
            atom queen
            king proton
            electron molecule
            Epoch 83: Loss did not decrease. Loss: 0,3219965128133412
            Epoch 83/300, Loss: 0,3219965128133412
            atom queen
            king proton
            electron molecule
            Epoch 84/300, Loss: 0,3219274013520515
            atom queen
            king proton
            electron molecule
            Epoch 85: Loss did not decrease. Loss: 0,32198904674464274
            Epoch 85/300, Loss: 0,32198904674464274
            atom queen
            king proton
            electron molecule
            Epoch 86/300, Loss: 0,3219259347614134
            atom queen
            king proton
            electron molecule
            Epoch 87: Loss did not decrease. Loss: 0,32198226132598345
            Epoch 87/300, Loss: 0,32198226132598345
            atom queen
            king proton
            electron molecule
            Epoch 88/300, Loss: 0,3219246261893804
            atom queen
            king proton
            electron molecule
            Epoch 89: Loss did not decrease. Loss: 0,3219760937511444
            Epoch 89/300, Loss: 0,3219760937511444
            atom queen
            king proton
            electron molecule
            Epoch 90/300, Loss: 0,3219234588053077
            atom queen
            king proton
            electron molecule
            Epoch 91: Loss did not decrease. Loss: 0,3219704871396829
            Epoch 91/300, Loss: 0,3219704871396829
            atom queen
            king proton
            electron molecule
            Epoch 92/300, Loss: 0,3219224175705138
            atom queen
            king proton
            electron molecule
            Epoch 93: Loss did not decrease. Loss: 0,3219653899552146
            Epoch 93/300, Loss: 0,3219653899552146
            atom queen
            king proton
            electron molecule
            Epoch 94/300, Loss: 0,3219214890446781
            atom queen
            king proton
            electron molecule
            Epoch 95: Loss did not decrease. Loss: 0,3219607554847262
            Epoch 95/300, Loss: 0,3219607554847262
            atom queen
            king proton
            electron molecule
            Epoch 96/300, Loss: 0,32192066121394464
            atom queen
            king proton
            electron molecule
            Epoch 97: Loss did not decrease. Loss: 0,3219565413718401
            Epoch 97/300, Loss: 0,3219565413718401
            atom queen
            king proton
            electron molecule
            Epoch 98/300, Loss: 0,3219199233381267
            atom queen
            king proton
            electron molecule
            Epoch 99: Loss did not decrease. Loss: 0,3219527091978852
            Epoch 99/300, Loss: 0,3219527091978852
            atom queen
            king proton
            electron molecule
            Epoch 100/300, Loss: 0,3219192658147458
            atom queen
            king proton
            electron molecule
            Epoch 101: Loss did not decrease. Loss: 0,32194922410542587
            Epoch 101/300, Loss: 0,32194922410542587
            atom queen
            king proton
            electron molecule
            Epoch 102/300, Loss: 0,32191868005795027
            atom queen
            king proton
            electron molecule
            Epoch 103: Loss did not decrease. Loss: 0,3219460544595708
            Epoch 103/300, Loss: 0,3219460544595708
            atom queen
            king proton
            electron molecule
            Epoch 104/300, Loss: 0,32191815839059273
            atom queen
            king proton
            electron molecule
            Epoch 105: Loss did not decrease. Loss: 0,32194317154296853
            Epoch 105/300, Loss: 0,32194317154296853
            atom queen
            king proton
            electron molecule
            Epoch 106/300, Loss: 0,32191769394797387
            atom queen
            king proton
            electron molecule
            Epoch 107: Loss did not decrease. Loss: 0,3219405492808823
            Epoch 107/300, Loss: 0,3219405492808823
            atom queen
            king proton
            electron molecule
            Epoch 108/300, Loss: 0,3219172805919369
            atom queen
            king proton
            electron molecule
            Epoch 109: Loss did not decrease. Loss: 0,3219381639931756
            Epoch 109/300, Loss: 0,3219381639931756
            atom queen
            king proton
            electron molecule
            Epoch 110/300, Loss: 0,3219169128341586
            atom queen
            king proton
            electron molecule
            Epoch 111: Loss did not decrease. Loss: 0,3219359941704012
            Epoch 111/300, Loss: 0,3219359941704012
            atom queen
            king proton
            electron molecule
            Epoch 112/300, Loss: 0,321916585767622
            atom queen
            king proton
            electron molecule
            Epoch 113: Loss did not decrease. Loss: 0,32193402027150947
            Epoch 113/300, Loss: 0,32193402027150947
            atom queen
            king proton
            electron molecule
            Epoch 114/300, Loss: 0,32191629500537017
            atom queen
            king proton
            electron molecule
            Epoch 115: Loss did not decrease. Loss: 0,3219322245409753
            Epoch 115/300, Loss: 0,3219322245409753
            atom queen
            king proton
            electron molecule
            Epoch 116/300, Loss: 0,3219160366257505
            atom queen
            king proton
            electron molecule
            Epoch 117: Loss did not decrease. Loss: 0,321930590843381
            Epoch 117/300, Loss: 0,321930590843381
            atom queen
            king proton
            electron molecule
            Epoch 118/300, Loss: 0,32191580712344586
            atom queen
            king proton
            electron molecule
            Epoch 119: Loss did not decrease. Loss: 0,32192910451370954
            Epoch 119/300, Loss: 0,32192910451370954
            atom queen
            king proton
            electron molecule
            Epoch 120/300, Loss: 0,32191560336566877
            atom queen
            king proton
            electron molecule
            Epoch 121: Loss did not decrease. Loss: 0,32192775222179343
            Epoch 121/300, Loss: 0,32192775222179343
            atom queen
            king proton
            electron molecule
            Epoch 122/300, Loss: 0,3219154225529683
            atom queen
            king proton
            electron molecule
            Epoch 123: Loss did not decrease. Loss: 0,3219265218495231
            Epoch 123/300, Loss: 0,3219265218495231
            atom queen
            king proton
            electron molecule
            Epoch 124/300, Loss: 0,3219152621841537
            atom queen
            king proton
            electron molecule
            Epoch 125: Loss did not decrease. Loss: 0,3219254023795703
            Epoch 125/300, Loss: 0,3219254023795703
            atom queen
            king proton
            electron molecule
            Epoch 126/300, Loss: 0,32191512002490086
            atom queen
            king proton
            electron molecule
            Epoch 127: Loss did not decrease. Loss: 0,32192438379450916
            Epoch 127/300, Loss: 0,32192438379450916
            atom queen
            king proton
            electron molecule
            Epoch 128/300, Loss: 0,3219149940796504
            atom queen
            king proton
            electron molecule
            Epoch 129: Loss did not decrease. Loss: 0,3219234569853342
            Epoch 129/300, Loss: 0,3219234569853342
            atom queen
            king proton
            electron molecule
            Epoch 130/300, Loss: 0,321914882566449
            atom queen
            king proton
            electron molecule
            Epoch 131: Loss did not decrease. Loss: 0,3219226136684707
            Epoch 131/300, Loss: 0,3219226136684707
            atom queen
            king proton
            electron molecule
            Epoch 132/300, Loss: 0,3219147838944266
            atom queen
            king proton
            electron molecule
            Epoch 133: Loss did not decrease. Loss: 0,3219218463104715
            Epoch 133/300, Loss: 0,3219218463104715
            atom queen
            king proton
            electron molecule
            Epoch 134/300, Loss: 0,32191469664362987
            atom queen
            king proton
            electron molecule
            Epoch 135: Loss did not decrease. Loss: 0,3219211480596698
            Epoch 135/300, Loss: 0,3219211480596698
            atom queen
            king proton
            electron molecule
            Epoch 136/300, Loss: 0,32191461954696604
            atom queen
            king proton
            electron molecule
            Epoch 137: Loss did not decrease. Loss: 0,32192051268412974
            Epoch 137/300, Loss: 0,32192051268412974
            atom queen
            king proton
            electron molecule
            Epoch 138/300, Loss: 0,3219145514740375
            atom queen
            king proton
            electron molecule
            Epoch 139: Loss did not decrease. Loss: 0,3219199345153034
            Epoch 139/300, Loss: 0,3219199345153034
            atom queen
            king proton
            electron molecule
            Epoch 140/300, Loss: 0,32191449141666745
            atom queen
            king proton
            electron molecule
            Epoch 141: Loss did not decrease. Loss: 0,32191940839685956
            Epoch 141/300, Loss: 0,32191940839685956
            atom queen
            king proton
            electron molecule
            Epoch 142/300, Loss: 0,3219144384759414
            atom queen
            king proton
            electron molecule
            Epoch 143: Loss did not decrease. Loss: 0,3219189296382007
            Epoch 143/300, Loss: 0,3219189296382007
            atom queen
            king proton
            electron molecule
            Epoch 144/300, Loss: 0,3219143918506053
            atom queen
            king proton
            electron molecule
            Epoch 145: Loss did not decrease. Loss: 0,32191849397223155
            Epoch 145/300, Loss: 0,32191849397223155
            atom queen
            king proton
            electron molecule
            Epoch 146/300, Loss: 0,321914350826681
            atom queen
            king proton
            electron molecule
            Epoch 147: Loss did not decrease. Loss: 0,32191809751698613
            Epoch 147/300, Loss: 0,32191809751698613
            atom queen
            king proton
            electron molecule
            Epoch 148/300, Loss: 0,32191431476816884
            atom queen
            king proton
            electron molecule
            Epoch 149: Loss did not decrease. Loss: 0,3219177367407537
            Epoch 149/300, Loss: 0,3219177367407537
            atom queen
            king proton
            electron molecule
            Epoch 150/300, Loss: 0,32191428310872844
            atom queen
            king proton
            electron molecule
            Epoch 151: Loss did not decrease. Loss: 0,3219174084303826
            Epoch 151/300, Loss: 0,3219174084303826
            atom queen
            king proton
            electron molecule
            Epoch 152/300, Loss: 0,32191425534423157
            atom queen
            king proton
            electron molecule
            Epoch 153: Loss did not decrease. Loss: 0,3219171096624684
            Epoch 153/300, Loss: 0,3219171096624684
            atom queen
            king proton
            electron molecule
            Epoch 154/300, Loss: 0,3219142310260989
            atom queen
            king proton
            electron molecule
            Epoch 155: Loss did not decrease. Loss: 0,3219168377771608
            Epoch 155/300, Loss: 0,3219168377771608
            atom queen
            king proton
            electron molecule
            Epoch 156/300, Loss: 0,3219142097553383
            atom queen
            king proton
            electron molecule
            Epoch 157: Loss did not decrease. Loss: 0,3219165903543501
            Epoch 157/300, Loss: 0,3219165903543501
            atom queen
            king proton
            electron molecule
            Epoch 158/300, Loss: 0,32191419117721226
            atom queen
            king proton
            electron molecule
            Epoch 159: Loss did not decrease. Loss: 0,3219163651920132
            Epoch 159/300, Loss: 0,3219163651920132
            atom queen
            king proton
            electron molecule
            Epoch 160/300, Loss: 0,3219141749764667
            atom queen
            king proton
            electron molecule
            Epoch 161: Loss did not decrease. Loss: 0,32191616028652514
            Epoch 161/300, Loss: 0,32191616028652514
            atom queen
            king proton
            electron molecule
            Epoch 162/300, Loss: 0,3219141608730651
            atom queen
            king proton
            electron molecule
            Epoch 163: Loss did not decrease. Loss: 0,32191597381475173
            Epoch 163/300, Loss: 0,32191597381475173
            atom queen
            king proton
            electron molecule
            Epoch 164/300, Loss: 0,3219141486183749
            atom queen
            king proton
            electron molecule
            Epoch 165: Loss did not decrease. Loss: 0,3219158041177664
            Epoch 165/300, Loss: 0,3219158041177664
            atom queen
            king proton
            electron molecule
            Epoch 166/300, Loss: 0,32191413799175633
            atom queen
            king proton
            electron molecule
            Epoch 167: Loss did not decrease. Loss: 0,3219156496860378
            Epoch 167/300, Loss: 0,3219156496860378
            atom queen
            king proton
            electron molecule
            Epoch 168/300, Loss: 0,3219141287975154
            atom queen
            king proton
            electron molecule
            Epoch 169: Loss did not decrease. Loss: 0,3219155091459594
            Epoch 169/300, Loss: 0,3219155091459594
            atom queen
            king proton
            electron molecule
            Epoch 170/300, Loss: 0,3219141208621799
            atom queen
            king proton
            electron molecule
            Epoch 171: Loss did not decrease. Loss: 0,32191538124759583
            Epoch 171/300, Loss: 0,32191538124759583
            atom queen
            king proton
            electron molecule
            Epoch 172/300, Loss: 0,32191411403206693
            atom queen
            king proton
            electron molecule
            Epoch 173: Loss did not decrease. Loss: 0,3219152648535351
            Epoch 173/300, Loss: 0,3219152648535351
            atom queen
            king proton
            electron molecule
            Epoch 174/300, Loss: 0,3219141081711086
            atom queen
            king proton
            electron molecule
            Epoch 175: Loss did not decrease. Loss: 0,3219151589287501
            Epoch 175/300, Loss: 0,3219151589287501
            atom queen
            king proton
            electron molecule
            Epoch 176/300, Loss: 0,3219141031589124
            atom queen
            king proton
            electron molecule
            Epoch 177: Loss did not decrease. Loss: 0,32191506253137336
            Epoch 177/300, Loss: 0,32191506253137336
            atom queen
            king proton
            electron molecule
            Epoch 178/300, Loss: 0,3219140988890275
            atom queen
            king proton
            electron molecule
            Epoch 179: Loss did not decrease. Loss: 0,3219149748043054
            Epoch 179/300, Loss: 0,3219149748043054
            atom queen
            king proton
            electron molecule
            Epoch 180/300, Loss: 0,32191409526739984
            atom queen
            king proton
            electron molecule
            Epoch 181: Loss did not decrease. Loss: 0,32191489496758036
            Epoch 181/300, Loss: 0,32191489496758036
            atom queen
            king proton
            electron molecule
            Epoch 182/300, Loss: 0,3219140922109908
            atom queen
            king proton
            electron molecule
            Epoch 183: Loss did not decrease. Loss: 0,3219148223114186
            Epoch 183/300, Loss: 0,3219148223114186
            atom queen
            king proton
            electron molecule
            Epoch 184/300, Loss: 0,32191408964654794
            atom queen
            king proton
            electron molecule
            Epoch 185: Loss did not decrease. Loss: 0,3219147561899083
            Epoch 185/300, Loss: 0,3219147561899083
            atom queen
            king proton
            electron molecule
            Epoch 186/300, Loss: 0,3219140875095064
            atom queen
            king proton
            electron molecule
            Epoch 187: Loss did not decrease. Loss: 0,32191469601525446
            Epoch 187/300, Loss: 0,32191469601525446
            atom queen
            king proton
            electron molecule
            Epoch 188/300, Loss: 0,32191408574301134
            atom queen
            king proton
            electron molecule
            Epoch 189: Loss did not decrease. Loss: 0,32191464125254704
            Epoch 189/300, Loss: 0,32191464125254704
            atom queen
            king proton
            electron molecule
            Epoch 190/300, Loss: 0,32191408429704627
            atom queen
            king proton
            electron molecule
            Epoch 191: Loss did not decrease. Loss: 0,3219145914150012
            Epoch 191/300, Loss: 0,3219145914150012
            atom queen
            king proton
            electron molecule
            Epoch 192/300, Loss: 0,3219140831276553
            atom queen
            king proton
            electron molecule
            Epoch 193: Loss did not decrease. Loss: 0,3219145460596253
            Epoch 193/300, Loss: 0,3219145460596253
            atom queen
            king proton
            electron molecule
            Epoch 194/300, Loss: 0,3219140821962529
            atom queen
            king proton
            electron molecule
            Epoch 195: Loss did not decrease. Loss: 0,32191450478327926
            Epoch 195/300, Loss: 0,32191450478327926
            atom queen
            king proton
            electron molecule
            Epoch 196/300, Loss: 0,3219140814690071
            atom queen
            king proton
            electron molecule
            Epoch 197: Loss did not decrease. Loss: 0,3219144672190886
            Epoch 197/300, Loss: 0,3219144672190886
            atom queen
            king proton
            electron molecule
            Epoch 198/300, Loss: 0,3219140809162922
            atom queen
            king proton
            electron molecule
            Epoch 199: Loss did not decrease. Loss: 0,32191443303318157
            Epoch 199/300, Loss: 0,32191443303318157
            atom queen
            king proton
            electron molecule
            Epoch 200/300, Loss: 0,32191408051220155
            atom queen
            king proton
            electron molecule
            Epoch 201: Loss did not decrease. Loss: 0,32191440192171833
            Epoch 201/300, Loss: 0,32191440192171833
            atom queen
            king proton
            electron molecule
            Epoch 202/300, Loss: 0,32191408023411416
            atom queen
            king proton
            electron molecule
            Epoch 203: Loss did not decrease. Loss: 0,3219143736081894
            Epoch 203/300, Loss: 0,3219143736081894
            atom queen
            king proton
            electron molecule
            Epoch 204/300, Loss: 0,3219140800623093
            atom queen
            king proton
            electron molecule
            Epoch 205: Loss did not decrease. Loss: 0,3219143478409574
            Epoch 205/300, Loss: 0,3219143478409574
            atom queen
            king proton
            electron molecule
            Epoch 206/300, Loss: 0,3219140799796258
            atom queen
            king proton
            electron molecule
            Epoch 207: Loss did not decrease. Loss: 0,32191432439101797
            Epoch 207/300, Loss: 0,32191432439101797
            atom queen
            king proton
            electron molecule
            Epoch 208/300, Loss: 0,32191407997115606
            atom queen
            king proton
            electron molecule
            Epoch 209: Loss did not decrease. Loss: 0,3219143030499649
            Epoch 209/300, Loss: 0,3219143030499649
            atom queen
            king proton
            electron molecule
            Epoch 210/300, Loss: 0,32191408002397964
            atom queen
            king proton
            electron molecule
            Epoch 211: Loss did not decrease. Loss: 0,32191428362813584
            Epoch 211/300, Loss: 0,32191428362813584
            atom queen
            king proton
            electron molecule
            Epoch 212/300, Loss: 0,3219140801269216
            atom queen
            king proton
            electron molecule
            Epoch 213: Loss did not decrease. Loss: 0,32191426595292805
            Epoch 213/300, Loss: 0,32191426595292805
            atom queen
            king proton
            electron molecule
            Epoch 214/300, Loss: 0,32191408027034224
            atom queen
            king proton
            electron molecule
            Epoch 215: Loss did not decrease. Loss: 0,3219142498672614
            Epoch 215/300, Loss: 0,3219142498672614
            atom queen
            king proton
            electron molecule
            Epoch 216/300, Loss: 0,32191408044594905
            atom queen
            king proton
            electron molecule
            Epoch 217: Loss did not decrease. Loss: 0,3219142352281845
            Epoch 217/300, Loss: 0,3219142352281845
            atom queen
            king proton
            electron molecule
            Epoch 218/300, Loss: 0,3219140806466307
            atom queen
            king proton
            electron molecule
            Epoch 219: Loss did not decrease. Loss: 0,32191422190560176
            Epoch 219/300, Loss: 0,32191422190560176
            atom queen
            king proton
            electron molecule
            Epoch 220/300, Loss: 0,3219140808663095
            atom queen
            king proton
            electron molecule
            Epoch 221: Loss did not decrease. Loss: 0,32191420978111834
            Epoch 221/300, Loss: 0,32191420978111834
            atom queen
            king proton
            electron molecule
            Epoch 222/300, Loss: 0,3219140810998124
            atom queen
            king proton
            electron molecule
            Epoch 223: Loss did not decrease. Loss: 0,32191419874698735
            Epoch 223/300, Loss: 0,32191419874698735
            atom queen
            king proton
            electron molecule
            Epoch 224/300, Loss: 0,3219140813427556
            atom queen
            king proton
            electron molecule
            Epoch 225: Loss did not decrease. Loss: 0,3219141887051519
            Epoch 225/300, Loss: 0,3219141887051519
            atom queen
            king proton
            electron molecule
            Epoch 226/300, Loss: 0,32191408159144325
            atom queen
            king proton
            electron molecule
            Epoch 227: Loss did not decrease. Loss: 0,32191417956637425
            Epoch 227/300, Loss: 0,32191417956637425
            atom queen
            king proton
            electron molecule
            Epoch 228/300, Loss: 0,3219140818427783
            atom queen
            king proton
            electron molecule
            Epoch 229: Loss did not decrease. Loss: 0,32191417124944177
            Epoch 229/300, Loss: 0,32191417124944177
            atom queen
            king proton
            electron molecule
            Epoch 230/300, Loss: 0,3219140820941837
            atom queen
            king proton
            electron molecule
            Epoch 231: Loss did not decrease. Loss: 0,3219141636804452
            Epoch 231/300, Loss: 0,3219141636804452
            atom queen
            king proton
            electron molecule
            Epoch 232/300, Loss: 0,32191408234353297
            atom queen
            king proton
            electron molecule
            Epoch 233: Loss did not decrease. Loss: 0,32191415679212293
            Epoch 233/300, Loss: 0,32191415679212293
            atom queen
            king proton
            electron molecule
            Epoch 234/300, Loss: 0,32191408258908855
            atom queen
            king proton
            electron molecule
            Epoch 235: Loss did not decrease. Loss: 0,32191415052326183
            Epoch 235/300, Loss: 0,32191415052326183
            atom queen
            king proton
            electron molecule
            Epoch 236/300, Loss: 0,32191408282945017
            atom queen
            king proton
            electron molecule
            Epoch 237: Loss did not decrease. Loss: 0,3219141448181538
            Epoch 237/300, Loss: 0,3219141448181538
            atom queen
            king proton
            electron molecule
            Epoch 238/300, Loss: 0,3219140830635052
            atom queen
            king proton
            electron molecule
            Epoch 239: Loss did not decrease. Loss: 0,3219141396261007
            Epoch 239/300, Loss: 0,3219141396261007
            atom queen
            king proton
            electron molecule
            Epoch 240/300, Loss: 0,3219140832903897
            atom queen
            king proton
            electron molecule
            Epoch 241: Loss did not decrease. Loss: 0,3219141349009637
            Epoch 241/300, Loss: 0,3219141349009637
            atom queen
            king proton
            electron molecule
            Epoch 242/300, Loss: 0,3219140835094505
            atom queen
            king proton
            electron molecule
            Epoch 243: Loss did not decrease. Loss: 0,3219141306007534
            Epoch 243/300, Loss: 0,3219141306007534
            atom queen
            king proton
            electron molecule
            Epoch 244/300, Loss: 0,3219140837202152
            atom queen
            king proton
            electron molecule
            Epoch 245: Loss did not decrease. Loss: 0,32191412668725666
            Epoch 245/300, Loss: 0,32191412668725666
            atom queen
            king proton
            electron molecule
            Epoch 246/300, Loss: 0,3219140839223637
            atom queen
            king proton
            electron molecule
            Epoch 247: Loss did not decrease. Loss: 0,32191412312569595
            Epoch 247/300, Loss: 0,32191412312569595
            atom queen
            king proton
            electron molecule
            Epoch 248/300, Loss: 0,32191408411570416
            atom queen
            king proton
            electron molecule
            Epoch 249: Loss did not decrease. Loss: 0,32191411988442264
            Epoch 249/300, Loss: 0,32191411988442264
            atom queen
            king proton
            electron molecule
            Epoch 250/300, Loss: 0,32191408430015267
            atom queen
            king proton
            electron molecule
            Epoch 251: Loss did not decrease. Loss: 0,3219141169346333
            Epoch 251/300, Loss: 0,3219141169346333
            atom queen
            king proton
            electron molecule
            Epoch 252/300, Loss: 0,3219140844757147
            atom queen
            king proton
            electron molecule
            Epoch 253: Loss did not decrease. Loss: 0,32191411425011496
            Epoch 253/300, Loss: 0,32191411425011496
            atom queen
            king proton
            electron molecule
            Epoch 254/300, Loss: 0,32191408464246946
            atom queen
            king proton
            electron molecule
            Epoch 255: Loss did not decrease. Loss: 0,32191411180701207
            Epoch 255/300, Loss: 0,32191411180701207
            atom queen
            king proton
            electron molecule
            Epoch 256/300, Loss: 0,3219140848005564
            atom queen
            king proton
            electron molecule
            Epoch 257: Loss did not decrease. Loss: 0,32191410958361466
            Epoch 257/300, Loss: 0,32191410958361466
            atom queen
            king proton
            electron molecule
            Epoch 258/300, Loss: 0,3219140849501634
            atom queen
            king proton
            electron molecule
            Epoch 259: Loss did not decrease. Loss: 0,32191410756016464
            Epoch 259/300, Loss: 0,32191410756016464
            atom queen
            king proton
            electron molecule
            Epoch 260/300, Loss: 0,3219140850915169
            atom queen
            king proton
            electron molecule
            Epoch 261: Loss did not decrease. Loss: 0,321914105718681
            Epoch 261/300, Loss: 0,321914105718681
            atom queen
            king proton
            electron molecule
            Epoch 262/300, Loss: 0,3219140852248728
            atom queen
            king proton
            electron molecule
            Epoch 263: Loss did not decrease. Loss: 0,3219141040427996
            Epoch 263/300, Loss: 0,3219141040427996
            atom queen
            king proton
            electron molecule
            Epoch 264/300, Loss: 0,32191408535050997
            atom queen
            king proton
            electron molecule
            Epoch 265: Loss did not decrease. Loss: 0,32191410251762814
            Epoch 265/300, Loss: 0,32191410251762814
            atom queen
            king proton
            electron molecule
            Epoch 266/300, Loss: 0,32191408546872263
            atom queen
            king proton
            electron molecule
            Epoch 267: Loss did not decrease. Loss: 0,32191410112961355
            Epoch 267/300, Loss: 0,32191410112961355
            atom queen
            king proton
            electron molecule
            Epoch 268/300, Loss: 0,3219140855798169
            atom queen
            king proton
            electron molecule
            Epoch 269: Loss did not decrease. Loss: 0,3219140998664213
            Epoch 269/300, Loss: 0,3219140998664213
            atom queen
            king proton
            electron molecule
            Epoch 270/300, Loss: 0,32191408568410435
            atom queen
            king proton
            electron molecule
            Epoch 271: Loss did not decrease. Loss: 0,3219140987168264
            Epoch 271/300, Loss: 0,3219140987168264
            atom queen
            king proton
            electron molecule
            Epoch 272/300, Loss: 0,3219140857818996
            atom queen
            king proton
            electron molecule
            Epoch 273: Loss did not decrease. Loss: 0,321914097670613
            Epoch 273/300, Loss: 0,321914097670613
            atom queen
            king proton
            electron molecule
            Epoch 274/300, Loss: 0,3219140858735167
            atom queen
            king proton
            electron molecule
            Epoch 275: Loss did not decrease. Loss: 0,3219140967184843
            Epoch 275/300, Loss: 0,3219140967184843
            atom queen
            king proton
            electron molecule
            Epoch 276/300, Loss: 0,3219140859592668
            atom queen
            king proton
            electron molecule
            Epoch 277: Loss did not decrease. Loss: 0,3219140958519792
            Epoch 277/300, Loss: 0,3219140958519792
            atom queen
            king proton
            electron molecule
            Epoch 278/300, Loss: 0,32191408603945587
            atom queen
            king proton
            electron molecule
            Epoch 279: Loss did not decrease. Loss: 0,321914095063398
            Epoch 279/300, Loss: 0,321914095063398
            atom queen
            king proton
            electron molecule
            Epoch 280/300, Loss: 0,3219140861143829
            atom queen
            king proton
            electron molecule
            Epoch 281: Loss did not decrease. Loss: 0,32191409434573265
            Epoch 281/300, Loss: 0,32191409434573265
            atom queen
            king proton
            electron molecule
            Epoch 282/300, Loss: 0,32191408618433903
            atom queen
            king proton
            electron molecule
            Epoch 283: Loss did not decrease. Loss: 0,3219140936926061
            Epoch 283/300, Loss: 0,3219140936926061
            atom queen
            king proton
            electron molecule
            Epoch 284/300, Loss: 0,3219140862496061
            atom queen
            king proton
            electron molecule
            Epoch 285: Loss did not decrease. Loss: 0,32191409309821445
            Epoch 285/300, Loss: 0,32191409309821445
            atom queen
            king proton
            electron molecule
            Epoch 286/300, Loss: 0,32191408631045604
            atom queen
            king proton
            electron molecule
            Epoch 287: Loss did not decrease. Loss: 0,3219140925572755
            Epoch 287/300, Loss: 0,3219140925572755
            atom queen
            king proton
            electron molecule
            Epoch 288/300, Loss: 0,32191408636715074
            atom queen
            king proton
            electron molecule
            Epoch 289: Loss did not decrease. Loss: 0,32191409206498256
            Epoch 289/300, Loss: 0,32191409206498256
            atom queen
            king proton
            electron molecule
            Epoch 290/300, Loss: 0,3219140864199404
            atom queen
            king proton
            electron molecule
            Epoch 291: Loss did not decrease. Loss: 0,3219140916169607
            Epoch 291/300, Loss: 0,3219140916169607
            atom queen
            king proton
            electron molecule
            Epoch 292/300, Loss: 0,321914086469065
            atom queen
            king proton
            electron molecule
            Epoch 293: Loss did not decrease. Loss: 0,32191409120922904
            Epoch 293/300, Loss: 0,32191409120922904
            atom queen
            king proton
            electron molecule
            Epoch 294/300, Loss: 0,3219140865147527
            atom queen
            king proton
            electron molecule
            Epoch 295: Loss did not decrease. Loss: 0,32191409083816397
            Epoch 295/300, Loss: 0,32191409083816397
            atom queen
            king proton
            electron molecule
            Epoch 296/300, Loss: 0,32191408655722115
            atom queen
            king proton
            electron molecule
            Epoch 297: Loss did not decrease. Loss: 0,32191409050046826
            Epoch 297/300, Loss: 0,32191409050046826
            atom queen
            king proton
            electron molecule
            Epoch 298/300, Loss: 0,32191408659667675
            atom queen
            king proton
            electron molecule
            Epoch 299: Loss did not decrease. Loss: 0,32191409019314104
            Epoch 299/300, Loss: 0,32191409019314104
            atom queen
            king proton
            electron molecule
            Epoch 300/300, Loss: 0,32191408663331506
            */


            /*
            atom ksi
            king bacolod
            electron supermen
            Epoch 1/300, Loss: 5,021179606661744
            atom municipality
            king strangeland
            electron molecule
            Epoch 2: Loss did not decrease. Loss: 8,38466597375682
            Epoch 2/300, Loss: 8,38466597375682
            atom queen
            king proton
            electron molecule
            Epoch 3/300, Loss: 0,374039619459938
            atom queen
            king proton
            electron molecule
            Epoch 4/300, Loss: 0,3248218078755969
            atom queen
            king proton
            electron molecule
            Epoch 5/300, Loss: 0,3225147650688526
            atom queen
            king proton
            electron molecule
            Epoch 6/300, Loss: 0,3221682235170677
            atom queen
            king proton
            electron molecule
            Epoch 7/300, Loss: 0,3219582759467434
            atom queen
            king proton
            electron molecule
            Epoch 8: Loss did not decrease. Loss: 0,32202474207382176
            Epoch 8/300, Loss: 0,32202474207382176
            atom queen
            king proton
            electron molecule
            Epoch 9/300, Loss: 0,32191875182962265
            atom queen
            king proton
            electron molecule
            Epoch 10: Loss did not decrease. Loss: 0,32200318189918825
            Epoch 10/300, Loss: 0,32200318189918825
            atom queen
            king proton
            electron molecule
            Epoch 11/300, Loss: 0,32191428748206735
            atom queen
            king proton
            electron molecule
            Epoch 12: Loss did not decrease. Loss: 0,3219934333531757
            Epoch 12/300, Loss: 0,3219934333531757
            atom queen
            king proton
            electron molecule
            Epoch 13/300, Loss: 0,321913454074467
            atom queen
            king proton
            electron molecule
            Epoch 14: Loss did not decrease. Loss: 0,3219856544866232
            Epoch 14/300, Loss: 0,3219856544866232
            atom queen
            king proton
            electron molecule
            Epoch 15/300, Loss: 0,3219130789808449
            atom queen
            king proton
            electron molecule
            Epoch 16: Loss did not decrease. Loss: 0,3219787376104933
            Epoch 16/300, Loss: 0,3219787376104933
            atom queen
            king proton
            electron molecule
            Epoch 17/300, Loss: 0,3219128124153772
            atom queen
            king proton
            electron molecule
            Epoch 18: Loss did not decrease. Loss: 0,3219725005221619
            Epoch 18/300, Loss: 0,3219725005221619
            atom queen
            king proton
            electron molecule
            Epoch 19/300, Loss: 0,3219126079772178
            atom queen
            king proton
            electron molecule
            Epoch 20: Loss did not decrease. Loss: 0,32196686679735187
            Epoch 20/300, Loss: 0,32196686679735187
            atom queen
            king proton
            electron molecule
            Epoch 21/300, Loss: 0,32191245312385314
            atom queen
            king proton
            electron molecule
            Epoch 22: Loss did not decrease. Loss: 0,3219617769455288
            Epoch 22/300, Loss: 0,3219617769455288
            atom queen
            king proton
            electron molecule
            Epoch 23/300, Loss: 0,32191233996157553
            atom queen
            king proton
            electron molecule
            Epoch 24: Loss did not decrease. Loss: 0,3219571782776033
            Epoch 24/300, Loss: 0,3219571782776033
            atom queen
            king proton
            electron molecule
            Epoch 25/300, Loss: 0,32191226196652606
            atom queen
            king proton
            electron molecule
            Epoch 26: Loss did not decrease. Loss: 0,32195302332463815
            Epoch 26/300, Loss: 0,32195302332463815
            atom queen
            king proton
            electron molecule
            Epoch 27/300, Loss: 0,3219122135206827
            atom queen
            king proton
            electron molecule
            Epoch 28: Loss did not decrease. Loss: 0,3219492692286392
            Epoch 28/300, Loss: 0,3219492692286392
            atom queen
            king proton
            electron molecule
            Epoch 29/300, Loss: 0,3219121897657871
            atom queen
            king proton
            electron molecule
            Epoch 30: Loss did not decrease. Loss: 0,3219458772820147
            Epoch 30/300, Loss: 0,3219458772820147
            atom queen
            king proton
            electron molecule
            Epoch 31/300, Loss: 0,32191218650378906
            atom queen
            king proton
            electron molecule
            Epoch 32: Loss did not decrease. Loss: 0,32194281252224516
            Epoch 32/300, Loss: 0,32194281252224516
            atom queen
            king proton
            electron molecule
            Epoch 33/300, Loss: 0,3219122001128752
            atom queen
            king proton
            electron molecule
            Epoch 34: Loss did not decrease. Loss: 0,3219400433676382
            Epoch 34/300, Loss: 0,3219400433676382
            atom queen
            king proton
            electron molecule
            Epoch 35/300, Loss: 0,32191222747435394
            atom queen
            king proton
            electron molecule
            Epoch 36: Loss did not decrease. Loss: 0,32193754128905316
            Epoch 36/300, Loss: 0,32193754128905316
            atom queen
            king proton
            electron molecule
            Epoch 37/300, Loss: 0,3219122659086865
            atom queen
            king proton
            electron molecule
            Epoch 38: Loss did not decrease. Loss: 0,32193528051384734
            Epoch 38/300, Loss: 0,32193528051384734
            atom queen
            king proton
            electron molecule
            Epoch 39/300, Loss: 0,3219123131194603
            atom queen
            king proton
            electron molecule
            Epoch 40: Loss did not decrease. Loss: 0,3219332377587914
            Epoch 40/300, Loss: 0,3219332377587914
            atom queen
            king proton
            electron molecule
            Epoch 41/300, Loss: 0,32191236714428867
            atom queen
            king proton
            electron molecule
            Epoch 42: Loss did not decrease. Loss: 0,32193139198905446
            Epoch 42/300, Loss: 0,32193139198905446
            atom queen
            king proton
            electron molecule
            Epoch 43/300, Loss: 0,321912426311767
            atom queen
            king proton
            electron molecule
            Epoch 44: Loss did not decrease. Loss: 0,3219297242006676
            Epoch 44/300, Loss: 0,3219297242006676
            atom queen
            king proton
            electron molecule
            Epoch 45/300, Loss: 0,3219124892037292
            atom queen
            king proton
            electron molecule
            Epoch 46: Loss did not decrease. Loss: 0,3219282172241413
            Epoch 46/300, Loss: 0,3219282172241413
            atom queen
            king proton
            electron molecule
            Epoch 47/300, Loss: 0,3219125546221524
            atom queen
            king proton
            electron molecule
            Epoch 48: Loss did not decrease. Loss: 0,321926855547153
            Epoch 48/300, Loss: 0,321926855547153
            atom queen
            king proton
            electron molecule
            Epoch 49/300, Loss: 0,32191262156013645
            atom queen
            king proton
            electron molecule
            Epoch 50: Loss did not decrease. Loss: 0,32192562515443207
            Epoch 50/300, Loss: 0,32192562515443207
            atom queen
            king proton
            electron molecule
            Epoch 51/300, Loss: 0,32191268917646393
            atom queen
            king proton
            electron molecule
            Epoch 52: Loss did not decrease. Loss: 0,32192451338315864
            Epoch 52/300, Loss: 0,32192451338315864
            atom queen
            king proton
            electron molecule
            Epoch 53/300, Loss: 0,32191275677330405
            atom queen
            king proton
            electron molecule
            Epoch 54: Loss did not decrease. Loss: 0,3219235087923618
            Epoch 54/300, Loss: 0,3219235087923618
            atom queen
            king proton
            electron molecule
            Epoch 55/300, Loss: 0,32191282377668257
            atom queen
            king proton
            electron molecule
            Epoch 56: Loss did not decrease. Loss: 0,3219226010449523
            Epoch 56/300, Loss: 0,3219226010449523
            atom queen
            king proton
            electron molecule
            Epoch 57/300, Loss: 0,321912889719383
            atom queen
            king proton
            electron molecule
            Epoch 58: Loss did not decrease. Loss: 0,32192178080116224
            Epoch 58/300, Loss: 0,32192178080116224
            atom queen
            king proton
            electron molecule
            Epoch 59/300, Loss: 0,32191295422598526
            atom queen
            king proton
            electron molecule
            Epoch 60: Loss did not decrease. Loss: 0,32192103962228363
            Epoch 60/300, Loss: 0,32192103962228363
            atom queen
            king proton
            electron molecule
            Epoch 61/300, Loss: 0,3219130169997891
            atom queen
            king proton
            electron molecule
            Epoch 62: Loss did not decrease. Loss: 0,3219203698837058
            Epoch 62/300, Loss: 0,3219203698837058
            atom queen
            king proton
            electron molecule
            Epoch 63/300, Loss: 0,32191307781139017
            atom queen
            king proton
            electron molecule
            Epoch 64: Loss did not decrease. Loss: 0,32191976469635325
            Epoch 64/300, Loss: 0,32191976469635325
            atom queen
            king proton
            electron molecule
            Epoch 65/300, Loss: 0,3219131364887144
            atom queen
            king proton
            electron molecule
            Epoch 66: Loss did not decrease. Loss: 0,3219192178357085
            Epoch 66/300, Loss: 0,3219192178357085
            atom queen
            king proton
            electron molecule
            Epoch 67/300, Loss: 0,32191319290833365
            atom queen
            king proton
            electron molecule
            Epoch 68: Loss did not decrease. Loss: 0,3219187236776871
            Epoch 68/300, Loss: 0,3219187236776871
            atom queen
            king proton
            electron molecule
            Epoch 69/300, Loss: 0,3219132469879056
            atom queen
            king proton
            electron molecule
            Epoch 70: Loss did not decrease. Loss: 0,3219182771407035
            Epoch 70/300, Loss: 0,3219182771407035
            atom queen
            king proton
            electron molecule
            Epoch 71/300, Loss: 0,3219132986796069
            atom queen
            king proton
            electron molecule
            Epoch 72: Loss did not decrease. Loss: 0,32191787363332663
            Epoch 72/300, Loss: 0,32191787363332663
            atom queen
            king proton
            electron molecule
            Epoch 73/300, Loss: 0,32191334796443455
            atom queen
            king proton
            electron molecule
            Epoch 74: Loss did not decrease. Loss: 0,32191750900698995
            Epoch 74/300, Loss: 0,32191750900698995
            atom queen
            king proton
            electron molecule
            Epoch 75/300, Loss: 0,3219133948472698
            atom queen
            king proton
            electron molecule
            Epoch 76: Loss did not decrease. Loss: 0,321917179513262
            Epoch 76/300, Loss: 0,321917179513262
            atom queen
            king proton
            electron molecule
            Epoch 77/300, Loss: 0,32191343935261546
            atom queen
            king proton
            electron molecule
            Epoch 78: Loss did not decrease. Loss: 0,3219168817652424
            Epoch 78/300, Loss: 0,3219168817652424
            atom queen
            king proton
            electron molecule
            Epoch 79/300, Loss: 0,321913481520917
            atom queen
            king proton
            electron molecule
            Epoch 80: Loss did not decrease. Loss: 0,32191661270268207
            Epoch 80/300, Loss: 0,32191661270268207
            atom queen
            king proton
            electron molecule
            Epoch 81/300, Loss: 0,32191352140540086
            atom queen
            king proton
            electron molecule
            Epoch 82: Loss did not decrease. Loss: 0,32191636956046993
            Epoch 82/300, Loss: 0,32191636956046993
            atom queen
            king proton
            electron molecule
            Epoch 83/300, Loss: 0,3219135590693621
            atom queen
            king proton
            electron molecule
            Epoch 84: Loss did not decrease. Loss: 0,3219161498401588
            Epoch 84/300, Loss: 0,3219161498401588
            atom queen
            king proton
            electron molecule
            Epoch 85/300, Loss: 0,3219135945838468
            atom queen
            king proton
            electron molecule
            Epoch 86: Loss did not decrease. Loss: 0,3219159512842406
            Epoch 86/300, Loss: 0,3219159512842406
            atom queen
            king proton
            electron molecule
            Epoch 87/300, Loss: 0,3219136280256761
            atom queen
            king proton
            electron molecule
            Epoch 88: Loss did not decrease. Loss: 0,32191577185290216
            Epoch 88/300, Loss: 0,32191577185290216
            atom queen
            king proton
            electron molecule
            Epoch 89/300, Loss: 0,32191365947577194
            atom queen
            king proton
            electron molecule
            Epoch 90: Loss did not decrease. Loss: 0,3219156097030253
            Epoch 90/300, Loss: 0,3219156097030253
            atom queen
            king proton
            electron molecule
            Epoch 91/300, Loss: 0,3219136890177434
            atom queen
            king proton
            electron molecule
            Epoch 92: Loss did not decrease. Loss: 0,321915463169212
            Epoch 92/300, Loss: 0,321915463169212
            atom queen
            king proton
            electron molecule
            Epoch 93/300, Loss: 0,3219137167366993
            atom queen
            king proton
            electron molecule
            Epoch 94: Loss did not decrease. Loss: 0,32191533074664097
            Epoch 94/300, Loss: 0,32191533074664097
            atom queen
            king proton
            electron molecule
            Epoch 95/300, Loss: 0,3219137427182582
            atom queen
            king proton
            electron molecule
            Epoch 96: Loss did not decrease. Loss: 0,3219152110755786
            Epoch 96/300, Loss: 0,3219152110755786
            atom queen
            king proton
            electron molecule
            Epoch 97/300, Loss: 0,3219137670477288
            atom queen
            king proton
            electron molecule
            Epoch 98: Loss did not decrease. Loss: 0,3219151029273836
            Epoch 98/300, Loss: 0,3219151029273836
            atom queen
            king proton
            electron molecule
            Epoch 99/300, Loss: 0,32191378980943725
            atom queen
            king proton
            electron molecule
            Epoch 100: Loss did not decrease. Loss: 0,3219150051918631
            Epoch 100/300, Loss: 0,3219150051918631
            atom queen
            king proton
            electron molecule
            Epoch 101/300, Loss: 0,3219138110861836
            atom queen
            king proton
            electron molecule
            Epoch 102: Loss did not decrease. Loss: 0,3219149168658479
            Epoch 102/300, Loss: 0,3219149168658479
            atom queen
            king proton
            electron molecule
            Epoch 103/300, Loss: 0,32191383095880327
            atom queen
            king proton
            electron molecule
            Epoch 104: Loss did not decrease. Loss: 0,32191483704287
            Epoch 104/300, Loss: 0,32191483704287
            atom queen
            king proton
            electron molecule
            Epoch 105/300, Loss: 0,32191384950582713
            atom queen
            king proton
            electron molecule
            Epoch 106: Loss did not decrease. Loss: 0,321914764903837
            Epoch 106/300, Loss: 0,321914764903837
            atom queen
            king proton
            electron molecule
            Epoch 107/300, Loss: 0,3219138668032186
            atom queen
            king proton
            electron molecule
            Epoch 108: Loss did not decrease. Loss: 0,32191469970860503
            Epoch 108/300, Loss: 0,32191469970860503
            atom queen
            king proton
            electron molecule
            Epoch 109/300, Loss: 0,3219138829241792
            atom queen
            king proton
            electron molecule
            Epoch 110: Loss did not decrease. Loss: 0,3219146407883654
            Epoch 110/300, Loss: 0,3219146407883654
            atom queen
            king proton
            electron molecule
            Epoch 111/300, Loss: 0,32191389793901265
            atom queen
            king proton
            electron molecule
            Epoch 112: Loss did not decrease. Loss: 0,32191458753876595
            Epoch 112/300, Loss: 0,32191458753876595
            atom queen
            king proton
            electron molecule
            Epoch 113/300, Loss: 0,32191391191503804
            atom queen
            king proton
            electron molecule
            Epoch 114: Loss did not decrease. Loss: 0,3219145394136951
            Epoch 114/300, Loss: 0,3219145394136951
            atom queen
            king proton
            electron molecule
            Epoch 115/300, Loss: 0,32191392491654164
            atom queen
            king proton
            electron molecule
            Epoch 116: Loss did not decrease. Loss: 0,32191449591966714
            Epoch 116/300, Loss: 0,32191449591966714
            atom queen
            king proton
            electron molecule
            Epoch 117/300, Loss: 0,3219139370047663
            atom queen
            king proton
            electron molecule
            Epoch 118: Loss did not decrease. Loss: 0,32191445661074697
            Epoch 118/300, Loss: 0,32191445661074697
            atom queen
            king proton
            electron molecule
            Epoch 119/300, Loss: 0,3219139482379251
            atom queen
            king proton
            electron molecule
            Epoch 120: Loss did not decrease. Loss: 0,321914421083967
            Epoch 120/300, Loss: 0,321914421083967
            atom queen
            king proton
            electron molecule
            Epoch 121/300, Loss: 0,32191395867124145
            atom queen
            king proton
            electron molecule
            Epoch 122: Loss did not decrease. Loss: 0,32191438897518326
            Epoch 122/300, Loss: 0,32191438897518326
            atom queen
            king proton
            electron molecule
            Epoch 123/300, Loss: 0,3219139683570061
            atom queen
            king proton
            electron molecule
            Epoch 124: Loss did not decrease. Loss: 0,3219143599553341
            Epoch 124/300, Loss: 0,3219143599553341
            atom queen
            king proton
            electron molecule
            Epoch 125/300, Loss: 0,3219139773446484
            atom queen
            king proton
            electron molecule
            Epoch 126: Loss did not decrease. Loss: 0,321914333727057
            Epoch 126/300, Loss: 0,321914333727057
            atom queen
            king proton
            electron molecule
            Epoch 127/300, Loss: 0,32191398568082125
            atom queen
            king proton
            electron molecule
            Epoch 128: Loss did not decrease. Loss: 0,3219143100216347
            Epoch 128/300, Loss: 0,3219143100216347
            atom queen
            king proton
            electron molecule
            Epoch 129/300, Loss: 0,32191399340949317
            atom queen
            king proton
            electron molecule
            Epoch 130: Loss did not decrease. Loss: 0,3219142885962329
            Epoch 130/300, Loss: 0,3219142885962329
            atom queen
            king proton
            electron molecule
            Epoch 131/300, Loss: 0,32191400057204783
            atom queen
            king proton
            electron molecule
            Epoch 132: Loss did not decrease. Loss: 0,32191426923140587
            Epoch 132/300, Loss: 0,32191426923140587
            atom queen
            king proton
            electron molecule
            Epoch 133/300, Loss: 0,3219140072073871
            atom queen
            king proton
            electron molecule
            Epoch 134: Loss did not decrease. Loss: 0,32191425172884297
            Epoch 134/300, Loss: 0,32191425172884297
            atom queen
            king proton
            electron molecule
            Epoch 135/300, Loss: 0,3219140133520386
            atom queen
            king proton
            electron molecule
            Epoch 136: Loss did not decrease. Loss: 0,3219142359093313
            Epoch 136/300, Loss: 0,3219142359093313
            atom queen
            king proton
            electron molecule
            Epoch 137/300, Loss: 0,3219140190402628
            atom queen
            king proton
            electron molecule
            Epoch 138: Loss did not decrease. Loss: 0,32191422161091565
            Epoch 138/300, Loss: 0,32191422161091565
            atom queen
            king proton
            electron molecule
            Epoch 139/300, Loss: 0,3219140243041612
            atom queen
            king proton
            electron molecule
            Epoch 140: Loss did not decrease. Loss: 0,3219142086872359
            Epoch 140/300, Loss: 0,3219142086872359
            atom queen
            king proton
            electron molecule
            Epoch 141/300, Loss: 0,3219140291737848
            atom queen
            king proton
            electron molecule
            Epoch 142: Loss did not decrease. Loss: 0,32191419700602325
            Epoch 142/300, Loss: 0,32191419700602325
            atom queen
            king proton
            electron molecule
            Epoch 143/300, Loss: 0,3219140336772394
            atom queen
            king proton
            electron molecule
            Epoch 144: Loss did not decrease. Loss: 0,32191418644774417
            Epoch 144/300, Loss: 0,32191418644774417
            atom queen
            king proton
            electron molecule
            Epoch 145/300, Loss: 0,3219140378407889
            atom queen
            king proton
            electron molecule
            Epoch 146: Loss did not decrease. Loss: 0,3219141769043728
            Epoch 146/300, Loss: 0,3219141769043728
            atom queen
            king proton
            electron molecule
            Epoch 147/300, Loss: 0,32191404168895815
            atom queen
            king proton
            electron molecule
            Epoch 148: Loss did not decrease. Loss: 0,32191416827828184
            Epoch 148/300, Loss: 0,32191416827828184
            atom queen
            king proton
            electron molecule
            Epoch 149/300, Loss: 0,32191404524462947
            atom queen
            king proton
            electron molecule
            Epoch 150: Loss did not decrease. Loss: 0,32191416048124166
            Epoch 150/300, Loss: 0,32191416048124166
            atom queen
            king proton
            electron molecule
            Epoch 151/300, Loss: 0,3219140485291385
            atom queen
            king proton
            electron molecule
            Epoch 152: Loss did not decrease. Loss: 0,321914153433515
            Epoch 152/300, Loss: 0,321914153433515
            atom queen
            king proton
            electron molecule
            Epoch 153/300, Loss: 0,32191405156236513
            atom queen
            king proton
            electron molecule
            Epoch 154: Loss did not decrease. Loss: 0,3219141470630389
            Epoch 154/300, Loss: 0,3219141470630389
            atom queen
            king proton
            electron molecule
            Epoch 155/300, Loss: 0,32191405436282094
            atom queen
            king proton
            electron molecule
            Epoch 156: Loss did not decrease. Loss: 0,3219141413046851
            Epoch 156/300, Loss: 0,3219141413046851
            atom queen
            king proton
            electron molecule
            Epoch 157/300, Loss: 0,3219140569477335
            atom queen
            king proton
            electron molecule
            Epoch 158: Loss did not decrease. Loss: 0,32191413609959335
            Epoch 158/300, Loss: 0,32191413609959335
            atom queen
            king proton
            electron molecule
            Epoch 159/300, Loss: 0,3219140593331257
            atom queen
            king proton
            electron molecule
            Epoch 160: Loss did not decrease. Loss: 0,3219141313945671
            Epoch 160/300, Loss: 0,3219141313945671
            atom queen
            king proton
            electron molecule
            Epoch 161/300, Loss: 0,3219140615338922
            atom queen
            king proton
            electron molecule
            Epoch 162: Loss did not decrease. Loss: 0,3219141271415281
            Epoch 162/300, Loss: 0,3219141271415281
            atom queen
            king proton
            electron molecule
            Epoch 163/300, Loss: 0,32191406356387126
            atom queen
            king proton
            electron molecule
            Epoch 164: Loss did not decrease. Loss: 0,3219141232970238
            Epoch 164/300, Loss: 0,3219141232970238
            atom queen
            king proton
            electron molecule
            Epoch 165/300, Loss: 0,3219140654359138
            atom queen
            king proton
            electron molecule
            Epoch 166: Loss did not decrease. Loss: 0,3219141198217821
            Epoch 166/300, Loss: 0,3219141198217821
            atom queen
            king proton
            electron molecule
            Epoch 167/300, Loss: 0,3219140671619485
            atom queen
            king proton
            electron molecule
            Epoch 168: Loss did not decrease. Loss: 0,32191411668030845
            Epoch 168/300, Loss: 0,32191411668030845
            atom queen
            king proton
            electron molecule
            Epoch 169/300, Loss: 0,32191406875304157
            atom queen
            king proton
            electron molecule
            Epoch 170: Loss did not decrease. Loss: 0,3219141138405231
            Epoch 170/300, Loss: 0,3219141138405231
            atom queen
            king proton
            electron molecule
            Epoch 171/300, Loss: 0,3219140702194561
            atom queen
            king proton
            electron molecule
            Epoch 172: Loss did not decrease. Loss: 0,32191411127343145
            Epoch 172/300, Loss: 0,32191411127343145
            atom queen
            king proton
            electron molecule
            Epoch 173/300, Loss: 0,3219140715707058
            atom queen
            king proton
            electron molecule
            Epoch 174: Loss did not decrease. Loss: 0,32191410895282724
            Epoch 174/300, Loss: 0,32191410895282724
            atom queen
            king proton
            electron molecule
            Epoch 175/300, Loss: 0,32191407281560613
            atom queen
            king proton
            electron molecule
            Epoch 176: Loss did not decrease. Loss: 0,3219141068550255
            Epoch 176/300, Loss: 0,3219141068550255
            atom queen
            king proton
            electron molecule
            Epoch 177/300, Loss: 0,32191407396232263
            atom queen
            king proton
            electron molecule
            Epoch 178: Loss did not decrease. Loss: 0,3219141049586175
            Epoch 178/300, Loss: 0,3219141049586175
            atom queen
            king proton
            electron molecule
            Epoch 179/300, Loss: 0,32191407501841646
            atom queen
            king proton
            electron molecule
            Epoch 180: Loss did not decrease. Loss: 0,3219141032442541
            Epoch 180/300, Loss: 0,3219141032442541
            atom queen
            king proton
            electron molecule
            Epoch 181/300, Loss: 0,32191407599088556
            atom queen
            king proton
            electron molecule
            Epoch 182: Loss did not decrease. Loss: 0,32191410169444645
            Epoch 182/300, Loss: 0,32191410169444645
            atom queen
            king proton
            electron molecule
            Epoch 183/300, Loss: 0,32191407688620544
            atom queen
            king proton
            electron molecule
            Epoch 184: Loss did not decrease. Loss: 0,3219141002933867
            Epoch 184/300, Loss: 0,3219141002933867
            atom queen
            king proton
            electron molecule
            Epoch 185/300, Loss: 0,3219140777103661
            atom queen
            king proton
            electron molecule
            Epoch 186: Loss did not decrease. Loss: 0,32191409902678664
            Epoch 186/300, Loss: 0,32191409902678664
            atom queen
            king proton
            electron molecule
            Epoch 187/300, Loss: 0,321914078468906
            atom queen
            king proton
            electron molecule
            Epoch 188: Loss did not decrease. Loss: 0,321914097881732
            Epoch 188/300, Loss: 0,321914097881732
            atom queen
            king proton
            electron molecule
            Epoch 189/300, Loss: 0,32191407916694564
            atom queen
            king proton
            electron molecule
            Epoch 190: Loss did not decrease. Loss: 0,3219140968465494
            Epoch 190/300, Loss: 0,3219140968465494
            atom queen
            king proton
            electron molecule
            Epoch 191/300, Loss: 0,32191407980921655
            atom queen
            king proton
            electron molecule
            Epoch 192: Loss did not decrease. Loss: 0,3219140959106881
            Epoch 192/300, Loss: 0,3219140959106881
            atom queen
            king proton
            electron molecule
            Epoch 193/300, Loss: 0,3219140804000908
            atom queen
            king proton
            electron molecule
            Epoch 194: Loss did not decrease. Loss: 0,3219140950646104
            Epoch 194/300, Loss: 0,3219140950646104
            atom queen
            king proton
            electron molecule
            Epoch 195/300, Loss: 0,32191408094360635
            atom queen
            king proton
            electron molecule
            Epoch 196: Loss did not decrease. Loss: 0,3219140942996958
            Epoch 196/300, Loss: 0,3219140942996958
            atom queen
            king proton
            electron molecule
            Epoch 197/300, Loss: 0,3219140814434919
            atom queen
            king proton
            electron molecule
            Epoch 198: Loss did not decrease. Loss: 0,3219140936081519
            Epoch 198/300, Loss: 0,3219140936081519
            atom queen
            king proton
            electron molecule
            Epoch 199/300, Loss: 0,3219140819031896
            atom queen
            king proton
            electron molecule
            Epoch 200: Loss did not decrease. Loss: 0,321914092982935
            Epoch 200/300, Loss: 0,321914092982935
            atom queen
            king proton
            electron molecule
            Epoch 201/300, Loss: 0,32191408232587665
            atom queen
            king proton
            electron molecule
            Epoch 202: Loss did not decrease. Loss: 0,3219140924176785
            Epoch 202/300, Loss: 0,3219140924176785
            atom queen
            king proton
            electron molecule
            Epoch 203/300, Loss: 0,32191408271448474
            atom queen
            king proton
            electron molecule
            Epoch 204: Loss did not decrease. Loss: 0,32191409190662684
            Epoch 204/300, Loss: 0,32191409190662684
            atom queen
            king proton
            electron molecule
            Epoch 205/300, Loss: 0,32191408307171787
            atom queen
            king proton
            electron molecule
            Epoch 206: Loss did not decrease. Loss: 0,3219140914445779
            Epoch 206/300, Loss: 0,3219140914445779
            atom queen
            king proton
            electron molecule
            Epoch 207/300, Loss: 0,32191408340007116
            atom queen
            king proton
            electron molecule
            Epoch 208: Loss did not decrease. Loss: 0,3219140910268288
            Epoch 208/300, Loss: 0,3219140910268288
            atom queen
            king proton
            electron molecule
            Epoch 209/300, Loss: 0,32191408370184416
            atom queen
            king proton
            electron molecule
            Epoch 210: Loss did not decrease. Loss: 0,3219140906491285
            Epoch 210/300, Loss: 0,3219140906491285
            atom queen
            king proton
            electron molecule
            Epoch 211/300, Loss: 0,321914083979158
            atom queen
            king proton
            electron molecule
            Epoch 212: Loss did not decrease. Loss: 0,3219140903076343
            Epoch 212/300, Loss: 0,3219140903076343
            atom queen
            king proton
            electron molecule
            Epoch 213/300, Loss: 0,3219140842339669
            atom queen
            king proton
            electron molecule
            Epoch 214: Loss did not decrease. Loss: 0,32191408999887255
            Epoch 214/300, Loss: 0,32191408999887255
            atom queen
            king proton
            electron molecule
            Epoch 215/300, Loss: 0,32191408446807285
            atom queen
            king proton
            electron molecule
            Epoch 216: Loss did not decrease. Loss: 0,32191408971970303
            Epoch 216/300, Loss: 0,32191408971970303
            atom queen
            king proton
            electron molecule
            Epoch 217/300, Loss: 0,32191408468313504
            atom queen
            king proton
            electron molecule
            Epoch 218: Loss did not decrease. Loss: 0,321914089467287
            Epoch 218/300, Loss: 0,321914089467287
            atom queen
            king proton
            electron molecule
            Epoch 219/300, Loss: 0,32191408488068307
            atom queen
            king proton
            electron molecule
            Epoch 220: Loss did not decrease. Loss: 0,32191408923905845
            Epoch 220/300, Loss: 0,32191408923905845
            atom queen
            king proton
            electron molecule
            Epoch 221/300, Loss: 0,32191408506212493
            atom queen
            king proton
            electron molecule
            Epoch 222: Loss did not decrease. Loss: 0,3219140890326973
            Epoch 222/300, Loss: 0,3219140890326973
            atom queen
            king proton
            electron molecule
            Epoch 223/300, Loss: 0,32191408522875803
            atom queen
            king proton
            electron molecule
            Epoch 224: Loss did not decrease. Loss: 0,32191408884610667
            Epoch 224/300, Loss: 0,32191408884610667
            atom queen
            king proton
            electron molecule
            Epoch 225/300, Loss: 0,3219140853817764
            atom queen
            king proton
            electron molecule
            Epoch 226: Loss did not decrease. Loss: 0,32191408867739063
            Epoch 226/300, Loss: 0,32191408867739063
            atom queen
            king proton
            electron molecule
            Epoch 227/300, Loss: 0,32191408552227957
            atom queen
            king proton
            electron molecule
            Epoch 228: Loss did not decrease. Loss: 0,3219140885248354
            Epoch 228/300, Loss: 0,3219140885248354
            atom queen
            king proton
            electron molecule
            Epoch 229/300, Loss: 0,3219140856512795
            atom queen
            king proton
            electron molecule
            Epoch 230: Loss did not decrease. Loss: 0,32191408838689134
            Epoch 230/300, Loss: 0,32191408838689134
            atom queen
            king proton
            electron molecule
            Epoch 231/300, Loss: 0,3219140857697077
            atom queen
            king proton
            electron molecule
            Epoch 232: Loss did not decrease. Loss: 0,321914088262158
            Epoch 232/300, Loss: 0,321914088262158
            atom queen
            king proton
            electron molecule
            Epoch 233/300, Loss: 0,321914085878421
            atom queen
            king proton
            electron molecule
            Epoch 234: Loss did not decrease. Loss: 0,3219140881493688
            Epoch 234/300, Loss: 0,3219140881493688
            atom queen
            king proton
            electron molecule
            Epoch 235/300, Loss: 0,3219140859782081
            atom queen
            king proton
            electron molecule
            Epoch 236: Loss did not decrease. Loss: 0,32191408804737903
            Epoch 236/300, Loss: 0,32191408804737903
            atom queen
            king proton
            electron molecule
            Epoch 237/300, Loss: 0,3219140860697944
            atom queen
            king proton
            electron molecule
            Epoch 238: Loss did not decrease. Loss: 0,32191408795515375
            Epoch 238/300, Loss: 0,32191408795515375
            atom queen
            king proton
            electron molecule
            Epoch 239/300, Loss: 0,3219140861538471
            atom queen
            king proton
            electron molecule
            Epoch 240: Loss did not decrease. Loss: 0,32191408787175707
            Epoch 240/300, Loss: 0,32191408787175707
            atom queen
            king proton
            electron molecule
            Epoch 241/300, Loss: 0,3219140862309799
            atom queen
            king proton
            electron molecule
            Epoch 242: Loss did not decrease. Loss: 0,32191408779634306
            Epoch 242/300, Loss: 0,32191408779634306
            atom queen
            king proton
            electron molecule
            Epoch 243/300, Loss: 0,32191408630175694
            atom queen
            king proton
            electron molecule
            Epoch 244: Loss did not decrease. Loss: 0,32191408772814706
            Epoch 244/300, Loss: 0,32191408772814706
            atom queen
            king proton
            electron molecule
            Epoch 245/300, Loss: 0,32191408636669716
            atom queen
            king proton
            electron molecule
            Epoch 246: Loss did not decrease. Loss: 0,32191408766647717
            Epoch 246/300, Loss: 0,32191408766647717
            atom queen
            king proton
            electron molecule
            Epoch 247/300, Loss: 0,32191408642627767
            atom queen
            king proton
            electron molecule
            Epoch 248: Loss did not decrease. Loss: 0,32191408761070867
            Epoch 248/300, Loss: 0,32191408761070867
            atom queen
            king proton
            electron molecule
            Epoch 249/300, Loss: 0,3219140864809366
            atom queen
            king proton
            electron molecule
            Epoch 250: Loss did not decrease. Loss: 0,321914087560276
            Epoch 250/300, Loss: 0,321914087560276
            atom queen
            king proton
            electron molecule
            Epoch 251/300, Loss: 0,3219140865310772
            atom queen
            king proton
            electron molecule
            Epoch 252: Loss did not decrease. Loss: 0,3219140875146683
            Epoch 252/300, Loss: 0,3219140875146683
            atom queen
            king proton
            electron molecule
            Epoch 253/300, Loss: 0,3219140865770698
            atom queen
            king proton
            electron molecule
            Epoch 254: Loss did not decrease. Loss: 0,32191408747342337
            Epoch 254/300, Loss: 0,32191408747342337
            atom queen
            king proton
            electron molecule
            Epoch 255/300, Loss: 0,3219140866192547
            atom queen
            king proton
            electron molecule
            Epoch 256: Loss did not decrease. Loss: 0,32191408743612354
            Epoch 256/300, Loss: 0,32191408743612354
            atom queen
            king proton
            electron molecule
            Epoch 257/300, Loss: 0,3219140866579446
            atom queen
            king proton
            electron molecule
            Epoch 258: Loss did not decrease. Loss: 0,32191408740239097
            Epoch 258/300, Loss: 0,32191408740239097
            atom queen
            king proton
            electron molecule
            Epoch 259/300, Loss: 0,32191408669342647
            atom queen
            king proton
            electron molecule
            Epoch 260: Loss did not decrease. Loss: 0,3219140873718842
            Epoch 260/300, Loss: 0,3219140873718842
            atom queen
            king proton
            electron molecule
            Epoch 261/300, Loss: 0,3219140867259648
            atom queen
            king proton
            electron molecule
            Epoch 262: Loss did not decrease. Loss: 0,32191408734429444
            Epoch 262/300, Loss: 0,32191408734429444
            atom queen
            king proton
            electron molecule
            Epoch 263/300, Loss: 0,32191408675580147
            atom queen
            king proton
            electron molecule
            Epoch 264: Loss did not decrease. Loss: 0,32191408731934257
            Epoch 264/300, Loss: 0,32191408731934257
            atom queen
            king proton
            electron molecule
            Epoch 265/300, Loss: 0,32191408678315936
            atom queen
            king proton
            electron molecule
            Epoch 266: Loss did not decrease. Loss: 0,321914087296776
            Epoch 266/300, Loss: 0,321914087296776
            atom queen
            king proton
            electron molecule
            Epoch 267/300, Loss: 0,32191408680824307
            atom queen
            king proton
            electron molecule
            Epoch 268: Loss did not decrease. Loss: 0,3219140872763664
            Epoch 268/300, Loss: 0,3219140872763664
            atom queen
            king proton
            electron molecule
            Epoch 269/300, Loss: 0,32191408683123995
            atom queen
            king proton
            electron molecule
            Epoch 270: Loss did not decrease. Loss: 0,3219140872579077
            Epoch 270/300, Loss: 0,3219140872579077
            atom queen
            king proton
            electron molecule
            Epoch 271/300, Loss: 0,32191408685232253
            atom queen
            king proton
            electron molecule
            Epoch 272: Loss did not decrease. Loss: 0,3219140872412128
            Epoch 272/300, Loss: 0,3219140872412128
            atom queen
            king proton
            electron molecule
            Epoch 273/300, Loss: 0,3219140868716491
            atom queen
            king proton
            electron molecule
            Epoch 274: Loss did not decrease. Loss: 0,3219140872261132
            Epoch 274/300, Loss: 0,3219140872261132
            atom queen
            king proton
            electron molecule
            Epoch 275/300, Loss: 0,3219140868893651
            atom queen
            king proton
            electron molecule
            Epoch 276: Loss did not decrease. Loss: 0,3219140872124564
            Epoch 276/300, Loss: 0,3219140872124564
            atom queen
            king proton
            electron molecule
            Epoch 277/300, Loss: 0,32191408690560375
            atom queen
            king proton
            electron molecule
            Epoch 278: Loss did not decrease. Loss: 0,3219140872001043
            Epoch 278/300, Loss: 0,3219140872001043
            atom queen
            king proton
            electron molecule
            Epoch 279/300, Loss: 0,32191408692048756
            atom queen
            king proton
            electron molecule
            Epoch 280: Loss did not decrease. Loss: 0,3219140871889319
            Epoch 280/300, Loss: 0,3219140871889319
            atom queen
            king proton
            electron molecule
            Epoch 281/300, Loss: 0,32191408693412865
            atom queen
            king proton
            electron molecule
            Epoch 282: Loss did not decrease. Loss: 0,32191408717882675
            Epoch 282/300, Loss: 0,32191408717882675
            atom queen
            king proton
            electron molecule
            Epoch 283/300, Loss: 0,3219140869466303
            atom queen
            king proton
            electron molecule
            Epoch 284: Loss did not decrease. Loss: 0,32191408716968645
            Epoch 284/300, Loss: 0,32191408716968645
            atom queen
            king proton
            electron molecule
            Epoch 285/300, Loss: 0,321914086958087
            atom queen
            king proton
            electron molecule
            Epoch 286: Loss did not decrease. Loss: 0,32191408716141917
            Epoch 286/300, Loss: 0,32191408716141917
            atom queen
            king proton
            electron molecule
            Epoch 287/300, Loss: 0,32191408696858587
            atom queen
            king proton
            electron molecule
            Epoch 288: Loss did not decrease. Loss: 0,3219140871539411
            Epoch 288/300, Loss: 0,3219140871539411
            atom queen
            king proton
            electron molecule
            Epoch 289/300, Loss: 0,32191408697820617
            atom queen
            king proton
            electron molecule
            Epoch 290: Loss did not decrease. Loss: 0,321914087147177
            Epoch 290/300, Loss: 0,321914087147177
            atom queen
            king proton
            electron molecule
            Epoch 291/300, Loss: 0,3219140869870214
            atom queen
            king proton
            electron molecule
            Epoch 292: Loss did not decrease. Loss: 0,3219140871410584
            Epoch 292/300, Loss: 0,3219140871410584
            atom queen
            king proton
            electron molecule
            Epoch 293/300, Loss: 0,32191408699509805
            atom queen
            king proton
            electron molecule
            Epoch 294: Loss did not decrease. Loss: 0,32191408713552383
            Epoch 294/300, Loss: 0,32191408713552383
            atom queen
            king proton
            electron molecule
            Epoch 295/300, Loss: 0,3219140870024981
            atom queen
            king proton
            electron molecule
            Epoch 296: Loss did not decrease. Loss: 0,32191408713051767
            Epoch 296/300, Loss: 0,32191408713051767
            atom queen
            king proton
            electron molecule
            Epoch 297/300, Loss: 0,3219140870092778
            atom queen
            king proton
            electron molecule
            Epoch 298: Loss did not decrease. Loss: 0,3219140871259889
            Epoch 298/300, Loss: 0,3219140871259889
            atom queen
            king proton
            electron molecule
            Epoch 299/300, Loss: 0,3219140870154889
            atom queen
            king proton
            electron molecule
            Epoch 300: Loss did not decrease. Loss: 0,3219140871218924
            Epoch 300/300, Loss: 0,3219140871218924 
            */


            /*
            atom che
            king scrimgeour
            electron che
            Epoch 1/300, Loss: 1,5207085311165134
            atom skywest
            king olonga
            electron bypasses
            Epoch 2: Loss did not decrease. Loss: 9,187638314504497
            Epoch 2/300, Loss: 9,187638314504497
            atom langton
            king proton
            electron molecule
            Epoch 3/300, Loss: 1,9141922132522564
            atom queen
            king proton
            electron molecule
            Epoch 4/300, Loss: 0,3239121759269345
            atom queen
            king proton
            electron molecule
            Epoch 5/300, Loss: 0,3232311957828123
            atom queen
            king proton
            electron molecule
            Epoch 6/300, Loss: 0,3229989641314838
            atom queen
            king proton
            electron molecule
            Epoch 7/300, Loss: 0,32224476068114133
            atom queen
            king proton
            electron molecule
            Epoch 8: Loss did not decrease. Loss: 0,3226994264165112
            Epoch 8/300, Loss: 0,3226994264165112
            atom queen
            king proton
            electron molecule
            Epoch 9/300, Loss: 0,32215374171581607
            atom queen
            king proton
            electron molecule
            Epoch 10: Loss did not decrease. Loss: 0,322600046189575
            Epoch 10/300, Loss: 0,322600046189575
            atom queen
            king proton
            electron molecule
            Epoch 11/300, Loss: 0,3221220564080662
            atom queen
            king proton
            electron molecule
            Epoch 12: Loss did not decrease. Loss: 0,32252695706702034
            Epoch 12/300, Loss: 0,32252695706702034
            atom queen
            king proton
            electron molecule
            Epoch 13/300, Loss: 0,32209893570230835
            atom queen
            king proton
            electron molecule
            Epoch 14: Loss did not decrease. Loss: 0,32246333100877084
            Epoch 14/300, Loss: 0,32246333100877084
            atom queen
            king proton
            electron molecule
            Epoch 15/300, Loss: 0,32207890934282507
            atom queen
            king proton
            electron molecule
            Epoch 16: Loss did not decrease. Loss: 0,32240662465800557
            Epoch 16/300, Loss: 0,32240662465800557
            atom queen
            king proton
            electron molecule
            Epoch 17/300, Loss: 0,3220611349810372
            atom queen
            king proton
            electron molecule
            Epoch 18: Loss did not decrease. Loss: 0,3223559078780473
            Epoch 18/300, Loss: 0,3223559078780473
            atom queen
            king proton
            electron molecule
            Epoch 19/300, Loss: 0,32204530279144034
            atom queen
            king proton
            electron molecule
            Epoch 20: Loss did not decrease. Loss: 0,32231050509910386
            Epoch 20/300, Loss: 0,32231050509910386
            atom queen
            king proton
            electron molecule
            Epoch 21/300, Loss: 0,32203118796821956
            atom queen
            king proton
            electron molecule
            Epoch 22: Loss did not decrease. Loss: 0,3222698366365691
            Epoch 22/300, Loss: 0,3222698366365691
            atom queen
            king proton
            electron molecule
            Epoch 23/300, Loss: 0,32201859814034955
            atom queen
            king proton
            electron molecule
            Epoch 24: Loss did not decrease. Loss: 0,32223339221558267
            Epoch 24/300, Loss: 0,32223339221558267
            atom queen
            king proton
            electron molecule
            Epoch 25/300, Loss: 0,3220073644468516
            atom queen
            king proton
            electron molecule
            Epoch 26: Loss did not decrease. Loss: 0,3222007203585312
            Epoch 26/300, Loss: 0,3222007203585312
            atom queen
            king proton
            electron molecule
            Epoch 27/300, Loss: 0,3219973378914781
            atom queen
            king proton
            electron molecule
            Epoch 28: Loss did not decrease. Loss: 0,32217142068976085
            Epoch 28/300, Loss: 0,32217142068976085
            atom queen
            king proton
            electron molecule
            Epoch 29/300, Loss: 0,32198838669458296
            atom queen
            king proton
            electron molecule
            Epoch 30: Loss did not decrease. Loss: 0,32214513753059604
            Epoch 30/300, Loss: 0,32214513753059604
            atom queen
            king proton
            electron molecule
            Epoch 31/300, Loss: 0,32198039409635887
            atom queen
            king proton
            electron molecule
            Epoch 32: Loss did not decrease. Loss: 0,32212155443585816
            Epoch 32/300, Loss: 0,32212155443585816
            atom queen
            king proton
            electron molecule
            Epoch 33/300, Loss: 0,3219732564896856
            atom queen
            king proton
            electron molecule
            Epoch 34: Loss did not decrease. Loss: 0,32210038949746217
            Epoch 34/300, Loss: 0,32210038949746217
            atom queen
            king proton
            electron molecule
            Epoch 35/300, Loss: 0,3219668818201947
            atom queen
            king proton
            electron molecule
            Epoch 36: Loss did not decrease. Loss: 0,32208139128680796
            Epoch 36/300, Loss: 0,32208139128680796
            atom queen
            king proton
            electron molecule
            Epoch 37/300, Loss: 0,32196118820771846
            atom queen
            king proton
            electron molecule
            Epoch 38: Loss did not decrease. Loss: 0,32206433533377465
            Epoch 38/300, Loss: 0,32206433533377465
            atom queen
            king proton
            electron molecule
            Epoch 39/300, Loss: 0,3219561027528535
            atom queen
            king proton
            electron molecule
            Epoch 40: Loss did not decrease. Loss: 0,32204902105942007
            Epoch 40/300, Loss: 0,32204902105942007
            atom queen
            king proton
            electron molecule
            Epoch 41/300, Loss: 0,3219515604994152
            atom queen
            king proton
            electron molecule
            Epoch 42: Loss did not decrease. Loss: 0,32203526909453967
            Epoch 42/300, Loss: 0,32203526909453967
            atom queen
            king proton
            electron molecule
            Epoch 43/300, Loss: 0,3219475035290271
            atom queen
            king proton
            electron molecule
            Epoch 44: Loss did not decrease. Loss: 0,322022918928154
            Epoch 44/300, Loss: 0,322022918928154
            atom queen
            king proton
            electron molecule
            Epoch 45/300, Loss: 0,3219438801683718
            atom queen
            king proton
            electron molecule
            Epoch 46: Loss did not decrease. Loss: 0,32201182683949936
            Epoch 46/300, Loss: 0,32201182683949936
            atom queen
            king proton
            electron molecule
            Epoch 47/300, Loss: 0,3219406442930242
            atom queen
            king proton
            electron molecule
            Epoch 48: Loss did not decrease. Loss: 0,32200186407473996
            Epoch 48/300, Loss: 0,32200186407473996
            atom queen
            king proton
            electron molecule
            Epoch 49/300, Loss: 0,32193775471449587
            atom queen
            king proton
            electron molecule
            Epoch 50: Loss did not decrease. Loss: 0,32199291523580764
            Epoch 50/300, Loss: 0,32199291523580764
            atom queen
            king proton
            electron molecule
            Epoch 51/300, Loss: 0,3219351746392968
            atom queen
            king proton
            electron molecule
            Epoch 52: Loss did not decrease. Loss: 0,3219848768538154
            Epoch 52/300, Loss: 0,3219848768538154
            atom queen
            king proton
            electron molecule
            Epoch 53/300, Loss: 0,3219328711905843
            atom queen
            king proton
            electron molecule
            Epoch 54: Loss did not decrease. Loss: 0,32197765612362594
            Epoch 54/300, Loss: 0,32197765612362594
            atom queen
            king proton
            electron molecule
            Epoch 55/300, Loss: 0,3219308149844007
            atom queen
            king proton
            electron molecule
            Epoch 56: Loss did not decrease. Loss: 0,3219711697795765
            Epoch 56/300, Loss: 0,3219711697795765
            atom queen
            king proton
            electron molecule
            Epoch 57/300, Loss: 0,3219289797536887
            atom queen
            king proton
            electron molecule
            Epoch 58: Loss did not decrease. Loss: 0,3219653430951878
            Epoch 58/300, Loss: 0,3219653430951878
            atom queen
            king proton
            electron molecule
            Epoch 59/300, Loss: 0,3219273420142431
            atom queen
            king proton
            electron molecule
            Epoch 60: Loss did not decrease. Loss: 0,321960108992061
            Epoch 60/300, Loss: 0,321960108992061
            atom queen
            king proton
            electron molecule
            Epoch 61/300, Loss: 0,32192588076756906
            atom queen
            king proton
            electron molecule
            Epoch 62: Loss did not decrease. Loss: 0,3219554072451512
            Epoch 62/300, Loss: 0,3219554072451512
            atom queen
            king proton
            electron molecule
            Epoch 63/300, Loss: 0,32192457723630313
            atom queen
            king proton
            electron molecule
            Epoch 64: Loss did not decrease. Loss: 0,3219511837732907
            Epoch 64/300, Loss: 0,3219511837732907
            atom queen
            king proton
            electron molecule
            Epoch 65/300, Loss: 0,3219234146284144
            atom queen
            king proton
            electron molecule
            Epoch 66: Loss did not decrease. Loss: 0,3219473900052562
            Epoch 66/300, Loss: 0,3219473900052562
            atom queen
            king proton
            electron molecule
            Epoch 67/300, Loss: 0,3219223779268991
            atom queen
            king proton
            electron molecule
            Epoch 68: Loss did not decrease. Loss: 0,32194398231289745
            Epoch 68/300, Loss: 0,32194398231289745
            atom queen
            king proton
            electron molecule
            Epoch 69/300, Loss: 0,32192145370208786
            atom queen
            king proton
            electron molecule
            Epoch 70: Loss did not decrease. Loss: 0,3219409215038811
            Epoch 70/300, Loss: 0,3219409215038811
            atom queen
            king proton
            electron molecule
            Epoch 71/300, Loss: 0,3219206299440391
            atom queen
            king proton
            electron molecule
            Epoch 72: Loss did not decrease. Loss: 0,3219381723675054
            Epoch 72/300, Loss: 0,3219381723675054
            atom queen
            king proton
            electron molecule
            Epoch 73/300, Loss: 0,3219198959127997
            atom queen
            king proton
            electron molecule
            Epoch 74: Loss did not decrease. Loss: 0,3219357032678131
            Epoch 74/300, Loss: 0,3219357032678131
            atom queen
            king proton
            electron molecule
            Epoch 75/300, Loss: 0,32191924200457317
            atom queen
            king proton
            electron molecule
            Epoch 76: Loss did not decrease. Loss: 0,32193348577890707
            Epoch 76/300, Loss: 0,32193348577890707
            atom queen
            king proton
            electron molecule
            Epoch 77/300, Loss: 0,3219186596320644
            atom queen
            king proton
            electron molecule
            Epoch 78: Loss did not decrease. Loss: 0,3219314943579506
            Epoch 78/300, Loss: 0,3219314943579506
            atom queen
            king proton
            electron molecule
            Epoch 79/300, Loss: 0,32191814111746986
            atom queen
            king proton
            electron molecule
            Epoch 80: Loss did not decrease. Loss: 0,3219297060518556
            Epoch 80/300, Loss: 0,3219297060518556
            atom queen
            king proton
            electron molecule
            Epoch 81/300, Loss: 0,32191767959675166
            atom queen
            king proton
            electron molecule
            Epoch 82: Loss did not decrease. Loss: 0,32192810023409874
            Epoch 82/300, Loss: 0,32192810023409874
            atom queen
            king proton
            electron molecule
            Epoch 83/300, Loss: 0,3219172689339959
            atom queen
            king proton
            electron molecule
            Epoch 84: Loss did not decrease. Loss: 0,32192665836850753
            Epoch 84/300, Loss: 0,32192665836850753
            atom queen
            king proton
            electron molecule
            Epoch 85/300, Loss: 0,3219169036447708
            atom queen
            king proton
            electron molecule
            Epoch 86: Loss did not decrease. Loss: 0,3219253637971978
            Epoch 86/300, Loss: 0,3219253637971978
            atom queen
            king proton
            electron molecule
            Epoch 87/300, Loss: 0,3219165788275415
            atom queen
            king proton
            electron molecule
            Epoch 88: Loss did not decrease. Loss: 0,3219242015501527
            Epoch 88/300, Loss: 0,3219242015501527
            atom queen
            king proton
            electron molecule
            Epoch 89/300, Loss: 0,3219162901022809
            atom queen
            king proton
            electron molecule
            Epoch 90: Loss did not decrease. Loss: 0,3219231581742038
            Epoch 90/300, Loss: 0,3219231581742038
            atom queen
            king proton
            electron molecule
            Epoch 91/300, Loss: 0,3219160335555217
            atom queen
            king proton
            electron molecule
            Epoch 92: Loss did not decrease. Loss: 0,3219222215794086
            Epoch 92/300, Loss: 0,3219222215794086
            atom queen
            king proton
            electron molecule
            Epoch 93/300, Loss: 0,32191580569117106
            atom queen
            king proton
            electron molecule
            Epoch 94: Loss did not decrease. Loss: 0,32192138090103933
            Epoch 94/300, Loss: 0,32192138090103933
            atom queen
            king proton
            electron molecule
            Epoch 95/300, Loss: 0,3219156033864821
            atom queen
            king proton
            electron molecule
            Epoch 96: Loss did not decrease. Loss: 0,32192062637557906
            Epoch 96/300, Loss: 0,32192062637557906
            atom queen
            king proton
            electron molecule
            Epoch 97/300, Loss: 0,3219154238526416
            atom queen
            king proton
            electron molecule
            Epoch 98: Loss did not decrease. Loss: 0,32191994922929085
            Epoch 98/300, Loss: 0,32191994922929085
            atom queen
            king proton
            electron molecule
            Epoch 99/300, Loss: 0,32191526459948927
            atom queen
            king proton
            electron molecule
            Epoch 100: Loss did not decrease. Loss: 0,32191934157807883
            Epoch 100/300, Loss: 0,32191934157807883
            atom queen
            king proton
            electron molecule
            Epoch 101/300, Loss: 0,32191512340393813
            atom queen
            king proton
            electron molecule
            Epoch 102: Loss did not decrease. Loss: 0,32191879633748693
            Epoch 102/300, Loss: 0,32191879633748693
            atom queen
            king proton
            electron molecule
            Epoch 103/300, Loss: 0,3219149982817071
            atom queen
            king proton
            electron molecule
            Epoch 104: Loss did not decrease. Loss: 0,32191830714180697
            Epoch 104/300, Loss: 0,32191830714180697
            atom queen
            king proton
            electron molecule
            Epoch 105/300, Loss: 0,3219148874620194
            atom queen
            king proton
            electron molecule
            Epoch 106: Loss did not decrease. Loss: 0,32191786827136715
            Epoch 106/300, Loss: 0,32191786827136715
            atom queen
            king proton
            electron molecule
            Epoch 107/300, Loss: 0,321914789364959
            atom queen
            king proton
            electron molecule
            Epoch 108: Loss did not decrease. Loss: 0,32191747458717185
            Epoch 108/300, Loss: 0,32191747458717185
            atom queen
            king proton
            electron molecule
            Epoch 109/300, Loss: 0,32191470258120397
            atom queen
            king proton
            electron molecule
            Epoch 110: Loss did not decrease. Loss: 0,3219171214721503
            Epoch 110/300, Loss: 0,3219171214721503
            atom queen
            king proton
            electron molecule
            Epoch 111/300, Loss: 0,3219146258538919
            atom queen
            king proton
            electron molecule
            Epoch 112: Loss did not decrease. Loss: 0,32191680477833845
            Epoch 112/300, Loss: 0,32191680477833845
            atom queen
            king proton
            electron molecule
            Epoch 113/300, Loss: 0,3219145580623915
            atom queen
            king proton
            electron molecule
            Epoch 114: Loss did not decrease. Loss: 0,32191652077940036
            Epoch 114/300, Loss: 0,32191652077940036
            atom queen
            king proton
            electron molecule
            Epoch 115/300, Loss: 0,3219144982077839
            atom queen
            king proton
            electron molecule
            Epoch 116: Loss did not decrease. Loss: 0,3219162661279439
            Epoch 116/300, Loss: 0,3219162661279439
            atom queen
            king proton
            electron molecule
            Epoch 117/300, Loss: 0,3219144453998722
            atom queen
            king proton
            electron molecule
            Epoch 118: Loss did not decrease. Loss: 0,32191603781714945
            Epoch 118/300, Loss: 0,32191603781714945
            atom queen
            king proton
            electron molecule
            Epoch 119/300, Loss: 0,3219143988455621
            atom queen
            king proton
            electron molecule
            Epoch 120: Loss did not decrease. Loss: 0,32191583314627387
            Epoch 120/300, Loss: 0,32191583314627387
            atom queen
            king proton
            electron molecule
            Epoch 121/300, Loss: 0,3219143578384681
            atom queen
            king proton
            electron molecule
            Epoch 122: Loss did not decrease. Loss: 0,32191564968963915
            Epoch 122/300, Loss: 0,32191564968963915
            atom queen
            king proton
            electron molecule
            Epoch 123/300, Loss: 0,32191432174961826
            atom queen
            king proton
            electron molecule
            Epoch 124: Loss did not decrease. Loss: 0,3219154852687543
            Epoch 124/300, Loss: 0,3219154852687543
            atom queen
            king proton
            electron molecule
            Epoch 125/300, Loss: 0,3219142900191417
            atom queen
            king proton
            electron molecule
            Epoch 126: Loss did not decrease. Loss: 0,3219153379272533
            Epoch 126/300, Loss: 0,3219153379272533
            atom queen
            king proton
            electron molecule
            Epoch 127/300, Loss: 0,32191426214883584
            atom queen
            king proton
            electron molecule
            Epoch 128: Loss did not decrease. Loss: 0,3219152059083661
            Epoch 128/300, Loss: 0,3219152059083661
            atom queen
            king proton
            electron molecule
            Epoch 129/300, Loss: 0,3219142376955231
            atom queen
            king proton
            electron molecule
            Epoch 130: Loss did not decrease. Loss: 0,32191508763466575
            Epoch 130/300, Loss: 0,32191508763466575
            atom queen
            king proton
            electron molecule
            Epoch 131/300, Loss: 0,32191421626511013
            atom queen
            king proton
            electron molecule
            Epoch 132: Loss did not decrease. Loss: 0,32191498168986327
            Epoch 132/300, Loss: 0,32191498168986327
            atom queen
            king proton
            electron molecule
            Epoch 133/300, Loss: 0,3219141975072804
            atom queen
            king proton
            electron molecule
            Epoch 134: Loss did not decrease. Loss: 0,32191488680244346
            Epoch 134/300, Loss: 0,32191488680244346
            atom queen
            king proton
            electron molecule
            Epoch 135/300, Loss: 0,32191418111074993
            atom queen
            king proton
            electron molecule
            Epoch 136: Loss did not decrease. Loss: 0,32191480183095444
            Epoch 136/300, Loss: 0,32191480183095444
            atom queen
            king proton
            electron molecule
            Epoch 137/300, Loss: 0,32191416679902973
            atom queen
            king proton
            electron molecule
            Epoch 138: Loss did not decrease. Loss: 0,3219147257507873
            Epoch 138/300, Loss: 0,3219147257507873
            atom queen
            king proton
            electron molecule
            Epoch 139/300, Loss: 0,3219141543266388
            atom queen
            king proton
            electron molecule
            Epoch 140: Loss did not decrease. Loss: 0,3219146576422914
            Epoch 140/300, Loss: 0,3219146576422914
            atom queen
            king proton
            electron molecule
            Epoch 141/300, Loss: 0,3219141434757235
            atom queen
            king proton
            electron molecule
            Epoch 142: Loss did not decrease. Loss: 0,3219145966800943
            Epoch 142/300, Loss: 0,3219145966800943
            atom queen
            king proton
            electron molecule
            Epoch 143/300, Loss: 0,3219141340530368
            atom queen
            king proton
            electron molecule
            Epoch 144: Loss did not decrease. Loss: 0,3219145421235022
            Epoch 144/300, Loss: 0,3219145421235022
            atom queen
            king proton
            electron molecule
            Epoch 145/300, Loss: 0,32191412588724283
            atom queen
            king proton
            electron molecule
            Epoch 146: Loss did not decrease. Loss: 0,3219144933078737
            Epoch 146/300, Loss: 0,3219144933078737
            atom queen
            king proton
            electron molecule
            Epoch 147/300, Loss: 0,32191411882650994
            atom queen
            king proton
            electron molecule
            Epoch 148: Loss did not decrease. Loss: 0,32191444963686716
            Epoch 148/300, Loss: 0,32191444963686716
            atom queen
            king proton
            electron molecule
            Epoch 149/300, Loss: 0,32191411273636183
            atom queen
            king proton
            electron molecule
            Epoch 150: Loss did not decrease. Loss: 0,3219144105754733
            Epoch 150/300, Loss: 0,3219144105754733
            atom queen
            king proton
            electron molecule
            Epoch 151/300, Loss: 0,3219141074977622
            atom queen
            king proton
            electron molecule
            Epoch 152: Loss did not decrease. Loss: 0,32191437564375674
            Epoch 152/300, Loss: 0,32191437564375674
            atom queen
            king proton
            electron molecule
            Epoch 153/300, Loss: 0,32191410300540446
            atom queen
            king proton
            electron molecule
            Epoch 154: Loss did not decrease. Loss: 0,3219143444112298
            Epoch 154/300, Loss: 0,3219143444112298
            atom queen
            king proton
            electron molecule
            Epoch 155/300, Loss: 0,3219140991661867
            atom queen
            king proton
            electron molecule
            Epoch 156: Loss did not decrease. Loss: 0,32191431649179975
            Epoch 156/300, Loss: 0,32191431649179975
            atom queen
            king proton
            electron molecule
            Epoch 157/300, Loss: 0,32191409589785164
            atom queen
            king proton
            electron molecule
            Epoch 158: Loss did not decrease. Loss: 0,3219142915392283
            Epoch 158/300, Loss: 0,3219142915392283
            atom queen
            king proton
            electron molecule
            Epoch 159/300, Loss: 0,3219140931277751
            atom queen
            king proton
            electron molecule
            Epoch 160: Loss did not decrease. Loss: 0,32191426924305167
            Epoch 160/300, Loss: 0,32191426924305167
            atom queen
            king proton
            electron molecule
            Epoch 161/300, Loss: 0,3219140907918857
            atom queen
            king proton
            electron molecule
            Epoch 162: Loss did not decrease. Loss: 0,3219142493249177
            Epoch 162/300, Loss: 0,3219142493249177
            atom queen
            king proton
            electron molecule
            Epoch 163/300, Loss: 0,3219140888337018
            atom queen
            king proton
            electron molecule
            Epoch 164: Loss did not decrease. Loss: 0,32191423153529336
            Epoch 164/300, Loss: 0,32191423153529336
            atom queen
            king proton
            electron molecule
            Epoch 165/300, Loss: 0,32191408720347475
            atom queen
            king proton
            electron molecule
            Epoch 166: Loss did not decrease. Loss: 0,3219142156505091
            Epoch 166/300, Loss: 0,3219142156505091
            atom queen
            king proton
            electron molecule
            Epoch 167/300, Loss: 0,3219140858574258
            atom queen
            king proton
            electron molecule
            Epoch 168: Loss did not decrease. Loss: 0,3219142014701027
            Epoch 168/300, Loss: 0,3219142014701027
            atom queen
            king proton
            electron molecule
            Epoch 169/300, Loss: 0,3219140847570651
            atom queen
            king proton
            electron molecule
            Epoch 170: Loss did not decrease. Loss: 0,3219141888144353
            Epoch 170/300, Loss: 0,3219141888144353
            atom queen
            king proton
            electron molecule
            Epoch 171/300, Loss: 0,32191408386858894
            atom queen
            king proton
            electron molecule
            Epoch 172: Loss did not decrease. Loss: 0,32191417752254997
            Epoch 172/300, Loss: 0,32191417752254997
            atom queen
            king proton
            electron molecule
            Epoch 173/300, Loss: 0,32191408316234016
            atom queen
            king proton
            electron molecule
            Epoch 174: Loss did not decrease. Loss: 0,32191416745024803
            Epoch 174/300, Loss: 0,32191416745024803
            atom queen
            king proton
            electron molecule
            Epoch 175/300, Loss: 0,32191408261233123
            atom queen
            king proton
            electron molecule
            Epoch 176: Loss did not decrease. Loss: 0,3219141584683632
            Epoch 176/300, Loss: 0,3219141584683632
            atom queen
            king proton
            electron molecule
            Epoch 177/300, Loss: 0,3219140821958177
            atom queen
            king proton
            electron molecule
            Epoch 178: Loss did not decrease. Loss: 0,3219141504612109
            Epoch 178/300, Loss: 0,3219141504612109
            atom queen
            king proton
            electron molecule
            Epoch 179/300, Loss: 0,321914081892921
            atom queen
            king proton
            electron molecule
            Epoch 180: Loss did not decrease. Loss: 0,3219141433251974
            Epoch 180/300, Loss: 0,3219141433251974
            atom queen
            king proton
            electron molecule
            Epoch 181/300, Loss: 0,3219140816862925
            atom queen
            king proton
            electron molecule
            Epoch 182: Loss did not decrease. Loss: 0,3219141369675697
            Epoch 182/300, Loss: 0,3219141369675697
            atom queen
            king proton
            electron molecule
            Epoch 183/300, Loss: 0,32191408156081636
            atom queen
            king proton
            electron molecule
            Epoch 184: Loss did not decrease. Loss: 0,32191413130529445
            Epoch 184/300, Loss: 0,32191413130529445
            atom queen
            king proton
            electron molecule
            Epoch 185/300, Loss: 0,3219140815033434
            atom queen
            king proton
            electron molecule
            Epoch 186: Loss did not decrease. Loss: 0,3219141262640524
            Epoch 186/300, Loss: 0,3219141262640524
            atom queen
            king proton
            electron molecule
            Epoch 187/300, Loss: 0,3219140815024582
            atom queen
            king proton
            electron molecule
            Epoch 188: Loss did not decrease. Loss: 0,32191412177733436
            Epoch 188/300, Loss: 0,32191412177733436
            atom queen
            king proton
            electron molecule
            Epoch 189/300, Loss: 0,3219140815482702
            atom queen
            king proton
            electron molecule
            Epoch 190: Loss did not decrease. Loss: 0,32191411778563106
            Epoch 190/300, Loss: 0,32191411778563106
            atom queen
            king proton
            electron molecule
            Epoch 191/300, Loss: 0,32191408163222945
            atom queen
            king proton
            electron molecule
            Epoch 192: Loss did not decrease. Loss: 0,3219141142357061
            Epoch 192/300, Loss: 0,3219141142357061
            atom queen
            king proton
            electron molecule
            Epoch 193/300, Loss: 0,3219140817469634
            atom queen
            king proton
            electron molecule
            Epoch 194: Loss did not decrease. Loss: 0,3219141110799437
            Epoch 194/300, Loss: 0,3219141110799437
            atom queen
            king proton
            electron molecule
            Epoch 195/300, Loss: 0,32191408188613263
            atom queen
            king proton
            electron molecule
            Epoch 196: Loss did not decrease. Loss: 0,321914108275763
            Epoch 196/300, Loss: 0,321914108275763
            atom queen
            king proton
            electron molecule
            Epoch 197/300, Loss: 0,3219140820443028
            atom queen
            king proton
            electron molecule
            Epoch 198: Loss did not decrease. Loss: 0,3219141057850938
            Epoch 198/300, Loss: 0,3219141057850938
            atom queen
            king proton
            electron molecule
            Epoch 199/300, Loss: 0,3219140822168315
            atom queen
            king proton
            electron molecule
            Epoch 200: Loss did not decrease. Loss: 0,32191410357390476
            Epoch 200/300, Loss: 0,32191410357390476
            atom queen
            king proton
            electron molecule
            Epoch 201/300, Loss: 0,3219140823997689
            atom queen
            king proton
            electron molecule
            Epoch 202: Loss did not decrease. Loss: 0,32191410161178297
            Epoch 202/300, Loss: 0,32191410161178297
            atom queen
            king proton
            electron molecule
            Epoch 203/300, Loss: 0,3219140825897688
            atom queen
            king proton
            electron molecule
            Epoch 204: Loss did not decrease. Loss: 0,32191409987155306
            Epoch 204/300, Loss: 0,32191409987155306
            atom queen
            king proton
            electron molecule
            Epoch 205/300, Loss: 0,32191408278401185
            atom queen
            king proton
            electron molecule
            Epoch 206: Loss did not decrease. Loss: 0,32191409832893975
            Epoch 206/300, Loss: 0,32191409832893975
            atom queen
            king proton
            electron molecule
            Epoch 207/300, Loss: 0,32191408298013585
            atom queen
            king proton
            electron molecule
            Epoch 208: Loss did not decrease. Loss: 0,32191409696226253
            Epoch 208/300, Loss: 0,32191409696226253
            atom queen
            king proton
            electron molecule
            Epoch 209/300, Loss: 0,3219140831761767
            atom queen
            king proton
            electron molecule
            Epoch 210: Loss did not decrease. Loss: 0,32191409575216307
            Epoch 210/300, Loss: 0,32191409575216307
            atom queen
            king proton
            electron molecule
            Epoch 211/300, Loss: 0,3219140833705142
            atom queen
            king proton
            electron molecule
            Epoch 212: Loss did not decrease. Loss: 0,3219140946813609
            Epoch 212/300, Loss: 0,3219140946813609
            atom queen
            king proton
            electron molecule
            Epoch 213/300, Loss: 0,32191408356182555
            atom queen
            king proton
            electron molecule
            Epoch 214: Loss did not decrease. Loss: 0,3219140937344345
            Epoch 214/300, Loss: 0,3219140937344345
            atom queen
            king proton
            electron molecule
            Epoch 215/300, Loss: 0,3219140837490448
            atom queen
            king proton
            electron molecule
            Epoch 216: Loss did not decrease. Loss: 0,3219140928976248
            Epoch 216/300, Loss: 0,3219140928976248
            atom queen
            king proton
            electron molecule
            Epoch 217/300, Loss: 0,32191408393132637
            atom queen
            king proton
            electron molecule
            Epoch 218: Loss did not decrease. Loss: 0,3219140921586593
            Epoch 218/300, Loss: 0,3219140921586593
            atom queen
            king proton
            electron molecule
            Epoch 219/300, Loss: 0,32191408410801375
            atom queen
            king proton
            electron molecule
            Epoch 220: Loss did not decrease. Loss: 0,3219140915065951
            Epoch 220/300, Loss: 0,3219140915065951
            atom queen
            king proton
            electron molecule
            Epoch 221/300, Loss: 0,3219140842786122
            atom queen
            king proton
            electron molecule
            Epoch 222: Loss did not decrease. Loss: 0,32191409093167683
            Epoch 222/300, Loss: 0,32191409093167683
            atom queen
            king proton
            electron molecule
            Epoch 223/300, Loss: 0,32191408444276376
            atom queen
            king proton
            electron molecule
            Epoch 224: Loss did not decrease. Loss: 0,32191409042521135
            Epoch 224/300, Loss: 0,32191409042521135
            atom queen
            king proton
            electron molecule
            Epoch 225/300, Loss: 0,321914084600227
            atom queen
            king proton
            electron molecule
            Epoch 226: Loss did not decrease. Loss: 0,32191408997945437
            Epoch 226/300, Loss: 0,32191408997945437
            atom queen
            king proton
            electron molecule
            Epoch 227/300, Loss: 0,32191408475085875
            atom queen
            king proton
            electron molecule
            Epoch 228: Loss did not decrease. Loss: 0,32191408958750867
            Epoch 228/300, Loss: 0,32191408958750867
            atom queen
            king proton
            electron molecule
            Epoch 229/300, Loss: 0,3219140848945976
            atom queen
            king proton
            electron molecule
            Epoch 230: Loss did not decrease. Loss: 0,3219140892432342
            Epoch 230/300, Loss: 0,3219140892432342
            atom queen
            king proton
            electron molecule
            Epoch 231/300, Loss: 0,32191408503145047
            atom queen
            king proton
            electron molecule
            Epoch 232: Loss did not decrease. Loss: 0,32191408894116674
            Epoch 232/300, Loss: 0,32191408894116674
            atom queen
            king proton
            electron molecule
            Epoch 233/300, Loss: 0,32191408516147996
            atom queen
            king proton
            electron molecule
            Epoch 234: Loss did not decrease. Loss: 0,32191408867644505
            Epoch 234/300, Loss: 0,32191408867644505
            atom queen
            king proton
            electron molecule
            Epoch 235/300, Loss: 0,32191408528479537
            atom queen
            king proton
            electron molecule
            Epoch 236: Loss did not decrease. Loss: 0,32191408844474695
            Epoch 236/300, Loss: 0,32191408844474695
            atom queen
            king proton
            electron molecule
            Epoch 237/300, Loss: 0,3219140854015426
            atom queen
            king proton
            electron molecule
            Epoch 238: Loss did not decrease. Loss: 0,3219140882422293
            Epoch 238/300, Loss: 0,3219140882422293
            atom queen
            king proton
            electron molecule
            Epoch 239/300, Loss: 0,32191408551189654
            atom queen
            king proton
            electron molecule
            Epoch 240: Loss did not decrease. Loss: 0,3219140880654786
            Epoch 240/300, Loss: 0,3219140880654786
            atom queen
            king proton
            electron molecule
            Epoch 241/300, Loss: 0,3219140856160552
            atom queen
            king proton
            electron molecule
            Epoch 242: Loss did not decrease. Loss: 0,3219140879114624
            Epoch 242/300, Loss: 0,3219140879114624
            atom queen
            king proton
            electron molecule
            Epoch 243/300, Loss: 0,321914085714233
            atom queen
            king proton
            electron molecule
            Epoch 244: Loss did not decrease. Loss: 0,3219140877774896
            Epoch 244/300, Loss: 0,3219140877774896
            atom queen
            king proton
            electron molecule
            Epoch 245/300, Loss: 0,32191408580665765
            atom queen
            king proton
            electron molecule
            Epoch 246: Loss did not decrease. Loss: 0,32191408766117174
            Epoch 246/300, Loss: 0,32191408766117174
            atom queen
            king proton
            electron molecule
            Epoch 247/300, Loss: 0,32191408589356424
            atom queen
            king proton
            electron molecule
            Epoch 248: Loss did not decrease. Loss: 0,321914087560391
            Epoch 248/300, Loss: 0,321914087560391
            atom queen
            king proton
            electron molecule
            Epoch 249/300, Loss: 0,32191408597519294
            atom queen
            king proton
            electron molecule
            Epoch 250: Loss did not decrease. Loss: 0,3219140874732705
            Epoch 250/300, Loss: 0,3219140874732705
            atom queen
            king proton
            electron molecule
            Epoch 251/300, Loss: 0,32191408605178623
            atom queen
            king proton
            electron molecule
            Epoch 252: Loss did not decrease. Loss: 0,3219140873981472
            Epoch 252/300, Loss: 0,3219140873981472
            atom queen
            king proton
            electron molecule
            Epoch 253/300, Loss: 0,32191408612358585
            atom queen
            king proton
            electron molecule
            Epoch 254: Loss did not decrease. Loss: 0,321914087333549
            Epoch 254/300, Loss: 0,321914087333549
            atom queen
            king proton
            electron molecule
            Epoch 255/300, Loss: 0,32191408619083123
            atom queen
            king proton
            electron molecule
            Epoch 256: Loss did not decrease. Loss: 0,3219140872781734
            Epoch 256/300, Loss: 0,3219140872781734
            atom queen
            king proton
            electron molecule
            Epoch 257/300, Loss: 0,3219140862537582
            atom queen
            king proton
            electron molecule
            Epoch 258: Loss did not decrease. Loss: 0,32191408723086884
            Epoch 258/300, Loss: 0,32191408723086884
            atom queen
            king proton
            electron molecule
            Epoch 259/300, Loss: 0,32191408631259694
            atom queen
            king proton
            electron molecule
            Epoch 260: Loss did not decrease. Loss: 0,3219140871906176
            Epoch 260/300, Loss: 0,3219140871906176
            atom queen
            king proton
            electron molecule
            Epoch 261/300, Loss: 0,32191408636757185
            atom queen
            king proton
            electron molecule
            Epoch 262: Loss did not decrease. Loss: 0,3219140871565214
            Epoch 262/300, Loss: 0,3219140871565214
            atom queen
            king proton
            electron molecule
            Epoch 263/300, Loss: 0,3219140864189
            atom queen
            king proton
            electron molecule
            Epoch 264: Loss did not decrease. Loss: 0,3219140871277873
            Epoch 264/300, Loss: 0,3219140871277873
            atom queen
            king proton
            electron molecule
            Epoch 265/300, Loss: 0,32191408646679137
            atom queen
            king proton
            electron molecule
            Epoch 266: Loss did not decrease. Loss: 0,3219140871037169
            Epoch 266/300, Loss: 0,3219140871037169
            atom queen
            king proton
            electron molecule
            Epoch 267/300, Loss: 0,32191408651144715
            atom queen
            king proton
            electron molecule
            Epoch 268: Loss did not decrease. Loss: 0,32191408708369457
            Epoch 268/300, Loss: 0,32191408708369457
            atom queen
            king proton
            electron molecule
            Epoch 269/300, Loss: 0,3219140865530609
            atom queen
            king proton
            electron molecule
            Epoch 270: Loss did not decrease. Loss: 0,32191408706717867
            Epoch 270/300, Loss: 0,32191408706717867
            atom queen
            king proton
            electron molecule
            Epoch 271/300, Loss: 0,3219140865918176
            atom queen
            king proton
            electron molecule
            Epoch 272: Loss did not decrease. Loss: 0,3219140870536934
            Epoch 272/300, Loss: 0,3219140870536934
            atom queen
            king proton
            electron molecule
            Epoch 273/300, Loss: 0,3219140866278937
            atom queen
            king proton
            electron molecule
            Epoch 274: Loss did not decrease. Loss: 0,3219140870428203
            Epoch 274/300, Loss: 0,3219140870428203
            atom queen
            king proton
            electron molecule
            Epoch 275/300, Loss: 0,321914086661457
            atom queen
            king proton
            electron molecule
            Epoch 276: Loss did not decrease. Loss: 0,32191408703419294
            Epoch 276/300, Loss: 0,32191408703419294
            atom queen
            king proton
            electron molecule
            Epoch 277/300, Loss: 0,32191408669266686
            atom queen
            king proton
            electron molecule
            Epoch 278: Loss did not decrease. Loss: 0,32191408702748986
            Epoch 278/300, Loss: 0,32191408702748986
            atom queen
            king proton
            electron molecule
            Epoch 279/300, Loss: 0,3219140867216749
            atom queen
            king proton
            electron molecule
            Epoch 280: Loss did not decrease. Loss: 0,32191408702243
            Epoch 280/300, Loss: 0,32191408702243
            atom queen
            king proton
            electron molecule
            Epoch 281/300, Loss: 0,32191408674862393
            atom queen
            king proton
            electron molecule
            Epoch 282: Loss did not decrease. Loss: 0,3219140870187678
            Epoch 282/300, Loss: 0,3219140870187678
            atom queen
            king proton
            electron molecule
            Epoch 283/300, Loss: 0,3219140867736493
            atom queen
            king proton
            electron molecule
            Epoch 284: Loss did not decrease. Loss: 0,3219140870162886
            Epoch 284/300, Loss: 0,3219140870162886
            atom queen
            king proton
            electron molecule
            Epoch 285/300, Loss: 0,3219140867968788
            atom queen
            king proton
            electron molecule
            Epoch 286: Loss did not decrease. Loss: 0,32191408701480617
            Epoch 286/300, Loss: 0,32191408701480617
            atom queen
            king proton
            electron molecule
            Epoch 287/300, Loss: 0,3219140868184325
            atom queen
            king proton
            electron molecule
            Epoch 288: Loss did not decrease. Loss: 0,32191408701415775
            Epoch 288/300, Loss: 0,32191408701415775
            atom queen
            king proton
            electron molecule
            Epoch 289/300, Loss: 0,3219140868384239
            atom queen
            king proton
            electron molecule
            Epoch 290: Loss did not decrease. Loss: 0,3219140870142026
            Epoch 290/300, Loss: 0,3219140870142026
            atom queen
            king proton
            electron molecule
            Epoch 291/300, Loss: 0,3219140868569594
            atom queen
            king proton
            electron molecule
            Epoch 292: Loss did not decrease. Loss: 0,32191408701481894
            Epoch 292/300, Loss: 0,32191408701481894
            atom queen
            king proton
            electron molecule
            Epoch 293/300, Loss: 0,32191408687413914
            atom queen
            king proton
            electron molecule
            Epoch 294: Loss did not decrease. Loss: 0,32191408701590113
            Epoch 294/300, Loss: 0,32191408701590113
            atom queen
            king proton
            electron molecule
            Epoch 295/300, Loss: 0,32191408689005657
            atom queen
            king proton
            electron molecule
            Epoch 296: Loss did not decrease. Loss: 0,3219140870173582
            Epoch 296/300, Loss: 0,3219140870173582
            atom queen
            king proton
            electron molecule
            Epoch 297/300, Loss: 0,3219140869048
            atom queen
            king proton
            electron molecule
            Epoch 298: Loss did not decrease. Loss: 0,3219140870191125
            Epoch 298/300, Loss: 0,3219140870191125
            atom queen
            king proton
            electron molecule
            Epoch 299/300, Loss: 0,3219140869184515
            atom queen
            king proton
            electron molecule
            Epoch 300: Loss did not decrease. Loss: 0,321914087021097
            Epoch 300/300, Loss: 0,321914087021097 
            */


            /*
            atom bluetongue
            king nair
            electron plates
            Epoch 1/300, Loss: 1,5440453983017488
            atom nnc
            king wind-tunnel
            electron fsn
            Epoch 2: Loss did not decrease. Loss: 8,18795568759145
            Epoch 2/300, Loss: 8,18795568759145
            atom sickout
            king proton
            electron molecule
            Epoch 3/300, Loss: 2,7460334289891772
            atom queen
            king proton
            electron molecule
            Epoch 4/300, Loss: 0,32636078043971456
            atom queen
            king proton
            electron molecule
            Epoch 5/300, Loss: 0,32253430424905444
            atom queen
            king proton
            electron molecule
            Epoch 6/300, Loss: 0,322207063827442
            atom queen
            king proton
            electron molecule
            Epoch 7: Loss did not decrease. Loss: 0,32226798016914066
            Epoch 7/300, Loss: 0,32226798016914066
            atom queen
            king proton
            electron molecule
            Epoch 8/300, Loss: 0,32218213823007735
            atom queen
            king proton
            electron molecule
            Epoch 9: Loss did not decrease. Loss: 0,3222436418438617
            Epoch 9/300, Loss: 0,3222436418438617
            atom queen
            king proton
            electron molecule
            Epoch 10/300, Loss: 0,3221575217605184
            atom queen
            king proton
            electron molecule
            Epoch 11: Loss did not decrease. Loss: 0,3222137990105483
            Epoch 11/300, Loss: 0,3222137990105483
            atom queen
            king proton
            electron molecule
            Epoch 12/300, Loss: 0,32213179609771875
            atom queen
            king proton
            electron molecule
            Epoch 13: Loss did not decrease. Loss: 0,32218565406622807
            Epoch 13/300, Loss: 0,32218565406622807
            atom queen
            king proton
            electron molecule
            Epoch 14/300, Loss: 0,32210840237823907
            atom queen
            king proton
            electron molecule
            Epoch 15: Loss did not decrease. Loss: 0,32216011247892457
            Epoch 15/300, Loss: 0,32216011247892457
            atom queen
            king proton
            electron molecule
            Epoch 16/300, Loss: 0,3220874755558606
            atom queen
            king proton
            electron molecule
            Epoch 17: Loss did not decrease. Loss: 0,32213702814325085
            Epoch 17/300, Loss: 0,32213702814325085
            atom queen
            king proton
            electron molecule
            Epoch 18/300, Loss: 0,32206878831595703
            atom queen
            king proton
            electron molecule
            Epoch 19: Loss did not decrease. Loss: 0,32211616512003016
            Epoch 19/300, Loss: 0,32211616512003016
            atom queen
            king proton
            electron molecule
            Epoch 20/300, Loss: 0,32205210194405415
            atom queen
            king proton
            electron molecule
            Epoch 21: Loss did not decrease. Loss: 0,32209730175166357
            Epoch 21/300, Loss: 0,32209730175166357
            atom queen
            king proton
            electron molecule
            Epoch 22/300, Loss: 0,32203720063270597
            atom queen
            king proton
            electron molecule
            Epoch 23: Loss did not decrease. Loss: 0,32208023895640064
            Epoch 23/300, Loss: 0,32208023895640064
            atom queen
            king proton
            electron molecule
            Epoch 24/300, Loss: 0,32202389222505734
            atom queen
            king proton
            electron molecule
            Epoch 25: Loss did not decrease. Loss: 0,32206479862475706
            Epoch 25/300, Loss: 0,32206479862475706
            atom queen
            king proton
            electron molecule
            Epoch 26/300, Loss: 0,3220120057795681
            atom queen
            king proton
            electron molecule
            Epoch 27: Loss did not decrease. Loss: 0,32205082129284884
            Epoch 27/300, Loss: 0,32205082129284884
            atom queen
            king proton
            electron molecule
            Epoch 28/300, Loss: 0,32200138912557236
            atom queen
            king proton
            electron molecule
            Epoch 29: Loss did not decrease. Loss: 0,32203816402219804
            Epoch 29/300, Loss: 0,32203816402219804
            atom queen
            king proton
            electron molecule
            Epoch 30/300, Loss: 0,32199190669779226
            atom queen
            king proton
            electron molecule
            Epoch 31: Loss did not decrease. Loss: 0,3220266985423173
            Epoch 31/300, Loss: 0,3220266985423173
            atom queen
            king proton
            electron molecule
            Epoch 32/300, Loss: 0,3219834376399678
            atom queen
            king proton
            electron molecule
            Epoch 33: Loss did not decrease. Loss: 0,3220163096264357
            Epoch 33/300, Loss: 0,3220163096264357
            atom queen
            king proton
            electron molecule
            Epoch 34/300, Loss: 0,32197587414273093
            atom queen
            king proton
            electron molecule
            Epoch 35: Loss did not decrease. Loss: 0,32200689366760965
            Epoch 35/300, Loss: 0,32200689366760965
            atom queen
            king proton
            electron molecule
            Epoch 36/300, Loss: 0,32196911998362193
            atom queen
            king proton
            electron molecule
            Epoch 37: Loss did not decrease. Loss: 0,321998357427036
            Epoch 37/300, Loss: 0,321998357427036
            atom queen
            king proton
            electron molecule
            Epoch 38/300, Loss: 0,3219630892418974
            atom queen
            king proton
            electron molecule
            Epoch 39: Loss did not decrease. Loss: 0,32199061693086417
            Epoch 39/300, Loss: 0,32199061693086417
            atom queen
            king proton
            electron molecule
            Epoch 40/300, Loss: 0,32195770516495337
            atom queen
            king proton
            electron molecule
            Epoch 41: Loss did not decrease. Loss: 0,3219835964955458
            Epoch 41/300, Loss: 0,3219835964955458
            atom queen
            king proton
            electron molecule
            Epoch 42/300, Loss: 0,3219528991666653
            atom queen
            king proton
            electron molecule
            Epoch 43: Loss did not decrease. Loss: 0,32197722786483834
            Epoch 43/300, Loss: 0,32197722786483834
            atom queen
            king proton
            electron molecule
            Epoch 44/300, Loss: 0,3219486099408411
            atom queen
            king proton
            electron molecule
            Epoch 45: Loss did not decrease. Loss: 0,3219714494441133
            Epoch 45/300, Loss: 0,3219714494441133
            atom queen
            king proton
            electron molecule
            Epoch 46/300, Loss: 0,32194478267538645
            atom queen
            king proton
            electron molecule
            Epoch 47: Loss did not decrease. Loss: 0,3219662056197341
            Epoch 47/300, Loss: 0,3219662056197341
            atom queen
            king proton
            electron molecule
            Epoch 48/300, Loss: 0,3219413683548123
            atom queen
            king proton
            electron molecule
            Epoch 49: Loss did not decrease. Loss: 0,32196144615301064
            Epoch 49/300, Loss: 0,32196144615301064
            atom queen
            king proton
            electron molecule
            Epoch 50/300, Loss: 0,3219383231404009
            atom queen
            king proton
            electron molecule
            Epoch 51: Loss did not decrease. Loss: 0,3219571256397126
            Epoch 51/300, Loss: 0,3219571256397126
            atom queen
            king proton
            electron molecule
            Epoch 52/300, Loss: 0,32193560781879027
            atom queen
            king proton
            electron molecule
            Epoch 53: Loss did not decrease. Loss: 0,32195320302735525
            Epoch 53/300, Loss: 0,32195320302735525
            atom queen
            king proton
            electron molecule
            Epoch 54/300, Loss: 0,3219331873109477
            atom queen
            king proton
            electron molecule
            Epoch 55: Loss did not decrease. Loss: 0,3219496411835095
            Epoch 55/300, Loss: 0,3219496411835095
            atom queen
            king proton
            electron molecule
            Epoch 56/300, Loss: 0,32193103023454184
            atom queen
            king proton
            electron molecule
            Epoch 57: Loss did not decrease. Loss: 0,3219464065092754
            Epoch 57/300, Loss: 0,3219464065092754
            atom queen
            king proton
            electron molecule
            Epoch 58/300, Loss: 0,3219291085136052
            atom queen
            king proton
            electron molecule
            Epoch 59: Loss did not decrease. Loss: 0,3219434685928087
            Epoch 59/300, Loss: 0,3219434685928087
            atom queen
            king proton
            electron molecule
            Epoch 60/300, Loss: 0,32192739703013956
            atom queen
            king proton
            electron molecule
            Epoch 61: Loss did not decrease. Loss: 0,32194079989842905
            Epoch 61/300, Loss: 0,32194079989842905
            atom queen
            king proton
            electron molecule
            Epoch 62/300, Loss: 0,32192587331297173
            atom queen
            king proton
            electron molecule
            Epoch 63: Loss did not decrease. Loss: 0,32193837548739807
            Epoch 63/300, Loss: 0,32193837548739807
            atom queen
            king proton
            electron molecule
            Epoch 64/300, Loss: 0,3219245172597287
            atom queen
            king proton
            electron molecule
            Epoch 65: Loss did not decrease. Loss: 0,3219361727669183
            Epoch 65/300, Loss: 0,3219361727669183
            atom queen
            king proton
            electron molecule
            Epoch 66/300, Loss: 0,3219233108882917
            atom queen
            king proton
            electron molecule
            Epoch 67: Loss did not decrease. Loss: 0,32193417126432683
            Epoch 67/300, Loss: 0,32193417126432683
            atom queen
            king proton
            electron molecule
            Epoch 68/300, Loss: 0,3219222381145148
            atom queen
            king proton
            electron molecule
            Epoch 69: Loss did not decrease. Loss: 0,3219323524238007
            Epoch 69/300, Loss: 0,3219323524238007
            atom queen
            king proton
            electron molecule
            Epoch 70/300, Loss: 0,3219212845533644
            atom queen
            king proton
            electron molecule
            Epoch 71: Loss did not decrease. Loss: 0,3219306994232108
            Epoch 71/300, Loss: 0,3219306994232108
            atom queen
            king proton
            electron molecule
            Epoch 72/300, Loss: 0,3219204373409563
            atom queen
            king proton
            electron molecule
            Epoch 73: Loss did not decrease. Loss: 0,3219291970090202
            Epoch 73/300, Loss: 0,3219291970090202
            atom queen
            king proton
            electron molecule
            Epoch 74/300, Loss: 0,32191968497525664
            atom queen
            king proton
            electron molecule
            Epoch 75: Loss did not decrease. Loss: 0,3219278313473679
            Epoch 75/300, Loss: 0,3219278313473679
            atom queen
            king proton
            electron molecule
            Epoch 76/300, Loss: 0,32191901717345844
            atom queen
            king proton
            electron molecule
            Epoch 77: Loss did not decrease. Loss: 0,32192658988967565
            Epoch 77/300, Loss: 0,32192658988967565
            atom queen
            king proton
            electron molecule
            Epoch 78/300, Loss: 0,321918424744265
            atom queen
            king proton
            electron molecule
            Epoch 79: Loss did not decrease. Loss: 0,3219254612513052
            Epoch 79/300, Loss: 0,3219254612513052
            atom queen
            king proton
            electron molecule
            Epoch 80/300, Loss: 0,3219178994735084
            atom queen
            king proton
            electron molecule
            Epoch 81: Loss did not decrease. Loss: 0,32192443510194696
            Epoch 81/300, Loss: 0,32192443510194696
            atom queen
            king proton
            electron molecule
            Epoch 82/300, Loss: 0,3219174340216964
            atom queen
            king proton
            electron molecule
            Epoch 83: Loss did not decrease. Loss: 0,32192350206656517
            Epoch 83/300, Loss: 0,32192350206656517
            atom queen
            king proton
            electron molecule
            Epoch 84/300, Loss: 0,3219170218322398
            atom queen
            king proton
            electron molecule
            Epoch 85: Loss did not decrease. Loss: 0,32192265363584766
            Epoch 85/300, Loss: 0,32192265363584766
            atom queen
            king proton
            electron molecule
            Epoch 86/300, Loss: 0,3219166570492404
            atom queen
            king proton
            electron molecule
            Epoch 87: Loss did not decrease. Loss: 0,3219218820852173
            Epoch 87/300, Loss: 0,3219218820852173
            atom queen
            king proton
            electron molecule
            Epoch 88/300, Loss: 0,3219163344438411
            atom queen
            king proton
            electron molecule
            Epoch 89: Loss did not decrease. Loss: 0,3219211804015651
            Epoch 89/300, Loss: 0,3219211804015651
            atom queen
            king proton
            electron molecule
            Epoch 90/300, Loss: 0,321916049348248
            atom queen
            king proton
            electron molecule
            Epoch 91: Loss did not decrease. Loss: 0,3219205422169447
            Epoch 91/300, Loss: 0,3219205422169447
            atom queen
            king proton
            electron molecule
            Epoch 92/300, Loss: 0,32191579759662314
            atom queen
            king proton
            electron molecule
            Epoch 93: Loss did not decrease. Loss: 0,3219199617485495
            Epoch 93/300, Loss: 0,3219199617485495
            atom queen
            king proton
            electron molecule
            Epoch 94/300, Loss: 0,32191557547213595
            atom queen
            king proton
            electron molecule
            Epoch 95: Loss did not decrease. Loss: 0,32191943374436577
            Epoch 95/300, Loss: 0,32191943374436577
            atom queen
            king proton
            electron molecule
            Epoch 96/300, Loss: 0,3219153796595345
            atom queen
            king proton
            electron molecule
            Epoch 97: Loss did not decrease. Loss: 0,321918953433946
            Epoch 97/300, Loss: 0,321918953433946
            atom queen
            king proton
            electron molecule
            Epoch 98/300, Loss: 0,3219152072026597
            atom queen
            king proton
            electron molecule
            Epoch 99: Loss did not decrease. Loss: 0,3219185164838174
            Epoch 99/300, Loss: 0,3219185164838174
            atom queen
            king proton
            electron molecule
            Epoch 100/300, Loss: 0,32191505546639804
            atom queen
            king proton
            electron molecule
            Epoch 101: Loss did not decrease. Loss: 0,3219181189570716
            Epoch 101/300, Loss: 0,3219181189570716
            atom queen
            king proton
            electron molecule
            Epoch 102/300, Loss: 0,32191492210260203
            atom queen
            king proton
            electron molecule
            Epoch 103: Loss did not decrease. Loss: 0,32191775727674304
            Epoch 103/300, Loss: 0,32191775727674304
            atom queen
            king proton
            electron molecule
            Epoch 104/300, Loss: 0,3219148050195782
            atom queen
            king proton
            electron molecule
            Epoch 105: Loss did not decrease. Loss: 0,32191742819260943
            Epoch 105/300, Loss: 0,32191742819260943
            atom queen
            king proton
            electron molecule
            Epoch 106/300, Loss: 0,32191470235476477
            atom queen
            king proton
            electron molecule
            Epoch 107: Loss did not decrease. Loss: 0,3219171287510903
            Epoch 107/300, Loss: 0,3219171287510903
            atom queen
            king proton
            electron molecule
            Epoch 108/300, Loss: 0,32191461245027236
            atom queen
            king proton
            electron molecule
            Epoch 109: Loss did not decrease. Loss: 0,3219168562679518
            Epoch 109/300, Loss: 0,3219168562679518
            atom queen
            king proton
            electron molecule
            Epoch 110/300, Loss: 0,32191453383099017
            atom queen
            king proton
            electron molecule
            Epoch 111: Loss did not decrease. Loss: 0,3219166083035475
            Epoch 111/300, Loss: 0,3219166083035475
            atom queen
            king proton
            electron molecule
            Epoch 112/300, Loss: 0,3219144651849909
            atom queen
            king proton
            electron molecule
            Epoch 113: Loss did not decrease. Loss: 0,3219163826403614
            Epoch 113/300, Loss: 0,3219163826403614
            atom queen
            king proton
            electron molecule
            Epoch 114/300, Loss: 0,32191440534599686
            atom queen
            king proton
            electron molecule
            Epoch 115: Loss did not decrease. Loss: 0,32191617726263133
            Epoch 115/300, Loss: 0,32191617726263133
            atom queen
            king proton
            electron molecule
            Epoch 116/300, Loss: 0,32191435327769297
            atom queen
            king proton
            electron molecule
            Epoch 117: Loss did not decrease. Loss: 0,32191599033786
            Epoch 117/300, Loss: 0,32191599033786
            atom queen
            king proton
            electron molecule
            Epoch 118/300, Loss: 0,32191430805969384
            atom queen
            king proton
            electron molecule
            Epoch 119: Loss did not decrease. Loss: 0,3219158202000368
            Epoch 119/300, Loss: 0,3219158202000368
            atom queen
            king proton
            electron molecule
            Epoch 120/300, Loss: 0,32191426887499375
            atom queen
            king proton
            electron molecule
            Epoch 121: Loss did not decrease. Loss: 0,32191566533440763
            Epoch 121/300, Loss: 0,32191566533440763
            atom queen
            king proton
            electron molecule
            Epoch 122/300, Loss: 0,321914234998745
            atom queen
            king proton
            electron molecule
            Epoch 123: Loss did not decrease. Loss: 0,3219155243636516
            Epoch 123/300, Loss: 0,3219155243636516
            atom queen
            king proton
            electron molecule
            Epoch 124/300, Loss: 0,32191420578822705
            atom queen
            king proton
            electron molecule
            Epoch 125: Loss did not decrease. Loss: 0,32191539603533054
            Epoch 125/300, Loss: 0,32191539603533054
            atom queen
            king proton
            electron molecule
            Epoch 126/300, Loss: 0,32191418067388
            atom queen
            king proton
            electron molecule
            Epoch 127: Loss did not decrease. Loss: 0,3219152792104937
            Epoch 127/300, Loss: 0,3219152792104937
            atom queen
            king proton
            electron molecule
            Epoch 128/300, Loss: 0,32191415915129323
            atom queen
            king proton
            electron molecule
            Epoch 129: Loss did not decrease. Loss: 0,3219151728533311
            Epoch 129/300, Loss: 0,3219151728533311
            atom queen
            king proton
            electron molecule
            Epoch 130/300, Loss: 0,3219141407740488
            atom queen
            king proton
            electron molecule
            Epoch 131: Loss did not decrease. Loss: 0,3219150760217763
            Epoch 131/300, Loss: 0,3219150760217763
            atom queen
            king proton
            electron molecule
            Epoch 132/300, Loss: 0,3219141251473295
            atom queen
            king proton
            electron molecule
            Epoch 133: Loss did not decrease. Loss: 0,3219149878589718
            Epoch 133/300, Loss: 0,3219149878589718
            atom queen
            king proton
            electron molecule
            Epoch 134/300, Loss: 0,3219141119222121
            atom queen
            king proton
            electron molecule
            Epoch 135: Loss did not decrease. Loss: 0,32191490758551894
            Epoch 135/300, Loss: 0,32191490758551894
            atom queen
            king proton
            electron molecule
            Epoch 136/300, Loss: 0,321914100790573
            atom queen
            king proton
            electron molecule
            Epoch 137: Loss did not decrease. Loss: 0,32191483449243347
            Epoch 137/300, Loss: 0,32191483449243347
            atom queen
            king proton
            electron molecule
            Epoch 138/300, Loss: 0,32191409148054323
            atom queen
            king proton
            electron molecule
            Epoch 139: Loss did not decrease. Loss: 0,3219147679347502
            Epoch 139/300, Loss: 0,3219147679347502
            atom queen
            king proton
            electron molecule
            Epoch 140/300, Loss: 0,3219140837524532
            atom queen
            king proton
            electron molecule
            Epoch 141: Loss did not decrease. Loss: 0,3219147073257102
            Epoch 141/300, Loss: 0,3219147073257102
            atom queen
            king proton
            electron molecule
            Epoch 142/300, Loss: 0,3219140773952163
            atom queen
            king proton
            electron molecule
            Epoch 143: Loss did not decrease. Loss: 0,32191465213147935
            Epoch 143/300, Loss: 0,32191465213147935
            atom queen
            king proton
            electron molecule
            Epoch 144/300, Loss: 0,3219140722231056
            atom queen
            king proton
            electron molecule
            Epoch 145: Loss did not decrease. Loss: 0,32191460186634985
            Epoch 145/300, Loss: 0,32191460186634985
            atom queen
            king proton
            electron molecule
            Epoch 146/300, Loss: 0,32191406807288053
            atom queen
            king proton
            electron molecule
            Epoch 147: Loss did not decrease. Loss: 0,3219145560883782
            Epoch 147/300, Loss: 0,3219145560883782
            atom queen
            king proton
            electron molecule
            Epoch 148/300, Loss: 0,32191406480122736
            atom queen
            king proton
            electron molecule
            Epoch 149: Loss did not decrease. Loss: 0,32191451439542296
            Epoch 149/300, Loss: 0,32191451439542296
            atom queen
            king proton
            electron molecule
            Epoch 150/300, Loss: 0,3219140622824808
            atom queen
            king proton
            electron molecule
            Epoch 151: Loss did not decrease. Loss: 0,32191447642154053
            Epoch 151/300, Loss: 0,32191447642154053
            atom queen
            king proton
            electron molecule
            Epoch 152/300, Loss: 0,3219140604065945
            atom queen
            king proton
            electron molecule
            Epoch 153: Loss did not decrease. Loss: 0,32191444183371204
            Epoch 153/300, Loss: 0,32191444183371204
            atom queen
            king proton
            electron molecule
            Epoch 154/300, Loss: 0,3219140590773355
            atom queen
            king proton
            electron molecule
            Epoch 155: Loss did not decrease. Loss: 0,321914410328866
            Epoch 155/300, Loss: 0,321914410328866
            atom queen
            king proton
            electron molecule
            Epoch 156/300, Loss: 0,3219140582106795
            atom queen
            king proton
            electron molecule
            Epoch 157: Loss did not decrease. Loss: 0,3219143816311721
            Epoch 157/300, Loss: 0,3219143816311721
            atom queen
            king proton
            electron molecule
            Epoch 158/300, Loss: 0,32191405773338294
            atom queen
            king proton
            electron molecule
            Epoch 159: Loss did not decrease. Loss: 0,3219143554895818
            Epoch 159/300, Loss: 0,3219143554895818
            atom queen
            king proton
            electron molecule
            Epoch 160/300, Loss: 0,32191405758171404
            atom queen
            king proton
            electron molecule
            Epoch 161: Loss did not decrease. Loss: 0,32191433167559036
            Epoch 161/300, Loss: 0,32191433167559036
            atom queen
            king proton
            electron molecule
            Epoch 162/300, Loss: 0,3219140577003271
            atom queen
            king proton
            electron molecule
            Epoch 163: Loss did not decrease. Loss: 0,32191430998120435
            Epoch 163/300, Loss: 0,32191430998120435
            atom queen
            king proton
            electron molecule
            Epoch 164/300, Loss: 0,3219140580412617
            atom queen
            king proton
            electron molecule
            Epoch 165: Loss did not decrease. Loss: 0,3219142902170908
            Epoch 165/300, Loss: 0,3219142902170908
            atom queen
            king proton
            electron molecule
            Epoch 166/300, Loss: 0,3219140585630562
            atom queen
            king proton
            electron molecule
            Epoch 167: Loss did not decrease. Loss: 0,32191427221089636
            Epoch 167/300, Loss: 0,32191427221089636
            atom queen
            king proton
            electron molecule
            Epoch 168/300, Loss: 0,32191405922996036
            atom queen
            king proton
            electron molecule
            Epoch 169: Loss did not decrease. Loss: 0,3219142558057178
            Epoch 169/300, Loss: 0,3219142558057178
            atom queen
            king proton
            electron molecule
            Epoch 170/300, Loss: 0,32191406001123785
            atom queen
            king proton
            electron molecule
            Epoch 171: Loss did not decrease. Loss: 0,32191424085871057
            Epoch 171/300, Loss: 0,32191424085871057
            atom queen
            king proton
            electron molecule
            Epoch 172/300, Loss: 0,32191406088054997
            atom queen
            king proton
            electron molecule
            Epoch 173: Loss did not decrease. Loss: 0,3219142272398238
            Epoch 173/300, Loss: 0,3219142272398238
            atom queen
            king proton
            electron molecule
            Epoch 174/300, Loss: 0,3219140618154073
            atom queen
            king proton
            electron molecule
            Epoch 175: Loss did not decrease. Loss: 0,32191421483064975
            Epoch 175/300, Loss: 0,32191421483064975
            atom queen
            king proton
            electron molecule
            Epoch 176/300, Loss: 0,3219140627966883
            atom queen
            king proton
            electron molecule
            Epoch 177: Loss did not decrease. Loss: 0,3219142035233767
            Epoch 177/300, Loss: 0,3219142035233767
            atom queen
            king proton
            electron molecule
            Epoch 178/300, Loss: 0,32191406380821125
            atom queen
            king proton
            electron molecule
            Epoch 179: Loss did not decrease. Loss: 0,3219141932198372
            Epoch 179/300, Loss: 0,3219141932198372
            atom queen
            king proton
            electron molecule
            Epoch 180/300, Loss: 0,3219140648363563
            atom queen
            king proton
            electron molecule
            Epoch 181: Loss did not decrease. Loss: 0,32191418383064124
            Epoch 181/300, Loss: 0,32191418383064124
            atom queen
            king proton
            electron molecule
            Epoch 182/300, Loss: 0,32191406586973437
            atom queen
            king proton
            electron molecule
            Epoch 183: Loss did not decrease. Loss: 0,321914175274389
            Epoch 183/300, Loss: 0,321914175274389
            atom queen
            king proton
            electron molecule
            Epoch 184/300, Loss: 0,321914066898892
            atom queen
            king proton
            electron molecule
            Epoch 185: Loss did not decrease. Loss: 0,3219141674769526
            Epoch 185/300, Loss: 0,3219141674769526
            atom queen
            king proton
            electron molecule
            Epoch 186/300, Loss: 0,321914067916054
            atom queen
            king proton
            electron molecule
            Epoch 187: Loss did not decrease. Loss: 0,32191416037082526
            Epoch 187/300, Loss: 0,32191416037082526
            atom queen
            king proton
            electron molecule
            Epoch 188/300, Loss: 0,3219140689148958
            atom queen
            king proton
            electron molecule
            Epoch 189: Loss did not decrease. Loss: 0,32191415389452643
            Epoch 189/300, Loss: 0,32191415389452643
            atom queen
            king proton
            electron molecule
            Epoch 190/300, Loss: 0,3219140698903437
            atom queen
            king proton
            electron molecule
            Epoch 191: Loss did not decrease. Loss: 0,32191414799206247
            Epoch 191/300, Loss: 0,32191414799206247
            atom queen
            king proton
            electron molecule
            Epoch 192/300, Loss: 0,3219140708383987
            atom queen
            king proton
            electron molecule
            Epoch 193: Loss did not decrease. Loss: 0,32191414261243445
            Epoch 193/300, Loss: 0,32191414261243445
            atom queen
            king proton
            electron molecule
            Epoch 194/300, Loss: 0,32191407175598413
            atom queen
            king proton
            electron molecule
            Epoch 195: Loss did not decrease. Loss: 0,3219141377091918
            Epoch 195/300, Loss: 0,3219141377091918
            atom queen
            king proton
            electron molecule
            Epoch 196/300, Loss: 0,32191407264080946
            atom queen
            king proton
            electron molecule
            Epoch 197: Loss did not decrease. Loss: 0,321914133240024
            Epoch 197/300, Loss: 0,321914133240024
            atom queen
            king proton
            electron molecule
            Epoch 198/300, Loss: 0,32191407349125317
            atom queen
            king proton
            electron molecule
            Epoch 199: Loss did not decrease. Loss: 0,3219141291663913
            Epoch 199/300, Loss: 0,3219141291663913
            atom queen
            king proton
            electron molecule
            Epoch 200/300, Loss: 0,3219140743062599
            atom queen
            king proton
            electron molecule
            Epoch 201: Loss did not decrease. Loss: 0,3219141254531864
            Epoch 201/300, Loss: 0,3219141254531864
            atom queen
            king proton
            electron molecule
            Epoch 202/300, Loss: 0,32191407508525055
            atom queen
            king proton
            electron molecule
            Epoch 203: Loss did not decrease. Loss: 0,32191412206842857
            Epoch 203/300, Loss: 0,32191412206842857
            atom queen
            king proton
            electron molecule
            Epoch 204/300, Loss: 0,32191407582804477
            atom queen
            king proton
            electron molecule
            Epoch 205: Loss did not decrease. Loss: 0,3219141189829839
            Epoch 205/300, Loss: 0,3219141189829839
            atom queen
            king proton
            electron molecule
            Epoch 206/300, Loss: 0,3219140765347925
            atom queen
            king proton
            electron molecule
            Epoch 207: Loss did not decrease. Loss: 0,32191411617031046
            Epoch 207/300, Loss: 0,32191411617031046
            atom queen
            king proton
            electron molecule
            Epoch 208/300, Loss: 0,32191407720591564
            atom queen
            king proton
            electron molecule
            Epoch 209: Loss did not decrease. Loss: 0,3219141136062276
            Epoch 209/300, Loss: 0,3219141136062276
            atom queen
            king proton
            electron molecule
            Epoch 210/300, Loss: 0,32191407784205733
            atom queen
            king proton
            electron molecule
            Epoch 211: Loss did not decrease. Loss: 0,3219141112687049
            Epoch 211/300, Loss: 0,3219141112687049
            atom queen
            king proton
            electron molecule
            Epoch 212/300, Loss: 0,32191407844403824
            atom queen
            king proton
            electron molecule
            Epoch 213: Loss did not decrease. Loss: 0,3219141091376701
            Epoch 213/300, Loss: 0,3219141091376701
            atom queen
            king proton
            electron molecule
            Epoch 214/300, Loss: 0,3219140790128186
            atom queen
            king proton
            electron molecule
            Epoch 215: Loss did not decrease. Loss: 0,3219141071948344
            Epoch 215/300, Loss: 0,3219141071948344
            atom queen
            king proton
            electron molecule
            Epoch 216/300, Loss: 0,32191407954946655
            atom queen
            king proton
            electron molecule
            Epoch 217: Loss did not decrease. Loss: 0,32191410542353405
            Epoch 217/300, Loss: 0,32191410542353405
            atom queen
            king proton
            electron molecule
            Epoch 218/300, Loss: 0,3219140800551298
            atom queen
            king proton
            electron molecule
            Epoch 219: Loss did not decrease. Loss: 0,32191410380858504
            Epoch 219/300, Loss: 0,32191410380858504
            atom queen
            king proton
            electron molecule
            Epoch 220/300, Loss: 0,32191408053101317
            atom queen
            king proton
            electron molecule
            Epoch 221: Loss did not decrease. Loss: 0,32191410233615125
            Epoch 221/300, Loss: 0,32191410233615125
            atom queen
            king proton
            electron molecule
            Epoch 222/300, Loss: 0,321914080978358
            atom queen
            king proton
            electron molecule
            Epoch 223: Loss did not decrease. Loss: 0,32191410099362455
            Epoch 223/300, Loss: 0,32191410099362455
            atom queen
            king proton
            electron molecule
            Epoch 224/300, Loss: 0,32191408139842576
            atom queen
            king proton
            electron molecule
            Epoch 225: Loss did not decrease. Loss: 0,3219140997695153
            Epoch 225/300, Loss: 0,3219140997695153
            atom queen
            king proton
            electron molecule
            Epoch 226/300, Loss: 0,32191408179248454
            atom queen
            king proton
            electron molecule
            Epoch 227: Loss did not decrease. Loss: 0,3219140986533529
            Epoch 227/300, Loss: 0,3219140986533529
            atom queen
            king proton
            electron molecule
            Epoch 228/300, Loss: 0,3219140821617966
            atom queen
            king proton
            electron molecule
            Epoch 229: Loss did not decrease. Loss: 0,3219140976355952
            Epoch 229/300, Loss: 0,3219140976355952
            atom queen
            king proton
            electron molecule
            Epoch 230/300, Loss: 0,3219140825076101
            atom queen
            king proton
            electron molecule
            Epoch 231: Loss did not decrease. Loss: 0,321914096707546
            Epoch 231/300, Loss: 0,321914096707546
            atom queen
            king proton
            electron molecule
            Epoch 232/300, Loss: 0,3219140828311495
            atom queen
            king proton
            electron molecule
            Epoch 233: Loss did not decrease. Loss: 0,32191409586127945
            Epoch 233/300, Loss: 0,32191409586127945
            atom queen
            king proton
            electron molecule
            Epoch 234/300, Loss: 0,32191408313361153
            atom queen
            king proton
            electron molecule
            Epoch 235: Loss did not decrease. Loss: 0,32191409508957175
            Epoch 235/300, Loss: 0,32191409508957175
            atom queen
            king proton
            electron molecule
            Epoch 236/300, Loss: 0,3219140834161585
            atom queen
            king proton
            electron molecule
            Epoch 237: Loss did not decrease. Loss: 0,32191409438583923
            Epoch 237/300, Loss: 0,32191409438583923
            atom queen
            king proton
            electron molecule
            Epoch 238/300, Loss: 0,321914083679916
            atom queen
            king proton
            electron molecule
            Epoch 239: Loss did not decrease. Loss: 0,3219140937440807
            Epoch 239/300, Loss: 0,3219140937440807
            atom queen
            king proton
            electron molecule
            Epoch 240/300, Loss: 0,3219140839259688
            atom queen
            king proton
            electron molecule
            Epoch 241: Loss did not decrease. Loss: 0,3219140931588263
            Epoch 241/300, Loss: 0,3219140931588263
            atom queen
            king proton
            electron molecule
            Epoch 242/300, Loss: 0,32191408415535994
            atom queen
            king proton
            electron molecule
            Epoch 243: Loss did not decrease. Loss: 0,32191409262509013
            Epoch 243/300, Loss: 0,32191409262509013
            atom queen
            king proton
            electron molecule
            Epoch 244/300, Loss: 0,32191408436908836
            atom queen
            king proton
            electron molecule
            Epoch 245: Loss did not decrease. Loss: 0,3219140921383275
            Epoch 245/300, Loss: 0,3219140921383275
            atom queen
            king proton
            electron molecule
            Epoch 246/300, Loss: 0,32191408456810955
            atom queen
            king proton
            electron molecule
            Epoch 247: Loss did not decrease. Loss: 0,3219140916943954
            Epoch 247/300, Loss: 0,3219140916943954
            atom queen
            king proton
            electron molecule
            Epoch 248/300, Loss: 0,3219140847533339
            atom queen
            king proton
            electron molecule
            Epoch 249: Loss did not decrease. Loss: 0,32191409128951726
            Epoch 249/300, Loss: 0,32191409128951726
            atom queen
            king proton
            electron molecule
            Epoch 250/300, Loss: 0,321914084925628
            atom queen
            king proton
            electron molecule
            Epoch 251: Loss did not decrease. Loss: 0,3219140909202502
            Epoch 251/300, Loss: 0,3219140909202502
            atom queen
            king proton
            electron molecule
            Epoch 252/300, Loss: 0,3219140850858147
            atom queen
            king proton
            electron molecule
            Epoch 253: Loss did not decrease. Loss: 0,3219140905834557
            Epoch 253/300, Loss: 0,3219140905834557
            atom queen
            king proton
            electron molecule
            Epoch 254/300, Loss: 0,32191408523467374
            atom queen
            king proton
            electron molecule
            Epoch 255: Loss did not decrease. Loss: 0,3219140902762724
            Epoch 255/300, Loss: 0,3219140902762724
            atom queen
            king proton
            electron molecule
            Epoch 256/300, Loss: 0,32191408537294336
            atom queen
            king proton
            electron molecule
            Epoch 257: Loss did not decrease. Loss: 0,3219140899960916
            Epoch 257/300, Loss: 0,3219140899960916
            atom queen
            king proton
            electron molecule
            Epoch 258/300, Loss: 0,32191408550132056
            atom queen
            king proton
            electron molecule
            Epoch 259: Loss did not decrease. Loss: 0,3219140897405352
            Epoch 259/300, Loss: 0,3219140897405352
            atom queen
            king proton
            electron molecule
            Epoch 260/300, Loss: 0,3219140856204636
            atom queen
            king proton
            electron molecule
            Epoch 261: Loss did not decrease. Loss: 0,3219140895074348
            Epoch 261/300, Loss: 0,3219140895074348
            atom queen
            king proton
            electron molecule
            Epoch 262/300, Loss: 0,3219140857309916
            atom queen
            king proton
            electron molecule
            Epoch 263: Loss did not decrease. Loss: 0,32191408929481335
            Epoch 263/300, Loss: 0,32191408929481335
            atom queen
            king proton
            electron molecule
            Epoch 264/300, Loss: 0,32191408583348896
            atom queen
            king proton
            electron molecule
            Epoch 265: Loss did not decrease. Loss: 0,32191408910086833
            Epoch 265/300, Loss: 0,32191408910086833
            atom queen
            king proton
            electron molecule
            Epoch 266/300, Loss: 0,3219140859285033
            atom queen
            king proton
            electron molecule
            Epoch 267: Loss did not decrease. Loss: 0,32191408892395607
            Epoch 267/300, Loss: 0,32191408892395607
            atom queen
            king proton
            electron molecule
            Epoch 268/300, Loss: 0,3219140860165499
            atom queen
            king proton
            electron molecule
            Epoch 269: Loss did not decrease. Loss: 0,32191408876257804
            Epoch 269/300, Loss: 0,32191408876257804
            atom queen
            king proton
            electron molecule
            Epoch 270/300, Loss: 0,3219140860981116
            atom queen
            king proton
            electron molecule
            Epoch 271: Loss did not decrease. Loss: 0,32191408861536813
            Epoch 271/300, Loss: 0,32191408861536813
            atom queen
            king proton
            electron molecule
            Epoch 272/300, Loss: 0,3219140861736412
            atom queen
            king proton
            electron molecule
            Epoch 273: Loss did not decrease. Loss: 0,3219140884810798
            Epoch 273/300, Loss: 0,3219140884810798
            atom queen
            king proton
            electron molecule
            Epoch 274/300, Loss: 0,32191408624356266
            atom queen
            king proton
            electron molecule
            Epoch 275: Loss did not decrease. Loss: 0,3219140883585769
            Epoch 275/300, Loss: 0,3219140883585769
            atom queen
            king proton
            electron molecule
            Epoch 276/300, Loss: 0,32191408630827245
            atom queen
            king proton
            electron molecule
            Epoch 277: Loss did not decrease. Loss: 0,3219140882468236
            Epoch 277/300, Loss: 0,3219140882468236
            atom queen
            king proton
            electron molecule
            Epoch 278/300, Loss: 0,3219140863681414
            atom queen
            king proton
            electron molecule
            Epoch 279: Loss did not decrease. Loss: 0,3219140881448748
            Epoch 279/300, Loss: 0,3219140881448748
            atom queen
            king proton
            electron molecule
            Epoch 280/300, Loss: 0,32191408642351577
            atom queen
            king proton
            electron molecule
            Epoch 281: Loss did not decrease. Loss: 0,321914088051869
            Epoch 281/300, Loss: 0,321914088051869
            atom queen
            king proton
            electron molecule
            Epoch 282/300, Loss: 0,32191408647471903
            atom queen
            king proton
            electron molecule
            Epoch 283: Loss did not decrease. Loss: 0,32191408796702053
            Epoch 283/300, Loss: 0,32191408796702053
            atom queen
            king proton
            electron molecule
            Epoch 284/300, Loss: 0,3219140865220527
            atom queen
            king proton
            electron molecule
            Epoch 285: Loss did not decrease. Loss: 0,32191408788961245
            Epoch 285/300, Loss: 0,32191408788961245
            atom queen
            king proton
            electron molecule
            Epoch 286/300, Loss: 0,3219140865657979
            atom queen
            king proton
            electron molecule
            Epoch 287: Loss did not decrease. Loss: 0,3219140878189916
            Epoch 287/300, Loss: 0,3219140878189916
            atom queen
            king proton
            electron molecule
            Epoch 288/300, Loss: 0,32191408660621657
            atom queen
            king proton
            electron molecule
            Epoch 289: Loss did not decrease. Loss: 0,3219140877545619
            Epoch 289/300, Loss: 0,3219140877545619
            atom queen
            king proton
            electron molecule
            Epoch 290/300, Loss: 0,3219140866435527
            atom queen
            king proton
            electron molecule
            Epoch 291: Loss did not decrease. Loss: 0,3219140876957795
            Epoch 291/300, Loss: 0,3219140876957795
            atom queen
            king proton
            electron molecule
            Epoch 292/300, Loss: 0,3219140866780334
            atom queen
            king proton
            electron molecule
            Epoch 293: Loss did not decrease. Loss: 0,32191408764214896
            Epoch 293/300, Loss: 0,32191408764214896
            atom queen
            king proton
            electron molecule
            Epoch 294/300, Loss: 0,3219140867098698
            atom queen
            king proton
            electron molecule
            Epoch 295: Loss did not decrease. Loss: 0,32191408759321793
            Epoch 295/300, Loss: 0,32191408759321793
            atom queen
            king proton
            electron molecule
            Epoch 296/300, Loss: 0,3219140867392582
            atom queen
            king proton
            electron molecule
            Epoch 297: Loss did not decrease. Loss: 0,321914087548574
            Epoch 297/300, Loss: 0,321914087548574
            atom queen
            king proton
            electron molecule
            Epoch 298/300, Loss: 0,32191408676638117
            atom queen
            king proton
            electron molecule
            Epoch 299: Loss did not decrease. Loss: 0,32191408750784106
            Epoch 299/300, Loss: 0,32191408750784106
            atom queen
            king proton
            electron molecule
            Epoch 300/300, Loss: 0,32191408679140815
            */


            /*
            atom wely
            king ainge
            electron naseerullah
            Epoch 1/300, Loss: 6,660198173325651
            atom prospectus
            king wely
            electron molecule
            Epoch 2/300, Loss: 5,931156629627307
            atom queen
            king proton
            electron molecule
            Epoch 3/300, Loss: 0,34713353730518964
            atom queen
            king proton
            electron molecule
            Epoch 4/300, Loss: 0,32333078110578856
            atom queen
            king proton
            electron molecule
            Epoch 5/300, Loss: 0,3224622454252393
            atom queen
            king proton
            electron molecule
            Epoch 6/300, Loss: 0,3221014976749052
            atom queen
            king proton
            electron molecule
            Epoch 7: Loss did not decrease. Loss: 0,32219483898249457
            Epoch 7/300, Loss: 0,32219483898249457
            atom queen
            king proton
            electron molecule
            Epoch 8/300, Loss: 0,3220318446866761
            atom queen
            king proton
            electron molecule
            Epoch 9: Loss did not decrease. Loss: 0,3221525072554389
            Epoch 9/300, Loss: 0,3221525072554389
            atom queen
            king proton
            electron molecule
            Epoch 10/300, Loss: 0,3220156050786734
            atom queen
            king proton
            electron molecule
            Epoch 11: Loss did not decrease. Loss: 0,32212665823705783
            Epoch 11/300, Loss: 0,32212665823705783
            atom queen
            king proton
            electron molecule
            Epoch 12/300, Loss: 0,3220050329761924
            atom queen
            king proton
            electron molecule
            Epoch 13: Loss did not decrease. Loss: 0,32210466180004454
            Epoch 13/300, Loss: 0,32210466180004454
            atom queen
            king proton
            electron molecule
            Epoch 14/300, Loss: 0,32199594641285767
            atom queen
            king proton
            electron molecule
            Epoch 15: Loss did not decrease. Loss: 0,32208504566969104
            Epoch 15/300, Loss: 0,32208504566969104
            atom queen
            king proton
            electron molecule
            Epoch 16/300, Loss: 0,3219878184921507
            atom queen
            king proton
            electron molecule
            Epoch 17: Loss did not decrease. Loss: 0,3220674513460098
            Epoch 17/300, Loss: 0,3220674513460098
            atom queen
            king proton
            electron molecule
            Epoch 18/300, Loss: 0,3219805111687317
            atom queen
            king proton
            electron molecule
            Epoch 19: Loss did not decrease. Loss: 0,3220516597552113
            Epoch 19/300, Loss: 0,3220516597552113
            atom queen
            king proton
            electron molecule
            Epoch 20/300, Loss: 0,32197393648108735
            atom queen
            king proton
            electron molecule
            Epoch 21: Loss did not decrease. Loss: 0,32203748536820714
            Epoch 21/300, Loss: 0,32203748536820714
            atom queen
            king proton
            electron molecule
            Epoch 22/300, Loss: 0,32196801947177683
            atom queen
            king proton
            electron molecule
            Epoch 23: Loss did not decrease. Loss: 0,32202476286863363
            Epoch 23/300, Loss: 0,32202476286863363
            atom queen
            king proton
            electron molecule
            Epoch 24/300, Loss: 0,3219626933861944
            atom queen
            king proton
            electron molecule
            Epoch 25: Loss did not decrease. Loss: 0,32201334399050946
            Epoch 25/300, Loss: 0,32201334399050946
            atom queen
            king proton
            electron molecule
            Epoch 26/300, Loss: 0,32195789842467315
            atom queen
            king proton
            electron molecule
            Epoch 27: Loss did not decrease. Loss: 0,32200309563783736
            Epoch 27/300, Loss: 0,32200309563783736
            atom queen
            king proton
            electron molecule
            Epoch 28/300, Loss: 0,321953580959308
            atom queen
            king proton
            electron molecule
            Epoch 29: Loss did not decrease. Loss: 0,32199389830679204
            Epoch 29/300, Loss: 0,32199389830679204
            atom queen
            king proton
            electron molecule
            Epoch 30/300, Loss: 0,32194969287386505
            atom queen
            king proton
            electron molecule
            Epoch 31: Loss did not decrease. Loss: 0,3219856446861457
            Epoch 31/300, Loss: 0,3219856446861457
            atom queen
            king proton
            electron molecule
            Epoch 32/300, Loss: 0,3219461909811293
            atom queen
            king proton
            electron molecule
            Epoch 33: Loss did not decrease. Loss: 0,32197823840614914
            Epoch 33/300, Loss: 0,32197823840614914
            atom queen
            king proton
            electron molecule
            Epoch 34/300, Loss: 0,32194303650504147
            atom queen
            king proton
            electron molecule
            Epoch 35: Loss did not decrease. Loss: 0,3219715929183535
            Epoch 35/300, Loss: 0,3219715929183535
            atom queen
            king proton
            electron molecule
            Epoch 36/300, Loss: 0,32194019461956935
            atom queen
            king proton
            electron molecule
            Epoch 37: Loss did not decrease. Loss: 0,32196563049192317
            Epoch 37/300, Loss: 0,32196563049192317
            atom queen
            king proton
            electron molecule
            Epoch 38/300, Loss: 0,3219376340376313
            atom queen
            king proton
            electron molecule
            Epoch 39: Loss did not decrease. Loss: 0,3219602813138309
            Epoch 39/300, Loss: 0,3219602813138309
            atom queen
            king proton
            electron molecule
            Epoch 40/300, Loss: 0,32193532664428864
            atom queen
            king proton
            electron molecule
            Epoch 41: Loss did not decrease. Loss: 0,3219554826818151
            Epoch 41/300, Loss: 0,3219554826818151
            atom queen
            king proton
            electron molecule
            Epoch 42/300, Loss: 0,3219332471691676
            atom queen
            king proton
            electron molecule
            Epoch 43: Loss did not decrease. Loss: 0,3219511782802586
            Epoch 43/300, Loss: 0,3219511782802586
            atom queen
            king proton
            electron molecule
            Epoch 44/300, Loss: 0,3219313728936893
            atom queen
            king proton
            electron molecule
            Epoch 45: Loss did not decrease. Loss: 0,3219473175302665
            Epoch 45/300, Loss: 0,3219473175302665
            atom queen
            king proton
            electron molecule
            Epoch 46/300, Loss: 0,32192968338923494
            atom queen
            king proton
            electron molecule
            Epoch 47: Loss did not decrease. Loss: 0,32194385500618256
            Epoch 47/300, Loss: 0,32194385500618256
            atom queen
            king proton
            electron molecule
            Epoch 48/300, Loss: 0,3219281602828338
            atom queen
            king proton
            electron molecule
            Epoch 49: Loss did not decrease. Loss: 0,3219407499116454
            Epoch 49/300, Loss: 0,3219407499116454
            atom queen
            king proton
            electron molecule
            Epoch 50/300, Loss: 0,3219267870473648
            atom queen
            king proton
            electron molecule
            Epoch 51: Loss did not decrease. Loss: 0,3219379656090211
            Epoch 51/300, Loss: 0,3219379656090211
            atom queen
            king proton
            electron molecule
            Epoch 52/300, Loss: 0,32192554881361646
            atom queen
            king proton
            electron molecule
            Epoch 53: Loss did not decrease. Loss: 0,32193546919672006
            Epoch 53/300, Loss: 0,32193546919672006
            atom queen
            king proton
            electron molecule
            Epoch 54/300, Loss: 0,32192443220184924
            atom queen
            king proton
            electron molecule
            Epoch 55: Loss did not decrease. Loss: 0,32193323112947986
            Epoch 55/300, Loss: 0,32193323112947986
            atom queen
            king proton
            electron molecule
            Epoch 56/300, Loss: 0,3219234251707803
            atom queen
            king proton
            electron molecule
            Epoch 57: Loss did not decrease. Loss: 0,3219312248772223
            Epoch 57/300, Loss: 0,3219312248772223
            atom queen
            king proton
            electron molecule
            Epoch 58/300, Loss: 0,3219225168821344
            atom queen
            king proton
            electron molecule
            Epoch 59: Loss did not decrease. Loss: 0,32192942661854
            Epoch 59/300, Loss: 0,32192942661854
            atom queen
            king proton
            electron molecule
            Epoch 60/300, Loss: 0,3219216975791171
            atom queen
            king proton
            electron molecule
            Epoch 61: Loss did not decrease. Loss: 0,32192781496529177
            Epoch 61/300, Loss: 0,32192781496529177
            atom queen
            king proton
            electron molecule
            Epoch 62/300, Loss: 0,32192095847734664
            atom queen
            king proton
            electron molecule
            Epoch 63: Loss did not decrease. Loss: 0,3219263707151366
            Epoch 63/300, Loss: 0,3219263707151366
            atom queen
            king proton
            electron molecule
            Epoch 64/300, Loss: 0,3219202916669393
            atom queen
            king proton
            electron molecule
            Epoch 65: Loss did not decrease. Loss: 0,3219250766291723
            Epoch 65/300, Loss: 0,3219250766291723
            atom queen
            king proton
            electron molecule
            Epoch 66/300, Loss: 0,3219196900245838
            atom queen
            king proton
            electron molecule
            Epoch 67: Loss did not decrease. Loss: 0,32192391723212727
            Epoch 67/300, Loss: 0,32192391723212727
            atom queen
            king proton
            electron molecule
            Epoch 68/300, Loss: 0,3219191471345688
            atom queen
            king proton
            electron molecule
            Epoch 69: Loss did not decrease. Loss: 0,3219228786328227
            Epoch 69/300, Loss: 0,3219228786328227
            atom queen
            king proton
            electron molecule
            Epoch 70/300, Loss: 0,32191865721783325
            atom queen
            king proton
            electron molecule
            Epoch 71: Loss did not decrease. Loss: 0,3219219483628477
            Epoch 71/300, Loss: 0,3219219483628477
            atom queen
            king proton
            electron molecule
            Epoch 72/300, Loss: 0,3219182150682141
            atom queen
            king proton
            electron molecule
            Epoch 73: Loss did not decrease. Loss: 0,3219211152316023
            Epoch 73/300, Loss: 0,3219211152316023
            atom queen
            king proton
            electron molecule
            Epoch 74/300, Loss: 0,32191781599514785
            atom queen
            king proton
            electron molecule
            Epoch 75: Loss did not decrease. Loss: 0,3219203691960544
            Epoch 75/300, Loss: 0,3219203691960544
            atom queen
            king proton
            electron molecule
            Epoch 76/300, Loss: 0,32191745577216485
            atom queen
            king proton
            electron molecule
            Epoch 77: Loss did not decrease. Loss: 0,3219197012437161
            Epoch 77/300, Loss: 0,3219197012437161
            atom queen
            king proton
            electron molecule
            Epoch 78/300, Loss: 0,32191713059058075
            atom queen
            king proton
            electron molecule
            Epoch 79: Loss did not decrease. Loss: 0,3219191032875037
            Epoch 79/300, Loss: 0,3219191032875037
            atom queen
            king proton
            electron molecule
            Epoch 80/300, Loss: 0,32191683701785595
            atom queen
            king proton
            electron molecule
            Epoch 81: Loss did not decrease. Loss: 0,32191856807127833
            Epoch 81/300, Loss: 0,32191856807127833
            atom queen
            king proton
            electron molecule
            Epoch 82/300, Loss: 0,32191657196014484
            atom queen
            king proton
            electron molecule
            Epoch 83: Loss did not decrease. Loss: 0,3219180890849837
            Epoch 83/300, Loss: 0,3219180890849837
            atom queen
            king proton
            electron molecule
            Epoch 84/300, Loss: 0,3219163326286095
            atom queen
            king proton
            electron molecule
            Epoch 85: Loss did not decrease. Loss: 0,32191766048841064
            Epoch 85/300, Loss: 0,32191766048841064
            atom queen
            king proton
            electron molecule
            Epoch 86/300, Loss: 0,3219161165091142
            atom queen
            king proton
            electron molecule
            Epoch 87: Loss did not decrease. Loss: 0,321917277042715
            Epoch 87/300, Loss: 0,321917277042715
            atom queen
            king proton
            electron molecule
            Epoch 88/300, Loss: 0,3219159213349582
            atom queen
            king proton
            electron molecule
            Epoch 89: Loss did not decrease. Loss: 0,3219169340489008
            Epoch 89/300, Loss: 0,3219169340489008
            atom queen
            king proton
            electron molecule
            Epoch 90/300, Loss: 0,3219157450623387
            atom queen
            king proton
            electron molecule
            Epoch 91: Loss did not decrease. Loss: 0,3219166272925654
            Epoch 91/300, Loss: 0,3219166272925654
            atom queen
            king proton
            electron molecule
            Epoch 92/300, Loss: 0,32191558584826796
            atom queen
            king proton
            electron molecule
            Epoch 93: Loss did not decrease. Loss: 0,3219163529942717
            Epoch 93/300, Loss: 0,3219163529942717
            atom queen
            king proton
            electron molecule
            Epoch 94/300, Loss: 0,32191544203069644
            atom queen
            king proton
            electron molecule
            Epoch 95: Loss did not decrease. Loss: 0,3219161077649728
            Epoch 95/300, Loss: 0,3219161077649728
            atom queen
            king proton
            electron molecule
            Epoch 96/300, Loss: 0,32191531211061947
            atom queen
            king proton
            electron molecule
            Epoch 97: Loss did not decrease. Loss: 0,321915888565984
            Epoch 97/300, Loss: 0,321915888565984
            atom queen
            king proton
            electron molecule
            Epoch 98/300, Loss: 0,3219151947359678
            atom queen
            king proton
            electron molecule
            Epoch 99: Loss did not decrease. Loss: 0,32191569267303183
            Epoch 99/300, Loss: 0,32191569267303183
            atom queen
            king proton
            electron molecule
            Epoch 100/300, Loss: 0,3219150886871025
            atom queen
            king proton
            electron molecule
            Epoch 101: Loss did not decrease. Loss: 0,3219155176439741
            Epoch 101/300, Loss: 0,3219155176439741
            atom queen
            king proton
            electron molecule
            Epoch 102/300, Loss: 0,32191499286375264
            atom queen
            king proton
            electron molecule
            Epoch 103: Loss did not decrease. Loss: 0,3219153612898124
            Epoch 103/300, Loss: 0,3219153612898124
            atom queen
            king proton
            electron molecule
            Epoch 104/300, Loss: 0,3219149062732514
            atom queen
            king proton
            electron molecule
            Epoch 105: Loss did not decrease. Loss: 0,3219152216486662
            Epoch 105/300, Loss: 0,3219152216486662
            atom queen
            king proton
            electron molecule
            Epoch 106/300, Loss: 0,32191482801994015
            atom queen
            king proton
            electron molecule
            Epoch 107: Loss did not decrease. Loss: 0,3219150969624056
            Epoch 107/300, Loss: 0,3219150969624056
            atom queen
            king proton
            electron molecule
            Epoch 108/300, Loss: 0,32191475729562347
            atom queen
            king proton
            electron molecule
            Epoch 109: Loss did not decrease. Loss: 0,32191498565567245
            Epoch 109/300, Loss: 0,32191498565567245
            atom queen
            king proton
            electron molecule
            Epoch 110/300, Loss: 0,3219146933709713
            atom queen
            king proton
            electron molecule
            Epoch 111: Loss did not decrease. Loss: 0,3219148863170486
            Epoch 111/300, Loss: 0,3219148863170486
            atom queen
            king proton
            electron molecule
            Epoch 112/300, Loss: 0,3219146355877704
            atom queen
            king proton
            electron molecule
            Epoch 113: Loss did not decrease. Loss: 0,32191479768215026
            Epoch 113/300, Loss: 0,32191479768215026
            atom queen
            king proton
            electron molecule
            Epoch 114/300, Loss: 0,32191458335194434
            atom queen
            king proton
            electron molecule
            Epoch 115: Loss did not decrease. Loss: 0,32191471861845394
            Epoch 115/300, Loss: 0,32191471861845394
            atom queen
            king proton
            electron molecule
            Epoch 116/300, Loss: 0,32191453612726234
            atom queen
            king proton
            electron molecule
            Epoch 117: Loss did not decrease. Loss: 0,3219146481116782
            Epoch 117/300, Loss: 0,3219146481116782
            atom queen
            king proton
            electron molecule
            Epoch 118/300, Loss: 0,32191449342966977
            atom queen
            king proton
            electron molecule
            Epoch 119: Loss did not decrease. Loss: 0,3219145852535597
            Epoch 119/300, Loss: 0,3219145852535597
            atom queen
            king proton
            electron molecule
            Epoch 120/300, Loss: 0,32191445482217823
            atom queen
            king proton
            electron molecule
            Epoch 121: Loss did not decrease. Loss: 0,32191452923088576
            Epoch 121/300, Loss: 0,32191452923088576
            atom queen
            king proton
            electron molecule
            Epoch 122/300, Loss: 0,321914419910259
            atom queen
            king proton
            electron molecule
            Epoch 123: Loss did not decrease. Loss: 0,32191447931565076
            Epoch 123/300, Loss: 0,32191447931565076
            atom queen
            king proton
            electron molecule
            Epoch 124/300, Loss: 0,3219143883376917
            atom queen
            king proton
            electron molecule
            Epoch 125: Loss did not decrease. Loss: 0,3219144348562252
            Epoch 125/300, Loss: 0,3219144348562252
            atom queen
            king proton
            electron molecule
            Epoch 126/300, Loss: 0,3219143597828197
            atom queen
            king proton
            electron molecule
            Epoch 127: Loss did not decrease. Loss: 0,3219143952694321
            Epoch 127/300, Loss: 0,3219143952694321
            atom queen
            king proton
            electron molecule
            Epoch 128/300, Loss: 0,3219143339551741
            atom queen
            king proton
            electron molecule
            Epoch 129: Loss did not decrease. Loss: 0,32191436003343815
            Epoch 129/300, Loss: 0,32191436003343815
            atom queen
            king proton
            electron molecule
            Epoch 130/300, Loss: 0,32191431059243264
            atom queen
            king proton
            electron molecule
            Epoch 131: Loss did not decrease. Loss: 0,32191432868137754
            Epoch 131/300, Loss: 0,32191432868137754
            atom queen
            king proton
            electron molecule
            Epoch 132/300, Loss: 0,32191428945767253
            atom queen
            king proton
            electron molecule
            Epoch 133: Loss did not decrease. Loss: 0,32191430079563205
            Epoch 133/300, Loss: 0,32191430079563205
            atom queen
            king proton
            electron molecule
            Epoch 134/300, Loss: 0,32191427033689657
            atom queen
            king proton
            electron molecule
            Epoch 135: Loss did not decrease. Loss: 0,3219142760027018
            Epoch 135/300, Loss: 0,3219142760027018
            atom queen
            king proton
            electron molecule
            Epoch 136/300, Loss: 0,3219142530368
            atom queen
            king proton
            electron molecule
            Epoch 137: Loss did not decrease. Loss: 0,3219142539686029
            Epoch 137/300, Loss: 0,3219142539686029
            atom queen
            king proton
            electron molecule
            Epoch 138/300, Loss: 0,3219142373827571
            atom queen
            king proton
            electron molecule
            Epoch 139/300, Loss: 0,32191423439474415
            atom queen
            king proton
            electron molecule
            Epoch 140/300, Loss: 0,32191422321700486
            atom queen
            king proton
            electron molecule
            Epoch 141/300, Loss: 0,3219142170142269
            atom queen
            king proton
            electron molecule
            Epoch 142/300, Loss: 0,32191421039700324
            atom queen
            king proton
            electron molecule
            Epoch 143/300, Loss: 0,32191420158852835
            atom queen
            king proton
            electron molecule
            Epoch 144/300, Loss: 0,32191419879395794
            atom queen
            king proton
            electron molecule
            Epoch 145/300, Loss: 0,32191418790452936
            atom queen
            king proton
            electron molecule
            Epoch 146: Loss did not decrease. Loss: 0,32191418829148594
            Epoch 146/300, Loss: 0,32191418829148594
            atom queen
            king proton
            electron molecule
            Epoch 147/300, Loss: 0,3219141757718487
            atom queen
            king proton
            electron molecule
            Epoch 148: Loss did not decrease. Loss: 0,32191417878441114
            Epoch 148/300, Loss: 0,32191417878441114
            atom queen
            king proton
            electron molecule
            Epoch 149/300, Loss: 0,3219141650204553
            atom queen
            king proton
            electron molecule
            Epoch 150: Loss did not decrease. Loss: 0,3219141701776788
            Epoch 150/300, Loss: 0,3219141701776788
            atom queen
            king proton
            electron molecule
            Epoch 151/300, Loss: 0,3219141554985275
            atom queen
            king proton
            electron molecule
            Epoch 152: Loss did not decrease. Loss: 0,3219141623853752
            Epoch 152/300, Loss: 0,3219141623853752
            atom queen
            king proton
            electron molecule
            Epoch 153/300, Loss: 0,3219141470705351
            atom queen
            king proton
            electron molecule
            Epoch 154: Loss did not decrease. Loss: 0,3219141553298427
            Epoch 154/300, Loss: 0,3219141553298427
            atom queen
            king proton
            electron molecule
            Epoch 155/300, Loss: 0,32191413961552057
            atom queen
            king proton
            electron molecule
            Epoch 156: Loss did not decrease. Loss: 0,32191414894088105
            Epoch 156/300, Loss: 0,32191414894088105
            atom queen
            king proton
            electron molecule
            Epoch 157/300, Loss: 0,32191413302556054
            atom queen
            king proton
            electron molecule
            Epoch 158: Loss did not decrease. Loss: 0,32191414315502653
            Epoch 158/300, Loss: 0,32191414315502653
            atom queen
            king proton
            electron molecule
            Epoch 159/300, Loss: 0,32191412720438567
            atom queen
            king proton
            electron molecule
            Epoch 160: Loss did not decrease. Loss: 0,32191413791490114
            Epoch 160/300, Loss: 0,32191413791490114
            atom queen
            king proton
            electron molecule
            Epoch 161/300, Loss: 0,32191412206614617
            atom queen
            king proton
            electron molecule
            Epoch 162: Loss did not decrease. Loss: 0,32191413316862577
            Epoch 162/300, Loss: 0,32191413316862577
            atom queen
            king proton
            electron molecule
            Epoch 163/300, Loss: 0,32191411753430677
            atom queen
            king proton
            electron molecule
            Epoch 164: Loss did not decrease. Loss: 0,3219141288692888
            Epoch 164/300, Loss: 0,3219141288692888
            atom queen
            king proton
            electron molecule
            Epoch 165/300, Loss: 0,3219141135406549
            atom queen
            king proton
            electron molecule
            Epoch 166: Loss did not decrease. Loss: 0,32191412497446753
            Epoch 166/300, Loss: 0,32191412497446753
            atom queen
            king proton
            electron molecule
            Epoch 167/300, Loss: 0,32191411002441567
            atom queen
            king proton
            electron molecule
            Epoch 168: Loss did not decrease. Loss: 0,32191412144579595
            Epoch 168/300, Loss: 0,32191412144579595
            atom queen
            king proton
            electron molecule
            Epoch 169/300, Loss: 0,32191410693145733
            atom queen
            king proton
            electron molecule
            Epoch 170: Loss did not decrease. Loss: 0,321914118248574
            Epoch 170/300, Loss: 0,321914118248574
            atom queen
            king proton
            electron molecule
            Epoch 171/300, Loss: 0,3219141042135818
            atom queen
            king proton
            electron molecule
            Epoch 172: Loss did not decrease. Loss: 0,3219141153514153
            Epoch 172/300, Loss: 0,3219141153514153
            atom queen
            king proton
            electron molecule
            Epoch 173/300, Loss: 0,32191410182788877
            atom queen
            king proton
            electron molecule
            Epoch 174: Loss did not decrease. Loss: 0,32191411272592707
            Epoch 174/300, Loss: 0,32191411272592707
            atom queen
            king proton
            electron molecule
            Epoch 175/300, Loss: 0,3219140997362077
            atom queen
            king proton
            electron molecule
            Epoch 176: Loss did not decrease. Loss: 0,32191411034642453
            Epoch 176/300, Loss: 0,32191411034642453
            atom queen
            king proton
            electron molecule
            Epoch 177/300, Loss: 0,3219140979045892
            atom queen
            king proton
            electron molecule
            Epoch 178: Loss did not decrease. Loss: 0,32191410818966965
            Epoch 178/300, Loss: 0,32191410818966965
            atom queen
            king proton
            electron molecule
            Epoch 179/300, Loss: 0,32191409630284934
            atom queen
            king proton
            electron molecule
            Epoch 180: Loss did not decrease. Loss: 0,3219141062346364
            Epoch 180/300, Loss: 0,3219141062346364
            atom queen
            king proton
            electron molecule
            Epoch 181/300, Loss: 0,32191409490416417
            atom queen
            king proton
            electron molecule
            Epoch 182: Loss did not decrease. Loss: 0,32191410446229934
            Epoch 182/300, Loss: 0,32191410446229934
            atom queen
            king proton
            electron molecule
            Epoch 183/300, Loss: 0,32191409368470525
            atom queen
            king proton
            electron molecule
            Epoch 184: Loss did not decrease. Loss: 0,32191410285544136
            Epoch 184/300, Loss: 0,32191410285544136
            atom queen
            king proton
            electron molecule
            Epoch 185/300, Loss: 0,3219140926233149
            atom queen
            king proton
            electron molecule
            Epoch 186: Loss did not decrease. Loss: 0,3219141013984807
            Epoch 186/300, Loss: 0,3219141013984807
            atom queen
            king proton
            electron molecule
            Epoch 187/300, Loss: 0,321914091701216
            atom queen
            king proton
            electron molecule
            Epoch 188: Loss did not decrease. Loss: 0,3219141000773143
            Epoch 188/300, Loss: 0,3219141000773143
            atom queen
            king proton
            electron molecule
            Epoch 189/300, Loss: 0,3219140909017519
            atom queen
            king proton
            electron molecule
            Epoch 190: Loss did not decrease. Loss: 0,32191409887917694
            Epoch 190/300, Loss: 0,32191409887917694
            atom queen
            king proton
            electron molecule
            Epoch 191/300, Loss: 0,32191409021015477
            atom queen
            king proton
            electron molecule
            Epoch 192: Loss did not decrease. Loss: 0,32191409779251323
            Epoch 192/300, Loss: 0,32191409779251323
            atom queen
            king proton
            electron molecule
            Epoch 193/300, Loss: 0,32191408961333917
            atom queen
            king proton
            electron molecule
            Epoch 194: Loss did not decrease. Loss: 0,32191409680686106
            Epoch 194/300, Loss: 0,32191409680686106
            atom queen
            king proton
            electron molecule
            Epoch 195/300, Loss: 0,32191408909971597
            atom queen
            king proton
            electron molecule
            Epoch 196: Loss did not decrease. Loss: 0,32191409591274933
            Epoch 196/300, Loss: 0,32191409591274933
            atom queen
            king proton
            electron molecule
            Epoch 197/300, Loss: 0,32191408865902843
            atom queen
            king proton
            electron molecule
            Epoch 198: Loss did not decrease. Loss: 0,3219140951016017
            Epoch 198/300, Loss: 0,3219140951016017
            atom queen
            king proton
            electron molecule
            Epoch 199/300, Loss: 0,3219140882822043
            atom queen
            king proton
            electron molecule
            Epoch 200: Loss did not decrease. Loss: 0,32191409436565194
            Epoch 200/300, Loss: 0,32191409436565194
            atom queen
            king proton
            electron molecule
            Epoch 201/300, Loss: 0,3219140879612243
            atom queen
            king proton
            electron molecule
            Epoch 202: Loss did not decrease. Loss: 0,3219140936978672
            Epoch 202/300, Loss: 0,3219140936978672
            atom queen
            king proton
            electron molecule
            Epoch 203/300, Loss: 0,3219140876890056
            atom queen
            king proton
            electron molecule
            Epoch 204: Loss did not decrease. Loss: 0,321914093091878
            Epoch 204/300, Loss: 0,321914093091878
            atom queen
            king proton
            electron molecule
            Epoch 205/300, Loss: 0,3219140874592963
            atom queen
            king proton
            electron molecule
            Epoch 206: Loss did not decrease. Loss: 0,32191409254191455
            Epoch 206/300, Loss: 0,32191409254191455
            atom queen
            king proton
            electron molecule
            Epoch 207/300, Loss: 0,3219140872665828
            atom queen
            king proton
            electron molecule
            Epoch 208: Loss did not decrease. Loss: 0,32191409204275057
            Epoch 208/300, Loss: 0,32191409204275057
            atom queen
            king proton
            electron molecule
            Epoch 209/300, Loss: 0,32191408710600705
            atom queen
            king proton
            electron molecule
            Epoch 210: Loss did not decrease. Loss: 0,32191409158965184
            Epoch 210/300, Loss: 0,32191409158965184
            atom queen
            king proton
            electron molecule
            Epoch 211/300, Loss: 0,3219140869732917
            atom queen
            king proton
            electron molecule
            Epoch 212: Loss did not decrease. Loss: 0,3219140911783288
            Epoch 212/300, Loss: 0,3219140911783288
            atom queen
            king proton
            electron molecule
            Epoch 213/300, Loss: 0,3219140868646748
            atom queen
            king proton
            electron molecule
            Epoch 214: Loss did not decrease. Loss: 0,32191409080489464
            Epoch 214/300, Loss: 0,32191409080489464
            atom queen
            king proton
            electron molecule
            Epoch 215/300, Loss: 0,3219140867768509
            atom queen
            king proton
            electron molecule
            Epoch 216: Loss did not decrease. Loss: 0,3219140904658274
            Epoch 216/300, Loss: 0,3219140904658274
            atom queen
            king proton
            electron molecule
            Epoch 217/300, Loss: 0,3219140867069192
            atom queen
            king proton
            electron molecule
            Epoch 218: Loss did not decrease. Loss: 0,32191409015793604
            Epoch 218/300, Loss: 0,32191409015793604
            atom queen
            king proton
            electron molecule
            Epoch 219/300, Loss: 0,3219140866523359
            atom queen
            king proton
            electron molecule
            Epoch 220: Loss did not decrease. Loss: 0,3219140898783273
            Epoch 220/300, Loss: 0,3219140898783273
            atom queen
            king proton
            electron molecule
            Epoch 221/300, Loss: 0,3219140866108747
            atom queen
            king proton
            electron molecule
            Epoch 222: Loss did not decrease. Loss: 0,3219140896243798
            Epoch 222/300, Loss: 0,3219140896243798
            atom queen
            king proton
            electron molecule
            Epoch 223/300, Loss: 0,3219140865805882
            atom queen
            king proton
            electron molecule
            Epoch 224: Loss did not decrease. Loss: 0,3219140893937163
            Epoch 224/300, Loss: 0,3219140893937163
            atom queen
            king proton
            electron molecule
            Epoch 225/300, Loss: 0,3219140865597766
            atom queen
            king proton
            electron molecule
            Epoch 226: Loss did not decrease. Loss: 0,3219140891841828
            Epoch 226/300, Loss: 0,3219140891841828
            atom queen
            king proton
            electron molecule
            Epoch 227/300, Loss: 0,32191408654695813
            atom queen
            king proton
            electron molecule
            Epoch 228: Loss did not decrease. Loss: 0,32191408899382545
            Epoch 228/300, Loss: 0,32191408899382545
            atom queen
            king proton
            electron molecule
            Epoch 229/300, Loss: 0,321914086540843
            atom queen
            king proton
            electron molecule
            Epoch 230: Loss did not decrease. Loss: 0,32191408882087325
            Epoch 230/300, Loss: 0,32191408882087325
            atom queen
            king proton
            electron molecule
            Epoch 231/300, Loss: 0,32191408654031134
            atom queen
            king proton
            electron molecule
            Epoch 232: Loss did not decrease. Loss: 0,32191408866372
            Epoch 232/300, Loss: 0,32191408866372
            atom queen
            king proton
            electron molecule
            Epoch 233/300, Loss: 0,321914086544392
            atom queen
            king proton
            electron molecule
            Epoch 234: Loss did not decrease. Loss: 0,3219140885209092
            Epoch 234/300, Loss: 0,3219140885209092
            atom queen
            king proton
            electron molecule
            Epoch 235/300, Loss: 0,32191408655224546
            atom queen
            king proton
            electron molecule
            Epoch 236: Loss did not decrease. Loss: 0,3219140883911196
            Epoch 236/300, Loss: 0,3219140883911196
            atom queen
            king proton
            electron molecule
            Epoch 237/300, Loss: 0,3219140865631473
            atom queen
            king proton
            electron molecule
            Epoch 238: Loss did not decrease. Loss: 0,3219140882731531
            Epoch 238/300, Loss: 0,3219140882731531
            atom queen
            king proton
            electron molecule
            Epoch 239/300, Loss: 0,32191408657647386
            atom queen
            king proton
            electron molecule
            Epoch 240: Loss did not decrease. Loss: 0,3219140881659224
            Epoch 240/300, Loss: 0,3219140881659224
            atom queen
            king proton
            electron molecule
            Epoch 241/300, Loss: 0,3219140865916909
            atom queen
            king proton
            electron molecule
            Epoch 242: Loss did not decrease. Loss: 0,32191408806844185
            Epoch 242/300, Loss: 0,32191408806844185
            atom queen
            king proton
            electron molecule
            Epoch 243/300, Loss: 0,3219140866083414
            atom queen
            king proton
            electron molecule
            Epoch 244: Loss did not decrease. Loss: 0,3219140879798161
            Epoch 244/300, Loss: 0,3219140879798161
            atom queen
            king proton
            electron molecule
            Epoch 245/300, Loss: 0,32191408662603643
            atom queen
            king proton
            electron molecule
            Epoch 246: Loss did not decrease. Loss: 0,32191408789923376
            Epoch 246/300, Loss: 0,32191408789923376
            atom queen
            king proton
            electron molecule
            Epoch 247/300, Loss: 0,3219140866444458
            atom queen
            king proton
            electron molecule
            Epoch 248: Loss did not decrease. Loss: 0,32191408782595776
            Epoch 248/300, Loss: 0,32191408782595776
            atom queen
            king proton
            electron molecule
            Epoch 249/300, Loss: 0,32191408666329185
            atom queen
            king proton
            electron molecule
            Epoch 250: Loss did not decrease. Loss: 0,3219140877593199
            Epoch 250/300, Loss: 0,3219140877593199
            atom queen
            king proton
            electron molecule
            Epoch 251/300, Loss: 0,32191408668234117
            atom queen
            king proton
            electron molecule
            Epoch 252: Loss did not decrease. Loss: 0,3219140876987128
            Epoch 252/300, Loss: 0,3219140876987128
            atom queen
            king proton
            electron molecule
            Epoch 253/300, Loss: 0,3219140867013995
            atom queen
            king proton
            electron molecule
            Epoch 254: Loss did not decrease. Loss: 0,3219140876435857
            Epoch 254/300, Loss: 0,3219140876435857
            atom queen
            king proton
            electron molecule
            Epoch 255/300, Loss: 0,32191408672030686
            atom queen
            king proton
            electron molecule
            Epoch 256: Loss did not decrease. Loss: 0,321914087593439
            Epoch 256/300, Loss: 0,321914087593439
            atom queen
            king proton
            electron molecule
            Epoch 257/300, Loss: 0,3219140867389318
            atom queen
            king proton
            electron molecule
            Epoch 258: Loss did not decrease. Loss: 0,32191408754781786
            Epoch 258/300, Loss: 0,32191408754781786
            atom queen
            king proton
            electron molecule
            Epoch 259/300, Loss: 0,3219140867571688
            atom queen
            king proton
            electron molecule
            Epoch 260: Loss did not decrease. Loss: 0,3219140875063106
            Epoch 260/300, Loss: 0,3219140875063106
            atom queen
            king proton
            electron molecule
            Epoch 261/300, Loss: 0,32191408677493377
            atom queen
            king proton
            electron molecule
            Epoch 262: Loss did not decrease. Loss: 0,3219140874685426
            Epoch 262/300, Loss: 0,3219140874685426
            atom queen
            king proton
            electron molecule
            Epoch 263/300, Loss: 0,321914086792161
            atom queen
            king proton
            electron molecule
            Epoch 264: Loss did not decrease. Loss: 0,32191408743417393
            Epoch 264/300, Loss: 0,32191408743417393
            atom queen
            king proton
            electron molecule
            Epoch 265/300, Loss: 0,3219140868088011
            atom queen
            king proton
            electron molecule
            Epoch 266: Loss did not decrease. Loss: 0,32191408740289595
            Epoch 266/300, Loss: 0,32191408740289595
            atom queen
            king proton
            electron molecule
            Epoch 267/300, Loss: 0,3219140868248182
            atom queen
            king proton
            electron molecule
            Epoch 268: Loss did not decrease. Loss: 0,321914087374428
            Epoch 268/300, Loss: 0,321914087374428
            atom queen
            king proton
            electron molecule
            Epoch 269/300, Loss: 0,32191408684018774
            atom queen
            king proton
            electron molecule
            Epoch 270: Loss did not decrease. Loss: 0,32191408734851545
            Epoch 270/300, Loss: 0,32191408734851545
            atom queen
            king proton
            electron molecule
            Epoch 271/300, Loss: 0,32191408685489453
            atom queen
            king proton
            electron molecule
            Epoch 272: Loss did not decrease. Loss: 0,3219140873249269
            Epoch 272/300, Loss: 0,3219140873249269
            atom queen
            king proton
            electron molecule
            Epoch 273/300, Loss: 0,32191408686893214
            atom queen
            king proton
            electron molecule
            Epoch 274: Loss did not decrease. Loss: 0,32191408730345183
            Epoch 274/300, Loss: 0,32191408730345183
            atom queen
            king proton
            electron molecule
            Epoch 275/300, Loss: 0,32191408688230044
            atom queen
            king proton
            electron molecule
            Epoch 276: Loss did not decrease. Loss: 0,3219140872838993
            Epoch 276/300, Loss: 0,3219140872838993
            atom queen
            king proton
            electron molecule
            Epoch 277/300, Loss: 0,32191408689500506
            atom queen
            king proton
            electron molecule
            Epoch 278: Loss did not decrease. Loss: 0,3219140872660957
            Epoch 278/300, Loss: 0,3219140872660957
            atom queen
            king proton
            electron molecule
            Epoch 279/300, Loss: 0,3219140869070561
            atom queen
            king proton
            electron molecule
            Epoch 280: Loss did not decrease. Loss: 0,3219140872498829
            Epoch 280/300, Loss: 0,3219140872498829
            atom queen
            king proton
            electron molecule
            Epoch 281/300, Loss: 0,3219140869184674
            atom queen
            king proton
            electron molecule
            Epoch 282: Loss did not decrease. Loss: 0,3219140872351179
            Epoch 282/300, Loss: 0,3219140872351179
            atom queen
            king proton
            electron molecule
            Epoch 283/300, Loss: 0,32191408692925555
            atom queen
            king proton
            electron molecule
            Epoch 284: Loss did not decrease. Loss: 0,3219140872216698
            Epoch 284/300, Loss: 0,3219140872216698
            atom queen
            king proton
            electron molecule
            Epoch 285/300, Loss: 0,32191408693943974
            atom queen
            king proton
            electron molecule
            Epoch 286: Loss did not decrease. Loss: 0,32191408720942055
            Epoch 286/300, Loss: 0,32191408720942055
            atom queen
            king proton
            electron molecule
            Epoch 287/300, Loss: 0,3219140869490406
            atom queen
            king proton
            electron molecule
            Epoch 288: Loss did not decrease. Loss: 0,32191408719826214
            Epoch 288/300, Loss: 0,32191408719826214
            atom queen
            king proton
            electron molecule
            Epoch 289/300, Loss: 0,32191408695808005
            atom queen
            king proton
            electron molecule
            Epoch 290: Loss did not decrease. Loss: 0,32191408718809655
            Epoch 290/300, Loss: 0,32191408718809655
            atom queen
            king proton
            electron molecule
            Epoch 291/300, Loss: 0,3219140869665809
            atom queen
            king proton
            electron molecule
            Epoch 292: Loss did not decrease. Loss: 0,3219140871788347
            Epoch 292/300, Loss: 0,3219140871788347
            atom queen
            king proton
            electron molecule
            Epoch 293/300, Loss: 0,3219140869745665
            atom queen
            king proton
            electron molecule
            Epoch 294: Loss did not decrease. Loss: 0,32191408717039544
            Epoch 294/300, Loss: 0,32191408717039544
            atom queen
            king proton
            electron molecule
            Epoch 295/300, Loss: 0,3219140869820602
            atom queen
            king proton
            electron molecule
            Epoch 296: Loss did not decrease. Loss: 0,3219140871627053
            Epoch 296/300, Loss: 0,3219140871627053
            atom queen
            king proton
            electron molecule
            Epoch 297/300, Loss: 0,3219140869890856
            atom queen
            king proton
            electron molecule
            Epoch 298: Loss did not decrease. Loss: 0,3219140871556972
            Epoch 298/300, Loss: 0,3219140871556972
            atom queen
            king proton
            electron molecule
            Epoch 299/300, Loss: 0,3219140869956658
            atom queen
            king proton
            electron molecule
            Epoch 300: Loss did not decrease. Loss: 0,32191408714930986
            Epoch 300/300, Loss: 0,32191408714930986
            */


            Console.Read();
        }
    }
}
