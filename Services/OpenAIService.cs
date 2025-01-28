using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using Twilio.TwiML;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Messaging;
using CandidateScreeningAI.Interface;
using Twilio.TwiML.Voice;
using System.Text.Json.Nodes;
using GroqApiLibrary;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.AI;
using System.Collections.Generic;


namespace CandidateScreeningAI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IChatClient _chatClient;
        private readonly string _apiKey = "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";
        private readonly string _modelName = "gpt-3.5-turbo";
        private readonly ConversationManager _conversationManager;
        private readonly HttpClient _httpClient;
        private readonly IGroqService _groqService;
        private readonly List<Microsoft.Extensions.AI.ChatMessage> lstChatMessage = new();



        public OpenAIService(ConversationManager conversationManager, HttpClient httpClient, IGroqService groqService)
        {
            ;
            _chatClient = new OpenAIClient(_apiKey).AsChatClient(_modelName);
            _conversationManager = conversationManager;
            _httpClient = httpClient;
            _groqService = groqService;
        }

        // Generates the dynamic follow-up question
        public async Task<string> GetFollowUpQuestionAsync(string userResponse)
        {
            var sessionKey = "Santhosh";
            var resumeSummary = string.Empty;
            if (!_conversationManager._conversationState.ContainsKey(sessionKey))
            {
                resumeSummary = await GetResumeSummary();
            }

            _conversationManager.AddMessage(sessionKey, "user", userResponse, resumeSummary);
            var conversationMessage = _conversationManager.GetMessages(sessionKey);

            var assistantResponse = await GetOpenAPIResponse(conversationMessage);
            _conversationManager.AddMessage(sessionKey, "assistant", assistantResponse);
            return assistantResponse.Trim(); // Return the AI-generated follow-up question
        }

        public async Task<string> GetOpenAPIResponse(JsonArray conversationMessage, int tokenSize = 200)
        {
            var chatMessage = new Microsoft.Extensions.AI.ChatMessage(){ Role = ChatRole.System, Text = "You are a helpful assistant." };
            var url = "https://api.openai.com/v1/chat/completions";

            var requestPayload = new JsonObject
            {
                ["model"] = "llama-3.2-90b-vision-preview",
                ["temperature"] = 0.7,
                ["max_completion_tokens"] = tokenSize,
                ["top_p"] = 0.5,
                ["messages"] = conversationMessage
            };

           // var jsonRequest = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(requestPayload.ToString(), Encoding.UTF8, "application/json");

            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            //var response = await _httpClient.PostAsync(url, content);

            //if (!response.IsSuccessStatusCode)
            //    throw new HttpRequestException($"OpenAI API error: {response.ReasonPhrase}");

            //var jsonResponse = await response.Content.ReadAsStringAsync();
            //dynamic result = JsonConvert.DeserializeObject(jsonResponse);

                    var chatRequest = new ChatOptions()
                    {
                        ModelId = "gpt-4o-mini",
                        TopP = (float?)0.5,
                        Temperature = (float?)0.7, // Adjust for response randomness
                        MaxOutputTokens = 150, // Control the length of the response
                    };

            lstChatMessage.Add(chatMessage);
            var result = await _chatClient.CompleteAsync(lstChatMessage, chatRequest);

            return result?.Choices[0]?.Text ?? "No response";
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
            var data = await _groqService.GroqResponse(prompt, 4096);
            return data;
        }
    }
}
