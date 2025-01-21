using CandidateScreeningAI.Data;
using CandidateScreeningAI.Interface;
using CandidateScreeningAI.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateScreeningAI.Services
{
    public class ResumeShortlistService : IResumeShortlistService
    {
        private readonly ApplicationDbContext _context;

        public ResumeShortlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Candidate>> ShortlistResumes(JobDescription jobDescription)
        {
            // Query candidates based on matching skills and minimum years of experience
            var shortlistedCandidates = await _context.Candidates
                .Where(c => c.YearsOfExperience >= jobDescription.MinExperience &&
                            c.Skills.Contains(jobDescription.SkillsRequired))
                .ToListAsync();

            return shortlistedCandidates;
        }
    }
}
