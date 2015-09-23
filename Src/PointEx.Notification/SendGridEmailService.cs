using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PointEx.Notification.Interfaces;
using SendGrid;

namespace PointEx.Notification
{
    public class SendGridEmailService : IEmailService, IIdentityMessageService
    {
        public Task SendMailAsync(System.Net.Mail.MailMessage message)
        {
            return SendToSendGridAsync(message.To.Select(x => x.Address), message.Subject, message.Body);
        }

        public Task SendMailAsync(System.Net.Mail.MailMessage message, string[] emailBCC)
        {
            return SendToSendGridAsync(message.To.Select(x => x.Address), message.Subject, message.Body, emailBCC);
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await SendToSendGridAsync(message.Destination, message.Subject, message.Body);
        }

        private async Task SendToSendGridAsync(string destinations, string subject, string body)
        {
            await SendToSendGridAsync(new List<string>() { destinations }, subject, body);
        }

        private async Task SendToSendGridAsync(IEnumerable<string> destinations, string subject, string body, string[] emailsBcc = null)
        {
            var emailMessage = new SendGridMessage();
            
            emailMessage.AddTo(destinations);
            
            if (emailsBcc != null)
            {
                System.Net.Mail.MailAddress[] bcc = new System.Net.Mail.MailAddress[emailsBcc.Length];
                for (int i = 0; i < emailsBcc.Length; i++)
                {
                    bcc[i] = new System.Net.Mail.MailAddress(emailsBcc[i]);
                }

                emailMessage.Bcc = bcc;
            }
            
            emailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["defaultEmailFromAddress"],
                                                    ConfigurationManager.AppSettings["fromAddress"]);
            emailMessage.Subject = subject;
            emailMessage.Text = body;
            emailMessage.Html = body;

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
