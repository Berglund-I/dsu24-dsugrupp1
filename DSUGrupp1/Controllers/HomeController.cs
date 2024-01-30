using DSUGrupp1.Models.DTO;
using DSUGrupp1.Models;
using DSUGrupp1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;


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


        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<ActionResult> Index()
        {
            //var apiResult = await _apiController.GetPopulationCount("2380","2022");
            //var apiResult = await _apiController.GetVaccinationsCount();

            //HomeViewModel model = new HomeViewModel();
            //model.Population = await _apiController.GetPopulationInSpecificDeSo("2380A0010", "2022");   
            //model.DataFromSpecificDeSo = await _apiController.GetVaccinationDataFromDeSo("2380A0010");


            var apiResult1 = await _apiController.GetPopulationCount("2380", "2022");
            var apiResult2 = await _apiController.GetVaccinationsCount();
            var vaccineDataAllDeso = await _apiController.GetVaccinationDataFromAllDeSos(apiResult2);


            var genderStatistics = new DisplayGenderStatisticsViewModel(apiResult1, vaccineDataAllDeso);


            ChartViewModel model = new ChartViewModel("3");

            return View(model);

            var deSoNames = await _apiController.GetDeSoNames();
            var forDropdown = await _apiController.GetVaccinationDataFromDeSo("2380A0010");

            
            return View();


        }
        //Not in use yet
        public IActionResult PopulateDeSoDropDown()
        {
            var model = new PopulateDeSoDropDownViewModel();
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
