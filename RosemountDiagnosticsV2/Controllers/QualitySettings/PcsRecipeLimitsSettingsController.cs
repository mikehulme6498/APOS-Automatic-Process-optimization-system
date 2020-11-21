using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Models;

namespace RosemountDiagnosticsV2.Controllers.QualitySettings
{
    public class PcsRecipeLimitsSettingsController : Controller
    {
        private readonly BatchContext _context;

        public PcsRecipeLimitsSettingsController(BatchContext context)
        {
            _context = context;
        }

        // GET: PcsRecipeLimitsSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecipeLimits.ToListAsync());
        }

       

        // GET: PcsRecipeLimitsSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PcsRecipeLimitsSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeLimitsId,RecipeType,LimitTypes,Min,Max,GuageMax,Target,Tolerance")] RecipeLimits recipeLimits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeLimits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipeLimits);
        }

        // GET: PcsRecipeLimitsSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeLimits = await _context.RecipeLimits.FindAsync(id);
            if (recipeLimits == null)
            {
                return NotFound();
            }
            return View(recipeLimits);
        }

        // POST: PcsRecipeLimitsSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeLimitsId,RecipeType,LimitTypes,Min,Max,GuageMax,Target,Tolerance")] RecipeLimits recipeLimits)
        {
            if (id != recipeLimits.RecipeLimitsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeLimits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeLimitsExists(recipeLimits.RecipeLimitsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipeLimits);
        }

        
        private bool RecipeLimitsExists(int id)
        {
            return _context.RecipeLimits.Any(e => e.RecipeLimitsId == id);
        }
    }
}
