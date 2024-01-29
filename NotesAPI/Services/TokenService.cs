using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NotesAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotesAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<TokenService> Logger;
        private readonly NotesDbContext NotesDbContext;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger, NotesDbContext notesDbContext)
        {
            Configuration = configuration;
            Logger = logger;
            NotesDbContext = notesDbContext;
        }

        #region GenerateToken()
        public async Task<(string token, string refreshToken)> GenerateToken(User user)
        {
            var keyEncoded = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"].ToString());
            var audience = Configuration["Jwt:Audience"].ToString();
            var issuer = Configuration["Jwt:Issuer"].ToString();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                //Expires = DateTime.UtcNow.AddYears(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyEncoded),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                DateCreatedUtc = DateTime.UtcNow,
                DateExpireUtc = DateTime.UtcNow.AddDays(7)
            };
            await NotesDbContext.RefreshTokens.AddAsync(refreshToken);
            await NotesDbContext.SaveChangesAsync();

            return (
                token: tokenString,
                refreshToken: refreshToken.Token
            );
        } 
        #endregion

        #region GetRefreshToken()
        public async Task<RefreshToken> GetRefreshToken(string refreshToken)
        {
            var token = await NotesDbContext.RefreshTokens
                .FirstOrDefaultAsync(p => p.Token == refreshToken);

            return token;
        }
        #endregion

        #region GetRefreshTokenByJwtId()
        public async Task<RefreshToken> GetRefreshTokenByJwtId(string jwtId)
        {
            var token = await NotesDbContext.RefreshTokens
                .FirstOrDefaultAsync(p => p.JwtId == jwtId);

            return token;
        }
        #endregion

        #region UpdateRefreshToken()
        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            NotesDbContext.Update(refreshToken);
            await NotesDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
