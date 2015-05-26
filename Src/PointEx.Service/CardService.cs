using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class CardService : ServiceBase, ICardService
    {
        public CardService(IPointExUow uow)
        {
            Uow = uow;
        }

        public Card GetById(int id)
        {
            return Uow.Cards.Get(id);
        }

        public Card GetByNumber(string number)
        {
            return Uow.Cards.Get(c => c.Number == number);
        }

        public bool ValidateCardNumber(string cardNumber)
        {
            var card = this.GetByNumber(cardNumber);

            if (card == null)
            {
                return false;
            }

            //TODO: Validate if the card is active.
            return true;
        }

        public IList<Card> GetByBeneficiaryId(int beneficiaryId)
        {
            return Uow.Cards.GetAll(c => c.BeneficiaryId == beneficiaryId).ToList();
        }
    }
}
