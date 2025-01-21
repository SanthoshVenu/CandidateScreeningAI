using Google.Cloud.TextToSpeech.V1;
using Microsoft.Extensions.Configuration;

namespace CandidateScreeningAI.Services
{
    public class GoogleTTSService : IGoogleTTSService
    {
        private readonly string _credentialsPath;

        public GoogleTTSService(IConfiguration configuration)
        {
            _credentialsPath = configuration["GoogleCloud:CredentialsPath"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _credentialsPath);
        }

        public async Task<byte[]> ConvertTextToSpeechAsync(string text, string language = "en-US")
        {
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput { Text = text };
            var voice = new VoiceSelectionParams
            {
                LanguageCode = language,
                SsmlGender = SsmlVoiceGender.Neutral
            };
            var config = new AudioConfig { AudioEncoding = AudioEncoding.Mp3 };

            var response = await client.SynthesizeSpeechAsync(input, voice, config);
            return response.AudioContent.ToByteArray();
        }
    }
}
