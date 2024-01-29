namespace NotesAPI.Model
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime DateExpireUtc { get; set; }
    }
}
