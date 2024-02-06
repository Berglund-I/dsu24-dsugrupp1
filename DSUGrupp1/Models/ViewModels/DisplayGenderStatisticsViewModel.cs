using DSUGrupp1.Controllers;
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

        public int VaccinatedFemalesSpecificDeso { get; set; }
        public int VaccinatedMalesSpecificDeso { get; set; }
        public int PopulationFemalesInDeso { get; set; }
        public int PopulationMalesInDeso { get; set; }


        public int AgeAtVaccinationdDate { get; set; }



        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto = null;
        private readonly ApiController _apiController = new ApiController();
        private double _vaccinatedFemalesPercent;
        private double _vaccinatedMalesPercent;
        private double _notVaccinatedFemalesPercent;
        private double _notVaccinatedMalesPercent;

        private double _vaccinatedFemalesPercentDeSo;
        private double _vaccinatedMalesPercentDeSo;
        private double _notVaccinatedFemalesPercentDeSo;
        private double _notVaccinatedMalesPercentDeSo;

        public DisplayGenderStatisticsViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDto)
        {
            PopulationMales = int.Parse(population.Data[0].Values[0]);
            PopulationFemales = int.Parse(population.Data[1].Values[0]);
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDto;
            CountVaccinatedGender();
            CountVaccinatedGenderPercent();
            

        }

        public DisplayGenderStatisticsViewModel(PopulationDto populationDeSo, string deSoCode, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDto)
        {
            PopulationMalesInDeso = int.Parse(populationDeSo.Data[0].Values[0]);
            PopulationFemalesInDeso = int.Parse(populationDeSo.Data[1].Values[0]);
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDto;
            CountVaccinatedGenderInDeso(deSoCode);
            CountVaccinatedGenderPercentInDeSo();
        }



        public void CountVaccinatedGenderInDeso(string desocode)
        {
            foreach (var deso in _vaccinationDataFromSpecificDeSoDto)
            {
                if (deso.Meta.DeSoCode == desocode)
                {
                    foreach (var patinet in deso.Patients)
                    {
                        if (patinet.Gender == "Male")
                        {
                            VaccinatedMalesSpecificDeso++;
                        }
                        if (patinet.Gender == "Female")
                        {
                            VaccinatedFemalesSpecificDeso++;
                        }
                    }

                }
            }
        }

        public void CountVaccinatedGenderPercentInDeSo()
        {
            if (PopulationFemalesInDeso <= 0 || PopulationMalesInDeso <= 0)
            {
                throw new Exception("Befolkningen får inte vara noll");
            }
            int totalPopulation = PopulationFemalesInDeso + PopulationMalesInDeso;
            _vaccinatedFemalesPercentDeSo = Math.Round((double)VaccinatedFemalesSpecificDeso / totalPopulation * 100, 2);
            _vaccinatedMalesPercentDeSo = Math.Round((double)VaccinatedMalesSpecificDeso / totalPopulation * 100, 2);
            _notVaccinatedFemalesPercentDeSo = Math.Round((double)(PopulationFemalesInDeso - VaccinatedFemalesSpecificDeso) / totalPopulation * 100, 2);
            _notVaccinatedMalesPercentDeSo = Math.Round((double)(PopulationMalesInDeso - VaccinatedMalesSpecificDeso) / totalPopulation * 100, 2);

        }

        public ChartViewModel GenerateChartFemalesDeSo()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % hos kvinnor och män i valt DeSo",
                type: "pie",
                labels: ["Vaccinerade kvinnor i procent", "Ovaccinerade kvinnor i procent", "Vaccinerade män i procent", "Ovaccinerade män i procent"],
                DatasetLabel: "Vaccinationsgrad bland kvinnor i specifikt DeSo",
                data: [_vaccinatedFemalesPercentDeSo, _notVaccinatedFemalesPercentDeSo, _vaccinatedMalesPercentDeSo, _notVaccinatedMalesPercentDeSo],
                bgcolor: ["rgb(178, 102, 255)", "rgb(255, 153, 204)", "rgb(0, 204, 0)", "rgb(0, 102, 204)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
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
            _vaccinatedFemalesPercent = Math.Round((double)VaccinatedFemales / PopulationFemales * 100, 2);
            _vaccinatedMalesPercent = Math.Round((double)VaccinatedMales / PopulationMales * 100, 2);
            _notVaccinatedFemalesPercent = Math.Round(100 - _vaccinatedFemalesPercent, 2);
            _notVaccinatedMalesPercent = Math.Round(100 - _vaccinatedMalesPercent, 2);

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

