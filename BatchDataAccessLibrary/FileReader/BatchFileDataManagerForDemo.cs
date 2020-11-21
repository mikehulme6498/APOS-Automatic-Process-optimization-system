using BatchDataAccessLibrary.Enums;
using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static BatchDataAccessLibrary.Models.Vessel;

namespace BatchDataAccessLibrary.FileReader
{
    public class BatchFileDataManagerForDemo : IBatchDataFileManager
    {
            readonly List<BatchReport> _batchReports;
            readonly MaterialsFound _materialsFound;
            private readonly IMaterialDetailsRepository _materialDetailsRepository;

            public BatchFileDataManagerForDemo(IMaterialDetailsRepository materialDetailsRepository)
            {
                _materialDetailsRepository = materialDetailsRepository;
                _batchReports = new List<BatchReport>();
                _materialsFound = new MaterialsFound(_materialDetailsRepository);
            }

            /// <summary>
            /// Takes a string of batch data. Each batch needs to be seperated with the text NEWREPORT. This function will use that to seperate into 
            /// each batch. It will return a list of batch reports. Each report will have a bool IsValidBatch. Only reports marked as a valid batch should
            /// be added to the database.
            /// </summary>
            /// <param name="textfromAllfiles">Make sure the end of each report has the text NEWREPORT</param>
            public List<BatchReport> ProcessStringIntoBatchReports(string textfromAllfiles)
            {

                string[] eachBatch = SplitStringIntoSeperateBatchArray(textfromAllfiles);
                List<string[]> batchesToProcess = RemoveUnnecessaryCommas(eachBatch);

                foreach (string[] batch in batchesToProcess)
                {
                    ProcessStringIntoReport(batch);
                }
                _materialsFound.AddNewMaterialsToDB();
                return _batchReports;
            }


            private string[] SplitStringIntoSeperateBatchArray(string textFromFiles)
            {
                string[] whereToSplitBatch = { "NEWREPORT" };
                return textFromFiles.Split(whereToSplitBatch, StringSplitOptions.None);
            }

