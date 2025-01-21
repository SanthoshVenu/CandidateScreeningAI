using System.ComponentModel.DataAnnotations;


namespace CandidateScreeningAI.Models
{
    public class InterviewWorkflow
    {
        [Key]
        public int InterviewId { get; set; }
        public int CandidateId { get; set; }
        public int JobId { get; set; }
        public string InterviewStatus { get; set; } = "Scheduled"; // Status of the interview
        public string? Decision { get; set; } // Interview decision (e.g., Pass, Fail, Pending)
        public DateTime? ScheduledAt { get; set; } // Date and time of the interview
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Candidate? Candidate { get; set; }
        public JobDescription? JobDescription { get; set; }
    }
}
