using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSUGrupp1.Models.ViewModels
{
    public class PopulateDeSoDropDownViewModel
    {
        private readonly ApiController _apiController;

        public int SelectedDeso { get; set; }
        public List<SelectListItem> DeSos { get; set; }

        public PopulateDeSoDropDownViewModel()
        {
            _apiController = new ApiController();
            var sortedDeSoData = SortDeSoInformation();
            DeSos = sortedDeSoData.Result;

        }
        /// <summary>
        /// Gets area data from api and sorts it to a list 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> SortDeSoInformation()
        {
            var response = await _apiController.GetDeSoNames();
            List<SelectListItem> sortedDeSoData = new List<SelectListItem>();

            for (int i = 0; i < response.Areas.Count(); i++)
            {
                SelectListItem DeSoData = new SelectListItem { Value = response.Areas[i].Deso, Text = response.Areas[i].DesoName };
                sortedDeSoData.Add(DeSoData);
            }

            return sortedDeSoData;
        }
         
    }
}
