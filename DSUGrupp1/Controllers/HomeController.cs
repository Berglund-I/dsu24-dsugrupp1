using DSUGrupp1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DSUGrupp1.Controllers;
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

        public IActionResult Index()
        {
            
            return View();
        }

        public async Task<ActionResult> Privacy()
        {
            //var apiResult = await _apiController.ScbApiCall("2380","2022");
            var apiResult = await _apiController.GetVaccinationsCount();

			if (apiResult is OkObjectResult okResult)
            {

                var jsonData = okResult.Value.ToString();


                return View();
            }
            else
            {

                return View("Error");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
