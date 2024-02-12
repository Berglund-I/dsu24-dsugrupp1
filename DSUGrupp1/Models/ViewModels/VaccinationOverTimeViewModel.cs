using DSUGrupp1.Models.DTO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Globalization;
using System;
using System.Diagnostics;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationOverTimeViewModel
    {

        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto;

        public List<double> VaccinationsperWeek {  get; set; }

        public List<double>? DataPoints { get; set; }

        public VaccinationOverTimeViewModel() 
        { 

        }

        public VaccinationOverTimeViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDtos)
        {
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDtos;

            
        }
        public ChartViewModel GenerateLineChart()
        {
            ChartViewModel chart = new ChartViewModel();

            List<DatasetsDto>? datasets = new List<DatasetsDto>();

            var weekLabel = Enumerable.Range(1, 52).Select(i => i.ToString()).ToList();

            List<string> colors = new List<string>()
            {
                "#70e000",
                "#006466",
                "#8900f2",
                "#f20089",

            };

            for (int year=2020;year <= 2023;year++)
            {              
                //1500 ms for all loops combined
                VaccinationsperWeek = CountVaccinationsWeekByWeek(year);

                string color = colors[year - 2020];
                DatasetsDto dataset = chart.GenerateDataSet(
                    DatasetLabel: $"{year}",
                    data: VaccinationsperWeek,
                    bgcolor: new List<string> {color},
                    bColor: color, 3
                    );
                
                datasets.Add(dataset);
            }
            chart.Chart = chart.CreateMultiSetChart(          
                    text: "Antal vaccinationer per vecka",
                    type: "line",
                    labels: weekLabel,
                    datasets:datasets);
            
            chart.JsonChart = chart.SerializeJson(chart.Chart);

        
            return chart;
        }

        public List<double> CountVaccinationsWeekByWeek(int year)
        {
            var ci = new CultureInfo("sv-SE");
            var cal = ci.Calendar;
            var lastDayOfYear = new DateTime(year, 12, 31);
            int weeksInYear = cal.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Preprocess to get vaccination dates for the given year
            var vaccinationsInYear = StoreVaccinationsByYear(year.ToString());
            var vaccinationsPerWeek = new List<double>();

            var parsedVaccinationDates = vaccinationsInYear
            .Select(dateString => DateTime.TryParse(dateString, out DateTime date) ? (DateTime?)date : null)
            .Select(date => date.Value)
            .ToList();

            List<double> vaccinationCounts = new List<double>();

            for (int week = 1; week <= weeksInYear; week++)
            {
                var weekStart = FirstDateOfWeek(year, week, ci);
                var weekEnd = weekStart.AddDays(6);

                var vaccinationCountThisWeek = parsedVaccinationDates.Count(date => date >= weekStart && date <= weekEnd);
                vaccinationCounts.Add(vaccinationCountThisWeek);
            }
            //for (int week = 1; week <= weeksInYear; week++)
            //{
            //    var weekStart = FirstDateOfWeek(year, week, ci);
            //    var weekEnd = weekStart.AddDays(6); // Assuming the week starts on Monday


            //    // Count vaccinations for the week
            //    //total execution time for all loops: 1340 ms
            //    var vaccinationCountThisWeek = vaccinationsInYear.Count(dateString =>
            //    {
            //        if (DateTime.TryParse(dateString, out DateTime vaccinationDate))
            //        {
            //            return vaccinationDate >= weekStart && vaccinationDate <= weekEnd;
            //        }
            //        return false;
            //    });

            //    vaccinationsPerWeek.Add(vaccinationCountThisWeek);
            //}
            return vaccinationCounts;
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
