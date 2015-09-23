using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Framework.Common.Web.Extensions;
using PointEx.Entities;
using PointEx.Entities.Models;
using PointEx.Notification.Interfaces;
using PointEx.Security.Managers;

namespace PointEx.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IEmailService _emailService;
        private readonly UrlHelper _urlHelper;

        public NotificationService(ApplicationUserManager userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public async Task SendAccountConfirmationEmail(string userId, string theme)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = _urlHelper.Action("FirstLogin", "Account", new { area = "", userId = userId, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
            var body = GetAccountConfirmationEmailBody(theme, callbackUrl);
            await _userManager.SendEmailAsync(userId, "Confirme su Cuenta", body);
        }

        private string GetAccountConfirmationEmailBody(string theme, string callback)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/AccountConfirmationEmail.html", theme);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));
            body = body.Replace("{callbackUrl}", callback);

            return body;
        }

        public async Task SendPointsExchangeConfirmationEmail(Prize prize, Beneficiary beneficiary, string[] emailsAdmin, DateTime exchangeDate, string theme)
        {
            var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], beneficiary.User.Email);
            
            
            mail.Subject = "Canje de Premio";
            mail.Body = GetPointsExchangeConfirmationEmailBody(prize, beneficiary, exchangeDate, theme);
            mail.IsBodyHtml = true;

            await _emailService.SendMailAsync(mail, emailsAdmin);
        }

        public async Task SendAddShopRequestEmail(Shop shop, string email, string theme)
        {
            var adminMail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], ConfigurationManager.AppSettings["AddShopRequestAdminEmail"]);

            adminMail.Subject = "Solicitud Adherir Comercio";
            adminMail.Body = GetAddShopRequestEmailBody(shop, theme);
            adminMail.IsBodyHtml = true;

            await _emailService.SendMailAsync(adminMail);

            var shopConfirmationEmail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], email);

            shopConfirmationEmail.Subject = "Solicitud Enviada";
            shopConfirmationEmail.Body = GetAddShopRequestConfirmationEmailBody(theme);
            shopConfirmationEmail.IsBodyHtml = true;

            await _emailService.SendMailAsync(shopConfirmationEmail);

        }

        public async Task SendAddBeneficiaryRequestEmail(Beneficiary beneficiary, string email, string theme)
        {
            var adminMail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], ConfigurationManager.AppSettings["AddShopRequestAdminEmail"]);

            adminMail.Subject = "Solicitud Alta Beneficiario";
            adminMail.Body = GetAddBeneficiaryRequestEmailBody(beneficiary, theme);
            adminMail.IsBodyHtml = true;

            await _emailService.SendMailAsync(adminMail);

            var beneficiaryConfirmationEmail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], email);

            beneficiaryConfirmationEmail.Subject = "Solicitud Enviada";
            beneficiaryConfirmationEmail.Body = GetAddBeneficiaryRequestConfirmationEmailBody(theme);
            beneficiaryConfirmationEmail.IsBodyHtml = true;

            await _emailService.SendMailAsync(beneficiaryConfirmationEmail);
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
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

            return body;
        }

        public async Task SendInformationRequestEmail(InformationRequestModel request, string theme)
        {
            var receivers = ConfigurationManager.AppSettings["RequestInformationReceivers"].Split(',');
            foreach (var receiver in receivers)
            {
                var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], receiver);
                mail.Subject = "Nueva consulta";
                mail.Body = GetInformationRequestEmailBody(request, theme, "InfoRequestToAdminEmail");
                mail.IsBodyHtml = true;

                await _emailService.SendMailAsync(mail);
            }


            var mailSuccessfulQuery = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], request.Email);
            mailSuccessfulQuery.Subject = "Consulta Exitosa";
            mailSuccessfulQuery.Body = GetInformationRequestEmailBody(request, theme, "InfoRequestAutomaticResponseEmail");
            mailSuccessfulQuery.IsBodyHtml = true;

            await _emailService.SendMailAsync(mailSuccessfulQuery);
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
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

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
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

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
            body = body.Replace("{TownName}", beneficiary.Town.Name);
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

            if (beneficiary.EducationalInstitutionId.HasValue)
            {
                body = body.Replace("{EducationInstitution}", beneficiary.EducationalInstitution.Name);
            }

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

            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

            return body;
        }

        public async Task SendPendingBenefitEmail(string benefitName, string beneficiaryEmail, bool created, string theme)
        {
            var subject = created
                ? "Beneficio pendiente de aprobación"
                : "Modificación de beneficio pendiente de aprobación";
            var template = created
                ? "PendingBenefitEmail"
                : "PendingBenefitUpdateEmail";


            var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], beneficiaryEmail);
            mail.Subject = subject;
            mail.Body = GetPendingBenefitEmailBody(benefitName, theme, template);
            mail.IsBodyHtml = true;

            await _emailService.SendMailAsync(mail);
        }


        private string GetPendingBenefitEmailBody(string benefitName, string theme, string templateName)
        {
            string body = string.Empty;

            var templatePath = String.Format("~/EmailTemplates/{0}/{1}.html", theme, templateName);
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{BenefitName}", benefitName);
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

            return body;
        }


        public async Task SendBenefitApprovedMail(Benefit benefit, string siteBaseUrl)
        {
            var email = benefit.Shop.User.Email;

            var mail = new MailMessage(ConfigurationManager.AppSettings["EmailSentFrom"], email);
            mail.Subject = "Beneficio Aprobado";

            mail.Body = GetBenefitApprovedEmailBody(benefit, siteBaseUrl);
            mail.IsBodyHtml = true;

            await _emailService.SendMailAsync(mail);
        }

        private string GetBenefitApprovedEmailBody(Benefit benefit, string siteBaseUrl)
        {
            string body = string.Empty;
            var theme = ConfigurationManager.AppSettings["Theme"];
            var templatePath = String.Format("~/EmailTemplates/{0}/{1}.html", theme, "BenefitApprovedEmail");
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(templatePath)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{BenefitName}", benefit.Name);
            body = body.Replace("{BaseUrl}", siteBaseUrl);
            body = body.Replace("{BenefitId}", benefit.Id.ToString());
            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

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

            body = body.Replace("{webURL}", _urlHelper.BaseFullUrl());
            body = body.Replace("{imageURL}", _urlHelper.ContentFullPath(@"~/Content/themes/Jovenes/images/"));

            return body;
        }
    }
}

