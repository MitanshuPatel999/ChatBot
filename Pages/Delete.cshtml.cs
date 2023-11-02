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
    public class DeleteModel : PageModel
    {
        private readonly ChatBot.Models.MyDbContext _context;

        public DeleteModel(ChatBot.Models.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Rules == null)
            {
                return NotFound();
            }
            var rule = await _context.Rules.FindAsync(id);

            if (rule != null)
            {
                Rule = rule;
                _context.Rules.Remove(Rule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
