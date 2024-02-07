using DSUGrupp1.Models.DTO;
using System.Security.Cryptography.X509Certificates;

namespace DSUGrupp1.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel() 
        {
            Charts = new List<ChartViewModel>();
            DeSoDropDown = new PopulateDeSoDropDownViewModel();
            FilterDropDown = new PopulateFiltersViewModel();
        }
        public List<ChartViewModel> Charts { get; set; }
        public PopulateDeSoDropDownViewModel DeSoDropDown { get; set; }
        public bool GenderMale { get; set; }
        public bool GenderFemale { get; set; }
        public PopulateFiltersViewModel FilterDropDown { get; set; }

    }
}
