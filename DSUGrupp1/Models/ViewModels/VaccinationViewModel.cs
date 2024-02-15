using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;


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
        public async Task<ChartViewModel> GenerateChart(List<Patient> patients)
        {

            ChartViewModel chart = new ChartViewModel();
            int population = await GetMunicipalityPopulation();

            var vaccinationValues = GetVaccinationValues(patients,population);

            chart.Chart = chart.CreateChart("", "bar", ["En dos", "Två doser", "Tre doser eller fler"], $"% av totalt {population} invånare", vaccinationValues, ["rgb(29, 52, 97)", "rgb(55, 105, 150)", "rgb(130, 156, 188)"], 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// Fetches vaccinations from API, calls for municipality population, adds all the vaccinated people together in a list and returns the total vaccination percentage.
        /// </summary>
        /// <returns></returns>
        public List<double> GetVaccinationValues(List<Patient> patients, int population)
        {
            
            int oneDose = 0, secondDose = 0, thirdDose = 0;

            oneDose = LinqQueryRepository.GetPatientsByDoseNumber(patients, 1).Count();
            secondDose = LinqQueryRepository.GetPatientsByDoseNumber(patients, 2).Count();
            thirdDose = LinqQueryRepository.GetPatientsByDoseNumber(patients, 3).Count();

            double vaccinatedPercentageDoseOne = CalculateVaccinationPercentage(population, oneDose);
            double vaccinatedPercentageDoseTwo = CalculateVaccinationPercentage(population, secondDose);
            double vaccinatedPercentageDoseThree = CalculateVaccinationPercentage(population, thirdDose);

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
        /// Calculates the percentage of vaccinated people.
        /// </summary>
        /// <param name="totalPopulation"></param>
        /// <param name="vaccinatedPeople"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public double CalculateVaccinationPercentage(int totalPopulation, int vaccinatedPeople)
        {

            if (totalPopulation <= 0)
            {
                throw new Exception("Antalet invånare kan ej vara noll");
            }

            double percentage = ((double)vaccinatedPeople / totalPopulation) * 100;
            return percentage;
        }
    }
}
