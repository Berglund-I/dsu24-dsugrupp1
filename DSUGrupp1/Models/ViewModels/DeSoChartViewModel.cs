using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class DeSoChartViewModel
    {
        private readonly ChartViewModel _chartViewModel = new ChartViewModel();
        private readonly ApiController _apiController = new ApiController();

        public string Id { get; set; } = "10";
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }
        public int Population {  get; set; }
        public int TotalPatients { get; set; }
        public int DoseOne { get; set; }
        public int DoseTwo { get; set; }
        public int DoseThree { get; set; }
        public int TotalInjections { get; set; }
        public int TotalVaccinations { get; set; }
        public DeSoChartViewModel(string deSoCode)
        {
            var chart = GetSetValuesForChart(deSoCode);
            JsonChart = _chartViewModel.SerializeJson(chart.Result);
        }
        private async Task<Chart> GetSetValuesForChart(string deSoCode)
        {
            var vaccinationDataResponse = await _apiController.GetVaccinationDataFromDeSo(deSoCode);
            var populationDataResponse = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022");

            Population = int.Parse(populationDataResponse.Data[0].Values[0]);
            TotalPatients = vaccinationDataResponse.Meta.TotalRecordsPatients;

            var doseCount = CalculateDoseCounts(vaccinationDataResponse);
            DoseOne = doseCount[0];
            DoseTwo = doseCount[1];
            DoseThree = doseCount[2];
            TotalInjections = doseCount[0] + doseCount[1] + doseCount[2] + doseCount[3];

            List<string> labels = new List<string>()
            {
                "1 Dos",
                "2 Doser",
                "3 eller fler Doser"
            };
            List<double> values = new List<double>()
            {
                DoseOne,
                DoseTwo,
                DoseThree,
            };
            List<string> colors = new List<string>()
            {
                "#3e95cd",
                "#8e5ea2",
                "#3cba9f"
            };
            Chart chart = _chartViewModel.CreateChart("Vaccinationsgrad i område: ", "bar", labels, "Borde ändras till lista?", values, colors, 5);
            return chart;
        }    

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
    }
}
