using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TimeMap.Application.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.Infrastructure.Repositories;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly List<User> _users;

    public JsonUserRepository()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "users.json");
        _users = LoadFromJson();

    }

    private List<User> LoadFromJson()
    {
        if (!File.Exists(_filePath))
            return new List<User>();

        var json = File.ReadAllText(_filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json);
        return users ?? new List<User>();
    }

    private void SavetoJson()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void AddUser(User user)
    {
        _users.Add(user);
        SavetoJson();
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }

    public User? GetUserById(Guid id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
}
