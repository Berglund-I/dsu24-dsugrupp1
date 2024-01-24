using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class TotalVaccinationsDto
    {
        [JsonProperty("total-records-patients")]
        public int TotalRecordsPatients { get; set; }

        [JsonProperty("total-vaccinations")]
        public int TotalVaccinations { get; set; }

        [JsonProperty("deso-code")]
        public string DeSoCode { get; set; }

        [JsonProperty("latest-change")]
        public DateTime LatestChange { get; set; }
    }
}
