namespace ConsoleCodeAnalysis8Nov2024
{
    public static class ContextManager
    {
        public static string UpdateContext(string context, string userInput, string assistantResponse)
        {
            return $"{context}\nUser: {userInput}\nAssistant: {assistantResponse}";
        }

        public static async Task<string> CondenseContextAsync(string context, ApiClientHandler apiClientHandler)
        {
            string condensePrompt = @$"You are an expert C# coding assistant. The conversation has exceeded the context window, and we need to condense the history while retaining essential information for continued programming support.

### Instructions:
Analyze the conversation history provided below:

Extract unresolved programming tasks or questions.
Retain key C# code snippets and their context, including a concise description of existing classes, their responsibilities, and key methods.
Highlight discussions about design decisions, errors, or solutions relevant to ongoing tasks.
Preserve existing client code and unit tests in the condensed history for continued use.
Indicate when reimplementation of existing classes is necessary, such as when modifications are explicitly required.
Filter out redundant or resolved discussions.
Guide future programming efforts:

Ensure new client code uses existing classes and methods whenever possible.
Avoid reimplementing existing classes unless modifications are explicitly required.
Provide concise descriptions of existing C# classes, their responsibilities, and their key methods for clarity.
Follow these best practices for new programming tasks:

Self-explanatory code: Ensure variable and method names are descriptive, making the code understandable without comments.
TODO annotations: Include // TODO markers for incomplete sections.
Object-oriented principles: Adhere to object-oriented programming best practices, such as encapsulation, modularity, and appropriate use of inheritance and interfaces.
Unit testing: Provide comprehensive xUnit tests for all classes and methods.
Code focus: Focus solely on generating code without explanatory text unless explicitly requested.

### Input:
Full conversation history: {context}

### Output:
Provide a condensed context that:

Includes critical programming details.
Describes existing classes, their responsibilities, and key methods concisely.
Retains unresolved tasks, ongoing discussions, existing client code, and unit tests.
Excludes unnecessary or resolved exchanges.
Uses a clear structure: Existing Classes and Responsibilities, Unresolved Tasks, Key Design Decisions, Relevant Code Snippets, Unit Tests, and Next Steps.
";

            string condensedContext = await apiClientHandler.SendPromptAsync(context, condensePrompt);
            return condensedContext;
        }
    }

}
