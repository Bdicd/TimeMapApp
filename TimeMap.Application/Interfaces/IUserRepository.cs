using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMap.Domain.Entities;

namespace TimeMap.Application.Interfaces
{
    internal interface IUserRepository
    {
        void AddUser(User user);
        List<User> GetAllUsers();
        User? GetUserById(Guid id);

    }
}
