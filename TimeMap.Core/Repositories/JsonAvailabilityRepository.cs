using System.Text.Json;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.Core.Repositories;

public class JsonAvailabilityRepository : IAvailabilityRepository
{
    private readonly string _filePath;
    private readonly List<Availability> _availabilities;
    private readonly string _absolutePath = "Data/availabilities.json";

    public JsonAvailabilityRepository()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, _absolutePath);
        _availabilities = LoadFromJson();
    }

    public void Add(Availability availability)
    {
        _availabilities.Add(availability);
        SaveToJson();
    }

    public List<Availability> GetAll()
    {
        return _availabilities;
    }

    public List<Availability> GetByUserId(Guid userId)
    {
        return [.. _availabilities.Where(a => a.UserId == userId)];
    }

    private List<Availability> LoadFromJson()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = File.ReadAllText(_filePath);
        try
        {
            var availabilities = JsonSerializer.Deserialize<List<Availability>>(json);
            return availabilities ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error load from json: {ex.Message}");
            throw;
        }
    }

    private void SaveToJson()
    {
        var json = JsonSerializer.Serialize(_availabilities, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}
