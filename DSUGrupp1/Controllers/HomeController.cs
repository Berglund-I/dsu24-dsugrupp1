using DSUGrupp1.Models.DTO;
using DSUGrupp1.Models;
using DSUGrupp1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using DSUGrupp1.Infastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DSUGrupp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiController _apiController;
        private readonly ListOfPatients _patientList;
        private readonly ILogger<HomeController> _logger;

        //Shouldn't be possible to change when the initial values is set
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _apiController = new ApiController();
        }

        public async Task<ActionResult> Index()
        {

            if (HomeModelStorage.ViewModel == null)
            {

                VaccinationViewModel vaccinations = new VaccinationViewModel();

                ChartViewModel municipalityChart = await vaccinations.GenerateChart();


                // Code exists here for future use when working with batches/filters
                DoseTypeViewModel batches = new DoseTypeViewModel();
                var batchTest = await batches.GetBatches();

                //HomeViewModel model = new HomeViewModel();
                //model.Population = await _apiController.GetPopulationInSpecificDeSo("2380A0010", "2022");   
                //model.DataFromSpecificDeSo = await _apiController.GetVaccinationDataFromDeSo("2380A0010");

                var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");
                var apiResult2 = await _apiController.GetVaccinationsCount();
                var vaccineDataAllDeso = await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);

                GetPatient(vaccineDataAllDeso, batchTest);
                


                DisplayAgeStatisticsViewModel ageStatistics = new DisplayAgeStatisticsViewModel(vaccineDataAllDeso);
                VaccinationOverTimeViewModel vaccinationOverTimeStatistics = new VaccinationOverTimeViewModel(apiResult1, vaccineDataAllDeso);


                ChartViewModel chartLineOverTime = vaccinationOverTimeStatistics.GenerateLineChart();

                ChartViewModel ageChart = ageStatistics.GenerateAgeChartForVaccinated();
                HomeModelStorage.AgeStatistics = ageStatistics;

                HomeViewModel model = new HomeViewModel(ListOfPatients.PatientList);

                DisplayGenderStatisticsViewModel genderStatistics = new DisplayGenderStatisticsViewModel(apiResult1, vaccineDataAllDeso);
                ChartViewModel chartGenderFemales = genderStatistics.GenerateChartFemales();
                ChartViewModel chartGenderMales = genderStatistics.GenerateChartMales();
                ChartViewModel chartGenderBoth = genderStatistics.GenerateChartBothGenders();

                model.Charts.Add(municipalityChart);
                model.Charts.Add(chartLineOverTime);
                model.Charts.Add(ageChart);
                model.Charts.Add(chartGenderFemales);
                model.Charts.Add(chartGenderMales);
                model.Charts.Add(chartGenderBoth);

                HomeModelStorage.ViewModel = model;
                //var data = new FilterDto();
                //data.Gender = "Male";
                //data.BatchNumber = "AZ002";
                //data.SiteId = 4;
                //data.MinAge = 20;
                //data.MaxAge = 30;
                //var result = GetChartFromFilteredOptions(data);

                return View(model);
            }
            return View(HomeModelStorage.ViewModel);
        }
        public ActionResult Detail()
        {

            return View(HomeModelStorage.ViewModel);
        }

        public ActionResult Map()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetChartFromDeSoCode([FromBody] TestFetch data)
        {
            var response = new DeSoChartViewModel(data.SelectedDeSo);
            
            return Ok(response);          
        }

        [HttpPost]

        public IActionResult GetChartFromFilteredOptions([FromBody] FilterDto data)
        {
            
            var response1 = LinqQueryRepository.GetSortedPatients(data, ListOfPatients.PatientList);

            var response = data;

            return Ok();
        }


        public IActionResult CreateChartBasedOnSelectedMinAgeAndMaxAge([FromBody] SliderValues sliderValues)
        {
            var homeViewModel = HomeModelStorage.ViewModel;
            List<string> deso = new List<string>();

            var ageStatistics = HomeModelStorage.AgeStatistics;


            ChartViewModel chart = ageStatistics.GenerateChartForSelectedAgeRange(sliderValues.LeftValue, sliderValues.RightValue);

            return Ok(chart);
        }

        public  IActionResult ResetChartToShowTheWholePopulation()
        {
            var homeViewModel = HomeModelStorage.ViewModel;
            List<string> deso = new List<string>();

            var ageStatistics = HomeModelStorage.AgeStatistics;

            ChartViewModel chart = ageStatistics.GenerateAgeChartForVaccinated();

            return Ok(chart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public async void GetPatient(List<VaccinationDataFromSpecificDeSoDto> vaccinationData, DoseTypeDto doseData)
        {     

            var response = await _apiController.GetDeSoNames();   
            var time = Stopwatch.StartNew();
            Parallel.ForEach(vaccinationData, v =>
            {
                Parallel.ForEach(v.Patients, p =>
                {
                    Patient patient = new Patient(p, doseData, v.Meta.DeSoCode, response);
                    lock (Patients)
                    {
                        Patients.Add(patient);
                    }
                });

            });

            ListOfPatients.PatientList = Patients;
        }
    }

    
}
