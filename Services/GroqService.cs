using CandidateScreeningAI.Interface;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using GroqApiLibrary;
using System.Text.Json.Nodes;

namespace CandidateScreeningAI.Services
{
    public class GroqService : IGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly ConversationManager _conversationManager;

        public GroqService(HttpClient httpClient, ConversationManager conversationManager)
        {
            _httpClient = httpClient;
            _conversationManager = conversationManager;
        }

        public async Task<string> GenerateResponseAsync(string userMessage)
        {
            try
            {
                var apiKey = "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"; 
                var groqApi = new GroqApiClient(apiKey);

                var sessionKey = "Santhosh";
                _conversationManager.AddMessage(sessionKey, "user", userMessage);
                var conversationMessage = _conversationManager.GetMessages(sessionKey);

                var requestPayload = new JsonObject
                {
                    ["model"] = "mixtral-8x7b-32768",
                    ["temperature"] = 0.7,
                    ["max_completion_tokens"] = 80,
                    ["top_p"] = 0.5,
                    ["messages"] = conversationMessage
                };

                var groqResponse = await groqApi.CreateChatCompletionAsync(requestPayload);
                var assistantMessage = groqResponse?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "No response from Groq.";

                // Add the assistant's response to the conversation state
                _conversationManager.AddMessage(sessionKey, "assistant", assistantMessage);
                Console.WriteLine(assistantMessage);
                return assistantMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
    }
}
