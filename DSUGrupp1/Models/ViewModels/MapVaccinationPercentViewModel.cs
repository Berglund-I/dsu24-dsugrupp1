using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DSUGrupp1.Models.ViewModels
{
    public class MapVaccinationPercentViewModel
    {
        public Dictionary<string, double> VaccinationPercentDeso { get; set; } = new Dictionary<string, double>();

        public string JsonVaccinationPercent { get; set; }
        public int TotalPatients { get; set; }
        public int Population { get; set; }


        private readonly ApiController _apiController = new ApiController();
        private readonly VaccinationViewModel _vaccinationViewModel = new VaccinationViewModel();



        public MapVaccinationPercentViewModel()
        {


        }

        public async Task InitializeAsync(List<VaccinationDataFromSpecificDeSoDto> desoList)
        {
            await SetValuesForVaccinationList(desoList);
            JsonVaccinationPercent = JsonConvert.SerializeObject(VaccinationPercentDeso);
        }

        private async Task SetValuesForVaccinationList(List<VaccinationDataFromSpecificDeSoDto> desoList)
        {
            foreach (var deso in desoList)
            {
                var populationMales = await _apiController.GetPopulationInSpecificDeSo(deso.Meta.DeSoCode, "2022", "1");
                var populationFemales = await _apiController.GetPopulationInSpecificDeSo(deso.Meta.DeSoCode, "2022", "2");
                Population = int.Parse(populationMales.Data[0].Values[0]) + int.Parse(populationFemales.Data[0].Values[0]);

                var vaccinationDataResponse = await _apiController.GetVaccinationDataFromDeSo(deso.Meta.DeSoCode);
                TotalPatients = vaccinationDataResponse.Meta.TotalRecordsPatients;

                int doseCount = CalculateDoseCounts(vaccinationDataResponse);
                double vaccinationPercent = _vaccinationViewModel.CalculateVaccinationPercentage(Population, doseCount);

                VaccinationPercentDeso.Add(deso.Meta.DeSoCode, vaccinationPercent);
            }

        }

        /// <summary>
        /// Sets values for dose 1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int CalculateDoseCounts(VaccinationDataFromSpecificDeSoDto data)
        {
            
            int doseCount = 0;

            for (int i = 0; i < data.Patients.Count(); i++)
            {
                foreach (var dose in data.Patients[i].Vaccinations)
                {
                    if (dose.DoseNumber == 1)
                    {
                        doseCount++;
                    }

                }
            }

            return doseCount;
        }

    }
}
