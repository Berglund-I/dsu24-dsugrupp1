using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Chart
    {
        public Chart() 
        {
            Type = "bar";
            Data = new ChartDataDto();
            Options = new OptionsDto(Type);
        }
        public string Type { get; set; }
        public ChartDataDto Data { get; set; }
        public OptionsDto Options { get; set; }  
        
    }
}
