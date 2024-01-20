namespace Magic_Villa_Api.Modeles.DTOs
{
    public class LoginResponseDto
    {
        public UserDto LocalUser { get; set; }

        public string Role { get; set; }
        public string Token { get; set; }
    }
}
