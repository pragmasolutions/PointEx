using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ICardService
    {
        Card GetById(int id);
        Card GetByNumber(string number);
        bool ValidateCardNumber(string cardNumber);
    }
}