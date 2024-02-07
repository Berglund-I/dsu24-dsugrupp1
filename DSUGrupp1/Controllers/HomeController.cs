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
                
                VaccinationViewModel vaccinations = new VaccinationViewModel();

                ChartViewModel municipalityChart = await vaccinations.GenerateChart();

                var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");
                var apiResult2 = await _apiController.GetVaccinationsCount();
                var vaccineDataAllDeso =  await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);               

                HomeViewModel model = new HomeViewModel();
                
                DisplayAgeStatisticsViewModel ageStatistics = new DisplayAgeStatisticsViewModel(vaccineDataAllDeso);               
                VaccinationOverTimeViewModel vaccinationOverTimeStatistics = new VaccinationOverTimeViewModel(apiResult1, vaccineDataAllDeso);

                ChartViewModel chartLineOverTime = vaccinationOverTimeStatistics.GenerateLineChart();
                ChartViewModel ageChart = await ageStatistics.GenerateChart();
                
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
