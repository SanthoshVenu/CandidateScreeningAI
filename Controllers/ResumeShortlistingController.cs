using CandidateScreeningAI.Data;
using CandidateScreeningAI.Interface;
using CandidateScreeningAI.Models;
using CandidateScreeningAI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CandidateScreeningAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeShortlistingController : ControllerBase
    {
        private readonly IResumeShortlistService _resumeShortlistService;
        private readonly ApplicationDbContext _context;


        public ResumeShortlistingController(IResumeShortlistService resumeShortlistService, ApplicationDbContext context)
        {
            _resumeShortlistService = resumeShortlistService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _context.JobDescriptions.ToListAsync();
            return Ok(candidates);
        }

        // POST api/resumeshortlisting
        [HttpPost]
        public async Task<IActionResult> ShortlistResumes([FromBody] JobDescription jobDescription)
        {
            if (jobDescription == null)
            {
                return BadRequest("Job description cannot be null.");
            }

            var shortlistedCandidates = await _resumeShortlistService.ShortlistResumes(jobDescription);
            return Ok(shortlistedCandidates);
        }
    }
}
