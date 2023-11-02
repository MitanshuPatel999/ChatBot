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
    public class DetailsModel : PageModel
    {
        private readonly ChatBot.Models.MyDbContext _context;

        public DetailsModel(ChatBot.Models.MyDbContext context)
        {
            _context = context;
        }

      public Rule Rule { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Rules == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules.FirstOrDefaultAsync(m => m.RuleId == id);
            if (rule == null)
            {
                return NotFound();
            }
            else 
            {
                Rule = rule;
            }
            return Page();
        }
    }
}
