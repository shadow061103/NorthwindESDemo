using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Common.Enum;

namespace NorthwindDemo.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string EnumDescription(this System.Enum value)
        {
            var data = typeof(CacheTypeEnum).GetField(value.ToString());
            var attr = System.Attribute.GetCustomAttribute(data, typeof(CacheDescriptionAttribute));

            return ((CacheDescriptionAttribute)attr).Description;
        }
    }
}