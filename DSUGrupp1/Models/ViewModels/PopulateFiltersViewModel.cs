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
            HashSet<string> uniqueNames = new HashSet<string>();

            foreach (var batch in response.Batches)
            {
                if (!uniqueNames.Contains(batch.VaccineName))
                {
                    SelectListItem batchData = new SelectListItem { Value = batch.VaccineName, Text = batch.VaccineName };
                    sortedBatchData.Add(batchData);
                    uniqueNames.Add(batch.VaccineName); 
                }
            }

            return sortedBatchData;           

        }

        //public async Task<List<SelectListItem>> SortVaccineCentral()
        //{
        //    var vaccineCentrals = await GetVaccinationSites();
        //    List<SelectListItem> sortedCentralData = new List<SelectListItem>();

        //    for (int i = 0; i < response.Data.Count(); i++)
        //    {
        //        //SelectListItem centralData = new SelectListItem { Value = response. }
        //    }

        //    return sortedCentralData;

        //}
        /// <summary>
        /// Gets all vaccination centrals from bulk vaccinationdata. Note there is designated apicall for this but sorting through the data already collected is faster.
        /// </summary>
        /// <param name="vaccinationData"></param>
        /// <returns></returns>
        public static List<VaccinationCentralDto> GetVaccinationSites(List<VaccinationDataFromSpecificDeSoDto> vaccinationData) 
        {
            List<VaccinationCentralDto> newList = new List<VaccinationCentralDto>();

            foreach (var list in vaccinationData)
            {
                foreach (var patient in list.Patients)
                {
                    foreach (var vaccination in patient.Vaccinations)
                    {
                        var vaccinationCentral = vaccination.VaccinationCentral;
                        
                        if (newList.Any(v => v.SiteId == vaccinationCentral.SiteId) == false)
                        {
                            newList.Add(vaccinationCentral);
                        }
                        if(newList.Count > 20)
                        {
                            return newList;
                        }
                    }                   
                }
            }
            return newList;
        }
    }
            
}
