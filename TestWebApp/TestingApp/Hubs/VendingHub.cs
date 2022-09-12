namespace TestingApp.Hubs
{
    public class VendingHub : Hub<IVendingHub>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VendingHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendError(int slot, int vendingNr) => await Clients.Group(vendingNr.ToString()).SendError(slot, vendingNr);

        public async Task EjectItem(List<VendingItems> vendingItems, int vendingNr) => await Clients.Group(vendingNr.ToString()).EjectItem(vendingItems);

        public async Task RegisterVendingmachine(int vendingNr)
        {
            Console.WriteLine($"Vending Machine {vendingNr} registered");
            await Groups.AddToGroupAsync(Context.ConnectionId, vendingNr.ToString());
        }
        public async Task AddArticleToBought(int slot, int vendingNr, string? username)
        {
            
            var fArticle = await _context.Articles.FirstOrDefaultAsync(x => x.IsInVending == true && x.VendingSlot == slot && x.VendingMachineNumber == vendingNr);
            if(fArticle == null) return;

            if (username != null)
            {
                // rfid payment via vending machine
                var fPerson = await _userManager.FindByNameAsync(username);
                if (fPerson == null) return;

                var boughtArticle = new BoughtArticle()
                {
                    Active = fArticle.Active,
                    Amount = 1,
                    IsInVending = true,
                    Name = fArticle.Name,
                    PriceId = fArticle.PriceId,
                    Type = fArticle.Type,
                    VendingMachineNumber = vendingNr,
                    VendingSlot = slot
                };

                var currentDate = DateTime.Now;

                var newPayment = new Payment()
                {
                    Articles = new List<BoughtArticle>() { boughtArticle },
                    CashAmount = boughtArticle.Price.Amount,
                    Date = currentDate,
                    Description = $"Bezahlung am {currentDate} über {boughtArticle.Price.Amount} € mittels des Automaten",
                    PersonId = fPerson.Id
                };

                fPerson.Payments.Add(newPayment);
                await _userManager.UpdateAsync(fPerson);
                return;
            }
            else
            {
                // coin payment via vending machine
                var boughtArticle = new BoughtArticle()
                {
                    Active = fArticle.Active,
                    Amount = 1,
                    IsInVending = true,
                    Name = fArticle.Name,
                    PriceId = fArticle.PriceId,
                    Type = fArticle.Type,
                    VendingMachineNumber = vendingNr,
                    VendingSlot = slot
                };

                _context.BoughtArticles.Add(boughtArticle);
                await _context.SaveChangesAsync();

                return;
            }
        }
    }
}