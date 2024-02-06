﻿using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class VaccinationOverTimeViewModel
    {
        private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto = null;

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


        private int CountVaccinationsBetweenDates(string startDateString, string endDateString)
        {
            DateTime startDate = DateTime.Parse(startDateString);
            DateTime endDate = DateTime.Parse(endDateString);

            int vaccinationThatWeek = 0;

            foreach (var list in _vaccinationDataFromSpecificDeSoDto)
            {
                foreach (var patient in list.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        DateTime dateOfVaccination = DateTime.Parse(vaccination.DateOfVaccination);

                        if (dateOfVaccination >= startDate && dateOfVaccination <= endDate)
                        {
                            vaccinationThatWeek++;
                        }
                    }
                }
            }

            return vaccinationThatWeek;
        }

      




    }
}
