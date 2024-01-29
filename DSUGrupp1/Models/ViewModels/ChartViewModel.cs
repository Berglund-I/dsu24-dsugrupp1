using DSUGrupp1.Models.DTO;
using Newtonsoft.Json; 
namespace DSUGrupp1.Models.ViewModels
{
    public class ChartViewModel
    {
        public ChartViewModel() 
        {
            Chart = new Chart();    
            JsonChart = JsonConvert.SerializeObject(Chart).ToLower(); 
        }
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }

        //public Chart CreateChart(string type, List<string> labels, string DatasetLabel, List<int> data, List<string> bgcolor, int bWidth = 5,)
        //{
        //    Chart template = new Chart
        //    {
        //        Type = type,
        //        Data = new ChartDataDto
        //        {
        //            Labels = labels,
        //            Datasets = new List<DatasetsDto>
        //            {
        //                new DatasetsDto
        //                {
        //                    Label =  DatasetLabel,
        //                    Data = data,
        //                    BackgroundColor = bgcolor,
        //                    BorderWidth = 0,

        //                },
        //            },
        //        },

        //    };

        //    return null;
        //}

    }
}
