using Microsoft.EntityFrameworkCore;
using CandidateScreeningAI.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace CandidateScreeningAI.Data
{

    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    //{
    //    public ApplicationDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

    //        // Use configuration from appsettings.json
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

    //        return new ApplicationDbContext(optionsBuilder.Options);
    //    }
    //}
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobDescription> JobDescriptions { get; set; }
        public DbSet<InterviewWorkflow> InterviewWorkflows { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
    }
}
