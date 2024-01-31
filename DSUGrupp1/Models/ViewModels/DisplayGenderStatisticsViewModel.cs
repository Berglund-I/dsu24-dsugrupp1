using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class DisplayGenderStatisticsViewModel
    {
        public int PopulationFemales { get; set; }
        public int VaccinatedFemales { get; set; }
        public int PopulationMales { get; set; }
        public int VaccinatiedMales { get; set; }
        public int AgeAtVaccinationdDate { get; set; }


        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto = null;

        public DisplayGenderStatisticsViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDto)
        {
            PopulationMales = int.Parse(population.Data[0].Values[0]);
            PopulationFemales = int.Parse(population.Data[1].Values[0]);
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDto;
            CountVaccinatedGender();


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
                        VaccinatiedMales++;
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
