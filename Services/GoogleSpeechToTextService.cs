using Python.Runtime;
using Microsoft.Extensions.Configuration;
using System;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

namespace CandidateScreeningAI.Services
{
    public class GoogleSpeechToTextService : ISpeechToTextService
    {
        private readonly string _credentialsPath;

        public GoogleSpeechToTextService(IConfiguration configuration)
        {
            _credentialsPath = configuration["GoogleCloud:CredentialsPath"];
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _credentialsPath);

            //Runtime.PythonDLL = @"C:\Users\SANTHOSH VENUGOPAL\AppData\Local\Programs\Python\Python313\python313.dll";

            //string pythonPath = @"C:\Users\SANTHOSH VENUGOPAL\source\repos\CandidateScreeningAI\Scripts";
            //Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath);
            // string pythonDllPath = @"C:\Users\SANTHOSH VENUGOPAL\AppData\Local\Programs\Python\Python313\python313.dll"; // Full path to python.dll
            // Environment.SetEnvironmentVariable("PYTHONNET_PYTHON", pythonDllPath); // Set PythonNet to use the correct python.dll

            // Optionally, set PYTHONHOME to Python installation directory (without python.dll)
            //string pythonHome = @"C:\Users\SANTHOSH VENUGOPAL\AppData\Local\Programs\Python\Python313";
            //Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);

            // Initialize the Python engine
        }

        public async Task<string> ConvertSpeechToTextAsync(string audioFilePath)
        {
            var speechClient = SpeechClient.Create();
            var audio = RecognitionAudio.FromFile(audioFilePath);
            var config = new RecognitionConfig
            {
                Encoding = (RecognitionConfig.Types.AudioEncoding)AudioEncoding.Linear16,
                SampleRateHertz = 16000,
                LanguageCode = "en-US",
            };
            var transcript = new List<string>();
            var response = await speechClient.RecognizeAsync(config, audio);
            if(response.Results.Count > 0)
            {
              foreach(var result in response.Results)
                {
                    transcript.Add(result.Alternatives[0].Transcript);
                }
            }
           // var result = response.Results[0].Alternatives[0].Transcript;
            return string.Join(" ", transcript);
        }
    }
}
