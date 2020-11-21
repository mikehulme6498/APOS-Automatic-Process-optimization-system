using BatchDataAccessLibrary.DataAccess;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class MaterialDetailsRepository : IMaterialDetailsRepository
    {
        private readonly BatchContext _context;

        public MaterialDetailsRepository(BatchContext context)
        {
            _context = context;
        }

        public List<string> GetMaterialNamesThatAreIncludedInMatVar()
        {
            return _context.MaterialDetails.Where(x => x.IncludeInMatVar == true).Select(x => x.Name).ToList();
        }

        public double GetAverageWeighTimeOfMaterial(string name)
        {
            return _context.MaterialDetails.Where(x => x.Name == name).Select(x => x.AvgWeighTime).FirstOrDefault();
        }
        public double GetAverageWaitTimeOfMaterial(string name)
        {
            return _context.MaterialDetails.Where(x => x.Name == name).Select(x => x.AvgWeighTime).FirstOrDefault();
        }
        public List<MaterialDetails> GetAllMaterialDetails()
        {
            return _context.MaterialDetails.ToList();
        }

        public List<string> GetMaterialNames()
        {
            return _context.MaterialDetails.OrderBy(x => x.Name).Select(x => x.Name).ToList();
        }

        public List<string> GetMaterialNamesByShortName()
        {
            return _context.MaterialDetails.OrderBy(x => x.ShortName).Select(x => x.ShortName).ToList();
        }

        public MaterialDetails GetSingleMaterial(string name)
        {
            return _context.MaterialDetails.Where(x => x.Name == name).FirstOrDefault();
        }

        public MaterialDetails GetSingleMaterial(int productCode)
        {
            return _context.MaterialDetails.Where(x => x.ProductCode == productCode).FirstOrDefault();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void LoadMaterialData()
        {
            //List<MaterialDetails> materials;

            //string filePath = Environment.CurrentDirectory;
            //string json = File.ReadAllText(filePath + @"\MaterialInfo.Json");
            //materials = JsonConvert.DeserializeObject<List<MaterialDetails>>(json);
            //_context.MaterialDetails.AddRange(materials);
            //_context.SaveChanges();
        }

        public double GetCostMaterialLoss(string material, double amountUnderOver)
        {
            double costPerKg = _context.MaterialDetails.Where(x => x.Name == material).Select(x => x.CostPerTon).FirstOrDefault() / 1000;
            return Math.Round(costPerKg * amountUnderOver, 2);
        }

        public bool IsIncludedInMatVar(string name)
        {
            return _context.MaterialDetails.Where(x => x.Name == name).Select(x => x.IncludeInMatVar).FirstOrDefault();
        }

        public MaterialDetails AddNewFoundMaterial(MaterialDetails newMaterial)
        {
            newMaterial.NeedsDetailsInput = true;
            newMaterial.IsActive = true;
            _context.MaterialDetails.Add(newMaterial);
            _context.SaveChanges();
            return newMaterial;
        }

        public List<MaterialDetails> GetMaterialsThatNeedDetailsEntering()
        {
            return _context.MaterialDetails.Where(x => x.NeedsDetailsInput == true).ToList();
        }

        public async Task<MaterialDetails> FindAsync(int? id)
        {
            return await _context.MaterialDetails.FindAsync(id);
        }

        public EntityEntry<MaterialDetails> Update(MaterialDetails materialDetails)
        {
            return _context.Update(materialDetails);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.MaterialDetails.Any(x => x.MaterialDetailsId == id);
        }

        public List<SelectListItem> GetListOfActiveMaterialsForDropDown()
        {
            List<SelectListItem> output = new List<SelectListItem>();

            foreach (var material in _context.MaterialDetails.Where(x => x.IsActive == true).OrderBy(x => x.ShortName).ToList())
            {
                output.Add(new SelectListItem { Text = material.ShortName, Value = material.Name });
            }
            return output;
        }
    }
}
