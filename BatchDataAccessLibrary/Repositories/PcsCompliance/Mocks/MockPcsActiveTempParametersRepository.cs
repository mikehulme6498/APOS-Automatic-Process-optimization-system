using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class MockPcsActiveTempParametersRepository : IPcsActiveTempParameters
    {
        List<PcsTempTargets> pcsTempTargets;

        public MockPcsActiveTempParametersRepository()
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
            return "[{'PcsTempTargetsId':1,'RecipeType':0,'Recipe':'BABYBEAR','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':2,'RecipeType':2,'Recipe':'BB-AQUA','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':3,'RecipeType':2,'Recipe':'BB-BLOSSOM','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':4,'RecipeType':2,'Recipe':'BB-BOUQUET','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':5,'RecipeType':2,'Recipe':'BB-COSMO','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':6,'RecipeType':2,'Recipe':'BB-ELXIR','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':7,'RecipeType':2,'Recipe':'BB-FIG','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':8,'RecipeType':2,'Recipe':'BB-GOA','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':9,'RecipeType':2,'Recipe':'BB-GOLD','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':10,'RecipeType':2,'Recipe':'BB-LAV','Target':51.50,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':11,'RecipeType':2,'Recipe':'BB-PASSION','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':12,'RecipeType':2,'Recipe':'BB-PETAL','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':13,'RecipeType':2,'Recipe':'BB-PURE','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':14,'RecipeType':2,'Recipe':'BB-RED','Target':51.00,'UpperLimit':52.50,'LowerLimit':49.50},{'PcsTempTargetsId':15,'RecipeType':2,'Recipe':'BB-SKY','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':16,'RecipeType':2,'Recipe':'BB-SUNBURS','Target':51.20,'UpperLimit':51.65,'LowerLimit':50.75},{'PcsTempTargetsId':17,'RecipeType':0,'Recipe':'BLUCON','Target':38.00,'UpperLimit':38.50,'LowerLimit':37.50},{'PcsTempTargetsId':18,'RecipeType':1,'Recipe':'BLUREG','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.05},{'PcsTempTargetsId':19,'RecipeType':0,'Recipe':'BOOSTBLUE','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':20,'RecipeType':0,'Recipe':'BOOSTPNK','Target':36.30,'UpperLimit':36.85,'LowerLimit':35.85},{'PcsTempTargetsId':21,'RecipeType':0,'Recipe':'BOOSTPUR','Target':36.00,'UpperLimit':36.45,'LowerLimit':35.55},{'PcsTempTargetsId':22,'RecipeType':0,'Recipe':'CASHMERE','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':23,'RecipeType':0,'Recipe':'COCONUT','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':24,'RecipeType':0,'Recipe':'COUTURE','Target':37.50,'UpperLimit':38.00,'LowerLimit':37.00},{'PcsTempTargetsId':25,'RecipeType':0,'Recipe':'HONEYCON','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':26,'RecipeType':0,'Recipe':'LAVCON','Target':37.00,'UpperLimit':37.45,'LowerLimit':36.55},{'PcsTempTargetsId':27,'RecipeType':0,'Recipe':'LAVSIM','Target':36.50,'UpperLimit':37.00,'LowerLimit':36.00},{'PcsTempTargetsId':28,'RecipeType':0,'Recipe':'LILYCON','Target':36.50,'UpperLimit':37.00,'LowerLimit':36.00},{'PcsTempTargetsId':29,'RecipeType':1,'Recipe':'LILYREG','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':30,'RecipeType':0,'Recipe':'ORANGECON','Target':37.50,'UpperLimit':37.95,'LowerLimit':37.05},{'PcsTempTargetsId':31,'RecipeType':0,'Recipe':'PUREPINK','Target':37.00,'UpperLimit':37.45,'LowerLimit':36.55},{'PcsTempTargetsId':32,'RecipeType':0,'Recipe':'PURETEAL','Target':37.00,'UpperLimit':38.00,'LowerLimit':36.00},{'PcsTempTargetsId':33,'RecipeType':0,'Recipe':'SUNCON','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':34,'RecipeType':1,'Recipe':'SUNREG','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':35,'RecipeType':0,'Recipe':'TEAGOLD','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':36,'RecipeType':0,'Recipe':'TEAGREEN','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':37,'RecipeType':0,'Recipe':'WATERLILY','Target':36.50,'UpperLimit':36.95,'LowerLimit':36.05},{'PcsTempTargetsId':38,'RecipeType':0,'Recipe':'WHTCON','Target':36.00,'UpperLimit':36.45,'LowerLimit':35.55},{'PcsTempTargetsId':39,'RecipeType':1,'Recipe':'WHTREG','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00},{'PcsTempTargetsId':39,'RecipeType':1,'Recipe':'DM-BLUREG','Target':43.50,'UpperLimit':43.70,'LowerLimit':43.00}]";
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
