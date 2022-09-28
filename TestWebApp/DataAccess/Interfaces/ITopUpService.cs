namespace DataAccess.Interfaces
{
    public interface ITopUpService
    {
        public State AddTopUpToPerson(TopUp topup, string userId);
        public State RemoveTopUpFromPerson(int topUpId, string userId);
        public List<TopUp>? GetTopUpsFromPerson(string userId);
    }
}
