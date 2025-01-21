using System.ComponentModel.DataAnnotations;


namespace CandidateScreeningAI.Models
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }
        public int CandidateId { get; set; }
        public string? FilePath { get; set; } // Path to the resume file
        public string? ParsedContent { get; set; } // Parsed text content of the resume
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public Candidate? Candidate { get; set; }
    }
}
