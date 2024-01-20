using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Modeles.DTOs;

namespace Magic_Villa_Api.Repo.IRepo
{
    public interface ILocalUserRepo

    {
        bool IsUnique(string username);
         Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
         Task<UserDto> Register(RegisterationRequest registerationRequestDto);
    }
}
