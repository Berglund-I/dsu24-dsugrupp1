using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using System.Globalization;

namespace DSUGrupp1.Models.ViewModels
{
    public class DeSoChartViewModel
    {
        private readonly ChartViewModel _chartViewModel = new ChartViewModel();
        private readonly ApiController _apiController = new ApiController();
        private readonly VaccinationViewModel _vaccinationViewModel = new VaccinationViewModel();
        private readonly DisplayGenderStatisticsViewModel _displayGenderStatistics = new DisplayGenderStatisticsViewModel();

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
        public List<Batch> Batches { get; set; }



        public DeSoChartViewModel(string deSoCode)
        {

            //var chartValues = GetSetValuesForChart(deSoCode);

            //JsonChartDose = _chartViewModel.SerializeJson(chartValues.Result);

            if (GetSetValuesForChart(deSoCode).Result)
            {
                var chart = GetChartDose(/*chartValues.Result*/);
                var chartTwo = GetChartGender(/*chartValues.Result*/);
                var chartThree = GetChartFilter(/*chartValues.Result*/);

                JsonChartDose = _chartViewModel.SerializeJson(chart);
                JsonChartGender = _chartViewModel.SerializeJson(chartTwo);
                JsonChartFilter = _chartViewModel.SerializeJson(chartThree);
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
            Chart chart = _chartViewModel.CreateChart("Test test test: ", "bar", labels, "Test", [VaccinatedMales, VaccinatedFemales], colors, 5);
            return chart;
        }

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
           
            return true;
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
