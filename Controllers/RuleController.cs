using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatBot.Models;
using System.Text;
using RestSharp;
using DetectLanguage;
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

[HttpGet("ruleshindi{query}")]
public IActionResult RulesHindi(string query)
{
    var translationService = new LanguageService();
    string query1 = translationService.Translate(query, "hi", "en");
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    Console.WriteLine(query);
    // translationService.Detect(query);
    string[] keywords=query1.Split(' ');
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

    top3Results[0].Content=translationService.Translate(top3Results[0].Content, "en", "hi");
    top3Results[0].Title=translationService.Translate(top3Results[0].Title, "en", "hi");
    Console.WriteLine(top3Results.Count());
    
     return Ok(top3Results);
}

[HttpGet("rulesmulti{query}")]
public IActionResult RulesMulti(string query)
{   
    var aa=new AsyncAwait();
    Task<List<Rule>> task=aa.AsaW(query,_context);
    var top3=task.Result;
    Console.WriteLine(top3.Count());
    return  Ok(top3);
    
}

}

public class AsyncAwait
{public async Task<List<Rule>> AsaW(string query,MyDbContext _context){
    var translationService = new LanguageService();
    Task<string> task=translationService.Detect(query);
    string languageCode=await task;
    Console.WriteLine(languageCode);
    string[] keywords=query.Split(' ');

    if (languageCode!="en"){
    string query1 = translationService.Translate(query, languageCode, "en");
    keywords=query1.Split(' ');
    }
    // Console.OutputEncoding = System.Text.Encoding.UTF8;
    var top3Results = _context.Rules.AsEnumerable()
    .Select(r => new
    {
       Rule = r,
        MatchCount = keywords.Count(keyword =>
            (r.Title != null && r.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            // (r.Content != null && r.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            (r.Source != null && r.Source.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
            (r.Category != null && r.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase))
        )
    })
    .Where(r => r.MatchCount > 0) // Filter rows with at least one keyword match
    .OrderByDescending(r => r.MatchCount)
    .Take(5) // Get the top 3 rows with the most keyword matches
    .Select(r => r.Rule)
    .ToList(); 

    if (languageCode!="en"){
    top3Results[0].Content=translationService.Translate(top3Results[0].Content, "en", languageCode);
    top3Results[0].Title=translationService.Translate(top3Results[0].Title, "en", languageCode);
    }

    Console.WriteLine(top3Results.Count());
    
     return top3Results;
}} 

public class LanguageService
{
    private const string MyMemoryApiUrl = "https://api.mymemory.translated.net/get";

    public string Translate(string text, string sourceLanguage, string targetLanguage)
    {
        var client = new RestClient(MyMemoryApiUrl);
        var request = new RestRequest();
        request.Method = Method.Get;

        // Prepare the query parameters
        request.AddParameter("q", text);
        request.AddParameter("langpair", $"{sourceLanguage}|{targetLanguage}");

        // Make the API request
        var response = client.Execute(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            // Parse the response JSON
            var responseBody = response.Content;
            dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody!)!;
            return responseJson.responseData.translatedText;
        }
        else
        {
            Console.WriteLine($"Error: {response.ErrorMessage}");
            return null!;
        }
    }
    public async Task<string> Detect(string text){
        DetectLanguageClient client = new DetectLanguageClient("ae5fe18158571bd7a684bb04b06ee911");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddParameter("q", text);
        string languageCode = await client.DetectCodeAsync(text);
        // Console.WriteLine(languageCode);
        return languageCode;
    }
}