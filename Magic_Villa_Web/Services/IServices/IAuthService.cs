using Magic_Villa_Web.DTOs;
using Magic_Villa_Web.Models;

namespace Magic_Villa_Web.Services.IServices
{
    public interface IAuthService :IBaseService
    {
        Task<T> RegisterAsync<T>(RegisterationRequest request);
        Task<T> LoginAsync<T>(LoginRequestDto request);
    }
}
