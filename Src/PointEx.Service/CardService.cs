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
            return Uow.Cards.Get(c => c.Number == number, c => c.Beneficiary);
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
        
        public bool Generate(int beneficiaryId)
        {
            var beneficiary = Uow.Beneficiaries.Get(b => b.Id == beneficiaryId, b => b.Cards);
            var cardNumber = GenerateNumber(beneficiary);

            foreach (var active in beneficiary.Cards.Where(c => c.EndDate == null))
            {
                active.EndDate = DateTime.Now;
                Uow.Cards.Edit(active);
            }

            var card = new Card()
            {
                BeneficiaryId = beneficiaryId,
                IssueDate = DateTime.Now,
                Number = cardNumber
            };
            Uow.Cards.Add(card);
            Uow.Commit();
            return true;
        }

        public string GenerateNumber(Beneficiary beneficiary)
        {
            var next = beneficiary.Cards.Count();

            var number = next < 10 ? String.Format("0{0}", next) : next.ToString();
            var cardNumber = String.Format("{0}{1}", beneficiary.IdentificationNumber, number);
            return cardNumber;
        }
    }
}
