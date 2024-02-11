using DSUGrupp1.Models.DTO;
using System.Reflection.Emit;

namespace DSUGrupp1.Models.ViewModels
{
    public class DisplayAgeStatisticsViewModel
    {
        public static List<VaccinationDataFromSpecificDeSoDto> VaccinationDataFromSpecificDeso { get; set; }


        public List<AgeGroupDoseCounts> AgeGroupDoseCounts { get; set; }

        public List<string> DoseColors = new List<string> { "rgb(255, 99, 132)", "rgb(54, 162, 235)", "rgb(255, 206, 86)" };
        public List<string> Labels = new List<string> { "16-30", "31-45", "46-60", "61+" };


        public DisplayAgeStatisticsViewModel(List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSos)
        {
            VaccinationDataFromSpecificDeso = vaccinationDataFromSpecificDeSos;
            CalculateAgeAndDoseCounts();
        }



        public ChartViewModel GenerateChart()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateAgeChart("bar", Labels, AgeGroupDoseCounts, DoseColors, 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        public ChartViewModel GenerateChartForSelectedAgeRange(int leftValue, int rightValue)
        {
            List<AgeGroupDoseCounts> ageGroupDoseCountsForRange = CalculateAgeAndDoseCountsForAgeRange(leftValue, rightValue);
            List<string> labelsForRange = ageGroupDoseCountsForRange.Select(a => a.AgeGroup).ToList();

            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateAgeChart("bar", labelsForRange, ageGroupDoseCountsForRange, DoseColors, 5);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }


        public List<AgeGroupDoseCounts> CalculateAgeAndDoseCounts()
        {
            AgeGroupDoseCounts = new List<AgeGroupDoseCounts>();

            foreach (var data in VaccinationDataFromSpecificDeso)
            {
                foreach (var patient in data.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        int ageAtVaccination = DateTime.Parse(vaccination.DateOfVaccination).Year - int.Parse(patient.YearOfBirth);
                        string ageGroup = DetermineAgeGroup(ageAtVaccination);

                        AgeGroupDoseCounts ageGroupDoseCounts = AgeGroupDoseCounts.FirstOrDefault(a => a.AgeGroup == ageGroup);
                        if (ageGroupDoseCounts == null)
                        {
                            ageGroupDoseCounts = new AgeGroupDoseCounts { AgeGroup = ageGroup };
                            AgeGroupDoseCounts.Add(ageGroupDoseCounts);
                        }

                        if (vaccination.DoseNumber == 1)
                        {
                            ageGroupDoseCounts.FirstDoseCount++;
                        }
                        else if (vaccination.DoseNumber == 2)
                        {
                            ageGroupDoseCounts.SecondDoseCount++;
                        }
                        else if (vaccination.DoseNumber == 3)
                        {
                            ageGroupDoseCounts.BoosterDoseCount++;
                        }
                    }
                }
            }

            AgeGroupDoseCounts = AgeGroupDoseCounts
        .OrderBy(a => Labels.IndexOf(a.AgeGroup))
        .ToList();

            return AgeGroupDoseCounts;


        }


        public List<AgeGroupDoseCounts> CalculateAgeAndDoseCountsForAgeRange(int minAge, int maxAge)
        {
            List<AgeGroupDoseCounts> ageGroupDoseCountsForRange = new List<AgeGroupDoseCounts>();

            foreach (var data in VaccinationDataFromSpecificDeso)
            {
                foreach (var patient in data.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        int ageAtVaccination = DateTime.Parse(vaccination.DateOfVaccination).Year - int.Parse(patient.YearOfBirth);

                        if (ageAtVaccination >= minAge && ageAtVaccination <= maxAge)
                        {
                            string ageGroup = DetermineAgeGroupForRange(ageAtVaccination, minAge, maxAge);

                            AgeGroupDoseCounts ageGroupDoseCounts = ageGroupDoseCountsForRange.FirstOrDefault(a => a.AgeGroup == ageGroup);
                            if (ageGroupDoseCounts == null)
                            {
                                ageGroupDoseCounts = new AgeGroupDoseCounts { AgeGroup = ageGroup };
                                ageGroupDoseCountsForRange.Add(ageGroupDoseCounts);
                            }

                            if (vaccination.DoseNumber == 1)
                            {
                                ageGroupDoseCounts.FirstDoseCount++;
                            }
                            else if (vaccination.DoseNumber == 2)
                            {
                                ageGroupDoseCounts.SecondDoseCount++;
                            }
                            else if (vaccination.DoseNumber == 3)
                            {
                                ageGroupDoseCounts.BoosterDoseCount++;
                            }
                        }
                    }
                }
            }

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

