using CandidateScreeningAI.Data;

namespace CandidateScreeningAI.Services
{
    public class InterviewWorkflowService : IInterviewWorkflowService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITelephonyService _telephonyService;
        private readonly IGoogleTTSService _googleTTSService;
        private readonly ISpeechToTextService _speechToTextService;


        public InterviewWorkflowService(
            ApplicationDbContext context,
            ITelephonyService telephonyService,
            IGoogleTTSService googleTTSService,
            ISpeechToTextService speechToTextService)
        {
            _context = context;
            _telephonyService = telephonyService;
            _googleTTSService = googleTTSService;
            _speechToTextService = speechToTextService;
        }


        public async Task<string> ConductInterviewAsync(int candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null) throw new Exception("Candidate not found");

            var questions = new List<string>
            {
                "Introduce yourself and tell about your total years of experience in Software Engineering",
            };
            await _telephonyService.MakeInteractiveCallAsync(candidate.PhoneNumber, questions);
            return $"STT Response: {questions}";
            //foreach (var question in questions)
            //{
            //    // Convert question to speech
            //   // var audioContent = await _googleTTSService.ConvertTextToSpeechAsync(question);
            //   // var audioFilePath = Path.Combine("Desktop");
            //   // Directory.CreateDirectory("Desktop");
            //   //// await File.WriteAllBytesAsync(audioFilePath, audioContent);
            //   // Console.WriteLine($"TTS Audio saved: {audioFilePath}");
            //    // Use telephony service to play the audio to the candidate
            //    //var filePath = @"C:\Users\SANTHOSH VENUGOPAL\source\repos\CandidateScreeningAI\wavfiles\harvard.wav";
            //    //var responseText = await _speechToTextService.ConvertSpeechToTextAsync(filePath);
            //    //await _telephonyService.MakeCallAsync(candidate.PhoneNumber, question);
            //    //return $"STT Response: {question}";
            //}
           // return string.Empty;
        }

        //public async Task ConductInterviewAsync(int candidateId)
        //{
        //    var candidate = await _context.Candidates.FindAsync(candidateId);
        //    if (candidate == null)
        //    {
        //        throw new Exception("Candidate not found");
        //    }

        //    var questions = new List<string>
        //    {
        //        "What is your expected salary?",
        //        "Are you willing to relocate?",
        //        "What are your strongest technical skills?"
        //    };

        //    foreach (var question in questions)
        //    {
        //        // Convert text to speech
        //        var audioContent = await _googleTTSService.ConvertTextToSpeechAsync(question);
        //        var audioFilePath = Path.Combine("output", $"{Guid.NewGuid()}.mp3");
        //        await File.WriteAllBytesAsync(audioFilePath, audioContent);

        //        // Use telephony service to play the audio to the candidate
        //        await _telephonyService.MakeCallAsync(candidate.PhoneNumber, $"Playing audio from {audioFilePath}");

        //        // Example: Convert response audio to text (assume response is saved as 'response_audio.wav')
        //        var responseText = await _speechToTextService.ConvertSpeechToTextAsync("response_audio.wav");
        //        Console.WriteLine($"Candidate Response: {responseText}");
        //    }
        //}
    }
}