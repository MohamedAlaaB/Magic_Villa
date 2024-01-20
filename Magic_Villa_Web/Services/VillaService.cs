using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using static Magic_Villa_Utility.SD;
using System;
using Newtonsoft.Json.Linq;

namespace Magic_Villa_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private IHttpClientFactory _httpClientFactory;
        private string VUrl;
        public VillaService(IHttpClientFactory httpClientFactory,IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            VUrl = configuration.GetValue<string>("ServiceUrl:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaCreateDTO villaDTO, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = villaDTO,
                Url = VUrl+ "api/v1/VillaApi/",
                Token = token
            }); 
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = VUrl + "api/v1/VillaApi/" + id,
                Token = token


            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest() 
            {
                ApiType = SD.ApiType.GET,
                Url=  VUrl + "api/v1/VillaApi/",
                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest() 
            {
                Url = VUrl + "api/v1/VillaApi/" + id ,
                ApiType = SD.ApiType.GET,
                Token = token
            });

        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO villaDTO, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                Url = VUrl + "api/v1/VillaApi/" + villaDTO.Id,
                ApiType = SD.ApiType.PUT,
                Data=villaDTO,
                Token = token
            });
        }
    }
}
