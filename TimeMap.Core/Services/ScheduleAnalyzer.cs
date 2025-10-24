using TimeMap.Core.Interfaces;
using TimeMap.Domain.Entities;
using TimeMap.Domain.Models;

namespace TimeMap.Core.Services;

public class ScheduleAnalyzer(IAvailabilityRepository availabilityRepository)
{

    public List<BestSlot> FindBestCommonSlots(DateTime startUtc, DateTime endUtc)
    {
        var relevantAvailabilities = availabilityRepository
            .GetAll()
            .Where(a => a.EndTimeUtc >= startUtc && a.StartTimeUtc <= endUtc)
            .ToList();

        if (relevantAvailabilities.Count == 0)
            return [];

        var allUserIds = relevantAvailabilities
            .Select(a => a.UserId)
            .Distinct()
            .ToList();

        int threshold = (int)Math.Floor(allUserIds.Count / 2.0) + 1; // "строго больше половины"


        List<BestSlot> bestSlots = CheckIntersections(relevantAvailabilities, startUtc, endUtc, threshold);


        return MergeNearSlots(bestSlots);
    }

    private static List<BestSlot> CheckIntersections(List<Availability> relevantAvailabilities, DateTime startUtc, DateTime endUtc, int threshold)
    {

        TimeSpan step = TimeSpan.FromMinutes(15);
        List<BestSlot> bestSlots = [];
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
        return bestSlots;
    }
    private static List<BestSlot> MergeNearSlots(List<BestSlot> slots)
    {
        List<BestSlot> merged = [];
        foreach (var slot in slots.OrderBy(s => s.StartUtc))
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
