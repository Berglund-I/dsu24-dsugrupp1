using DSUGrupp1.Controllers;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationViewModel
    {
        private readonly ApiController _apiController;

        public VaccinationViewModel()
        {
            _apiController = new ApiController();
        }

        public async Task<ChartViewModel> GenerateChart()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart("bar", ["En dos", "Två doser", "Tre doser eller fler"], "Vaccinationsgrad i Östersunds kommun i %", await GetVaccinationValues(), ["rgb(119, 0, 255)", "rgb(119, 0, 255)", "rgb(119, 0, 255)"], 10);
            chart.JsonChart = JsonConvert.SerializeObject(chart.Chart).ToLower();
            return chart;
        }

        /// <summary>
        /// Fetches population and vaccinations from API, adds all the vaccinated people together in a variable and returns the total vaccination percentage.
        /// </summary>
        /// <returns></returns>
        public async Task<List<double>> GetVaccinationValues()
        {
            var populationData = await _apiController.GetPopulationCount("2380", "2022");
            var vaccineData = await _apiController.GetVaccinationsCount();

            int totalPopulation = int.Parse(populationData.Data[0].Values[0]);
            int oneDose = 0, secondDose = 0, thirdDose = 0;

            foreach (var deSo in vaccineData.Data)
            {
                oneDose += deSo.Data[0].Count;
                secondDose += deSo.Data[1].Count;
                thirdDose += deSo.Data[2].Count;
            }

            double vaccinatedPercentageDoseOne = CalculateVaccinationPercentage(totalPopulation, oneDose);
            double vaccinatedPercentageDoseTwo = CalculateVaccinationPercentage(totalPopulation, secondDose);
            double vaccinatedPercentageDoseThree = CalculateVaccinationPercentage(totalPopulation, thirdDose);

            List<double> percentageValues = [vaccinatedPercentageDoseOne, vaccinatedPercentageDoseTwo, vaccinatedPercentageDoseThree];

            return percentageValues;
        }

        /// <summary>
        /// Calculates the percentage of vaccinated people in a specified DeSo
        /// </summary>
        /// <param name="totalPopulation"></param>
        /// <param name="vaccinatedPeople"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double CalculateVaccinationPercentage(int totalPopulation, int vaccinatedPeople)
        {

            //För att inte dela med noll
            if (totalPopulation <= 0)
            {
                throw new Exception("Antalet invånare kan ej vara noll");
            }

            if (vaccinatedPeople < 0)
            {
                throw new Exception("Antalet vaccinerade kan ej vara noll");
            }

            double percentage = (double)vaccinatedPeople / totalPopulation * 100;
            return percentage;
        }
    }
}
