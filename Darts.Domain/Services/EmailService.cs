using Darts.Domain.Abstracts;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;
using System.Net;
using System;
using Darts.Contract.Interfaces;
using Development.Support.Interfaces;

namespace Darts.Domain.Services
{
   public class EmailService : IEmailService
   {
      private readonly ILogService _logger;

      public EmailService(ILogService logger)
      {
         _logger = logger;
      }

      public async Task<HttpStatusCode> SendEmailAsync(string from, string to, string subject, string message)
      {

         _logger.Information<EmailService>($"Sending email from: {from} to: {to}, subject: {subject}, message:{message}");

         try
         {

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var sendFrom = new EmailAddress(from);
            var sendTo = new EmailAddress(to);
            var plainTextContent = message;
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(sendFrom, sendTo, subject, plainTextContent, null);
            Response response = await client.SendEmailAsync(msg);

            _logger.Information<EmailService>($"Email sent with status code: {response.StatusCode}");
            return response.StatusCode;
         }
         catch(Exception e)
         {
            _logger.Exception<EmailService>(e, true);
         }

         return HttpStatusCode.InternalServerError;

      }
   }
}
