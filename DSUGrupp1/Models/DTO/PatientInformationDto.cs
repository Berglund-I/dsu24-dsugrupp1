using Newtonsoft.Json;


namespace DSUGrupp1.Models.DTO
{
    public class PatientInformationDto
    {
       

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("year-of-birth")]
        public string YearOfBirth { get; set; }

        [JsonProperty("vaccinations")]
        public List<VaccinationInformationDto> Vaccinations { get; set; }
       
    }
}