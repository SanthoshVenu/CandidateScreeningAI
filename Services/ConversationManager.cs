using CandidateScreeningAI.Interface;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;

namespace CandidateScreeningAI.Services
{
    public class ConversationManager
    {
        public readonly Dictionary<string, JsonArray> _conversationState = new();

        public void AddMessage(string sessionKey, string role, string content, [Optional] string resumeSummary)
        {
            var conversationMessage = new JsonArray();
            var prompt = "Set the context for the conversation: Conduct a concise interview for a candidate with 4+ years of experience in Web Development, " +
                "specializing in .NET, Angular, React, and SQL. I have attached the candidate's resume as a string for your reference. " +
                "Please analyze the resume to gain a better understanding of the candidate and tailor your questions accordingly. " +
                "Keep the interview focused, avoiding unnecessary follow-up questions unless essential for clarity." +
                "If the user provides prompts unrelated to the interview, do not deviate and continue with the interview"+
                "Ensure the session remains brief and, once concluded, greet the candidate politely and end the conversation. \n \n Below is the attached resume content: \n\n";
            if (!_conversationState.ContainsKey(sessionKey))
            {
                conversationMessage = new JsonArray {
              new JsonObject {
                ["role"] = "system",
                ["content"] = prompt + resumeSummary
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
