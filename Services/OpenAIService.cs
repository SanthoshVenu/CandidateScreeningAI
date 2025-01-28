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
using CandidateScreeningAI.Helper;


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
        private readonly ChatMessageSingleton chatMessageList = ChatMessageSingleton.Instance;



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
                var prompt = @"Conduct a structured and concise technical interview for a candidate with 4+ years of experience in Web Development. 
The candidate specializes in .NET Core, Angular, React, and SQL. A summary of the candidate's resume is provided below for reference.

Your objective is to:  
1. Analyze the resume summary to understand the candidate's expertise.  
2. Ask precise and relevant technical questions one at a time, focusing on .NET Core, React and SQL.  
3. Avoid asking all questions at once. Instead, respond to the candidate’s answer before proceeding to the next question.  
4. Keep the interview concise by avoiding unnecessary or overly broad follow-up questions unless required for clarification. 
5. Dont repeat the question more than once.
6. Your first message should always starts with. Thanks for waiting patiently, lets start our interview and then only you need to ask your first question.

Important Guidelines:  
- If the candidate provides prompts unrelated to the interview, stay focused and redirect the conversation to the interview.  
- Once the interview concludes, politely thank the candidate and end the session.

Resume Summary:  
" + resumeSummary;
                chatMessageList.AddMessage(new Microsoft.Extensions.AI.ChatMessage() { Role = ChatRole.System, Text = prompt });

            }

            _conversationManager.AddMessage(sessionKey, "user", userResponse, resumeSummary);
            //var conversationMessage = _conversationManager.GetMessages(sessionKey);
            chatMessageList.AddMessage(new Microsoft.Extensions.AI.ChatMessage() { Role = ChatRole.User, Text = userResponse });

            var assistantResponse = await GetOpenAPIResponse(chatMessageList.GetMessages());
            chatMessageList.AddMessage(new Microsoft.Extensions.AI.ChatMessage() { Role = ChatRole.Assistant, Text = assistantResponse });
            return assistantResponse.Trim(); // Return the AI-generated follow-up question
        }

        public async Task<string> GetOpenAPIResponse(List<Microsoft.Extensions.AI.ChatMessage> chatMessageList, int tokenSize = 200)
        {

            var chatRequest = new ChatOptions()
            {
                ModelId = "gpt-4o-mini",
                TopP = (float?)0.5,
                Temperature = (float?)0.7, // Adjust for response randomness
                MaxOutputTokens = tokenSize, // Control the length of the response
            };

            var result = await _chatClient.CompleteAsync(chatMessageList, chatRequest);

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
