using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PointEx.Entities;
using PointEx.Security.Managers;

namespace PointEx.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationUserManager _userManager;

        public NotificationService(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task SendAccountConfirmationEmail(string userId)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = urlhelper.Action("FirstLogin", "Account", new { area = "", userId = userId, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
            await _userManager.SendEmailAsync(userId, "Confirme su Cuenta", "Por favor confirme su cuenta y cambie su contraseña haciendo click <a href=\"" + callbackUrl + "\">aquí</a>");
        }

        public async Task SendPointsExchangeConfirmationEmail(Prize prize, Beneficiary beneficiary, DateTime exchangeDate)
        {
            using (var client = GetSmtpClient())
            {
                var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], beneficiary.User.Email);

                mail.Subject = "Canje de Premio";
                mail.Body = GetPointsExchangeConfirmationEmailBody(prize, beneficiary, exchangeDate);
                mail.IsBodyHtml = true;

                await client.SendMailAsync(mail);
            }
        }

        private SmtpClient GetSmtpClient()
        {
            var credentialUserName = ConfigurationManager.AppSettings["EmailSentFrom"];

            var pwd = ConfigurationManager.AppSettings["EmailPassword"];

            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmptClient"]);

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            NetworkCredential credentials =
                new NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            return client;
        }

        private string GetPointsExchangeConfirmationEmailBody(Prize prize, Beneficiary beneficiary, DateTime exchangeDate)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/PointExchangeConfirmationEmail.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{PrizeName}", prize.Name);
            body = body.Replace("{PrizeDescription}", prize.Description);
            body = body.Replace("{BeneficiaryName}", beneficiary.Name);
            body = body.Replace("{ExchangeDate}", exchangeDate.ToShortDateString());
            body = body.Replace("{PrizePointsNeeded}", prize.PointsNeeded.ToString());

            return body;
        }
    }
}
