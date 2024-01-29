using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotesAPI.Controllers.Model.Authorization
{
    public class LoginRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public enum LoginErrorResponseCode
    {
        [Description("Invalid user credentials")]
        InvalidCredentials = 1
    }
}
