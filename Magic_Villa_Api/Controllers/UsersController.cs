using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Modeles.DTOs;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.AspNetCore.Mvc;

namespace Magic_Villa_Api.Controllers
{

    [Route("api/v{version:apiversion}/UsersApi")]
    //[Route("api/UsersApi")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : Controller
    {
        private ILocalUserRepo _localUserRepo;
        private APIResponse _apiResponse;
        public UsersController(ILocalUserRepo localUserRepo)
        {
            this._apiResponse = new();
            _localUserRepo = localUserRepo;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginresponse = await _localUserRepo.Login(loginRequestDto);
            if (loginresponse.LocalUser == null || loginresponse.Token == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add("invalid password or username");
                return BadRequest(_apiResponse);
            }
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.Result = loginresponse;
            return Ok(_apiResponse);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest registerationRequest)
        {
            bool isunique =  _localUserRepo.IsUnique(registerationRequest.UserName);
            if (!isunique)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add("Please submit Unique UserName");
                return BadRequest(_apiResponse);
            }
            var registerres = await _localUserRepo.Register(registerationRequest);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            _apiResponse.Result = registerres;
            return Ok(_apiResponse);
        }
    }
}
