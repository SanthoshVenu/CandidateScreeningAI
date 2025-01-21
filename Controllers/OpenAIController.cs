//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OpenAI;

//namespace CandidateScreeningAI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class OpenAIController : ControllerBase
//    {
//        [HttpGet]
//        [Route("UseChatGPT")]
//        public async Task<IActionResult> UseChatGPT(string query)
//        {
//            string outputResult = "";
//            OpenAIClient client = new(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
//            CompletionRequest completionRequest = new CompletionRequest();
//            completionRequest.Prompt = query;
//            completionRequest.Model = OpenAI_API.Models.Model.DavinciText;
//            completionRequest.MaxTokens = 1024;

//            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);

//            foreach (var completion in completions.Completions)
//            {
//                outputResult += completion.Text;
//            }

//            return Ok(outputResult);

//        }
//    }
