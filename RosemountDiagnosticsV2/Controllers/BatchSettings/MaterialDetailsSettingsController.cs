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
using Microsoft.AspNetCore.Authorization;

namespace RosemountDiagnosticsV2.Controllers.BatchSettings
{
    public class MaterialDetailsSettingsController : Controller
    {
        private readonly BatchContext _context;
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public MaterialDetailsSettingsController(BatchContext context, IMaterialDetailsRepository materialDetailsRepository)
        {
            _context = context;
            _materialDetailsRepository = materialDetailsRepository;
        }

        // GET: MaterialDetailsSettings
        public IActionResult Index()
        {
           return View(_materialDetailsRepository.GetAllMaterialDetails().Where(x => x.NeedsDetailsInput == true).ToList());
        }

        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var materialDetails = await _materialDetailsRepository.FindAsync(id);
            if (materialDetails == null)
            {
                return NotFound();
            }
            return View(materialDetails);
        }

        // POST: MaterialDetailsSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaterialDetailsId,Name,ShortName,ProductCode,AvgWeighTime,AvgWaitTime,CostPerTon,IncludeInMatVar,ParallelWeighGroup,ParallelGroupOrder,IsActive,NeedsDetailsInput,StartDate,EndDate")] MaterialDetails materialDetails)
        {
            if (id != materialDetails.MaterialDetailsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _materialDetailsRepository.Update(materialDetails);
                    await _materialDetailsRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialDetailsExists(materialDetails.MaterialDetailsId))
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
            return View(materialDetails);
        }      

        private bool MaterialDetailsExists(int id)
        {
            return _materialDetailsRepository.Any(id);
        }
    }
}
