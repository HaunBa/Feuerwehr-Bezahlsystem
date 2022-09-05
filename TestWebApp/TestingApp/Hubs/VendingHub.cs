namespace TestingApp.Hubs
{
    public class VendingHub : Hub<IVendingHub>
    {
        public async Task SendError(int slot, int vendingNr) => await Clients.Group(vendingNr.ToString()).SendError(slot, vendingNr);

        public async Task EjectItem(List<VendingItems> vendingItems, int vendingNr) => await Clients.Group(vendingNr.ToString()).EjectItem(vendingItems);

        public async Task RegisterVendingmachine(int vendingNr) => await Groups.AddToGroupAsync(Context.ConnectionId, vendingNr.ToString());
    }
}