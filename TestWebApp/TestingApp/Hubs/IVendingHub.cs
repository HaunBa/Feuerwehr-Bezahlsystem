namespace TestingApp.Hubs
{
    public interface IVendingHub
    {
        Task SendError(int slot, int vendingNr);
        Task EjectItem(List<VendingItems> vendingItems);
        Task RegisterVendingmachine(int vendingNr);
        Task AddArticleToBought(int slot, int vendingNr, string? username);
    }
}
