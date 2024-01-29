using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotesAPI.Controllers.Model.Auth
{
    public class RefreshTokenRequestModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public enum RefreshTokenErrorCode
    {
        [Description("Token is not expired")]
        TokenIsNotExpired = 1,

        [Description("Refresh token doesn't exist")]
        RefreshTokenDoesntExist = 2,

        [Description("Refresh token has expired")]
        RefreshTokenHasExpired = 3,

        [Description("Refresh token has been already used")]
        RefreshTokenHasBeenUsed = 4,

        [Description("Refresh token has been revoked")]
        RefreshTokenHasBeenRevoked = 5,

        [Description("Refresh token is invalid")]
        InvalidRefreshToken = 6,

        [Description("Token or refresh token is invalid")]
        TokenProcessingError = 7,
    }
}
