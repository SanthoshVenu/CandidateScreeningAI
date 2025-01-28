using System.Text.Json.Nodes;

namespace CandidateScreeningAI.Interface
{
    public interface IGroqService
    {
        Task<string> GenerateResponseAsync(string userMessage);
        Task<string> GroqResponse(JsonArray conversationMessage, int tokenSize = 80);
    }
}
