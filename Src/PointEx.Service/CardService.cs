using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Service.Exceptions;

namespace PointEx.Service
{
    public class CardService : ServiceBase, ICardService
    {
        private readonly IClock _clock;

        public CardService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
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
            return Uow.Cards.GetAll(c => c.BeneficiaryId == beneficiaryId).OrderByDescending(c => c.IssueDate).ToList();
        }

        public Card CancelCard(int cardId)
        {
            var card = GetById(cardId);

            if (card == null)
            {
                throw new NotFoundException("No se encontro la tarjeta");
            }

            if (card.EndDate.HasValue)
            {
                throw new ApplicationException("La Tarjeta ya fue cancelada");
            }

            card.EndDate = _clock.Now;

            Uow.Cards.Edit(card);
            Uow.Commit();

            return card;
        }

        public void Create(Card card)
        {
            if (!IsCardNumberAvailable(card.Number))
            {
                throw new ApplicationException("El numero de tarjeta no esta disponible");
            }

            card.IssueDate = _clock.Now;
            Uow.Cards.Add(card);
            Uow.Commit();
        }

        public bool IsCardNumberAvailable(string number)
        {
            var card = this.GetByNumber(number);
            return card == null;
        }
    }
}
