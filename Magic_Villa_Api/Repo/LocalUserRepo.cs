using AutoMapper;
using Magic_Villa_Api.Data;
using Magic_Villa_Api.Modeles;
using Magic_Villa_Api.Modeles.DTOs;
using Magic_Villa_Api.Repo.IRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Magic_Villa_Api.Repo
{
    public class LocalUserRepo : ILocalUserRepo
    {
        private readonly applicationDbContext _applicationDbContext;
        private UserManager<AppUser> _usermanager;
        private RoleManager<IdentityRole> _rolemanager;
        private IMapper _mapper;
        public string _secretKey;
        public LocalUserRepo(applicationDbContext applicationDbContext ,IConfiguration configuration ,UserManager<AppUser> usermanager ,IMapper mapper, RoleManager<IdentityRole> rolemanager)
        {
               _applicationDbContext = applicationDbContext;
            _usermanager = usermanager;
            _mapper = mapper;
            _rolemanager = rolemanager;
            _secretKey = configuration.GetValue<string>("AppSettings:Secret");
        }
       
       
        public bool IsUnique(string username)
        {
            var user = _applicationDbContext.appUsers.Where(x=>x.UserName == username).FirstOrDefault();
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async  Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _applicationDbContext.appUsers.FirstOrDefault(x=>x.UserName ==  loginRequestDto.UserName );
            var isvalid =await _usermanager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || isvalid == false) {
                return  new LoginResponseDto { LocalUser = null, Token = null }; ;
            }
            var roles = await _usermanager.GetRolesAsync(user);
            //1 - declare token handler
            var tokenhandler = new JwtSecurityTokenHandler();

            //2 - turn secret key into bytes array
            var key = Encoding.ASCII.GetBytes(s: _secretKey);
            //3- declare tokken discripter and add token properties
            var discripter = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                   
                    new Claim(ClaimTypes.Name,user.Name.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            //4- create tokken using discriper
            var token =tokenhandler.CreateJwtSecurityToken(discripter);
            //5- assing tokken to login response
            var loginresponse = new LoginResponseDto
            {
                LocalUser = _mapper.Map<UserDto>(user) ,
                Role = roles.FirstOrDefault(),
                Token = tokenhandler.WriteToken(token) ,
            };
            return loginresponse;
        }

        public async Task<UserDto> Register(RegisterationRequest registerationRequestDto)
        {
          
            AppUser user = new ()
            {
                Name = registerationRequestDto.Name,
                UserName = registerationRequestDto.UserName,
               Email = registerationRequestDto.UserName,
               NormalizedEmail = registerationRequestDto.UserName.ToUpper(),
            };
            try
            {
                var result = await _usermanager.CreateAsync(user, registerationRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_rolemanager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                    {
                        await _rolemanager.CreateAsync(new IdentityRole("Admin"));
                        await _rolemanager.CreateAsync(new IdentityRole("Customer"));
                    }
                    await _usermanager.AddToRoleAsync(user, "Admin");
                    var usertoreturn =  _applicationDbContext.appUsers.FirstOrDefault(x=> x.UserName == registerationRequestDto.UserName);
                    return _mapper.Map<UserDto>(usertoreturn);
                }
            }
            catch (Exception)
            {

                throw;
            }
            _applicationDbContext.appUsers.Add(user);
            await _applicationDbContext.SaveChangesAsync();
            
            return new UserDto(); 
        }
    }
}
