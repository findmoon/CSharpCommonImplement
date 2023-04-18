
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Models;
using WebAPI_CURD.Models.Users;
using WebAPI_CURD.Services;

namespace WebAPI_CURD.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Json(users);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            return Json(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRequest model)
        {
            await _userService.Create(model);
            return Json(new ResponseModel<User> { message = "User created", Data = await _userService.GetByEmail(model.Email) });
        }

        [Route("{id}")]
        public async Task<ActionResult> Update( UpdateRequest model)
        {
            await _userService.Update(model);
            return Json(new ResponseModel<User> { message = "User updated", Data = await _userService.GetByEmail(model.Email) });
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Json(new ResponseModel<User> { message = "User deleted" });
        }
    }
}