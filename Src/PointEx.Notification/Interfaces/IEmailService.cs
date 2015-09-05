using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Notification.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(MailMessage message);
        Task SendMailAsync(MailMessage message, string[] emailsBcc);
        
    }
}
