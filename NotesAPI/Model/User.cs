namespace NotesAPI.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateCreatedUtc { get; set; }

    }
}
