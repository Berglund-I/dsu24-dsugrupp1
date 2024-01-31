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
        public string Id { get; set; }  
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }


        public Chart CreateChart(string type, List<string> labels, string DatasetLabel, List<double> data, List<string> bgcolor, int bWidth = 5)
        {
            Chart template = new Chart
            {
                type = type,
                data = new ChartDataDto
                {
                    labels = labels,
                    datasets = new List<DatasetsDto>
                    {
                        new DatasetsDto
                        {
                            label = DatasetLabel,
                            data = data,
                            backgroundColor = bgcolor,
                            borderWidth = bWidth,
                        },
                    },
                },
                options = new OptionsDto
                {

                    plugins = new PluginsDto
                    {

                        title = new TitleDto
                        {
                            display = true,
                            text = "",
                        }
                    }
                }
            };

            return template;
        }

    }
}
