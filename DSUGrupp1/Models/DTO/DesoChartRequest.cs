using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class DesoChartRequest
    {
        [JsonProperty("selectedDeSo")]
        public string SelectedDeSo { get; set; }

        public DesoChartRequest()
        {
            
        }

    }
}
