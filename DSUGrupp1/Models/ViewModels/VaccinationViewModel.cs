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

        /// <summary>
        /// Method that generates a chart that uses GetVaccinationValues & CalculateVaccinationPercentage for percentage values. Hard coded text for labels in the new chart.
        /// </summary>
        /// <returns></returns>
        public async Task<ChartViewModel> GenerateChart()
        {
            ChartViewModel chart = new ChartViewModel();


            int population = await GetMunicipalityPopulation();
            chart.Chart = chart.CreateChart("", "bar", ["En dos", "Två doser", "Tre doser eller fler"], $"% av totalt {population} invånare", await GetVaccinationValues(), ["rgb(29, 52, 97)", "rgb(55, 105, 150)", "rgb(130, 156, 188)"], 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// Fetches vaccinations from API, calls for municipality population, adds all the vaccinated people together in a list and returns the total vaccination percentage.
        /// </summary>
        /// <returns></returns>
        public async Task<List<double>> GetVaccinationValues()
        {
            var vaccineData = await _apiController.GetVaccinationsCount();

            int totalPopulation = await GetMunicipalityPopulation();        
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
        /// Method that fetches population statistics, aswell as merges genders together to get a full population count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetMunicipalityPopulation()
        {
            var populationData = await _apiController.GetPopulationCount("2380", "2022");
            int totalPopulation = int.Parse(populationData.Data[0].Values[0]) + int.Parse(populationData.Data[1].Values[0]);
            return totalPopulation;
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
