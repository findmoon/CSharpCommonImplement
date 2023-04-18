namespace WebAPI_CURD.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI_CURD.Entities;
using WebAPI_CURD.Models;
using WebAPI_CURD.Models.Users;
using WebAPI_CURD.Services;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRequest model)
    {
        var user = await _userService.Create(model);
        return Ok(new ResponseModel<User> { message = "User created",Data= user });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRequest model)
    {
        var user = await _userService.Update(id, model);
        return Ok(new ResponseModel<User> { message = "User updated", Data = user });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return Ok(new ResponseModel<User> { message = "User deleted" });
    }
}