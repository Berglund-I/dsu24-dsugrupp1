using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using DSUGrupp1.Models.DTO;
using System.Net;
using DSUGrupp1.Infastructure;


namespace DSUGrupp1.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {

        /// <summary>
        /// Gets the total population of a specified Deso for a specified year.
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

                },
                Response = new Response { Format = "json" }
            };

            string jsonRequest = JsonConvert.SerializeObject(apiQuery);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

            var apiResponse = await ApiEngine.Fetch<PopulationDto>(requestUrl, HttpMethod.Post, content);
            
            if(apiResponse.IsSuccessful) 
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
        public async Task<IActionResult> GetVaccinationsCount()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/count";

			var apiResponse = await ApiEngine.Fetch<VaccineCountDto>(requestUrl, HttpMethod.Get);

			if (apiResponse.IsSuccessful)
			{
				return Ok(apiResponse.Data);
			}
			else
			{
				return StatusCode((int)apiResponse.StatusCode);
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
                throw new Exception(apiResponse.ErrorMessage);
            }
        }

    }
}
