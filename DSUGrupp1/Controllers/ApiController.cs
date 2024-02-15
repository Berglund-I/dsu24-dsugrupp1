using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using DSUGrupp1.Models.DTO;
using DSUGrupp1.Infastructure;
using System.Threading;


namespace DSUGrupp1.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {

        /// <summary>
        /// Gets the total population of a regional code
        /// </summary>
        /// <param name="desoCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PopulationDto> GetPopulationCount(string desoCode, string year)
        {
            string requestUrl = "https://api.scb.se/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

            var apiQuery = new ApiQueryDto
            {
                Query = new List<QueryItem>
                {
                    new QueryItem
                    {
                        Code = "Region",
                        Selection = new Selection { Filter = "vs:RegionKommun07", Values = new List<string> { $"{desoCode}" } }
                    },
                    new QueryItem
                    {
                        Code = "ContentsCode",
                        Selection = new Selection { Filter = "item", Values = new List<string> { "BE0101N1" } }
                    },
                    new QueryItem
                    {
                        Code = "Tid",
                        Selection = new Selection { Filter = "item", Values = new List<string> { $"{year}" } }
                    },
                    new QueryItem
                    {
                        Code = "Kon",
                        Selection = new Selection { Filter = "item", Values = new List<string> { "1", "2" } }
                    },

                },
                Response = new Response { Format = "json" }
            };

            string jsonRequest = JsonConvert.SerializeObject(apiQuery);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            var apiResponse = await ApiEngine.Fetch<PopulationDto>(requestUrl, HttpMethod.Post, content);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }
            else
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

        }
        /// <summary>
        /// Gets data for vaccinations in all Deso's. These are sorted after Deso thereafter after dose.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<VaccineCountDto> GetVaccinationsCount()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/count";

            var apiResponse = await ApiEngine.Fetch<VaccineCountDto>(requestUrl, HttpMethod.Get);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }
            else
            {
                string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Backup", "apiVaccinationsCount.json");

                string jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);
                VaccineCountDto data = JsonConvert.DeserializeObject<VaccineCountDto>(jsonData);
                return data;
            }
        }

        /// <summary>
        /// Gets vaccination data from a specific DeSo
        /// </summary>
        /// <param name="deSoCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<VaccinationDataFromSpecificDeSoDto> GetVaccinationDataFromDeSo(string deSoCode)
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/";

            string jsonRequest = requestUrl + deSoCode;

            var apiResponse = await ApiEngine.Fetch<VaccinationDataFromSpecificDeSoDto>(jsonRequest, HttpMethod.Get);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;

            }
            else
            {
                string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Backup", "StatsPerDeSo", deSoCode + ".json");

                string jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);
                VaccinationDataFromSpecificDeSoDto data = JsonConvert.DeserializeObject<VaccinationDataFromSpecificDeSoDto>(jsonData);
                return data;
            }
        }

        /// <summary>
        /// Gets vaccinationdata from all DeSos 
        /// </summary>
        /// <param name="vaccineCount"></param>
        /// <returns></returns>
        public async Task<List<VaccinationDataFromSpecificDeSoDto>> GetVaccinationDataFromAllDeSos(VaccineCountDto vaccineCount)
        {
            var tasks = vaccineCount.Data.Select(vaccineData =>
                GetVaccinationDataFromDeSo(vaccineData.Deso)).ToList();

            var responses = await Task.WhenAll(tasks);

            
            var allVaccinationData = responses.ToList();

            return allVaccinationData;
        }


        [HttpPost]
        public async Task<PopulationDto> GetPopulationInSpecificDeSo(List<string> desoCode, string year, string gender)
        {
            string requestUrl = "https://api.scb.se/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101Y/FolkmDesoAldKonN";

            var apiQuery = new ApiQueryDto
            {
                Query = new List<QueryItem>
                {
                    new QueryItem
                    {
                        Code = "Region",
                        Selection = new Selection { Filter = "vs:DeSoHE", Values = desoCode }
                    },
                    new QueryItem
                    {
                        Code = "Alder",
                        Selection = new Selection { Filter = "item", Values = new List<string> { "totalt"} }
                    },
                    new QueryItem
                    {
                        Code = "Kon",
                        Selection = new Selection { Filter = "item", Values = new List<string> { $"{gender}" } }
                    },
                     new QueryItem
                    {
                        Code = "Tid",
                        Selection = new Selection { Filter = "item", Values = new List<string> { $"{year}" } }
                    },

                },
                Response = new Response { Format = "json" }
            };

            string jsonRequest = JsonConvert.SerializeObject(apiQuery);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            var apiResponse = await ApiEngine.Fetch<PopulationDto>(requestUrl, HttpMethod.Post, content);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }
            else
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

        }

        /// <summary>
        /// Gets DeSo names and DeSo codes
        /// </summary>
        /// <returns></returns>
        public async Task<DesoInfoDTO> GetDeSoNames()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/deso";

            var apiResponse = await ApiEngine.Fetch<DesoInfoDTO>(requestUrl, HttpMethod.Get);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }
            else
            {
                string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Backup", "apiDesoName.json");

                string jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);
                DesoInfoDTO data = JsonConvert.DeserializeObject<DesoInfoDTO>(jsonData);
                return data;
            }
        }

        /// <summary>
        /// Gets a the dose types aswell as total uses of it
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DoseTypeDto> GetDoseTypes()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/batches";

            var apiResponse = await ApiEngine.Fetch<DoseTypeDto>(requestUrl, HttpMethod.Get);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }
            else
            {
                string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Backup", "apiBatches.json");

                string jsonData = await System.IO.File.ReadAllTextAsync(jsonPath);
                DoseTypeDto data = JsonConvert.DeserializeObject<DoseTypeDto>(jsonData);
                return data;
            }
    }
       
    }

}

