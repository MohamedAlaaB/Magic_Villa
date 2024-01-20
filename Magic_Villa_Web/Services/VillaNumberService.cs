using Magic_Villa_Utility;
using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Models;
using Magic_Villa_Web.Services.IServices;
using static Magic_Villa_Utility.SD;
using System;
using Magic_Villa_Web.Modeles.DTOs;

namespace Magic_Villa_Web.Services
{
    public class VillaNumberService : BaseService ,IVillaNumberService
    {
        private IHttpClientFactory _httpClientFactory;
        private string VUrl;
        public VillaNumberService(IHttpClientFactory httpClientFactory,IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            VUrl = configuration.GetValue<string>("ServiceUrl:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO villaDTO, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = villaDTO,
                Url = VUrl+ "api/v1/VillaNumber/" ,
                Token = token
            }); 
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = VUrl + "api/v1/VillaNumber/" + id,

                Token = token

            });
        }

        public Task<T> GetAllAsync<T>( string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest() 
            {
                ApiType = SD.ApiType.GET,
                Url=  VUrl + "api/v1/VillaNumber/",

                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest() 
            {
                Url = VUrl + "api/v1/VillaNumber/" + id ,
                ApiType = SD.ApiType.GET,
                Token = token
            });

        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villaDTO, string token)
        {
            return SendAsync<T>(apiRequest: new APIRequest()
            {
                Url = VUrl + "api/v1/VillaNumber/" + villaDTO.VillaNo,
                ApiType = SD.ApiType.PUT,
                Data=villaDTO,
                Token = token
            });
        }
    }
}
