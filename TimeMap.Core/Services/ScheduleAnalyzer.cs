using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMap.Domain.Entities;
using TimeMap.Core.Interfaces;
using TimeMap.Domain.Models;

namespace TimeMap.Core.Services;

public class ScheduleAnalyzer(IAvailabilityRepository availabilityRepository)
{

    public List<BestSlot> FindBestCommonSlots(DateTime startUtc, DateTime endUtc)
    {
        var allAvailabilities = availabilityRepository.GetAll();
        var relevantAvailabilities = allAvailabilities
                .Where(a => a.EndTimeUtc >= startUtc && a.StartTimeUtc <= endUtc)
                .ToList();
        // there must be some good logic 
        if (!relevantAvailabilities.Any())
            return new List<BestSlot>();

        var allUserIds = relevantAvailabilities
            .Select(a => a.UserId)
            .Distinct()
            .ToList();

        int threshold = (int)Math.Floor(allUserIds.Count / 2.0) + 1; // "строго больше половины"

        var bestSlots = new List<BestSlot>();
        TimeSpan step = TimeSpan.FromMinutes(15);

        for (DateTime slotStart = startUtc; slotStart < endUtc; slotStart += step)
        {
            DateTime slotEnd = slotStart + step;

            var freeUsers = relevantAvailabilities
                .Where(a => a.StartTimeUtc <= slotStart && a.EndTimeUtc >= slotEnd)
                .Select(a => a.UserId)
                .Distinct()
                .ToList();


            if (freeUsers.Count >= threshold)
            {
                bestSlots.Add(new BestSlot
                {
                    StartUtc = slotStart,
                    EndUtc = slotEnd,
                    UserIds = freeUsers
                });
            }
        }
        List<BestSlot> merged = [];
        foreach (var slot in bestSlots.OrderBy(s => s.StartUtc))
        {
            if (merged.Count == 0)
            {
                merged.Add(slot);
                continue;
            }

            var last = merged.Last();
            if (last.EndUtc == slot.StartUtc &&
                last.UserIds.OrderBy(x => x).SequenceEqual(slot.UserIds.OrderBy(x => x)))
            {
                // расширяем предыдущий слот
                merged[merged.Count - 1] = new BestSlot
                {
                    StartUtc = last.StartUtc,
                    EndUtc = slot.EndUtc,
                    UserIds = last.UserIds
                };
            }
            else
            {
                merged.Add(slot);
            }
        }

        return merged;
    }
}
