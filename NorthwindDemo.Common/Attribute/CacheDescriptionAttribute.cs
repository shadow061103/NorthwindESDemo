namespace NorthwindDemo.Common.Attribute
{
    public sealed class CacheDescriptionAttribute : System.Attribute
    {
        public string Description { get; }

        public CacheDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}