            private List<string[]> RemoveUnnecessaryCommas(string[] batches)
            {
                List<string[]> output = new List<string[]>();

                foreach (string line in batches)
                {
                    string[] newline = line.Split(new[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (newline.Count() < 2) { continue; }

                    for (int i = 0; i < newline.Length; i++)
                    {
                        if (newline[i].StartsWith(","))
                        {
                            newline[i] = newline[i].Substring(1);
                        }
                    }
                    output.Add(newline);
                }
                return output;
            }

            private void ProcessStringIntoReport(string[] data)
            {
                BatchReport report = new BatchReport();
                Vessel tempVessel = new Vessel();
                bool stayInCalciumPreWeigherLoop = true;
                report.OriginalReport = data.StringArrayToString();

                report.FileName = GetFileName(report.OriginalReport);

                if (!report.OriginalReport.Contains("PROCESS BATCH REPORT") || !report.OriginalReport.Contains("MATERIAL") || !report.OriginalReport.Contains("CAMPAIGN"))
                {
                    RecordFault(report, "Text file did not contain any information relevent to the process.", null);
                    _batchReports.Add(report);
                    return;
                }

                foreach (string line in data)
                {
                    List<string> lineInfo = FormatBatchStringForInspection(line);

                    if (lineInfo.Count == 0 || lineInfo.Count == 1) { continue; }
                    if (lineInfo[0] == "MATERIAL") { continue; }
                    if (lineInfo.Contains("ABORTED"))
                    {
                        RecordFault(report, "Batch was aborted", null);
                        continue;
                    }
                    if (!report.IsValidBatch) { break; }

                    if (lineInfo.Contains("CACL") && report.Recipe.StartsWith("BB-") && stayInCalciumPreWeigherLoop == true)
                    {
                        tempVessel.VesselName = "Pre-weigher-9 Part 1";
                        tempVessel.VesselType = Vessel.VesselTypes.CalciumPreWeigher;
                        string temp = lineInfo[0] + " " + lineInfo[1] + " " + lineInfo[2];
                        lineInfo.RemoveRange(0, 3);
                        lineInfo.Insert(0, temp);
                        GetMaterialData(lineInfo, tempVessel, report);
                        continue;
                    }

                    if (lineInfo.Contains("Dump") && report.Recipe.StartsWith("BB-") && stayInCalciumPreWeigherLoop == true && tempVessel.VesselType != Vessel.VesselTypes.PerfumePreWeigher)
                    {
                        tempVessel.DumpTime = Convert.ToDouble(lineInfo[7]);
                        tempVessel.Decrease = Convert.ToDouble(lineInfo[4]);
                        stayInCalciumPreWeigherLoop = false;
                        continue;
                    }

                    if (FoundBatchMetaData(report, line) == true)
                    {
                        continue;
                    }

                    if (lineInfo[0] == "-------")
                    {
                        SetVesselName(report, tempVessel, lineInfo);
                    }

                    if (lineInfo[1] == "COMPLETED")
                    {
                        FinishCurrentTempVessel(report, tempVessel, lineInfo);
                        tempVessel = new Vessel();
                    }

                    if (lineInfo.Count >= 11) // Materials with all figures (Target weight to rawmat temp) - 1 parts to name
                    {
                        GetMaterialData(SortMaterialNameIntoOneString(lineInfo), tempVessel, report);
                        continue;
                    }

                    if (lineInfo.Count == 9)
                    {
                        GetEmptyVxxxIncreaseInfo(report, tempVessel, lineInfo);
                        continue;
                    }

                    if (lineInfo.Count == 8 && lineInfo[0] != "Filename")
                    {
                        GetDumpToVxxxInfo(report, tempVessel, lineInfo);
                        continue;
                    }

                    if (lineInfo.Count == 7 && lineInfo[0]!="PROCESS")
                    {
                        GetMillingTimeInfo(report, tempVessel, lineInfo);
                    }

                    if (lineInfo.Count == 5)
                    {
                        if (lineInfo[0].ToLower() == "dump")
                        {
                            GetMaterialDataDump(lineInfo, tempVessel, report);
                            continue;
                        }
                    }

                }
                if (report.IsValidBatch)
                {

                    if (!report.OriginalReport.Contains("END-OF-REPORT") && !report.OriginalReport.Contains("ABORTED"))
                    {
                        RecordFault(report, "Batch report was incomplete, possibly due to power spike in the making plant.", null);
                    }

                    if (report.AllVessels.Count < 3)
                    {
                        RecordFault(report, "It appears some of the report is missing, could not obtain all preweigher and mixer information.", null);
                    }
                    FinalCheckForErrors(report);
                }

                CreateWaterIfMissing(report);
                ClearOtherConversionFaultsIfAborted(report);
                SetStreamName(report);
                report.NewMakeTime = report.MakingTime;
                _batchReports.Add(report);

            }

            private void FinalCheckForErrors(BatchReport report)
            {
                if (report.MakingTime == 0)
                {
                    RecordFault(report, "Could not find data such as total make time, batch appears incomplete", null);
                }

                foreach (var vessel in report.AllVessels)
                {
                    if(vessel.VesselName == null && vessel.Materials.Count >= 1)
                    {
                        RecordFault(report, $"Could not retrieve a vessel name it appears some of the report is missing", null);
                    }
                }
            }

        private void SetStreamName(BatchReport report)
            {
            if (report.IsValidBatch)
            {
                Vessel mainMixerVessel = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();

                if (mainMixerVessel != null)
                {
                    switch (mainMixerVessel.VesselName)
                    {
                        case "MainMixer1":
                            report.StreamName = "Main Mixer 1";
                            break;
                        case "MainMixer2":
                            report.StreamName = "Main Mixer 2";
                            break;
                        case "MainMixer3":
                            report.StreamName = "Main Mixer 3";
                            break;
                        case "MainMixer4":
                            report.StreamName = "Main Mixer 4";
                            break;
                        case "MainMixer5":
                            report.StreamName = "Main Mixer 5";
                            break;
                        default:
                            report.StreamName = "Unknown";
                            break;
                    }
                }
            }
            }

            private void ClearOtherConversionFaultsIfAborted(BatchReport report)
            {
                BatchConversionFault tempFault = null;
                bool abortedFound = false;

                foreach (var fault in report.ConversionFaults)
                {
                    if (fault.Message.Contains("aborted"))
                    {
                        tempFault = fault;
                        abortedFound = true;
                    }
                }

                if (abortedFound)
                {
                    report.ConversionFaults.Clear();
                    report.ConversionFaults.Add(tempFault);
                }
            }

            private string GetFileName(string reportText)
            {
                string fileName;
                try
                {
                    int index = reportText.IndexOf("Filename : ") + 11;
                    int length = reportText.LastIndexOf(".txt") - index + 4;
                    fileName = reportText.Substring(index, length);
                }
                catch
                {
                    fileName = "Could not retrieve filename";
                }
                return fileName;
            }

            private void GetMillingTimeInfo(BatchReport report, Vessel tempVessel, List<string> lineInfo)
            {
                string date = report.StartTime.ToShortDateString();
                string time = lineInfo[3];

                if (time.EndsWith("."))
                {
                    time = time.Substring(0, time.Length - 1);
                }

                try
                {
                    Material material = new Material()
                    {
                        Name = lineInfo[0],
                        MillingFinishTime = HelperMethods.AdjustDateIfTimePassesIntoNextDay(date, time, report.StartTime),
                        StartTime = HelperMethods.AdjustDateIfTimePassesIntoNextDay(date, time, report.StartTime),
                        MillingRunTime = Convert.ToDouble(lineInfo[6]),
                    };
                    tempVessel.Materials.Add(material);
                }
                catch (Exception ex)
                {
                    string message = "An error occured when trying to convert " + lineInfo[0] + " time.";
                    RecordFault(report, message, ex.Message);
                }
            }

            private void GetDumpToVxxxInfo(BatchReport report, Vessel tempVessel, List<string> lineInfo)
            {
                try
                {
                    tempVessel.DumpTime = Convert.ToDouble(lineInfo[7]);
                    tempVessel.Decrease = Convert.ToDouble(lineInfo[4]);
                }
                catch (Exception ex)
                {
                    string message = "An error occured when trying to convert vessel " + tempVessel.VesselName + " dump time.";
                    RecordFault(report, message, ex.Message);
                }
            }

            private void GetEmptyVxxxIncreaseInfo(BatchReport report, Vessel tempVessel, List<string> lineInfo)
            {
                try
                {
                    Material material = new Material()
                    {
                        Name = lineInfo[0] + " " + lineInfo[1],
                        ActualWeight = Convert.ToDouble(lineInfo[4]),
                        WaitTime = Convert.ToDouble(lineInfo[5]),
                        WeighTime = Convert.ToDouble(lineInfo[6]),
                        VesselTemp = Convert.ToDouble(lineInfo[7]),
                        AgitatorSpeed = Convert.ToDouble(lineInfo[8]),

                    };

                    Material temp = tempVessel.getSingleMaterialFromList(tempVessel.Materials.Count - 1);
                    material.StartTime = temp.StartTime.AddMinutes(5);
                    tempVessel.Materials.Add(material);
                    //BatchReport.CheckIfCurrentMaterialIsNew(material.Name);

                }
                catch (Exception ex)
                {
                    string message = "An error occured when converting " + lineInfo[0] + " " + lineInfo[1] + ".";
                    RecordFault(report, message, ex.Message);
                }
            }

            private void SetVesselName(BatchReport report, Vessel tempVessel, List<string> lineInfo)
            {
                try
                {
                    tempVessel.VesselName = lineInfo[2];
                    tempVessel.VesselType = SetVesselType(lineInfo[2]);

                    if (tempVessel.VesselName == "Pre-weigher 9")
                    {
                        tempVessel.VesselName = "Pre-weigher 9 Part 2";
                    }
                }
                catch (Exception ex)
                {
                    string message = "An error occured when trying to set the vessel name.";
                    RecordFault(report, message, ex.Message);
                }
            }

            private VesselTypes SetVesselType(string name)
            {
                if (name == "PreWeigher4" || name == "PreWeigher5" || name == "PreWeigher6" || name == "PreWeigher7" || name == "PreWeigher8")
                {
                    return VesselTypes.PerfumePreWeigher;
                }
                else if (name == "PreWeigher1" || name == "PreWeigher2" || name == "PreWeigher3")
                {
                    return VesselTypes.ActivePreWeigher;
                }
                else if (name == "PreWeigher9" || name == "PreWeigher9 Part 1")
                {
                    return VesselTypes.CalciumPreWeigher;
                }
                else if (name == "MainMixer1" || name == "MainMixer2" || name == "MainMixer3" || name == "MainMixer4" || name == "MainMixer5")
                {
                    return VesselTypes.MainMixer;
                }
                else
                {
                    return VesselTypes.None;
                }
            }

        private List<String> FormatBatchStringForInspection(string unformattedString)
            {
                string[] split = unformattedString.Split(' ');

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i].StartsWith(","))
                    {
                        split[i] = split[i].Substring(1);
                    }

                    if (split[i].EndsWith(","))
                    {
                        split[i] = split[i].Substring(0, split[i].Length - 1);
                    }
                }
                return split.Where(x => !string.IsNullOrEmpty(x)
                                                    && x != "C"
                                                    && x != "M"
                                                    && x != "Kg"
                                                    && x != "="
                                                    && x != "decrease").ToList();
            }

