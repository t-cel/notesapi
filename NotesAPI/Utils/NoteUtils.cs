namespace NotesAPI
{
    public static class NoteUtils
    {
        #region GetTagFromContent()
        public static string GetTagFromContent(string content)
        {
            if (Utils.ContainsEmailAddress(content))
            {
                return TagType.Email;
            }

            if (Utils.ContainsPhoneNumber(content))
            {
                return TagType.Phone;
            }

            return TagType.None;
        }
        #endregion
    }
}
