namespace NotesAPI.Model
{
    public class Note
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
    }
}
