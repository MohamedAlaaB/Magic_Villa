using Magic_Villa_Web.DTOs;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAsync<T>(int id, string token);
        Task<T> GetAllAsync<T>(string token);
        Task<T> CreateAsync<T>(VillaCreateDTO villaDTO, string token);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villaDTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
