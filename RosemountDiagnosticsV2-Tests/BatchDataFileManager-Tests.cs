namespace RosemountDiagnosticsV2_Tests
{

    //[TestClass]
    //public class BatchDataFileManager_Tests
    //{
    //    private readonly MockBatchReportRepository repo = new MockBatchReportRepository();
    //    private readonly BatchDataFileManager fileReader = new BatchDataFileManager(new MockMaterialDetailsRepository());
    //    private readonly string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\TestBatchFiles";

    //    [TestMethod]
    //    public void BigBangBatchShouldReturn5Vessels()
    //    {
    //        int expectedVesselCount = 5;
    //        BatchReport report = repo.GetBatchByBatchNumber("8396-1", 2020);
    //        Assert.AreEqual(expectedVesselCount, report.AllVessels.Count);
    //    }

    //    [TestMethod]
    //    public void ConcBatchShouldReturn3Vessels()
    //    {
    //        int expectedVesselCount = 3;
    //        BatchReport report = repo.GetBatchByBatchNumber("8316-37", 2020);
    //        Assert.AreEqual(expectedVesselCount, report.AllVessels.Count);
    //    }

    //    // TESTS FOR BATCHES BEING NOT VALID

    //    [TestMethod]
    //    public void ShouldReturnAnInvalidBatch()
    //    {
    //        var batchReport = CreateBatchReportFromFile("ShouldHaveOneConversionFault.txt");
    //        Assert.AreEqual(false, batchReport.IsValidBatch);
    //    }

    //    [TestMethod]
    //    public void ShouldReturnAnInvalidBatchDueToMissingVessel()
    //    {
    //        var batchReport = CreateBatchReportFromFile("PreweigherTitleRemoved.txt");
    //        Assert.AreEqual(false, batchReport.IsValidBatch);
    //    }
    //    [TestMethod]
    //    public void ShouldReturnAnInvalidBatchDueToMostOfReportMissing()
    //    {
    //        var batchReport = CreateBatchReportFromFile("MostOfReportMissing.txt");
    //        Assert.AreEqual(false, batchReport.IsValidBatch);
    //    }
    //    [TestMethod]
    //    public void ShouldReturnAnInvalidBatchDueToEndOfReportMissing()
    //    {
    //        var batchReport = CreateBatchReportFromFile("EndOfReportMissing.txt");
    //        Assert.AreEqual(false, batchReport.IsValidBatch);
    //    }

    //    // END OF TESTS FOR INVALID BATCHES

    //    [TestMethod]
    //    public void ReportShouldbeStream1()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream1.txt");
    //        Assert.AreEqual("Stream 1", batchReport.StreamName);
    //    }

    //    [TestMethod]
    //    public void ReportShouldbeStream2()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream2.txt");
    //        Assert.AreEqual("Stream 2", batchReport.StreamName);
    //    }
    //    [TestMethod]
    //    public void ReportShouldbeStream3()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream3.txt");
    //        Assert.AreEqual("Stream 3", batchReport.StreamName);
    //    }
    //    [TestMethod]
    //    public void ReportShouldbeStream11()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream11.txt");
    //        Assert.AreEqual("Stream 11", batchReport.StreamName);
    //    }
    //    [TestMethod]
    //    public void ReportShouldbeStream21()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream21.txt");
    //        Assert.AreEqual("Stream 21", batchReport.StreamName);
    //    }

    //    [TestMethod] //16
    //    public void CheckMetaDataShouldPass()
    //    {
    //        var batchReport = CreateBatchReportFromFile("CheckMetaData.txt");
    //        Assert.AreEqual("WHTCON", batchReport.Recipe);
    //        Assert.AreEqual(6965, batchReport.Campaign);
    //        Assert.AreEqual(2, batchReport.BatchNo);
    //        Assert.AreEqual(RecipeTypes.Conc, batchReport.RecipeType);
    //        Assert.AreEqual(60.6, batchReport.MakingTime);
    //        Assert.AreEqual(60.6, batchReport.NewMakeTime);
    //        Assert.AreEqual(28.0, batchReport.Visco);
    //        Assert.AreEqual(2.3, batchReport.Ph);
    //        Assert.AreEqual("V601", batchReport.AllocatedTo);
    //        Assert.AreEqual(15.013, batchReport.TotalRecipeWeight);
    //        Assert.AreEqual(14.970, batchReport.TotalActualWeight);
    //        Assert.AreEqual(15.101, batchReport.VesselWeightIncrease);
    //    }

    //    [TestMethod]
    //    public void ReportFileNameShouldBeStream1()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream1.txt");
    //        Assert.AreEqual("Stream1.txt", batchReport.FileName);
    //    }

    //    [TestMethod]
    //    public void ReportFileNameShouldBePreWeigherTitleRemoved()
    //    {
    //        var batchReport = CreateBatchReportFromFile("PreweigherTitleRemoved.txt");
    //        Assert.AreEqual("PreweigherTitleRemoved.txt", batchReport.FileName);
    //    }


    //    [TestMethod]
    //    public void MainMixerShouldHave16Materials()
    //    {
    //        var batchReport = CreateBatchReportFromFile("CheckMetaData.txt");
    //        var mainMixer = batchReport.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();
    //        Assert.AreEqual(16, mainMixer.Materials.Count());
    //    }

    //    [TestMethod]
    //    public void ActivePreWeigherShouldHave1Materials()
    //    {
    //        var batchReport = CreateBatchReportFromFile("CheckMetaData.txt");
    //        var activePreweigher = batchReport.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.ActivePreWeigher).FirstOrDefault();
    //        Assert.AreEqual(1, activePreweigher.Materials.Count());
    //    }

    //    [TestMethod]
    //    public void PerfumePreWeigherShouldHave2Materials()
    //    {
    //        var batchReport = CreateBatchReportFromFile("CheckMetaData.txt");
    //        var perfumePreweigher = batchReport.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.PerfumePreWeigher).FirstOrDefault();
    //        Assert.AreEqual(2, perfumePreweigher.Materials.Count());
    //    }

    //    [TestMethod]
    //    public void RecipeTypeShouldbeConc()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream1.txt");
    //        Assert.AreEqual(RecipeTypes.Conc, batchReport.RecipeType);
    //    }

    //    [TestMethod]
    //    public void RecipeTypeShouldbeBigBang()
    //    {
    //        var batchReport = CreateBatchReportFromFile("Stream11.txt");
    //        Assert.AreEqual(RecipeTypes.BigBang, batchReport.RecipeType);
    //    }


    //    [TestMethod]
    //    public void RecipeTypeShouldbeReg()
    //    {
    //        var batchReport = CreateBatchReportFromFile("RegBatch.txt");
    //        Assert.AreEqual(RecipeTypes.Reg, batchReport.RecipeType);
    //    }

    //    [TestMethod]
    //    public void BatchListShouldBeEmpty()
    //    {
    //        List<BatchReport> batchReport = fileReader.ProcessStringIntoBatchReports("BKABKA");

    //        Assert.AreEqual(0, batchReport.Count);
    //    }
    //    private BatchReport CreateBatchReportFromFile(string fileName)
    //    {
    //        var path = string.Format("{0}\\{1}", directory, fileName);
    //        var textFromFile = File.ReadAllText(path);
    //        textFromFile += "Filename : " + fileName;
    //        return fileReader.ProcessStringIntoBatchReports(textFromFile).FirstOrDefault();
    //    }

    //    [TestMethod]
    //    public void MainMixerShouldContainHotWater()
    //    {
    //        var batchReport = CreateBatchReportFromFile("HotWaterMissing.txt");
    //        Vessel mainMixer = batchReport.AllVessels.Where(x => x.VesselType == VesselTypes.MainMixer).First();
    //        Material water = mainMixer.Materials.Where(x => x.Name == "HOT WTR").FirstOrDefault();
    //        // 11,294.7

    //        Assert.AreEqual(true, mainMixer.Materials.Exists(x => x.Name == "HOT WTR"));
    //        Assert.AreEqual(3747.3, water.ActualWeight, 0.1);
    //        Assert.AreEqual(true, batchReport.IsBatchAdjusted);
    //        Assert.AreEqual(1, batchReport.BatchIssues
    //            .Where(x => x.FaultType == BatchIssue.FaultTypes.BatchAdjusted)
    //            .ToList()
    //            .Count());
    //    }

    //    [TestMethod]
    //    public void MainMixerShouldContainColdWater()
    //    {
    //        var batchReport = CreateBatchReportFromFile("ColdWaterMissing.txt");
    //        Vessel mainMixer = batchReport.AllVessels.Where(x => x.VesselType == VesselTypes.MainMixer).First();
    //        Material water = mainMixer.Materials.Where(x => x.Name == "COLD WTR").FirstOrDefault();
    //        // 5622.7
    //        Assert.AreEqual(true, mainMixer.Materials.Exists(x => x.Name == "COLD WTR"));
    //        Assert.AreEqual(9419.3, water.ActualWeight, 0.1);
    //        Assert.AreEqual(true, batchReport.IsBatchAdjusted);
    //        Assert.AreEqual(1, batchReport.BatchIssues
    //           .Where(x => x.FaultType == BatchIssue.FaultTypes.BatchAdjusted)
    //           .ToList()
    //           .Count());
    //    }



    //  }
}
