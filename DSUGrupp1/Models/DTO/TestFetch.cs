using Newtonsoft.Json;

namespace DSUGrupp1.Models.DTO
{
    public class TestFetch
    {
        [JsonProperty("selectedDeSo")]
        public string SelectedDeSo { get; set; }

        public TestFetch()
        {
            
        }

    }
}