            private List<string> SortMaterialNameIntoOneString(List<string> lineInfo)
            {
                if (lineInfo.Count > 12) // Materials with all figures (Target weight to rawmat temp) - 3 parts to name
                {
                    string temp = lineInfo[0] + " " + lineInfo[1] + " " + lineInfo[2];
                    lineInfo.RemoveRange(0, 3);
                    lineInfo.Insert(0, temp);

                }

                if (lineInfo.Count > 11) // Materials with all figures (Target weight to rawmat temp) - 2 parts to name
                {
                    string temp = lineInfo[0] + " " + lineInfo[1];
                    lineInfo.RemoveRange(0, 2);
                    lineInfo.Insert(0, temp);
                }

                return lineInfo;
            }

            private void FinishCurrentTempVessel(BatchReport report, Vessel tempVessel, List<string> lineInfo)
            {

                try
                {
                    tempVessel.TimeCompleted = DateTime.ParseExact(lineInfo[5] + " " + lineInfo[3], "dd-MMM-yy HH:mm:ss", null);
                    report.AllVessels.Add(tempVessel);
                    //tempVessel = new Vessel();
                }
                catch (Exception ex)
                {
                    string message = "Could not finish vessel " + tempVessel.VesselName + ".";
                    RecordFault(report, message, ex.Message);
                }

            }

