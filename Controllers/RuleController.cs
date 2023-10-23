using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatBot.Models;
namespace ChatBot.Data;


[Route("api/rules")]
[ApiController]
public class RuleController : ControllerBase
{
    private readonly MyDbContext _context; // Replace with your database context

    public RuleController(MyDbContext context)
    {
        _context = context;
    }

[HttpGet("by-title")]
public ActionResult<IEnumerable<Rule>> GetRulesByTitle(string title)
{
    var rules = _context.Rules.Where(r => r.Title.Contains(title)).ToList();
    return Ok(rules);
}

[HttpGet("by-category")]
public ActionResult<IEnumerable<Rule>> GetRulesByCategory(string category)
{
    var rules = _context.Rules.Where(r => r.Category == category).ToList();
    return Ok(rules);
}

[HttpGet("{id}")]
public ActionResult<Rule> GetRule(int id)
{
    var rule = _context.Rules.FirstOrDefault(r => r.RuleId == id);

    if (rule == null)
    {
        return NotFound();
    }

    return Ok(rule);
}

// [HttpGet("searchrules{keyword}")]
// public IActionResult SearchRules(string keyword)
// {
//     var rules = _context.Rules.Where(r => r.Title.Contains(keyword) || r.Content.Contains(keyword)).ToList();
//     return Ok(rules);
// }

[HttpGet("searchrules{query}")]
public IActionResult SearchRules(string query)
{
    string[] keywords=query.Split(' ');
    var top3Results = _context.Rules.AsEnumerable()
    .Select(r => new
    {
       Rule = r,
        MatchCount = keywords.Count(keyword =>
            (r.Title != null && r.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            (r.Content != null && r.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            (r.Source != null && r.Source.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        )
    })
    .Where(r => r.MatchCount > 0) // Filter rows with at least one keyword match
    .OrderByDescending(r => r.MatchCount)
    .Take(3) // Get the top 3 rows with the most keyword matches
    .Select(r => r.Rule)
    .ToList(); 
     return Ok(top3Results);
}
}
