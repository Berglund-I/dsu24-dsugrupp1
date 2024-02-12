using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DSUGrupp1.Models.ViewModels
{
    public class DeSoChartViewModel
    {
        private readonly ChartViewModel _chartViewModel = new ChartViewModel();
        private readonly ApiController _apiController = new ApiController();
        private readonly VaccinationViewModel _vaccinationViewModel = new VaccinationViewModel();
        private readonly DisplayGenderStatisticsViewModel _displayGenderStatistics = new DisplayGenderStatisticsViewModel();
        private readonly VaccinationOverTimeViewModel _vaccinationOverTimeViewModel = new VaccinationOverTimeViewModel();
       

        public string JsonChartDose { get; set; }
        public string JsonChartGender { get; set; }
        public int Population {  get; set; }
        public int TotalPatients { get; set; }
        public int DoseOne { get; set; }
        public int DoseTwo { get; set; }
        public int DoseThree { get; set; }
        public int TotalInjections { get; set; }
        public List<double> TotalPopulationVaccinationPercentage { get; set; }
        public double VaccinatedMalesPercentage { get; set; }
        public double VaccinatedFemalesPercentage { get; set; }
        public double NotVaccinatedMalesPercentage { get; set; }
        public double NotVaccinatedFemalesPercentage { get; set; }
        public int VaccinatedMales { get; set; }
        public int VaccinatedFemales { get; set; }
        public string SelectedDeSo {  get; set; }
        public string JsonChartVaccinationOverTime { get; set; }
        public List<DatasetsDto> Datasets { get; set; }
        public List<Batch> Batches { get; set; }
        public List<Patient> Patients { get; set; }




        public DeSoChartViewModel(string deSoCode)
        {
            SelectedDeSo = deSoCode;
            //var chartValues = GetSetValuesForChart(deSoCode);

            //JsonChartDose = _chartViewModel.SerializeJson(chartValues.Result);


            if (GetSetValuesForChart(deSoCode).Result)
            {
                var chart = GetChartDose(/*chartValues.Result*/);
                var chartTwo = GetChartGender(/*chartValues.Result*/);
                var chartThree = GetChartOverTime(/*chartValues.Result*/);

                JsonChartDose = _chartViewModel.SerializeJson(chart);
                JsonChartGender = _chartViewModel.SerializeJson(chartTwo);
                JsonChartVaccinationOverTime = _chartViewModel.SerializeJson(chartThree);
            };

            //var chart = GetChartDose(/*chartValues.Result*/);

            //JsonChartDose = _chartViewModel.SerializeJson(chart);


        }

        /// <summary>
        /// Sets data for the chart that displays doses in a DeSo
        /// </summary>
        /// <returns></returns>
        private Chart GetChartDose(/*Chart chartValues*/)
        {
           
            List<string> labels = new List<string>()
                {
                    "1 Dos",
                    "2 Doser",
                    "3 eller fler Doser"
                };

            List<string> colors = new List<string>()
                {
                    "#3e95cd",
                    "#8e5ea2",
                    "#3cba9f"
                };
            Chart chart = _chartViewModel.CreateChart("Vaccinationsgrad i området: ", "bar", labels, "Procentuell vaccinationsgrad", TotalPopulationVaccinationPercentage, colors, 5);
            return chart;

        }
        /// <summary>
        /// Sets data for the chart that displays gender allocation in a DeSo
        /// </summary>
        /// <returns></returns>
        private Chart GetChartGender(/*Chart chartValues*/)
        {

            List<string> labels = new List<string>()
                {
                    "Vaccinerade män", 
                    "Vaccinerade kvinnor",  
                };

            List<string> colors = new List<string>()
                {
                    "#3e95cd",
                    "#8e5ea2",
                };
            Chart chart = _chartViewModel.CreateChart("Vaccinationer i området fördelat över könen: ", "pie", labels, "Vaccinationer", [VaccinatedMales, VaccinatedFemales], colors, 5);
            return chart;

        }
      

        private Chart GetChartOverTime()
        {
            List<string> weekLabels = Enumerable.Range(1, 52).Select(i => i.ToString()).ToList();
           
            
            Chart chart = _chartViewModel.CreateMultiSetChart("Antal vaccinationer per vecka", "line", weekLabels, Datasets);
            return chart;
        }


        //public void GetPatient(VaccinationDataFromSpecificDeSoDto patientData, DoseTypeDto doseData)
        //{
        //    Patients = new List<Patient>();
        //    foreach (var p in patientData.Patients)
        //    {
        //        Patient patient = new Patient(p, doseData);
        //        Patients.Add(patient);
        //    }          
        //}

      
        /// <summary>
        /// Gets and sets values for the class properties
        /// </summary>
        /// <param name="deSoCode"></param>
        /// <returns></returns>
        private async Task<bool> GetSetValuesForChart(string deSoCode)
        {
            var vaccinationDataResponse = await _apiController.GetVaccinationDataFromDeSo(deSoCode);
            var populationMales = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022", "1");
            var populationFemales = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022", "2");
            var getBatches = await _apiController.GetDoseTypes();

            //GetPatient(vaccinationDataResponse, getBatches);

            Population = int.Parse(populationMales.Data[0].Values[0]) + int.Parse(populationFemales.Data[0].Values[0]);
            TotalPatients = vaccinationDataResponse.Meta.TotalRecordsPatients;

            List<int> vaccinatedGender = _displayGenderStatistics.CountVaccinatedGender(vaccinationDataResponse);
            List<double> vaccinatedGenderPercent = _displayGenderStatistics.CountVaccinatedGenderPercent(int.Parse(populationMales.Data[0].Values[0]), int.Parse(populationFemales.Data[0].Values[0]), vaccinatedGender[0], vaccinatedGender[1]);

            VaccinatedMales = vaccinatedGender[0];
            VaccinatedFemales = vaccinatedGender[1];

            VaccinatedMalesPercentage = vaccinatedGenderPercent[0];
            VaccinatedFemalesPercentage = vaccinatedGenderPercent[1];
            NotVaccinatedMalesPercentage = vaccinatedGenderPercent[2];
            NotVaccinatedFemalesPercentage = vaccinatedGenderPercent[3];

            var doseCount = CalculateDoseCounts(vaccinationDataResponse);
            DoseOne = doseCount[0];
            DoseTwo = doseCount[1];
            DoseThree = doseCount[2];
            TotalInjections = doseCount[0] + doseCount[1] + doseCount[2] + doseCount[3];


            List<double> vaccinationPercentage = new List<double>();

            for (int i = 0; i < doseCount.Count() - 1; i++)
            {
                vaccinationPercentage.Add(_vaccinationViewModel.CalculateVaccinationPercentage(Population, doseCount[i]));
            }
            TotalPopulationVaccinationPercentage = vaccinationPercentage;

            GetBatches(vaccinationDataResponse);
       
          
            List<DatasetsDto> datasets = new List<DatasetsDto>();
            // Lägg till logik för att fylla diagrammet med data över tid
            for (int year = 2020; year <= 2023; year++)
            {
                List<double> vaccinationsPerWeek = CountVaccinationsWeekByWeek(vaccinationDataResponse, year);
                string color = ChartViewModel.GenerateRandomColor();
                DatasetsDto dataset = _chartViewModel.GenerateDataSet(
                    DatasetLabel: $"{year}",
                    data: vaccinationsPerWeek,
                    bgcolor: new List<string> { color },
                    bColor: color,
                    bWidth: 3
                );

                datasets.Add(dataset);
            }

            Datasets = datasets;
                





            return true;
        }

        private List<double> CountVaccinationsWeekByWeek(VaccinationDataFromSpecificDeSoDto vaccinationDataFromSpecificDeSoDto, int year)
        {
            var ci = new CultureInfo("sv-SE");
            var cal = ci.Calendar;
            var lastDayOfYear = new DateTime(year, 12, 31);
            int weeksInYear = cal.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Preprocess to get vaccination dates for the given year
            var vaccinationsInYear = StoreVaccinationsByYear(vaccinationDataFromSpecificDeSoDto, year.ToString());
            var vaccinationsPerWeek = new List<double>();

            var parsedVaccinationDates = vaccinationsInYear
            .Select(dateString => DateTime.TryParse(dateString, out DateTime date) ? (DateTime?)date : null)
            .Select(date => date.Value)
            .ToList();

            List<double> vaccinationCounts = new List<double>();

            for (int week = 1; week <= weeksInYear; week++)
            {
                var weekStart = FirstDateOfWeek(year, week, ci);
                var weekEnd = weekStart.AddDays(6);

                var vaccinationCountThisWeek = parsedVaccinationDates.Count(date => date >= weekStart && date <= weekEnd);
                vaccinationCounts.Add(vaccinationCountThisWeek);
            }
            return vaccinationCounts;
        }
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            var calendar = ci.Calendar;
            var firstWeek = calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            int weekOfYearMultiplier = weekOfYear - 1;

            if (firstWeek >= 52)
            {
                firstWeekDay = jan1.AddDays(daysOffset + 7);
            }


            return firstWeekDay.AddDays(weekOfYearMultiplier * 7);
        }
        public List<string> StoreVaccinationsByYear(VaccinationDataFromSpecificDeSoDto vaccinationDataFromSpecificDeSoDto, string year)
        {
            List<string> vaccinationsInGivenYear = new List<string>();

            foreach (var deSoCode in vaccinationDataFromSpecificDeSoDto.Meta.DeSoCode)
            {
                if(vaccinationDataFromSpecificDeSoDto.Meta.DeSoCode == SelectedDeSo)
                {
                    foreach(var patient in vaccinationDataFromSpecificDeSoDto.Patients)
                    {
                        foreach(var vaccination in patient.Vaccinations)
                        {
                            if (DateTime.TryParse(vaccination.DateOfVaccination, out DateTime vaccinationDate))
                            {
                                if (vaccinationDate.Year.ToString() == year)
                                {
                                    vaccinationsInGivenYear.Add(vaccination.DateOfVaccination);
                                }

                            }
                        }
                    }
                }
            }

            return vaccinationsInGivenYear;
        }
        

        /// <summary>
        /// Sets values for dose 1, 2, 3 and booster
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<int> CalculateDoseCounts(VaccinationDataFromSpecificDeSoDto data)
        {
            List<int> doseCount = new List<int>();
            int doseOne = 0;
            int doseTwo = 0;
            int doseThree = 0;
            int booster = 0;

            for (int i = 0; i < data.Patients.Count(); i++)
            {
                foreach(var dose in data.Patients[i].Vaccinations)
                {
                    if(dose.DoseNumber == 1)
                    {
                        doseOne++; 
                    }
                    else if (dose.DoseNumber == 2)
                    {
                        doseTwo++;
                    }
                    else if (dose.DoseNumber == 3)
                    {
                        doseThree++;                   
                    }
                    else if (dose.DoseNumber > 3)
                    {
                        booster++;
                    }
                }             
            }
            doseCount.Add(doseOne);
            doseCount.Add(doseTwo);
            doseCount.Add(doseThree);
            doseCount.Add(booster);

            return doseCount;
        }

        /// <summary>
        /// Gets all used batches in deSo and gender allocation
        /// </summary>
        /// <param name="data"></param>
        public void GetBatches(VaccinationDataFromSpecificDeSoDto data)
        {
            Dictionary<string, Batch> batches = new Dictionary<string, Batch>();
            
            foreach(var patients in data.Patients)
            {
                
                for(int i = 0; i < patients.Vaccinations.Count(); i++)
                {
                    string batchNumber = patients.Vaccinations[i].BatchNumber;

                    if (!batches.ContainsKey(batchNumber))
                    {
                        Batch batch = new Batch()
                        {
                            BatchNumber = batchNumber                           
                        };
                        if (patients.Gender == "Male")
                        {
                            batch.Male++;
                        }
                        else
                        {
                            batch.Female++;
                        }
                        batches.Add(batchNumber, batch);
                    }
                    else
                    {
                        Batch existingBatch = batches[batchNumber];

                        if (patients.Gender == "Male")
                        {
                            existingBatch.Male++;
                        }
                        else
                        {
                            existingBatch.Female++;
                        }
                    }
                }                                      
            }
            Batches = batches.Values.ToList();
        }


    }
}
