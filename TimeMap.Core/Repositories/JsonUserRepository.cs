using System.Text.Json;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.Core.Repositories;

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
            return [];

        var json = File.ReadAllText(_filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json);
        return users ?? [];
    }

    private void SavetoJson()
    {
        var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Add(User user)
    {
        _users.Add(user);
        SavetoJson();
    }

    public List<User> GetAll()
    {
        return _users;
    }

    public User? GetById(Guid id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
}
