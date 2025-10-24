using Microsoft.AspNetCore.Mvc;
using TimeMap.API.Controllers;
using TimeMap.API.Models.Requests;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.UnitTests
{
    class InMemoryUserRepoForController : IUserRepository
    {
        private readonly List<User> _items = new();
        public void Add(User user) => _items.Add(user);
        public List<User> GetAll() => _items;
        public User? GetById(Guid id) => _items.Find(u => u.Id == id);
    }

    public class UserControllerTests
    {
        [Fact]
        public void GetAll_ReturnsUsers()
        {
            var repo = new InMemoryUserRepoForController();
            repo.Add(new User { Id = Guid.NewGuid(), Name = "a", Password = "p" });
            var ctl = new UserController(repo);
            var res = ctl.GetAll();
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            var users = Assert.IsType<List<User>>(ok.Value);
            Assert.Single(users);
        }

        [Fact]
        public void AddUser_CreatesUser()
        {
            var repo = new InMemoryUserRepoForController();
            var ctl = new UserController(repo);
            var res = ctl.AddUser(new CreateUserRequest { Name = "n", Password = "p" });
            var created = Assert.IsType<CreatedAtActionResult>(res);
            var user = Assert.IsType<User>(created.Value);
            Assert.Equal("n", user.Name);
            Assert.Equal("p", user.Password);
            Assert.Single(repo.GetAll());
        }
    }
}
