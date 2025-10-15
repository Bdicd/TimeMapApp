using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeMap.Application.Interfaces;
using TimeMap.Domain.Entities;

namespace TimeMap.Infrastructure.Repositories;

public class JsonAvailabilityRepository : IAvailabilityRepository
{
    private readonly string _filePath;
    private readonly List<Availability> _availabilities;

    public JsonAvailabilityRepository()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "availabilities.json");
        _availabilities = LoadFromJson();
    }

    private List<Availability> LoadFromJson()
    {
        if (!File.Exists(_filePath))
            return new List<Availability>();

        var json = File.ReadAllText(_filePath);
        var availabilities = JsonSerializer.Deserialize<List<Availability>>(json);
        return availabilities ?? new List<Availability>();
    }

    private void SaveToJson()
    {
        var json = JsonSerializer.Serialize(_availabilities, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void AddAvailability(Availability availability)
    {
        _availabilities.Add(availability);
        SaveToJson();
    }

    public List<Availability> GetAllAvailabilities()
    {
        return _availabilities;
    }

    public List<Availability> GetAvailabilitiesByUser(Guid userId)
    {
        return _availabilities.Where(a => a.UserId == userId).ToList();
    }
}
