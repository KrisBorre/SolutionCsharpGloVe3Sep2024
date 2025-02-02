﻿namespace ConsoleCodeAnalysis10Nov2024
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

            {
                #region Linq

                //                var project7 = new Project9Oct2024 { Name = "Linq" };


                //                Conversation conversation1 = CreateNewConversation(project7);
                //                SourceCodeIteration sourceCodeIteration1 = projectPromptHandler.CreateNewIteration(conversation1);

                //                sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"using System;
                //using System.Linq;

                //class Example
                //{
                //    static void Main(string[] args)
                //    {
                //        Func<int, int> square = x => x * x;
                //        var numbers = new[] { 1, 2, 3 };
                //        var doubled = numbers.Select(n => n * 2).ToList();
                //        numbers.ToList().ForEach(n => Console.WriteLine(n));
                //    }
                //}"));

                //                foreach (var sourceCode in sourceCodeIteration1.SourceCodes)
                //                {
                //                    string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                //                    CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                //                    sourceCode.IsCompiled = true;
                //                    sourceCode.CompilationSuccess = compilationResult.Success;

                //                    if (compilationResult.Success)
                //                    {
                //                        ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                //                        sourceCode.ExecutionOutput = executionResult.Output;
                //                    }
                //                    else
                //                    {
                //                        sourceCode.CompilationErrors = compilationResult.Diagnostics;
                //                    }
                //                }


                //                Print(project7);

                //                /*

                //                */

                #endregion
            }


            {
                #region Linq

                //                var project7 = new Project9Oct2024 { Name = "Linq" };


                //                Conversation conversation1 = CreateNewConversation(project7);
                //                SourceCodeIteration sourceCodeIteration1 = projectPromptHandler.CreateNewIteration(conversation1);

                //                sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"using System;
                //using System.Linq;
                //using System.Collections;
                //using System.Collections.Generic;

                //class Example
                //{
                //    static void Main(string[] args)
                //    {
                //        Func<int, int> square = x => x * x;
                //        var numbers = new[] { 1, 2, 3 };
                //        var doubled = numbers.Select(n => n * 2).ToList();
                //        numbers.ToList().ForEach(n => Console.WriteLine(n));
                //    }
                //}"));

                //                foreach (var sourceCode in sourceCodeIteration1.SourceCodes)
                //                {
                //                    string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                //                    CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                //                    sourceCode.IsCompiled = true;
                //                    sourceCode.CompilationSuccess = compilationResult.Success;

                //                    if (compilationResult.Success)
                //                    {
                //                        ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                //                        sourceCode.ExecutionOutput = executionResult.Output;
                //                    }
                //                    else
                //                    {
                //                        sourceCode.CompilationErrors = compilationResult.Diagnostics;
                //                    }
                //                }


                //                Print(project7);

                //                /*

                //                */

                #endregion
            }


            {
                #region Linq

                //                var project7 = new Project9Oct2024 { Name = "Linq" };


                //                Conversation conversation1 = CreateNewConversation(project7);
                //                SourceCodeIteration sourceCodeIteration1 = projectPromptHandler.CreateNewIteration(conversation1);

                //                sourceCodeIteration1.SourceCodes.Add(new SourceCode8Nov2024(@"using System;
                //using System.Linq;
                //using System.Collections;
                //using System.Collections.Generic;

                //class Example
                //{
                //    static void Main(string[] args)
                //    {
                //        Func<int, int> square = x => x * x;
                //        var numbers = new[] { 1, 2, 3 };
                //        var doubled = numbers.Select(n => n * 2).ToList();
                //        numbers.ToList().ForEach(n => Console.WriteLine(n));
                //    }
                //}"));

                //                foreach (var sourceCode in sourceCodeIteration1.SourceCodes)
                //                {
                //                    string modifiedCode = projectPromptHandler.RemoveConsoleReadKey(sourceCode.Code);
                //                    CompilationResult compilationResult = _compiler.Compile(modifiedCode);

                //                    sourceCode.IsCompiled = true;
                //                    sourceCode.CompilationSuccess = compilationResult.Success;

                //                    if (compilationResult.Success)
                //                    {
                //                        ExecutionResult executionResult = _executor.Execute(compilationResult.Assembly);
                //                        sourceCode.ExecutionOutput = executionResult.Output;
                //                    }
                //                    else
                //                    {
                //                        sourceCode.CompilationErrors = compilationResult.Diagnostics;
                //                    }
                //                }


                //                Print(project7);

                //                /*
                //                Project: Linq
                //conversation #1
                //iteration #1
                //Code: using System;
                //using System.Linq;
                //using System.Collections;
                //using System.Collections.Generic;

                //class Example
                //{
                //    static void Main(string[] args)
                //    {
                //        Func<int, int> square = x => x * x;
                //        var numbers = new[] { 1, 2, 3 };
                //        var doubled = numbers.Select(n => n * 2).ToList();
                //        numbers.ToList().ForEach(n => Console.WriteLine(n));
                //    }
                //}
                //Compilation Errors: (12,23): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
                //(13,9): error CS0012: The type 'List<>' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
                //                */

                #endregion
            }


            {
                #region Linq

                var project7 = new Project9Oct2024 { Name = "Linq" };


                Conversation conversation1 = CreateNewConversation(project7);
                SourceCodeIteration sourceCodeIteration1 = projectPromptHandler.CreateNewIteration(conversation1);

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


                Print(project7);

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

                #endregion
            }


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