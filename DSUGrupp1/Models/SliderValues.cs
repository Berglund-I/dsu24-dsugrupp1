using Newtonsoft.Json;

namespace DSUGrupp1.Models
{
    public class SliderValues
    {

        [JsonProperty("leftValue")]
        public int LeftValue { get; set; }
        [JsonProperty("rightValue")]
        public int RightValue { get; set; }


    }
}