using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System.Collections.Generic;

namespace BatchDataAccessLibrary.FileReader
{
    public class MaterialsFound
    {
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public List<string> MaterialName { get; private set; } = new List<string>();

        public MaterialsFound(IMaterialDetailsRepository materialDetailsRepository)
        {
            _materialDetailsRepository = materialDetailsRepository;
        }
        public void AddNewMaterial(string name)
        {
            if (!MaterialName.Contains(name))
            {
                MaterialName.Add(name);
            }
        }

        public void AddNewMaterialsToDB()
        {
            foreach (var material in MaterialName)
            {
                if (!_materialDetailsRepository.GetMaterialNames().Contains(material))
                {
                    _materialDetailsRepository.AddNewFoundMaterial(new MaterialDetails
                    {
                        Name = material,
                        AvgWaitTime = 0,
                        AvgWeighTime = 0,
                        ProductCode = 0,
                        ShortName = "Unknown",
                        CostPerTon = 0,
                        IncludeInMatVar = false,
                        NeedsDetailsInput = true
                    });
                }
            }
        }
    }
}
