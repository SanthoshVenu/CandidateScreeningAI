using System.ComponentModel.DataAnnotations;


namespace CandidateScreeningAI.Models
{
    public class JobDescription
    {
        [Key]
        public int JobId { get; set; }
        public string? JobTitle { get; set; } // Job title (e.g., Data Scientist)
        public string? SkillsRequired { get; set; } // Skills required for the job
        public decimal MinExperience { get; set; } // Minimum years of experience
        public string? LocationPreference { get; set; } // Preferred location
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public ICollection<InterviewWorkflow>? InterviewWorkflows { get; set; }
    }
}
