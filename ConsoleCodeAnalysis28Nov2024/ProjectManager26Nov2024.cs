namespace ConsoleCodeAnalysis28Nov2024
{
    public class ProjectManager26Nov2024
    {       
        public Conversation CreateNewConversation(Project9Oct2024 project)
        {
            var conversation = new Conversation
            {
                Number = project.Conversations.Count + 1
            };
            project.Conversations.Add(conversation);
            return conversation;
        }

        public SourceCodeIteration26Nov2024 CreateNewIteration(Conversation conversation)
        {
            var iteration = new SourceCodeIteration26Nov2024
            {
                Number = conversation.SourceCodeIterations.Count + 1
            };
            conversation.SourceCodeIterations.Add(iteration);
            return iteration;
        }
    }
}
