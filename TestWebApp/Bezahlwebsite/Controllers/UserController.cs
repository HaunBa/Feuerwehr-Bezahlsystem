using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DataAccess.Extensions.Enums;

namespace Bezahlwebsite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ApplicationUser? GetUserById(string id)
        {
            return _userService.GetUserById(id);
        }

        [HttpGet]
        public List<ApplicationUser> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet]
        public ApplicationUser? GetUserByUsername(string username)
        {
            return _userService.GetUserByUsername(username);
        }

        [HttpPut]
        public State UpdateUser(string id, ApplicationUser user)
        {
            return _userService.UpdateUser(id, user);
        }

        [HttpDelete]
        public State DeleteUser(string id)
        {
            return _userService.DeleteUser(id);
        }

        [HttpGet]
        public ApplicationUser? GetUserWithPayments(string id)
        {
            return _userService.GetUserWithPayments(id);
        }

        [HttpGet]
        public ApplicationUser? GetUserWithTopUps(string id)
        {
            return _userService.GetUserWithTopUps(id);
        }

        [HttpGet]
        public ApplicationUser? GetUserWithAllInfos(string id)
        {
            return _userService.GetUserWithAllInfos(id);
        }
    }
}
