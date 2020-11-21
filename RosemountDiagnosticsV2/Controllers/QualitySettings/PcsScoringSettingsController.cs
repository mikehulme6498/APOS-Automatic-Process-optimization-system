using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Models;
using RosemountDiagnosticsV2.View_Models.Quality;

namespace RosemountDiagnosticsV2.Controllers.QualitySettings
{
    public class PcsScoringSettingsController : Controller
    {
        private readonly BatchContext _context;

        public PcsScoringSettingsController(BatchContext context)
        {
            _context = context;
        }

        // GET: PcsScoringSettings
        public async Task<IActionResult> Index()
        {
            PcsScoringSettingsViewModel pcsScoringSettingsViewModel = new PcsScoringSettingsViewModel
            {
                ScoringParams = await _context.PcsScoringsTargets.FirstAsync(),
                ToleranceParams = await _context.PcsToleranceParameters.FirstAsync()
            };
            return View(pcsScoringSettingsViewModel);
        }

        
        // GET: PcsScoringSettings/Create
       
        // GET: PcsScoringSettings/Edit/5
        public async Task<IActionResult> EditPcsScoring(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsScoring = await _context.PcsScoringsTargets.FindAsync(id);
            if (pcsScoring == null)
            {
                return NotFound();
            }
            return View(pcsScoring);
        }

        // POST: PcsScoringSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPcsScoring(int id, [Bind("PcsScoringId,Score2Target,Score1Lower")] PcsScoring pcsScoring)
        {
            if (id != pcsScoring.PcsScoringId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pcsScoring);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcsScoringExists(pcsScoring.PcsScoringId))
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
            return View(pcsScoring);
        }

       
        private bool PcsScoringExists(int id)
        {
            return _context.PcsScoringsTargets.Any(e => e.PcsScoringId == id);
        }

        public async Task<IActionResult> EditTolerances(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsTolerance = await _context.PcsToleranceParameters.FindAsync(id);
            if (pcsTolerance == null)
            {
                return NotFound();
            }
            return View(pcsTolerance);
        }

        // POST: PcsScoringSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTolerances(int id, [Bind("PcsToleranceParametersId,TolerancePercent,OutOfRangePercent")] PcsToleranceParameters pcsTolerance)
        {
            if (id != pcsTolerance.PcsToleranceParametersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pcsTolerance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcsToleranceExists(pcsTolerance.PcsToleranceParametersId))
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
            return View(pcsTolerance);
        }


        private bool PcsToleranceExists(int id)
        {
            return _context.PcsToleranceParameters.Any(e => e.PcsToleranceParametersId == id);
        }
    }
}
