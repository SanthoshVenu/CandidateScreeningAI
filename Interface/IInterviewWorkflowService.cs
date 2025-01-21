namespace CandidateScreeningAI.Services
{
    public interface IInterviewWorkflowService
    {
        Task<string> ConductInterviewAsync(int candidateId);
    }
}
