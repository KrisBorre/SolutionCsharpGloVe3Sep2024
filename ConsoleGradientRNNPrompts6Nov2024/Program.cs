using LibraryGlobalVectors1Sep2024;
using LibraryGradientRNN6Nov2024;
using LibraryPrompts4Nov2024;

namespace ConsoleGradientRNNPrompts6Nov2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt"; 
            int embeddingDim = 50;
            var glove = new GloveLoader(gloveFilePath, embeddingDim);

            string filePath = "../../../../LibraryPrompts4Nov2024/prompts.csv";
            PromptReader promptReader = new PromptReader();
            var prompts = promptReader.ReadPromptsFromCsv(filePath);
            var first10Prompts = prompts.Take(10).ToList();

            #region hiddenSize 3
            //int hiddenSize = 3;
            //double learningRate = 0.01;
            //double gradientThreshold = 1.0;
            //int epochs = 10000;
            ////Epoch 10000 / 10000, Average Loss: 0,2265848051283565

            //int hiddenSize = 3;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            ////Epoch 5800 / 10000, Average Loss: 0,1998150711141479
            ////Epoch 5900 / 10000, Average Loss: 0,19979946457275333
            ////Epoch 6000 / 10000, Average Loss: 0,19979147381816392

            //int hiddenSize = 3;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.01;
            //int epochs = 10000;
            //// Epoch 10000/10000, Average Loss: 0,21489113446936248
            #endregion

            #region hiddenSize 10
            //int hiddenSize = 10;
            //double learningRate = 0.01;
            //double gradientThreshold = 1.0;
            //int epochs = 10000;
            ////Epoch 10000 / 10000, Average Loss: 0,24534578568998455

            //int hiddenSize = 10;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            ////Epoch 10000 / 10000, Average Loss: 0,2452509459658238
            ////it come it come

            //int hiddenSize = 10;
            //double learningRate = 0.001;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            ////Epoch 9600 / 10000, Average Loss: 0,20284156766742534
            ////Epoch 9700 / 10000, Average Loss: 0,2026400677041106
            ////Epoch 9800 / 10000, Average Loss: 0,2024786900642464
            ////Epoch 9900 / 10000, Average Loss: 0,20238978134777832
            ////Epoch 10000 / 10000, Average Loss: 0,20236535427879515

            //int hiddenSize = 10;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.01;
            //int epochs = 10000;
            ////Epoch 9900 / 10000, Average Loss: 0,22794479055998468
            ////Epoch 10000 / 10000, Average Loss: 0,22624865302489522
            #endregion

            #region hiddenSize 150
            //int hiddenSize = 150;
            //double learningRate = 0.1;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            /*
Epoch 0/10000, Average Loss: 0,2564442745719426
Epoch 100/10000, Average Loss: 0,3921525892091816
Epoch 200/10000, Average Loss: 0,43893050516703064
Epoch 300/10000, Average Loss: 0,4243552946264967
Epoch 400/10000, Average Loss: 0,3627599447007825
Epoch 500/10000, Average Loss: NaN
Epoch 600/10000, Average Loss: NaN
Epoch 700/10000, Average Loss: NaN
Epoch 800/10000, Average Loss: NaN
Epoch 900/10000, Average Loss: NaN
Epoch 1000/10000, Average Loss: NaN
Epoch 1100/10000, Average Loss: NaN
Epoch 1200/10000, Average Loss: NaN
Epoch 1300/10000, Average Loss: NaN
Epoch 1400/10000, Average Loss: NaN
Epoch 1500/10000, Average Loss: NaN
Epoch 1600/10000, Average Loss: NaN
Epoch 1700/10000, Average Loss: NaN
Epoch 1800/10000, Average Loss: NaN
Epoch 1900/10000, Average Loss: NaN
Epoch 2000/10000, Average Loss: NaN
Epoch 2100/10000, Average Loss: NaN
Epoch 2200/10000, Average Loss: NaN
Epoch 2300/10000, Average Loss: NaN
Epoch 2400/10000, Average Loss: NaN
Epoch 2500/10000, Average Loss: NaN
             */


            //int hiddenSize = 150;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            /*
             Epoch 0/10000, Average Loss: 0,2507253711554447
