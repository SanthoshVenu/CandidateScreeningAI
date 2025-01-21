using CandidateScreeningAI.Models;

namespace CandidateScreeningAI.Interface
{
    public interface IResumeShortlistService
    {
        Task<List<Candidate>> ShortlistResumes(JobDescription jobDescription);
    }
}
