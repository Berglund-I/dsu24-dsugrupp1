using DSUGrupp1.Controllers;

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
        /// Fetches population and vaccinations from API, adds all the vaccinated people together in a variable and returns the total vaccination percentage.
        /// </summary>
        /// <returns></returns>
        public async Task<Object> GetVaccinationValues()
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

            double[] percentageValues = { vaccinatedPercentageDoseOne, vaccinatedPercentageDoseTwo, vaccinatedPercentageDoseThree };

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
