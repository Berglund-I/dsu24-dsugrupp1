using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using System.Drawing;
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
        public string JsonChartFilter { get; set; }
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




        public DeSoChartViewModel(string deSoCode, List<Patient> patients)
        {
            SelectedDeSo = deSoCode;
            Patients = patients;

            if (GetSetValuesForChart(deSoCode).Result)
            {
                var chart = GetChartDose(/*chartValues.Result*/);
                var chartTwo = GetChartGender(/*chartValues.Result*/);
                var chartThree = GetChartFilter(/*chartValues.Result*/);
                var chartFour = GetChartOverTime(/*chartValues.Result*/);

                JsonChartDose = _chartViewModel.SerializeJson(chart);
                JsonChartGender = _chartViewModel.SerializeJson(chartTwo);
                JsonChartFilter = _chartViewModel.SerializeJson(chartThree);
                JsonChartVaccinationOverTime = _chartViewModel.SerializeJson(chartFour);
            };
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
      
        /// <summary>
        /// Generates a chart for vaccinations by week for the selected deso
        /// </summary>
        /// <returns></returns>
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
        /// Sets data for the chart that displays the first filter charts on the detail site
        /// </summary>
        /// <returns></returns>
        private Chart GetChartFilter(/*Chart chartValues*/)
        {
            List<string> labels = new List<string>()
            {
                "Test",
                "Test",
            };
            List<string> colors = new List<string>()
            {
                "#ffffff",
                "#000000",
            };
            Chart chart = _chartViewModel.CreateChart("Hejsan hoppsan ", "bar", labels, "Test", [VaccinatedMales, VaccinatedFemales], colors, 5);
            return chart;
        }

        /// <summary>
        /// Gets and sets values for the class properties
        /// </summary>
        /// <param name="deSoCode"></param>
        /// <returns></returns>
        private async Task<bool> GetSetValuesForChart(string deSoCode)
        {
            var populationMales = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022", "1");
            var populationFemales = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022", "2");

            var desoPatients = LinqQueryRepository.GetPatientsByDeSo(Patients, deSoCode);
            var getBatches = await _apiController.GetDoseTypes();

            Population = int.Parse(populationMales.Data[0].Values[0]) + int.Parse(populationFemales.Data[0].Values[0]);
            TotalPatients = desoPatients.Count();

            VaccinatedMales = LinqQueryRepository.GetPatientsByGender(desoPatients, "Male").Count();
            VaccinatedFemales = LinqQueryRepository.GetPatientsByGender(desoPatients, "Female").Count();

            List<double> vaccinatedGenderPercent = _displayGenderStatistics.CountVaccinatedGenderPercent(
                int.Parse(populationMales.Data[0].Values[0]), 
                int.Parse(populationFemales.Data[0].Values[0]), 
                VaccinatedMales, 
                VaccinatedFemales);

            VaccinatedMalesPercentage = vaccinatedGenderPercent[0];
            VaccinatedFemalesPercentage = vaccinatedGenderPercent[1];
            NotVaccinatedMalesPercentage = vaccinatedGenderPercent[2];
            NotVaccinatedFemalesPercentage = vaccinatedGenderPercent[3];

            var patientsWithBooster = LinqQueryRepository.GetPatientsWithBoosterDose(desoPatients);
            
            //Consider adding to LinqQueryRepository
            int totalBoosterDoses = patientsWithBooster
                .Sum(patient => patient.Vaccinations.Count(vaccination => vaccination.DoseNumber > 3));

            List<int> doseCount =
            [
                LinqQueryRepository.GetPatientsByDoseNumber(desoPatients,1).Count(),
                LinqQueryRepository.GetPatientsByDoseNumber(desoPatients, 2).Count(),
                LinqQueryRepository.GetPatientsByDoseNumber(desoPatients, 3).Count(),
                totalBoosterDoses,
            ];
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

            GetBatches(desoPatients);
       
          
            List<DatasetsDto> datasets = new List<DatasetsDto>();
            List<string> colors = new List<string>()
            {
                "#70e000",
                "#006466",
                "#8900f2",
                "#f20089",
            };


            for (int year = 2020; year <= 2023; year++)
            {
                List<double> vaccinationsPerWeek = VaccinationOverTimeViewModel.CountVaccinationsWeekByWeek(year,desoPatients);
                string color = colors[year - 2020];
                DatasetsDto dataset = _chartViewModel.GenerateDataSet(
                    DatasetLabel: $"{year}",
                    data: vaccinationsPerWeek,
                    bgcolor: new List<string> {color},
                    bColor: color,
                    bWidth: 3
                );
                datasets.Add(dataset);
            }

            Datasets = datasets;
            return true;
        }

        /// <summary>
        /// Gets all used batches in deSo and gender allocation.
        /// </summary>
        /// <param name="patients"></param>
        public void GetBatches(List<Patient> patients)
        {
            var batches = patients
                .SelectMany(patient => patient.Vaccinations.Select(vaccination => new { patient.Gender, vaccination.BatchNumber }))
                .GroupBy(x => x.BatchNumber)
                .Select(group => new Batch
                {
                    BatchNumber = group.Key,
                    Male = group.Count(x => x.Gender == "Male"),
                    Female = group.Count(x => x.Gender == "Female")
                })
                .ToList();

            Batches = batches; 
        }       
    }
}
