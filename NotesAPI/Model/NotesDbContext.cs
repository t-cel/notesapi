using Microsoft.EntityFrameworkCore;

namespace NotesAPI.Model
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword("test");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "user1@test.xyz", PasswordHash = passwordHash, DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc) },
                new User { Id = 2, Email = "user2@test.xyz", PasswordHash = passwordHash, DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc) },
                new User { Id = 3, Email = "user3@test.xyz", PasswordHash = passwordHash, DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc) }
            );

            modelBuilder.Entity<Note>().HasData(
                new Note { Id = 1, Content = "test email@test.xyz 123", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "EMAIL", UserId = 1 },
                new Note { Id = 2, Content = "test +48123123123 abc", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "PHONE", UserId = 1 },
                new Note { Id = 3, Content = "test 123", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "NONE", UserId = 1 },
                new Note { Id = 4, Content = "test 2 512312522 abc", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "PHONE", UserId = 1 },
                new Note { Id = 5, Content = "test 123", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "NONE", UserId = 2 },
                new Note { Id = 6, Content = "test 123", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "NONE", UserId = 2 },
                new Note { Id = 7, Content = "test 2 512312522 abc", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "PHONE", UserId = 3 },
                new Note { Id = 8, Content = "test 2 512312522 abc", DateCreatedUtc = new DateTime(2024, 1, 29, 12, 12, 12, DateTimeKind.Utc), Tag = "PHONE", UserId = 3 }
            );
        }
    }
}
