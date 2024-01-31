using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class DisplayGenderStatisticsViewModel
    {
        public int PopulationFemales { get; set; }
        public int VaccinatedFemales { get; set; }
        public int PopulationMales { get; set; }
        public int VaccinatedMales { get; set; }



        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto = null;
        private double vaccinatedFemalesPercent;
        private double vaccinatedMalesPercent;
        private double notVaccinatedFemalesPercent;
        private double notVaccinatedMalesPercent;


        public DisplayGenderStatisticsViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDto)
        {
            PopulationMales = int.Parse(population.Data[0].Values[0]);
            PopulationFemales = int.Parse(population.Data[1].Values[0]);
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDto;
            CountVaccinatedGender();
            CountVaccinatedGenderPercent();


        }

        public void CountVaccinatedGenderPercent() 
        {
            vaccinatedFemalesPercent = Math.Round((double)VaccinatedFemales / PopulationFemales * 100, 2);
            vaccinatedMalesPercent = Math.Round((double)VaccinatedMales / PopulationMales * 100, 2);
            notVaccinatedFemalesPercent = Math.Round(100 - vaccinatedFemalesPercent, 2);
            notVaccinatedMalesPercent = Math.Round(100 - vaccinatedMalesPercent, 2);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ChartViewModel> GenerateChart()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                type: "pie",
                labels: ["Vaccinerade kvinnor i procent", "Ovaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad bland kvinnor",
                data: [vaccinatedFemalesPercent, notVaccinatedFemalesPercent],
                bgcolor: ["rgb(119, 0, 255)", "rgb(119, 0, 255)"], 5);
            chart.JsonChart = JsonConvert.SerializeObject(chart.Chart).ToLower();
            return chart;
        }


        /// <summary>
        /// This method counts the number of vaccinated males and females by iterating through vaccination data for specific Deso codes. 
        /// It checks the gender of each patient and increments counters accordingly, returning an array with the count of vaccinated males and females.
        /// </summary>
        /// <returns></returns>
        public void CountVaccinatedGender()
        {
            foreach (var list in _vaccinationDataFromSpecificDeSoDto)
            {
                foreach (var patient in list.Patients)
                {
                    if (patient.Gender == "Male")
                    {
                        VaccinatedMales++;
                    }
                    else if (patient.Gender == "Female")
                    {
                        VaccinatedFemales++;
                    }
                }
            }
        }

    }
}
