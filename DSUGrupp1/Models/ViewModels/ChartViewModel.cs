﻿using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
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

        public Chart CreateChart(string text, string type, List<string> labels, string DatasetLabel, List<double> data, List<string> bgcolor, int bWidth = 5)
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
                            Fill = false 
                           
                        },
                    },
                },
                Options = new OptionsDto
                {
                    Responsive = true,
                    MaintainAspectRatio = true,
                   
                    Plugins = new PluginsDto
                    {

                        Title = new TitleDto
                        {
                            Display = true,
                            Text = text,
                        }
                    }
                }
            };

            return template;
        }
        public Chart CreateMultiSetChart(string text, string type, List<string> labels, List<DatasetsDto> datasets)
        {
            Chart template = new Chart
            {
                Type = type,
                Data = new ChartDataDto
                {
                    Labels = labels,
                    Datasets = datasets,
                },
                Options = new OptionsDto
                {
                    Responsive = true,
                    MaintainAspectRatio = true,

                    Plugins = new PluginsDto
                    {

                        Title = new TitleDto
                        {
                            Display = true,
                            Text = text,
                        }
                    }
                }
            };

            return template;
        }
        public DatasetsDto GenerateDataSet(List<string> labels, string DatasetLabel, List<double> data, List<string> bgcolor, int bWidth = 5)
        {
            DatasetsDto dataSet = new DatasetsDto
            {
                Label = DatasetLabel,
                Data = data,
                BackgroundColor = bgcolor,
                BorderWidth = bWidth,
                Fill = false,
            };
            return dataSet;
        }

        public Chart CreateAgeChart(string type, List<string> labels, Dictionary<string, AgeGroupDoseCounts> data, List<string> doseColors, int bWidth = 5)
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
                            Label = "Dos 1",
                            Data = new List<double>
                            {
                                data["16-30"].FirstDoseCount,
                                data["31-45"].FirstDoseCount,
                                data["46-60"].FirstDoseCount,
                                data["61+"].FirstDoseCount
                            },
                            BackgroundColor = new List<string> { doseColors[0] },
                            BorderWidth = bWidth
                        },
                        new DatasetsDto
                        {
                            Label = "Dos 2",
                            Data = new List<double>
                            {
                                data["16-30"].SecondDoseCount,
                                data["31-45"].SecondDoseCount,
                                data["46-60"].SecondDoseCount,
                                data["61+"].SecondDoseCount
                            },
                            BackgroundColor = new List<string> { doseColors[1] },
                            BorderWidth = bWidth
                        },
                        new DatasetsDto
                        {
                            Label = "Påfyllnadsdos",
                            Data = new List<double>
                            {
                                data["16-30"].BoosterDoseCount,
                                data["31-45"].BoosterDoseCount,
                                data["46-60"].BoosterDoseCount,
                                data["61+"].BoosterDoseCount
                            },
                            BackgroundColor = new List<string> { doseColors[2] },
                            BorderWidth = bWidth
                        }
                    }
                },
                Options = new OptionsDto
                {
                    Responsive = true,
                    MaintainAspectRatio = false,
                    Plugins = new PluginsDto
                    {
                        Title = new TitleDto
                        {
                            Display = true,
                            Text = "Åldersgrupp"
                        }
                    }
                }
            };

            return template;
        }


    }


}

