using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotesAPI.Controllers.Model.Authorization
{
    public class RegisterRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterResponseModel
    {
        public string Token { get; set; }
    }

    public enum RegisterErrorResponseCode
    {
        [Description("User with provided email already exists")]
        UserExists = 1,
    }
}
