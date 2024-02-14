using DSUGrupp1.Models.DTO;
using System.Globalization;
using DSUGrupp1.Infastructure;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationOverTimeViewModel
    {
        private List<Patient> _patients;

        //double neccesary?
        public List<double> VaccinationsperWeek { get; set; }
        
        //not needed?
        //public List<double>? DataPoints { get; set; }

        public VaccinationOverTimeViewModel() { }

        public VaccinationOverTimeViewModel(List<Patient> patients)
        {
            _patients = patients;
        }

        public ChartViewModel GenerateLineChart()
        {
            ChartViewModel chart = new ChartViewModel();
            List<DatasetsDto>? datasets = new List<DatasetsDto>();

            var weekLabel = Enumerable.Range(1, 53).Select(i => i.ToString()).ToList();
            List<string> colors = new List<string>() { "#70e000", "#006466", "#8900f2", "#f20089", };

            for (int year = 2020; year <= 2023; year++)
            {                 
                VaccinationsperWeek = CountVaccinationsWeekByWeek(year,_patients);

                string color = colors[year - 2020];
                DatasetsDto dataset = chart.GenerateDataSet(
                    DatasetLabel: $"{year}",
                    data: VaccinationsperWeek,
                    bgcolor: new List<string> { color },
                    bColor: color,
                    bWidth: 3
                );

                datasets.Add(dataset);
            }

            chart.Chart = chart.CreateMultiSetChart(
                text: "Antal vaccinationer per vecka",
                type: "line",
                labels: weekLabel,
                datasets: datasets);

            chart.JsonChart = chart.SerializeJson(chart.Chart);

            return chart;
        }

        /// <summary>
        /// Gets the date of the monday in a given week number in a given year, according to ISO 8601 standards.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
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
        /// <summary>
        /// Counts the vaccinations week by week for a given year following ISO 8601 standards, returns it as a list of numbers
        /// </summary>
        /// <param name="year"></param>
        /// <param name="patients"></param>
        /// <returns></returns>
        public static List<double> CountVaccinationsWeekByWeek(int year,List<Patient> patients)
        {
            var ci = new CultureInfo("sv-SE");
            var cal = ci.Calendar;
            var lastDayOfYear = new DateTime(year, 12, 31);
            int weeksInYear = cal.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var vaccinationsPerWeek = new List<double>();

            for (int week = 1; week <= weeksInYear; week++)
            {
                var weekStart = FirstDateOfWeek(year, week, ci);
                var weekEnd = weekStart.AddDays(6);

                vaccinationsPerWeek.Add(LinqQueryRepository.GetPatientsByDates(patients, weekStart, weekEnd).Count());

            }

            return vaccinationsPerWeek;
        }
      
    }
}
