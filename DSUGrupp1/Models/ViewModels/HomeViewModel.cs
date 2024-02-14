
namespace DSUGrupp1.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(List<Patient> patients) 
        {
            Charts = new List<ChartViewModel>();
            DeSoDropDown = new PopulateDeSoDropDownViewModel(patients);
            FilterDropDown = new PopulateFiltersViewModel(patients);
        }
        public List<ChartViewModel> Charts { get; set; }
        public PopulateDeSoDropDownViewModel DeSoDropDown { get; set; }
        public bool GenderMale { get; set; }
        public bool GenderFemale { get; set; }
        public PopulateFiltersViewModel FilterDropDown { get; set; }
    }
}
