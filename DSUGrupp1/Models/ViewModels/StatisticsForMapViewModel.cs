using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class StatisticsForMapViewModel
    {
        public Dictionary<string, double> VaccinationPercentDeso { get; set; } = new Dictionary<string, double>();

        public string JsonVaccinationPercent { get; set; }


        public StatisticsForMapViewModel(List<Resident> residents, List<Patient> patients)
        {
           
            SetValuesForVaccinationList(residents, patients);
            JsonVaccinationPercent = JsonConvert.SerializeObject(VaccinationPercentDeso);


        }


        /// <summary>
        /// Method that calculates the vaccination rate in percent for each deso.
        /// </summary>
        /// <param name="residents"></param>
        /// <param name="patients"></param>
        private void SetValuesForVaccinationList(List<Resident> residents, List<Patient> patients)
        {
            var desoList = LinqQueryRepository.GetDesoList(patients);
            foreach (var deso in desoList)
            {
                var sortedPatients = LinqQueryRepository.GetPatientsByDeSo(patients, deso);
                var sortedResidents = LinqQueryRepository.GetResidentsByDeSo(residents, deso);

                double vaccinatedPercent = (double)sortedPatients.Count() / sortedResidents.Count() * 100;
                VaccinationPercentDeso.Add(deso, vaccinatedPercent);

            }
        }


    }
}