            private void GetMaterialData(List<string> lineInfo, Vessel vessel, BatchReport report)
            {
                string name = lineInfo[0];

                int count = vessel.Materials.Count(x => x.Name.StartsWith(name));
                if (count >= 1)
                {
                    name += count;
                }

                try
                {
                    Material material = new Material()
                    {
                        Name = name,
                        TargetWeight = Convert.ToDouble(lineInfo[1]),
                        ActualWeight = Convert.ToDouble(lineInfo[2]),
                        StartTime = HelperMethods.AdjustDateIfTimePassesIntoNextDay(report.StartTime.ToShortDateString(), lineInfo[3], report.StartTime),
                        WaitTime = Convert.ToDouble(lineInfo[4]),
                        WeighTime = Convert.ToDouble(lineInfo[5]),
                        VesselBefore = Convert.ToDouble(lineInfo[6]),
                        WeightAfter = Convert.ToDouble(lineInfo[7]),
                        VesselTemp = Convert.ToDouble(lineInfo[8]),
                        AgitatorSpeed = Convert.ToDouble(lineInfo[9]),
                        RawMatTemp = Convert.ToDouble(lineInfo[10])
                    };

                    _materialsFound.AddNewMaterial(material.Name);
                    //BatchReport.CheckIfCurrentMaterialIsNew(material.Name);
                    vessel.Materials.Add(material);

                }
                catch (Exception ex)

                {
                    string message = "Could not convert " + lineInfo[0] + ". Due to incorrect conversion format.";
                    RecordFault(report, message, ex.Message);
                }
            }

            private void RecordFault(BatchReport report, string message, string exceptionMessage)
            {
                BatchConversionFault fault = new BatchConversionFault
                {
                    Campaign = report.Campaign.ToString() + "-" + report.BatchNo,
                    Message = message,
                    Date = DateTime.Now,
                    ExceptionMessage = exceptionMessage,
                    FileName = report.FileName
                };

                if (fault.Campaign == "0-0")
                {
                    fault.Message += "\nBatch Number was not found but the file was " + fault.FileName;
                }

                report.ConversionFaults.Add(fault);
                report.IsValidBatch = false;
            }

