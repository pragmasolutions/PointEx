using System.Threading.Tasks;
namespace PointEx.Service
{
    public interface IPointsExchangeService : IServive
    {
        Task ExchangePoints(int prizeId, int beneficiaryId);
    }
}