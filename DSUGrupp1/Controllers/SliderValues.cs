using Newtonsoft.Json;

namespace DSUGrupp1.Controllers
{
    public class SliderValues
    {
       
            [JsonProperty("leftValue")]
            public int LeftValue { get; set; }
            [JsonProperty("rightValue")]
            public int RightValue { get; set; }


    }
}