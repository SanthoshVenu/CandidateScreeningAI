using CandidateScreeningAI.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace CandidateScreeningAI.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var request = new
            {
                model = "llama2:7b", // Specify the Llama model
                prompt
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error calling Ollama API: {response.ReasonPhrase}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            string[] jsonObjects = responseContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var responses = new List<string>();
            foreach (string jsonObject in jsonObjects)
            {
                // Deserialize each object
                var data = JsonConvert.DeserializeObject<ResponseModel>(jsonObject).Response;
                responses.Add(data);
            }
            return string.Join(" ",responses) ?? "No response from Ollama.";
        }

        public class ResponseModel
        {
            public string? Model { get; set; }
            public DateTime Created_At { get; set; }
            public string? Response { get; set; }
            public bool Done { get; set; }
        }
    }
}
