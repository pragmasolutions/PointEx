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
using PointEx.Entities.Models;
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

        public async Task SendPointsExchangeConfirmationEmail(Prize prize, Beneficiary beneficiary, DateTime exchangeDate, string theme)
        {
            using (var client = GetSmtpClient())
            {
                var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], beneficiary.User.Email);

                mail.Subject = "Canje de Premio";
                mail.Body = GetPointsExchangeConfirmationEmailBody(prize, beneficiary, exchangeDate, theme);
                mail.IsBodyHtml = true;

                await client.SendMailAsync(mail);
            }
        }

        public async Task SendAddShopRequestEmail(Shop shop, string email, string theme)
        {
            using (var client = GetSmtpClient())
            {
                var adminMail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], ConfigurationManager.AppSettings["AddShopRequestAdminEmail"]);

                adminMail.Subject = "Solicitud Adherir Comercio";
                adminMail.Body = GetAddShopRequestEmailBody(shop, theme);
                adminMail.IsBodyHtml = true;

                await client.SendMailAsync(adminMail);

                var shopConfirmationEmail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], email);

                shopConfirmationEmail.Subject = "Solicitud Enviada";
                shopConfirmationEmail.Body = GetAddShopRequestConfirmationEmailBody(theme);
                shopConfirmationEmail.IsBodyHtml = true;

                await client.SendMailAsync(shopConfirmationEmail);
            }
        }

        public async Task SendAddBeneficiaryRequestEmail(Beneficiary beneficiary, string email, string theme)
        {
            using (var client = GetSmtpClient())
            {
                var adminMail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], ConfigurationManager.AppSettings["AddShopRequestAdminEmail"]);

                adminMail.Subject = "Solicitud Adherir Comercio";
                adminMail.Body = GetAddBeneficiaryRequestEmailBody(beneficiary, theme);
                adminMail.IsBodyHtml = true;

                await client.SendMailAsync(adminMail);

                var shopConfirmationEmail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], email);

                shopConfirmationEmail.Subject = "Solicitud Enviada";
                shopConfirmationEmail.Body = GetAddBeneficiaryRequestConfirmationEmailBody(theme);
                shopConfirmationEmail.IsBodyHtml = true;

                await client.SendMailAsync(shopConfirmationEmail);
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

        private string GetPointsExchangeConfirmationEmailBody(Prize prize, Beneficiary beneficiary, DateTime exchangeDate, string theme)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/PointExchangeConfirmationEmail.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
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

        public async Task SendInformationRequestEmail(InformationRequestModel request, string theme)
        {
            var receivers = ConfigurationManager.AppSettings["RequestInformationReceivers"].Split(',');
            foreach (var receiver in receivers)
            {
                using (var client = GetSmtpClient())
                {
                    var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], receiver);
                    mail.Subject = "Nueva consulta";
                    mail.Body = GetInformationRequestEmailBody(request, theme, "InfoRequestToAdminEmail");
                    mail.IsBodyHtml = true;

                    await client.SendMailAsync(mail);
                }
            }
            

            using (var client = GetSmtpClient())
            {
                var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], request.Email);
                mail.Subject = "Consulta Exitosa";
                mail.Body = GetInformationRequestEmailBody(request, theme, "InfoRequestAutomaticResponseEmail");
                mail.IsBodyHtml = true;

                await client.SendMailAsync(mail);
            }
        }

        private string GetInformationRequestEmailBody(InformationRequestModel request, string theme, string filename)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/{1}.html", theme, filename);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Name}", request.Name);
            body = body.Replace("{Date}", String.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            body = body.Replace("{Cause}", request.Cause);
            body = body.Replace("{Text}", request.Text);
            body = body.Replace("{Email}", request.Email);
            body = body.Replace("{PhoneNumber}", request.PhoneNumber);
            

            return body;
        }

        private string GetAddShopRequestEmailBody(Shop shop, string theme)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/AddShopRequest.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Name}", shop.Name);
            body = body.Replace("{Address}", shop.Address);
            body = body.Replace("{Phone}", shop.Phone);
            body = body.Replace("{TownName}", shop.Town.Name);

            return body;
        }

        private string GetAddBeneficiaryRequestEmailBody(Beneficiary beneficiary, string theme)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/AddBeneficiaryRequest.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Name}", beneficiary.Name);
            body = body.Replace("{Address}", beneficiary.Address);
            if (beneficiary.EducationalInstitutionId.HasValue)
            {
                body = body.Replace("{EducationInstitution}", beneficiary.EducationalInstitution.Name);
            }

            body = body.Replace("{TownName}", beneficiary.Town.Name);

            return body;
        }

        private string GetAddShopRequestConfirmationEmailBody(string theme)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/AddShopRequestConfirmation.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }

        private string GetAddBeneficiaryRequestConfirmationEmailBody(string theme)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/AddBeneficiaryRequestConfirmation.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }

    }
}

