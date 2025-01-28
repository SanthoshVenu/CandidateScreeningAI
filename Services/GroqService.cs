using CandidateScreeningAI.Interface;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using GroqApiLibrary;
using System.Text.Json.Nodes;
using Twilio.Jwt.AccessToken;

namespace CandidateScreeningAI.Services
{
    public class GroqService : IGroqService
    {
        private readonly ConversationManager _conversationManager;

        public GroqService(ConversationManager conversationManager)
        {
            _conversationManager = conversationManager;
        }

        public async Task<string> GenerateResponseAsync(string userMessage)
        {
            try
            {
                var sessionKey = "Santhosh";
                var resumeSummary = string.Empty;
                if (!_conversationManager._conversationState.ContainsKey(sessionKey))
                {
                    resumeSummary = await GetResumeSummary();
                }

                _conversationManager.AddMessage(sessionKey, "user", userMessage, resumeSummary);
                var conversationMessage = _conversationManager.GetMessages(sessionKey);

                // Add the assistant's response to the conversation state
                var assistantResponse = await GroqResponse(conversationMessage);
                _conversationManager.AddMessage(sessionKey, "assistant", assistantResponse);
                Console.WriteLine(assistantResponse);
                return assistantResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> GroqResponse( JsonArray conversationMessage, int tokenSize = 80)
        {
            var apiKey = "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";
            var groqApi = new GroqApiClient(apiKey);

            var requestPayload = new JsonObject
            {
                ["model"] = "llama-3.2-90b-vision-preview",
                ["temperature"] = 0.7,
                ["max_completion_tokens"] = tokenSize,
                ["top_p"] = 0.5,
                ["messages"] = conversationMessage
            };

            var groqResponse = await groqApi.CreateChatCompletionAsync(requestPayload);
            return groqResponse?["choices"]?[0]?["message"]?["content"]?.ToString() ?? "No response from Groq.";
        }

        public async Task<string> GetResumeSummary()
        {
            var pdfExtract = new PdfExtractor().ExtractText("");
            var prompt = new JsonArray {
              new JsonObject {
                ["role"] = "user",
                ["content"] = "Please summarise this resume and provide me the key points such that interviwer can get to know about their expertise \n \n \n" + pdfExtract
              }
            };
            var data = await GroqResponse(prompt, 4096);
            return data;
        }
    }
}
