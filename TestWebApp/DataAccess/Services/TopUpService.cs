namespace DataAccess.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        public TopUpService(ApplicationDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }
        public State AddTopUpToPerson(TopUp topup, string userId)
        {
            var user = _userService.GetUserById(userId);
            if (user == null) return State.Error;

            user.TopUps.Add(topup);
            _db.Update(user);
            _db.SaveChanges();

            return State.Successful;
        }

        public List<TopUp>? GetTopUpsFromPerson(string userId)
        {
            var user = _userService.GetUserById(userId);
            if (user == null) return null;

            return user.TopUps;
        }

        public State RemoveTopUpFromPerson(int topUpId, string userId)
        {
            var user = _userService.GetUserById(userId);
            if(user == null) return State.Error;
            var topUp = user.TopUps.FirstOrDefault(x => x.TopUpId == topUpId);
            if (topUp == null) return State.Error;

            _db.TopUps.Remove(topUp);
            _db.SaveChanges();

            return State.Successful;
        }
    }
}
