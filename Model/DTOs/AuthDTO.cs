namespace EXE_API_Backend.Models.DTO
{
    public class LoginModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class RegisterModel
    {
        public string fullName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
} 