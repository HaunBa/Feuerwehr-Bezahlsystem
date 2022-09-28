using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DataAccess.Extensions.Enums;

namespace Bezahlwebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;
        public TopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [HttpPost]
        public State AddTopUpToPerson(TopUp topup, string userId)
        {
            return _topUpService.AddTopUpToPerson(topup, userId);
        }

        [HttpDelete]
        public State RemoveTopUpFromPerson(int topUpId, string userId)
        {
            return _topUpService.RemoveTopUpFromPerson(topUpId, userId);
        }

        [HttpGet]
        public List<TopUp>? GetTopUpsFromPerson(string userId)
        {
            return _topUpService.GetTopUpsFromPerson(userId);
        }
    }
}
