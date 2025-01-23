using System.Text.Json.Nodes;

namespace CandidateScreeningAI.Services
{
    public class ConversationManager
    {
        private readonly Dictionary<string, JsonArray> _conversationState = new();

        public void AddMessage(string sessionKey, string role, string content)
        {
            var conversationMessage = new JsonArray();
            if (!_conversationState.ContainsKey(sessionKey))
            {
                conversationMessage = new JsonArray {
              new JsonObject {
                ["role"] = "system",
                ["content"] = "You are a friendly and professional assistant conducting an interview for an Software Professional. Respond concisely, acknowledge the user's input, and ask follow-up questions where relevant"
              },
              new JsonObject {
                ["role"] = "user",
                ["content"] = content
              }
            };
            }
            else
            {
                conversationMessage = new JsonArray {
              new JsonObject {
                ["role"] = role,
                ["content"] = content
              }
            };
            }
            _conversationState[sessionKey] = conversationMessage;
        }

        public JsonArray GetMessages(string sessionKey)
        {
            return _conversationState.ContainsKey(sessionKey) ? _conversationState[sessionKey] : new JsonArray();
        }

        public void ClearState(string sessionKey)
        {
            if (_conversationState.ContainsKey(sessionKey))
            {
                _conversationState.Remove(sessionKey);
            }
        }
    }
}
