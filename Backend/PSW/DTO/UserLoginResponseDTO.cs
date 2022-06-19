using PSW.Model;

namespace PSW.DTO
{
    public class UserLoginResponseDTO
    {
        public User User { get; set; }
        public string Token { get; set; }

        public UserLoginResponseDTO(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}
