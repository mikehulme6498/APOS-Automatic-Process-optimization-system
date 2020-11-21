using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Interfaces;

namespace RosemountDiagnosticsV2.Controllers.QualitySettings
{
    public class PcsTempTargetsSettingsController : Controller
    {
        private readonly BatchContext _context;
        private readonly IPcsActiveTempParameters _pcsActiveTempParameters;

        public PcsTempTargetsSettingsController(BatchContext context, IPcsActiveTempParameters pcsActiveTempParameters)
        {
            _context = context;
            _pcsActiveTempParameters = pcsActiveTempParameters;
        }

        // GET: PcsTempTargetsSettings
        public async Task<IActionResult> Index()
        {
            
            return View(await _pcsActiveTempParameters.GetAllTempParameters());
            //return View(await _context.PcsTempsTargets.ToListAsync());
        }

        // GET: PcsTempTargetsSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsTempTargets = await _context.PcsTempsTargets
                .FirstOrDefaultAsync(m => m.PcsTempTargetsId == id);
            if (pcsTempTargets == null)
            {
                return NotFound();
            }

            return View(pcsTempTargets);
        }

        // GET: PcsTempTargetsSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PcsTempTargetsSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PcsTempTargetsId,RecipeType,Recipe,Target,UpperLimit,LowerLimit")] PcsTempTargets pcsTempTargets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pcsTempTargets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pcsTempTargets);
        }

        // GET: PcsTempTargetsSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsTempTargets = await _context.PcsTempsTargets.FindAsync(id);
            if (pcsTempTargets == null)
            {
                return NotFound();
            }
            return View(pcsTempTargets);
        }

        // POST: PcsTempTargetsSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PcsTempTargetsId,RecipeType,Recipe,Target,UpperLimit,LowerLimit")] PcsTempTargets pcsTempTargets)
        {
            if (id != pcsTempTargets.PcsTempTargetsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pcsTempTargets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcsTempTargetsExists(pcsTempTargets.PcsTempTargetsId))
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
            return View(pcsTempTargets);
        }

        // GET: PcsTempTargetsSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsTempTargets = await _context.PcsTempsTargets
                .FirstOrDefaultAsync(m => m.PcsTempTargetsId == id);
            if (pcsTempTargets == null)
            {
                return NotFound();
            }

            return View(pcsTempTargets);
        }

        // POST: PcsTempTargetsSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pcsTempTargets = await _context.PcsTempsTargets.FindAsync(id);
            _context.PcsTempsTargets.Remove(pcsTempTargets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcsTempTargetsExists(int id)
        {
            return _context.PcsTempsTargets.Any(e => e.PcsTempTargetsId == id);
        }
    }
}
