using Newtonsoft.Json;

namespace DSUGrupp1.Models
{
    public class Test
    {
        [JsonProperty("batchNumber")]
        public string BatchNumber { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
