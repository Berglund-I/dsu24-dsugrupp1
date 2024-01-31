using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace DSUGrupp1.Models.ViewModels
{
    public class ChartViewModel
    {
        public ChartViewModel() 
        {
            Chart = new Chart();
            JsonChart = SerializeJson(Chart);
        }
        public string Id { get; set; }  
        public Chart Chart { get; set; }
        public string JsonChart { get; set; }
        
        /// <summary>
        /// Takes a Chart object and converts it in to a camelcased Json string for use in JS
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public string SerializeJson(Chart chart)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            var json = JsonConvert.SerializeObject(chart, settings);
            return json;
        }

        public Chart CreateChart(string type, List<string> labels, string DatasetLabel, List<double> data, List<string> bgcolor, int bWidth = 5)
        {
            Chart template = new Chart
            {
                Type = type,
                Data = new ChartDataDto
                {
                    Labels = labels,
                    Datasets = new List<DatasetsDto>
                    {
                        new DatasetsDto
                        {
                            Label = DatasetLabel,
                            Data = data,
                            BackgroundColor = bgcolor,
                            BorderWidth = bWidth,
                        },
                    },
                },
                Options = new OptionsDto
                {

                    Plugins = new PluginsDto
                    {

                        Title = new TitleDto
                        {
                            Display = true,
                            Text = "",
                        }
                    }
                }
            };

            return template;
        }

    }
}
