﻿using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel() 
        {
            Charts = new List<ChartViewModel>(); 
        }
        public List<ChartViewModel> Charts { get; set; }

        public VaccinationDataFromSpecificDeSoDto? DataFromSpecificDeSo { get; set; }

        public PopulationDto? Population { get; set; }

    }
}
