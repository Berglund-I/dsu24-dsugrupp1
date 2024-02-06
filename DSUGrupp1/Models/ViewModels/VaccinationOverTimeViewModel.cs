using DSUGrupp1.Models.DTO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Globalization;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationOverTimeViewModel
    {
        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto;

        public List<double> VaccinationsperWeek {  get; set; }

        public List<double> DataPoints { get; set; }

        public VaccinationOverTimeViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDtos)
        {
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDtos;

            VaccinationsperWeek = CountVaccinationsWeekByWeek(2021);
            
        }
        public ChartViewModel GenerateLineChart()
        {
            var weekLabels = Enumerable.Range(1, 52).Select(i => i.ToString()).ToList();
            var dataPoints = Enumerable.Range(1, VaccinationsperWeek.Count)
            .Zip(VaccinationsperWeek, (week, count) => new { x = week.ToString(), y = count })
            .ToList();
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad över alla veckor på året",
                type: "line",
                labels: weekLabels,
                DatasetLabel: "Vaccinationsgrad över veckorna under detta år",
                data: VaccinationsperWeek,
                bgcolor: ["rgb(178, 102, 255)", "rgb(255, 153, 204)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            
            return chart;
        }
        private List<double> CountVaccinationsWeekByWeek(int year)
        {
            var ci = new CultureInfo("sv-SE");
            var cal = ci.Calendar;
            var lastDayOfYear = new DateTime(year, 12, 31);
            int weeksInYear = cal.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Preprocess to get vaccination dates for the given year
            var vaccinationsInYear = StoreVaccinationsByYear(year.ToString());

            var vaccinationsPerWeek = new List<double>();

            for (int week = 1; week <= weeksInYear; week++)
            {
                var weekStart = FirstDateOfWeek(year, week, ci);
                var weekEnd = weekStart.AddDays(6); // Assuming the week starts on Monday

                // Count vaccinations for the week
                var vaccinationCountThisWeek = vaccinationsInYear.Count(dateString =>
                {
                    if (DateTime.TryParse(dateString, out DateTime vaccinationDate))
                    {
                        return vaccinationDate >= weekStart && vaccinationDate <= weekEnd;
                    }
                    return false;
                });

                vaccinationsPerWeek.Add(vaccinationCountThisWeek);
            }

            return vaccinationsPerWeek;
        }

        /// <summary>
        /// Determines the Date for the first day of a given week.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(int year,int weekOfYear,CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            var calendar = ci.Calendar;
            var firstWeek = calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            
            int weekOfYearMultiplier = weekOfYear - 1;
            
            if (firstWeek >= 52)
            {
                firstWeekDay = jan1.AddDays(daysOffset + 7);
            }
            

            return firstWeekDay.AddDays(weekOfYearMultiplier * 7);
        }
        
        private List<string> StoreVaccinationsByYear(string year)
        {
            List<string> vaccinationsInGivenYear = new List<string>();

            foreach (var list in _vaccinationDataFromSpecificDeSoDto)
            {
                foreach (var patient in list.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        if (DateTime.TryParse(vaccination.DateOfVaccination, out DateTime vaccinationDate))
                        {
                            if (vaccinationDate.Year.ToString() == year)
                            {
                                vaccinationsInGivenYear.Add(vaccination.DateOfVaccination);
                            }

                        }

                    }
                }
            }

            return vaccinationsInGivenYear;

        }
    }
}
