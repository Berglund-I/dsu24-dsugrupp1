using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class TotalVaccinationsDto
    {
        [JsonProperty("total-records-patients")]
        public int totalrecordspatients { get; set; }

        [JsonProperty("total-vaccinations")]
        public int totalvaccinations { get; set; }

        [JsonProperty("deso-code")]
        public string desocode { get; set; }

        [JsonProperty("latest-change")]
        public DateTime latestchange { get; set; }
    }
}
