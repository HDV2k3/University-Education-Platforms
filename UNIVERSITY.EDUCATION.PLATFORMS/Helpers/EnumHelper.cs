using System.ComponentModel;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if(fieldInfo == null)
            {
                return enumValue.ToString();
            }
            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
        public static string GetDescriptionFromKey<T>(string key) where T : Enum
        {
            try
            {
                var type = typeof(T);
                var name = Enum.GetNames(type).FirstOrDefault(e => e == key);
                if(name == null)
                {
                    return key;
                }
                var field = type.GetField(name);
                if(field == null)
                {
                    return key;
                }
                var fields = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute item in fields)
                {
                    var data = new DataEnum()
                    {
                        Name = name,
                        Description = item.Description
                    };
                    return item?.Description ?? string.Empty;
                }
                return key;
            }
            catch
            {
                return key;
            }
        }
    }
}
