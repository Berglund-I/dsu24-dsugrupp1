using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DSUGrupp1.Models.ViewModels
{
    public class PopulateFiltersViewModel
    {
        private readonly ApiController _apiController;

        public string SelectedBatch { get; set; }
        public List<SelectListItem> Batches { get; set; }
        public List<SelectListItem> VaccineName { get; set; }
        public List<SelectListItem> VaccineCentral {  get; set; }

        public PopulateFiltersViewModel()
        {
            _apiController = new ApiController();
            var sortedBatchData = SortBatchNumber();
            var sortedVaccineName = SortVaccineName();
            Batches = sortedBatchData.Result;
            VaccineName = sortedVaccineName.Result;
        }

        public async Task<List<SelectListItem>> SortBatchNumber()
        {
            var response = await _apiController.GetDoseTypes();
            List<SelectListItem> sortedBatchData = new List<SelectListItem>();

            for (int i = 0; i < response.Batches.Count(); i++)
            {
                SelectListItem batchData = new SelectListItem { Value = response.Batches[i].BatchNumber, Text = response.Batches[i].BatchNumber};
                sortedBatchData.Add(batchData);
            }

            return sortedBatchData;
        }

        public async Task<List<SelectListItem>> SortVaccineName()
        {
            var response = await _apiController.GetDoseTypes();
            List<SelectListItem> sortedBatchData = new List<SelectListItem>();

            for (int i = 0; i < response.Batches.Count(); i++)
            {
                SelectListItem batchData = new SelectListItem { Value = response.Batches[i].VaccineName, Text = response.Batches[i].VaccineName };
                sortedBatchData.Add(batchData);
            }

            return sortedBatchData;
        }

        public async Task<List<SelectListItem>> SortVaccineCentral()
        {
            var response = await _apiController.GetVaccinationsCount();
            List<SelectListItem> sortedCentralData = new List<SelectListItem>();

            for (int i = 0; i < response.Data.Count(); i++)
            {
                //SelectListItem centralData = new SelectListItem { Value = response. }
            }

            return sortedCentralData;

        }
    }
            
}
