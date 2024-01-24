using DSUGrupp1.Models.DTO;
using Newtonsoft.Json;
using System.Net;

namespace DSUGrupp1.Infastructure
{
    public static class ApiEngine
    {
        public static async Task<ApiResponse<T>> Fetch<T>(string apiUrl, HttpMethod method, HttpContent content = null)
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

                if (method == HttpMethod.Post)
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
    }
}
