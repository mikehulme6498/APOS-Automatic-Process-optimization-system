using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Models;

namespace RosemountDiagnosticsV2.Controllers.BatchSettings
{
    public class BatchReportSettingsController : Controller
    {
        private readonly BatchContext _context;

        public BatchReportSettingsController(BatchContext context)
        {
            _context = context;
        }

        // GET: BatchReportSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.BatchReports.Take(20).ToListAsync());
        }

        // GET: BatchReportSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchReport = await _context.BatchReports
                .FirstOrDefaultAsync(m => m.BatchReportId == id);
            if (batchReport == null)
            {
                return NotFound();
            }

            return View(batchReport);
        }

        // GET: BatchReportSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BatchReportSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatchReportId,OriginalReport,StreamName,Recipe,Campaign,BatchNo,WeekNo,StartTime,MakingTime,QATime,PreQaTemp,Visco,Ph,SG,Appearance,VisualColour,MeasuredColour,Odour,OverallQAStatus,StockTankAllocationTime,AllocatedTo,DropTime,TotalRecipeWeight,TotalActualWeight,VesselWeightIncrease,RecipeType,NewMakeTime")] BatchReport batchReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batchReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(batchReport);
        }

        // GET: BatchReportSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchReport = await _context.BatchReports.FindAsync(id);
            if (batchReport == null)
            {
                return NotFound();
            }
            return View(batchReport);
        }

        // POST: BatchReportSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatchReportId,OriginalReport,StreamName,Recipe,Campaign,BatchNo,WeekNo,StartTime,MakingTime,QATime,PreQaTemp,Visco,Ph,SG,Appearance,VisualColour,MeasuredColour,Odour,OverallQAStatus,StockTankAllocationTime,AllocatedTo,DropTime,TotalRecipeWeight,TotalActualWeight,VesselWeightIncrease,RecipeType,NewMakeTime")] BatchReport batchReport)
        {
            if (id != batchReport.BatchReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batchReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchReportExists(batchReport.BatchReportId))
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
            return View(batchReport);
        }

        // GET: BatchReportSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batchReport = await _context.BatchReports
                .FirstOrDefaultAsync(m => m.BatchReportId == id);
            if (batchReport == null)
            {
                return NotFound();
            }

            return View(batchReport);
        }

        // POST: BatchReportSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batchReport = await _context.BatchReports.FindAsync(id);
            _context.BatchReports.Remove(batchReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchReportExists(int id)
        {
            return _context.BatchReports.Any(e => e.BatchReportId == id);
        }
    }
}
