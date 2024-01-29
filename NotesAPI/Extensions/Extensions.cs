using System.ComponentModel;

namespace NotesAPI.Extensions
{
    public static class Extensions
    {
        public static string GetDescription(this Enum item)
        {
            System.Reflection.FieldInfo fieldInfo =
                item.GetType().GetField(item.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length > 0)
            {
                var attrib = attribArray[0] as DescriptionAttribute;

                if (attrib != null)
                    return attrib.Description;
            }
            return item.ToString();
        }
    }
}
