using DSUGrupp1.Models;
using DSUGrupp1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


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
            //var apiResult = await _apiController.ScbApiCall("2380","2022");
            var apiResult = await _apiController.GetVaccinationsCount();

            HomeViewModel model = new HomeViewModel();
            model.Population = await _apiController.GetPopulationCount("2380A0010", "2022");
        
            model.DataFromSpecificDeSo = await _apiController.GetVaccinationDataFromDeSo("2380A0010");
            return View(model);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
