using DSUGrupp1.Models.DTO;
using System.Globalization;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationOverTimeViewModel
    {
        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto;

        public List<int> VaccinationsperWeek {  get; set; }

        public VaccinationOverTimeViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDtos)
        {
            _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDtos;


        }

        public ChartViewModel GenerateLineChart()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad över alla veckor på året",
                type: "line",
                labels: ["Antal vaccinerade", "Vecka"],
                DatasetLabel: "Vaccinationsgrad över veckorna under detta år",
                data: [],
                bgcolor: ["rgb(178, 102, 255)", "rgb(255, 153, 204)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        public static DateTime FirstDateOfWeek(int year,int weekOfYear,CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            var calendar = ci.Calendar;
            var firstWeek = calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            
            weekOfYear -= 1;
            
            if (firstWeek >= 52)
            {
                firstWeekDay = jan1.AddDays(daysOffset + 7);
            }
            

            return firstWeekDay.AddDays(weekOfYear * 7);
        }







    }
}
