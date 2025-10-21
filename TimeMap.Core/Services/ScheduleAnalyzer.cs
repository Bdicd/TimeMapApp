using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMap.Domain.Entities;
using TimeMap.Core.Interfaces;
using TimeMap.Core.Models;

namespace TimeMap.Core.Services;

public class ScheduleAnalyzer(IAvailabilityRepository availabilityRepository)
{
    private readonly IAvailabilityRepository _availabilityRepository = availabilityRepository;


    List<BestSlot> FindBestCommonSlots(DateTime start, DateTime end)
    {
        var allAvailabilities = _availabilityRepository.GetAll();
        var relevantAvailabilities = allAvailabilities
                .Where(a => a.EndTimeUtc >= start && a.StartTimeUtc <= end)
                .ToList();
        return new List<BestSlot>();
    }
}
