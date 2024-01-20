using Magic_Villa_Web.Modeles;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Net.Http;
using System.Text;

namespace Magic_Villa_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse APIResponse { get; set; }

        //we need to use httpclient fastory using dependancy injectiion
        //and initialize new api response 
        public IHttpClientFactory httpClientFactory { get; set; }
        public BaseService(IHttpClientFactory httpClientFactory)
        {
                this.APIResponse = new();
                this.httpClientFactory = httpClientFactory;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                ///when sending request we need to creat new client and HttpRequestMessage
                ///then set 3 things inside HttpRequestMessage
                ///header,
                ///uri(request.url),
                ///method
                ///and content 

                var client =  httpClientFactory.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");   
                
                message.RequestUri = new Uri(apiRequest.Url);

                switch (apiRequest.ApiType)
                {                
                    case Magic_Villa_Utility.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Magic_Villa_Utility.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Magic_Villa_Utility.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

                }
                // now we need to get the response from api 
                //we declare new HttpResponseMessage then we save the
                //the return of send async inside
                HttpResponseMessage apiresponse = null;
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                apiresponse= await client.SendAsync(message);
                var apiContent = await apiresponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse APIResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiresponse.StatusCode == System.Net.HttpStatusCode.NotFound || apiresponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        APIResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        APIResponse.IsSuccess = false;
                       
                    }
                    var res = JsonConvert.SerializeObject(APIResponse);
                    var returnobj = JsonConvert.DeserializeObject<T>(res);
                    return returnobj;
                }
                catch (Exception e)
                {
                    var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return APIResponse;
                
                }

               

            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    Errors = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
