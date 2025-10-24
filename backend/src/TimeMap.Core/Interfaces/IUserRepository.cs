using TimeMap.Domain.Entities;

namespace TimeMap.Core.Interfaces;

public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
    User? GetById(Guid id);
}
