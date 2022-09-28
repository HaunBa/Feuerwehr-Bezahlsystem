namespace DataAccess.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;

        public PaymentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public State AddPaymentToPerson(Payment payment, string userId)
        {
            var user = _userService.GetUserWithPayments(userId);
            if (user == null) return State.Error;
            
            user.Payments.Add(payment);
            _db.Update(user);
            _db.SaveChanges();

            return State.Successful;
        }

        public List<Payment>? GetPaymentsFromUser(string userId)
        {
            var user = _userService.GetUserWithPayments(userId);
            if (user == null) return null;

            return user.Payments;
        }

        public State RemovePaymentFromPerson(int paymentId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
