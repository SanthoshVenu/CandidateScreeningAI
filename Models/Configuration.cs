using System.ComponentModel.DataAnnotations;

namespace CandidateScreeningAI.Models
{
    public class Configuration
    {
        [Key]
        public string? ConfigKey { get; set; } // Unique key for the configuration setting
        public string? ConfigValue { get; set; } // Value of the configuration setting
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
