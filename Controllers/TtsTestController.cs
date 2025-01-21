using CandidateScreeningAI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CandidateScreeningAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TtsTestController : ControllerBase
    {
        private readonly IGoogleTTSService _googleTtsService;

        public TtsTestController(IGoogleTTSService googleTtsService)
        {
            _googleTtsService = googleTtsService;
        }

        [HttpPost]
        public async Task<IActionResult> TestTts([FromBody] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("Text cannot be empty.");
            }


            string folderPath = @"C:\Users\SANTHOSH VENUGOPAL\source\repos\CandidateScreeningAI\output";
            string fileName = "tts_output.mp3" + DateTime.UtcNow.ToString() ;
            var audioBytes = await _googleTtsService.ConvertTextToSpeechAsync(text);
            var filePath = Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            } // Ensure the output directory exists
            await System.IO.File.WriteAllBytesAsync(filePath, audioBytes);

            return Ok($"TTS audio saved at: {filePath}");
        }
    }
}
