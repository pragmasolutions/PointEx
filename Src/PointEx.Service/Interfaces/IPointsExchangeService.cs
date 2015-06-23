using System.Collections.Generic;
using System.Threading.Tasks;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IPointsExchangeService : IServive
    {
        Task ExchangePoints(int prizeId, int beneficiaryId);
    }
}