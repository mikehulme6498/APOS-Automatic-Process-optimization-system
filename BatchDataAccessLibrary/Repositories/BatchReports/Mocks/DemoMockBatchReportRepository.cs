using BatchDataAccessLibrary.FileReader;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchDataAccessLibrary.Repositories
{
    public class DemoMockBatchReportRepository : IBatchRepository
    {
        readonly List<BatchReport> allReports = new List<BatchReport>();
        private readonly IMaterialDetailsRepository _materialDetailsRepository;

        public IEnumerable<BatchReport> AllBatches => GetAllBatchReports();

        public DemoMockBatchReportRepository()
        {
            _materialDetailsRepository = new MockMaterialDetailsRepository();
            BatchFileDataManagerForDemo fileManager = new BatchFileDataManagerForDemo(_materialDetailsRepository);
            //string textFromFiles = GetDataFromTestFiles();
            //string text = TextFromFiles();
            //allReports = fileManager.ProcessStringIntoBatchReports(text);
            //SetBatchIdNumber();
        }

        private void SetBatchIdNumber()
        {
            for (int i = GetLastBatchId(); i < allReports.Count; i++)
            {
                allReports[i].BatchReportId = i;
            }
        }

        private int GetLastBatchId()
        {
            int lastId = 0;

            for (int i = 0; i < allReports.Count; i++)
            {
                if (allReports[i].BatchReportId == 0)
                {
                    if (i != 0)
                    {
                        lastId = allReports[i - 1].BatchReportId;
                        return lastId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return lastId;
        }

        public IEnumerable<BatchReport> GetAllBatchReports()
        {
            return allReports;
        }

        public BatchReport GetBatchByBatchNumber(string batchNum, int year)
        {
            int campaign = Convert.ToInt32(batchNum.Substring(0, batchNum.IndexOf('-')));
            int batch = Convert.ToInt32(batchNum.Substring(batchNum.IndexOf('-') + 1));
            return allReports.Where(x => x.Campaign == campaign && x.BatchNo == batch && x.StartTime.Year == year).FirstOrDefault();
        }

        public List<BatchReport> GetBatchesByWeek(int weekNum, int year)
        {
            return allReports.Where(x => x.WeekNo == weekNum && x.StartTime.Year == year).ToList();
        }
        public BatchReport GetBatchById(int id)
        {
            return allReports.Where(x => x.BatchReportId == id).FirstOrDefault();
        }

        public async Task<bool> BatchExists(int campaign, int batch, DateTime startDateTime)
        {
            BatchReport report = allReports.Where(x => x.Campaign == campaign && x.BatchNo == batch && x.StartTime == startDateTime).FirstOrDefault();
            return report != null;
        }

        public void Add(BatchReport report)
        {
            allReports.Add(report);
            SetBatchIdNumber();
        }

        public void AddRange(List<BatchReport> reports)
        {
            foreach (var report in reports)
            {
                allReports.Add(report);
            }
            SetBatchIdNumber();
        }

        public void AddConversionFaults(List<BatchConversionFault> faults)
        {
            return;
        }

        public void SaveChanges(BatchReport report)
        {
            if (report != null)
            {
                var reportToUpdate = allReports.Find(x => x.Campaign == report.Campaign && x.BatchNo == report.BatchNo);
                allReports.Remove(reportToUpdate);
                allReports.Add(report);
            }
        }
        public List<int> GetYearsInSystemForDropDown()
        {
            var years = allReports.OrderByDescending(x => x.StartTime.Year).Select(x => x.StartTime.Year).Distinct().ToList();
            List<int> yearsAvailable = new List<int>();
            foreach (var year in years)
            {
                yearsAvailable.Add(year);
            }
            return yearsAvailable;
        }

        public List<int> GetWeeksInSystemForDropDown(int year)
        {
            var weeks = allReports.Where(x => x.StartTime.Year == year)
                .OrderByDescending(x => x.StartTime.Year)
                .ThenByDescending(x => x.WeekNo)
                .Select(x => x.WeekNo)
                .Distinct()
                .ToList();
            return weeks;
        }

        public List<BatchReport> GetBatchesByYear(int year)
        {
            return allReports.Where(x => x.StartTime.Year == year).ToList();
        }

        public List<BatchReport> GetBatchesByDates(DateTime dateFrom, DateTime dateTo)
        {
            return allReports.Where(x => x.StartTime >= dateFrom && x.StartTime <= dateTo).ToList();
        }

        public List<string> GetAllRecipeNames()
        {
            return allReports
                .Select(x => x.Recipe)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public List<SelectListItem> GetRecipeNamesForDropDown()
        {
            List<SelectListItem> output = new List<SelectListItem>();
            foreach (var recipe in GetAllRecipeNames())
            {
                output.Add(new SelectListItem { Text = recipe, Value = recipe });
            }
            return output;
        }

        //private string GetDataFromTestFiles()
        //{
        //    string textfrombatches = null;
        //    string path = AppDomain.CurrentDomain.BaseDirectory;
        //    DirectoryInfo directoryInfo = new DirectoryInfo(path);
        //    DirectoryInfo[] folders = directoryInfo.GetDirectories();
        //    FileInfo[] files = null;

        //    files = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);

        //    foreach (var file in files)
        //    {
        //        string[] textfromfile = File.ReadAllLines(file.FullName);
        //        textfrombatches += textfromfile.StringArrayToString();
        //        textfrombatches += "NEWREPORT";
        //    }

        //    return textfrombatches;
        //}



        public string TextFromFiles()
        {
            return @"PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 32
            BATCH START TIME :      13:47:23 on 06-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       95.9 14:16:43    0.2 M    15.4 M      1.6    97.7      0.0       0.0    0.0 C

              Material 71      20.0       20.8 14:19:28    0.1 M     1.3 M     98.1   119.4      0.0       0.0    8.4 C

            Dump to MainMixer5 - decrease = 120.6 Kg Dump Time     2.2 M

            Material 49        15.0       15.9  14:44:02

            TOTAL VESSEL TIME:          53.3 Mins
            VESSEL COMPLETED AT:  14:44:17 ON 06-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1323.5 13:59:20    0.1 M     7.4 M     -4.9  1317.8     65.0     100.0   63.2 C

            Dump to MainMixer5 - decrease = 1320.6 Kg Dump Time     5.5 M

            TOTAL VESSEL TIME:          40.2 Mins
            VESSEL COMPLETED AT:  14:31:36 ON 06-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.4 14:07:05    0.2 M     1.1 M   3625.7  5122.5     20.2      11.0    0.0 C

                 Material 68      12.4       12.1 14:10:23    0.2 M     2.2 M   6531.9  9424.1     38.1      11.0    0.0 C

                 Material 63    5087.1     5119.7 14:12:14   13.8 M     6.6 M   3427.5 11272.7     41.7      11.0   80.0 C

            ManifoldFlush      15.0       14.8 14:12:30    0.2 M     1.1 M  10698.8 11384.8     41.5      11.0    0.0 C

                Material 64       7.5        7.8 14:14:43    0.2 M     1.3 M  11734.2 12325.0     39.0      11.0    0.0 C

            Flush      40.0       39.9 14:17:09    0.2 M     1.5 M  12716.8 13350.8     36.4      11.0    0.0 C

                Material 67    8127.8     8122.7 14:17:20    7.7 M    20.1 M     -0.2 13352.9     36.3      11.0    8.4 C

                     Material 59      12.4       12.1 14:19:55    0.2 M     1.9 M  13357.4 13376.4     36.3      11.0    0.0 C

               Material59 Flush      20.0       19.8 14:23:00    0.1 M     1.5 M  13376.8 13401.5     36.2      21.0    8.4 C

            Empty PreWeigher2 -   Increase = 1337.3 Kg          0.2 M     5.4 M                     38.1      21.0

               Material 54     150.0      124.4 14:32:56    0.1 M     0.8 M  14743.8 14872.3     38.4      21.0   80.1 C

            Material 53       3.7        3.5 14:35:47    0.2 M     1.8 M  14871.5 14878.1     38.5      21.0   50.6 C

            ManifoldFlush      15.0       15.9 14:37:43    0.2 M     1.0 M  14878.9 14890.4     38.5      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 116.5 Kg          0.3 M     2.0 M                     34.8      21.0

               Material 88       6.8        6.8 14:47:09    0.1 M     1.1 M  14817.1 14824.5     31.7      21.0    0.0 C

            Flush 1      40.0       39.8 14:49:07    0.1 M     1.3 M  14826.2 14860.4     30.3      21.0    0.0 C

            MAKING TIME:              63.2 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.3 C
            QA TIME:                  12.0 MINS
            ENTERED VISCOSITY:        31.0 cPoise
            ENTERED pH:                2.5
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:    77.8 Mins
            Allocated To:    Stocktank2   14.895 Te
            DROP TIME:                14.1 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   14.999 Te
            Vessel Weight Increase: 14.892 Te

            TOTAL BATCH TIME:         168.5 Mins
            BATCH COMPLETED AT:  16:36:10 ON 06-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 33
            BATCH START TIME :      16:38:20 on 06-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       95.9 16:59:21    0.2 M    16.0 M      1.1    97.2      0.0       0.0    0.0 C

              Material 71      20.0       19.7 17:01:42    0.1 M     1.3 M     97.6   117.9      0.0       0.0    8.5 C

            Dump to MainMixer5 - decrease = 120.8 Kg Dump Time     2.2 M

            Material 49        15.0       15.7  17:27:43

            TOTAL VESSEL TIME:          46.4 Mins
            VESSEL COMPLETED AT:  17:27:55 ON 06-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1341.1 16:47:44    0.1 M     5.1 M     -8.2  1333.1     64.8     100.0   62.1 C

            Dump to MainMixer5 - decrease = 1342.8 Kg Dump Time     5.4 M

            TOTAL VESSEL TIME:          30.0 Mins
            VESSEL COMPLETED AT:  17:11:55 ON 06-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.4 16:46:53    0.1 M     1.0 M   4061.2  5074.7     42.0      11.0    0.0 C

                 Material 68      12.4       12.5 16:49:27    0.1 M     1.9 M   5739.6  7665.7     45.2      11.0    0.0 C

            ManifoldFlush      15.0       14.1 16:51:06    0.1 M     0.9 M   8547.4  9530.0     47.3      11.0    0.0 C

                 Material 63    5120.2     5141.0 16:51:22    0.1 M     8.8 M    794.5  9642.5     47.2      11.0   80.0 C

                Material 64       7.5        6.6 16:53:38    0.1 M     1.8 M   9797.8 10327.6     44.2      11.0    0.0 C

            Flush      40.0       40.4 16:55:43    0.1 M     1.4 M  10569.9 11240.2     42.3      11.0    0.0 C

                     Material 59      12.4       12.4 16:58:16    0.1 M     1.8 M  11528.6 12325.4     39.4      11.0    0.0 C

                Material 67    8094.7     8088.8 17:01:24    0.1 M    20.9 M      8.0 13417.1     36.3      11.0    8.5 C

               Material59 Flush      20.0       19.9 17:04:01    0.1 M     1.4 M  13418.4 13446.8     36.2      21.0    8.5 C

            Empty PreWeigher2 -   Increase = 1359.5 Kg          0.2 M     5.2 M                     38.0      21.0

               Material 54     150.0      112.0 17:18:05    0.1 M     5.8 M  14807.2 14921.7     38.5      21.0   79.9 C

            Material 53       3.7        3.7 17:19:38    0.1 M     0.8 M  14920.5 14924.2     38.5      21.0   49.4 C

            ManifoldFlush 1      15.0       14.8 17:21:06    0.1 M     0.9 M  14924.6 14937.8     38.5      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 126.0 Kg          0.2 M     2.1 M                     35.1      21.0

               Material 88       6.8        6.6 17:30:48    0.1 M     1.1 M  14868.6 14875.2     31.9      21.0    0.0 C

            Flush 1      40.0       40.1 17:32:34    0.1 M     1.3 M  14876.0 14928.7     30.5      21.0    0.0 C

            MAKING TIME:              55.2 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.5 C
            QA TIME:                   2.8 MINS
            ENTERED VISCOSITY:        30.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:   414.8 Mins
            Allocated To:    Stocktank5    0.121 Te
            Allocated To:    Stocktank6   14.836 Te
            DROP TIME:                13.9 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   14.989 Te
            Vessel Weight Increase: 14.935 Te

            TOTAL BATCH TIME:         488.9 Mins
            BATCH COMPLETED AT:  00:47:37 ON 07-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 34
            BATCH START TIME :      00:49:44 on 07-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       95.8 01:11:51    0.1 M    17.0 M     -0.0    95.9      0.0       0.0    0.0 C

              Material 71      20.0       19.2 01:13:58    0.1 M     1.2 M     96.4   116.1      0.0       0.0    8.3 C

            Dump to MainMixer5 - decrease = 117.2 Kg Dump Time     2.2 M

            Material 49        15.0       16.1  01:32:41

            TOTAL VESSEL TIME:          39.7 Mins
            VESSEL COMPLETED AT:  01:32:58 ON 07-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1337.5 00:58:13    0.1 M     4.9 M      0.3  1335.7     64.5     100.0   62.3 C

            Dump to MainMixer5 - decrease = 1335.2 Kg Dump Time     5.5 M

            TOTAL VESSEL TIME:          28.5 Mins
            VESSEL COMPLETED AT:  01:21:11 ON 07-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 00:57:27    0.2 M     1.0 M   4434.4  5810.9     46.9      11.0    0.0 C

                 Material 68      12.4       12.5 01:00:03    0.2 M     1.9 M   6690.5  8991.5     49.8      11.0    0.0 C

                 Material 63    5122.7     5145.8 01:00:07    0.2 M     6.8 M    591.0  9031.1     49.7      11.0   80.0 C

            ManifoldFlush      15.0       15.7 01:01:36    0.1 M     0.9 M   9281.1  9767.3     47.2      11.0    0.0 C

                Material 64       7.5        5.9 01:04:19    0.1 M     2.1 M  10046.2 10726.8     42.8      11.0    0.0 C

            Flush      40.0       40.1 01:06:19    0.1 M     1.3 M  10999.6 11715.6     40.9      11.0    0.0 C

                     Material 59      12.4       12.4 01:08:44    0.1 M     1.7 M  12020.1 12889.0     37.9      11.0    0.0 C

                Material 67    8092.2     8096.5 01:10:24    0.1 M    18.5 M      2.6 13437.7     36.2      11.0    8.3 C

               Material59 Flush      20.0       20.1 01:13:05    0.1 M     1.5 M  13437.7 13466.6     36.0      21.0    8.3 C

            Empty PreWeigher2 -   Increase = 1346.8 Kg          0.2 M     5.3 M                     37.9      21.0

               Material 54     150.0      121.5 01:22:13    0.1 M     0.7 M  14817.1 14941.1     38.2      21.0   79.9 C

            Material 53       3.7        3.8 01:23:43    0.1 M     0.8 M  14942.3 14944.0     38.3      21.0   45.7 C

            ManifoldFlush 1      15.0       14.5 01:25:14    0.1 M     0.9 M  14947.3 14958.8     38.3      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 121.9 Kg          0.2 M     2.0 M                     35.1      21.0

               Material 88       6.8        6.8 01:35:44    0.1 M     1.2 M  14916.4 14920.9     32.1      21.0    0.0 C

            Flush 1      40.0       40.0 01:37:49    0.1 M     1.4 M  14920.5 14971.6     30.5      21.0    0.0 C

            MAKING TIME:              49.6 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.3 C
            QA TIME:                   6.2 MINS
            ENTERED VISCOSITY:        29.0 cPoise
            ENTERED pH:                2.5
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:   139.5 Mins
            Allocated To:    Stocktank6   15.001 Te
            DROP TIME:                14.2 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   15.008 Te
            Vessel Weight Increase: 14.982 Te

            TOTAL BATCH TIME:         210.9 Mins
            BATCH COMPLETED AT:  04:20:56 ON 07-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 35
            BATCH START TIME :      04:23:07 on 07-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       96.0 04:46:34    0.2 M    18.0 M      1.5    97.8      0.0       0.0    0.0 C

              Material 71      20.0       20.5 04:48:47    0.1 M     1.2 M     98.0   119.0      0.0       0.0    8.4 C

            Dump to MainMixer5 - decrease = 119.9 Kg Dump Time     2.1 M

            Material 49        15.0       16.4  05:07:31

            TOTAL VESSEL TIME:          41.0 Mins
            VESSEL COMPLETED AT:  05:07:41 ON 07-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1328.0 04:31:40    0.1 M     4.8 M     -0.5  1326.4     64.3     100.0   64.3 C

            Dump to MainMixer5 - decrease = 1318.0 Kg Dump Time     5.5 M

            TOTAL VESSEL TIME:          30.3 Mins
            VESSEL COMPLETED AT:  04:56:33 ON 07-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 04:31:06    0.2 M     1.0 M   4456.3  5894.1     50.9      11.0    0.0 C

                 Material 63    5116.1     5160.2 04:33:27    0.2 M     6.5 M    557.2  8480.2     52.7      11.0   79.9 C

                 Material 68      12.4       12.4 04:33:42    0.1 M     1.8 M   6842.5  8599.7     52.2      11.0    0.0 C

            ManifoldFlush      15.0       15.1 04:35:23    0.1 M     1.0 M   8864.6  9324.4     49.1      11.0    0.0 C

                Material 64       7.5        8.2 04:37:49    0.1 M     1.7 M   9607.8 10141.0     45.2      11.0    0.0 C

            Flush      40.0       39.7 04:39:56    0.1 M     1.4 M  10382.4 11070.0     43.0      11.0    0.0 C

                     Material 59      12.4       12.4 04:42:23    0.1 M     1.7 M  11332.0 12116.5     40.0      11.0    0.0 C

                Material 67    8098.8     8101.5 04:45:51    0.2 M    20.4 M      5.1 13451.8     36.4      11.0    8.3 C

               Material59 Flush      20.0       19.8 04:48:26    0.1 M     1.4 M  13450.5 13479.8     36.2      21.0    8.4 C

            Empty PreWeigher2 -   Increase = 1333.2 Kg          0.2 M     5.3 M                     38.0      21.0

               Material 54     150.0      119.0 04:57:36    0.1 M     0.7 M  14820.8 14942.8     38.3      21.0   79.9 C

            Material 53       3.7        3.5 04:59:28    0.1 M     1.2 M  14944.8 14949.8     38.4      21.0   45.1 C

            ManifoldFlush 1      15.0       14.9 05:00:53    0.1 M     0.8 M  14948.1 14963.4     38.5      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 126.0 Kg          0.2 M     2.0 M                     35.5      21.0

               Material 88       6.8        6.8 05:10:37    0.1 M     1.0 M  14915.6 14919.7     32.1      21.0    0.0 C

            Flush 1      40.0       40.1 05:12:24    0.1 M     1.3 M  14922.2 14979.4     30.7      21.0    0.0 C

            MAKING TIME:              50.3 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.6 C
            QA TIME:                  23.0 MINS
            ENTERED VISCOSITY:        29.0 cPoise
            ENTERED pH:                2.5
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:   159.3 Mins
            Allocated To:    Stocktank6   15.010 Te
            DROP TIME:                14.0 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   15.018 Te
            Vessel Weight Increase: 15.006 Te

            TOTAL BATCH TIME:         247.8 Mins
            BATCH COMPLETED AT:  08:31:13 ON 07-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 36
            BATCH START TIME :      08:33:10 on 07-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       96.0 09:09:12    0.2 M    20.2 M      1.9    98.0      0.0       0.0    0.0 C

              Material 71      20.0       20.4 09:11:23    0.1 M     1.2 M     98.1   118.9      0.0       0.0    8.4 C

            Dump to MainMixer5 - decrease = 119.7 Kg Dump Time     2.1 M

            Material 49        15.0       15.0  09:17:59

            TOTAL VESSEL TIME:          41.6 Mins
            VESSEL COMPLETED AT:  09:18:10 ON 07-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1330.5 08:41:54    0.1 M     5.3 M     -1.8  1330.8     64.8     100.0   61.9 C

            Dump to MainMixer5 - decrease = 1332.3 Kg Dump Time     5.4 M

            TOTAL VESSEL TIME:          31.1 Mins
            VESSEL COMPLETED AT:  09:07:07 ON 07-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 08:41:13    0.1 M     0.9 M   3979.2  4947.4     43.1      11.0    0.0 C

                 Material 68      12.4       12.2 08:43:55    0.2 M     2.0 M   5560.4  7541.3     45.8      11.0    0.0 C

            ManifoldFlush      15.0       15.1 08:45:32    0.1 M     1.0 M   8066.6  9074.3     46.4      11.0    0.0 C

                 Material 63    5094.8     5105.8 08:46:24    0.1 M     9.9 M    490.4  9721.6     46.8      11.0   80.0 C

                Material 64       7.5        8.8 08:48:24    0.1 M     2.2 M   9627.6 10204.0     44.4      11.0    0.0 C

            Flush      40.0       39.9 08:50:27    0.1 M     1.4 M  10433.9 11089.0     42.6      11.0    0.0 C

                     Material 59      12.4       12.4 08:52:55    0.1 M     1.7 M  11384.0 12145.7     39.8      11.0    0.0 C

                Material 67    8120.1     8113.8 08:56:27    0.1 M    21.2 M      6.7 13411.8     36.3      11.0    8.5 C

               Material59 Flush      20.0       19.7 08:59:07    0.1 M     1.5 M  13410.6 13434.9     36.1      21.0    8.4 C

            Empty PreWeigher2 -   Increase = 1346.8 Kg          0.2 M     5.3 M                     38.0      21.0

               Material 54     150.0      131.8 09:08:13    0.1 M     0.7 M  14784.6 14924.2     38.3      21.0   80.0 C

            Material 53       3.7        3.7 09:09:44    0.1 M     0.8 M  14923.4 14921.7     38.4      21.0   46.1 C

            ManifoldFlush 1      15.0       15.2 09:11:16    0.1 M     0.9 M  14923.8 14940.3     38.5      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 119.4 Kg          0.2 M     1.9 M                     35.4      21.0

               Material 88       6.8        6.7 09:21:09    0.1 M     1.1 M  14901.6 14908.1     32.0      21.0    0.0 C

            Flush 1      40.0       39.9 09:23:01    0.1 M     1.3 M  14909.4 14963.4     30.5      21.0    0.0 C

            MAKING TIME:              50.9 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.5 C
            QA TIME:                   5.5 MINS
            ENTERED VISCOSITY:        38.0 cPoise
            ENTERED pH:                2.5
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:    93.0 Mins
            Allocated To:    Stocktank6   14.997 Te
            DROP TIME:                14.2 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   14.991 Te
            Vessel Weight Increase: 14.986 Te

            TOTAL BATCH TIME:         164.9 Mins
            BATCH COMPLETED AT:  11:18:25 ON 07-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer5
                        ===========================

            RECIPE           : Recipe 38

            PRODUCT          : W-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8316
            BATCH NO         : 37
            BATCH START TIME :      11:21:00 on 07-Jan-20







            ------- VESSEL PreWeigher7 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      96.0       96.0 11:40:10    0.2 M    12.6 M      1.4    97.9      0.0       0.0    0.0 C

              Material 71      20.0       18.3 11:42:58    0.1 M     1.4 M     98.2   117.1      0.0       0.0    8.3 C

            Dump to MainMixer5 - decrease = 118.2 Kg Dump Time     2.0 M

            Material 49        15.0       15.9  12:54:32

            TOTAL VESSEL TIME:          89.4 Mins
            VESSEL COMPLETED AT:  12:54:43 ON 07-Jan-20


            ------- VESSEL PreWeigher2 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1334.2 12:31:23    0.1 M     4.9 M     -6.2  1328.6     64.8     100.0   60.6 C

            Dump to MainMixer5 - decrease = 1331.3 Kg Dump Time     5.4 M

            TOTAL VESSEL TIME:          18.4 Mins
            VESSEL COMPLETED AT:  12:44:05 ON 07-Jan-20












            ------- VESSEL MainMixer5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 11:32:46    0.2 M     1.1 M   3530.5  3986.6      9.0      11.0    0.0 C

                 Material 68      12.4       12.5 11:35:36    0.1 M     1.9 M   4328.6  5152.1      8.9      11.0    0.0 C

            ManifoldFlush      15.0       15.1 11:37:35    0.2 M     1.0 M   5531.2  5987.2      9.0      11.0    0.0 C

                Material 67    8092.6     8093.3 11:43:28    0.1 M    19.6 M     10.0  8140.3      8.8      11.0    8.3 C

                Material 64       7.5        6.6 12:10:53   23.8 M     8.8 M   8142.8  8144.4      8.9      11.0    0.0 C

            Flush      40.0       39.8 12:13:02    0.2 M     1.4 M   8144.0  8195.9      9.2      11.0    0.0 C

                     Material 59      12.4       12.3 12:15:35    0.1 M     1.8 M   8202.5  8223.5      9.4      11.0    0.0 C

                 Material 63    5122.3     5162.3 12:33:08    0.2 M     6.7 M   8225.2 13373.1     36.3      11.0   79.9 C

               Material59 Flush      20.0       20.1 12:35:59    0.1 M     1.5 M  13392.4 13425.0     36.5      21.0    8.6 C

            Empty PreWeigher2 -   Increase = 1344.3 Kg          0.2 M     5.3 M                     38.3      21.0

               Material 54     150.0      117.4 12:45:06    0.1 M     0.7 M  14774.7 14894.1     38.6      21.0   79.8 C

            Material 53       3.7        3.8 12:46:33    0.1 M     0.8 M  14898.7 14898.7     38.7      21.0   48.0 C

            ManifoldFlush 1      15.0       14.3 12:47:58    0.1 M     0.9 M  14899.9 14913.9     38.7      21.0    0.0 C

            Empty PreWeigher7 -   Increase = 121.5 Kg          0.2 M     1.9 M                     36.2      21.0

               Material 88       6.8        6.8 12:57:44    0.1 M     1.1 M  14849.2 14852.1     33.1      21.0    0.0 C

            Flush 1     40.0       40.1 12:59:33    0.1 M     1.3 M  14852.5 14906.5     31.7      21.0    0.0 C

            MAKING TIME:              99.4 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              30.7 C
            QA TIME:                  21.0 MINS
            ENTERED VISCOSITY:        35.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     2.5 Mins
            Allocated To:    Stocktank6   14.947 Te
            DROP TIME:                14.1 Mins
            Total Recipe Weights:   15.005 Te
            Total Actual Weights:   15.012 Te
            Vessel Weight Increase: 14.937 Te

            TOTAL BATCH TIME:         138.2 Mins
            BATCH COMPLETED AT:  13:39:42 ON 07-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer4
                        ===========================

            RECIPE           : BB-Recipe 8

            PRODUCT          : B-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8345
            BATCH NO         : 2
            BATCH START TIME :      08:55:05 on 06-Jan-20







            ------- VESSEL PreWeigher5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 95     305.0      305.5 09:10:08    0.2 M     9.6 M      5.8   311.5      0.0       0.0    0.0 C

            Dump to MainMixer4 - decrease = 305.4 Kg Dump Time     3.0 M

            Material 49        15.0       17.5  10:10:02

            TOTAL VESSEL TIME:          71.5 Mins
            VESSEL COMPLETED AT:  10:10:11 ON 06-Jan-20

            CACL TO PreWeigher9      39.7       39.6 09:29:32    0.1 M     1.3 M     26.0    62.6      0.0      30.0    0.0 C

            Dump to MainMixer4 - decrease = 62.6 Kg Dump Time     1.7 M

            TOTAL VESSEL TIME:           8.1 Mins
            VESSEL COMPLETED AT:  09:33:39 ON 06-Jan-20

            ------- VESSEL PreWeigher9  (PART 2) ---------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

             HOT TO PreWeigher9      33.9       33.5 09:38:42    0.1 M     2.0 M      0.0    32.6      0.0      30.0    0.0 C

            CACL TO PreWeigher9      48.5       48.8 09:40:37    0.1 M     1.5 M     32.6    79.4      0.0      30.0    0.0 C

            Dump to MainMixer4 - decrease =    79.4 Kg Dump Time     2.0 M

            TOTAL VESSEL TIME:          14.9 Mins
            VESSEL COMPLETED AT:  09:51:02 ON 06-Jan-20


            ------- VESSEL PreWeigher1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    3166.7     3162.8 09:13:40    0.2 M    10.3 M    -38.3  3128.2     64.1     100.0   61.8 C

            Dump to MainMixer4 - decrease =  3171.8 Kg Dump Time    11.2 M

            TOTAL VESSEL TIME:          45.6 Mins
            VESSEL COMPLETED AT:  09:47:59 ON 06-Jan-20












            ------- VESSEL MainMixer4 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       6.0        5.9 09:05:07    0.1 M     1.4 M   3863.8  5283.2     19.3      11.0    0.0 C

                Material 67    4293.0     4296.0 09:05:15    0.2 M     7.7 M     77.6  5354.0     20.3      11.0    8.7 C

                 Material 68      25.5       25.6 09:09:13    0.1 M     3.2 M   5780.8  7363.3     37.3      11.0    0.0 C

            ManifoldFlush      15.0       14.5 09:11:09    0.1 M     1.1 M   7649.2  8110.7     41.1      11.0    0.0 C

                Material 64       7.5        7.0 09:16:30    0.2 M     4.5 M   8407.7  9037.2     45.8      11.0    0.0 C

            Flush      40.0       39.8 09:19:16    0.2 M     1.4 M   9763.6 10734.7     50.6      11.0    0.0 C

                 Material 63    6612.9     6546.6 09:20:28    0.2 M    17.2 M   3505.8 11117.4     52.1      11.0   80.1 C

                     Material 59      20.4       20.4 09:22:21    0.2 M     2.2 M  11095.6 11142.5     52.1      11.0    0.0 C

               Material59 Flush      20.0       20.1 09:26:11    0.1 M     1.5 M  11072.1 10979.0     51.5      29.0    8.6 C

               Material 57     135.0      135.4 09:28:52    0.2 M     1.6 M  10958.8 11103.0     51.1      29.0    0.0 C

             PerfumeFlush      50.0       50.2 09:31:10    0.1 M     1.3 M  11103.0 11202.3     51.2      29.0    0.0 C

            Empty PreWeigher9 -   Increase =    60.5 Kg          0.2 M     1.6 M                     51.2      29.0

            Empty PreWeigher1 -   Increase =  3190.0 Kg          0.1 M    11.1 M                     54.3      29.0

               Material 54     150.0      228.2 09:49:44    0.1 M     1.1 M  14470.6 14735.9     54.8      29.0   79.9 C

            Empty PreWeigher9 -   Increase =    82.3 Kg          0.2 M     1.8 M                     54.9      29.0

            MILLING Finished at 09:53:06. Ran for  18.3 M

            Empty PreWeigher5 -   Increase =   377.3 Kg          0.1 M     3.0 M                     33.3      29.0

            Flush 1      40.0       39.9 10:12:38    0.1 M     1.3 M  15161.1 15204.8     31.5      29.0    0.0 C

            MAKING TIME:              83.9 MINS
            TOTAL MILLING TIME:       18.0 MINS
            PRE QA TEMP:              29.6 C
            QA TIME:                   0.3 MINS
            ENTERED VISCOSITY:        58.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     2.6 Mins
            Allocated To:    Stocktank1   15.218 Te
            DROP TIME:                14.2 Mins
            Total Recipe Weights:   15.052 Te
            Total Actual Weights:   15.066 Te
            Vessel Weight Increase: 15.138 Te

            TOTAL BATCH TIME:         101.9 Mins
            BATCH COMPLETED AT:  10:37:23 ON 06-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer3
                        ===========================

            RECIPE           : Recipe 19

            PRODUCT          : K-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8350
            BATCH NO         : 1
            BATCH START TIME :      10:04:13 on 06-Jan-20







            ------- VESSEL PreWeigher8 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                   Material 11     126.0      125.4 10:14:02    0.1 M     5.0 M     -2.7   122.8      0.0       0.0    0.0 C

              Material 71      20.0       19.2 10:16:09    0.1 M     1.0 M    123.0   142.4      0.0       0.0    8.7 C

            Dump to MainMixer3 - decrease = 153.2 Kg Dump Time     1.5 M

            Material 49        15.0       15.1  11:02:50

            TOTAL VESSEL TIME:          55.6 Mins
            VESSEL COMPLETED AT:  11:03:01 ON 06-Jan-20


            ------- VESSEL PreWeigher3 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    1333.0     1334.9 10:11:16    0.1 M     3.8 M     41.6  1375.5     65.0     100.0   64.7 C

            Dump to MainMixer3 - decrease = 1347.3 Kg Dump Time     6.5 M

            TOTAL VESSEL TIME:          44.1 Mins
            VESSEL COMPLETED AT:  10:50:59 ON 06-Jan-20












            ------- VESSEL MainMixer3 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 10:12:16    0.1 M     1.1 M   3988.8  5253.9     51.1      10.0    0.0 C

                 Material 68      12.4       12.4 10:14:47    0.1 M     1.9 M   5862.8  7983.3     52.8      10.0    0.0 C

                 Material 63    5159.3     5164.0 10:15:31    0.1 M     8.0 M    298.6  8456.8     53.3      10.0   80.0 C

            ManifoldFlush      15.0       15.1 10:16:15    0.1 M     0.9 M   8384.8  8733.3     51.7      10.0    0.0 C

                Material 64      15.0       15.2 10:18:23    0.1 M     1.6 M   8921.7  9511.1     48.2      10.0    0.0 C

            Flush      40.0       39.9 10:20:35    0.1 M     1.3 M   9809.6 10371.8     45.2      10.0    0.0 C

                     Material 59      12.4       12.4 10:22:47    0.1 M     1.7 M  10542.5 11176.2     42.5      10.0    0.0 C

                Material 67    7998.6     7999.0 10:29:04    0.2 M    22.8 M    -52.8 13273.8     37.0      10.0    8.6 C

               Material59 Flush      20.0       20.0 10:38:02    0.1 M     1.4 M  13414.9 13442.6     36.8      23.0    8.6 C

               Material 57      22.5       20.8 10:40:32    0.2 M     1.7 M  13445.4 13444.0     36.8      23.0    0.0 C

             PerfumeFlush      40.0       40.3 10:42:36    0.1 M     1.2 M  13445.0 13554.2     36.9      23.0    0.0 C

            Empty PreWeigher3 -   Increase = 1383.3 Kg          0.1 M     6.4 M                     39.5      23.0

               Material 54     150.0       84.4 10:52:39    0.1 M     1.2 M  14957.5 15045.7     39.2      23.0   79.9 C

            Material 53       3.7        3.4 10:54:40    0.1 M     1.3 M  15045.2 15049.5     39.1      23.0   48.3 C

            ManifoldFlush 1      15.0       14.6 10:56:03    0.1 M     0.9 M  15051.9 15066.2     39.1      23.0    0.0 C

            Empty PreWeigher8 -   Increase = 164.9 Kg          0.1 M     1.4 M                     35.8      23.0

               Material 88       6.8        6.8 11:06:32    0.1 M     1.0 M  15239.3 15246.5     32.2      23.0    0.0 C

            Flush 1      40.0       39.9 11:08:14    0.1 M     1.3 M  15246.0 15310.8     30.9      23.0    0.0 C

            MAKING TIME:              65.4 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              30.3 C
            QA TIME:                  13.8 MINS
            ENTERED VISCOSITY:        27.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     2.8 Mins
            Allocated To:    Stocktank13   15.305 Te
            DROP TIME:                13.9 Mins
            Total Recipe Weights:   15.048 Te
            Total Actual Weights:   14.987 Te
            Vessel Weight Increase: 15.357 Te

            TOTAL BATCH TIME:          97.1 Mins
            BATCH COMPLETED AT:  11:41:36 ON 06-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer4
                        ===========================

            RECIPE           : RE-Recipe 39

            PRODUCT          : K-REG
            COLOUR           : WHITE
            CAMPAIGN NO      : 8375
            BATCH NO         : 1
            BATCH START TIME :      22:07:08 on 08-Jan-20







            ------- VESSEL PreWeigher5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      36.0       36.0 22:20:39    0.1 M     8.6 M      4.2    40.5      0.0       0.0    0.0 C

            Dump to MainMixer4 - decrease = 34.6 Kg Dump Time     1.8 M

            Material 49        15.0       18.1  23:35:21

            TOTAL VESSEL TIME:          84.8 Mins
            VESSEL COMPLETED AT:  23:35:36 ON 08-Jan-20


            ------- VESSEL PreWeigher1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51     493.0      494.8 22:13:08    0.1 M     2.3 M     13.1   508.7     62.4     100.0   63.9 C

               Material 21      73.5       61.6 23:08:58    0.2 M    55.1 M    506.8   562.2     65.0     100.0   63.4 C

            Dump to MainMixer4 - decrease = 548.1 Kg Dump Time     3.9 M

            TOTAL VESSEL TIME:          66.4 Mins
            VESSEL COMPLETED AT:  23:16:28 ON 08-Jan-20












            ------- VESSEL MainMixer4 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 22:14:46    0.2 M     1.0 M   4253.2  5492.0     39.9      10.0    0.0 C

                Material 64      15.0       14.8 22:17:07    0.2 M     1.6 M   6363.4  8247.9     42.2      10.0    0.0 C

            Flush      40.0       39.7 22:19:22    0.2 M     1.5 M   9113.0 10940.2     43.9      10.0    0.0 C

                 Material 68      12.4       12.3 22:22:13    0.1 M     2.1 M  11812.0 14074.3     44.5      10.0    0.0 C

                Material 67    6903.9     6900.8 22:22:13    0.1 M    13.2 M     92.9 14074.3     44.6      10.0    8.7 C

                 Material 63    7145.0     7089.7 22:23:02    0.1 M    12.3 M    859.6 14264.2     45.3      10.0   79.9 C

            ManifoldFlush      20.0       19.5 22:23:59    0.1 M     1.1 M  14251.8 14293.0     45.4      10.0    0.0 C

                     Material 59       7.9        7.9 22:26:06    0.1 M     1.4 M  14295.9 14305.4     45.4      10.0    0.0 C

               Material59 Flush      20.0       19.9 22:37:02    0.1 M     1.5 M  14326.0 14358.6     44.7      20.0    8.7 C

            Empty PreWeigher1 -   Increase = 551.6 Kg         33.6 M     3.7 M                     45.2      20.0

               Material 54     150.0      227.4 23:26:30    0.1 M     9.6 M  14917.2 15145.9     45.8      20.0   79.9 C

            Empty PreWeigher5 -   Increase = 66.3 Kg          0.1 M     1.7 M                     45.8      20.0

                Material 24       9.0        9.0 23:51:24    0.2 M     6.4 M  15136.4 15152.5     31.4      18.0    0.0 C

            Material 22      10.0       10.0 23:54:53    0.1 M     2.9 M  15146.7 15164.8     29.6      18.0   80.1 C

               Material 88       6.8        6.7 23:56:44    0.2 M     1.2 M  15167.3 15175.9     29.6      18.0    0.0 C

            Flush 1      40.0       40.1 23:59:04    0.1 M     1.5 M  15174.3 15222.9     29.7      18.0    0.0 C

            Flush 2     40.0       39.9 00:01:49    0.2 M     1.5 M  15230.3 15271.1     29.8      18.0    0.0 C

            MAKING TIME:             115.9 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              29.9 C
            QA TIME:                   0.9 MINS
            ENTERED VISCOSITY:        11.0 cPoise
            ENTERED pH:                2.3
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:   230.1 Mins
            Allocated To:    Stocktank7    0.142 Te
            DROP TIME:                 1.2 Mins
            Total Recipe Weights:   15.041 Te
            Total Actual Weights:   15.052 Te
            Vessel Weight Increase: 15.188 Te

            TOTAL BATCH TIME:         349.6 Mins
            BATCH COMPLETED AT:  03:56:59 ON 09-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer4
                        ===========================

            RECIPE           : RE-Recipe 39

            PRODUCT          : K-REG
            COLOUR           : WHITE
            CAMPAIGN NO      : 8385
            BATCH NO         : 1
            BATCH START TIME :      18:52:53 on 09-Jan-20







            ------- VESSEL PreWeigher5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 70      36.0       36.6 19:07:56    0.2 M     9.5 M      4.4    42.6      0.0       0.0    0.0 C

            Dump to MainMixer4 - decrease = 36.9 Kg Dump Time     1.9 M

            Material 49        15.0       15.5  20:14:21

            TOTAL VESSEL TIME:          77.5 Mins
            VESSEL COMPLETED AT:  20:14:33 ON 09-Jan-20


            ------- VESSEL PreWeigher1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51     493.0      493.2 19:30:47    0.1 M     2.3 M      3.3   496.4     63.3     100.0   56.0 C

               Material 21      73.5       86.9 19:35:20    0.1 M     3.7 M    497.5   591.1     63.7     100.0   51.8 C

            Dump to MainMixer4 - decrease = 586.0 Kg Dump Time     3.9 M

            TOTAL VESSEL TIME:          36.0 Mins
            VESSEL COMPLETED AT:  20:03:48 ON 09-Jan-20












            ------- VESSEL MainMixer4 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 19:03:13    0.2 M     1.0 M   3658.7  4269.6      9.9      10.0    0.0 C

                Material 64      15.0       15.1 19:06:14    0.2 M     2.2 M   4744.7  6175.1      9.7      10.0    0.0 C

                Material 67    6921.8     6925.7 19:08:07    0.2 M    12.6 M     81.7  7077.8     10.0      10.0    9.0 C

            Flush      40.0       40.0 19:08:31    0.2 M     1.4 M   6671.2  7085.6     10.1      10.0    0.0 C

                 Material 68      12.4       12.3 19:11:19    0.2 M     2.1 M   7091.0  7104.6     10.3      10.0    0.0 C

            ManifoldFlush      20.0       19.8 19:13:14    0.2 M     1.1 M   7109.5  7125.6     10.4      10.0    0.0 C

                     Material 59       7.9        7.8 19:15:32    0.2 M     1.5 M   7127.6  7136.7     10.5      10.0    0.0 C

                 Material 63    7127.1     7056.5 19:40:51    0.1 M    12.4 M   7136.3 14311.2     45.3      10.0   80.0 C

               Material59 Flush      20.0       19.9 19:57:53    0.2 M     1.4 M  14306.6 14341.7     44.5      20.0    9.2 C

            Empty PreWeigher1 -   Increase = 589.1 Kg          0.1 M     3.9 M                     45.2      20.0

               Material 54     150.0      238.9 20:05:23    0.1 M     1.2 M  14931.6 15177.2     45.8      20.0   79.9 C

            Empty PreWeigher5 -   Increase = 64.6 Kg          0.1 M     1.9 M                     45.8      20.0

                Material 24       9.0        8.9 20:30:05    0.2 M     6.5 M  15213.0 15230.7     31.4      18.0    0.0 C

            Material 22      10.0        9.9 20:33:21    0.1 M     2.7 M  15226.2 15232.8     29.8      18.0   79.9 C

               Material 88       6.8        6.7 20:35:03    0.1 M     1.1 M  15236.1 15242.7     29.8      18.0    0.0 C

            Flush 1      40.0       40.0 20:37:01    0.1 M     1.4 M  15243.5 15311.1     29.9      18.0    0.0 C

            Flush 2      40.0       39.5 20:39:42    0.2 M     1.5 M  15311.9 15364.2     30.1      18.0    0.0 C

            MAKING TIME:             107.8 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              30.1 C
            QA TIME:                   0.5 MINS
            ENTERED VISCOSITY:        33.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     2.0 Mins
            Allocated To:    Stocktank7   15.371 Te
            DROP TIME:                14.4 Mins
            Total Recipe Weights:   15.041 Te
            Total Actual Weights:   15.077 Te
            Vessel Weight Increase: 15.290 Te

            TOTAL BATCH TIME:         126.2 Mins
            BATCH COMPLETED AT:  20:59:34 ON 09-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer1
                        ===========================

            RECIPE           : DM-RE-Recipe 18

            PRODUCT          : K-REG
            COLOUR           : BLUE
            CAMPAIGN NO      : 8386
            BATCH NO         : 2
            BATCH START TIME :      22:09:08 on 09-Jan-20







            ------- VESSEL PreWeigher4 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 10      47.1       47.0 22:18:09    0.2 M     3.7 M      1.5    48.6      0.0       0.0    0.0 C

            Dump to MainMixer1 - decrease = 45.4 Kg Dump Time     1.2 M

            Material 49        15.0       16.5  23:10:22

            TOTAL VESSEL TIME:          57.8 Mins
            VESSEL COMPLETED AT:  23:10:32 ON 09-Jan-20


            ------- VESSEL PreWeigher1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51     483.0      485.0 22:15:04    0.1 M     2.3 M      7.7   491.9     61.6     100.0   55.2 C

               Material 21      73.5       67.6 22:34:16    0.2 M    18.5 M    491.9   562.6     63.0     100.0   51.0 C

            Dump to MainMixer1 - decrease = 553.1 Kg Dump Time     3.8 M

            TOTAL VESSEL TIME:          48.7 Mins
            VESSEL COMPLETED AT:  23:00:53 ON 09-Jan-20












            ------- VESSEL MainMixer1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 64      15.0       15.5 22:21:30    0.2 M     2.4 M   3971.0  5328.0     76.9      10.0    0.0 C

                 Material 63    5967.6     5949.8 22:22:45    0.2 M    10.0 M   -270.2  5803.1     77.2      10.0   79.9 C

            Flush      40.0       40.0 22:23:36    0.1 M     1.4 M   5664.9  5844.3     77.0      10.0    0.0 C

                 Material 68      12.4       12.4 22:26:06    0.1 M     1.9 M   5851.1  6537.3     71.8      10.0    0.0 C

            ManifoldFlush      20.0       20.5 22:27:48    0.1 M     1.0 M   6757.9  7151.0     67.1      10.0    0.0 C

                     Material 59       7.9        7.9 22:29:59    0.1 M     1.5 M   7366.5  7926.5     62.0      10.0    0.0 C

                Material 67    6982.3     6984.3 22:44:58   12.9 M    20.9 M   5849.0 12972.1     42.2      10.0    9.0 C

            Material 94    1000.0      996.7 22:52:25    0.1 M     6.9 M  12964.4 14034.2     44.7      10.0    0.0 C

               Material59 Flush      20.0       19.8 22:55:12    0.1 M     1.5 M  14033.4 14059.6     44.6      20.0    9.1 C

            Empty PreWeigher1 -   Increase = 550.6 Kg          0.1 M     3.7 M                     45.3      20.0

               Material 54     150.0      170.3 23:01:58    0.1 M     0.7 M  14611.9 14784.8     45.7      20.0   80.0 C

                Material 81      17.4       17.4 23:05:12    0.0 M     2.0 M  14781.0 14802.0     45.7      20.0    0.0 C

              Material 48       2.0        2.0 23:06:27    0.0 M     0.8 M  14804.1 14807.6     45.7      20.0    0.0 C

            Empty PreWeigher4 -   Increase = 53.2 Kg          0.1 M     1.2 M                     45.7      20.0

               Material 89      50.0       49.9 23:14:48    0.0 M     3.4 M  14863.8 14915.7     45.7      18.0    0.0 C

                Material 24       9.0        8.9 23:27:45    0.2 M     3.9 M  14894.3 14899.8     33.4      18.0    0.0 C

            Material 22      15.0       15.0 23:31:24    0.1 M     3.1 M  14897.3 14913.1     31.0      18.0   79.9 C

               Material 88       7.6        7.6 23:32:44    0.1 M     1.1 M  14917.9 14920.9     30.3      18.0    0.0 C

            Flush 1      40.0       40.1 23:34:35    0.1 M     1.3 M  14919.1 14969.4     29.8      18.0    0.0 C

                   Material 47      30.0       29.9 23:37:19    0.1 M     2.2 M  14980.1 15020.0     29.9      18.0    0.0 C

            Flush 2      40.0       39.9 23:39:10    0.1 M     1.3 M  15019.6 15059.1     30.0      18.0    0.0 C

            MAKING TIME:              90.7 MINS
            TOTAL MILLING TIME:        0.0 MINS
            PRE QA TEMP:              30.1 C
            QA TIME:                   7.4 MINS
            ENTERED VISCOSITY:        19.0 cPoise
            ENTERED pH:                2.5
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     1.4 Mins
            Allocated To:    Stocktank5   15.092 Te
            DROP TIME:                14.0 Mins
            Total Recipe Weights:   15.045 Te
            Total Actual Weights:   15.045 Te
            Vessel Weight Increase: 15.361 Te

            TOTAL BATCH TIME:         114.6 Mins
            BATCH COMPLETED AT:  00:04:01 ON 10-Jan-20

            END-OF-REPORT
            NEWREPORT
                        PROCESS BATCH REPORT - MainMixer4
                        ===========================

            RECIPE           : BB-Recipe 15

            PRODUCT          : B-CON
            COLOUR           : WHITE
            CAMPAIGN NO      : 8396
            BATCH NO         : 1
            BATCH START TIME :      18:47:34 on 10-Jan-20







            ------- VESSEL PreWeigher5 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

              Material 14     190.5      191.4 19:02:26    0.2 M     9.9 M      6.1   197.6      0.0       0.0    0.0 C

            Dump to MainMixer4 - decrease = 192.3 Kg Dump Time     2.3 M

            Material 49        15.0       17.3  20:24:33

            TOTAL VESSEL TIME:          93.6 Mins
            VESSEL COMPLETED AT:  20:24:44 ON 10-Jan-20

            CACL TO PreWeigher9      39.7       39.5 19:15:08    0.1 M     1.4 M     26.3    62.7      0.0      30.0    0.0 C

            Dump to MainMixer4 - decrease = 62.7 Kg Dump Time     1.7 M

            TOTAL VESSEL TIME:          38.1 Mins
            VESSEL COMPLETED AT:  19:49:06 ON 10-Jan-20

            ------- VESSEL PreWeigher9  (PART 2) ---------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

             HOT TO PreWeigher9      33.9       34.1 19:54:04    0.1 M     2.0 M      0.0    33.0      0.0      30.0    0.0 C

            CACL TO PreWeigher9      48.5       48.4 19:56:04    0.1 M     1.5 M     33.0    79.4      0.0      30.0    0.0 C

            Dump to MainMixer4 - decrease =    79.4 Kg Dump Time     2.0 M

            TOTAL VESSEL TIME:          15.0 Mins
            VESSEL COMPLETED AT:  20:06:33 ON 10-Jan-20


            ------- VESSEL PreWeigher1 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

                Material 51    3166.7     3165.5 19:00:28    0.1 M     9.3 M    -26.4  3140.2     62.5     100.0   58.2 C

            Dump to MainMixer4 - decrease =  3167.2 Kg Dump Time    11.1 M

            TOTAL VESSEL TIME:          72.9 Mins
            VESSEL COMPLETED AT:  20:03:28 ON 10-Jan-20












            ------- VESSEL MainMixer4 --------------------

                            TARGET ACTUAL   START WAIT     WEIGH VESSEL WEIGHT VESSEL    AGIT RAWMAT
            MATERIAL WEIGHT     WEIGHT TIME     TIME TIME     BEFORE AFTER     TEMP SPEED   TEMP
            -----------------------------------------------------------------------------------------------------------

               Material 1       3.3        3.3 18:55:58    0.2 M     1.1 M   4064.1  5140.6     33.0      11.0    0.0 C

                Material 67    4324.0     4322.1 18:58:13    0.1 M     8.5 M     61.1  6936.9     34.7      11.0    9.3 C

                 Material 68      25.5       25.4 18:59:51    0.2 M     3.1 M   5814.2  7510.0     38.0      11.0    0.0 C

            ManifoldFlush      15.0       15.8 19:01:36    0.2 M     1.0 M   7957.0  8593.5     42.9      11.0    0.0 C

                Material 64       7.5        7.8 19:03:27    0.1 M     1.2 M   8982.4  9737.2     47.4      11.0    0.0 C

            Flush      40.0       39.6 19:05:47    0.1 M     1.3 M  10331.7 11098.0     51.9      11.0    0.0 C

                 Material 63    6615.9     6561.0 19:06:06    0.2 M    14.9 M    734.3 11121.5     52.0      11.0   80.0 C

                     Material 59      20.4       20.4 19:08:31    0.1 M     2.1 M  11128.5 11154.5     52.1      11.0    0.0 C

               Material59 Flush      20.0       19.9 19:11:44    0.1 M     1.5 M  11121.1 10977.7     51.4      29.0    9.2 C

                Material 74     183.0      183.1 19:44:40   28.7 M     3.6 M  10969.1 11159.8     50.5      29.0    0.0 C

             PerfumeFlush      50.0       50.0 19:46:43    0.1 M     1.2 M  11173.8 11250.1     50.5      29.0    0.0 C

            Empty PreWeigher9 -   Increase =    60.5 Kg          0.2 M     1.6 M                     50.6      29.0

            Empty PreWeigher1 -   Increase =  3232.5 Kg          0.2 M    11.1 M                     53.6      29.0

               Material 54     150.0      212.9 20:05:10    0.1 M     1.1 M  14553.8 14803.1     54.2      29.0   79.9 C

            Empty PreWeigher9 -   Increase =    85.2 Kg          0.2 M     1.8 M                     54.3      29.0

            MILLING Finished at 20:08:37. Ran for  18.5 M

            Empty PreWeigher5 -   Increase =   220.4 Kg          0.0 M     2.3 M                     33.9      29.0

               Material 88       6.8        6.8 20:27:29    0.1 M     1.0 M  15084.9 15084.9     31.6      29.0    0.0 C

            Flush 1      40.0       39.9 20:29:15    0.1 M     1.3 M  15094.0 15148.3     30.4      29.0    0.0 C

              Material 85     400.0      402.5 20:36:23    0.1 M     4.5 M  15157.0 15559.1     29.3      29.0    0.0 C

            MAKING TIME:             111.4 MINS
            TOTAL MILLING TIME:       18.0 MINS
            PRE QA TEMP:              29.3 C
            QA TIME:                   4.2 MINS
            ENTERED VISCOSITY:        56.0 cPoise
            ENTERED pH:                2.4
            ENTERED S.G.:            1.000
            APPEARANCE:                 OK
            VISUAL COLOUR:              OK
            MEASURED COLOUR:            OK
            ODOUR CHECK:                OK
            OVERALL QA STATUS:        PASS

            STOCK ALLOCATION TIME:     1.9 Mins
            Allocated To:    Stocktank8   15.578 Te
            DROP TIME:                14.5 Mins
            Total Recipe Weights:   15.423 Te
            Total Actual Weights:   15.435 Te
            Vessel Weight Increase: 15.517 Te

            TOTAL BATCH TIME:         133.1 Mins
            BATCH COMPLETED AT:  21:01:01 ON 10-Jan-20

            END-OF-REPORT
            NEWREPORT";
        }


    }
}
