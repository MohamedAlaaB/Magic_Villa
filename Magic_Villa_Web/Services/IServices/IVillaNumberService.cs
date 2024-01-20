using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Modeles.DTOs;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAsync<T>(int id ,string token);
        Task<T> GetAllAsync<T>( string token);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO villaDTO, string token);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villaDTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
