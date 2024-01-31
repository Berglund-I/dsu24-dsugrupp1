using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Chart
    {
        public Chart() 
        {
            type = "bar";
            data = new ChartDataDto();
            options = new OptionsDto();
        }
        public string type { get; set; }
        public ChartDataDto data { get; set; }
        public OptionsDto options { get; set; }  
        
    }
}
