using DSUGrupp1.Controllers;
using DSUGrupp1.Infastructure;
using DSUGrupp1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


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
