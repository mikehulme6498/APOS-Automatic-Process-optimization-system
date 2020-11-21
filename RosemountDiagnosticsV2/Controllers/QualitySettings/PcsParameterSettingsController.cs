using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Models;
using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RosemountDiagnosticsV2.Controllers.QualitySettings
{
    public class PcsParameterSettingsController : Controller
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;
        private readonly IPcsWeightParameterRepository _pcsWeightParameterRepository;

        public PcsParameterSettingsController(IMaterialDetailsRepository materialDetailsRepository, IPcsWeightParameterRepository pcsWeightParameterRepository)
        {
            _materialDetailsRepository = materialDetailsRepository;
            _pcsWeightParameterRepository = pcsWeightParameterRepository;
        }

        // GET: PcsParameterSettings
        public IActionResult Index()
        {
            return View(_pcsWeightParameterRepository.GetAllParameters());
        }

       [Authorize]
        // GET: PcsParameterSettings/Create
        public IActionResult Create()
        {
            ViewBag.Materials = _materialDetailsRepository.GetListOfActiveMaterialsForDropDown();
            return View();
        }

        // POST: PcsParameterSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PcsWeightParametersId,RecipeType,Parameter")] PcsWeightParameters pcsWeightParameters)
        {
            var values = Enum.GetValues(typeof(RecipeTypes));

            if (ModelState.IsValid)
            {
                if (pcsWeightParameters.RecipeType == RecipeTypes.All)
                {
                    foreach (var enumValue in values)
                    {
                        if (enumValue.ToString() != "All")
                        {
                            var TempPcsWeightParameters = new PcsWeightParameters()
                            {
                                RecipeType = (RecipeTypes)Enum.Parse(typeof(RecipeTypes), enumValue.ToString()),
                                Parameter = pcsWeightParameters.Parameter
                            };                            
                            _pcsWeightParameterRepository.Add(TempPcsWeightParameters);                            
                        }
                    }                    
                }
                else
                {
                    _pcsWeightParameterRepository.Add(pcsWeightParameters);
                }

                await _pcsWeightParameterRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pcsWeightParameters);
        }

        
        [Authorize]
        // GET: PcsParameterSettings/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var pcsWeightParameters = await _context.PcsParameters
            var pcsWeightParameters = _pcsWeightParameterRepository.GetAllParameters()
                .FirstOrDefault(m => m.PcsWeightParametersId == id);
            if (pcsWeightParameters == null)
            {
                return NotFound();
            }

            return View(pcsWeightParameters);
        }
        [Authorize]
        // POST: PcsParameterSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pcsWeightParameters = await _pcsWeightParameterRepository.FindAsync(id);
            _pcsWeightParameterRepository.Remove(pcsWeightParameters);
            await _pcsWeightParameterRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcsWeightParametersExists(int id)
        {
            return _pcsWeightParameterRepository.Any(id);
        }
    }
}