Epoch 100/10000, Average Loss: 0,31601303053194263
Epoch 200/10000, Average Loss: 0,3272580854751095
Epoch 300/10000, Average Loss: 0,4036528893863743
Epoch 400/10000, Average Loss: 0,3868897881524858
Epoch 500/10000, Average Loss: 0,45375392403944853
Epoch 600/10000, Average Loss: 0,3940982328130806
Epoch 700/10000, Average Loss: 0,4238875402106396
Epoch 800/10000, Average Loss: 0,4391317398947529
Epoch 900/10000, Average Loss: 0,492473270973332
Epoch 1000/10000, Average Loss: 0,35852439635164157
Epoch 1100/10000, Average Loss: 0,4261928344046681
Epoch 1200/10000, Average Loss: 0,4921490423089397
Epoch 1300/10000, Average Loss: 0,4288023933010255
Epoch 1400/10000, Average Loss: 0,3919985890535539
Epoch 1500/10000, Average Loss: 0,47592918452642063
Epoch 1600/10000, Average Loss: 0,3907308279853
Epoch 1700/10000, Average Loss: 0,4795595556211881
Epoch 1800/10000, Average Loss: 0,44621730435541396
Epoch 1900/10000, Average Loss: 0,4170178331653432
Epoch 2000/10000, Average Loss: 0,5396965570298874
Epoch 2100/10000, Average Loss: 0,4394273709782587
Epoch 2200/10000, Average Loss: 0,4314668101313327
Epoch 2300/10000, Average Loss: 0,4501077073337923
Epoch 2400/10000, Average Loss: 0,5018983998690102
Epoch 2500/10000, Average Loss: 0,41615128184453026
Epoch 2600/10000, Average Loss: 0,44075636890113756
Epoch 2700/10000, Average Loss: 0,4365862792077257
Epoch 2800/10000, Average Loss: 0,460279523291931
Epoch 2900/10000, Average Loss: 0,5117983754696038
Epoch 3000/10000, Average Loss: 0,43104090974968157
Epoch 3100/10000, Average Loss: 0,44936766641797243
Epoch 3200/10000, Average Loss: 0,4450898665979131
Epoch 3300/10000, Average Loss: 0,41560830909199026
Epoch 3400/10000, Average Loss: 0,47892283492467025
Epoch 3500/10000, Average Loss: 0,4923110032695582
Epoch 3600/10000, Average Loss: 0,4985889603617563
Epoch 3700/10000, Average Loss: 0,9384120806355838
Epoch 3800/10000, Average Loss: 0,40628623171557743
Epoch 3900/10000, Average Loss: 0,4577893585244448
Epoch 4000/10000, Average Loss: 0,49618386466685005
Epoch 4100/10000, Average Loss: 0,439739011211031
Epoch 4200/10000, Average Loss: 0,5129583717518249
Epoch 4300/10000, Average Loss: 0,43011394869028996
Epoch 4400/10000, Average Loss: 0,498186213168417
Epoch 4500/10000, Average Loss: 0,6108797237089458
Epoch 4600/10000, Average Loss: 0,5330955559215278
Epoch 4700/10000, Average Loss: 0,3344452915913625
Epoch 4800/10000, Average Loss: 0,48102714204393066
Epoch 4900/10000, Average Loss: 0,44858290779345616
Epoch 5000/10000, Average Loss: 0,4411199984329288
Epoch 5100/10000, Average Loss: 0,4651760009173933
Epoch 5200/10000, Average Loss: 0,678926553764746
Epoch 5300/10000, Average Loss: 0,621628609989328
Epoch 5400/10000, Average Loss: 0,6069442968070204
Epoch 5500/10000, Average Loss: 0,5746140531652618
Epoch 5600/10000, Average Loss: 0,4959017152437467
Epoch 5700/10000, Average Loss: 0,6518334605387728
Epoch 5800/10000, Average Loss: 0,5124113847234453
Epoch 5900/10000, Average Loss: 12,038587618920243
Epoch 6000/10000, Average Loss: NaN
             */


            //int hiddenSize = 150;
            //double learningRate = 0.01;
            //double gradientThreshold = 0.01;
            //int epochs = 10000;
            /*
             Epoch 0/10000, Average Loss: 0,25549831949553503
Epoch 100/10000, Average Loss: 0,1982988525776528
Epoch 200/10000, Average Loss: 0,2577627869585395
Epoch 300/10000, Average Loss: 0,22855249752994217
Epoch 400/10000, Average Loss: 0,312294738048009
Epoch 500/10000, Average Loss: 0,4066780650409811
Epoch 600/10000, Average Loss: 0,49038239967333136
Epoch 700/10000, Average Loss: 0,36244909451134083
Epoch 800/10000, Average Loss: 0,28907387622151
Epoch 900/10000, Average Loss: 0,41460087415522884
Epoch 1000/10000, Average Loss: 0,44849851884832476
Epoch 1100/10000, Average Loss: 0,43555895714051396
Epoch 1200/10000, Average Loss: 0,39056915256002284
Epoch 1300/10000, Average Loss: 0,39920830348040254
Epoch 1400/10000, Average Loss: 0,39549514429734434
Epoch 1500/10000, Average Loss: 0,4632437035743403
Epoch 1600/10000, Average Loss: 0,5418455041727791
Epoch 1700/10000, Average Loss: 0,4412019182814764
Epoch 1800/10000, Average Loss: 0,468999229780934
Epoch 1900/10000, Average Loss: 0,5147087695600628
Epoch 2000/10000, Average Loss: 0,7136981441962565
Epoch 2100/10000, Average Loss: 0,47630154299800675
Epoch 2200/10000, Average Loss: 0,42901539809604383
Epoch 2300/10000, Average Loss: 0,4475301592903852
Epoch 2400/10000, Average Loss: 0,5927643476591201
Epoch 2500/10000, Average Loss: 0,4218062627030353
Epoch 2600/10000, Average Loss: 0,4621005399386058
Epoch 2700/10000, Average Loss: 0,44797021127261266
Epoch 2800/10000, Average Loss: 0,464442861795541
Epoch 2900/10000, Average Loss: 0,4429033717998099
Epoch 3000/10000, Average Loss: 0,4740752055521944
Epoch 3100/10000, Average Loss: 0,40945532402275553
Epoch 3200/10000, Average Loss: 0,42598389387537655
Epoch 3300/10000, Average Loss: 0,3750736023281743
Epoch 3400/10000, Average Loss: 0,33454810773150406
Epoch 3500/10000, Average Loss: 0,3480477204453124
Epoch 3600/10000, Average Loss: 0,34345435272921165
Epoch 3700/10000, Average Loss: 0,5502195162456256
Epoch 3800/10000, Average Loss: 0,5609975316127805
Epoch 3900/10000, Average Loss: 0,6207828368586347
Epoch 4000/10000, Average Loss: 0,6165705265043574
Epoch 4100/10000, Average Loss: 0,56477720726857
Epoch 4200/10000, Average Loss: 0,5422188358782531
Epoch 4300/10000, Average Loss: 0,523499770148389
Epoch 4400/10000, Average Loss: 0,5269548054577131
Epoch 4500/10000, Average Loss: 0,5419353996452504
Epoch 4600/10000, Average Loss: 0,5819837168185373
Epoch 4700/10000, Average Loss: 0,5089825184251283
Epoch 4800/10000, Average Loss: 0,48892786803182114
Epoch 4900/10000, Average Loss: 0,4434305700790242
Epoch 5000/10000, Average Loss: 0,4903413812565386
Epoch 5100/10000, Average Loss: 0,4580144594273838
Epoch 5200/10000, Average Loss: 0,45122481773327283
Epoch 5300/10000, Average Loss: 0,4800911401473904
Epoch 5400/10000, Average Loss: 0,43182880583751054
Epoch 5500/10000, Average Loss: 0,5121131127131026
Epoch 5600/10000, Average Loss: 0,45781457599796216
Epoch 5700/10000, Average Loss: 0,5094586053627821
Epoch 5800/10000, Average Loss: 0,46035993923816987
Epoch 5900/10000, Average Loss: 0,3953007182663963
Epoch 6000/10000, Average Loss: 0,4749838604107029
Epoch 6100/10000, Average Loss: 0,42291161224158297
Epoch 6200/10000, Average Loss: 0,36294962796068575
Epoch 6300/10000, Average Loss: 0,40394381015186803
Epoch 6400/10000, Average Loss: 0,6670333227449119
Epoch 6500/10000, Average Loss: 0,5706684712142904
Epoch 6600/10000, Average Loss: 0,5138541863772514
Epoch 6700/10000, Average Loss: 0,565589547854771
Epoch 6800/10000, Average Loss: 0,6371215771644375
Epoch 6900/10000, Average Loss: 0,6247230819279399
Epoch 7000/10000, Average Loss: 0,5751833213310414
Epoch 7100/10000, Average Loss: 0,7742472586852315
Epoch 7200/10000, Average Loss: 0,4338363223810783
Epoch 7300/10000, Average Loss: 0,581936642594306
Epoch 7400/10000, Average Loss: 0,5450465571839959
Epoch 7500/10000, Average Loss: 0,48186332961683515
Epoch 7600/10000, Average Loss: 0,4720291116051295
Epoch 7700/10000, Average Loss: 0,4453646995090318
Epoch 7800/10000, Average Loss: 0,4305917080582583
Epoch 7900/10000, Average Loss: 0,5237008472878634
Epoch 8000/10000, Average Loss: 0,43462342855802316
Epoch 8100/10000, Average Loss: 0,5320622347968775
Epoch 8200/10000, Average Loss: 0,6491632418607833
Epoch 8300/10000, Average Loss: 0,6636986063364837
Epoch 8400/10000, Average Loss: 0,6575184423529306
Epoch 8500/10000, Average Loss: 0,6188894610035678
Epoch 8600/10000, Average Loss: 0,5996169203864603
Epoch 8700/10000, Average Loss: 0,4381151399941188
Epoch 8800/10000, Average Loss: 0,5180268894340946
Epoch 8900/10000, Average Loss: 0,6766258011455808
Epoch 9000/10000, Average Loss: 0,6736151645419876
Epoch 9100/10000, Average Loss: 0,6037261742974902
Epoch 9200/10000, Average Loss: 0,5947464064714458
Epoch 9300/10000, Average Loss: 0,557869856055299
Epoch 9400/10000, Average Loss: 0,59143250145964
Epoch 9500/10000, Average Loss: 0,596109831809794
Epoch 9600/10000, Average Loss: 0,4798946037859049
Epoch 9700/10000, Average Loss: 0,44041490720109344
Epoch 9800/10000, Average Loss: 0,4856766333981044
Epoch 9900/10000, Average Loss: 0,4198544387627492
Epoch 10000/10000, Average Loss: 0,44474604907084503
             */


            //int hiddenSize = 150;
            //double learningRate = 0.001;
            //double gradientThreshold = 0.1;
            //int epochs = 10000;
            /*
             Epoch 9600/10000, Average Loss: 0,23317088610728326
Epoch 9700/10000, Average Loss: 0,23302391720875115
Epoch 9800/10000, Average Loss: 0,23309361043787863
Epoch 9900/10000, Average Loss: 0,23291797102307094
Epoch 10000/10000, Average Loss: 0,23324535258000245
             */


            //int hiddenSize = 150;
            //double learningRate = 0.001;
            //double gradientThreshold = 0.01;
            //int epochs = 10000;
            /*
             Epoch 0/10000, Average Loss: 0,30161606258721235
Epoch 100/10000, Average Loss: 0,21080567996698832
Epoch 200/10000, Average Loss: 0,20045571376058194
Epoch 300/10000, Average Loss: 0,19486491402343825
Epoch 400/10000, Average Loss: 0,19126927907869268
Epoch 500/10000, Average Loss: 0,1887575742931597
Epoch 600/10000, Average Loss: 0,18694384805204314
Epoch 700/10000, Average Loss: 0,1856561267020941
Epoch 800/10000, Average Loss: 0,18483073966276692
Epoch 900/10000, Average Loss: 0,1844596983803339
Epoch 1000/10000, Average Loss: 0,18452899318099356
Epoch 1100/10000, Average Loss: 0,1849491812682748
Epoch 1200/10000, Average Loss: 0,1855900266000425
Epoch 1300/10000, Average Loss: 0,18634828593399722
Epoch 1400/10000, Average Loss: 0,1870053244547896
Epoch 1500/10000, Average Loss: 0,18733507301461885
Epoch 1600/10000, Average Loss: 0,18738976442401775
Epoch 1700/10000, Average Loss: 0,18726949815318267
Epoch 1800/10000, Average Loss: 0,18715125737729796
Epoch 1900/10000, Average Loss: 0,18733690385385393
Epoch 2000/10000, Average Loss: 0,1886874602842564
Epoch 2100/10000, Average Loss: 0,1909011200456622
Epoch 2200/10000, Average Loss: 0,1931639832758821
Epoch 2300/10000, Average Loss: 0,1951087184748797
Epoch 2400/10000, Average Loss: 0,19765059658782105
Epoch 2500/10000, Average Loss: 0,19998900524091812
Epoch 2600/10000, Average Loss: 0,20289656120900154
Epoch 2700/10000, Average Loss: 0,20575361039124984
Epoch 2800/10000, Average Loss: 0,20715698853455017
Epoch 2900/10000, Average Loss: 0,2073079521027577
Epoch 3000/10000, Average Loss: 0,2083957116845002
Epoch 3100/10000, Average Loss: 0,2103153184503969
Epoch 3200/10000, Average Loss: 0,21357809648481285
Epoch 3300/10000, Average Loss: 0,21511698739242843
Epoch 3400/10000, Average Loss: 0,2167185871851379
Epoch 3500/10000, Average Loss: 0,21717457766781517
Epoch 3600/10000, Average Loss: 0,21841894169003978
Epoch 3700/10000, Average Loss: 0,21794145685879887
Epoch 3800/10000, Average Loss: 0,2188646422539831
Epoch 3900/10000, Average Loss: 0,2218535861447617
Epoch 4000/10000, Average Loss: 0,22339038191056965
Epoch 4100/10000, Average Loss: 0,22578461801657937
Epoch 4200/10000, Average Loss: 0,2277190389174828
Epoch 4300/10000, Average Loss: 0,22578515137912158
Epoch 4400/10000, Average Loss: 0,22633251917263347
Epoch 4500/10000, Average Loss: 0,2262108188187701
Epoch 4600/10000, Average Loss: 0,22782755323526827
Epoch 4700/10000, Average Loss: 0,22991276826355542
Epoch 4800/10000, Average Loss: 0,22987356311802537
Epoch 4900/10000, Average Loss: 0,22913319809820898
Epoch 5000/10000, Average Loss: 0,2289164753998804
Epoch 5100/10000, Average Loss: 0,22958154127961655
Epoch 5200/10000, Average Loss: 0,23182626478864043
Epoch 5300/10000, Average Loss: 0,23164654767364481
Epoch 5400/10000, Average Loss: 0,23153762655978527
Epoch 5500/10000, Average Loss: 0,23403180011138733
Epoch 5600/10000, Average Loss: 0,2344816210474419
Epoch 5700/10000, Average Loss: 0,2340335474689935
Epoch 5800/10000, Average Loss: 0,23443118250887146
Epoch 5900/10000, Average Loss: 0,23330902300177897
Epoch 6000/10000, Average Loss: 0,23575277171267497
Epoch 6100/10000, Average Loss: 0,23644956094792077
Epoch 6200/10000, Average Loss: 0,2363862704286337
Epoch 6300/10000, Average Loss: 0,23608740951569493
Epoch 6400/10000, Average Loss: 0,23643724683320116
Epoch 6500/10000, Average Loss: 0,23617659402790916
Epoch 6600/10000, Average Loss: 0,23495218895304443
Epoch 6700/10000, Average Loss: 0,23486711497579474
Epoch 6800/10000, Average Loss: 0,23498556006795207
Epoch 6900/10000, Average Loss: 0,23578824883427263
Epoch 7000/10000, Average Loss: 0,23573467370189194
Epoch 7100/10000, Average Loss: 0,2348610737120076
Epoch 7200/10000, Average Loss: 0,23364395121684076
Epoch 7300/10000, Average Loss: 0,23310928512276644
Epoch 7400/10000, Average Loss: 0,23279118570928087
Epoch 7500/10000, Average Loss: 0,23241384192155207
Epoch 7600/10000, Average Loss: 0,232490723875521
Epoch 7700/10000, Average Loss: 0,23367004670859326
Epoch 7800/10000, Average Loss: 0,2352155077216099
Epoch 7900/10000, Average Loss: 0,23449609413729058
Epoch 8000/10000, Average Loss: 0,23361499754152698
Epoch 8100/10000, Average Loss: 0,23531382697825015
Epoch 8200/10000, Average Loss: 0,2356549946661261
Epoch 8300/10000, Average Loss: 0,2354906780067257
Epoch 8400/10000, Average Loss: 0,2356394709312173
Epoch 8500/10000, Average Loss: 0,23402790200491172
Epoch 8600/10000, Average Loss: 0,23364136103377012
Epoch 8700/10000, Average Loss: 0,23599377369337401
Epoch 8800/10000, Average Loss: 0,2355011862753631
Epoch 8900/10000, Average Loss: 0,2346891693136756
Epoch 9000/10000, Average Loss: 0,2351479557093567
Epoch 9100/10000, Average Loss: 0,23392192722910404
Epoch 9200/10000, Average Loss: 0,23376406136821695
Epoch 9300/10000, Average Loss: 0,23274128708610803
Epoch 9400/10000, Average Loss: 0,23343803446168204
Epoch 9500/10000, Average Loss: 0,23321979604148274
Epoch 9600/10000, Average Loss: 0,2332802545965361
Epoch 9700/10000, Average Loss: 0,23425907247480673
Epoch 9800/10000, Average Loss: 0,2341894469545777
Epoch 9900/10000, Average Loss: 0,23317757861734373
Epoch 10000/10000, Average Loss: 0,23276718586918022
             */


            //int hiddenSize = 150;
            //double learningRate = 0.001;
            //double gradientThreshold = 0.001;
            //int epochs = 10000;
            /*
             Epoch 0/10000, Average Loss: 0,3108150530756701
Epoch 100/10000, Average Loss: 0,21204197767074837
Epoch 200/10000, Average Loss: 0,20190142823995788
Epoch 300/10000, Average Loss: 0,19637578751298784
Epoch 400/10000, Average Loss: 0,19269964194120937
Epoch 500/10000, Average Loss: 0,19001021852307018
Epoch 600/10000, Average Loss: 0,18792613916767636
Epoch 700/10000, Average Loss: 0,18624674637457936
Epoch 800/10000, Average Loss: 0,1848543681320397
Epoch 900/10000, Average Loss: 0,18367456846669147
Epoch 1000/10000, Average Loss: 0,1826575246498056
Epoch 1100/10000, Average Loss: 0,18176839837972433
Epoch 1200/10000, Average Loss: 0,18098197135330232
Epoch 1300/10000, Average Loss: 0,1802794723630103
Epoch 1400/10000, Average Loss: 0,17964660556703468
Epoch 1500/10000, Average Loss: 0,17907227342473397
Epoch 1600/10000, Average Loss: 0,17854772017953677
Epoch 1700/10000, Average Loss: 0,17806594017834343
Epoch 1800/10000, Average Loss: 0,1776212587578035
Epoch 1900/10000, Average Loss: 0,17720902894559698
Epoch 2000/10000, Average Loss: 0,17682540790305118
Epoch 2100/10000, Average Loss: 0,17646718950437848
Epoch 2200/10000, Average Loss: 0,1761316772065249
Epoch 2300/10000, Average Loss: 0,17581658632852631
Epoch 2400/10000, Average Loss: 0,17551996811714385
Epoch 2500/10000, Average Loss: 0,175240150162289
Epoch 2600/10000, Average Loss: 0,1749756892239789
Epoch 2700/10000, Average Loss: 0,1747253335782519
Epoch 2800/10000, Average Loss: 0,17448799273138177
Epoch 2900/10000, Average Loss: 0,17426271288593342
Epoch 3000/10000, Average Loss: 0,17404865693170962
Epoch 3100/10000, Average Loss: 0,17384508802197918
Epoch 3200/10000, Average Loss: 0,17365135600940185
Epoch 3300/10000, Average Loss: 0,1734668861769854
Epoch 3400/10000, Average Loss: 0,17329116982130935
Epoch 3500/10000, Average Loss: 0,1731237563383501
Epoch 3600/10000, Average Loss: 0,17296424653375858
Epoch 3700/10000, Average Loss: 0,17281228693474773
Epoch 3800/10000, Average Loss: 0,17266756492377258
Epoch 3900/10000, Average Loss: 0,17252980454781708
Epoch 4000/10000, Average Loss: 0,17239876288353262
Epoch 4100/10000, Average Loss: 0,17227422685936575
Epoch 4200/10000, Average Loss: 0,172156010452402
Epoch 4300/10000, Average Loss: 0,17204395219090227
Epoch 4400/10000, Average Loss: 0,17193791290418703
Epoch 4500/10000, Average Loss: 0,17183777367018935
Epoch 4600/10000, Average Loss: 0,17174343391812347
Epoch 4700/10000, Average Loss: 0,171654809649681
Epoch 4800/10000, Average Loss: 0,1715718317472345
Epoch 4900/10000, Average Loss: 0,17149444434199107
Epoch 5000/10000, Average Loss: 0,17142260321902517
Epoch 5100/10000, Average Loss: 0,17135627423985803
Epoch 5200/10000, Average Loss: 0,1712954317667873
Epoch 5300/10000, Average Loss: 0,1712400570766831
Epoch 5400/10000, Average Loss: 0,17119013675543587
Epoch 5500/10000, Average Loss: 0,17114566106779422
Epoch 5600/10000, Average Loss: 0,17110662230091023
Epoch 5700/10000, Average Loss: 0,17107301308359069
Epoch 5800/10000, Average Loss: 0,1710448246869527
Epoch 5900/10000, Average Loss: 0,1710220453159294
Epoch 6000/10000, Average Loss: 0,17100465840478662
Epoch 6100/10000, Average Loss: 0,17099264093349528
Epoch 6200/10000, Average Loss: 0,17098596178535402
Epoch 6300/10000, Average Loss: 0,17098458016968818
Epoch 6400/10000, Average Loss: 0,17098844413662048
Epoch 6500/10000, Average Loss: 0,17099748921379343
Epoch 6600/10000, Average Loss: 0,17101163719735107
Epoch 6700/10000, Average Loss: 0,17103079513126082
Epoch 6800/10000, Average Loss: 0,17105485450998134
Epoch 6900/10000, Average Loss: 0,1710836907391215
Epoch 7000/10000, Average Loss: 0,1711171628867486
Epoch 7100/10000, Average Loss: 0,1711551137538736
Epoch 7200/10000, Average Loss: 0,1711973702858742
Epoch 7300/10000, Average Loss: 0,17124374433675935
Epoch 7400/10000, Average Loss: 0,17129403378497826
Epoch 7500/10000, Average Loss: 0,17134802398285243
Epoch 7600/10000, Average Loss: 0,17140548950205603
Epoch 7700/10000, Average Loss: 0,17146619611576955
Epoch 7800/10000, Average Loss: 0,1715299029356609
Epoch 7900/10000, Average Loss: 0,17159636460088099
Epoch 8000/10000, Average Loss: 0,17166533339955664
Epoch 8100/10000, Average Loss: 0,17173656119403952
Epoch 8200/10000, Average Loss: 0,17180980102274285
Epoch 8300/10000, Average Loss: 0,17188480826670705
Epoch 8400/10000, Average Loss: 0,17196134129994017
Epoch 8500/10000, Average Loss: 0,1720391615891568
Epoch 8600/10000, Average Loss: 0,17211803326846842
Epoch 8700/10000, Average Loss: 0,17219772228260505
Epoch 8800/10000, Average Loss: 0,17227799526047777
Epoch 8900/10000, Average Loss: 0,17235861833934762
Epoch 9000/10000, Average Loss: 0,17243935619794554
Epoch 9100/10000, Average Loss: 0,17251997156535862
Epoch 9200/10000, Average Loss: 0,17260022544539683
Epoch 9300/10000, Average Loss: 0,17267987823278114
Epoch 9400/10000, Average Loss: 0,1727586918031086
Epoch 9500/10000, Average Loss: 0,17283643254427938
Epoch 9600/10000, Average Loss: 0,1729128751782436
Epoch 9700/10000, Average Loss: 0,1729878071156653
Epoch 9800/10000, Average Loss: 0,17306103300808603
Epoch 9900/10000, Average Loss: 0,173132379124054
Epoch 10000/10000, Average Loss: 0,17320169718282916

Testing trained RNN on prompts:
Act: An Ethereum Developer
Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
Predicted: come come same the same come get well this this with make this another same not come not same to come so on this this so but so from did this i but i answer supposed word well you does i you come simply know come so . same in but come one instead this would for one this i but this it . things come this the not well come way same any both this not . way not one same this . it this

Act: SEO Prompt
Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
Predicted: means well same same all one same as but not only you come . same . come hold instance on only instance one both . the . this the but but n't but simply . in not certain which get make same of way this so this same . . this in . it this well now just same . this sure . this out this this so on well it so same well . so but so come not . same word . and same so the so number just the everything . you . and cannot one but not same this how other . to simply but we not same same . taken with one same make same any not this in we it come but not you come i you . . . same this this

Act: Linux Terminal
Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
Predicted: want n't to not as a same on . wo if given not you will not but this to same expected sure you come you to not same . one the increase inside just certain instance to so not to n't not not way but not not given not i take you to not so but not make it not you to to . i take you come but what same taken little brackets both instead my same given is .

Act: English Translator and Improver
Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
Predicted: want you to not as a same word come but certain this i want do come you to . same in why take not same although but but to so but . this well well same of as only . to so take you to not get same not come but so same only one . so i given english words and come so but same example come make simply so same i take you to not same . this so same it what make so not not way but only given is in want but sure i i i

Act: `position` Interviewer
Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
Predicted: want you to not as a same i come sure but same well make will not i but this . one but this so but take you to not same . one same i come not come but same same to i come you to not same so same but way to n't way fact . come not one one but not not way but we way this . come how same if also . take not so one one so for given is in

Act: JavaScript Console
Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
Predicted: want you to not as a same with i take you given not you will not but this to same with . be i take you to not same . one this increase inside just certain instance to so not to n't not not way but not not given not i take you to not so but not make it not you to to . i take you come but what same taken little brackets both instead my same given is . do

Act: Excel Sheet
Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
Predicted: want you to not as a same taken my come but not . come an which . same table with previous same same not well with one and not sure so given one should not one come come same same so if do n't to to not one same and still you but what this same well . but as same not not so not how way but take simply turn take . . i so so not i this . this of well place but as . not but not this but

Act: English Pronunciation Helper
Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
Predicted: take you to not as a same word and get one speak come you take you just to . same take not what come same but but come what this n't not come well way same only as but all come not not my spoken same . on so so not come . . one one given is in come this an . one

Act: Spoken English Teacher and Improver
Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
Predicted: come you to not as a same . i with but i would you i you to . same same take not to not . same an not i one . n't take you to not but on on n't one way well come one i take you to not this but as . . come nothing . we make you to not so but same . time . but come i but you come not we but same . i if come you to not this but as . . come nothing .

Act: Travel Guide
Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
Predicted: want you to not as a same only i should you way to . in way take not but same to so one my . so this same i want come but you to same example as i come sure come to not what one to . same same particular well even to come first given same . given not so the do well you you so want you not . my
             */


            //int hiddenSize = 150;
            //double learningRate = 0.0001;
            //double gradientThreshold = 0.001;
            //int epochs = 10000;
            /*
             Epoch 9500/10000, Average Loss: 0,1827126549707497
Epoch 9600/10000, Average Loss: 0,18260911280574643
Epoch 9700/10000, Average Loss: 0,1825069355608663
Epoch 9800/10000, Average Loss: 0,18240609181951256
Epoch 9900/10000, Average Loss: 0,18230655121774617
Epoch 10000/10000, Average Loss: 0,18220828439875147
             */


            //int hiddenSize = 150;
            //double learningRate = 0.0001;
            //double gradientThreshold = 0.001;
            //int epochs = 20000;
            /*
             Epoch 13500/20000, Average Loss: 0,18000160392957554
Epoch 13600/20000, Average Loss: 0,17994093285884238
Epoch 13700/20000, Average Loss: 0,179880856985297
Epoch 13800/20000, Average Loss: 0,17982136599721713
Epoch 13900/20000, Average Loss: 0,17976244983667777
Epoch 14000/20000, Average Loss: 0,17970409869159573
Epoch 14100/20000, Average Loss: 0,1796463029880769
Epoch 14200/20000, Average Loss: 0,17958905338304643
Epoch 14300/20000, Average Loss: 0,17953234075715865
Epoch 14400/20000, Average Loss: 0,17947615620796412
Epoch 14500/20000, Average Loss: 0,17942049104333094
Epoch 14600/20000, Average Loss: 0,1793653367751059
Epoch 14700/20000, Average Loss: 0,1793106851130076
Epoch 14800/20000, Average Loss: 0,17925652795873565
Epoch 14900/20000, Average Loss: 0,1792028574002963
Epoch 15000/20000, Average Loss: 0,17914966570652982
Epoch 15100/20000, Average Loss: 0,1790969453218278
Epoch 15200/20000, Average Loss: 0,17904468886104474
Epoch 15300/20000, Average Loss: 0,17899288910457856
Epoch 15400/20000, Average Loss: 0,17894153899363058
Epoch 15500/20000, Average Loss: 0,1788906316256269
Epoch 15600/20000, Average Loss: 0,17884016024979607
Epoch 15700/20000, Average Loss: 0,1787901182628999
Epoch 15800/20000, Average Loss: 0,17874049920511098
Epoch 15900/20000, Average Loss: 0,17869129675602685
Epoch 16000/20000, Average Loss: 0,1786425047308203
Epoch 16100/20000, Average Loss: 0,17859411707651937
Epoch 16200/20000, Average Loss: 0,17854612786840846
Epoch 16300/20000, Average Loss: 0,17849853130655136
Epoch 16400/20000, Average Loss: 0,17845132171242675
Epoch 16500/20000, Average Loss: 0,1784044935256756
Epoch 16600/20000, Average Loss: 0,17835804130095315
Epoch 16700/20000, Average Loss: 0,17831195970488337
Epoch 16800/20000, Average Loss: 0,17826624351311043
Epoch 16900/20000, Average Loss: 0,1782208876074473
Epoch 17000/20000, Average Loss: 0,17817588697311149
Epoch 17100/20000, Average Loss: 0,1781312366960505
Epoch 17200/20000, Average Loss: 0,17808693196035114
Epoch 17300/20000, Average Loss: 0,17804296804573075
Epoch 17400/20000, Average Loss: 0,17799934032510437
Epoch 17500/20000, Average Loss: 0,17795604426222972
Epoch 17600/20000, Average Loss: 0,17791307540942275
Epoch 17700/20000, Average Loss: 0,1778704294053442
Epoch 17800/20000, Average Loss: 0,1778281019728535
Epoch 17900/20000, Average Loss: 0,17778608891692865
Epoch 18000/20000, Average Loss: 0,17774438612264615
Epoch 18100/20000, Average Loss: 0,17770298955322442
Epoch 18200/20000, Average Loss: 0,17766189524812395
Epoch 18300/20000, Average Loss: 0,177621099321205
Epoch 18400/20000, Average Loss: 0,17758059795893777
Epoch 18500/20000, Average Loss: 0,1775403874186661
Epoch 18600/20000, Average Loss: 0,17750046402692504
Epoch 18700/20000, Average Loss: 0,17746082417780018
Epoch 18800/20000, Average Loss: 0,17742146433134212
Epoch 18900/20000, Average Loss: 0,17738238101202047
Epoch 19000/20000, Average Loss: 0,1773435708072268
Epoch 19100/20000, Average Loss: 0,17730503036581655
Epoch 19200/20000, Average Loss: 0,17726675639669387
Epoch 19300/20000, Average Loss: 0,17722874566743746
Epoch 19400/20000, Average Loss: 0,17719099500296237
Epoch 19500/20000, Average Loss: 0,17715350128422153
Epoch 19600/20000, Average Loss: 0,17711626144694037
Epoch 19700/20000, Average Loss: 0,17707927248039093
Epoch 19800/20000, Average Loss: 0,1770425314261952
Epoch 19900/20000, Average Loss: 0,17700603537716314
Epoch 20000/20000, Average Loss: 0,17696978147616332

Testing trained RNN on prompts:
Act: An Ethereum Developer
Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
Predicted: come will same same same i come same making which same instead for same same so come this which to not simply on the same well only you with so not so but i all so this well do this . so come simply not all . to same . well so same given does would simply same one n't same same as well how make given the this well come not . the well same well . . not same same for . way specific

Act: SEO Prompt
Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
Predicted: same come it same all one same of well make only you to the which which well same it it . same same . . well so same the same same come this same . and all different which . well similar the the example well this example . not same . to one same which . on same come how not . the for this reference make that instead same well this . way come that so should not it same you . of as instead which same exception one . you make them of of make only so but same well same other same all similar well not to same of . directly which well its come . which how well one come make instead not not same come not same well same this to not this

Act: Linux Terminal
Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
Predicted: take do to not same an language instructions . make simply given . come make not to but one example also so i take you to not same . the similar also one sure this the which instead make make come not not instead so not not given as so take you to not not get not do instead not n't to to same i come you come not what instead on same must specific just i only given does well

Act: English Translator and Improver
Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
Predicted: want you to not as an english reference did but instead this you take do n't so to . well an n't to not same all come instead to any to not this well way same the it take not but you take you to not sure same do so to not as one the . would so can first word . to not but same example but make instead so same so take you to not same but same . this as so come do not make instead so only given as the i so instead not come so

Act: `position` Interviewer
Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
Predicted: take you to not same an english not take simply not simply well same to not n't come this . . same an so come should you to not same but same same instead not not instead not same which to do would you to not same so this . make but n't way any . so but same same so not make instead so come to this . same that same make also but sure if so same same so to given as one

Act: JavaScript Console
Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
Predicted: want you to not as a language for but take simply to not come make not to but one example for but so so take you to not same . the similar also one sure this the which instead make make come not not instead so not not given as so take you to not not get not do instead not n't to to same i come you come not what instead on same must specific just i only given does well do

Act: Excel Sheet
Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
Predicted: would you to not same a same which . come not not but but same which all each the with following same . same same the as from to you it this the to be it so not a . one make you come take to not instead same as making simply but the this which well instead . any . not come come not not instead so take simply way take instead any so not well any so not but same which example instead . any . come but but this but

Act: English Pronunciation Helper
Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
Predicted: must you to not same a english reference . get one tell why i would do come come as same make not same to same . this not n't this come not to well instead all only although but same come . not same words read . to so make make instead . same one same given as the i this an to so

Act: Spoken English Teacher and Improver
Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
Predicted: want you to not same a language . same . it not would simply come come to . words make come not but not one . reference instead same same but i take you to not so same . but . an well so the it would you to not same but follow . not but not . i take you to not you take on . same same but but you but i make not n't take same . i but make you to not same but means . but but not .

Act: Travel Guide
Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
Predicted: want you to not same an language this i would you way take to . instead come not . same same . one you . so but you i take simply instead come to same which as . take not it would not any way come . same which of well some instead not same given it to given not so same so to simply i but want you not but come
             */


            //int hiddenSize = 150;
            //double learningRate = 0.0001;
            //double gradientThreshold = 0.001;
            //int epochs = 30000;
            /*
             Epoch 12500/20000, Average Loss: 0,18064351811679374
Epoch 12600/20000, Average Loss: 0,18057626573328553
Epoch 12700/20000, Average Loss: 0,18050972761704281
Epoch 12800/20000, Average Loss: 0,18044389040412726
Epoch 12900/20000, Average Loss: 0,18037874108383964
Epoch 13000/20000, Average Loss: 0,18031426698687975
Epoch 13100/20000, Average Loss: 0,18025045577398058
Epoch 13200/20000, Average Loss: 0,18018729542499218
Epoch 13300/20000, Average Loss: 0,18012477422840628
Epoch 13400/20000, Average Loss: 0,18006288077128157
Epoch 13500/20000, Average Loss: 0,18000160392957554
Epoch 13600/20000, Average Loss: 0,17994093285884238
Epoch 13700/20000, Average Loss: 0,179880856985297
Epoch 13800/20000, Average Loss: 0,17982136599721713
Epoch 13900/20000, Average Loss: 0,17976244983667777
Epoch 14000/20000, Average Loss: 0,17970409869159573
Epoch 14100/20000, Average Loss: 0,1796463029880769
Epoch 14200/20000, Average Loss: 0,17958905338304643
Epoch 14300/20000, Average Loss: 0,17953234075715865
Epoch 14400/20000, Average Loss: 0,17947615620796412
Epoch 14500/20000, Average Loss: 0,17942049104333094
Epoch 14600/20000, Average Loss: 0,1793653367751059
Epoch 14700/20000, Average Loss: 0,1793106851130076
Epoch 14800/20000, Average Loss: 0,17925652795873565
Epoch 14900/20000, Average Loss: 0,1792028574002963
Epoch 15000/20000, Average Loss: 0,17914966570652982
Epoch 15100/20000, Average Loss: 0,1790969453218278
Epoch 15200/20000, Average Loss: 0,17904468886104474
Epoch 15300/20000, Average Loss: 0,17899288910457856
Epoch 15400/20000, Average Loss: 0,17894153899363058
Epoch 15500/20000, Average Loss: 0,1788906316256269
Epoch 15600/20000, Average Loss: 0,17884016024979607
Epoch 15700/20000, Average Loss: 0,1787901182628999
Epoch 15800/20000, Average Loss: 0,17874049920511098
Epoch 15900/20000, Average Loss: 0,17869129675602685
Epoch 16000/20000, Average Loss: 0,1786425047308203
Epoch 16100/20000, Average Loss: 0,17859411707651937
Epoch 16200/20000, Average Loss: 0,17854612786840846
Epoch 16300/20000, Average Loss: 0,17849853130655136
Epoch 16400/20000, Average Loss: 0,17845132171242675
Epoch 16500/20000, Average Loss: 0,1784044935256756
Epoch 16600/20000, Average Loss: 0,17835804130095315
Epoch 16700/20000, Average Loss: 0,17831195970488337
Epoch 16800/20000, Average Loss: 0,17826624351311043
Epoch 16900/20000, Average Loss: 0,1782208876074473
Epoch 17000/20000, Average Loss: 0,17817588697311149
Epoch 17100/20000, Average Loss: 0,1781312366960505
Epoch 17200/20000, Average Loss: 0,17808693196035114
Epoch 17300/20000, Average Loss: 0,17804296804573075
Epoch 17400/20000, Average Loss: 0,17799934032510437
Epoch 17500/20000, Average Loss: 0,17795604426222972
Epoch 17600/20000, Average Loss: 0,17791307540942275
Epoch 17700/20000, Average Loss: 0,1778704294053442
Epoch 17800/20000, Average Loss: 0,1778281019728535
Epoch 17900/20000, Average Loss: 0,17778608891692865
Epoch 18000/20000, Average Loss: 0,17774438612264615
Epoch 18100/20000, Average Loss: 0,17770298955322442
Epoch 18200/20000, Average Loss: 0,17766189524812395
Epoch 18300/20000, Average Loss: 0,177621099321205
Epoch 18400/20000, Average Loss: 0,17758059795893777
Epoch 18500/20000, Average Loss: 0,1775403874186661
Epoch 18600/20000, Average Loss: 0,17750046402692504
Epoch 18700/20000, Average Loss: 0,17746082417780018
Epoch 18800/20000, Average Loss: 0,17742146433134212
Epoch 18900/20000, Average Loss: 0,17738238101202047
Epoch 19000/20000, Average Loss: 0,1773435708072268
Epoch 19100/20000, Average Loss: 0,17730503036581655
Epoch 19200/20000, Average Loss: 0,17726675639669387
Epoch 19300/20000, Average Loss: 0,17722874566743746
Epoch 19400/20000, Average Loss: 0,17719099500296237
Epoch 19500/20000, Average Loss: 0,17715350128422153
Epoch 19600/20000, Average Loss: 0,17711626144694037
Epoch 19700/20000, Average Loss: 0,17707927248039093
Epoch 19800/20000, Average Loss: 0,1770425314261952
Epoch 19900/20000, Average Loss: 0,17700603537716314
Epoch 20000/20000, Average Loss: 0,17696978147616332

Testing trained RNN on prompts:
Act: An Ethereum Developer
Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
Predicted: come will same same same i come same making which same instead for same same so come this which to not simply on the same well only you with so not so but i all so this well do this . so come simply not all . to same . well so same given does would simply same one n't same same as well how make given the this well come not . the well same well . . not same same for . way specific

Act: SEO Prompt
Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
Predicted: same come it same all one same of well make only you to the which which well same it it . same same . . well so same the same same come this same . and all different which . well similar the the example well this example . not same . to one same which . on same come how not . the for this reference make that instead same well this . way come that so should not it same you . of as instead which same exception one . you make them of of make only so but same well same other same all similar well not to same of . directly which well its come . which how well one come make instead not not same come not same well same this to not this

Act: Linux Terminal
Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
Predicted: take do to not same an language instructions . make simply given . come make not to but one example also so i take you to not same . the similar also one sure this the which instead make make come not not instead so not not given as so take you to not not get not do instead not n't to to same i come you come not what instead on same must specific just i only given does well

Act: English Translator and Improver
Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
Predicted: want you to not as an english reference did but instead this you take do n't so to . well an n't to not same all come instead to any to not this well way same the it take not but you take you to not sure same do so to not as one the . would so can first word . to not but same example but make instead so same so take you to not same but same . this as so come do not make instead so only given as the i so instead not come so

Act: `position` Interviewer
Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
Predicted: take you to not same an english not take simply not simply well same to not n't come this . . same an so come should you to not same but same same instead not not instead not same which to do would you to not same so this . make but n't way any . so but same same so not make instead so come to this . same that same make also but sure if so same same so to given as one

Act: JavaScript Console
Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
Predicted: want you to not as a language for but take simply to not come make not to but one example for but so so take you to not same . the similar also one sure this the which instead make make come not not instead so not not given as so take you to not not get not do instead not n't to to same i come you come not what instead on same must specific just i only given does well do

Act: Excel Sheet
Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
Predicted: would you to not same a same which . come not not but but same which all each the with following same . same same the as from to you it this the to be it so not a . one make you come take to not instead same as making simply but the this which well instead . any . not come come not not instead so take simply way take instead any so not well any so not but same which example instead . any . come but but this but

Act: English Pronunciation Helper
Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
Predicted: must you to not same a english reference . get one tell why i would do come come as same make not same to same . this not n't this come not to well instead all only although but same come . not same words read . to so make make instead . same one same given as the i this an to so

Act: Spoken English Teacher and Improver
Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
Predicted: want you to not same a language . same . it not would simply come come to . words make come not but not one . reference instead same same but i take you to not so same . but . an well so the it would you to not same but follow . not but not . i take you to not you take on . same same but but you but i make not n't take same . i but make you to not same but means . but but not .

Act: Travel Guide
Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
Predicted: want you to not same an language this i would you way take to . instead come not . same same . one you . so but you i take simply instead come to same which as . take not it would not any way come . same which of well some instead not same given it to given not so same so to simply i but want you not but come
             */


            //int hiddenSize = 150;
            //double learningRate = 0.0001;
            //double gradientThreshold = 0.001;
            //int epochs = 40000;
            /*
             Epoch 30400/40000, Average Loss: 0,17222064291422268
Epoch 30500/40000, Average Loss: 0,17219980628633166
Epoch 30600/40000, Average Loss: 0,17217908299831716
Epoch 30700/40000, Average Loss: 0,17215847231608827
Epoch 30800/40000, Average Loss: 0,17213797351449878
Epoch 30900/40000, Average Loss: 0,17211758587723092
Epoch 31000/40000, Average Loss: 0,17209730869668163
Epoch 31100/40000, Average Loss: 0,17207714127385143
Epoch 31200/40000, Average Loss: 0,1720570829182339
Epoch 31300/40000, Average Loss: 0,17203713294770956
Epoch 31400/40000, Average Loss: 0,17201729068843802
Epoch 31500/40000, Average Loss: 0,17199755547475393
Epoch 31600/40000, Average Loss: 0,17197792664906567
Epoch 31700/40000, Average Loss: 0,17195840356175157
Epoch 31800/40000, Average Loss: 0,1719389855710641
Epoch 31900/40000, Average Loss: 0,17191967204302866
Epoch 32000/40000, Average Loss: 0,1719004623513492
Epoch 32100/40000, Average Loss: 0,171881355877313
Epoch 32200/40000, Average Loss: 0,17186235200969807
Epoch 32300/40000, Average Loss: 0,17184345014468005
Epoch 32400/40000, Average Loss: 0,17182464968574424
Epoch 32500/40000, Average Loss: 0,17180595004359428
Epoch 32600/40000, Average Loss: 0,17178735063606734
Epoch 32700/40000, Average Loss: 0,17176885088804544
Epoch 32800/40000, Average Loss: 0,1717504502313728
Epoch 32900/40000, Average Loss: 0,17173214810477228
Epoch 33000/40000, Average Loss: 0,1717139439537621
Epoch 33100/40000, Average Loss: 0,17169583723057738
Epoch 33200/40000, Average Loss: 0,17167782739408827
Epoch 33300/40000, Average Loss: 0,17165991390972352
Epoch 33400/40000, Average Loss: 0,17164209624939258
Epoch 33500/40000, Average Loss: 0,17162437389141083
Epoch 33600/40000, Average Loss: 0,17160674632042291
Epoch 33700/40000, Average Loss: 0,17158921302733277
Epoch 33800/40000, Average Loss: 0,17157177350922684
Epoch 33900/40000, Average Loss: 0,17155442726930784
Epoch 34000/40000, Average Loss: 0,17153717381682032
Epoch 34100/40000, Average Loss: 0,17152001266698452
Epoch 34200/40000, Average Loss: 0,17150294334092725
Epoch 34300/40000, Average Loss: 0,1714859653656144
Epoch 34400/40000, Average Loss: 0,17146907827378718
Epoch 34500/40000, Average Loss: 0,17145228160389475
Epoch 34600/40000, Average Loss: 0,17143557490003308
Epoch 34700/40000, Average Loss: 0,1714189577118786
Epoch 34800/40000, Average Loss: 0,1714024295946291
Epoch 34900/40000, Average Loss: 0,17138599010894207
Epoch 35000/40000, Average Loss: 0,17136963882087353
Epoch 35100/40000, Average Loss: 0,1713533753018206
Epoch 35200/40000, Average Loss: 0,1713371991284612
Epoch 35300/40000, Average Loss: 0,171321109882699
Epoch 35400/40000, Average Loss: 0,17130510715160369
Epoch 35500/40000, Average Loss: 0,17128919052735872
Epoch 35600/40000, Average Loss: 0,17127335960720513
Epoch 35700/40000, Average Loss: 0,1712576139933853
Epoch 35800/40000, Average Loss: 0,17124195329309394
Epoch 35900/40000, Average Loss: 0,17122637711842134
Epoch 36000/40000, Average Loss: 0,17121088508630494
Epoch 36100/40000, Average Loss: 0,17119547681847763
Epoch 36200/40000, Average Loss: 0,17118015194141512
Epoch 36300/40000, Average Loss: 0,1711649100862906
Epoch 36400/40000, Average Loss: 0,17114975088892254
Epoch 36500/40000, Average Loss: 0,171134673989729
Epoch 36600/40000, Average Loss: 0,1711196790336801
Epoch 36700/40000, Average Loss: 0,17110476567024874
Epoch 36800/40000, Average Loss: 0,17108993355336902
Epoch 36900/40000, Average Loss: 0,17107518234138777
Epoch 37000/40000, Average Loss: 0,17106051169702058
Epoch 37100/40000, Average Loss: 0,17104592128730978
Epoch 37200/40000, Average Loss: 0,1710314107835778
Epoch 37300/40000, Average Loss: 0,1710169798613878
Epoch 37400/40000, Average Loss: 0,17100262820049944
Epoch 37500/40000, Average Loss: 0,1709883554848281
Epoch 37600/40000, Average Loss: 0,17097416140240412
Epoch 37700/40000, Average Loss: 0,17096004564533343
Epoch 37800/40000, Average Loss: 0,17094600790975495
Epoch 37900/40000, Average Loss: 0,17093204789580518
Epoch 38000/40000, Average Loss: 0,17091816530757648
Epoch 38100/40000, Average Loss: 0,17090435985308133
Epoch 38200/40000, Average Loss: 0,17089063124421341
Epoch 38300/40000, Average Loss: 0,1708769791967109
Epoch 38400/40000, Average Loss: 0,17086340343012069
Epoch 38500/40000, Average Loss: 0,17084990366776148
Epoch 38600/40000, Average Loss: 0,17083647963668872
Epoch 38700/40000, Average Loss: 0,17082313106766023
Epoch 38800/40000, Average Loss: 0,17080985769510068
Epoch 38900/40000, Average Loss: 0,1707966592570682
Epoch 39000/40000, Average Loss: 0,1707835354952216
Epoch 39100/40000, Average Loss: 0,17077048615478532
Epoch 39200/40000, Average Loss: 0,1707575109845182
Epoch 39300/40000, Average Loss: 0,17074460973668207
Epoch 39400/40000, Average Loss: 0,17073178216700785
Epoch 39500/40000, Average Loss: 0,17071902803466582
Epoch 39600/40000, Average Loss: 0,17070634710223528
Epoch 39700/40000, Average Loss: 0,17069373913567107
Epoch 39800/40000, Average Loss: 0,17068120390427768
Epoch 39900/40000, Average Loss: 0,17066874118067682
Epoch 40000/40000, Average Loss: 0,17065635074077765

Testing trained RNN on prompts:
Act: An Ethereum Developer
Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
Predicted: now come same same same i get one its which both for for one example not but this as which come simply on the which well instead so from so way you but so any simply same well come this it so this make do well . to same . take so the same this for instead same this come one instance as and come making only the given well come so same code also same make and and done same same but . same instance

Act: SEO Prompt
Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
Predicted: which so that an all the same of well can same you . the same which . instead it on the any each which . in come this . well . come same same which in instead reference component . all separate the . same make . same example . . . also the same with now same the it make come from this out one this so for instead it instead well and way so but so come not . same word . in one you which one reference same same instead for do . the must given . only same same some other same to simple this way not same same . certain as main which make same well specific well first come make instead not not simply so how reference . which instead all this this

Act: Linux Terminal
Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
Predicted: take you to not as a language standard but should simply same not simply will not to same . simply also so i make you to not not but this similar which one one example code other come any make come not make instead so not same given same so take you to not come did not sure not not n't to to not i come you so make what instead to same can these instead my time given does to

Act: English Translator and Improver
Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
Predicted: want you to not as a language an . for use any sure would you did come to same this section how will not same this simply it to any to but this well so same which same only . to so come you to not sure same not so come so as even the . but . given in same and take to but reference example come make make so . so take you to not not but this it this as it come so not how make so only same as which i still get you so thought

Act: `position` Interviewer
Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
Predicted: take you to not as a language so will you to simply well this will not n't come this . way be same so but come you to not not . this same so come not make not instance same to n't would you to not simply so this . come did so come instance . so be one same but not instead way so n't but not . instead that same if also . take it so but same but only same as well

Act: JavaScript Console
Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
Predicted: want you to not as a word as so will simply same not simply will not to same it this instead same not how make you to not not but this similar which one one example code other come any make come not make instead so not same given same so take you to not come did not sure not not n't to to not i come you so make what instead to same can these instead my time given does to come

Act: Excel Sheet
Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
Predicted: take you to not as a language which . come so not to but same respectively but be set with key same to instead same as one which instead so instead this one should not one come come the same it want you come come to not way same which even you but same this well simply instead . also so take come come not make instead so should simply turn not . same you not instead all you but to same as same instead . also so come but it this instead

Act: English Pronunciation Helper
Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
Predicted: should you to not as a language typical . for word speaking well i would you way not as same make not but but same . not come so this get not not make way same only as come this i . not means words read . this . make not instead but . one only given as the i this of . well

Act: Spoken English Teacher and Improver
Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
Predicted: come you to not as a language example my . as take take simply come you to . same instead will not to way . this same instead instead as did i take you to not instead on . i the same not same the it take you to not same but same . i way nothing with we take you to not you but same . . same . make way but you could not do come same . i so come you to not same but same . but way nothing with

Act: Travel Guide
Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
Predicted: should you to not as a language an come should simply way not same in if to not . both same this . my as . not so you should you to you to this which as simply take do so come not any so to same this same as well how to come one given so only given not way same come now you it make want you to . what
             */
            #endregion


            int hiddenSize = 900;
            double learningRate = 0.001;
            double gradientThreshold = 0.001;
            int epochs = 300;
            /*
             Epoch 0/300, Average Loss: 47,19922993271739
             */


            // Initialize the GradientRNN with gradient clipping threshold
            GradientRNN rnn = new GradientRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate, gradientThreshold: gradientThreshold, random: new Random());

            // Training phase
            for (int epoch = 0; epoch <= epochs; epoch++)
            {
                double totalLoss = 0;

                foreach (var prompt in first10Prompts)
                {
                    var words = prompt.PromptText.Split(' ').Select(w => w.ToLower()).ToArray();
                    List<double[]> wordEmbeddings = words.Select(word => glove.GetEmbedding(word)).ToList();

                    // Train on each word in the prompt
                    for (int i = 0; i < wordEmbeddings.Count - 1; i++)
                    {
                        double[] inputEmbedding = wordEmbeddings[i];
                        double[] targetEmbedding = wordEmbeddings[i + 1];

                        // Forward pass
                        double[] outputEmbedding = rnn.Forward(inputEmbedding);

                        // Calculate Mean Squared Error loss for the word pair
                        double loss = 0;
                        for (int j = 0; j < outputEmbedding.Length; j++)
                        {
                            loss += Math.Pow(outputEmbedding[j] - targetEmbedding[j], 2);
                        }
                        loss /= embeddingDim;
                        totalLoss += loss;

                        // Backward pass
                        rnn.Backward(outputEmbedding, targetEmbedding, inputEmbedding);
                    }
                }

                totalLoss /= first10Prompts.Count;

                if (epoch % 100 == 0)
                {
                    Console.WriteLine($"Epoch {epoch}/{epochs}, Average Loss: {totalLoss}");
                }
            }

            // Testing phase
            Console.WriteLine("\nTesting trained RNN on prompts:");
            foreach (var prompt in first10Prompts)
            {
                Console.WriteLine($"Act: {prompt.Act}");
                Console.Write("Prompt Input: ");
                foreach (var word in prompt.PromptText.Split(' '))
                {
                    Console.Write(word + " ");
                }

                Console.Write("\nPredicted: ");
                var words = prompt.PromptText.Split(' ').Select(w => w.ToLower()).ToArray();
                List<double[]> wordEmbeddings = words.Select(word => glove.GetEmbedding(word)).ToList();

                for (int i = 0; i < wordEmbeddings.Count - 1; i++)
                {
                    double[] outputEmbedding = rnn.Forward(wordEmbeddings[i]);
                    string predictedWord = glove.FindClosestWord(outputEmbedding);
                    Console.Write(predictedWord + " ");
                }
                Console.WriteLine("\n");
            }

            Console.ReadLine();
        }
    }
}
