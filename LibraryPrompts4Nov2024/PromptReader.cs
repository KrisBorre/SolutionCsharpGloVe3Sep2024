namespace LibraryPrompts4Nov2024
{
    public class PromptReader
    {
        public PromptReader()
        {
            
        }

        public List<Prompt> ReadPromptsFromCsv(string filePath)
        {
            var prompts = new List<Prompt>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return prompts;
            }

            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false; // Skip the header row
                        continue;
                    }

                    var values = line.Split(new[] { ',' }, 2); // Split only on the first comma
                    if (values.Length == 2)
                    {
                        prompts.Add(new Prompt
                        {
                            Act = values[0].Trim('"'),
                            PromptText = values[1].Trim('"')
                        });
                    }
                }
            }
            return prompts;
        }
    }
}
