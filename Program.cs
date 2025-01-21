using CandidateScreeningAI.Data;
using CandidateScreeningAI.Interface;
using CandidateScreeningAI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IResumeShortlistService, ResumeShortlistService>();
builder.Services.AddScoped<ITelephonyService, TwilioTelephonyService>();
builder.Services.AddScoped<IInterviewWorkflowService, InterviewWorkflowService>();
builder.Services.AddScoped<IGoogleTTSService, GoogleTTSService>();
builder.Services.AddScoped<ISpeechToTextService, GoogleSpeechToTextService>();
builder.Services.AddScoped<INaturalLanguageProcessor, NaturalLanguageProcessor>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the root
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5000");