using DSUGrupp1.Infastructure;
using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{

    public class DisplayGenderStatisticsViewModel
    {
        public int PopulationFemales { get; set; }
        public int VaccinatedFemales { get; set; }
        public int PopulationMales { get; set; }
        public int VaccinatedMales { get; set; }

        private double _vaccinatedFemalesPercent;
        private double _vaccinatedMalesPercent;
        private double _notVaccinatedFemalesPercent;
        private double _notVaccinatedMalesPercent;

        public DisplayGenderStatisticsViewModel() { }

        public DisplayGenderStatisticsViewModel(PopulationDto population, List<Patient> patients)
        {
            PopulationMales = int.Parse(population.Data[0].Values[0]);
            PopulationFemales = int.Parse(population.Data[1].Values[0]);
        
            VaccinatedMales = LinqQueryRepository.GetPatientsByGender(patients, "Male").Count;
            VaccinatedFemales = LinqQueryRepository.GetPatientsByGender(patients, "Female").Count;

            CountVaccinatedGenderPercent(PopulationMales, PopulationFemales, VaccinatedMales, VaccinatedFemales);
        }

        public List<double> CountVaccinatedGenderPercent(int populationMales, int populationFemales, int vaccinatedMales, int vaccinatedFemales)
        {
            if (populationFemales <= 0 || populationMales <= 0)
            {
                throw new Exception("Population cannot be zero");
            }

            List<double> vaccinationPercent = new List<double>();
            _vaccinatedMalesPercent = Math.Round((double)vaccinatedMales / populationMales * 100, 2);
            _vaccinatedFemalesPercent = Math.Round((double)vaccinatedFemales / populationFemales * 100, 2);
            _notVaccinatedMalesPercent = Math.Round(100 - _vaccinatedMalesPercent, 2);
            _notVaccinatedFemalesPercent = Math.Round(100 - _vaccinatedFemalesPercent, 2);

            vaccinationPercent.Add(_vaccinatedMalesPercent);
            vaccinationPercent.Add(_vaccinatedFemalesPercent);
            vaccinationPercent.Add(_notVaccinatedMalesPercent);
            vaccinationPercent.Add(_notVaccinatedFemalesPercent);

            return vaccinationPercent;
        }
        public ChartViewModel GenerateChartFemales()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % hos kvinnor",
                type: "pie",
                labels: ["Vaccinerade kvinnor i procent", "Ovaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad bland kvinnor",
                data: [_vaccinatedFemalesPercent, _notVaccinatedFemalesPercent],
                bgcolor: ["rgb(178, 102, 255)", "rgb(255, 153, 204)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// A method that generates a Chart for the vaccination percentage of men.
        /// </summary>
        /// <returns></returns>
        public ChartViewModel GenerateChartMales()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % hos män",
                type: "pie",
                labels: ["Vaccinerade män i procent", "Ovaccinerade män i procent"],
                DatasetLabel: "Vaccinationsgrad bland män",
                data: [_vaccinatedMalesPercent, _notVaccinatedMalesPercent],
                bgcolor: ["rgb(0, 204, 0)", "rgb(0, 102, 204)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }

        /// <summary>
        /// A method that generates a Chart for the vaccination percentage of women and men.
        /// </summary>
        /// <returns></returns>
        public ChartViewModel GenerateChartBothGenders()
        {
            ChartViewModel chart = new ChartViewModel();
            chart.Chart = chart.CreateChart(
                text: "Vaccinationsgrad i % mellan könen",
                type: "pie",
                labels: ["Vaccinerade män i procent", "Vaccinerade kvinnor i procent"],
                DatasetLabel: "Vaccinationsgrad mellan könen",
                data: [_vaccinatedMalesPercent, _vaccinatedFemalesPercent],
                bgcolor: ["rgb(0, 76, 153)", "rgb(255, 102, 178)"], 3);
            chart.JsonChart = chart.SerializeJson(chart.Chart);
            return chart;
        }
    }
    //public class DisplayGenderStatisticsViewModel
    //{
    //    public int PopulationFemales { get; set; }
    //    public int VaccinatedFemales { get; set; }
    //    public int PopulationMales { get; set; }
    //    public int VaccinatedMales { get; set; }


    //    public int AgeAtVaccinationdDate { get; set; }



    //    private List<VaccinationDataFromSpecificDeSoDto> _vaccinationDataFromSpecificDeSoDto = null;
    //    private double _vaccinatedFemalesPercent;
    //    private double _vaccinatedMalesPercent;
    //    private double _notVaccinatedFemalesPercent;
    //    private double _notVaccinatedMalesPercent;

    //    public DisplayGenderStatisticsViewModel()
    //    {

    //    }
    //    public DisplayGenderStatisticsViewModel(PopulationDto population, List<VaccinationDataFromSpecificDeSoDto> vaccinationDataFromSpecificDeSoDto)
    //    {
    //        int populationMales = int.Parse(population.Data[0].Values[0]);
    //        int populationFemales = int.Parse(population.Data[1].Values[0]);
    //        _vaccinationDataFromSpecificDeSoDto = vaccinationDataFromSpecificDeSoDto;

    //        List<List<int>> totalGender = new List<List<int>>();
    //        foreach(var list in vaccinationDataFromSpecificDeSoDto)
    //        {
    //            totalGender.Add(CountVaccinatedGender(list));
    //        }

    //        List<int> sortedGender = SortingListOfGender(totalGender);

    //        VaccinatedMales = sortedGender[0];
    //        VaccinatedFemales = sortedGender[1];

    //        CountVaccinatedGenderPercent(populationMales, populationFemales, sortedGender[0], sortedGender[1]);

    //    }

    //    /// <summary>
    //    /// A method that calculates the percentage of vaccinated and unvaccinated women and men.
    //    /// </summary>
    //    /// <exception cref="Exception"></exception>
    //    public List<double> CountVaccinatedGenderPercent(int populationMales, int populationFemales, int vaccinatedMales, int vaccinatedFemales) 
    //    {
    //        if (populationFemales <= 0|| populationMales <= 0)
    //        {
    //            throw new Exception("Antalet kan ej vara noll");
    //        }

    //        List<double> vaccinationPercent = new List<double>();    
    //        _vaccinatedMalesPercent = Math.Round((double)vaccinatedMales / populationMales * 100, 2);
    //        _vaccinatedFemalesPercent = Math.Round((double)vaccinatedFemales / populationFemales * 100, 2);          
    //        _notVaccinatedMalesPercent = Math.Round(100 - _vaccinatedMalesPercent, 2);
    //        _notVaccinatedFemalesPercent = Math.Round(100 - _vaccinatedFemalesPercent, 2);

    //        vaccinationPercent.Add(_vaccinatedMalesPercent);
    //        vaccinationPercent.Add(_vaccinatedFemalesPercent);
    //        vaccinationPercent.Add(_notVaccinatedMalesPercent);
    //        vaccinationPercent.Add(_notVaccinatedFemalesPercent);

    //        return vaccinationPercent;
    //    }


    /// <summary>
    /// A method that generates a Chart for the vaccination percentage of women.
    /// </summary>
    /// <returns></returns>



    //    /// <summary>
    //    /// A method that checks through the patients in different desos and sorts them into women and men.
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<int> CountVaccinatedGender(VaccinationDataFromSpecificDeSoDto patients)
    //    {
    //        List<int> genders = new List<int>();
    //        int male = 0;
    //        int female = 0;

    //        foreach (var patient in patients.Patients)
    //        {
    //            if (patient.Gender == "Male")
    //            {
    //                male++;
    //            }
    //            else if (patient.Gender == "Female")
    //            {
    //                female++;
    //            }
    //        }
    //        genders.Add(male);
    //        genders.Add(female);

    //        return genders;
    //    }

    //    public List<int> SortingListOfGender(List<List<int>> genders)
    //    {
    //        List<int> sorted = new List<int>();

    //        int male = 0;
    //        int females = 0;

    //        foreach (var gender in genders)
    //        {
    //            for (int i = 0; i < 2; i++)
    //            {
    //                if (i == 0)
    //                {
    //                    male += gender[i];
    //                }
    //                else
    //                {
    //                    females += gender[i];
    //                }

    //            }
    //        }
    //        sorted.Add(male);
    //        sorted.Add(females);

    //        return sorted;
    //    }

    //}
}
