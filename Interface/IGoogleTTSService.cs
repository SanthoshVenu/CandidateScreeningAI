namespace CandidateScreeningAI.Services
{
    public interface IGoogleTTSService
    {
        Task<byte[]> ConvertTextToSpeechAsync(string text, string language = "en-US");
    }
}
