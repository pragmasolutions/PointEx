using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ICardService
    {
        Card GetById(int id);
        Card GetByNumber(string number);
        bool ValidateCardNumber(string cardNumber);
        IList<Card> GetByBeneficiaryId(int id);
        Card CancelCard(int cardId);
        void Create(Card card);
        bool IsCardNumberAvailable(string number);
    }
}