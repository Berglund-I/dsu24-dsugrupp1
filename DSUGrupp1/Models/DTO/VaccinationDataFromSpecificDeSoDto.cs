using Newtonsoft.Json;
using System.Collections;

namespace DSUGrupp1.Models.DTO
{
    public class VaccinationDataFromSpecificDeSoDto
    {
        public TotalVaccinationsDto Meta { get; set; }

        [JsonProperty("patients")]
        public List<PatientInformationDto> Patients { get; set; }
    }
}
