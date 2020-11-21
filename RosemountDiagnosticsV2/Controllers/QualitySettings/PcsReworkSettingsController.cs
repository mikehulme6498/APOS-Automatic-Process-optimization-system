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
    public class PcsReworkSettingsController : Controller
    {
       
        private readonly IBatchRepository _batchRepository;
        private readonly IPcsReworkParameters _pcsReworkParameters;

        public PcsReworkSettingsController(IBatchRepository batchRepository, IPcsReworkParameters pcsReworkParameters)
        {
            _batchRepository = batchRepository;
            _pcsReworkParameters = pcsReworkParameters;
        }

        // GET: PcsReworkSettings
        public async Task<IActionResult> Index()
        {
            return View(_pcsReworkParameters.GetListOfAllReworkParemeters());
        }

        

        // GET: PcsReworkSettings/Create
        public IActionResult Create()
        {
            //List<SelectListItem> recipeNames = new List<SelectListItem>();
            //foreach (var recipeName in _batchRepository.GetAllRecipeNames())
            //{
            //    recipeNames.Add(new SelectListItem { Text = recipeName, Value = recipeName });
            //}
            ViewBag.Recipes = _batchRepository.GetRecipeNamesForDropDown();
            return View();
        }

        // POST: PcsReworkSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PcsReworkParametersId,RecipeName,TargetReworkAmount")] PcsReworkParameters pcsReworkParameters)
        {
            if (ModelState.IsValid)
            {
                _pcsReworkParameters.Add(pcsReworkParameters);
                await _pcsReworkParameters.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pcsReworkParameters);
        }

        // GET: PcsReworkSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsReworkParameters = await _pcsReworkParameters.FindAsync(id);
            if (pcsReworkParameters == null)
            {
                return NotFound();
            }
            return View(pcsReworkParameters);
        }

        // POST: PcsReworkSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PcsReworkParametersId,RecipeName,TargetReworkAmount")] PcsReworkParameters pcsReworkParameters)
        {
            if (id != pcsReworkParameters.PcsReworkParametersId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pcsReworkParameters.Update(pcsReworkParameters);
                    await _pcsReworkParameters.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcsReworkParametersExists(pcsReworkParameters.PcsReworkParametersId))
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
            return View(pcsReworkParameters);
        }

        // GET: PcsReworkSettings/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcsReworkParameters = _pcsReworkParameters.GetListOfAllReworkParemeters()
                .FirstOrDefault(m => m.PcsReworkParametersId == id);
            if (pcsReworkParameters == null)
            {
                return NotFound();
            }

            return View(pcsReworkParameters);
        }

        // POST: PcsReworkSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pcsReworkParameters = await _pcsReworkParameters.FindAsync(id);
            _pcsReworkParameters.Remove(pcsReworkParameters);
            await _pcsReworkParameters.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcsReworkParametersExists(int id)
        {
            return _pcsReworkParameters.Any(id);
        }
    }
}