            private void GetMaterialDataDump(List<string> lineInfo, Vessel vessel, BatchReport report)
            {
                try
                {
                    Material material = new Material()
                    {
                        Name = lineInfo[0] + " " + lineInfo[1],
                        TargetWeight = Convert.ToDouble(lineInfo[2]),
                        ActualWeight = Convert.ToDouble(lineInfo[3]),
                        StartTime = HelperMethods.AdjustDateIfTimePassesIntoNextDay(report.StartTime.ToShortDateString(), lineInfo[4], report.StartTime),
                        WaitTime = -1,
                        WeighTime = -1,
                        VesselBefore = -1,
                        WeightAfter = -1,
                        VesselTemp = -1,
                        AgitatorSpeed = -1,
                        RawMatTemp = -1
                    };
                    vessel.Materials.Add(material);
                    _materialsFound.AddNewMaterial(material.Name);

                }
                catch (Exception ex)
                {
                    string message = "Dump data for " + lineInfo[0] + " " + lineInfo[1] + " could not be converted due to incorrect format.";
                    RecordFault(report, message, ex.Message);
                }
            }

            private bool FoundBatchMetaData(BatchReport report, string line)
            {
                if (line.ToUpper().Contains("RECIPE") && !line.ToUpper().Contains("WEIGHTS"))
                {
                    try
                    {
                        report.Recipe = GetValueFromLine(line);
                        SetRecipeType(report);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //report.Recipe = null;
                        RecordFault(report, "Could not convert recipe name.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("RECIPE") && line.ToUpper().Contains("WEIGHTS"))
                {
                    try
                    {
                        report.TotalRecipeWeight = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //report.Recipe = null;
                        RecordFault(report, "Could not convert recipe weight.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("CAMPAIGN"))
                {
                    try
                    {
                        report.Campaign = Convert.ToInt32(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert campaign number.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("BATCH NO"))
                {
                    try
                    {
                        report.BatchNo = Convert.ToInt32(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert batch number.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("START TIME"))
                {
                    try
                    {
                        string tempTime = GetValueFromLine(line);
                        //tempTime = tempTime.Substring(0, tempTime.IndexOf(" ")).Trim();
                        string tempDate = line.Substring(line.IndexOf("on") + 2).Trim();
                        string tempDateTime = tempDate + " " + tempTime;
                        report.StartTime = Convert.ToDateTime(tempDateTime);
                        report.WeekNo = HelperMethods.GetWeekNumber(report.StartTime);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert start time.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("MAKING TIME"))
                {
                    try
                    {
                        report.MakingTime = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert making time.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("PRE QA TEMP"))
                {
                    try
                    {
                        report.PreQaTemp = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert Pre QA temp.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("VISCOSITY"))
                {
                    try
                    {
                        report.Visco = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert viscosity.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("TOTAL") && line.ToUpper().Contains("ACTUAL WEIGHTS"))
                {
                    try
                    {
                        report.TotalActualWeight = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert batch weight.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("ALLOCATION"))
                {
                    try
                    {
                        report.StockTankAllocationTime = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert stock allocation time.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("PRE QA TEMP"))
                {
                    try
                    {
                        report.PreQaTemp = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert Pre QA temp.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("QA TIME"))
                {
                    try
                    {
                        report.QATime = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert QA Time.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("PH"))
                {
                    try
                    {
                        report.Ph = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert pH.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("S.G"))
                {
                    try
                    {
                        report.SG = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert S.G.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("APPEARANCE"))
                {
                    try
                    {
                        report.Appearance = GetValueFromLine(line);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert Appearance.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("VISUAL COLOUR"))
                {
                    try
                    {
                        report.VisualColour = GetValueFromLine(line);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert visual colour.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("MEASURED COLOUR"))
                {
                    try
                    {
                        report.MeasuredColour = GetValueFromLine(line);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert measured colour.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("ODOUR CHECK"))
                {
                    try
                    {
                        report.Odour = GetValueFromLine(line);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert odour check.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("QA STATUS"))
                {
                    try
                    {
                        report.OverallQAStatus = GetValueFromLine(line);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert overall QA status.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("ALLOCATED TO"))
                {
                    try
                    {
                        if (report.AllocatedTo == null)
                        {
                            report.AllocatedTo = GetValueFromLine(line);
                        }
                        else
                        {
                            report.AllocatedTo += " & " + GetValueFromLine(line);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert allocated to.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("DROP TIME"))
                {
                    try
                    {
                        report.DropTime = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert drop time.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("TOTAL RECIPE"))
                {
                    try
                    {
                        report.TotalRecipeWeight = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert total recipe weight.", ex.Message);
                    }
                }
                if (line.ToUpper().Contains("VESSEL WEIGHT INCREASE"))
                {
                    try
                    {
                        report.VesselWeightIncrease = Convert.ToDouble(GetValueFromLine(line));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RecordFault(report, "Could not convert vessel weight increase.", ex.Message);
                    }
                }



                return false;
            }

            private void SetRecipeType(BatchReport report)
            {
                if (report.Recipe.StartsWith("BB-"))
                {
                    report.RecipeType = RecipeTypes.BigBang;
                }
                else if (report.Recipe.StartsWith("RE-"))
                {
                    report.RecipeType = RecipeTypes.Reg;
                }
                else
                {
                    report.RecipeType = RecipeTypes.Conc;
                }
            }

            private string GetValueFromLine(string input)
            {

            int index = input.IndexOf(":");
            input = input.Substring(index + 1).TrimStart();

                if (input.Contains(" ") && !input.Contains("Recipe"))
                {
                    input = input.Substring(0, input.IndexOf(" ")).Trim();
                }
                
                return input;
                
            }

            private void CreateWaterIfMissing(BatchReport report)
            {
                // This function was written because some reports appear to have water missing and it causes faults in reporting

                if (report.IsValidBatch)
                {
                    Vessel mainMixer = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();

                if (mainMixer != null)
                {
                    if (!mainMixer.Materials.Any(x => x.Name == "Material 63"))
                    {
                        mainMixer.Materials.Add(CalculateWaterDetails("Material 63", report));
                        AddBatchAdjustment(report, "hot water was added to the batch as it was missing from the original");
                    }
                    if (!mainMixer.Materials.Any(x => x.Name == "Material 67"))
                    {
                        mainMixer.Materials.Add(CalculateWaterDetails("Material 67", report));
                        AddBatchAdjustment(report, "cold water was added to the batch as it was missing from the original");
                    }
                }
                }
            }

            private void AddBatchAdjustment(BatchReport report, string message)
            {
                report.IsBatchAdjusted = true;
                report.BatchIssues.Add(new BatchIssue
                {
                    FaultType = BatchIssue.FaultTypes.BatchAdjusted,
                    Message = message,
                    TimeLost = 0,
                    WeightDiffference = 0,
                    IssueCreatedBy = "BatchDataFileManager - Function CreateWaterIfMissing",
                    MaterialName = "WaterMissing",

                });
            }

            private Material CalculateWaterDetails(string nameOfMaterial, BatchReport report)
            {
                Vessel mainMixer = report.AllVessels.Where(x => x.VesselType == Vessel.VesselTypes.MainMixer).FirstOrDefault();

                double totalWeightsOfMaterials = BatchHelperMethods.CountActualWeightOfAllMaterialsInVessel(mainMixer);
                double totalVesselWeight = report.TotalActualWeight * 1000;
                double weightOfMissingWater = totalVesselWeight - totalWeightsOfMaterials;
                double rawMatTemp = nameOfMaterial == "Material 63" ? 80.0 : 20;
                double vesselTemp = nameOfMaterial == "Material 63" ? 77 : 44;
                DateTime StartTimeOfHCL = BatchHelperMethods.GetSingleMaterialFromVessel(report, "Material 59").StartTime;

                return new Material
                {
                    Name = nameOfMaterial,
                    ActualWeight = weightOfMissingWater,
                    StartTime = StartTimeOfHCL.Subtract(new TimeSpan(0, 0, 6, 0, 0)),
                    AgitatorSpeed = 11,
                    RawMatTemp = rawMatTemp,
                    TargetWeight = weightOfMissingWater,
                    WeighTime = 5,
                    VesselTemp = vesselTemp,
                    WaitTime = 0.1
                };
            }
        }
    }





