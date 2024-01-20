using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using Microsoft.Extensions.Configuration;

namespace Magic_Villa_Web.Services
{
    public class AuthServicecs : BaseService, IAuthService
    {
        private IHttpClientFactory _httpClientFactory;
        private string VUrl;
        public AuthServicecs(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            VUrl = configuration.GetValue<string>("ServiceUrl:VillaAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDto request)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = VUrl + "api/v1/UsersApi/Login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequest request)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = VUrl + "api/v1/UsersApi/Register"
            });
        }
    }
}
