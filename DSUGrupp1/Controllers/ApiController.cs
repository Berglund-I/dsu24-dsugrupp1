using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DSUGrupp1.Models.API;
using System.Text;
using DSUGrupp1.Models.DTO;


namespace DSUGrupp1.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly HttpClient? _httpClient;

        public ApiController()
        {
            _httpClient = new HttpClient();   
        }

        [HttpPost]
        public async Task<IActionResult> ScbApiCall(string desoCode,string year)
        {
            var apiQuery = new ApiQuery
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

            try
            {
                var response = await _httpClient.PostAsync("https://api.scb.se/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<ResponseObject>(responseContent);
                    var values = responseObject.Data[0].Values[0];

                    return Ok(values);
                }
				return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

			}
            catch (Exception ex)
            {
               
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> GetVaccinationsCount()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/count";
          
            try
            {
				var response = await _httpClient.GetAsync($"{requestUrl}");
				
                if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();

					var responseObject = JsonConvert.DeserializeObject<SwaggerDTO>(responseContent);

					return Ok(responseObject);
				}
				return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
			}
			catch (Exception ex)
			{

				return StatusCode(500, ex.Message);
			}

        }

        public async Task<VaccinationDataFromSpecifikDeSoDto> GetVaccinationDataFromDeSo(string deSoCode)
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/";
            VaccinationDataFromSpecifikDeSoDto vaccinationData = new VaccinationDataFromSpecifikDeSoDto();  

            string jsonRequest = requestUrl + deSoCode;


            var response = await _httpClient.GetAsync(jsonRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                vaccinationData = JsonConvert.DeserializeObject<VaccinationDataFromSpecifikDeSoDto>(responseContent);    
            }
            return vaccinationData;
        }

    }
}
