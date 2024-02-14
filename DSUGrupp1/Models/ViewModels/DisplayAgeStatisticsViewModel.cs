using DSUGrupp1.Models.DTO;
using System.Reflection.Emit;

namespace DSUGrupp1.Models.ViewModels
{
    public class DisplayAgeStatisticsViewModel
    {
        public List<VaccinationDataFromSpecificDeSoDto> VaccinationDataFromSpecificDeso { get; set; }

        public List<Patient> Patients { get; set; }

        public List<AgeGroupDoseCounts> AgeGroupDoseCounts { get; set; }

        public List<string> DoseColors = new List<string> { "rgb(255, 99, 132)", "rgb(54, 162, 235)", "rgb(255, 206, 86)" };
        public List<string> Labels = new List<string> { "16-30", "31-45", "46-60", "61+" };

        public DisplayAgeStatisticsViewModel(List<Patient> patients)
        {
            Patients = patients;
            AgeGroupDoseCounts = new List<AgeGroupDoseCounts>();
            CalculateAgeAndDoseCounts();
        }
        public ChartViewModel GenerateAgeChartForVaccinated()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateAgeChart("bar", Labels, AgeGroupDoseCounts, DoseColors, 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }
        public ChartViewModel GenerateChartForSelectedAgeRange(int leftValue, int rightValue)
        {
            List<AgeGroupDoseCounts> ageGroupDoseCountsForRange = CalculateAgeAndDoseCountsForSelectedAgeRange(leftValue, rightValue);
            List<string> labelsForRange = ageGroupDoseCountsForRange.Select(a => a.AgeGroup).ToList();

            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateAgeChart("bar", labelsForRange, ageGroupDoseCountsForRange, DoseColors, 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }
        public void CalculateAgeAndDoseCounts()
        {
            var currentYear = DateTime.Now.Year;
            AgeGroupDoseCounts = Patients
                .SelectMany(patient => patient.Vaccinations.Select(vaccination => new
                {
                    AgeAtVaccination = currentYear - patient.YearOfBirth,
                    vaccination.DoseNumber
                }))
                .GroupBy(x => DetermineAgeGroup(x.AgeAtVaccination))
                .Select(g => new AgeGroupDoseCounts
                {
                    AgeGroup = g.Key,
                    FirstDoseCount = g.Count(v => v.DoseNumber == 1),
                    SecondDoseCount = g.Count(v => v.DoseNumber == 2),
                    BoosterDoseCount = g.Count(v => v.DoseNumber == 3) 
                })
                .OrderBy(a => Labels.IndexOf(a.AgeGroup))
                .ToList();
        }
        public List<AgeGroupDoseCounts> CalculateAgeAndDoseCountsForSelectedAgeRange(int minAge, int maxAge)
        {
            var currentYear = DateTime.Now.Year;
            var ageGroupDoseCountsForRange = Patients
                .Where(patient =>
                {
                    var age = currentYear - patient.YearOfBirth;
                    return age >= minAge && age <= maxAge;
                })
                .SelectMany(patient => patient.Vaccinations.Select(vaccination => new
                {
                    Age = currentYear - patient.YearOfBirth,
                    vaccination.DoseNumber
                }))
                .GroupBy(x => DetermineAgeGroupForRange(x.Age,minAge,maxAge))
                .Select(g => new AgeGroupDoseCounts
                {
                    AgeGroup = g.Key,
                    FirstDoseCount = g.Count(v => v.DoseNumber == 1),
                    SecondDoseCount = g.Count(v => v.DoseNumber == 2),
                    BoosterDoseCount = g.Count(v => v.DoseNumber == 3)
                })
                .ToList();
            return ageGroupDoseCountsForRange;
        }
        
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

        private string DetermineAgeGroupForRange(int age, int minAge, int maxAge)
        {
            if (age >= minAge && age <= maxAge)
            {
                return $"{minAge}-{maxAge}";
            }
            else
            {
                return "Ålder existerar ej";
            }
        }
    }
}

