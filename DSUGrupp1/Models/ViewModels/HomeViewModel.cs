using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(List<Patient> patients) 
        {
            Charts = new List<ChartViewModel>();
            DeSoDropDown = new PopulateDeSoDropDownViewModel(patients);
        }
        public List<ChartViewModel> Charts { get; set; }
        public PopulateDeSoDropDownViewModel DeSoDropDown { get; set; }

        //public VaccinationDataFromSpecificDeSoDto? DataFromSpecificDeSo { get; set; }

        //public PopulationDto? Population { get; set; }

    }
}
