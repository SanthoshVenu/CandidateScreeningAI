using System.ComponentModel.DataAnnotations;

namespace CandidateScreeningAI.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal YearsOfExperience { get; set; }
        public string? Skills { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
