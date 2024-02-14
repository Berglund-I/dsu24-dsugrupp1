using DSUGrupp1.Infastructure;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace DSUGrupp1.Models.ViewModels
{
    public class PopulateDeSoDropDownViewModel
    {
        public string SelectedDeso { get; set; }
        public List<SelectListItem> DeSos { get; set; }

        public PopulateDeSoDropDownViewModel(List<Patient> patients)
        {
            DeSos = LinqQueryRepository.GetDesoInformation(patients);
        }        
    }
}
