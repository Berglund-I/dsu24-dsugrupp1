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


        public int AgeAtVaccinationdDate { get; set; }



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

        /// <summary>
        /// A method that calculates the percentage of vaccinated and unvaccinated women and men.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CountVaccinatedGenderPercent() 
        {
            if (PopulationFemales <= 0)
            {
                throw new Exception("Antalet kvinnor kan ej vara noll");
            }
            if (PopulationMales <= 0)
            {
                throw new Exception("Antalet män kan ej vara noll");
            }
            vaccinatedFemalesPercent = Math.Round((double)VaccinatedFemales / PopulationFemales * 100, 2);
            vaccinatedMalesPercent = Math.Round((double)VaccinatedMales / PopulationMales * 100, 2);
            notVaccinatedFemalesPercent = Math.Round(100 - vaccinatedFemalesPercent, 2);
            notVaccinatedMalesPercent = Math.Round(100 - vaccinatedMalesPercent, 2);

        }


        /// <summary>
        /// A method that generates a Chart for the vaccination percentage of women.
        /// </summary>
        /// <returns></returns>
        public ChartViewModel GenerateChartFemales()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccination för kön", 
                type: "pie",
                labels: ["Vaccinerade kvinnor i procent", "Ovaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad bland kvinnor",
                data: [vaccinatedFemalesPercent, notVaccinatedFemalesPercent],
                bgcolor: ["rgb(119, 0, 255)", "rgb(119, 0, 255)"], 5);
            chart.JsonChart = JsonConvert.SerializeObject(chart.Chart).ToLower();
            return chart;
        }


        /// <summary>
        /// A method that checks through the patients in different desos and sorts them into women and men.
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
