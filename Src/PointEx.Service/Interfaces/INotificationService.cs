using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public interface INotificationService
    {
        Task SendAccountConfirmationEmail(string userId);
    }
}