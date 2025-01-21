namespace CandidateScreeningAI.Services
{
    public interface ISpeechToTextService
    {
        Task<string> ConvertSpeechToTextAsync(string audioFilePath);
    }
}
