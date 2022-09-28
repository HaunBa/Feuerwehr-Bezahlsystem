using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Extensions.Enums;

namespace DataAccess.Interfaces
{
    public interface IPaymentService
    {
        public State AddPaymentToPerson(Payment payment, string userId);
        public State RemovePaymentFromPerson(int paymentId, string userId);
        public List<Payment>? GetPaymentsFromUser(string userId);
    }
}
