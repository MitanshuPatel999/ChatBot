using System;
using System.Collections.Generic;

namespace ChatBot.Models;

public partial class UserRulesFavorite
{
    public int FavoriteId { get; set; }

    public int? UserId { get; set; }

    public int? RuleId { get; set; }

    public virtual Rule? Rule { get; set; }

    public virtual User? User { get; set; }
}
