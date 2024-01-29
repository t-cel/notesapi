using NotesAPI.Controllers.Model.Authorization;
using NotesAPI.Model;

namespace NotesAPI.Services
{
    public class AuthService
    {
        private readonly NotesDbContext DbContext;

        public AuthService(NotesDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Register(string email, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                DateCreatedUtc = DateTime.UtcNow
            };

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();
        }

        public bool VerifyPassword(User user, string password)
        {
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return false;
            }

            return true;
        }
    }
}
