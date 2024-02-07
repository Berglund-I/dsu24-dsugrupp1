using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class BatchDto
    {
        [JsonProperty("batch-number")]
        public string BatchNumber { get; set; }
        [JsonProperty("vaccine-name")]
        public string VaccineName { get; set; }
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty("total-uses")]
        public int TotalUses { get; set; }
    }
}
