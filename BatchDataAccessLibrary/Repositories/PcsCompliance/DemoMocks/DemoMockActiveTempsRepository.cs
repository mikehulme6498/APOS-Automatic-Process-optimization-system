using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories.PcsCompliance.DemoMocks
{
    public class DemoMockActiveTempsRepository : IPcsActiveTempParameters
    {
        List<PcsTempTargets> pcsTempTargets;

        public DemoMockActiveTempsRepository()
        {
            pcsTempTargets = new List<PcsTempTargets>();
            CreatePcsTempTargets();
        }

        private void CreatePcsTempTargets()
        {
            pcsTempTargets = JsonConvert.DeserializeObject<List<PcsTempTargets>>(GetTemperatureJson());
        }

        private string GetTemperatureJson()
        {
            return "[{'PcsTempTargetsId':1,'RecipeType':0,'Recipe':'Recipe 1','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':2,'RecipeType':2,'Recipe':'BB-Recipe 2','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':3,'RecipeType':2,'Recipe':'BB-Recipe 3','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':4,'RecipeType':2,'Recipe':'BB-Recipe 4','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':5,'RecipeType':2,'Recipe':'BB-Recipe 5','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':6,'RecipeType':2,'Recipe':'BB-Recipe 6','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':7,'RecipeType':2,'Recipe':'BB-Recipe 7','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':8,'RecipeType':2,'Recipe':'BB-Recipe 8','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':9,'RecipeType':2,'Recipe':'BB-Recipe 9','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':10,'RecipeType':2,'Recipe':'BB-Recipe 10','Target':51.50,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':11,'RecipeType':2,'Recipe':'BB-Recipe 11','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':12,'RecipeType':2,'Recipe':'BB-Recipe 12','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':13,'RecipeType':2,'Recipe':'BB-Recipe 13','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':14,'RecipeType':2,'Recipe':'BB-Recipe 14','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':15,'RecipeType':2,'Recipe':'BB-Recipe 15','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':16,'RecipeType':2,'Recipe':'BB-Recipe 16','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':17,'RecipeType':0,'Recipe':'Recipe 17','Target':38.00,'UpperLimit':38.50,'LowerLimit':37.50},{'PcsTempTargetsId':18,'RecipeType':1,'Recipe':'RE-Recipe 18','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.05},{'PcsTempTargetsId':19,'RecipeType':0,'Recipe':'Recipe 19','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':20,'RecipeType':0,'Recipe':'Recipe 20','Target':36.30,'UpperLimit':36.85,'LowerLimit':35.85},{'PcsTempTargetsId':21,'RecipeType':0,'Recipe':'Recipe 21','Target':36.00,'UpperLimit':36.45,'LowerLimit':35.55},{'PcsTempTargetsId':22,'RecipeType':0,'Recipe':'Recipe 22','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':23,'RecipeType':0,'Recipe':'Recipe 23','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':24,'RecipeType':0,'Recipe':'Recipe 24','Target':37.50,'UpperLimit':38.00,'LowerLimit':37.00},{'PcsTempTargetsId':25,'RecipeType':0,'Recipe':'Recipe 25','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':26,'RecipeType':0,'Recipe':'Recipe 26','Target':37.00,'UpperLimit':37.45,'LowerLimit':36.55},{'PcsTempTargetsId':27,'RecipeType':0,'Recipe':'Recipe 27','Target':36.50,'UpperLimit':37.00,'LowerLimit':36.00},{'PcsTempTargetsId':28,'RecipeType':0,'Recipe':'Recipe 28','Target':36.50,'UpperLimit':37.00,'LowerLimit':36.00},{'PcsTempTargetsId':29,'RecipeType':1,'Recipe':'RE-Recipe 29','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':30,'RecipeType':0,'Recipe':'Recipe 30','Target':37.50,'UpperLimit':37.95,'LowerLimit':37.05},{'PcsTempTargetsId':31,'RecipeType':0,'Recipe':'Recipe 31','Target':37.00,'UpperLimit':37.45,'LowerLimit':36.55},{'PcsTempTargetsId':32,'RecipeType':0,'Recipe':'Recipe 32','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':33,'RecipeType':0,'Recipe':'Recipe 33','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':34,'RecipeType':1,'Recipe':'RE-Recipe 34','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':35,'RecipeType':0,'Recipe':'Recipe 35','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':36,'RecipeType':0,'Recipe':'Recipe 36','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':37,'RecipeType':0,'Recipe':'Recipe 37','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':38,'RecipeType':0,'Recipe':'Recipe 38','Target':36.00,'UpperLimit':36.45,'LowerLimit':35.55},{'PcsTempTargetsId':39,'RecipeType':1,'Recipe':'RE-Recipe 39','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':39,'RecipeType':1,'Recipe':'DM-RE-Recipe 18','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00}]"; 
        }

        public PcsTempTargets GetTargetsFor(string recipeName)
        {
            return pcsTempTargets.Where(x => x.Recipe == recipeName).FirstOrDefault();
        }

        public List<decimal> GetListOfDistinctTempsForRecipe(RecipeTypes recipeType)
        {
            return pcsTempTargets.Where(x => x.RecipeType == recipeType).Select(x => x.Target).Distinct().ToList();
        }

        public Dictionary<decimal, List<PcsTempTargets>> GetListOfTargetsSeperatedByTargetTemps(RecipeTypes recipeType)
        {
            var recipeByType = pcsTempTargets.Where(x => x.RecipeType == recipeType).ToList();
            var recipesSeperatedByTargets = recipeByType.GroupBy(x => x.Target).ToDictionary(group => group.Key, group => group.ToList());
            return recipesSeperatedByTargets;
        }

        public async Task<List<PcsTempTargets>> GetAllTempParameters()
        {
            Task<List<PcsTempTargets>> task = Task.Run(() =>
            {
                return pcsTempTargets.ToList();
            });

            return await task;
        }
    }
}
