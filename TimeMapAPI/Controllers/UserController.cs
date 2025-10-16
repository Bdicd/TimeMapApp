using Microsoft.AspNetCore.Mvc;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<List<User>> GetAll()
    {
        var users = _userRepository.GetAll();
        return Ok(users);
    }
    [HttpPost]
    public ActionResult AddUser([FromBody] string name)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        _userRepository.Add(newUser);
        return CreatedAtAction(nameof(GetAll), new { id = newUser.Id }, newUser);
    }
}
