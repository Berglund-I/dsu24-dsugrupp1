using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class VaccinationCentralDto
    {
        [JsonProperty("site-id")]
        public int SiteId { get; set; }
        [JsonProperty("site-name")]
        public string SiteName { get; set;}
    }
}
