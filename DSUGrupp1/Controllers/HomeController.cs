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


namespace DSUGrupp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiController _apiController;
       

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _apiController = new ApiController();     
        }

        public async Task<ActionResult> Index()
        {

            if(HomeModelStorage.ViewModel == null)
            {



                //0 ms
                VaccinationViewModel vaccinations = new VaccinationViewModel();

                //470 ms
                ChartViewModel municipalityChart = await vaccinations.GenerateChart();

                //78 ms
                var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");

                // 80 ms
                var apiResult2 = await _apiController.GetVaccinationsCount();
                var stopwatch = Stopwatch.StartNew();
                //2000 ms avg -> 1600 ms avg
                var vaccineDataAllDeso =  await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);
                stopwatch.Stop();
                System.Diagnostics.Debug.WriteLine($"Duration: {stopwatch.ElapsedMilliseconds} milliseconds");
                //31 ms
                HomeViewModel model = new HomeViewModel();
                //41 ms
                DisplayAgeStatisticsViewModel ageStatistics = new DisplayAgeStatisticsViewModel(vaccineDataAllDeso);
                //0 ms
                VaccinationOverTimeViewModel vaccinationOverTimeStatistics = new VaccinationOverTimeViewModel(apiResult1, vaccineDataAllDeso);
                //1615 ms -> 221ms
                ChartViewModel chartLineOverTime = vaccinationOverTimeStatistics.GenerateLineChart();


                //34 ms
                ChartViewModel ageChart = await ageStatistics.GenerateChart();
                
                //22 ms for block
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

                return View(model);
            }
            return View(HomeModelStorage.ViewModel);
        }

        public ActionResult Detail()
        {

            return View(HomeModelStorage.ViewModel);
        }

        [HttpPost]
        public IActionResult GetChartFromDeSoCode([FromBody] TestFetch data)
        {
            var response = new DeSoChartViewModel(data.SelectedDeSo);
            
            return Ok(response);          
        }
 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
