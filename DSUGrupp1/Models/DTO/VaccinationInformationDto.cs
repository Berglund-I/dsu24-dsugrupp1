using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    
        public class VaccinationInformationDto
        {
            [JsonProperty("date-of-vaccination")]
            public string DateOfVaccination { get; set; }
            [JsonProperty("dose-number")]
            public int DoseNumber { get; set; }
            [JsonProperty("batch-number")]
            public string BatchNumber { get; set; }
            [JsonProperty("place-of-injection")]
            public string PlaceOfInjection { get; set; }
            [JsonProperty("vaccination-site")]
            public VaccinationCentralDto? VaccinationCentral { get; set; }
            
        }

    
}
