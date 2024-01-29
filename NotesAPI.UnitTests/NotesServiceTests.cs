namespace NotesAPI.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetTagFromContent_ReturnsPhoneTag()
        {
            var content = "Notatka testowa (555) 555-1234 test";
            var tag = NoteUtils.GetTagFromContent(content);

            Assert.That(tag, Is.EqualTo(TagType.Phone));
        }

        [Test]
        public void GetTagFromContent_ReturnsEmailTag()
        {
            var content = "Notatka testowa test@test.com test";
            var tag = NoteUtils.GetTagFromContent(content);

            Assert.That(tag, Is.EqualTo(TagType.Email));
        }

        [Test]
        public void GetTagFromContent_ReturnsEmptyTag()
        {
            var content = "Notatka testowa test";
            var tag = NoteUtils.GetTagFromContent(content);

            Assert.That(tag, Is.EqualTo(TagType.None));
        }
    }
}