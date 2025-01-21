using Microsoft.Extensions.Configuration;
using OpenAI;
using Twilio.TwiML;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML.Messaging;
using Microsoft.Extensions.AI;
using CandidateScreeningAI.Interface;

namespace CandidateScreeningAI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IChatClient _chatClient;
        private readonly string _apiKey = "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";
        private readonly string _modelName = "gpt-3.5-turbo";

        public OpenAIService()
        {;
            _chatClient = new OpenAIClient(_apiKey).AsChatClient(_modelName);
        }

        // Generates the dynamic follow-up question
        public async Task<string> GetFollowUpQuestionAsync(string userResponse)
        {
            // Construct the prompt for OpenAI
            var prompt = @$"
            Based on the following response, generate a professional follow-up question related to software development skills:

            User Response: {userResponse}

            Follow-up Question:
            ";

            // Chat history including system instructions
            List<ChatMessage> chatHistory = new()
            {
                new ChatMessage(ChatRole.System, "You are an interviewer asking questions related to software development skills.")
            };

            // Add user response to chat history
            chatHistory.Add(new ChatMessage(ChatRole.User, prompt));

            // Collect AI's response
            string response = string.Empty;
            //await foreach (var item in _chatClient.CompleteStreamingAsync(chatHistory))
            //{
            //    response += item.Text;
            //}

            // Add the assistant's response to the chat history (for future context if needed)
            //chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

            response = "You have done very well Santhosh";
            return response.Trim(); // Return the AI-generated follow-up question
        }
    }
}
