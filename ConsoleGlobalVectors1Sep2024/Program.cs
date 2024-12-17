namespace ConsoleGlobalVectors1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string gloveFilePath = "../../../../../../../GloVe/glove.6B.50d.txt";

            // Sample words in different languages to check for embeddings
            var wordsToCheck = new Dictionary<string, List<string>>
            {
                { "Dutch", new List<string> { "hallo", "wereld", "vader", "moeder", "fiets" } },
                { "German", new List<string> { "hallo", "welt", "vater", "mutter", "fahrrad" } },
                { "Italian", new List<string> { "ciao", "mondo", "padre", "madre", "bicicletta" } },
                { "French", new List<string> { "bonjour", "monde", "père", "mère", "vélo" } },
                { "Spanish", new List<string> { "hola", "mundo", "padre", "madre", "bicicleta" } },
                { "English", new List<string> { "hello", "world", "father", "mother", "bicycle" } }
            };

            if (!File.Exists(gloveFilePath))
            {
                Console.WriteLine($"File not found: {gloveFilePath}");
                return;
            }

            Console.WriteLine("Reading GloVe file...");
            var embeddingsFound = new Dictionary<string, HashSet<string>>();

            // Initialize results storage
            foreach (var language in wordsToCheck.Keys)
            {
                embeddingsFound[language] = new HashSet<string>();
            }

            // Read and check each line of the GloVe file
            using (var reader = new StreamReader(gloveFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Extract the word (first token before the embedding values)
                    var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 0) continue;

                    string word = tokens[0];

                    // Check if the word is in the lists
                    foreach (var language in wordsToCheck)
                    {
                        if (language.Value.Contains(word))
                        {
                            embeddingsFound[language.Key].Add(word);
                        }
                    }
                }
            }

            // Display the results
            foreach (var language in embeddingsFound)
            {
                Console.WriteLine($"\n{language.Key} words found:");
                foreach (var word in language.Value)
                {
                    Console.WriteLine($" - {word}");
                }

                if (language.Value.Count == 0)
                {
                    Console.WriteLine(" - None found");
                }
            }

            Console.WriteLine("\nCheck complete.");

            /*
            Reading GloVe file...

            Dutch words found:
             - vader
             - hallo
             - wereld

            German words found:
             - welt
             - mutter
             - vater
             - hallo

            Italian words found:
             - padre
             - madre
             - mondo
             - ciao

            French words found:
             - monde
             - père
             - bonjour
             - mère
             - vélo

            Spanish words found:
             - mundo
             - padre
             - madre
             - hola
             - bicicleta

            English words found:
             - world
             - father
             - mother
             - bicycle
             - hello

            Check complete.
            */

            /*
            Reading GloVe file...

            Dutch words found:
             - vader
             - hallo
             - wereld

            German words found:
             - welt
             - mutter
             - vater
             - hallo

            Italian words found:
             - padre
             - madre
             - mondo
             - ciao

            French words found:
             - monde
             - père
             - bonjour
             - mère
             - vélo

            Spanish words found:
             - mundo
             - padre
             - madre
             - hola
             - bicicleta

            English words found:
             - world
             - father
             - mother
             - bicycle
             - hello

            Check complete. 
            */

            Console.ReadLine();
        }
    }

}
