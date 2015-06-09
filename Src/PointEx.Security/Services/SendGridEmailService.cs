using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace PointEx.Security.Services
{
    public class SendGridEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSendGridAsync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task ConfigSendGridAsync(IdentityMessage message)
        {
            var emailMessage = new SendGridMessage();

            emailMessage.AddTo(message.Destination);
            emailMessage.From = new System.Net.Mail.MailAddress("no-reply@pointex.net", "PointEx");
            emailMessage.Subject = message.Subject;
            emailMessage.Text = message.Body;
            emailMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
                                                    ConfigurationManager.AppSettings["emailService:Password"]);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(emailMessage);
            }
            else
            {
                //Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }
}
