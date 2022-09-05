namespace TestingApp.Hubs
{
    public interface IVendingHub
    {
        Task SendError(int slot, int vendingNr);
        Task EjectItem(List<VendingItems> vendingItems);
        Task RegisterVendingmachine(int vendingNr);
    }
}
