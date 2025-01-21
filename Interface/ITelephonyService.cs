namespace CandidateScreeningAI.Services
{
    public interface ITelephonyService
    {
        Task MakeInteractiveCallAsync(string phoneNumber, List<string> questions);
    }
}
