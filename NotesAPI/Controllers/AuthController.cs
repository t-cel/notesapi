using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NotesAPI.Controllers.Model;
using NotesAPI.Controllers.Model.Auth;
using NotesAPI.Controllers.Model.Authorization;
using NotesAPI.Services;
using System.IdentityModel.Tokens.Jwt;

namespace NotesAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService AuthService;
        private readonly UsersService UsersService;
        private readonly TokenService TokenService;
        private readonly ILogger<AuthController> Logger;

        #region AuthController()
        public AuthController(AuthService authService, UsersService usersService, TokenService tokenService, ILogger<AuthController> logger)
        {
            AuthService = authService;
            UsersService = usersService;
            TokenService = tokenService;
            Logger = logger;
        } 
        #endregion

        #region Register()
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse<RegisterErrorResponseCode>))]
        public async Task<ActionResult> Register([FromBody] RegisterRequestModel model)
        {
            var existingUser = await UsersService.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new ErrorResponse<RegisterErrorResponseCode>(RegisterErrorResponseCode.UserExists));
            }

            await AuthService.Register(model.Email, model.Password);
            return Ok();
        }
        #endregion

        #region Login()
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse<LoginErrorResponseCode>))]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel model)
        {
            var user = await UsersService.GetUserByEmail(model.Email);
            if (user == null)
            {
                return BadRequest(new ErrorResponse<LoginErrorResponseCode>(LoginErrorResponseCode.InvalidCredentials));
            }

            var verifyResult = AuthService.VerifyPassword(user, model.Password);
            if (!verifyResult)
            {
                return BadRequest(new ErrorResponse<LoginErrorResponseCode>(LoginErrorResponseCode.InvalidCredentials));
            }

            var (token, refreshToken) = await TokenService.GenerateToken(user);
            return Ok(new LoginResponseModel
            { 
                Token = token,
                RefreshToken = refreshToken
            });
        }
        #endregion

        #region LogOut()
        [HttpGet("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> LogOut()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString()
                .Replace("Bearer ", string.Empty);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.ReadJwtToken(accessToken);
            var refreshToken = await TokenService.GetRefreshTokenByJwtId(token.Id);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await TokenService.UpdateRefreshToken(refreshToken);
            }

            return Ok();
        } 
        #endregion

        #region RefreshToken()
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse<RefreshTokenErrorCode>))]
        public async Task<ActionResult<RefreshTokenResponseModel>> RefreshToken([FromBody] RefreshTokenRequestModel model)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtTokenHandler.ReadJwtToken(model.Token);
                var utcExpiryDate = long.Parse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expDate = Utils.UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.TokenIsNotExpired));
                }

                var refreshToken = await TokenService.GetRefreshToken(model.RefreshToken);
                if (refreshToken == null)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.RefreshTokenDoesntExist));
                }

                if (DateTime.UtcNow > refreshToken.DateExpireUtc)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.RefreshTokenHasExpired));
                }

                if (refreshToken.IsUsed)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.RefreshTokenHasBeenUsed));
                }

                if (refreshToken.IsRevoked)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.RefreshTokenHasBeenRevoked));
                }

                var jti = token.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (refreshToken.JwtId != jti)
                {
                    return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.InvalidRefreshToken));
                }

                var user = await UsersService.GetUserById(refreshToken.UserId);
                var (newToken, newRefreshToken) = await TokenService.GenerateToken(user);

                refreshToken.IsUsed = true;

                await TokenService.UpdateRefreshToken(refreshToken);
                return Ok(new RefreshTokenResponseModel
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest(new ErrorResponse<RefreshTokenErrorCode>(RefreshTokenErrorCode.TokenProcessingError));
            }
        }
        #endregion
    }
}
