using DSUGrupp1.Models.DTO;

namespace DSUGrupp1.Models
{
    public class Chart
    {
        public string Type { get; set; }
        public ChartDataDto Data { get; set; }
        public OptionsDto Options{ get; set; }  
        
    }
}
