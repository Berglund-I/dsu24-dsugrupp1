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
                text: "Vaccinationsgrad i % hos kvinnor", 
                type: "pie",
                labels: ["Vaccinerade kvinnor i procent", "Ovaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad bland kvinnor",
                data: [vaccinatedFemalesPercent, notVaccinatedFemalesPercent],
                bgcolor: ["rgb(178, 102, 255)", "rgb(255, 153, 204)"], 3);
                chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// A method that generates a Chart for the vaccination percentage of men.
        /// </summary>
        /// <returns></returns>
        public ChartViewModel GenerateChartMales()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % hos män",
                type: "pie",
                labels: ["Vaccinerade män i procent", "Ovaccinerade män i procent"],
                DatasetLabel: "Vaccinationsgrad bland män",
                data: [vaccinatedMalesPercent, notVaccinatedMalesPercent],
                bgcolor: ["rgb(0, 204, 0)", "rgb(0, 102, 204)"], 3);
                chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// A method that generates a Chart for the vaccination percentage of women and men.
        /// </summary>
        /// <returns></returns>
        public ChartViewModel GenerateChartBothGenders()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % mellan könen",
                type: "pie",
                labels: ["Vaccinerade män i procent", "Vaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad mellan könen",
                data: [vaccinatedMalesPercent, vaccinatedFemalesPercent],
                bgcolor: ["rgb(0, 76, 153)", "rgb(255, 102, 178)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
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
