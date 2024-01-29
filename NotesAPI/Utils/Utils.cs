using System.Text.RegularExpressions;

namespace NotesAPI
{
    public static class Utils
    {
        #region ContainsEmailAddress()
        public static bool ContainsEmailAddress(string s)
        {
            var regex = new Regex("[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+");
            return regex.IsMatch(s);
        }
        #endregion

        #region ContainsPhoneNumber()
        public static bool ContainsPhoneNumber(string s)
        {
            var regex = new Regex("[\\+]?[(]?[0-9]{2,3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{3,6}");
            return regex.IsMatch(s);
        }
        #endregion

        #region UnixTimeStampToDateTime()
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        } 
        #endregion
    }
}
