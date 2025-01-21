namespace CandidateScreeningAI.Interface
{
    public interface IOpenAIService
    {
        Task<string> GetFollowUpQuestionAsync(string candidateResponse);
    }
}
