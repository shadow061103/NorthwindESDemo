namespace NorthwindDemo.Common.Attribute
{
    public sealed class EnumDescriptionAttribute : System.Attribute
    {
        public string Description { get; }

        public EnumDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}