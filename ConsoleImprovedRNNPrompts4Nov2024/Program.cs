using LibraryGlobalVectors1Sep2024;
using LibraryImprovedRNN1Nov2024;
using LibraryPrompts4Nov2024;

namespace ConsoleImprovedRNNPrompts4Nov2024
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
            //double learningRate = 0.1;
            //int epochs = 20000;
            //The average loss increases
            //Epoch 19601 / 20000, Average Loss: 0,7303837995863361
            //Epoch 19701 / 20000, Average Loss: 0,7303838027452236
            //Epoch 19801 / 20000, Average Loss: 0,7303838058721484
            //Epoch 19901 / 20000, Average Loss: 0,7303838089675928

            //int hiddenSize = 3;
            //double learningRate = 0.01;
            //int epochs = 20000;
            ////Epoch 19601 / 20000, Average Loss: 0,4398205059391791
            ////Epoch 19701 / 20000, Average Loss: 0,43982052882596595
            ////Epoch 19801 / 20000, Average Loss: 0,4398205514810309
            ////Epoch 19901 / 20000, Average Loss: 0,4398205739078733
            // rni suppose mankern

            //int hiddenSize = 3;
            //double learningRate = 0.001;
            //int epochs = 20000;
            ////Epoch 1701 / 20000, Average Loss: 0,20080374245407467
            ////Epoch 1801 / 20000, Average Loss: 0,20092838874782956
            ////Epoch 1901 / 20000, Average Loss: 0,20109528271816965
            ////Epoch 19701 / 20000, Average Loss: 0,42168179361786395
            ////Epoch 19801 / 20000, Average Loss: 0,4216825578989069
            ////Epoch 19901 / 20000, Average Loss: 0,42168330811362675
            //// v ahn-chae kv ahn-chae kv a

            //int hiddenSize = 3;
            //double learningRate = 0.0001;
            //int epochs = 10000;
            ////Epoch 9701 / 10000, Average Loss: 0,2038722889343115
            ////Epoch 9801 / 10000, Average Loss: 0,20384370022933013
            ////Epoch 9901 / 10000, Average Loss: 0,2038151995484913
            //// me so which so to you same s

            //int hiddenSize = 3;
            //double learningRate = 0.00001;
            //int epochs = 10000;
            ////Epoch 9701 / 10000, Average Loss: 0,21504478806429345
            ////Epoch 9801 / 10000, Average Loss: 0,21501809852236137
            ////Epoch 9901 / 10000, Average Loss: 0,21499180641202717
            //// but but instead
            #endregion

            #region hiddenSize 10
            //int hiddenSize = 10;
            //double learningRate = 0.0001;
            //int epochs = 10000;
            //Epoch 9701 / 10000, Average Loss: 0,1968696220369003
            //Epoch 9801 / 10000, Average Loss: 0,19682482206944296
            //Epoch 9901 / 10000, Average Loss: 0,1967811395716275
            //the make not not same

            //int hiddenSize = 10;
            //double learningRate = 0.001;
            //int epochs = 10000;
            ////Epoch 4601 / 10000, Average Loss: 0,19265209082236223
            ////Epoch 4701 / 10000, Average Loss: 0,19266949606719555
            ////Epoch 4801 / 10000, Average Loss: 0,19269204009779148

            //int hiddenSize = 10;
            //double learningRate = 0.001;
            //int epochs = 4800;
            //Epoch 4501 / 4800, Average Loss: 0,1933847237141782
            //Epoch 4601 / 4800, Average Loss: 0,19332891094968574
            //Epoch 4701 / 4800, Average Loss: 0,19327757139533092
            // any but the so not but so not answer i do answer . well not same in

            //int hiddenSize = 10;
            //double learningRate = 0.002;
            //int epochs = 5000;
            /*
            Epoch 2300/5000, Average Loss: 0,19177389154644264
            Epoch 2400/5000, Average Loss: 0,1916848416292775
            Epoch 2500/5000, Average Loss: 0,19155112912355357
            Epoch 2600/5000, Average Loss: 0,19138067648627352
            Epoch 2700/5000, Average Loss: 0,1912349492704871
            Epoch 2800/5000, Average Loss: 0,19115722426684428
            Epoch 2900/5000, Average Loss: 0,19118651758641475
            Epoch 3000/5000, Average Loss: 0,19139577123621693
            Epoch 3100/5000, Average Loss: 0,19176498896547747
            Epoch 3200/5000, Average Loss: 0,1921442452459198
            Epoch 3300/5000, Average Loss: 0,19252488852545424
            Epoch 3400/5000, Average Loss: 0,19243664168756122
            Epoch 3500/5000, Average Loss: 0,19209275796263897
            Epoch 3600/5000, Average Loss: 0,19186693923413278
            Epoch 3700/5000, Average Loss: 0,19194068801485778
            Epoch 3800/5000, Average Loss: 0,1921656097467185
            Epoch 3900/5000, Average Loss: 0,19229437205002622
            Epoch 4000/5000, Average Loss: 0,1923431002623164
            Epoch 4100/5000, Average Loss: 0,19233565209927064
            Epoch 4200/5000, Average Loss: 0,1922926973595358
            Epoch 4300/5000, Average Loss: 0,1922404907986023
            Epoch 4400/5000, Average Loss: 0,1921910338273374
            Epoch 4500/5000, Average Loss: 0,19211096820084966
            Epoch 4600/5000, Average Loss: 0,1919655077187747
            Epoch 4700/5000, Average Loss: 0,19175475263283587
            Epoch 4800/5000, Average Loss: 0,191528819958328
            Epoch 4900/5000, Average Loss: 0,19134228798814576
            Epoch 5000/5000, Average Loss: 0,1911983540202752

            Testing trained RNN on prompts:
            Act: An Ethereum Developer
            Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
            Predicted: but but the same same but come . to . same to same same same come come this . only come you . which not i to but . come come but . come not come same same n't it same come come but n't come it which same . to come same same so to same same . come . same but to but but same . only but n't come same given . come . . but but . same but . . not

            Act: SEO Prompt
            Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
            Predicted: but come . same . but same . but not but i to . same . come only so so . same same . . . come . . . . but but same . . but this . come . . . but same come . same . come . . to but this . come . . . but come . but . same same come to . not but . . but come to but come come same same i . . come come . same answer . but come come n't come . come this so not . but . to . come same so come to same same but . . . . come . but this but only come come but but not same but i not . only but not but .

            Act: Linux Terminal
            Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
            Predicted: want you to to same same same misspelled n't want you to not but to not . . to same to come but come you to not simply one same same not . . not same . come come but come come come come come come come same but . would you to not so so come to to not i to to same but take you come to simply same which but take i n't come same given but .

            Act: English Translator and Improver
            Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
            Predicted: want you to not same same same same come come come but but want you you not to same same a not take not same same so . to come but . same but come same . but but but but but want you to not you one in not . . . but . . not come i an same take but come but same same come come not come same but take you to not simply one same . not . but to come take come come come this same . only come come n't come come come

            Act: `position` Interviewer
            Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
            Predicted: want you to should same one an but would you same answer but . to not come to same . . . same come come come you to not simply one same same come take come come not same same not but come you to not simply but same . same but come to same . but but same same come come come come come come to same . not but same not same . take but but same same but this same . to

            Act: JavaScript Console
            Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
            Predicted: want you to not same same same . come come you same not but to not . . to same . . but come take you to not simply one same same come . . not same . come come but come come come come come come come same but . would you to not so so come to to not i to to same but take you come to simply same which but take i n't come same given but . come

            Act: Excel Sheet
            Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
            Predicted: want you to not same one same . but come come not . to same . but . . . not . . but . . . . come come so not . should come but but come . same come come you n't to not you one same same come come . to same same . . . but but not but come come come come come come you come to so you one to to come so not . same . same . . but but come to to same but

            Act: English Pronunciation Helper
            Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
            Predicted: want you to not same one same same . n't same you but but take you come to same but to not same but same but come but come this come come but not but but same . not but so . but but i same . . but come come come but . . not same . to i not in only but

            Act: Spoken English Teacher and Improver
            Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
            Predicted: come you to not same same same . so . . come come you you not to same same same to not . but to same same come come same same come come you to not you an as but . same but come same but take you to not same one same . come come but . come come you to not you but in which but the . come but to but take come you take same as come but come you to not same one this . come come but .

            Act: Travel Guide
            Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
            Predicted: want you to not same same same same come come you come would same same come to not . same but come . come not so but same come take you not simply to same same but . would come so would not same . to . same . . but . but come same same so only same come but which come to . but come come you you to same
            */


            //int hiddenSize = 10;
            //double learningRate = 0.005;
            //int epochs = 10000;
            /*
            Epoch 6200/10000, Average Loss: 0,453984035838468
            Epoch 6300/10000, Average Loss: 0,4539843669871555
            Epoch 6400/10000, Average Loss: 0,4539846875696957
            Epoch 6500/10000, Average Loss: 0,45398499808345233
            Epoch 6600/10000, Average Loss: 0,4539852989950752
            Epoch 6700/10000, Average Loss: 0,45398559074282463
            Epoch 6800/10000, Average Loss: 0,45398587373870536
            Epoch 6900/10000, Average Loss: 0,45398614837039464
            Epoch 7000/10000, Average Loss: 0,45398641500301934
            Epoch 7100/10000, Average Loss: 0,4539866739807684
            Epoch 7200/10000, Average Loss: 0,45398692562836757
            Epoch 7300/10000, Average Loss: 0,4539871702524449
            Epoch 7400/10000, Average Loss: 0,4539874081427658
            Epoch 7500/10000, Average Loss: 0,4539876395733796
            Epoch 7600/10000, Average Loss: 0,45398786480367087
            Epoch 7700/10000, Average Loss: 0,45398808407932467
            Epoch 7800/10000, Average Loss: 0,45398829763322357
            Epoch 7900/10000, Average Loss: 0,4539885056862646
            Epoch 8000/10000, Average Loss: 0,4539887084481243
            Epoch 8100/10000, Average Loss: 0,4539889061179621
            Epoch 8200/10000, Average Loss: 0,4539890988850682
            Epoch 8300/10000, Average Loss: 0,45398928692947144
            Epoch 8400/10000, Average Loss: 0,45398947042249527
            Epoch 8500/10000, Average Loss: 0,45398964952728094
            Epoch 8600/10000, Average Loss: 0,45398982439926333
            Epoch 8700/10000, Average Loss: 0,4539899951866322
            Epoch 8800/10000, Average Loss: 0,4539901620307365
            Epoch 8900/10000, Average Loss: 0,4539903250664846
            Epoch 9000/10000, Average Loss: 0,45399048442269885
            Epoch 9100/10000, Average Loss: 0,4539906402224611
            Epoch 9200/10000, Average Loss: 0,45399079258342645
            Epoch 9300/10000, Average Loss: 0,4539909416181186
            Epoch 9400/10000, Average Loss: 0,453991087434208
            Epoch 9500/10000, Average Loss: 0,45399123013476544
            Epoch 9600/10000, Average Loss: 0,4539913698185144
            Epoch 9700/10000, Average Loss: 0,45399150658005044
            Epoch 9800/10000, Average Loss: 0,4539916405100556
            Epoch 9900/10000, Average Loss: 0,45399177169550076
            Epoch 10000/10000, Average Loss: 0,45399190021983377

            Testing trained RNN on prompts:
            Act: An Ethereum Developer
            Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
            Predicted: suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni

            Act: SEO Prompt
            Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
            Predicted: suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose

            Act: Linux Terminal
            Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
            Predicted: mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni

            Act: English Translator and Improver
            Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
            Predicted: suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni

            Act: `position` Interviewer
            Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
            Predicted: suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose

            Act: JavaScript Console
            Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
            Predicted: mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni

            Act: Excel Sheet
            Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
            Predicted: suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose

            Act: English Pronunciation Helper
            Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
            Predicted: mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose

            Act: Spoken English Teacher and Improver
            Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
            Predicted: mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose

            Act: Travel Guide
            Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
            Predicted: mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose mankerni suppose
            */


            //int hiddenSize = 10;
            //double learningRate = 0.01;
            //int epochs = 10000;
            //Epoch 1 / 10000, Average Loss: 0,29700032764831996
            //Epoch 101 / 10000, Average Loss: 0,1991771249228372
            //Epoch 201 / 10000, Average Loss: 0,1968223975920004
            //Epoch 301 / 10000, Average Loss: 0,19729736022103073
            //Epoch 401 / 10000, Average Loss: 0,1987387562981857
            //Epoch 501 / 10000, Average Loss: 0,4914936654090153

            //int hiddenSize = 10;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //Epoch 1 / 10000, Average Loss: 0,23792612156700974
            //Epoch 101 / 10000, Average Loss: 6249228151,926357
            #endregion

            #region hiddenSize 30
            //int hiddenSize = 30;
            //double learningRate = 0.1;
            //int epochs = 10000;
            //// NaN

            //int hiddenSize = 30;
            //double learningRate = 0.000001;
            //int epochs = 10000;
            //Epoch 9701 / 10000, Average Loss: 0,23896833052667388
            //Epoch 9801 / 10000, Average Loss: 0,23886643591857
            //Epoch 9901 / 10000, Average Loss: 0,23876531395394923
            // to no not same but not not same
            #endregion


            //int hiddenSize = 40;
            //double learningRate = 0.00001;
            //int epochs = 20000;
            ////Epoch 19900 / 20000, Average Loss: 0,20417892668146004
            ////Epoch 20000 / 20000, Average Loss: 0,20412317483966652
            //// come to instead similar but


            //int hiddenSize = 45;
            //double learningRate = 0.00001;
            //int epochs = 20000;
            /*
            Testing trained RNN on prompts:
            Act: An Ethereum Developer
            Prompt Input: Imagine you are an experienced Ethereum developer tasked with creating a smart contract for a blockchain messenger. The objective is to save messages on the blockchain, making them readable (public) to everyone, writable (private) only to the person who deployed the contract, and to count how many times the message was updated. Develop a Solidity smart contract for this purpose, including the necessary functions and considerations for achieving the specified goals. Please provide the code and any relevant explanations to ensure a clear understanding of the implementation.
            Predicted: come to not same instead but it one same same same instead same same which instead but same . this instead not . same same . it instead on so not so but so it not same to n't same same instead but same so come it . same . same but same same same to same one for so same same well same . same not same same well n't come same same which . same well same not instead same well it same same

            Act: SEO Prompt
            Prompt Input: Using WebPilot, create an outline for an article that will be 2,000 words on the keyword 'Best SEO prompts' based on the top 10 results from Google. Include every relevant heading possible. Keep the keyword density of the headings high. For each section of the outline, include the word count. Include FAQs section in the outline too, based on people also ask section from Google for the keyword. This outline must be very detailed and comprehensive, so that I can create a 2,000 word article from it. Generate a long list of LSI and NLP keywords related to my keyword. Also include any other words related to the keyword. Give me a list of 3 relevant external links to include and the recommended anchor text. Make sure they're not competing articles. Split the outline into part 1 and part 2.
            Predicted: same so same same which same same which . make not so all same same . so same they this for same this . but this so same to . it so if same . well this which same so same same of same same . same same which so same same of which same well so this for it same not same . on this same . same well so to to which so but to it come instead not same same same . same so it same same same same instead same how even which instead to so that same same same . same instead same but so come same same . instead . same . instead same same same well . so make make so not instead but not same which same which all same same

            Act: Linux Terminal
            Prompt Input: I want you to act as a linux terminal. I will type commands and you will reply with what the terminal should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is pwd
            Predicted: to come take not not . as provided but come not same same same to not but same to same . so so come come take not not but same same . well same only reference which . same to so come not but so come not same instead not come you come not not come if take not not come take to this so come not come not not but same same either same come so to same this as

            Act: English Translator and Improver
            Prompt Input: I want you to act as an English translator, spelling corrector and improver. I will speak to you in any language and you will detect the language, translate it and answer in the corrected and improved version of my text, in English. I want you to replace my simplified A0-level words and sentences with more beautiful and elegant, upper level English words and sentences. Keep the meaning same, but make them more literary. I want you to only reply the correction, the improvements and nothing else, do not write explanations. My first sentence is ""istanbulu cok seviyom burada olmak cok guzel
            Predicted: take come take not not . as same . same well instead so come not come not to this same same instead to not so this but it to same not this same . even same the same to so this it come come take not not but same so all not it same . to same so given which the which not instead but same . but not make not same so take come take not not but same . same . same to so come not but so to same well this instead we so come come come

            Act: `position` Interviewer
            Prompt Input: I want you to act as an interviewer. I will be the candidate and you will ask me the interview questions for the `position` position. I want you to only reply as the interviewer. Do not write all the conservation at once. I want you to only do the interview with me. Ask me the questions and wait for my answers. Do not write explanations. Ask me the questions one by one like an interviewer does and wait for my answers. My first sentence is ""Hi
            Predicted: come come take not not . as so take not not same so instead to not n't come same . same so same . so come come take not not but this same . come not to not same same well so come come take not not come same to same i if take same . so instead same to but come not but so come come same . not it same it same to come instead instead same to but to same . this

            Act: JavaScript Console
            Prompt Input: I want you to act as a javascript console. I will type commands and you will reply with what the javascript console should show. I want you to only reply with the terminal output inside one unique code block, and nothing else. do not write explanations. do not type commands unless I instruct you to do so. when i need to tell you something in english, i will do so by putting text inside curly brackets {like this}. my first command is console.log(""Hello World"");
            Predicted: to come take not not . as the so come not same same same to not but same to same the to not so come come take not not but same same . well same only reference which . same to so come not but so come not same instead not come you come not not come if take not not come take to this so come not come not not but same same either same come so to same this as so

            Act: Excel Sheet
            Prompt Input: I want you to act as a text based excel. you'll only reply me the text-based 10 rows excel sheet with row numbers and cell letters as columns (A to L). First column header should be empty to reference row number. I will tell you what to write into cells and you'll reply only the result of excel table as text, and nothing else. Do not write explanations. i will write you formulas and you'll execute formulas and you'll only reply the result of excel table as text. First, reply me the empty sheet.
            Predicted: come come take not not . as which this instead so not to come same on well same to one same same same same instead same this which so so so same . as so not . not . same but come not n't take not not but same which not so but same this same this instead one same but same to so come not but so come not but come same come but so same instead but not well same . this instead one same but so but come same .

            Act: English Pronunciation Helper
            Prompt Input: I want you to act as an English pronunciation assistant for Turkish speaking people. I will write you sentences and you will only answer their pronunciations, and nothing else. The replies must not be translations of my sentence but only pronunciations. Pronunciations should use Turkish Latin letters for phonetics. Do not write explanations on replies. My first sentence is ""how the weather is in Istanbul?
            Predicted: would do take not not . as same of not same same . instead come not but come so not to not not come not but same to so same to so not not the this to same but to so same not same this the . that way not not . so this it to same . this instead this . this .

            Act: Spoken English Teacher and Improver
            Prompt Input: I want you to act as a spoken English teacher and improver. I will speak to you in English and you will reply to me in English to practice my spoken English. I want you to keep your reply neat, limiting the reply to 100 words. I want you to strictly correct my grammar mistakes, typos, and factual errors. I want you to ask me a question in your reply. Now let's start practicing, you could ask me a question first. Remember, I want you to strictly correct my grammar mistakes, typos, and factual errors.
            Predicted: take come take not not . which well for . same but come not come not to this the for to not but not take this the same same to same not take come take not not same well so it same well so . but come come take not same . instead same not so not one so take come to not come come same it this to so it but come so take not come come same it so so come come take not . . instead same not so not one

            Act: Travel Guide
            Prompt Input: I want you to act as a travel guide. I will write you my location and you will suggest a place to visit near my location. In some cases, I will also give you the type of places I will visit. You will also suggest me places of similar type that are close to my first location. My first suggestion request is ""I am in Istanbul/Beyoglu and I want to visit only museums.
            Predicted: come come take not not . as it so come not but take to . that to not but same instead same it same to so this same so come not not not take same which this . would not so take not not . come it this same same to same not not to same but to same but so this but should this come same take come not it instead
            */


            #region hiddenSize 50
            //int hiddenSize = 50;
            //double learningRate = 0.00001;
            //int epochs = 100000;
            ////Epoch 65001 / 100000, Average Loss: 0,19226625264986613
            ////Epoch 65101 / 100000, Average Loss: 0,19225688498847587
            ////Epoch 65201 / 100000, Average Loss: 0,19224755135200408
            ////Epoch 65301 / 100000, Average Loss: 0,19223825164260278
            ////Epoch 82201 / 100000, Average Loss: 0,1910589435420859
            ////Epoch 82301 / 100000, Average Loss: 0,19105354029418167
            ////Epoch 82401 / 100000, Average Loss: 0,19104814369525253
            ////Epoch 99701 / 100000, Average Loss: 0,1900242303323076
            ////Epoch 99801 / 100000, Average Loss: 0,1900173727797374
            ////Epoch 99901 / 100000, Average Loss: 0,19001050959914123
            ////take you to not as one an reference . get same word but n't want simply come 
            #endregion

            #region hiddenSize 60
            //int hiddenSize = 60;
            //double learningRate = 0.00001;
            //int epochs = 2000;
            //Epoch 1801 / 2000, Average Loss: 0,23308940774564788
            //Epoch 1901 / 2000, Average Loss: 0,23246138716698042
            //but not take but same instead it same

            //int hiddenSize = 60;
            //double learningRate = 0.00001;
            //int epochs = 20000;
            ////Epoch 19801 / 20000, Average Loss: 0,20338795509935195
            ////Epoch 19901 / 20000, Average Loss: 0,20333099160106616
            //// of the this but to this also come same . which
            #endregion


            int hiddenSize = 150;
            double learningRate = 0.0001;
            int epochs = 1000;
            /*
             Epoch 0/1000, Average Loss: 0,42414431406015973
Epoch 100/1000, Average Loss: 0,23310524156452397
Epoch 200/1000, Average Loss: 0,22509451883908774
Epoch 300/1000, Average Loss: 0,22011606116171106
Epoch 400/1000, Average Loss: 0,21663756150156552
Epoch 500/1000, Average Loss: 0,21400751695214262
Epoch 600/1000, Average Loss: 0,21189952391391526
Epoch 700/1000, Average Loss: 0,2101367178697998
Epoch 800/1000, Average Loss: 0,20861793863623318
Epoch 900/1000, Average Loss: 0,20728192875391724
Epoch 1000/1000, Average Loss: 0,2060892048513238
             */

            //int hiddenSize = 150;
            //double learningRate = 0.00001;
            //int epochs = 1000;
            /*
            Epoch 0/1000, Average Loss: 0,48238385200158423
            Epoch 100/1000, Average Loss: 0,27551979547139227
            Epoch 200/1000, Average Loss: 0,2455880357393517
            Epoch 300/1000, Average Loss: 0,24024590507695437
            Epoch 400/1000, Average Loss: 0,23831934721659512
            Epoch 500/1000, Average Loss: 0,23689630723818791
            Epoch 600/1000, Average Loss: 0,23562476902872506
            Epoch 700/1000, Average Loss: 0,23445071363959152
            Epoch 800/1000, Average Loss: 0,23335815585368228
            Epoch 900/1000, Average Loss: 0,23233738930816186
            Epoch 1000/1000, Average Loss: 0,2313806313938535
            */



            ImprovedRNN rnn = new ImprovedRNN(inputSize: embeddingDim, hiddenSize: hiddenSize, outputSize: embeddingDim, learningRate: learningRate);

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
