namespace CandidateScreeningAI.Interface
{
    public interface IOllamaService
    {
        Task<string> GenerateResponseAsync(string prompt);
    }
}
