using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace TodoApp.AWSServices
{
    public class SimpleEmailService
    {
        private AmazonSimpleEmailServiceClient _client {get; set;} = new AmazonSimpleEmailServiceClient();
        private string _fromMail = Environment.GetEnvironmentVariable("FromMail");
        private string _toMail = Environment.GetEnvironmentVariable("ToMail");
        public async Task SendEmail(string subject, string message)
        {
            var sendEmailRequest = new SendEmailRequest()
            {
                Source = _fromMail,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { _toMail }
                },
                Message = new Message()
                {
                    Subject = new Content("New insert"),
                    Body = new Body(new Content(message))
                }
            };
            var emailresponse = await _client.SendEmailAsync(sendEmailRequest);
            if (emailresponse.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not add Note, {emailresponse.HttpStatusCode}");
        }
    }
}