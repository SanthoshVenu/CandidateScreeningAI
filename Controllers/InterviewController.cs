using CandidateScreeningAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateScreeningAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewWorkflowService _interviewWorkflowService;

        public InterviewController(IInterviewWorkflowService interviewWorkflowService)
        {
            _interviewWorkflowService = interviewWorkflowService;
        }

        [HttpPost("{candidateId}")]
        public async Task<IActionResult> ConductInterview(int candidateId)
        {
            try
            {
                var result = await _interviewWorkflowService.ConductInterviewAsync(candidateId);
                return Ok("Interview conducted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
