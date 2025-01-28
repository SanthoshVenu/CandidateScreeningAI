using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML.Voice;
using Google.Cloud.Speech.V1;
using static Twilio.TwiML.Voice.Gather;
using Task = System.Threading.Tasks.Task;

namespace CandidateScreeningAI.Services
{
    public class TwilioTelephonyService : ITelephonyService
    {
        private readonly string _accountSid = "AC6a319549a25f908ae878803687429feb";
        private readonly string _authToken = "3ae3a1bccdb8c13cedce92553291dea3";
        private readonly string _twilioPhoneNumber = "+16286666346";

        public TwilioTelephonyService()
        {
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task MakeInteractiveCallAsync(string phoneNumber, List<string> questions)
        {
            // Build the TwiML response
            var response = new Twilio.TwiML.VoiceResponse();
            var webhookUrl = "https://09b6-110-224-88-65.ngrok-free.app/api/TwilioWebhook/process-response";


            foreach (var question in questions)
            {
                // Add Gather for each question using Append
                var gather = new Gather
                {
                    Input = new[] { InputEnum.Speech },
                    Action = new Uri(webhookUrl),
                    Method = Twilio.Http.HttpMethod.Post,
                    Language = LanguageEnum.EnIn,
                    Hints = "hello, interview, job, skills",
                };
                gather.Say(question);
                //response.Record(timeout: 5, transcribe: true);
                response.Append(gather); // Use Append to add the gather to the response
            }

            // Add a closing message
            response.Say("Thank you for your responses. Goodbye!");

            // Convert TwiML to string
            var twiml = response.ToString();

            // Make the call
            await CallResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_twilioPhoneNumber),
                twiml: new Twilio.Types.Twiml(twiml)
            );
        }

        //public async Task MakeCallAsync(string phoneNumber, string message)
        //{
        //    var webHookUrl = "https://09b6-110-224-88-65.ngrok-free.app/api/TwilioWebhook/process-response";
        //    await CallResource.CreateAsync(
        //        to: new Twilio.Types.PhoneNumber(phoneNumber),
        //        from: new Twilio.Types.PhoneNumber(_twilioPhoneNumber),
        //        twiml: new Twilio.Types.Twiml($@"
        //            <Response>
        //                <Say>{message}</Say>
        //                <Gather input=""speech"" action=""{webHookUrl}"" method=""POST"" />
        //            </Response>")
        //    );         
        //}
    }
}
