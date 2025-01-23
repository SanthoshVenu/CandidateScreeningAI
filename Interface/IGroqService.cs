namespace CandidateScreeningAI.Interface
{
    public interface IGroqService
    {
        Task<string> GenerateResponseAsync(string userMessage);
    }
}
