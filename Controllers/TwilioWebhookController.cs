﻿using CandidateScreeningAI.Interface;
using CandidateScreeningAI.Services;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace CandidateScreeningAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioWebhookController : ControllerBase
    {
        private readonly ISpeechToTextService _speechToTextService;
        private readonly IOpenAIService _openAIService;
        private readonly IOllamaService _ollamaService;
        private readonly IGroqService _groqService;
        public TwilioWebhookController(ISpeechToTextService speechToTextService, IOpenAIService openAIService, IOllamaService ollamaService, IGroqService groqService)
        {
            _speechToTextService = speechToTextService;
            _openAIService = openAIService;
            _ollamaService = ollamaService;
            _groqService = groqService;
        }

        [HttpPost("process-response")]
        public async Task<IActionResult> ProcessResponse()
        {
            try
            {
                var form = await Request.ReadFormAsync();

                // Extract SpeechResult and RecordingUrl
                string speechResult = form["SpeechResult"];
                //string speechResult = "Hi, This is Santhosh . I have five years of experience in Web Development";
                string recordingUrl = form["RecordingUrl"];
                var twimlResponse = new VoiceResponse();
                // var userInput = "I have five years of experience in web development";
                // Generate response using Groq
                var response = await _groqService.GenerateResponseAsync(speechResult);
                twimlResponse.Say(response);
                var webhookUrl = "https://09b6-110-224-88-65.ngrok-free.app/api/TwilioWebhook/process-response";

                Console.WriteLine($"Generated Response: {response}");
                twimlResponse.Gather(input: new[] { Gather.InputEnum.Speech }, language: "en-IN", action: new Uri(webhookUrl), method: "POST");
                return Content(twimlResponse.ToString(), "application/xml");
                //return Ok(new { UserInput = userInput, GroqResponse = response });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the response.");
            }
        }

        //[HttpPost("process-response")]
        //public async Task<IActionResult> ProcessResponse()
        //{
        //    try
        //    {
        //        // Read all form data
        //        //var form = await Request.ReadFormAsync();

        //        //// Extract SpeechResult and RecordingUrl
        //        //string speechResult = form["SpeechResult"];
        //        //string recordingUrl = form["RecordingUrl"];

        //        //Console.WriteLine($"SpeechResult: {speechResult}");
        //        //Console.WriteLine($"RecordingUrl: {recordingUrl}");

        //        //if (string.IsNullOrWhiteSpace(speechResult))
        //        //{
        //        //    var response = new VoiceResponse();
        //        //    response.Say("I didn't catch that. Can you please repeat?");
        //        //    return Content(response.ToString(), "application/xml");
        //        //}
        //       // var followUpQuestion = await _openAIService.GetFollowUpQuestionAsync("Can u say what is openai");

        //        var generatedResponse = await _ollamaService.GenerateResponseAsync("Hi");

        //        var twimlResponse = new VoiceResponse();
        //        twimlResponse.Say(generatedResponse);
        //        var webhookUrl = "https://09b6-110-224-88-65.ngrok-free.app/api/TwilioWebhook/process-response";

        //        Console.WriteLine($"Generated Response: {generatedResponse}");
        //        twimlResponse.Gather(input: new[] { Gather.InputEnum.Speech }, action: new Uri(webhookUrl), method: "POST");
        //        return Content(twimlResponse.ToString(), "application/xml");

        //        //else if (!string.IsNullOrEmpty(recordingUrl))
        //        //{
        //        //    // Handle RecordingUrl
        //        //    var responseText = await _speechToTextService.ConvertSpeechToTextAsync(recordingUrl);
        //        //    return Ok($"Processed Response: {responseText}");
        //        //}

        //        // If neither SpeechResult nor RecordingUrl is present
        //       // return BadRequest("No speech input or recording URL provided.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error processing Twilio webhook: {ex.Message}");
        //        return StatusCode(500, "An error occurred while processing the response.");
        //    }
        //}
    }
}
