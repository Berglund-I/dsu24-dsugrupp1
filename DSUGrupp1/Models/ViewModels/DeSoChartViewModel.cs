using Newtonsoft.Json;

namespace DSUGrupp1.Models.ViewModels
{
    public class DeSoChartViewModel
    {
        public string Id { get; set; } = "10";
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }

        public DeSoChartViewModel(string deSoCode)
        {
            
            Chart = new Chart();
            JsonChart = JsonConvert.SerializeObject(Chart).ToLower();
        }
        
    
        

    }
}
