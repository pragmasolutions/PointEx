namespace PointEx.Service
{
    public interface IPointsExchangeService : IServive
    {
        void ExchangePoints(int prizeId, int beneficiaryId);
    }
}