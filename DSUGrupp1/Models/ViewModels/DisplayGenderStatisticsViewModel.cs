using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{

    public class DisplayGenderStatisticsViewModel
    {
        public int PopulationFemales { get; set; }
        public int VaccinatedFemales { get; set; }
        public int PopulationMales { get; set; }
        public int VaccinatedMales { get; set; }

        private double _vaccinatedFemalesPercent;
        private double _vaccinatedMalesPercent;
        private double _notVaccinatedFemalesPercent;
        private double _notVaccinatedMalesPercent;

        public DisplayGenderStatisticsViewModel() { }

        public DisplayGenderStatisticsViewModel(PopulationDto population, List<Patient> patients)
        {
            PopulationMales = int.Parse(population.Data[0].Values[0]);
            PopulationFemales = int.Parse(population.Data[1].Values[0]);
        
            VaccinatedMales = LinqQueryRepository.GetPatientsByGender(patients, "Male").Count;
            VaccinatedFemales = LinqQueryRepository.GetPatientsByGender(patients, "Female").Count;

            CountVaccinatedGenderPercent(PopulationMales, PopulationFemales, VaccinatedMales, VaccinatedFemales);
        }
        /// <summary>
        /// Calculates the percentage of vaccinated males/females
        /// </summary>
        /// <param name="populationMales"></param>
        /// <param name="populationFemales"></param>
        /// <param name="vaccinatedMales"></param>
        /// <param name="vaccinatedFemales"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<double> CountVaccinatedGenderPercent(int populationMales, int populationFemales, int vaccinatedMales, int vaccinatedFemales)
        {
            if (populationFemales <= 0 || populationMales <= 0)
            {
                throw new Exception("Population cannot be zero");
            }

            List<double> vaccinationPercent = new List<double>();
            _vaccinatedMalesPercent = Math.Round((double)vaccinatedMales / populationMales * 100, 2);
            _vaccinatedFemalesPercent = Math.Round((double)vaccinatedFemales / populationFemales * 100, 2);
            _notVaccinatedMalesPercent = Math.Round(100 - _vaccinatedMalesPercent, 2);
            _notVaccinatedFemalesPercent = Math.Round(100 - _vaccinatedFemalesPercent, 2);

            vaccinationPercent.Add(_vaccinatedMalesPercent);
            vaccinationPercent.Add(_vaccinatedFemalesPercent);
            vaccinationPercent.Add(_notVaccinatedMalesPercent);
            vaccinationPercent.Add(_notVaccinatedFemalesPercent);

            return vaccinationPercent;
        }

        //TODO Unify GenerateChart to a single method.
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
                data: [_vaccinatedFemalesPercent, _notVaccinatedFemalesPercent],
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
                data: [_vaccinatedMalesPercent, _notVaccinatedMalesPercent],
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
                data: [_vaccinatedMalesPercent, _vaccinatedFemalesPercent],
                bgcolor: ["rgb(0, 76, 153)", "rgb(255, 102, 178)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }
    }
}
