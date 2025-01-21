using CandidateScreeningAI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CandidateScreeningAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SttTestController : ControllerBase
    {
        private readonly ISpeechToTextService _googleSpeechToTextService;

        public SttTestController(ISpeechToTextService googleSpeechToTextService)
        {
            _googleSpeechToTextService = googleSpeechToTextService;
        }

        [HttpPost("process-response")]
        public async Task<IActionResult> ProcessResponse([FromForm] string RecordingUrl)
        {
            if (string.IsNullOrWhiteSpace(RecordingUrl))
            {
                return BadRequest("No audio file received.");
            }

            // Download the audio file
            var audioFilePath = Path.Combine("input", "user_response.wav");
            Directory.CreateDirectory("input");
            using (var httpClient = new HttpClient())
            {
                var audioData = await httpClient.GetByteArrayAsync(RecordingUrl);
                await System.IO.File.WriteAllBytesAsync(audioFilePath, audioData);
            }

            // Convert audio to text using Google Speech-to-Text
            var transcribedText = _googleSpeechToTextService.ConvertSpeechToTextAsync(audioFilePath);

            // Process the transcription as needed
            Console.WriteLine($"User Response Transcription: {transcribedText}");

            // Respond with a follow-up TwiML or finalize the call
            return Content(@"<?xml version='1.0' encoding='UTF-8'?>
                <Response>
                    <Say>Thank you for your response. Goodbye!</Say>
                    <Hangup />
                </Response>", "application/xml");
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertSpeechToText(string filePath)
        {
            filePath = @"C:\Users\SANTHOSH VENUGOPAL\source\repos\CandidateScreeningAI\wavfiles\harvard.wav";

            //if (audioFile == null || audioFile.Length == 0)
            //{
            //    return BadRequest("No file uploaded.");
            //}

          //  var filePath = Path.Combine("Uploads", audioFile.FileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await audioFile.CopyToAsync(stream);
            //}

            var text = await _googleSpeechToTextService.ConvertSpeechToTextAsync(filePath);

            // Optionally, clean up the file after processing
            //System.IO.File.Delete(filePath);

            return Ok(new { Transcription = text });
        }
    }
}
