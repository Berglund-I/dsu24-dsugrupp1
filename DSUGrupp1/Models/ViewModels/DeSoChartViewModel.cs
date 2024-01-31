using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class DeSoChartViewModel
    {
        private readonly ChartViewModel _chartViewModel = new ChartViewModel();
        private readonly ApiController _apiController = new ApiController();
        //private readonly DisplayAgeStatisticsViewModel _displayAgeStatisticsViewModel = new DisplayAgeStatisticsViewModel();
        public string Id { get; set; } = "10";
        //public ChartViewModel ChartViewModel { get; set; }
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }

        public DeSoChartViewModel(string deSoCode)
        {
            var chart = GetSetValuesForChart(deSoCode);
            //Chart = new Chart();
            //JsonChart = JsonConvert.SerializeObject(Chart).ToLower();
        }

        private async Task<Chart> GetSetValuesForChart(string deSoCode)
        {
            var model = await _apiController.GetVaccinationDataFromDeSo(deSoCode);

            var model2 = await _apiController.GetPopulationInSpecificDeSo(deSoCode, "2022");

            int population = int.Parse(model2.Data[0].Values[0]);
            int totalPatients = model.Meta.TotalRecordsPatients;

            CalculateDoseCounts(model);

            return null;
        }    

        private void CalculateDoseCounts(VaccinationDataFromSpecificDeSoDto data)
        {
            for(int i = 0; i < data.Patients.Count(); i++)
            {
                foreach(var dose in data.Patients[i].Vaccinations)
                {

                }
                //for (int j = 0; j < ); j++)
                //{

                //}
            }
        //    foreach (var data in VaccinationDataFromSpecificDeso)
        //    {
        //        foreach (var patient in data.Patients)
        //        {
        //            foreach (var vaccination in patient.Vaccinations)
        //            {


        //                int ageWhenVaccinated = DateTime.Parse(vaccination.DateOfVaccination).Year - int.Parse(patient.YearOfBirth);


        //                string ageGroup = DetermineAgeGroup(ageWhenVaccinated);
        //                bool doesContainAgeGroup = AgeGroupDoseCounts.ContainsKey(ageGroup);


        //                if (!doesContainAgeGroup)
        //                {
        //                    AgeGroupDoseCounts[ageGroup] = new AgeGroupDoseCounts();
        //                }

        //                if (vaccination.DoseNumber == 1)
        //                {
        //                    AgeGroupDoseCounts[ageGroup].FirstDoseCount++;
        //                }
        //                else if (vaccination.DoseNumber == 2)
        //                {
        //                    AgeGroupDoseCounts[ageGroup].SecondDoseCount++;
        //                }
        //                else if (vaccination.DoseNumber == 3)
        //                {
        //                    AgeGroupDoseCounts[ageGroup].BoosterDoseCount++;
        //                }


        //                if (vaccination.DoseNumber == 3)
        //                {
        //                    break;
        //                }
        //            }
        //        }


            //}
        }
    }
}
