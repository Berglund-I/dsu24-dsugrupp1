using DSUGrupp1.Models.DTO;
using DSUGrupp1.Models;
using DSUGrupp1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;


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

            VaccinationViewModel vaccinations = new VaccinationViewModel();
            ChartViewModel chart = await vaccinations.GenerateChart();

           

            //var apiResult = await _apiController.GetPopulationCount("2380","2022");
            //var apiResult = await _apiController.GetVaccinationsCount();

            //HomeViewModel model = new HomeViewModel();
            //model.Population = await _apiController.GetPopulationInSpecificDeSo("2380A0010", "2022");   
            //model.DataFromSpecificDeSo = await _apiController.GetVaccinationDataFromDeSo("2380A0010");

            HomeViewModel model = new HomeViewModel();
            
            model.Charts.Add(chart);

            
            var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");
            var apiResult2 = await _apiController.GetVaccinationsCount();
            var vaccineDataAllDeso = await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);

            
            DisplayGenderStatisticsViewModel genderStatistics = new DisplayGenderStatisticsViewModel(apiResult1, vaccineDataAllDeso);
            ChartViewModel chartGender = genderStatistics.GenerateChartFemales();
            model.Charts.Add(chartGender);

            //var ageStatistics = new DisplayAgeStatisticsViewModel(vaccineDataAllDeso);


            

            //ChartViewModel model = new ChartViewModel("3");

            return View(model);

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
