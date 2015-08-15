using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ICardService
    {
        Card GetById(int id);
        Card GetByNumber(string number);
        IList<Card> GetByBeneficiaryId(int id);
        Card CancelCard(int cardId);
        string GenerateNumber(Beneficiary beneficiary);
        bool Generate(int beneficiaryId);
    }
}