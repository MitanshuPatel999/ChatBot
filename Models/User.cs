using System;
using System.Collections.Generic;

namespace ChatBot.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<UserRulesFavorite> UserRulesFavorites { get; set; } = new List<UserRulesFavorite>();
}
