namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        public readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            this.userManager = userManager;
        }

        public State DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return State.Successful;
            }

            return State.Error;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public ApplicationUser? GetUserById(string id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser? GetUserByUsername(string username)
        {
            return _db.Users.FirstOrDefault(x => x.UserName == username);
        }

        public ApplicationUser? GetUserWithAllInfos(string id)
        {
            return _db.Users.Include(x => x.TopUps).Include(x => x.Payments).FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser? GetUserWithPayments(string id)
        {
            return _db.Users.Include(x => x.Payments).FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser? GetUserWithTopUps(string id)
        {
            return _db.Users.Include(x => x.TopUps).FirstOrDefault(x => x.Id == id);
        }

        public State UpdateUser(string id, ApplicationUser user)
        {
            var fUser = GetUserById(id);
            if (fUser != null)
            {
                fUser = user;
                fUser.Id = id;

                _db.Users.Update(fUser);
                _db.SaveChanges();
            }

            return State.Error;
        }
    }
}
