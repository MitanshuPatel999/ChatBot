using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatBot.Models;

namespace ChatBot.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ChatBot.Models.MyDbContext _context;

        public IndexModel(ChatBot.Models.MyDbContext context)
        {
            _context = context;
        }

        public IList<Rule> Rule { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Rules != null)
            {
                Rule = await _context.Rules.ToListAsync();
            }
        }
    }
}
