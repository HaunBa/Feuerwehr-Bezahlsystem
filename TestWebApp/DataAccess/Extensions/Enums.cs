namespace DataAccess.Extensions
{
    public class Enums
    {
        public enum ArtType
        {
            Food = 0,
            Drink = 1,
            Else = 2
        }

        public enum Roles
        {
            SuperAdmin,
            Admin,
            Moderator,
            User
        }

        public enum State
        {
            Successful,
            Error
        }
    }
}
