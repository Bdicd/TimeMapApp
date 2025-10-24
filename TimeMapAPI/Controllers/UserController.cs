using Microsoft.AspNetCore.Mvc;
using TimeMap.API.Models.Requests;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;

    [HttpGet]
    //TODO в какой ситуации в открытом API может понадобиться возвращать список всех пользователей?
    public ActionResult<List<User>> GetAll()
    {
        var users = _userRepository.GetAll();
        return Ok(users);
    }

    [HttpPost]
    public ActionResult AddUser([FromBody] CreateUserRequest request)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Password = request.Password
        };

        _userRepository.Add(newUser);
        //TODO подумать над безопасностью - стоит ли возвращать созданного пользователя с паролем?
        return CreatedAtAction(nameof(GetAll), new { id = newUser.Id }, newUser);
    }
}
