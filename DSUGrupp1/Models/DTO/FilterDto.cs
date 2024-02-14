using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class FilterDto
    {
        [JsonProperty("deSoCode")]
        public string DeSoCode { get; set; }
        [JsonProperty("batchNumber")]
        public string BatchNumber { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("minAge")]
        public int MinAge { get; set; }
        [JsonProperty("maxAge")]
        public int MaxAge { get; set; }
        [JsonProperty("siteId")]
        public int SiteId { get; set; }
        [JsonProperty("numberOfDoses")]
        public int NumberOfDoses { get; set; }
        [JsonProperty("typeOfVaccine")]
        public string TypeOfVaccine { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
     
    }
}
