using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeMap.Core.Models;

public class BestSlot
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public List<Guid> UserIds { get; init; } = [];
}
