using System;
using System.Collections.Generic;

namespace ChatBot.Models;

public partial class Rule
{
    public int RuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Category { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? PublicationDate { get; set; }

    public string? Source { get; set; }

    public virtual ICollection<Interpretation> Interpretations { get; set; } = new List<Interpretation>();

    public virtual ICollection<UserRulesFavorite> UserRulesFavorites { get; set; } = new List<UserRulesFavorite>();
}
