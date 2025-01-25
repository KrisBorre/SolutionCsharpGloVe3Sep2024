namespace ConsoleCodeAnalysis8Nov2024
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var apiClient = new ApiClient("http://localhost:8080", 4096);



            MetadataReferenceManager8Nov2024 _metadataManager = new MetadataReferenceManager8Nov2024();
            CodeCompiler _compiler = new CodeCompiler(_metadataManager);
            CodeExecutor _executor = new CodeExecutor();

            var project7 = new Project9Oct2024 { Name = "IFormattable" };

            project7.Prompts.Add(@"Write a C# console application that demonstrates the IFormattable interface for custom string formatting.

Write a Person class to implement IFormattable.
Override the ToString method to provide custom formats for displaying Person information (e.g., Name only, Age only, or both).
Write a Main method to demonstrate formatting Person objects using different formats.");




            var apiClientHandler = new ApiClientHandler(apiClient);
            var projectPromptHandler = new ProjectPromptHandler8Nov2024(apiClientHandler);



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