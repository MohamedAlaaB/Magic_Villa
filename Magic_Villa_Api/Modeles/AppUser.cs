using Microsoft.AspNetCore.Identity;

namespace Magic_Villa_Api.Modeles
{
    public class AppUser :IdentityUser
    {
        public string Name { get; set; }
    }
}
