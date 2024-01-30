using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<ChartViewModel> Charts { get; set; }

        public VaccinationDataFromSpecificDeSoDto DataFromSpecificDeSo { get; set; }

        public PopulationDto Population { get; set; }

    }
}
