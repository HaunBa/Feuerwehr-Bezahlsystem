using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DataAccess.Extensions.Enums;

namespace Bezahlwebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public State AddPaymentToPerson(Payment payment, string userId)
        {
            return _paymentService.AddPaymentToPerson(payment, userId);
        }

        [HttpDelete]
        public State RemovePaymentFromPerson(int paymentId, string userId)
        {
            return _paymentService.RemovePaymentFromPerson(paymentId, userId);
        }

        [HttpGet]
        public List<Payment>? GetPaymentsFromUser(string userId)
        {
            return _paymentService.GetPaymentsFromUser(userId);
        }
    }
}
