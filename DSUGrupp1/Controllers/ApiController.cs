using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DSUGrupp1.Models.API;
using System.Text;
using DSUGrupp1.Models.DTO;
using System.Net;


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
        public async Task<ApiResponse<T>> Fetch<T>(string apiUrl, HttpMethod method,HttpContent content = null)
        {
			using HttpClient client = new HttpClient();
			ApiResponse<T> apiResponse = new ApiResponse<T>();

			try
			{
				if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out _))
				{
					apiResponse.ErrorMessage = "Invalid URL";
					HttpResponseMessage invalidUrlResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
					{
						ReasonPhrase = "Invalid URL"
					};

					apiResponse.Response = invalidUrlResponse;
					return apiResponse;
				}

                HttpResponseMessage response;

                if(method == HttpMethod.Post)
                {
                    response = await client.PostAsync(apiUrl, content);
                }
                else
                {
                    response = await client.GetAsync(apiUrl);
                }
				
				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();
					apiResponse.Data = JsonConvert.DeserializeObject<T>(responseData);
				}
				else
				{
					apiResponse.StatusCode = response.StatusCode;
				}
			}
			catch (Exception)
			{

				apiResponse.ErrorMessage = "Error";
			}
			return apiResponse;
        }

        [HttpPost]
        public async Task<IActionResult> ScbApiCall(string desoCode, string year)
        {
            string requestUrl = "https://api.scb.se/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

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

            var apiResponse = await Fetch<ResponseObject>(requestUrl, HttpMethod.Post, content);
            
            if(apiResponse.IsSuccessful) 
            {
                return Ok(apiResponse.Data);
            }
            else
            {
				return StatusCode((int)apiResponse.StatusCode);
			}

		}

        public async Task<IActionResult> GetVaccinationsCount()
        {
            string requestUrl = "https://grupp1.dsvkurs.miun.se/api/vaccinations/count";

			var apiResponse = await Fetch<SwaggerDTO>(requestUrl, HttpMethod.Get);

			if (apiResponse.IsSuccessful)
			{
				return Ok(apiResponse.Data);
			}
			else
			{
				return StatusCode((int)apiResponse.StatusCode);
			}
		}

    }
}
