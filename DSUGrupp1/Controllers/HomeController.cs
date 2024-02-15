using DSUGrupp1.Models.DTO;
using DSUGrupp1.Models;
using DSUGrupp1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DSUGrupp1.Infastructure;
using Microsoft.Extensions.Caching.Memory;



namespace DSUGrupp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiController _apiController;
        private readonly ListOfPatients _patientList;
        private readonly ListOfPopulation _listOfResidents;
        private readonly ILogger<HomeController> _logger;



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
                


                DoseTypeViewModel batches = new DoseTypeViewModel();
                
                var batchTest = await batches.GetBatches();

                var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");
                var apiResult2 = await _apiController.GetVaccinationsCount();

                var vaccineDataAllDeso = await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);
                


                await GetPatient(vaccineDataAllDeso, batchTest);
                await GetResident(apiResult2);
                
                ChartViewModel municipalityChart = await vaccinations.GenerateChart(Patients);



                DisplayAgeStatisticsViewModel ageStatistics = new DisplayAgeStatisticsViewModel(Patients);


                VaccinationOverTimeViewModel vaccinationOverTimeStatistics = new VaccinationOverTimeViewModel(Patients);

                ChartViewModel chartLineOverTime = vaccinationOverTimeStatistics.GenerateLineChart();

                ChartViewModel ageChart = ageStatistics.GenerateAgeChartForVaccinated();
                
                HomeModelStorage.AgeStatistics = ageStatistics;

                HomeViewModel model = new HomeViewModel(Patients);

                DisplayGenderStatisticsViewModel genderStatistics = new DisplayGenderStatisticsViewModel(apiResult1, Patients);

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
            var response = new StatisticsForMapViewModel(ListOfPopulation.ListOfResidents, ListOfPatients.PatientList);
            return View(response);
        }

        [HttpPost]
        public IActionResult GetChartFromDeSoCode([FromBody] DesoChartRequest data)
        {
            if(!string.IsNullOrEmpty(data.SelectedDeSo))
            {
                var response = new DeSoChartViewModel(data.SelectedDeSo, ListOfPatients.PatientList, ListOfPopulation.ListOfResidents);
                return Ok(response);
            }
            return Ok();
        }

        [HttpPost]

        public IActionResult GetChartFromFilteredOptions([FromBody] FilterDto data)
        {
            var response = LinqQueryRepository.GetSortedPatients(data, ListOfPatients.PatientList);
            var patients = response.Count();
            var chart = new ChartViewModel();

            if (patients <= 5){
                var notEnoughPatients = chart.CreateChart("INTE TILLRÄCKLIGT MED PATIENTER", "bar", ["Error"], "Ingen data tillgänglig", [0], ["#ffffff"], 0);
                return Ok(notEnoughPatients);
            }

            var newChart = chart.CreateChart("Antal patienter utifrån filtrering", "bar", ["Antal patienter"], "Patienter", [patients], ["#0000FF"], 5);
            return Ok(newChart);
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

        
        public async Task GetPatient(List<VaccinationDataFromSpecificDeSoDto> vaccinationData, DoseTypeDto doseData)
        {     

            var response = await _apiController.GetDeSoNames();   
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

        public async Task GetResident(VaccineCountDto vaccineCount)
        {
            List<string> deSos = vaccineCount.Data.Select(d => d.Deso).ToList();

            var responsMale = await _apiController.GetPopulationInSpecificDeSo(deSos, "2022", "1");
            var responsFemale = await _apiController.GetPopulationInSpecificDeSo(deSos, "2022", "2");

            List<DataItem> population = new List<DataItem>();
            population.AddRange(responsMale.Data);
            population.AddRange(responsFemale.Data);

            List<Resident> sortedPopulation = new List<Resident>();
            foreach (var p in population)
            {
                for (int i = 0; i < int.Parse(p.Values[0]); i++)
                {
                    Resident resident = new Resident(p);

                    sortedPopulation.Add(resident);
                }
            }
            ListOfPopulation.ListOfResidents = sortedPopulation;
        }
    }

    
}
