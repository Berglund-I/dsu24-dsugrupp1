using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class DisplayAgeStatisticsViewModel
    {
        public List<VaccinationDataFromSpecificDeSoDto> VaccinationDataFromSpecificDeso { get; set; }
        

        public Dictionary<string, AgeGroupDoseCounts> AgeGroupDoseCounts { get; set; }

        public List<string> doseColors = new List<string> { "rgb(255, 99, 132)", "rgb(54, 162, 235)", "rgb(255, 206, 86)" };
        public List<string> labels = new List<string> { "16-30", "31-45", "46-60", "61+" };


        public DisplayAgeStatisticsViewModel(List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeso)
        {
            VaccinationDataFromSpecificDeso = vaccinationDataFromSpecificDeso;
            AgeGroupDoseCounts = new Dictionary<string, AgeGroupDoseCounts>();
            CalculateAgeAndDoseCounts();
        }

        public async Task<ChartViewModel> GenerateChart()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateAgeChart("bar", labels, await CalculateAgeAndDoseCounts(), doseColors, 10);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// Calulates the age of the patients when they got vaccinated and how many doses they got
        /// </summary>

        private async Task<Dictionary<string, AgeGroupDoseCounts>> CalculateAgeAndDoseCounts()
        {
            List<double> groupAgeAndDoseCount = new List<double>();
            foreach (var data in VaccinationDataFromSpecificDeso)
            {
                foreach (var patient in data.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        

                        int ageWhenVaccinated = DateTime.Parse(vaccination.DateOfVaccination).Year - int.Parse(patient.YearOfBirth);

                        
                        string ageGroup = DetermineAgeGroup(ageWhenVaccinated);
                        bool doesContainAgeGroup = AgeGroupDoseCounts.ContainsKey(ageGroup);


                        if (!doesContainAgeGroup)
                        { 
                            AgeGroupDoseCounts[ageGroup] = new AgeGroupDoseCounts();
                        }

                        if  (vaccination.DoseNumber == 1)
                        {
                            AgeGroupDoseCounts[ageGroup].FirstDoseCount++;
                        }
                        else if (vaccination.DoseNumber == 2)
                        {
                            AgeGroupDoseCounts[ageGroup].SecondDoseCount++;
                        }
                        else if (vaccination.DoseNumber == 3)
                        {
                            AgeGroupDoseCounts[ageGroup].BoosterDoseCount++;
                        }

                        
                        if ( vaccination.DoseNumber == 3)
                        {
                            break;
                        }
                    }
                }

                
            }
            
            

            return AgeGroupDoseCounts;
        }

        /// <summary>
        /// Groups the ages into age groups
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        private string DetermineAgeGroup(int age)
        {
            if (age < 31)
                return "16-30";
            else if (age < 46)
                return "31-45";
            else if (age < 61)
                return "46-60";
            
            else
                return "61+";
        }


    }
}
