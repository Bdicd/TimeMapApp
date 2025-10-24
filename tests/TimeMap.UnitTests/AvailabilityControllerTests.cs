using Microsoft.AspNetCore.Mvc;
using TimeMap.API.Controllers;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.UnitTests
{
    class InMemoryAvailabilityRepoForController : IAvailabilityRepository
    {
        private readonly List<Availability> _items = new();
        public void Add(Availability availability) => _items.Add(availability);
        public List<Availability> GetAll() => _items;
        public List<Availability> GetByUserId(Guid userId) => _items.FindAll(a => a.UserId == userId);
    }

    class InMemoryUserRepoForAvailController : IUserRepository
    {
        private readonly List<User> _items = new();
        public void Add(User user) => _items.Add(user);
        public List<User> GetAll() => _items;
        public User? GetById(Guid id) => _items.Find(u => u.Id == id);
    }

    public class AvailabilityControllerTests
    {
        [Fact]
        public void GetAll_ReturnsAvailabilities()
        {
            var repo = new InMemoryAvailabilityRepoForController();
            repo.Add(new Availability { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), StartTimeUtc = DateTime.UtcNow, EndTimeUtc = DateTime.UtcNow.AddHours(1) });
            var ctl = new AvailabilityController(repo, new InMemoryUserRepoForAvailController());
            var res = ctl.GetAll();
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            var list = Assert.IsType<List<Availability>>(ok.Value);
            Assert.Single(list);
        }

        [Fact]
        public void AddAvailability_UserNotFound_ReturnsNotFound()
        {
            var repo = new InMemoryAvailabilityRepoForController();
            var urepo = new InMemoryUserRepoForAvailController();
            var ctl = new AvailabilityController(repo, urepo);
            var res = ctl.AddAvailability(Guid.NewGuid(), "p", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            Assert.IsType<NotFoundObjectResult>(res);
        }

        [Fact]
        public void AddAvailability_WrongPassword_ReturnsUnauthorized()
        {
            var repo = new InMemoryAvailabilityRepoForController();
            var urepo = new InMemoryUserRepoForAvailController();
            var uid = Guid.NewGuid();
            urepo.Add(new User { Id = uid, Name = "n", Password = "correct" });
            var ctl = new AvailabilityController(repo, urepo);
            var res = ctl.AddAvailability(uid, "wrong", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            Assert.IsType<UnauthorizedObjectResult>(res);
        }

        [Fact]
        public void AddAvailability_Succeeds_AddsAvailability()
        {
            var repo = new InMemoryAvailabilityRepoForController();
            var urepo = new InMemoryUserRepoForAvailController();
            var uid = Guid.NewGuid();
            urepo.Add(new User { Id = uid, Name = "n", Password = "p" });
            var ctl = new AvailabilityController(repo, urepo);
            var res = ctl.AddAvailability(uid, "p", DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
            var ok = Assert.IsType<OkObjectResult>(res);
            var avail = Assert.IsType<Availability>(ok.Value);
            Assert.Single(repo.GetAll());
            Assert.Equal(uid, avail.UserId);
        }
    }
}
