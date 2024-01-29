using Microsoft.EntityFrameworkCore;
using NotesAPI.Model;

namespace NotesAPI.Services
{
    public class UsersService
    {
        private readonly NotesDbContext DbContext;

        public UsersService(NotesDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region GetUserByEmail()
        public async Task<User> GetUserByEmail(string email)
        {
            return await DbContext.Users
                .FirstOrDefaultAsync(p => p.Email == email);
        }
        #endregion

        #region GetUserById()
        public async Task<User> GetUserById(long id)
        {
            return await DbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion
    }
}
