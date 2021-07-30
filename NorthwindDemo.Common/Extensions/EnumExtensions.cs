using NorthwindDemo.Common.Attribute;
using NorthwindDemo.Common.Caching;

namespace NorthwindDemo.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string EnumDescription(this System.Enum value)
        {
            var data = typeof(CacheTypeEnum).GetField(value.ToString());
            var attr = System.Attribute.GetCustomAttribute(data, typeof(EnumDescriptionAttribute));

            return ((EnumDescriptionAttribute)attr).Description;
        }
    }
}