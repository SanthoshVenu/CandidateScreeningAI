using Microsoft.Extensions.AI;

namespace CandidateScreeningAI.Helper
{
    public class ChatMessageSingleton
    {
        // The single instance of the class
        private static readonly Lazy<ChatMessageSingleton> _instance =
            new Lazy<ChatMessageSingleton>(() => new ChatMessageSingleton());

        // The underlying list
        private readonly List<ChatMessage> _chatMessages;

        // Private constructor to prevent instantiation
        private ChatMessageSingleton()
        {
            _chatMessages = new List<ChatMessage>();
        }

        // Public static property to access the singleton instance
        public static ChatMessageSingleton Instance => _instance.Value;

        // Method to add a message to the list
        public void AddMessage(ChatMessage message)
        {
            _chatMessages.Add(message);
        }

        // Method to retrieve all messages
        public List<ChatMessage> GetMessages()
        {
            return _chatMessages;
        }

        // Method to clear the list
        public void ClearMessages()
        {
            _chatMessages.Clear();
        }

        // Method to get the count of messages
        public int MessageCount => _chatMessages.Count;
    }
}
