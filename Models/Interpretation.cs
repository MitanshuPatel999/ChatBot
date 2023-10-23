using System;
using System.Collections.Generic;

namespace ChatBot.Models;

public partial class Interpretation
{
    public int InterpretationId { get; set; }

    public int? RuleId { get; set; }

    public string InterpretationText { get; set; } = null!;

    public string? Author { get; set; }

    public virtual Rule? Rule { get; set; }
}
