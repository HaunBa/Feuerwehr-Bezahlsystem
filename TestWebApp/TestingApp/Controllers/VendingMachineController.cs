using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendingMachineController : ControllerBase
    {
        [HttpGet]
        private string GetCurrentConfigVersion()
        {
            return "1.0.0.0";
        }
    }
}
