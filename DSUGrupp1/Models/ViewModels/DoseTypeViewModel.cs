using DSUGrupp1.Controllers;
using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class DoseTypeViewModel
    {
        private readonly ApiController _apiController;

        public DoseTypeViewModel()
        {
            _apiController = new ApiController();
        }
        
        /// <summary>
        /// Only exists for future use. Currenty fetches data from ApiController.
        /// </summary>
        /// <returns></returns>
        public async Task<DoseTypeDto> GetBatches()
        {
            var getBatches = await _apiController.GetDoseTypes();

            return getBatches;
        }

    }
}
