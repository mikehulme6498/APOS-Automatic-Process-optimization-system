using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Interfaces
{
    public interface IMaterialDetailsRepository
    {
        List<string> GetMaterialNamesThatAreIncludedInMatVar();
        double GetAverageWeighTimeOfMaterial(string name);
        double GetAverageWaitTimeOfMaterial(string name);
        List<MaterialDetails> GetAllMaterialDetails();
        MaterialDetails GetSingleMaterial(string name);
        MaterialDetails GetSingleMaterial(int productCode);
        bool IsIncludedInMatVar(string name);
        double GetCostMaterialLoss(string material, double amountUnderOver);
        List<string> GetMaterialNames();
        List<string> GetMaterialNamesByShortName();
        MaterialDetails AddNewFoundMaterial(MaterialDetails material);
        List<MaterialDetails> GetMaterialsThatNeedDetailsEntering();
        List<SelectListItem> GetListOfActiveMaterialsForDropDown();
        Task<MaterialDetails> FindAsync(int? id);
        EntityEntry<MaterialDetails> Update(MaterialDetails materialDetails);
        void LoadMaterialData();
        void SaveChanges();
        bool Any(int id);
        Task SaveChangesAsync();
    }
}
