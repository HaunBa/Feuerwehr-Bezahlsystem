namespace DataAccess.Interfaces
{
    public interface IUserService
    {
        public ApplicationUser? GetUserById(string id);
        public List<ApplicationUser> GetAllUsers();
        public ApplicationUser? GetUserByUsername(string username);
        public State UpdateUser(string id, ApplicationUser user);
        public State DeleteUser(string id);
        public ApplicationUser? GetUserWithPayments(string id);
        public ApplicationUser? GetUserWithTopUps(string id);
        public ApplicationUser? GetUserWithAllInfos(string id);
    }
}
